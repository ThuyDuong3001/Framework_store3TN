using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Scontents { get; set; }
        public string Contents { get; set; }
        public string Thumb { get; set; }
        public bool Published { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Author { get; set; }
        public bool IsHot { get; set; }
        public int? Views { get; set; }
    }
}
