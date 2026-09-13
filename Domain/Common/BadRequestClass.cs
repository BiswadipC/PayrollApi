using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class BadRequestClass : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public BadRequestClass(IDictionary<string, string[]> errors) : base("Bad Request.")
        {
            this.errors = errors;
        } // constructor...
    } // class...
}
