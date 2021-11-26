using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models.ViewModels;
using Masonry_Utility.BrainTree;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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


        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public OrderController(IOrderDetailRepository orderDetailRepo, IOrderHeaderRepository orderHeaderRepo, IBrainTreeGate brain)
        {
            //_db = db;
            
            _orderDetailRepo = orderDetailRepo;
            _orderHeaderRepo = orderHeaderRepo;
            _brain = brain;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
