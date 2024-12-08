using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Application.DTOs.CashRegisters;

namespace DeLong.Service.Services;

public class CashRegisterService:ICashRegisterService
{
    private readonly IMapper mapper;
    private readonly IRepository<CashRegister> cashRegisterRepository;
    public CashRegisterService(IRepository<CashRegister> cashRegisterRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.cashRegisterRepository = cashRegisterRepository;
    }

    public async ValueTask<CashRegisterResultDto> AddAsync(CashRegisterCreationDto dto)
    {
        CashRegister existCashRegister = await this.cashRegisterRepository.GetAsync(u => u.WarehouseId.Equals(dto.WarehouseId));
        if (existCashRegister is not null)
            throw new AlreadyExistException($"This Cashregister is already exists with Id = {dto.WarehouseId}");

        var mappedCashRegisters = this.mapper.Map<CashRegister>(dto);
        await this.cashRegisterRepository.CreateAsync(mappedCashRegisters);
        await this.cashRegisterRepository.SaveChanges();

        var result = this.mapper.Map<CashRegisterResultDto>(mappedCashRegisters);
        return result;
    }

    public async ValueTask<CashRegisterResultDto> ModifyAsync(CashRegisterUpdateDto dto)
    {
        CashRegister existCashRegister = await this.cashRegisterRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This CashRegister is not found with ID = {dto.Id}");

        var mappedCashRegister = this.mapper.Map(dto, existCashRegister);
        this.cashRegisterRepository.Update(mappedCashRegister);
        await this.cashRegisterRepository.SaveChanges();

        var result = this.mapper.Map<CashRegisterResultDto>(mappedCashRegister);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        CashRegister existCashRegisters = await this.cashRegisterRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This CashRegisters is not found with ID = {id}");

        this.cashRegisterRepository.Delete(existCashRegisters);
        await this.cashRegisterRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CashRegisterResultDto> RetrieveByIdAsync(long id)
    {
        CashRegister existCashRegister = await this.cashRegisterRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This cashregister is not found with ID = {id}");

        var result = this.mapper.Map<CashRegisterResultDto>(existCashRegister);
        return result;
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var cashregisters = await this.cashRegisterRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = cashregisters.Where(cashregister => cashregister.WarehouseId.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedCashRegisters = this.mapper.Map<List<CashRegisterResultDto>>(result);
        return mappedCashRegisters;
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync()
    {
        var cashregisters = await this.cashRegisterRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<CashRegisterResultDto>>(cashregisters);
        return result;
    }
}
