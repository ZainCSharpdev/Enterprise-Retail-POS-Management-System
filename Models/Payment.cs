using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int SaleId { get; set; }

    public int? BillId { get; set; }

    public decimal Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? GatewayTransactionId { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
