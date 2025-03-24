using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.CashTransfers;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class CashTransferService : ICashTransferService
{
    private readonly IRepository<CashTransfer> _repository;
    private readonly IMapper _mapper;

    public CashTransferService(IRepository<CashTransfer> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashTransferResultDto> AddAsync(CashTransferCreationDto dto)
    {
        var mappedCashTransfer = _mapper.Map<CashTransfer>(dto);
        await _repository.CreateAsync(mappedCashTransfer);
        await _repository.SaveChanges();

        return _mapper.Map<CashTransferResultDto>(mappedCashTransfer);
    }

    public async ValueTask<CashTransferResultDto> ModifyAsync(CashTransferUpdateDto dto)
    {
        var existCashTransfer = await _repository.GetAsync(t => t.Id == dto.Id)
            ?? throw new NotFoundException($"CashTransfer not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashTransfer);
        _repository.Update(existCashTransfer);
        await _repository.SaveChanges();

        return _mapper.Map<CashTransferResultDto>(existCashTransfer);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashTransfer = await _repository.GetAsync(t => t.Id == id)
            ?? throw new NotFoundException($"CashTransfer not found with ID = {id}");

        _repository.Delete(existCashTransfer);
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashTransferResultDto> RetrieveByIdAsync(long id)
    {
        var existCashTransfer = await _repository.GetAsync(t => t.Id == id)
            ?? throw new NotFoundException($"CashTransfer not found with ID = {id}");

        return _mapper.Map<CashTransferResultDto>(existCashTransfer);
    }

    public async ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllAsync()
    {
        var cashTransfers = await _repository.GetAll()
            .Where(t => !t.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashTransferResultDto>>(cashTransfers);
    }

    public async ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllByCashRegisterIdAsync(long cashRegisterId)
    {
        var cashTransfers = await _repository.GetAll()
            .Where(t => t.CashRegisterId == cashRegisterId && !t.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashTransferResultDto>>(cashTransfers);
    }
}