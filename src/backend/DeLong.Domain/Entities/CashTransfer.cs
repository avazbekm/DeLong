using DeLong.Domain.Common;
using DeLong.Domain.Enums;

namespace DeLong.Domain.Entities;

public class CashTransfer : Auditable
{
    public long CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }
    public long BranchId { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public CashTransferType TransferType { get; set; } // Yangi qo‘shildi
    public DateTimeOffset TransferDate { get; set; } = DateTimeOffset.UtcNow;
}