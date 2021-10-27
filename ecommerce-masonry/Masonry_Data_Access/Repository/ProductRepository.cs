using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            Product objFromDb = _db.Product.FirstOrDefault(u => u.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Price = obj.Price;
                objFromDb.ShortDesc = obj.ShortDesc;
                objFromDb.Description = obj.Description;
                objFromDb.Image = obj.Image;
                objFromDb.Category = obj.Category;
                objFromDb.ApplicationType = obj.ApplicationType;
            }
        }
    }
}
