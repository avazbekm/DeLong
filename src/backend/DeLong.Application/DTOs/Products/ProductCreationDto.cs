using DeLong.Domain.Entities;

namespace DeLong.Application.DTOs.Products;

public class ProductCreationDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public decimal Price { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public bool IsActive { get; set; }
}
