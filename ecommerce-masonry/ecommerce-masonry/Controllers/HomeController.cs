using Masonry_Data_Access;
using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using Masonry_Models.ViewModels;
using Masonry_Utility;
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
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;


        public HomeController(ILogger<HomeController> logger, IProductRepository prodRepo, ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _productRepo = prodRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel() // Here we populate both properties for our HomeViewModel!!
            {
                Products = _productRepo.GetAll(includeProperties: "Category,ApplicationType"),
                //Products = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType),
                Categories = _categoryRepo.GetAll()
                //Categories = _db.Category
            };
            return View(homeViewModel); // If we don't assign homeViewModel to our view we get a System.NullReferenceException: 'Object reference not set to an instance of an object.'
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                // Here we know session exists, we want to retrive that session and add an item to it
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // If there's something, it gts retrived 
            }
            // The above logic was imported from DetailsPost to retrive session and check if product exists in session to set the boolean flag.


            DetailsViewModel DetailsViewModel = new DetailsViewModel()
            {
                Product = _productRepo.FirstOrDefault(u=>u.Id== id, includeProperties: "Category,ApplicationType"),
                IsInCart = false
            };

            foreach (var item in shoppingCartList) // For each item in shoppingCartList
            {
                if (item.ProductId == id) // Check if it is already in shoppingCartList
                {
                    DetailsViewModel.IsInCart = true; // If it is, set flag to true. Otherwise it's false by default, no need for else.
                }
            }

            return View(DetailsViewModel);
        }
        
        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsViewModel detailsViewModel)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) !=null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                // Here we know session exists, we want to retrive that session and add an item to it
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // If there's something, it gts retrived 
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id, Sqft = detailsViewModel.Product.TempSqft }); // If empty we directly add to it
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
            TempData[WebConstance.Success] = "Successfully added to cart!";
            return RedirectToAction(nameof(Index)); // To redirect back to home page we use redirect and nameof method, of an action method to avoid magic strings.
        }

        public IActionResult RemoveFromCart(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {
                // Here we know session exists, we want to retrive that session and add an item to it
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // If there's something, it gts retrived 
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id); // If there is any item in the shoppingCartList that matches this id we store it here

            if (itemToRemove != null) // We check if there is an item to be removed.
            {
                shoppingCartList.Remove(itemToRemove);
            }

            //shoppingCartList.Add(new ShoppingCart { ProductId = id }); // If empty we directly add to it
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList); // We set the shopping cart again with the new list which deoes not contain the product id that was selected.
            TempData[WebConstance.Success] = "Successfully removed from cart!";
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
