using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Ratings = new HashSet<Rating>();
            WishLists = new HashSet<WishList>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string Ram { get; set; }
        public string Color { get; set; }
        public string Storage { get; set; }
        public string Details { get; set; }
        public int? CatId { get; set; }
        public string Series { get; set; }
        public int? PriceOld { get; set; }
        public int Price { get; set; }
        public int? Discount { get; set; }
        public string Thumb { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool BestSellers { get; set; }
        public bool HomeFlag { get; set; }
        public bool Active { get; set; }
        public int? UnitsInStock { get; set; }

        public virtual Category Cat { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
