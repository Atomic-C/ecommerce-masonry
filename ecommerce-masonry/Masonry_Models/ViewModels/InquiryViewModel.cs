using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masonry_Models.ViewModels
{
    public class InquiryViewModel
    {
        public InquiryHeader InquiryHeader { get; set; }
        public IEnumerable<InquiryDetails> InquiryDetails { get; set; }
    }
}
