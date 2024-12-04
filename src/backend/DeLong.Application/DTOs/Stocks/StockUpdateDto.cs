namespace DeLong.Application.DTOs.Stocks;

public class StockUpdateDto
{
    public long Id { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; } // maxsulot soni
    public decimal MinStockLevel { get; set; }
}
