using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class UnAuthorizedException : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public UnAuthorizedException(IDictionary<string, string[]> errors) : base("You are not authorized to use this page")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
