using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Masonry_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullUserName { get; set; }
        // We use NotMapped because we don't want to add this to the database, and only use these properties in our view!
        [NotMapped]
        public string   StreetAddress { get; set; }
        [NotMapped]
        public string   City { get; set; }
        [NotMapped]
        public string   State { get; set; }
        [NotMapped]
        public string   PostalCode { get; set; }
    }
}
