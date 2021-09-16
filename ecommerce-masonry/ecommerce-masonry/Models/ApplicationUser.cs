using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullUserName { get; set; }
    }
}
