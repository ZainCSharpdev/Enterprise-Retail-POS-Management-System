using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Batch
{
    public int Batchid { get; set; }

    public long? Batchnumber { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
