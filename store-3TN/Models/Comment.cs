using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Comment
    {
        public int CmtId { get; set; }
        public int? ProductId { get; set; }
        public int? CustomerId { get; set; }
        public string Comment1 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ReplyTo { get; set; }
    }
}
