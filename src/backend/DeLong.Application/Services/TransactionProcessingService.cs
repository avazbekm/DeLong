using AutoMapper;
using DeLong.Domain.Enums;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Service.DTOs.Prices;
using Microsoft.Extensions.Logging;
using DeLong.Application.Interfaces;
using DeLong.Service.DTOs.ReceiveItems;
using DeLong.Service.DTOs.CreditorDebts;
using DeLong.Application.DTOs.Transactions;

namespace DeLong.Service.Services;

public class TransactionProcessingService : AuditableService, ITransactionProcessingService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<TransactionItem> _transactionItemRepository;
    private readonly IPriceServer _priceService;
    private readonly ICreditorDebtService _creditorDebtService;
    private readonly ILogger<TransactionProcessingService> _logger;

    public TransactionProcessingService(
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Transaction> transactionRepository,
        IRepository<TransactionItem> transactionItemRepository,
        IPriceServer priceService,
        ICreditorDebtService creditorDebtService,
        ILogger<TransactionProcessingService> logger)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _transactionRepository = transactionRepository;
        _transactionItemRepository = transactionItemRepository;
        _priceService = priceService;
        _creditorDebtService = creditorDebtService;
        _logger = logger;
    }

    // Eski metod uchun (requestId siz)
    public async Task<TransactionResultDto> ProcessTransactionAsync(List<ReceiveItemDto> receiveItems)
    {
        return await ProcessTransactionAsync(receiveItems, null);
    }

    public async Task<TransactionResultDto> ProcessTransactionAsync(List<ReceiveItemDto> receiveItems, Guid? requestId = null)
    {
        if (receiveItems == null || !receiveItems.Any())
        {
            _logger.LogWarning("Tranzaksiya uchun mahsulotlar ro‘yxati bo‘sh.");
            throw new ArgumentException("Hech qanday mahsulot kiritilmagan.");
        }

        // Yetkazib beruvchi tekshiruvi
        var supplierIds = receiveItems.Select(i => i.SupplierId).Distinct().ToList();
        if (supplierIds.Count > 1)
        {
            _logger.LogWarning("Bir nechta yetkazib beruvchi tanlangan: {SupplierIds}", string.Join(", ", supplierIds));
            throw new ArgumentException("Faqat bitta yetkazib beruvchi tanlanishi kerak.");
        }
        if (supplierIds.First() <= 0)
        {
            _logger.LogWarning("Yetkazib beruvchi ID si noto‘g‘ri: {SupplierId}", supplierIds.First());
            throw new ArgumentException("Yetkazib beruvchi tanlanmagan.");
        }

        // Idempotentlik tekshiruvi
        if (requestId.HasValue)
        {
            var existingTransaction = await _transactionRepository.GetAsync(t =>
                t.RequestId == requestId.Value && !t.IsDeleted);
            if (existingTransaction != null)
            {
                _logger.LogInformation("Tranzaksiya allaqachon mavjud: RequestId={RequestId}, TransactionId={TransactionId}", requestId, existingTransaction.Id);
                return _mapper.Map<TransactionResultDto>(existingTransaction);
            }
        }

        Transaction transactionEntity = null; // try dan tashqarida e’lon qilindi
        using var transaction = await _transactionRepository.BeginTransactionAsync();
        try
        {
            // 1. Transaction yaratish
            transactionEntity = new Transaction
            {
                SupplierIdFrom = receiveItems.First().SupplierId,
                BranchId = GetCurrentBranchId(),
                BranchIdTo = GetCurrentBranchId(),
                TransactionType = TransactionType.Kirim,
                Comment = "Yetkazib beruvchidan mahsulot keldi.",
                RequestId = requestId
            };
            SetCreatedFields(transactionEntity);
            await _transactionRepository.CreateAsync(transactionEntity);
            await _transactionRepository.SaveChanges();

            // 2. Price larni qayta ishlash va TransactionItem lar yaratish
            var transactionItems = new List<TransactionItem>();
            decimal totalAmount = 0;
            foreach (var item in receiveItems)
            {
                // TotalAmount ni tekshirish
                var expectedTotal = item.CostPrice * item.Quantity;
                if (Math.Abs(item.TotalAmount - expectedTotal) > 0.01m)
                {
                    _logger.LogWarning("TotalAmount noto‘g‘ri: Product={ProductName}, TotalAmount={TotalAmount}, Expected={Expected}",
                        item.ProductName, item.TotalAmount, expectedTotal);
                    throw new ArgumentException($"TotalAmount noto‘g‘ri: {item.ProductName} uchun {item.TotalAmount} != {item.CostPrice} * {item.Quantity}");
                }
                totalAmount += item.TotalAmount;

                long priceId;
                if (item.IsUpdate)
                {
                    if (item.PriceId <= 0)
                    {
                        _logger.LogWarning("Narx ID si noto‘g‘ri: PriceId={PriceId}, Product={ProductName}", item.PriceId, item.ProductName);
                        throw new ArgumentException($"Narx ID si noto‘g‘ri: {item.PriceId}");
                    }
                    var priceQuantity = await _priceService.RetrieveByIdAsync(item.PriceId);
                    var updateDto = new PriceUpdateDto
                    {
                        Id = item.PriceId,
                        Quantity = item.Quantity + priceQuantity.Quantity,
                        ProductId = item.ProductId,
                        CostPrice = item.CostPrice,
                        SellingPrice = item.SellingPrice,
                        SupplierId = item.SupplierId,
                        UnitOfMeasure = item.UnitOfMeasure
                    };
                    var updateResult = await _priceService.ModifyAsync(updateDto);
                    if (updateResult is null)
                    {
                        _logger.LogError("Narx yangilashda xato: ProductId={ProductId}", item.ProductId);
                        throw new Exception($"Narx yangilashda xato: ProductId={item.ProductId}");
                    }
                    priceId = item.PriceId;
                }
                else
                {
                    var creationDto = new PriceCreationDto
                    {
                        ProductId = item.ProductId,
                        CostPrice = item.CostPrice,
                        SellingPrice = item.SellingPrice,
                        Quantity = item.Quantity,
                        UnitOfMeasure = item.UnitOfMeasure,
                        SupplierId = item.SupplierId
                    };
                    var priceResult = await _priceService.AddAsync(creationDto);
                    if (priceResult == null)
                    {
                        _logger.LogError("Narx qo‘shishda xato: ProductId={ProductId}", item.ProductId);
                        throw new Exception($"Narx qo‘shishda xato: ProductId={item.ProductId}");
                    }
                    priceId = priceResult.Id;
                }

                var transactionItem = new TransactionItem
                {
                    TransactionId = transactionEntity.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitOfMeasure = item.UnitOfMeasure,
                    PriceProduct = item.CostPrice,
                    BranchId = GetCurrentBranchId()
                };
                SetCreatedFields(transactionItem);
                transactionItems.Add(transactionItem);
            }

            // 3. TransactionItem larni saqlash
            foreach (var item in transactionItems)
            {
                await _transactionItemRepository.CreateAsync(item);
            }

            // 4. CreditorDebt ga yozish
            var creditorDebt = new CreditorDebtCreationDto
            {
                SupplierId = receiveItems.First().SupplierId,
                Date = DateTimeOffset.UtcNow,
                RemainingAmount = totalAmount,
                Description = $"Tranzaksiya #{transactionEntity.Id} uchun qarz",
                BranchId = GetCurrentBranchId()
            };
            await _creditorDebtService.AddAsync(creditorDebt);

            // 5. Tranzaksiyani yakunlash
            await _transactionRepository.SaveChanges();
            await transaction.CommitAsync();

            _logger.LogInformation("Tranzaksiya muvaffaqiyatli yakunlandi: TransactionId={TransactionId}, TotalAmount={TotalAmount}, SupplierId={SupplierId}",
                transactionEntity.Id, totalAmount, transactionEntity.SupplierIdFrom);

            return _mapper.Map<TransactionResultDto>(transactionEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tranzaksiyada xato: TransactionId={TransactionId}", transactionEntity?.Id ?? 0);
            await transaction.RollbackAsync();
            throw new Exception($"Tranzaksiyada xato: {ex.Message}", ex);
        }
    }
}