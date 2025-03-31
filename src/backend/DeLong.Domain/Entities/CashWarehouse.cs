using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CashWarehouse : Auditable
{
    public long BranchId { get; set; }
    public decimal UzsBalance { get; set; } // Ombordagi so‘m
    public decimal UzpBalance { get; set; }  // plastik mablag' qoldiq
    public decimal UsdBalance { get; set; } // Ombordagi dollar
    public List<CashRegister> CashRegisters { get; set; } = new();
}