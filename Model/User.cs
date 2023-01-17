using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace winetranet_application.Model
{
    public partial class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneMobile { get; set; }
        public int? Service { get; set; }
        public int? Site { get; set; }

   

        public virtual Service ServiceNavigation { get; set; }
        public virtual Site SiteNavigation { get; set; }

        
    }
}
