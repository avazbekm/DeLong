﻿using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class InvoiceItem : Auditable
{
    public long InvoiceId { get; set; }
    public required Invoice Invoice { get; set; }

    public long ProductId { get; set; }
    public required Product Product { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
