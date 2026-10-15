using System;
using System.Collections.Generic;

namespace test.Models;

public partial class Promotion
{
    public int PromoId { get; set; }

    public string PromoCode { get; set; } = null!;

    public decimal Discount { get; set; }
}
