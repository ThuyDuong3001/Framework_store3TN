using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace store_3TN.Models
{
    public partial class Location
    {
        public Province Province { get; set; }
        public District District { get; set; }
        public Ward Ward { get; set; }
    }
}