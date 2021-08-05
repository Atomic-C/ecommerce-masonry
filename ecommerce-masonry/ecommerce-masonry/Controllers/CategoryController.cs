using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ecommerce_masonry.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _db = db;
        }

        // More on DI: https://www.freecodecamp.org/news/a-quick-intro-to-dependency-injection-what-it-is-and-when-to-use-it-7578c84fa88f/

        public IActionResult Index()
        {
            IEnumerable<Category> objectList = _db.Category; // Retrieve all categories from database and store on objectList
            return View(objectList);
        }

        // GET FOR CREATE
        public IActionResult Create() // Here we display empty box to enter name and display order for new category to create
        {
            return View();
        }

        // POST FOR CREATE
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj); // So this adds to the database.
                _db.SaveChanges(); // But this is what actually saves it?!?

                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
            return View(obj);
        }
    }
}
