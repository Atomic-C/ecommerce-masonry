using ecommerce_masonry.Data;   
using ecommerce_masonry.Models;
using ecommerce_masonry.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) !=null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {// Session exists and we can retrive all of the products
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).ToList();
                //shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // Convert to list as you retrive or as above, after we retribe is the same!!
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList(); // Here we find out all distinct products in cart using projections  .Select(i => i.ProductId)
            IEnumerable<Product> productList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {// Session exists and we can retrive all of the products
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).ToList();
                //shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // Convert to list as you retrive or as above, after we retribe is the same!!
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u=>u.ProductId == id)); // First or default to retrive object based on ID

            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

    }
}
