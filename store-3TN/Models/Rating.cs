using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Rating
    {
        public int RatingId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Rate { get; set; }
        public string CmtContent { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
