using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) :base(db) // Why do we use this base keyword??
        {
            _db = db;
        }


    }
}
