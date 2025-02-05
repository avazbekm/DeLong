using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Invoices;

namespace DeLong.Service.Interfaces;

public interface IInvoiceService
{
    ValueTask<InvoiceResultDto> AddAsync(InvoiceCreationDto dto);
    ValueTask<InvoiceResultDto> ModifyAsync(InvoiceUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<InvoiceResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<InvoiceResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<InvoiceResultDto>> RetrieveAllAsync();
}