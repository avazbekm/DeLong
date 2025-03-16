using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CashTransfer : Auditable
{
    public long CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }

    public decimal UzsBalance { get; set; }  // so'm mablag' qoldiq
    public decimal UzpBalance { get; set; }  // plastik mablag' qoldiq
    public decimal UsdBalance { get; set; }  // dollar mablag' qoldiq
    public decimal DebtAmount { get; set; } // Nasiya summasi
    public string TransferType { get; set; } = string.Empty; // "FromStorage" (ombordan kassaga) yoki "ToStorage" (kassadan omborga)
}