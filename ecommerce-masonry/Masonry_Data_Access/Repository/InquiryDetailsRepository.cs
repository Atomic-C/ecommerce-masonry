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
    public class InquiryDetailsRepository : Repository<InquiryDetails>, IInquiryDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(InquiryDetails obj)
        {
            _db.InquiryDetails.Update(obj);
        }


    }
}
