using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Masonry_Utility
{
    public static class WebConstance
    {
        public const string imagePath = @"\images\product\";
        public const string SessionCart = "ShoppingCartSession";
        public const string SessionInquiryId = "InquirySession";

        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public const string EmailAdmin = "pedro.p.sousa@sapo.pt";

        public const string CategoryName = "Category";
        public const string ApplicationTypeName = "ApplicationType";

        public const string Success = "Success";
        public const string Error = "Error";


        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "InProcess";
        public const string StatusShipped = "Shipped";
        public const string StatusCanceled = "Canceled";
        public const string StatusRefunded = "Refunded";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>(

            new List<string>
            {
                StatusPending, StatusApproved, StatusInProcess, StatusShipped, StatusCanceled, StatusRefunded
            });
    }
}
