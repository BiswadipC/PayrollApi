using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class BadRequestException : Exception
    {
        public IDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        public BadRequestException(IDictionary<string, string[]> errors)
        {
            this.errors = errors;
        } // constructor...
    } // BadRequestException...
}
