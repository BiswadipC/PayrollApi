using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class UnAuthorizedClass : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public UnAuthorizedClass(IDictionary<string, string[]> errors) : base("Access Denied.")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
