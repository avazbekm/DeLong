using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CashRegister : Auditable
{
    public long WarehouseId { get; set; }
    public required Warehouse Warehouse { get; set; }
    public decimal Balance { get; set; }
}