using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Discount : Auditable
{
    public long SaleId { get; set; } // Qaysi sotuvga tegishli
    public Sale? Sale { get; set; } // Bog‘lanish uchun navigation property

    public decimal Amount { get; set; } // Chegirma summasi
}