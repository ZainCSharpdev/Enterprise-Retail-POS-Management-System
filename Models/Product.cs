using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public int? Categoryid { get; set; }

    public string ProductName { get; set; } = null!;

    public string ManufacturerName { get; set; } = null!;

    public int TotalQuantity { get; set; }

    public decimal Price { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public decimal UnitPrice { get; set; }

    public string? SetSize { get; set; }

    public int? Batchid { get; set; }

    public virtual Batch? Batch { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
