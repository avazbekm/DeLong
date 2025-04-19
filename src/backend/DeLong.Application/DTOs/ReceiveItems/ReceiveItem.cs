namespace DeLong.Service.DTOs.ReceiveItems;

public class ReceiveItemDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public long SupplierId { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsUpdate { get; set; }
    public long PriceId { get; set; }
}
