using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Assets;

namespace DeLong.Service.Interfaces;

public interface IAssetService
{
    ValueTask<AssetResultDto> AddAsync(AssetCreationDto dto);
    ValueTask<AssetResultDto> ModifyAsync(AssetUpdateDto dto);
    ValueTask<bool> RemoveAsync(long id);
    ValueTask<AssetResultDto> RetrieveByIdAsync(long id);
    ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync();
}
