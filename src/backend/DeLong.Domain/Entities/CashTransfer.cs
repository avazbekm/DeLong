using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CashTransfer : Auditable
{
    public long CashRegisterId { get; set; }
    public CashRegister? CashRegister { get; set; }

    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTimeOffset TransferDate { get; set; }
}