using DeLong.Domain.Enums;

namespace DeLong.Application.DTOs.Invoices;

public class InvoiceUpdateDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public Status Status { get; set; } // Paid, Pending, Cancelled
}
