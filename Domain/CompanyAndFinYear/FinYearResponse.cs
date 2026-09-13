using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CompanyAndFinYear
{
    public class FinYearResponse
    {
        public int YearId {  get; set; }
        public string FromDate {  get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
    } // class...
}
