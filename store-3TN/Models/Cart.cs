using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Cart
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
        public int TotalMoney => Product.Price * Amount;
    }
}
