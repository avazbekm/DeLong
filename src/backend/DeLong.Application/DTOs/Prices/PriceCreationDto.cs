namespace DeLong.Service.DTOs.Prices;

public class PriceCreationDto
{
    public long? SupplierId { get; set; }
    public long ProductId { get; set; }
    public decimal CostPrice { get; set; }  // Kelish narxi
    public decimal SellingPrice { get; set; }  // Sotish narxi
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public decimal Quantity { get; set; } // miqdori
    public long BranchId { get; set; }
}
