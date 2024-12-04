using DeLong.Domain.Entities;

namespace DeLong.Application.DTOs.CashRegisters;

public class CashRegisterResultDto
{
    public long Id { get; set; }
    public decimal Balance { get; set; }
    public required Warehouse Warehouse { get; set; }
}

