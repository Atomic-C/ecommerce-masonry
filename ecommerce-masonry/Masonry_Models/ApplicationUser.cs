using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masonry_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullUserName { get; set; }
    }
}
