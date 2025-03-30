using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Branch : Auditable
{
    public string BranchName { get; set; } = string.Empty;
    public string Location {  get; set; } = string.Empty;
}