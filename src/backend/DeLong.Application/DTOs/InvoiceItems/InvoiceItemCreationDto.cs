namespace DeLong.Application.DTOs.InvoiceItems;

public class InvoiceItemCreationDto
{
    public long InvoiceId { get; set; }
    public long ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
