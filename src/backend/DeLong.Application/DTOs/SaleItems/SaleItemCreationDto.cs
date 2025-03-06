namespace DeLong.Service.DTOs.SaleItems;

public class SaleItemCreationDto
{
    public long SaleId { get; set; }
    public long ProductId { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}