using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using ecommerce_masonry.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET FOR UPSERT
        public IActionResult Upsert(int? id) // Here we display empty box to enter name and display order for new category to create
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.CategoryName,
            //    Value = id.ToString()
            //}); // Here we are retriving all categories from database and we convert them to selectlistitem so we can have them in an enumerable object and then display them in a dropdown


            ////Then we pass this category dropdown to the view, sow e can display it
            //ViewBag.CategoryDropDown = CategoryDropDown; // Controller ---> view passing

            //Product product = new Product();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = id.ToString()
                })
            };

            if (id == null) // If it's null, it's for CREATE :D
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _db.Product.Find(id); // This means id is not null and we need to retrieve product from database and pass it to the view just like we did with editing of category.
                if (productViewModel == null)
                {
                    return NotFound();
                }
                return View(productViewModel); // If it does find the product, returns back to the view with product!!!
            }
        }

        // POST FOR UPSERT
        [HttpPost] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj); // So this adds to the database.
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
