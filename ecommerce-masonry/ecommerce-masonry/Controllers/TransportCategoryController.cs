using Masonry_Data_Access;
using Masonry_Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ecommerce_masonry.Controllers
{
    public class TransportCategoryController : Controller

    {
        private readonly ApplicationDbContext _db; // Here we save an instance of Dbontext that was passed on constructor after dependency injection created it 

        public TransportCategoryController(ApplicationDbContext db) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<TransportCategory> objList = _db.TransportCategory;

            //System.Diagnostics.Debug.WriteLine("Output is: " + objList); // Use this for debug purposes <3
            // Source: https://www.youtube.com/watch?v=IpGNe3jLxQo&t=84s

            return View(objList);
        }

        // GET FOR CREATE
        public IActionResult Create()
        {
            return View(); // Here we display empty box to enter name for new transport category to create
        }

        // POST FOR CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransportCategory obj)
        {
            if (ModelState.IsValid)
            {
                _db.TransportCategory.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index"); // RedirectToAction redirects us to the index in this controler, so no need to define the name of the controller here.
            }
            return View(obj);
        }
    }
}
