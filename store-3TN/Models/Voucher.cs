using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Voucher
    {
        public int VoucherId { get; set; }
        public int VoucherType { get; set; }
        public string Desc { get; set; }
        public int? Amount { get; set; }
        public int? TotalMoneyRequire { get; set; }
        public int Value { get; set; }
        public bool IsActive { get; set; }
    }
}
