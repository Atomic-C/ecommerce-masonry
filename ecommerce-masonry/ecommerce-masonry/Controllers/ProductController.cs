using Masonry_Data_Access;
using Masonry_Data_Access.Repository.IRepository;
using Masonry_Models;
using Masonry_Models.ViewModels;
using Masonry_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ecommerce_masonry.Controllers
{
    [Authorize(Roles = WebConstance.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IWebHostEnvironment _webHostEnvironment; // ASP.NET CORE Built in dependency to use our web constance
        public ProductController(IProductRepository prodRepo, IWebHostEnvironment webHostEnvironment) // We populate the property above using dependency injection
        {
            // This object will have an instance of the dbcontext that dependency injection creates and passes to us through the constructor.
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment; // We get IWebHostEnvironment via dependency injection?!?
        }

        // More on DI: https://www.freecodecamp.org/news/a-quick-intro-to-dependency-injection-what-it-is-and-when-to-use-it-7578c84fa88f/

        public IActionResult Index()
        {   // With eager loading we have less database calls, as opposed to the below commented foreach block. 
            IEnumerable<Product> objectList = _prodRepo.GetAll(includeProperties:"Category,ApplicationType");


            return View(objectList);
        }

        // GET FOR UPSERT
        public IActionResult Upsert(int? id) // Here we display empty box to enter name and display order for new category to create
        {

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _prodRepo.GetAllDropDownList(WebConstance.CategoryName),
                ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WebConstance.ApplicationTypeName)
            };

            if (id == null) // If it's null, it's for CREATE :D
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _prodRepo.Find(id.GetValueOrDefault()); // This means id is not null and we need to retrieve product from database and pass it to the view just like we did with editing of category.
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
        public IActionResult Upsert(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var files = HttpContext.Request.Form.Files; // Here we retrive our uploaded image
                string webRootPath = _webHostEnvironment.WebRootPath; // This is our path to our www root folder. I don't get it. Don't we have a WebConstance class for this?

                if (productViewModel.Product.Id == 0) // To find out if we're calling function for Create or Update
                {
                    // Create
                    string upload = webRootPath + WebConstance.imagePath; // We get our path to the folder where we save Image
                    string filename = Guid.NewGuid().ToString(); // This creates random filename
                    string extension = Path.GetExtension(files[0].FileName); // We get extension of uploaded file

                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream); // We copy file to new location
                    }

                    productViewModel.Product.Image = filename + extension; // Here we're storing the new guid name for file and extension. Not the path!!
                    productViewModel.Product.ProdCounter = productViewModel.Product.ProdCounter + 1;
                    _prodRepo.Add(productViewModel.Product); // Here we add the product
                    TempData[WebConstance.Success] = "Successfully created!";

                }
                else
                {
                    // Update 
                    // Since we're retriving this only to get the name of the old image, so we don't need to track it
                    var objFromDb = _prodRepo.FirstOrDefault(u => u.Id == productViewModel.Product.Id, isTracking:false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstance.imagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image); // We store old file here

                        if (System.IO.File.Exists(oldFile)) // We check if file exists
                        {
                            System.IO.File.Delete(oldFile); // We delete if the file exists
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream); // This moves in the new image inside the folder!!
                        }
                        productViewModel.Product.Image = filename + extension; // The image is saved here. file and extension.
                    }
                    else // In case we updated anything else but image
                    {
                        productViewModel.Product.Image = objFromDb.Image; // Image was not updated and we keep it as is
                    }
                    _prodRepo.Update(productViewModel.Product);
                }
                _prodRepo.Save(); // This is what actually saves it after we update.
                TempData[WebConstance.Success] = "Successfully updated!"; 
                return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
            }
            productViewModel.CategorySelectList = _prodRepo.GetAllDropDownList(WebConstance.CategoryName);
            productViewModel.ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WebConstance.ApplicationTypeName);
            return View(productViewModel); // We need the view model that was originally passed above.
        }


        // GET FOR DELETE
        public IActionResult Delete(int? id) // Receive id from asp-route on View Category Index
        {
            if (id == null || id == 0) // Check these invalid conditions, we don't want them
            {
                return NotFound();
            }
            // Below is eager loading concept.
            //This is a great way to save resources because only one query gets executed
            Product product = _prodRepo.FirstOrDefault(u=>u.Id==id,includeProperties:"Category,ApplicationType");
            // product.Category = _db.Category.Find(product.CategoryId);

            var obj = _prodRepo.Find(id.GetValueOrDefault()); // We retrieve category from the database if valid.

            if (product == null) // Check this invalid condition, we don't want it. (If not found)
            {
                return NotFound();
            }
            return View(product); // If we found the record, pass it to the view so we can display it!!!
        }

        // POST FOR DELETE
        [HttpPost, ActionName("Delete")] // We define this as a post action method, with this attribute
        [ValidateAntiForgeryToken] // This is for validation purposes - built in mechanic
        public IActionResult DeletePost(int? id)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            var obj = _prodRepo.Find(id.GetValueOrDefault()); // We retrieve category from the database if valid.
            if (obj == null)
            {
                return NotFound();
            }

            string upload = webRootPath + WebConstance.imagePath;


            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _prodRepo.Remove(obj); // So this Removes the database.
            _prodRepo.Save(); // But this is what actually saves it?!?
            TempData[WebConstance.Success] = "Successfully deleted!";
            return RedirectToAction("Index"); // We're in the same controller we don't need to define controller name here
        }
    }
}
