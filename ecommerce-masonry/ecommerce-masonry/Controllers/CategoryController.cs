﻿using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ecommerce_masonry.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objectList = _db.Category;
            return View(objectList);
        }
    }
}