using System.ComponentModel.DataAnnotations;

namespace ecommerce_masonry.Models
{
    public class Category // This class represents our table. Model represents a table we want in our database.
    {
        [Key] // This data annotation tells entity framework that this column needs to be an identity column and a pkey for our table
        public int ID { get; set; } // This is a class property and represents a column
        public string CategoryName { get; set; } // This is a class property and represents a column
        public int DisplayOrder { get; set; } // This is a class property and represents a column

    }
}
