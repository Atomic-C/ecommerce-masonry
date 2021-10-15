using System.ComponentModel.DataAnnotations;

namespace Masonry_Models
{
    public class TransportCategory
    {
        [Key]
        public int Id { get; set; } // This is a class property and represents a column
        [Required]
        public string Name { get; set; } // This is a class property and represents a column

    }
}
