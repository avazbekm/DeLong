using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Service.DTOs.Branchs;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Data.IRepository;
using System.Text.Json;
using System.Linq.Expressions;

namespace DeLong.Service.Services;

public class BranchService : AuditableService, IBranchService
{
    private readonly IRepository<Branch> _repository;
    private readonly IChangeHistoryRepository _changeHistoryRepository;
    private readonly IMapper _mapper;

    public BranchService(
        IRepository<Branch> repository,
        IChangeHistoryRepository changeHistoryRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _repository = repository;
        _changeHistoryRepository = changeHistoryRepository;
        _mapper = mapper;
    }

    public async ValueTask<BranchResultDto> AddAsync(BranchCreationDto dto)
    {
        var branch = _mapper.Map<Branch>(dto);
        SetCreatedFields(branch);

        await _repository.CreateAsync(branch);
        await _repository.SaveChanges();

        await _changeHistoryRepository.LogChangeAsync<Branch>(null, branch, branch.Id, nameof(Branch), "Insert");

        return _mapper.Map<BranchResultDto>(branch);
    }

    public async ValueTask<BranchResultDto> ModifyAsync(BranchUpdateDto dto)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == dto.Id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {dto.Id}");

        var oldBranch = new Branch
        {
            Id = existBranch.Id,
            BranchName = existBranch.BranchName,
            Location = existBranch.Location,
            CreatedBy = existBranch.CreatedBy,
            CreatedAt = existBranch.CreatedAt,
            UpdatedBy = existBranch.UpdatedBy,
            UpdatedAt = existBranch.UpdatedAt,
            IsDeleted = existBranch.IsDeleted
        };

        _mapper.Map(dto, existBranch);
        SetUpdatedFields(existBranch);

        _repository.Update(existBranch);
        await _repository.SaveChanges();

        await _changeHistoryRepository.LogChangeAsync(oldBranch, existBranch, existBranch.Id, nameof(Branch), "Update");

        return _mapper.Map<BranchResultDto>(existBranch);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {id}");

        var oldBranch = new Branch
        {
            Id = existBranch.Id,
            BranchName = existBranch.BranchName,
            Location = existBranch.Location,
            CreatedBy = existBranch.CreatedBy,
            CreatedAt = existBranch.CreatedAt,
            UpdatedBy = existBranch.UpdatedBy,
            UpdatedAt = existBranch.UpdatedAt,
            IsDeleted = existBranch.IsDeleted
        };

        existBranch.IsDeleted = true;
        SetUpdatedFields(existBranch);

        _repository.Update(existBranch);
        await _repository.SaveChanges();

        await _changeHistoryRepository.LogChangeAsync(oldBranch, existBranch, existBranch.Id, nameof(Branch), "Delete");

        return true;
    }

    public async ValueTask<BranchResultDto> RetrieveByIdAsync(long id)
    {
        var existBranch = await _repository.GetAsync(b => b.Id == id && !b.IsDeleted)
            ?? throw new NotFoundException($"Branch not found with ID = {id}");

        return _mapper.Map<BranchResultDto>(existBranch);
    }

    public async ValueTask<IEnumerable<BranchResultDto>> RetrieveAllAsync()
    {
        var branches = await _repository.GetAll(b => !b.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<BranchResultDto>>(branches);
    }

    public async ValueTask<IEnumerable<BranchChangeHistoryDto>> GetChangeHistoryAsync(long branchId)
    {
        // Check if branch exists
        var branchExists = await _repository.GetAsync(b => b.Id == branchId && !b.IsDeleted);
        if (branchExists == null)
            throw new NotFoundException($"Branch not found with ID = {branchId}");

        // Get change history for this branch
        var changes = await _changeHistoryRepository.GetAllChangeHistoryAsync(nameof(Branch));
            

        return changes.Select(change => new BranchChangeHistoryDto
        {
            Id = change.Id,
            ChangeType = change.ChangeType,
            OldValues = change.OldValues,
            NewValues = change.NewValues,
            CreatedAt = change.CreatedAt,
            CreatedBy = change.CreatedBy
        });
    }
}