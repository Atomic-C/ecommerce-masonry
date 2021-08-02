using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Controllers
{
    public class TransportCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
