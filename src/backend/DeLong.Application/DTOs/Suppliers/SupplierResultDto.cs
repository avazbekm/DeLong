﻿namespace DeLong.Application.DTOs.Suppliers;

public class SupplierResultDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public long CreatedBy { get; set; }
}
