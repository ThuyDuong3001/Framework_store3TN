using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Thumb { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
