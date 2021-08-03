﻿using ecommerce_masonry.Data;
using ecommerce_masonry.Models;
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
            return View(objList);
        }
    }
}
