using System.ComponentModel.DataAnnotations;

namespace Masonry_Models
{
    public class ApplicationType // Inside category are the columns we want! These are properties of this class.
    {
        [Key] // Once we add this annotation it tells entity framework that this column needs to be an id column and pkey for our table
        public int ID { get; set; }
        [Required]
        public string CategoryName { get; set; }

    }
}
