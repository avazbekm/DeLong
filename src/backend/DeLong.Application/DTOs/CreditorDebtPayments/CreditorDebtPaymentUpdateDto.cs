﻿using System.ComponentModel.DataAnnotations;

namespace DeLong.Service.DTOs.CreditorDebtPayments;

public class CreditorDebtPaymentUpdateDto
{
    public long Id { get; set; }
    public long? CreditorDebtId { get; set; }
    public decimal? Amount { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public string? Description { get; set; }
    public long? BranchId { get; set; }
}