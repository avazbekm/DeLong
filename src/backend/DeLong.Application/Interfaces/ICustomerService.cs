using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Customers;

namespace DeLong.Service.Interfaces;
#pragma warning disable
public interface ICustomerService
{
    ValueTask<CustomerResultDto> AddAsync(CustomerCreationDto dto);
    ValueTask<bool> ModifyAsync(CustomerUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByIdAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByInnAsync(int INN);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync();
}