using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public DateTime SaleDate { get; set; }

    public string CustomerNumber { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal? Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal NetAmount { get; set; }

    public string Status { get; set; } = null!;

    public string Method { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
