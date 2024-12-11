using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Application.DTOs.Invoices;

namespace DeLong.Service.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IMapper mapper;
    private readonly IRepository<Invoice> invoiceRepository;
    public InvoiceService(IRepository<Invoice> invoiceRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.invoiceRepository = invoiceRepository;
    }

    public async ValueTask<InvoiceResultDto> AddAsync(InvoiceCreationDto dto)
    {
        Invoice existInvoice = await this.invoiceRepository.GetAsync(u => u.CustomerId.Equals(dto.CustomerId));
        if (existInvoice is not null)
            throw new AlreadyExistException($"This invoice is already exists with CustomerId = {dto.CustomerId}");

        var mappedInvoices = this.mapper.Map<Invoice>(dto);
        await this.invoiceRepository.CreateAsync(mappedInvoices);
        await this.invoiceRepository.SaveChanges();

        var result = this.mapper.Map<InvoiceResultDto>(mappedInvoices);
        return result;
    }

    public async ValueTask<InvoiceResultDto> ModifyAsync(InvoiceUpdateDto dto)
    {
        Invoice existInvoice = await this.invoiceRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This invoice is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existInvoice);
        this.invoiceRepository.Update(existInvoice);
        await this.invoiceRepository.SaveChanges();

        var result = this.mapper.Map<InvoiceResultDto>(existInvoice);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Invoice existInvoices = await this.invoiceRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This invoice is not found with ID = {id}");

        this.invoiceRepository.Delete(existInvoices);
        await this.invoiceRepository.SaveChanges();
        return true;
    }

    public async ValueTask<InvoiceResultDto> RetrieveByIdAsync(long id)
    {
        Invoice existInvoice = await this.invoiceRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This invoice is not found with ID = {id}");

        var result = this.mapper.Map<InvoiceResultDto>(existInvoice);
        return result;
    }

    public async ValueTask<IEnumerable<InvoiceResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var invoices = await this.invoiceRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = invoices.Where(invoice => invoice.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedInvoices = this.mapper.Map<List<InvoiceResultDto>>(result);
        return mappedInvoices;
    }

    public async ValueTask<IEnumerable<InvoiceResultDto>> RetrieveAllAsync()
    {
        var invoices = await this.invoiceRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<InvoiceResultDto>>(invoices);
        return result;
    }
}