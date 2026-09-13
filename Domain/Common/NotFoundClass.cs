using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class NotFoundClass : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public NotFoundClass(IDictionary<string, string[]> errors) : base("Page you are looking for is not available.")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
