using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public static class GlobalConstantsClass
    {
        public const string PageNotFoundKey = "404";
        public const string BadRequestKey = "Bad Request Error";
        public const string UnAuthorizedKey = "401";

        public const string PageNotFoundError = "Page you are looking for is either not available or removed.";
        public const string UnAuthorizedError = "Unauthorized: You are not authorized to view the content of this page.";
    } // GlobalConstantsClass...
}
