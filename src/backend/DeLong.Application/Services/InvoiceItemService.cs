using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.DTOs.InvoiceItems;

namespace DeLong.Service.Services;

public class InvoiceItemService:IInvoiceItemService
{
    private readonly IMapper mapper;
    private readonly IRepository<InvoiceItem> invoiceItemRepository;
    public InvoiceItemService(IRepository<InvoiceItem> invoiceItemRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.invoiceItemRepository = invoiceItemRepository;
    }

    public async ValueTask<InvoiceItemResultDto> AddAsync(InvoiceItemCreationDto dto)
    {
        InvoiceItem existInvoiceItem = await this.invoiceItemRepository.GetAsync(u => u.InvoiceId.Equals(dto.InvoiceId));
        if (existInvoiceItem is not null)
            throw new AlreadyExistException($"This invoiceItem is already exists with InvoiceId = {dto.InvoiceId}");

        var mappedInvoiceItems = this.mapper.Map<InvoiceItem>(dto);
        await this.invoiceItemRepository.CreateAsync(mappedInvoiceItems);
        await this.invoiceItemRepository.SaveChanges();

        var result = this.mapper.Map<InvoiceItemResultDto>(mappedInvoiceItems);
        return result;
    }

    public async ValueTask<InvoiceItemResultDto> ModifyAsync(InvoiceItemUpdateDto dto)
    {
        InvoiceItem existInvoiceItems = await this.invoiceItemRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This invoiceItem is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existInvoiceItems);
        this.invoiceItemRepository.Update(existInvoiceItems);
        await this.invoiceItemRepository.SaveChanges();

        var result = this.mapper.Map<InvoiceItemResultDto>(existInvoiceItems);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        InvoiceItem existInvoiceItems = await this.invoiceItemRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This invoiceItem is not found with ID = {id}");

        this.invoiceItemRepository.Delete(existInvoiceItems);
        await this.invoiceItemRepository.SaveChanges();
        return true;
    }

    public async ValueTask<InvoiceItemResultDto> RetrieveByIdAsync(long id)
    {
        InvoiceItem existInvoiceItems = await this.invoiceItemRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This invoiceItem is not found with ID = {id}");

        var result = this.mapper.Map<InvoiceItemResultDto>(existInvoiceItems);
        return result;
    }

    public async ValueTask<IEnumerable<InvoiceItemResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var invoiceItems = await this.invoiceItemRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = invoiceItems.Where(invoiceItem => invoiceItem.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedInvoiceItems = this.mapper.Map<List<InvoiceItemResultDto>>(result);
        return mappedInvoiceItems;
    }

    public async ValueTask<IEnumerable<InvoiceItemResultDto>> RetrieveAllAsync()
    {
        var invoiceItems = await this.invoiceItemRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<InvoiceItemResultDto>>(invoiceItems);
        return result;
    }
}
