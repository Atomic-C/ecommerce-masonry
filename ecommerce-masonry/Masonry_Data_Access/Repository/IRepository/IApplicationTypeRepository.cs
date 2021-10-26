using Masonry_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Data_Access.Repository.IRepository
{
    public interface IApplicationTypeRepository :IRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}
