using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Product : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string ProductSign { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public required Category Category { get; set; }
    public decimal? MinStockLevel { get; set; }  // minimal qoldiqni belgilab qo'yish
    public bool IsActive { get; set; }
    public long BranchId { get; set; }

    public ICollection<Price> Prices { get; set; } = new List<Price>();
}
