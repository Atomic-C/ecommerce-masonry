using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models.ViewModels;
using Masonry_Utility;
using Masonry_Utility.BrainTree;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_masonry.Controllers
{
    public class OrderController : Controller
    {
        //private readonly ApplicationDbContext _db;

        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IOrderHeaderRepository _orderHeaderRepo;
        private readonly IBrainTreeGate _brain;

        [BindProperty] // We're binding this property so when we post we do not have to retrive, it's there already
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public OrderController(IOrderDetailRepository orderDetailRepo, IOrderHeaderRepository orderHeaderRepo, IBrainTreeGate brain)
        {
            //_db = db;

            _orderDetailRepo = orderDetailRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _brain = brain;
        }
        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhoneNumber = null, string Status = null)
        {
            OrderListViewModel orderListViewModel = new OrderListViewModel()
            {
                OrderHeaderList = _orderHeaderRepo.GetAll(),
            StatusList = WebConstance.listStatus.ToList().Select(i => new SelectListItem
            {
                Text = i,
                Value = i
            })
            };

            if (!string.IsNullOrEmpty(searchName)) // if it's not null & it's not empty...
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(u=>u.FullName.ToLower().Contains(searchName.ToLower()));
            }   
            
            if (!string.IsNullOrEmpty(searchEmail)) // if it's not null & it's not empty...
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(u=>u.Email.ToLower().Contains(searchEmail.ToLower()));
            } 
            
            if (!string.IsNullOrEmpty(searchPhoneNumber)) // if it's not null & it's not empty...
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(u=>u.PhoneNumber.ToLower().Contains(searchPhoneNumber.ToLower()));
            }     
            
            if (!string.IsNullOrEmpty(Status) && Status!= "--Order Status--") // if it's not null & it's not empty...
            {
                orderListViewModel.OrderHeaderList = orderListViewModel.OrderHeaderList.Where(u=>u.OrderStatus.ToLower().Contains(Status.ToLower()));
            }

            return View(orderListViewModel);
    }
        public IActionResult Details(int id)
        {
            OrderDetailsViewModel = new OrderDetailsViewModel
            {
                OrderHeader = _orderHeaderRepo.FirstOrDefault(u => u.Id == id),
                OrderDetail = _orderDetailRepo.GetAll(o => o.OrderHeaderId == id, includeProperties: "Product"),

            };
            return View(OrderDetailsViewModel);
        }
}
}
