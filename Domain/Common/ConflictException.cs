using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class ConflictException : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public ConflictException(IDictionary<string, string[]> errors)
        {
            this.errors = errors;
        } // constructor...
    } // ConflictException...
}
