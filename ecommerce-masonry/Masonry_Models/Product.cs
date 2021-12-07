using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Masonry_Models
{
    public class Product
    {

        public Product()
        {
            TempSqft = 1;
        }

        [Key]
        public int Id { get; set; } // This will be the primary key
        [Required] // Required fields exist so EF core alters the database, it makes the column not nullable. Add migration 
        // Then push to database always.
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; } // Thisn column will be a mapping entity between category and product 
        [ForeignKey("CategoryId")] // EF core becomes aware of the above with the tag before this sentence.
        public virtual Category Category { get; set; } //EF core automatically adds a mapping between product and category!!
        // It will also create a category ID colum which will be the foreign key between both the tables.

        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }
        [NotMapped] // We use NotMapped because we don't want to add this to the database and only use this property in our view!
        [Range(1,10000, ErrorMessage ="Square feet must be at least 1.")]
        public int TempSqft { get; set; }
    }
}
