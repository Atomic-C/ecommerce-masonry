using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using Masonry_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ecommerce_masonry.Controllers
{
    [Authorize(Roles = WebConstance.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _appType;

        public ApplicationTypeController(IApplicationTypeRepository appType) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _appType = appType;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objectList = _appType.GetAll();
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
                _appType.Add(obj);
                _appType.Save();
                TempData[WebConstance.Success] = "Application created successfully!";
                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
                TempData[WebConstance.Error] = "Error while deleting application!";
            return View(obj);
        }

        // GET FOR EDIT
        public IActionResult Edit(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }

            var obj = _appType.Find(id.GetValueOrDefault()); // We retrieve category from the database if valid.

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
                _appType.Update(obj); // So this Updates the database.
                _appType.Save(); // But this is what actually saves it?!?
                TempData[WebConstance.Success] = "Updated successfully!";
                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
                TempData[WebConstance.Error] = "Error while updating";
            return View(obj);
        }

        // GET FOR DELETE
        public IActionResult Delete(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }

            var obj = _appType.Find(id.GetValueOrDefault()); // We retrieve category from the database if valid.

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
            var obj = _appType.Find(id.GetValueOrDefault()); // We retrieve category from the database if valid.
            if (obj == null)
            {
                TempData[WebConstance.Success] = "Deleted successfully!";
                return NotFound();
            }
            _appType.Remove(obj); // So this Removes the database.
            _appType.Save(); // But this is what actually saves it?!?

                TempData[WebConstance.Success] = "Error while deleting!";
            return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
        }
    }
}
