using DeLong.Domain.Entities;

namespace DeLong.Application.DTOs.Stocks;

public class StockResultDto
{
    public long Id { get; set; }
    public Warehouse Warehouse { get; set; }
    public Product Product { get; set; }
    public decimal Quantity { get; set; } // maxsulot soni
    public decimal MinStockLevel { get; set; }
}
