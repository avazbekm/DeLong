using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Product : Auditable
{
    public string Name { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public required Category Category { get; set; }

    public bool IsActive { get; set; }
  
    public ICollection<Price> Prices { get; set; } = new List<Price>();
}
