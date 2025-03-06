using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Debt : Auditable
{
    public long SaleId { get; set; }
    public Sale? Sale { get; set; }

    public decimal RemainingAmount { get; set; } // Hali to‘lanmagan qarz miqdori
    public DateTime DueDate { get; set; } // To‘lash muddati 🕒
    public bool IsSettled { get; set; } // Yangi xususiyat: Qarz to‘liq to‘langanmi?
    public List<DebtPayment> DebtPayments { get; set; } = new();
}