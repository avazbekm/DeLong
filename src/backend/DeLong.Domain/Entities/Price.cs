using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Price : Auditable
{
    public decimal ArrivalPrice { get; set; }  // Kelish narxi
    public decimal SellingPrice { get; set; }  // Sotish narxi
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public int Quantity { get; set; } // miqdori
    public long ProductId { get; set; }
    public Product Product { get; set; }
}
