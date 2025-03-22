using DeLong.Application.DTOs.Customers;
using DeLong.Domain.Configurations;

namespace DeLong.Service.Interfaces;
#pragma warning disable
public interface ICustomerService
{
    ValueTask<CustomerResultDto> AddAsync(CustomerCreationDto dto);
    ValueTask<CustomerResultDto> ModifyAsync(CustomerUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByIdAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByInnAsync(int INN);
    ValueTask<CustomerResultDto> RetrieveByJshshirAsync(string jshshir);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync();
}
