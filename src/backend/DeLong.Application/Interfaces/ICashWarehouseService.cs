using DeLong.Service.DTOs.CashWarehouse;

namespace DeLong.Service.Interfaces;

public interface ICashWarehouseService
{
    ValueTask<CashWarehouseResultDto> AddAsync(CashWarehouseCreationDto dto);
    ValueTask<CashWarehouseResultDto> ModifyAsync(CashWarehouseUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<CashWarehouseResultDto> RetrieveByIdAsync();
    ValueTask<IEnumerable<CashWarehouseResultDto>> RetrieveAllAsync();
}