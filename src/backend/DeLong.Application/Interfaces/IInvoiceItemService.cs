using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.InvoiceItems;

namespace DeLong.Service.Interfaces;

public interface IInvoiceItemService
{
    ValueTask<InvoiceItemResultDto> AddAsync(InvoiceItemCreationDto dto);
    ValueTask<InvoiceItemResultDto> ModifyAsync(InvoiceItemUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<InvoiceItemResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<InvoiceItemResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<InvoiceItemResultDto>> RetrieveAllAsync();
}
