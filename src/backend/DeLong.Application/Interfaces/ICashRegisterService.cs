using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.CashRegisters;

namespace DeLong.Service.Interfaces;

#pragma warning disable // warninglarni o'chirish uchun
public interface ICashRegisterService
{
    ValueTask<CashRegisterResultDto> AddAsync(CashRegisterCreationDto dto);
    ValueTask<CashRegisterResultDto> ModifyAsync(CashRegisterUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CashRegisterResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync();
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByUserIdAsync(long userId);
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByWarehouseIdAsync(long warehouseId);
    ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveOpenRegistersAsync();
}