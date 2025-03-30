using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Asset : Auditable
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long BranchId { get; set; }
}
