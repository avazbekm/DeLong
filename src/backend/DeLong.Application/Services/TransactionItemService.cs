using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.TransactionItems;

namespace DeLong.Service.Services;

public class TransactionItemService : ITransactionItemService
{
    private readonly IMapper mapper;
    private readonly IRepository<TransactionItem> transactionItemRepository;

    public TransactionItemService(IRepository<TransactionItem> transactionItemRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.transactionItemRepository = transactionItemRepository;
    }

    public async ValueTask<TransactionItemResultDto> AddAsync(TransactionItemCreationDto dto)
    {
        // ProductId bo'yicha takrorlanishni tekshirish
        TransactionItem existItem = await this.transactionItemRepository.GetAsync(u => u.ProductId.Equals(dto.ProductId));
        if (existItem != null)
            throw new AlreadyExistException($"TransactionItem already exists with ProductId = {dto.ProductId}");

        var mappedItem = this.mapper.Map<TransactionItem>(dto);
        await this.transactionItemRepository.CreateAsync(mappedItem);
        await this.transactionItemRepository.SaveChanges();

        var result = this.mapper.Map<TransactionItemResultDto>(mappedItem);
        return result;
    }

    public async ValueTask<TransactionItemResultDto> ModifyAsync(TransactionItemUpdateDto dto)
    {
        TransactionItem existItem = await this.transactionItemRepository.GetAsync(u => u.ProductId.Equals(dto.ProductId))
            ?? throw new NotFoundException($"TransactionItem not found with ProductId = {dto.ProductId}");

        this.mapper.Map(dto, existItem);
        this.transactionItemRepository.Update(existItem);
        await this.transactionItemRepository.SaveChanges();

        var result = this.mapper.Map<TransactionItemResultDto>(existItem);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        TransactionItem existItem = await this.transactionItemRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"TransactionItem not found with ID = {id}");

        this.transactionItemRepository.Delete(existItem);
        await this.transactionItemRepository.SaveChanges();
        return true;
    }

    public async ValueTask<TransactionItemResultDto> RetrieveByIdAsync(long id)
    {
        TransactionItem existItem = await this.transactionItemRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"TransactionItem not found with ID = {id}");

        var result = this.mapper.Map<TransactionItemResultDto>(existItem);
        return result;
    }

    public async ValueTask<IEnumerable<TransactionItemResultDto>> RetrieveAllAsync()
    {
        var items = await this.transactionItemRepository.GetAll()
            .Include(i => i.Product) // Product ni yuklash
            .ToListAsync();
        return this.mapper.Map<IEnumerable<TransactionItemResultDto>>(items);
    }
}