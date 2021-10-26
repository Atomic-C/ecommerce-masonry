using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db) :base(db) // Why do we use this base keyword??
        {
            _db = db;
        }

        public void Update(ApplicationType obj)
        {
            var objFromDb = _db.ApplicationType.FirstOrDefault(u => u.ID == obj.ID);

            if (objFromDb != null)
            {
                objFromDb.CategoryName = obj.CategoryName;
                // We don't need to save changes here because we already save them inside our generic repository
            }
        }
    }
}
