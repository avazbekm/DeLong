using DeLong.Domain.Entities;
using DeLong.Domain.Enums;

namespace DeLong.Application.DTOs.Invoices;

public class InvoiceResultDto
{
    public long Id { get; set; }
    public required Customer Customer { get; set; }

    public decimal TotalAmount { get; set; }
    public Status Status { get; set; } // Paid, Pending, Cancelled
}
