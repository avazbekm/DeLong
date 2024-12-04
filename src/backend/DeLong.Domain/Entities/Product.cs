using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Product : Auditable
{
    public string Name { get; set; } = string.Empty; 

    public long CategoryId { get; set; }
    public required Category Category { get; set; }

    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty; // kg,dona,karobka,litr
    public bool IsActive { get; set; }
}

