using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Customers;

namespace DeLong.Service.Interfaces;

public interface ICustomerService
{
    ValueTask<CustomerResultDto> AddAsync(CustomerCreationDto dto);
    ValueTask<CustomerResultDto> ModifyAsync(CustomerUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByIdAsync(long id);
    ValueTask<CustomerResultDto> RetrieveByPhoneAsync(string phone);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync();
}
