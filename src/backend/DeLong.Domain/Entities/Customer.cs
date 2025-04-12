using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Customer : Auditable
{
    public string CompanyName { get; set; } = string.Empty;
    public string ManagerName {  get; set; } = string.Empty;
    public string ManagerPhone { get; set; } = string.Empty;
    public string? MFO { get; set; } = string.Empty;
    public int? INN { get; set; }
    public string? BankAccount { get; set; } = string.Empty;
    public string? BankName { get; set; } = string.Empty;
    public string? OKONX { get; set; }
    public string? YurAddress { get; set; } = string.Empty;
    public string? EmployeeName { get; set; } = string.Empty;
    public string? EmployeePhone { get; set; } = string.Empty;
    public long BranchId { get; set; }
}
