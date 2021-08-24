using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ecommerce_masonry.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
