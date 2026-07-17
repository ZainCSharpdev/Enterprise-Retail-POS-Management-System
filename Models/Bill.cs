using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int? SaleId { get; set; }

    public string? BillImage { get; set; }

    public string? PdfUrl { get; set; }

    public DateTime? InsertedDate { get; set; }

    public string? Status { get; set; }

    public decimal? Amount { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public DateTime? BillDate { get; set; }

    public virtual Sale? Sale { get; set; }
}
