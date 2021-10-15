using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masonry_Models.ViewModels
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            ProductList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }
        public IList<Product> ProductList { get; set; }

    }
}
