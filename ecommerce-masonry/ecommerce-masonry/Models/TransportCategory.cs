using System.ComponentModel.DataAnnotations;

namespace ecommerce_masonry.Models
{
    public class TransportCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
