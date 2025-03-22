using DeLong.Application.DTOs.Warehouses;
using DeLong.Domain.Configurations;

namespace DeLong.Service.Interfaces;

public interface IWarehouseService
{
    ValueTask<WarehouseResultDto> AddAsync(WarehouseCreationDto dto);
    ValueTask<WarehouseResultDto> ModifyAsync(WarehouseUpdatedDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<WarehouseResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync();
}
