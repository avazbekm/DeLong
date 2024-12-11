using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.DTOs.Transactions;

namespace DeLong.Service.Services;

public class TransactionService:ITransactionService
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
        Transaction existTransactions = await this.transactionRepository.GetAsync(u => u.ProductId.Equals(dto.ProductId));
        if (existTransactions is not null)
            throw new AlreadyExistException($"This Transaction is already exists with ProductId = {dto.ProductId}");

        var mappedTransactions = this.mapper.Map<Transaction>(dto);
        await this.transactionRepository.CreateAsync(mappedTransactions);
        await this.transactionRepository.SaveChanges();

        var result = this.mapper.Map<TransactionResultDto>(mappedTransactions);
        return result;
    }

    public async ValueTask<TransactionResultDto> ModifyAsync(TransactionUpdateDto dto)
    {
        Transaction existTransactions = await this.transactionRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Transaction is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existTransactions);
        this.transactionRepository.Update(existTransactions);
        await this.transactionRepository.SaveChanges();

        var result = this.mapper.Map<TransactionResultDto>(existTransactions);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Transaction existTransactions = await this.transactionRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Transaction is not found with ID = {id}");

        this.transactionRepository.Delete(existTransactions);
        await this.transactionRepository.SaveChanges();
        return true;
    }

    public async ValueTask<TransactionResultDto> RetrieveByIdAsync(long id)
    {
        Transaction existTransactions = await this.transactionRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Transaction is not found with ID = {id}");

        var result = this.mapper.Map<TransactionResultDto>(existTransactions);
        return result;
    }

    public async ValueTask<IEnumerable<TransactionResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var transactions = await this.transactionRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = transactions.Where(transaction => transaction.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedTransactions = this.mapper.Map<List<TransactionResultDto>>(result);
        return mappedTransactions;
    }

    public async ValueTask<IEnumerable<TransactionResultDto>> RetrieveAllAsync()
    {
        var transactions = await this.transactionRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<TransactionResultDto>>(transactions);
        return result;
    }
}
