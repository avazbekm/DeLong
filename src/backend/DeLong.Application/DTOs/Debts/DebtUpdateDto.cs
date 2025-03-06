namespace DeLong.Service.DTOs.Debts;

public class DebtUpdateDto
{
    public long Id { get; set; }  // Qarzning ID’si
    public long SaleId { get; set; }
    public decimal RemainingAmount { get; set; } // To'lanmagan qarz miqdori
    public bool IsSettled { get; set; } // Yangi xususiyat: Qarz to‘liq to‘langanmi?
    public DateTimeOffset DueDate { get; set; } // To‘lash muddati 🕒
}

