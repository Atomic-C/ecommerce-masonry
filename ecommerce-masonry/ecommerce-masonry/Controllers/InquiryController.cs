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
                // Find a way to get rid of CAST
            };
            return View(InquiryViewModel);
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
