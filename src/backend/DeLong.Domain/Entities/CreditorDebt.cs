using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class CreditorDebt : Auditable
{
    public long SupplierId { get; set; }
    public virtual Supplier? Supplier { get; set; }

    public long TransactionId { get; set; }
    public DateTimeOffset Date { get; set; } // Qarzdorlik sanasi
    public decimal RemainingAmount { get; set; } // Qoldiq qarz
    public string? Description { get; set; } // Izoh (ixtiyoriy)
    public bool IsSettled { get; set; } // Yangi xususiyat: Qarz to‘liq to‘langanmi?
    public long BranchId { get; set; }
    public List<CreditorDebtPayment> CreditorDebtPayments { get; set; } = new();
}