﻿using DeLong.Domain.Entities;

namespace DeLong.Service.DTOs.Prices;

public class PriceResultDto
{
    public long ProductId { get; set; }
    public decimal ArrivalPrice { get; set; }  // Kelish narxi
    public decimal SellingPrice { get; set; }  // Sotish narxi
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public int Quantity { get; set; } // miqdori
    public Product Product { get; set; }
}
