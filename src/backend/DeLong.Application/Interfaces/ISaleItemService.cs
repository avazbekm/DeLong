using DeLong.Domain.Configurations;
using DeLong.Service.DTOs.SaleItems;

public interface ISaleItemService
{
    ValueTask<SaleItemResultDto> AddAsync(SaleItemCreationDto dto);
    ValueTask<SaleItemResultDto> ModifyAsync(SaleItemUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<SaleItemResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter);
    ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllBySaleIdAsync(long saleId);
}