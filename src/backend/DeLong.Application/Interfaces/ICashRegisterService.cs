using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.CashRegisters;

namespace DeLong.Service.Interfaces;

public interface ICashRegisterService
{
    ValueTask<CashRegisterResultDto> AddAsync(CashRegisterCreationDto dto);
    ValueTask<CashRegisterResultDto> ModifyAsync(CashRegisterUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CashRegisterResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync();
}
