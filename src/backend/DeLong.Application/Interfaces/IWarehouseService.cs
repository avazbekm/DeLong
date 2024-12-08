using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Warehouses;

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
