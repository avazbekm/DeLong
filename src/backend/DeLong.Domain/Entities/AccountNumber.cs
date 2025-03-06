using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class AccountNumber : Auditable
{
    public long? UserId { get; set; }
    public long? CustomerId { get; set; }
    public decimal UzsBalance {  get; set; }  // so'm mablag' qoldiq
    public decimal UsdBalance {  get; set; }  // dollar mablag' qoldiq
}