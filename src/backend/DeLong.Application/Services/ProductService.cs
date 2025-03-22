using AutoMapper;
using DeLong.Application.DTOs.Products;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class ProductService : IProductService
{
    private readonly IMapper mapper;
    private readonly IRepository<Product> productRepository;
    public ProductService(IRepository<Product> productRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.productRepository = productRepository;
    }

    public async ValueTask<ProductResultDto> AddAsync(ProductCreationDto dto)
    {
        Product existProduct = await this.productRepository.GetAsync(u => u.Name.Equals(dto.Name));
        if (existProduct is not null)
            throw new AlreadyExistException($"This Product is already exists with Name = {dto.Name}");

        var mappedProducts = this.mapper.Map<Product>(dto);
        await this.productRepository.CreateAsync(mappedProducts);
        await this.productRepository.SaveChanges();

        var result = this.mapper.Map<ProductResultDto>(mappedProducts);
        return result;
    }

    public async ValueTask<ProductResultDto> ModifyAsync(ProductUpdateDto dto)
    {
        Product existProduct = await this.productRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Product is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existProduct);
        this.productRepository.Update(existProduct);
        await this.productRepository.SaveChanges();

        var result = this.mapper.Map<ProductResultDto>(existProduct);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Product existProduct = await this.productRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Product is not found with ID = {id}");

        this.productRepository.Delete(existProduct);
        await this.productRepository.SaveChanges();
        return true;
    }

    public async ValueTask<ProductResultDto> RetrieveByIdAsync(long id)
    {
        Product existProduct = await this.productRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Product is not found with ID = {id}");

        var result = this.mapper.Map<ProductResultDto>(existProduct);
        return result;
    }

    public async ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var products = await this.productRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = products.Where(product => product.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedProducts = this.mapper.Map<List<ProductResultDto>>(result);
        return mappedProducts;
    }

    public async ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync()
    {
        var products = await this.productRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<ProductResultDto>>(products);
        return result;
    }
}
