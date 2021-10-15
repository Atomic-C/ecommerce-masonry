using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Masonry_Models
{
    public class Category // This class represents our table. Model represents a table we want in our database.
    {
        [Key] // This data annotation tells entity framework that this column needs to be an identity column and a pkey for our table
        public int ID { get; set; } // This is a class property and represents a column
        [Required]
        public string CategoryName { get; set; } // This is a class property and represents a column
        [DisplayName("Display Order")] // This change of the name is possible because we've used tag helpers in Category->Create
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display order for category must be greater than 0")]
        public int DisplayOrder { get; set; } // This is a class property and represents a column

    }
}
