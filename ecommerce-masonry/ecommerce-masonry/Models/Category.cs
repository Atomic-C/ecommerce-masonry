using System.ComponentModel.DataAnnotations;

namespace ecommerce_masonry.Models
{
    public class Category // This class IS the table. This is the model that represents the table we want in our database.
    {
        [Key] // This data anotation is used to tell entity framework that this column is an identity column and a pkey.
        public int Id { get; set; } // This property is a table column
        public int Name { get; set; } // This property is a table column
        public int DisplayOrder { get; set; } // This property is a table column
    }
}
