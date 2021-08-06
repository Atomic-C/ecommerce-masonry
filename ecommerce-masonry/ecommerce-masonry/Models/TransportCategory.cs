using System.ComponentModel.DataAnnotations;

namespace ecommerce_masonry.Models
{
    public class TransportCategory
    {
        [Key]
        public int Id { get; set; } // This is a class property and represents a column
        public string Name { get; set; } // This is a class property and represents a column

    }
}
