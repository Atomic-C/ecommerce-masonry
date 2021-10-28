using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Models
{
    public class InquiryDetails
    {
        [Key]
        public int MyProperty { get; set; }

        [Required]
        public string InquiryHeaderId { get; set; } // We need a FK reference for our Inquiry Header
        [ForeignKey("InquiryHeaderId")]
        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        public string ProductId { get; set; } // We need a FK reference for our Inquiry Header
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
