using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Employee;

namespace DeLong.Service.Interfaces;

public interface IEmployeeService
{
    ValueTask<EmployeeResultDto> AddAsync(EmployeeCreationDto dto);
    ValueTask<EmployeeResultDto> ModifyAsync(EmployeeUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<IEnumerable<EmployeeResultDto>> RetrieveAllAsync();
    ValueTask<EmployeeResultDto> RetrieveByIdAsync(long id);
    ValueTask<Employee> VerifyEmployeeAsync(string username, string password); // Yangi metod
}