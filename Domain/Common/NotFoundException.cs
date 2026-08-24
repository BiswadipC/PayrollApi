using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public class NotFoundException : Exception
    {
        public IDictionary<string, string[]> errors { get; } = new Dictionary<string, string[]>();

        public NotFoundException(IDictionary<string, string[]> errors)
        {
            this.errors = errors;
        } // constructor...
    } // NotFoundException...
}
