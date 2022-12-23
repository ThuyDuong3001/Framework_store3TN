using System;
using System.Collections.Generic;

#nullable disable

namespace store_3TN.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; }
    }
}
