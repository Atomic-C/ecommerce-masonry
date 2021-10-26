using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            var objFromDb = _db.Category.FirstOrDefault(u => u.ID == obj.ID);
                if (objFromDb != null)
                {
                 objFromDb.CategoryName = obj.CategoryName;
                 objFromDb.DisplayOrder = obj.DisplayOrder;
                 // We don't need to save changes here because we already save them inside our generic repository
                }
        }
    }
}
