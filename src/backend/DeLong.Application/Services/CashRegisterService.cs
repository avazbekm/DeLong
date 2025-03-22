using AutoMapper;
using DeLong.Application.DTOs.CashRegisters;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

#pragma warning disable // warninglarni o'chirish uchun
public class CashRegisterService : ICashRegisterService
{
    private readonly IRepository<CashRegister> _repository;
    private readonly IMapper _mapper;

    public CashRegisterService(IRepository<CashRegister> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashRegisterResultDto> AddAsync(CashRegisterCreationDto dto)
    {
        var mappedCashRegister = _mapper.Map<CashRegister>(dto);
        await _repository.CreateAsync(mappedCashRegister);
        await _repository.SaveChanges();

        return _mapper.Map<CashRegisterResultDto>(mappedCashRegister);
    }

    public async ValueTask<CashRegisterResultDto> ModifyAsync(CashRegisterUpdateDto dto)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == dto.Id)
            ?? throw new NotFoundException($"CashRegister not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashRegister);
        _repository.Update(existCashRegister);
        await _repository.SaveChanges();

        return _mapper.Map<CashRegisterResultDto>(existCashRegister);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == id)
            ?? throw new NotFoundException($"CashRegister not found with ID = {id}");

        _repository.Delete(existCashRegister);
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashRegisterResultDto> RetrieveByIdAsync(long id)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == id)
            ?? throw new NotFoundException($"CashRegister not found with ID = {id}");

        return _mapper.Map<CashRegisterResultDto>(existCashRegister);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync()
    {
        var cashRegisters = await _repository.GetAll()
            .Where(r => !r.IsDeleted) // Soft delete tekshiruvi
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByUserIdAsync(long userId)
    {
        var cashRegisters = await _repository.GetAll()
            .Where(r => r.UserId == userId && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByWarehouseIdAsync(long warehouseId)
    {
        var cashRegisters = await _repository.GetAll()
            .Where(r => r.WarehouseId == warehouseId && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveOpenRegistersAsync()
    {
        var openRegisters = await _repository.GetAll()
            .Where(r => r.ClosedAt == null && !r.IsDeleted) // Ochiq va o‘chirilmagan kassalar
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(openRegisters);
    }
}