using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Cash : Auditable
{
    public decimal UzsBalance { get; set; }  // so'm mablag' qoldiq
    public decimal UzpBalance { get; set; }  // plastik mablag' qoldiq
    public decimal UsdBalance { get; set; }  // dollar mablag' qoldiq
}