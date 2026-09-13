using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class ForbiddenClass : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public ForbiddenClass(IDictionary<string, string[]> errors) : base("Forbidden.")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
