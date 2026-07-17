using System;
using System.Collections.Generic;

namespace POSbackend.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public long? CustomerNumber { get; set; }
}
