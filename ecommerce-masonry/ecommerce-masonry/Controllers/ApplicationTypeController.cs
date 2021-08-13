using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ecommerce_masonry.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objectList = _db.ApplicationType;
            return View(objectList);
        }

        // GET FOR CREATE
        public IActionResult Create()
        {

            return View();
        }
        // POST FOR CREATE
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(obj);
                _db.SaveChanges();

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

            var obj = _db.ApplicationType.Find(id); // We retrieve category from the database if valid.

            if (obj == null) // Check this invalid condition, we don't want it. (If not found)
            {
                return NotFound();
            }
            return View(obj); // If we found the record, pass it to the view so we can display it!!!
        }

        // POST FOR EDIT
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj); // So this Updates the database.
                _db.SaveChanges(); // But this is what actually saves it?!?

                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
            return View(obj);
        }

        // GET FOR DELETE
        public IActionResult Delete(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }

            var obj = _db.ApplicationType.Find(id); // We retrieve category from the database if valid.

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
            var obj = _db.ApplicationType.Find(id); // We retrieve category from the database if valid.
            if (obj == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(obj); // So this Removes the database.
            _db.SaveChanges(); // But this is what actually saves it?!?

            return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
        }
    }
}
