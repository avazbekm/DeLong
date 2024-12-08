using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Stocks;

namespace DeLong.Service.Interfaces;

public interface IStockService
{
    ValueTask<StockResultDto> AddAsync(StockCreationDto dto);
    ValueTask<StockResultDto> ModifyAsync(StockUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<StockResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<StockResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<StockResultDto>> RetrieveAllAsync();
}
