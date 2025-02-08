using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class KursDollar : Auditable
{
    public decimal SellingDollar { get; set; }
    public decimal AdmissionDollar { get; set; }
    public string TodayDate { get; set; } =string.Empty;
}
