using DeLong.Domain.Entities;

namespace DeLong.Application.DTOs.InvoiceItems;

public class InvoiceItemResultDto
{
    public long Id { get; set; }
    public required Invoice Invoice { get; set; }
    public required Product Product { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
