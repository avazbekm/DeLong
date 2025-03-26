using AutoMapper;
using DeLong.Application.DTOs.Transactions;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class TransactionService : AuditableService, ITransactionService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Transaction> _transactionRepository;

    public TransactionService(IRepository<Transaction> transactionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _transactionRepository = transactionRepository;
    }

    public async ValueTask<TransactionResultDto> AddAsync(TransactionCreationDto dto)
    {
        var mappedTransaction = _mapper.Map<Transaction>(dto);
        SetCreatedFields(mappedTransaction); // Auditable maydonlarni qo‘shish (CreatedBy, CreatedAt)

        await _transactionRepository.CreateAsync(mappedTransaction);
        await _transactionRepository.SaveChanges();

        return _mapper.Map<TransactionResultDto>(mappedTransaction);
    }

    public async ValueTask<TransactionResultDto> ModifyAsync(TransactionUpdateDto dto)
    {
        var existTransaction = await _transactionRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"Transaction not found with ID = {dto.Id}");

        _mapper.Map(dto, existTransaction);
        SetUpdatedFields(existTransaction); // Auditable maydonlarni yangilash (UpdatedBy, UpdatedAt)

        _transactionRepository.Update(existTransaction);
        await _transactionRepository.SaveChanges();

        return _mapper.Map<TransactionResultDto>(existTransaction);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existTransaction = await _transactionRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"Transaction not found with ID = {id}");

        existTransaction.IsDeleted = true; // Soft delete
        SetUpdatedFields(existTransaction); // Auditable maydonlarni yangilash

        _transactionRepository.Update(existTransaction);
        await _transactionRepository.SaveChanges();
        return true;
    }

    public async ValueTask<TransactionResultDto> RetrieveByIdAsync(long id)
    {
        var existTransaction = await _transactionRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted,
            includes: new[] { "Items" }) // TransactionItem’larni yuklash
            ?? throw new NotFoundException($"Transaction not found with ID = {id}");

        return _mapper.Map<TransactionResultDto>(existTransaction);
    }

    public async ValueTask<IEnumerable<TransactionResultDto>> RetrieveAllAsync()
    {
        var transactions = await _transactionRepository.GetAll(t => !t.IsDeleted,
            includes: new[] { "Items" }) // TransactionItem’larni yuklash
            .ToListAsync();

        return _mapper.Map<IEnumerable<TransactionResultDto>>(transactions);
    }
}