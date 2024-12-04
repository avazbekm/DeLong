using DeLong.Domain.Enums;

namespace DeLong.Application.DTOs.Invoices;

public class InvoiceCreationDto
{
    public long CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public Status Status { get; set; } // Paid, Pending, Cancelled
}
