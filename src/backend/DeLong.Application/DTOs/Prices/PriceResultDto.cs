using DeLong.Domain.Entities;

namespace DeLong.Service.DTOs.Prices;

public class PriceResultDto
{
    public long Id { get; set; }

    public long? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public long ProductId { get; set; }
    public Product Product { get; set; }

    public decimal CostPrice { get; set; }  // Kelish narxi
    public decimal SellingPrice { get; set; }  // Sotish narxi
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public decimal Quantity { get; set; } // miqdori
}
