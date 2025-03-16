using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Service.DTOs.CashWarehouse;

namespace DeLong.Service.Services;

public class CashWarehouseService : ICashWarehouseService
{
    private readonly IRepository<CashWarehouse> _repository;
    private readonly IMapper _mapper;

    public CashWarehouseService(IRepository<CashWarehouse> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashWarehouseResultDto> AddAsync(CashWarehouseCreationDto dto)
    {
        var mappedCashWarehouse = _mapper.Map<CashWarehouse>(dto);
        await _repository.CreateAsync(mappedCashWarehouse);
        await _repository.SaveChanges();

        return _mapper.Map<CashWarehouseResultDto>(mappedCashWarehouse);
    }

    public async ValueTask<CashWarehouseResultDto> ModifyAsync(CashWarehouseUpdateDto dto)
    {
        var existCashWarehouse = await _repository.GetAsync(w => w.Id == dto.Id)
            ?? throw new NotFoundException($"CashWarehouse not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashWarehouse);
        _repository.Update(existCashWarehouse);
        await _repository.SaveChanges();

        return _mapper.Map<CashWarehouseResultDto>(existCashWarehouse);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashWarehouse = await _repository.GetAsync(w => w.Id == id)
            ?? throw new NotFoundException($"CashWarehouse not found with ID = {id}");

        _repository.Delete(existCashWarehouse);
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashWarehouseResultDto> RetrieveByIdAsync(long id)
    {
        var existCashWarehouse = await _repository.GetAsync(w => w.Id == id)
            ?? throw new NotFoundException($"CashWarehouse not found with ID = {id}");

        return _mapper.Map<CashWarehouseResultDto>(existCashWarehouse);
    }

    public async ValueTask<IEnumerable<CashWarehouseResultDto>> RetrieveAllAsync()
    {
        var cashWarehouses = await _repository.GetAll()
            .Where(w => !w.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashWarehouseResultDto>>(cashWarehouses);
    }
}