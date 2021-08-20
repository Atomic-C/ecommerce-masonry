using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ecommerce_masonry.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _db = db;
        }

        // More on DI: https://www.freecodecamp.org/news/a-quick-intro-to-dependency-injection-what-it-is-and-when-to-use-it-7578c84fa88f/

        public IActionResult Index()
        {
            IEnumerable<Product> objectList = _db.Product; // Retrieve all categories from database and store on objectList

            foreach (var obj in objectList) // This iterates through all of the products that we have in the objectList
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.ID == obj.CategoryId);
            } // each object will load the Category model based on the condition above.

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

        // GET FOR EDIT
        public IActionResult Edit(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }

            var obj = _db.Category.Find(id); // We retrieve category from the database if valid.

            if (obj == null) // Check this invalid condition, we don't want it. (If not found)
            {
                return NotFound();
            }
            return View(obj); // If we found the record, pass it to the view so we can display it!!!
        }

        // POST FOR EDIT
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(obj); // So this Updates the database.
                _db.SaveChanges(); // But this is what actually saves it?!?

                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
            return View(obj);
        }
        /*
        TODO: GOOGLE
        SqlException: Cannot insert explicit value for identity column in table 'Category' when IDENTITY_INSERT is set to OFF.

        https://appuals.com/how-to-fix-the-error-cannot-insert-explicit-value-for-identity-column-in-table-when-identity_insert-is-set-to-off/

            https://makolyte.com/sqlexception-cannot-insert-explicit-value-for-identity-column/

        I ended up messing the database a little and had to revert it:
        https://stackoverflow.com/questions/38192450/how-to-unapply-a-migration-in-asp-net-core-with-ef-core
        */

        // GET FOR DELETE
        public IActionResult Delete(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }

            var obj = _db.Category.Find(id); // We retrieve category from the database if valid.

            if (obj == null) // Check this invalid condition, we don't want it. (If not found)
            {
                return NotFound();
            }
            return View(obj); // If we found the record, pass it to the view so we can display it!!!
        }

        // POST FOR DELETE
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id); // We retrieve category from the database if valid.
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj); // So this Removes the database.
            _db.SaveChanges(); // But this is what actually saves it?!?

            return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
        }
    }
}
