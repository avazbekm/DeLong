﻿namespace DeLong.Service.DTOs.TransactionItems;

public class TransactionItemCreationDto
{
    public long TransactionId { get; set; }
    public long ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal PriceProduct { get; set; }
    public decimal SellingPrice { get; set; }

}