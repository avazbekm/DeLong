using AutoMapper;
using DeLong.Application.DTOs.Categories;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly IMapper mapper;
    private readonly IRepository<Category> categoryRepository;
    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.categoryRepository = categoryRepository;
    }

    public async ValueTask<CategoryResultDto> AddAsync(CategoryCreationDto dto)
    {
        Category existCategory = await this.categoryRepository.GetAsync(u => u.Name.Equals(dto.Name));
        if (existCategory is not null)
            throw new AlreadyExistException($"This Category is already exists with Name = {dto.Name}");

        var mappedCategorys = this.mapper.Map<Category>(dto);
        await this.categoryRepository.CreateAsync(mappedCategorys);
        await this.categoryRepository.SaveChanges();

        var result = this.mapper.Map<CategoryResultDto>(mappedCategorys);
        return result;
    }

    public async ValueTask<CategoryResultDto> ModifyAsync(CategoryUpdateDto dto)
    {
        Category existCategory = await this.categoryRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This category is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existCategory);
        this.categoryRepository.Update(existCategory);
        await this.categoryRepository.SaveChanges();

        var result = this.mapper.Map<CategoryResultDto>(existCategory);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Category existCategory = await this.categoryRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This category is not found with ID = {id}");

        this.categoryRepository.Delete(existCategory);
        await this.categoryRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CategoryResultDto> RetrieveByIdAsync(long id)
    {
        Category existCategory = await this.categoryRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This category is not found with ID = {id}");

        var result = this.mapper.Map<CategoryResultDto>(existCategory);
        return result;
    }

    public async ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var categories = await this.categoryRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = categories.Where(category => category.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedCategories = this.mapper.Map<List<CategoryResultDto>>(result);
        return mappedCategories;
    }

    public async ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync()
    {
        var categories = await this.categoryRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<List<CategoryResultDto>>(categories);
        return result;
    }
}
