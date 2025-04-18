﻿namespace DeLong.Application.DTOs.Categories;

public class CategoryResultDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}