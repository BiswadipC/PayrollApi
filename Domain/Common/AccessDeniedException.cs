using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class AccessDeniedException : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public AccessDeniedException(IDictionary<string, string[]> errors) : base("You do not have access to the given resources.")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
