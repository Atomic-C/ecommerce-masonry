using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using Masonry_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository
{
    public class OrderHeaderRepository : Repository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(InquiryHeader obj)
        {
            _db.InquiryHeaders.Update(obj);
        }


    }
}
