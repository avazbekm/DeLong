﻿using DeLong.Domain.Common;
using DeLong.Domain.Enums;

namespace DeLong.Domain.Entities;

public class User : Auditable
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Patronomyc { get; set; } = string.Empty;
    public string SeriaPasport { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }
    public DateTimeOffset DateOfIssue { get; set; } // pasport berilgan sana
    public DateTimeOffset DateOfExpiry { get; set; } // Amal qilish muddati
    public Gender Gender { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string TelegramPhone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string JSHSHIR { get; set; } = string.Empty;
    public Role Role { get; set; }
    public long BranchId { get; set; }
}
