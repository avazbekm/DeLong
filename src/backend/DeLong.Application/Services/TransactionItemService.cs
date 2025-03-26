using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.TransactionItems;

namespace DeLong.Service.Services;

public class TransactionItemService : AuditableService, ITransactionItemService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TransactionItem> _transactionItemRepository;

    public TransactionItemService(IRepository<TransactionItem> transactionItemRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _transactionItemRepository = transactionItemRepository;
    }

    public async ValueTask<TransactionItemResultDto> AddAsync(TransactionItemCreationDto dto)
    {
        var mappedItem = _mapper.Map<TransactionItem>(dto);
        SetCreatedFields(mappedItem); // Auditable maydonlarni qo‘shish

        await _transactionItemRepository.CreateAsync(mappedItem);
        await _transactionItemRepository.SaveChanges();
        return _mapper.Map<TransactionItemResultDto>(mappedItem);
    }

    public async ValueTask<TransactionItemResultDto> ModifyAsync(TransactionItemUpdateDto dto)
    {
        var existItem = await _transactionItemRepository.GetAsync(u => u.Id == dto.Id && !u.IsDeleted)
            ?? throw new NotFoundException($"TransactionItem not found with ID = {dto.Id}");

        _mapper.Map(dto, existItem);
        SetUpdatedFields(existItem); // Auditable maydonlarni yangilash

        _transactionItemRepository.Update(existItem);
        await _transactionItemRepository.SaveChanges();
        return _mapper.Map<TransactionItemResultDto>(existItem);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existItem = await _transactionItemRepository.GetAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new NotFoundException($"TransactionItem not found with ID = {id}");

        existItem.IsDeleted = true; // Soft delete
        SetUpdatedFields(existItem); // Auditable maydonlarni yangilash

        _transactionItemRepository.Update(existItem);
        await _transactionItemRepository.SaveChanges();
        return true;
    }

    public async ValueTask<TransactionItemResultDto> RetrieveByIdAsync(long id)
    {
        var existItem = await _transactionItemRepository.GetAsync(u => u.Id == id && !u.IsDeleted,
            includes: new[] { "Product" }) // Product ni include qilish
            ?? throw new NotFoundException($"TransactionItem not found with ID = {id}");

        return _mapper.Map<TransactionItemResultDto>(existItem);
    }

    public async ValueTask<IEnumerable<TransactionItemResultDto>> RetrieveAllAsync()
    {
        var items = await _transactionItemRepository.GetAll(t => !t.IsDeleted,
            includes: new[] { "Product" }) // Product ni include qilish
            .ToListAsync();
        return _mapper.Map<IEnumerable<TransactionItemResultDto>>(items);
    }
}