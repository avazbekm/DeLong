using AutoMapper;
using DeLong.Application.DTOs.Transactions;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly IMapper mapper;
    private readonly IRepository<Transaction> transactionRepository;

    public TransactionService(IRepository<Transaction> transactionRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.transactionRepository = transactionRepository;
    }

    public async ValueTask<TransactionResultDto> AddAsync(TransactionCreationDto dto)
    {
        var mappedTransaction = this.mapper.Map<Transaction>(dto);
        await this.transactionRepository.CreateAsync(mappedTransaction);
        await this.transactionRepository.SaveChanges();

        var result = this.mapper.Map<TransactionResultDto>(mappedTransaction);
        return result;
    }

    public async ValueTask<TransactionResultDto> ModifyAsync(TransactionUpdateDto dto)
    {
        Transaction existTransaction = await this.transactionRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"Transaction not found with ID = {dto.Id}");

        this.mapper.Map(dto, existTransaction);
        this.transactionRepository.Update(existTransaction);
        await this.transactionRepository.SaveChanges();

        var result = this.mapper.Map<TransactionResultDto>(existTransaction);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Transaction existTransaction = await this.transactionRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"Transaction not found with ID = {id}");

        this.transactionRepository.Delete(existTransaction);
        await this.transactionRepository.SaveChanges();
        return true;
    }

    public async ValueTask<TransactionResultDto> RetrieveByIdAsync(long id)
    {
        Transaction existTransaction = await this.transactionRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"Transaction not found with ID = {id}");

        var result = this.mapper.Map<TransactionResultDto>(existTransaction);
        return result;
    }

    public async ValueTask<IEnumerable<TransactionResultDto>> RetrieveAllAsync()
    {
        var transactions = await this.transactionRepository.GetAll()
            .Include(t => t.Items) // TransactionItem larini yuklash
            .ToListAsync();
        return this.mapper.Map<IEnumerable<TransactionResultDto>>(transactions);
    }
}