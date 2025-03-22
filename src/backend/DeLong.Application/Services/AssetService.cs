using AutoMapper;
using DeLong.Application.DTOs.Assets;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class AssetService : IAssetService
{
    private readonly IMapper mapper;
    private readonly IRepository<Asset> assetRepository;
    public AssetService(IRepository<Asset> assetRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.assetRepository = assetRepository;
    }

    public async ValueTask<AssetResultDto> AddAsync(AssetCreationDto dto)
    {
        Asset existAsset = await this.assetRepository.GetAsync(u => u.FileName.Equals(dto.FileName));
        if (existAsset is not null)
            throw new AlreadyExistException($"This asset is already exists with FileName = {dto.FileName}");

        var mappedAssets = this.mapper.Map<Asset>(dto);
        await this.assetRepository.CreateAsync(mappedAssets);
        await this.assetRepository.SaveChanges();

        var result = this.mapper.Map<AssetResultDto>(mappedAssets);
        return result;
    }

    public async ValueTask<AssetResultDto> ModifyAsync(AssetUpdateDto dto)
    {
        Asset existAsset = await this.assetRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This asset is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existAsset);
        this.assetRepository.Update(existAsset);
        await this.assetRepository.SaveChanges();

        var result = this.mapper.Map<AssetResultDto>(existAsset);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Asset existAsset = await this.assetRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This asset is not found with ID = {id}");

        this.assetRepository.Delete(existAsset);
        await this.assetRepository.SaveChanges();
        return true;
    }

    public async ValueTask<AssetResultDto> RetrieveByIdAsync(long id)
    {
        Asset existAsset = await this.assetRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This asset is not found with ID = {id}");

        var result = this.mapper.Map<AssetResultDto>(existAsset);
        return result;
    }

    public async ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var customers = await this.assetRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = customers.Where(customer => customer.FileName.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedAssets = this.mapper.Map<List<AssetResultDto>>(result);
        return mappedAssets;
    }

    public async ValueTask<IEnumerable<AssetResultDto>> RetrieveAllAsync()
    {
        var customers = await this.assetRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<AssetResultDto>>(customers);
        return result;
    }
}
