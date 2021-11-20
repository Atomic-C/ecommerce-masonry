using Masonry_Data_Access;
using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using Masonry_Models.ViewModels;
using Masonry_Utility;
using Masonry_Utility.BrainTree;
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
        //private readonly ApplicationDbContext _db;
        private readonly IProductRepository _prodRepo;
        private readonly IApplicationUserRepository _applicationUserRepo;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepo;
        private readonly IInquiryDetailsRepository _inquiryDetailsRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IBrainTreeGate _brain;


        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserViewModel ProductUserViewModel { get; set; }
        public CartController(IProductRepository productRepo, IApplicationUserRepository applicationUserRepo, IInquiryHeaderRepository inquiryHeaderRepo, IInquiryDetailsRepository inquiryDetailsRepo, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender, IOrderDetailRepository orderDetailRepo, IOrderHeaderRepository orderHeaderRepo, IBrainTreeGate brain)
        {
            //_db = db;
            _prodRepo = productRepo;
            _applicationUserRepo = applicationUserRepo;
            _inquiryHeaderRepo = inquiryHeaderRepo;
            _inquiryDetailsRepo = inquiryDetailsRepo;

            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _orderDetailRepo = orderDetailRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _brain = brain;
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
            IEnumerable<Product> productListTemp = _prodRepo.GetAll(u => prodInCart.Contains(u.Id));
            IList<Product> productList = new List<Product>();
            //IEnumerable<Product> productList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            foreach (var item in shoppingCartList)
            {
                Product prodTemp = productListTemp.FirstOrDefault(u => u.Id == item.ProductId);
                prodTemp.TempSqft = item.Sqft;
                productList.Add(prodTemp);
            }

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
            TempData[WebConstance.Success] = "Successfully removed from cart!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> ProdList)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            foreach (Product item in ProdList)
            {
                shoppingCartList.Add(new ShoppingCart { ProductId = item.Id, Sqft = item.TempSqft });
            }
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Summary));
        }
        
        public IActionResult Summary()
        {
            ApplicationUser applicationUser;

            if (User.IsInRole(WebConstance.AdminRole))
            {
                if (HttpContext.Session.Get<int>(WebConstance.SessionInquiryId) != 0)
                {
                    //cart has been loaded using an imnquiry
                    InquiryHeader inquiryHeader = _inquiryHeaderRepo.FirstOrDefault(u => u.Id == HttpContext.Session.Get<int>(WebConstance.SessionInquiryId)); // retrive inquiry from database

                    applicationUser = new ApplicationUser()
                    {
                        Email = inquiryHeader.Email,
                        FullUserName = inquiryHeader.FullName,
                        PhoneNumber = inquiryHeader.PhoneNumber

                    };
                }
                else
                {
                    applicationUser = new ApplicationUser(); 
                    // This means user is admin user and is placing order for a costumert hat walked in store 
                }
                // Here we get client token.
                // Only if the user is admin user.
                var gateway = _brain.GetGateWay(); // we call getgateway because if it does not exist it is created remember?
                var clientToken = gateway.ClientToken.Generate(); // This is provided by braintree team, and gets us the client token.
                ViewBag.ClientToken = clientToken; // I don't like viewbag, but we only need it in this page. Usually I use VewModels.
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //var userId = User.FindFirstValue(ClaimTypes.Name);
                applicationUser = _applicationUserRepo.FirstOrDefault(u=>u.Id == claim.Value);
            }



            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).Count() > 0)
            {// Session exists and we can retrive all of the products
                shoppingCartList = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstance.SessionCart).ToList();
                //shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstance.SessionCart); // Convert to list as you retrive or as above, after we retribe is the same!!
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList(); // Here we find out all distinct products in cart using projections  .Select(i => i.ProductId)
            IEnumerable<Product> productList = _prodRepo.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = applicationUser,
                //ProductList = productList.ToList()
            };

            //    We do not have sqft inside productList, but we do inside shoppingCartList
            //    So foreach item in it,  we retrive sqft
            foreach (var item in shoppingCartList)
            {
                Product prodTemp = _prodRepo.FirstOrDefault(u => u.Id == item.ProductId); // We retrive product
                prodTemp.TempSqft = item.Sqft; // we set product sqft here
                ProductUserViewModel.ProductList.Add(prodTemp); // We add the product to the list here.

            }

            return View(ProductUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel ProductUserViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // Below we separare the logic inside the SummaryPost, for both Order and Inquiry cases, for admins and users respectively
            if (User.IsInRole(WebConstance.AdminRole))
            { // This means we need to create an order
                double orderTotal = 0.0;

                foreach (var item in ProductUserViewModel.ProductList)
                {
                    orderTotal = item.Price * item.TempSqft; // we calculate total price here
                }
                OrderHeader orderHeader = new OrderHeader()
                {
                    CreatedByUserId = claim.Value,
                    FinalOrderTotal = orderTotal,
                    City = ProductUserViewModel.ApplicationUser.City,
                    StreetAddress = ProductUserViewModel.ApplicationUser.StreetAddress,
                    State = ProductUserViewModel.ApplicationUser.State,
                    PostalCode = ProductUserViewModel.ApplicationUser.PostalCode,
                    FullName = ProductUserViewModel.ApplicationUser.FullUserName,
                    Email = ProductUserViewModel.ApplicationUser.Email,
                    PhoneNumber = ProductUserViewModel.ApplicationUser.PhoneNumber,
                    OrderDate = DateTime.Now,
                    OrderStatus = WebConstance.StatusPending

                };
                _orderHeaderRepo.Add(orderHeader);
                _orderHeaderRepo.Save();

                foreach (var item in ProductUserViewModel.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderHeaderId = orderHeader.Id,
                        PricePerSqFt = item.Price,
                        Sqft = item.TempSqft,
                        ProductId = item.Id
                    };
                    _orderDetailRepo.Add(orderDetail);
                }
                _orderDetailRepo.Save();
                return RedirectToAction(nameof(InquiryConfirmation), new { id=orderHeader.Id });
            }
            else
            { // We need to create an Inquiry

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

            InquiryHeader inquiryHeader = new InquiryHeader() // check ;
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserViewModel.ApplicationUser.FullUserName,
                PhoneNumber = ProductUserViewModel.ApplicationUser.PhoneNumber,
                Email = ProductUserViewModel.ApplicationUser.Email,
                InquiryDate = DateTime.Now
                
            };

            _inquiryHeaderRepo.Add(inquiryHeader);
            _inquiryHeaderRepo.Save();

            foreach (var item in ProductUserViewModel.ProductList)
            {
                InquiryDetails inquiryDetail = new InquiryDetails()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = item.Id
                };
                _inquiryDetailsRepo.Add(inquiryDetail);
            }
                _inquiryDetailsRepo.Save();
            TempData[WebConstance.Success] = "Successful inquiry!";

            }

            return RedirectToAction(nameof(InquiryConfirmation));
        } 
        
        public IActionResult InquiryConfirmation(int id = 0)
        {
            OrderHeader orderHeader = _orderHeaderRepo.FirstOrDefault(u=>u.Id == id); // We retrive orderheader id from the repo.
            HttpContext.Session.Clear();

            return View(orderHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(IEnumerable<Product> ProdList)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            foreach (Product item in ProdList)
            {
                shoppingCartList.Add(new ShoppingCart { ProductId=item.Id,Sqft=item.TempSqft });
            }
            HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
