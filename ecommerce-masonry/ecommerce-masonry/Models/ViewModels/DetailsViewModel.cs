using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new Product(); // This way we do not have to do this inside the consuming controller and when we get a DetailsViewModel it will automatically initialize product to a new product
        }
        public Product Product { get; set; }
        public bool IsInCart { get; set; }
    }
}
