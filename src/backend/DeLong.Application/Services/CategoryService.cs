using AutoMapper;
using DeLong.Application.DTOs.Categories;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class CategoryService : AuditableService, ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async ValueTask<CategoryResultDto> AddAsync(CategoryCreationDto dto)
    {
        Category existCategory = await _categoryRepository.GetAsync(u => u.Name.Equals(dto.Name) && !u.IsDeleted);
        if (existCategory is not null)
            throw new AlreadyExistException($"This Category is already exists with Name = {dto.Name}");

        var mappedCategory = _mapper.Map<Category>(dto);
        SetCreatedFields(mappedCategory); // Auditable maydonlarni qo‘shish
        mappedCategory.BranchId = GetCurrentBranchId();
        await _categoryRepository.CreateAsync(mappedCategory);
        await _categoryRepository.SaveChanges();

        return _mapper.Map<CategoryResultDto>(mappedCategory);
    }

    public async ValueTask<CategoryResultDto> ModifyAsync(CategoryUpdateDto dto)
    {
        Category existCategory = await _categoryRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This category is not found with ID = {dto.Id}");

        _mapper.Map(dto, existCategory);
        SetUpdatedFields(existCategory); // Auditable maydonlarni yangilash

        _categoryRepository.Update(existCategory);
        await _categoryRepository.SaveChanges();

        return _mapper.Map<CategoryResultDto>(existCategory);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Category existCategory = await _categoryRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This category is not found with ID = {id}");

        existCategory.IsDeleted = true; // Soft delete
        SetUpdatedFields(existCategory); // Auditable maydonlarni yangilash

        _categoryRepository.Update(existCategory); // Delete o‘rniga Update
        await _categoryRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CategoryResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        Category existCategory = await _categoryRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This category is not found with ID = {id}");

        return _mapper.Map<CategoryResultDto>(existCategory);
    }

    public async ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var categoriesQuery = _categoryRepository.GetAll(u => !u.IsDeleted)
            .ToPaginate(@params)
            .OrderBy(filter);

        if (!string.IsNullOrEmpty(search))
        {
            categoriesQuery = categoriesQuery.Where(category => category.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var categories = await categoriesQuery.ToListAsync();
        return _mapper.Map<List<CategoryResultDto>>(categories);
    }

    public async ValueTask<IEnumerable<CategoryResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var categories = await _categoryRepository.GetAll(u => !u.IsDeleted && u.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<List<CategoryResultDto>>(categories);
    }
}
