using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CashRegister : Auditable
{
    public long WarehouseId { get; set; }
    public required Warehouse Warehouse { get; set; }
    public decimal UzsBalance { get; set; }  // so'm mablag' qoldiq
    public decimal UzpBalance { get; set; }  // plastik mablag' qoldiq
    public decimal UsdBalance { get; set; }  // dollar mablag' qoldiq
}
