using Masonry_Models;
using Masonry_Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate(); // This applies pending migrations to the database!!
                }
            }
            catch (Exception)
            {

            }
            //if (!await _roleManager.RoleExistsAsync(WebConstance.AdminRole))
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(WebConstance.AdminRole));
            //    await _roleManager.CreateAsync(new IdentityRole(WebConstance.CustomerRole));
            //} // This is the old code

            if (!_roleManager.RoleExistsAsync(WebConstance.AdminRole).GetAwaiter().GetResult()) // This is what we do when we have an async await method
            {
                _roleManager.CreateAsync(new IdentityRole(WebConstance.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebConstance.CustomerRole)).GetAwaiter().GetResult(); 
                // This makes sure that when the line executed it waits until the result is fetched.
            }
            else
            {
                return; // This happens if roles already exist: Nothing.
            }
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "apagardps@gmail.com",
                Email = "apagardps@gmail.com",
                EmailConfirmed = true,
                FullUserName = "DeDacto Admin",
                PhoneNumber = "910320910",

            }, "Admin4us!").GetAwaiter().GetResult(); // I used GetAwaiter and GetResult to make sure this is executed. Shouldn't I?

            ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u => u.Email == "apagardps@gmail.com");
            _userManager.AddToRoleAsync(user,WebConstance.AdminRole).GetAwaiter().GetResult();
        }
    }
}
