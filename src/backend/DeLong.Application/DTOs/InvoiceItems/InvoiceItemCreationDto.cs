namespace DeLong.Application.DTOs.InvoiceItems;

public class InvoiceItemCreationDto
{
    public int InvoiceId { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
