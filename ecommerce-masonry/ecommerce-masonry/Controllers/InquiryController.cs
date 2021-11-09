using Masonry_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Masonry_Utility;
using Masonry_Data_Access;
using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models.ViewModels;

namespace ecommerce_masonry.Controllers
{
    public class InquiryController : Controller
    {
    private readonly IInquiryHeaderRepository _inquiryHeaderRepo;
    private readonly IInquiryDetailsRepository _inquiryDetailsRepo;

    [BindProperty]
    public InquiryViewModel InquiryViewModel { get; set; }

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailsRepository inquiryDetailsRepository)
        {
            _inquiryHeaderRepo = inquiryHeaderRepository; // We add dependency injection for header and detail
            _inquiryDetailsRepo = inquiryDetailsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }        
        public IActionResult Details(int id)
        {
            InquiryViewModel = new InquiryViewModel()
            {
                InquiryHeader = _inquiryHeaderRepo.FirstOrDefault(u => u.Id == id),
                InquiryDetails = _inquiryDetailsRepo.GetAll(u => u.InquiryHeaderId == id,includeProperties: "Product")
            };
            return View(InquiryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            InquiryViewModel.InquiryDetails = _inquiryDetailsRepo.GetAll(u => u.InquiryHeaderId == InquiryViewModel.InquiryHeader.Id);

            foreach (var item in InquiryViewModel.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = item.ProductId
                };
                shoppingCartList.Add(shoppingCart);
            }
                HttpContext.Session.Clear();
                HttpContext.Session.Set(WebConstance.SessionCart, shoppingCartList);
                HttpContext.Session.Set(WebConstance.SessionInquiryId, InquiryViewModel.InquiryHeader.Id);
                // If above line is zero session was set normally in homepage. If not, it was set using inquiry button.
                return RedirectToAction("Index", "Cart"); // Redirect to Index action inside cart controller

        }

            [HttpPost]
            public IActionResult Delete()
            {
                InquiryHeader inquiryHeader = _inquiryHeaderRepo.FirstOrDefault(u => u.Id == InquiryViewModel.InquiryHeader.Id);
                IEnumerable<InquiryDetails> inquiryDetails = _inquiryDetailsRepo.GetAll(u => u.InquiryHeaderId == InquiryViewModel.InquiryHeader.Id);

            _inquiryHeaderRepo.Remove(inquiryHeader);
            _inquiryDetailsRepo.RemoveRange(inquiryDetails); // I had to add a RemoveRange on IRepository for this...
            _inquiryDetailsRepo.Save();
            _inquiryHeaderRepo.Save();

            return RedirectToAction(nameof(Index)); ;
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inquiryHeaderRepo.GetAll() });
        }
        #endregion
    }
}
