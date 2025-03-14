﻿using DeLong.Domain.Common;

namespace DeLong.Domain.Entities;

public class Warehouse : Auditable
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public long EmployeeId { get; set; }
    public ICollection<CashRegister> CashRegisters { get; set; }
}