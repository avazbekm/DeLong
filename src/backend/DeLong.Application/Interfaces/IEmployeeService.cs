using DeLong.Service.DTOs.Employee;

namespace DeLong.Service.Interfaces;

public interface IEmployeeService
{
    ValueTask<EmployeeResultDto> AddAsync(EmployeeCreationDto dto);
    ValueTask<EmployeeResultDto> ModifyAsync(EmployeeUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<EmployeeResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<EmployeeResultDto>> RetrieveAllAsync();
}