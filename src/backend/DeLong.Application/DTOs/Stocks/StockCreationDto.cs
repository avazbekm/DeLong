namespace DeLong.Application.DTOs.Stocks;

public class StockCreationDto
{
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; } // maxsulot soni
    public decimal MinStockLevel { get; set; }
}
