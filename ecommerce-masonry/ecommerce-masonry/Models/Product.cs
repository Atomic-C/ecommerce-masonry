﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_masonry.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; } // This will be the primary key
        [Required] // Required fields exist so EF core alters the database, it makes the column not nullable. Add migration 
        // Then push to database always.
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0.1, double.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; } // Thisn column will be a mapping entity between category and product 
        [ForeignKey("CategoryId")] // EF core becomes aware of the above with the tag before this sentence.
        public virtual Category Category { get; set; } //EF core automatically adds a mapping between product and category!!
        // It will also create a category ID colum which will be the foreign key between both the tables.
    }
}