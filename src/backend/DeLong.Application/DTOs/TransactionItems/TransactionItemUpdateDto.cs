﻿namespace DeLong.Service.DTOs.TransactionItems;

public class TransactionItemUpdateDto
{
    public long Id { get; set; }
    public long TransactionId { get; set; }
    public long ProductId { get; set; } // Mahsulot identifikatori (yangilash uchun kerak)
    public decimal? Quantity { get; set; }     // Yangilanishi mumkin bo‘lgan miqdor
    public string UnitOfMeasure { get; set; } = string.Empty; // Yangilanishi mumkin
    public decimal? PriceProduct { get; set; } // Yangilanishi mumkin bo‘lgan narx
}