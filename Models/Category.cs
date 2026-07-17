using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Category
{
    public int Categoryid { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
