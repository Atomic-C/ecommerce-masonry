using Masonry_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Masonry_Utility;
using Masonry_Data_Access;
using Masonry_Data_Access.Repository.IRepository;

namespace ecommerce_masonry.Controllers
{
    public class InquiryController : Controller
    {
    private readonly IInquiryHeaderRepository _inquiryHeaderRepo;
    private readonly IInquiryDetailsRepository _inquiryDetailsRepo;

        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailsRepository inquiryDetailsRepository)
        {
            _inquiryHeaderRepo = inquiryHeaderRepository; // We add dependency injection for header and detail
            _inquiryDetailsRepo = inquiryDetailsRepository;
        }

        public IActionResult Index()
        {
            return View();
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
