namespace DeLong.Application.DTOs.Warehouses;

public class WarehouseResultDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ManagerName { get; set; } = string.Empty;
}
