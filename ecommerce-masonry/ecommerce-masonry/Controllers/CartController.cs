using ecommerce_masonry.Data;   
using Masonry_Models;
using Masonry_Models.ViewModels;
using Masonry_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce_masonry.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }
        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {


            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {// Session exists and we can retrive all of the products
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).ToList();
                //shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // Convert to list as you retrive or as above, after we retribe is the same!!
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList(); // Here we find out all distinct products in cart using projections  .Select(i => i.ProductId)
            IEnumerable<Product> productList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            ProductUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productList.ToList()
            };

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel ProductUserViewModel)
        {

            var PathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "New Inquiry";
            string htmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(PathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }
            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products: { 3}

            StringBuilder productListSB = new StringBuilder();
            foreach (var item in ProductUserViewModel.ProductList)
            {
                productListSB.Append($" - Name: {item.Name} <span style='font-size:14px;'> (ID: {item.Id}) </span> <br />");
            }

            string messageBody = string.Format(htmlBody, ProductUserViewModel.ApplicationUser.FullUserName, ProductUserViewModel.ApplicationUser.Email, ProductUserViewModel.ApplicationUser.PhoneNumber, productListSB.ToString());

            await _emailSender.SendEmailAsync(WebConstance.EmailAdmin, subject, messageBody);

            return RedirectToAction(nameof(InquiryConfirmation));
        } 
        
        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();

            return View();
        }

    }
}
