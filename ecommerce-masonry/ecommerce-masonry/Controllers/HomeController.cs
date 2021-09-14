using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using ecommerce_masonry.Models.ViewModels;
using ecommerce_masonry.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel() // Here we populate both properties for our HomeViewModel!!
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                Categories = _db.Category
            };
            return View(homeViewModel); // If we don't assign homeViewModel to our view we get a System.NullReferenceException: 'Object reference not set to an instance of an object.'
        }

        public IActionResult Details(int id)
        {
            DetailsViewModel DetailsViewModel = new DetailsViewModel()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).Where(u => u.Id == id).FirstOrDefault(),
                IsInCart = false
            };
            return View(DetailsViewModel);
        }
        
        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) !=null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                // Here we know session exists, we want to retrive that session and add an item to it
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // If there's something, it gts retrived 
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id }); // If empty we directly add to it
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index)); // To redirect back to home page we use redirect and nameof method, of an action method to avoid magic strings.
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
