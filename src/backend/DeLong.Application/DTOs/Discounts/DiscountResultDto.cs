﻿
public class DiscountResultDto
{
    public long Id { get; set; } // Chegirma ID’si
    public long SaleId { get; set; } // Qaysi sotuvga tegishli
    public decimal Amount { get; set; } // Chegirma summasi
    public long CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; } // Yaratilgan vaqt
}