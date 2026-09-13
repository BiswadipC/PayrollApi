using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Company
{
    public class CompanyDTO
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public List<FinYearDTO> Years { get; set; } = new List<FinYearDTO>();
        public string Verb {  get; set; } = string.Empty;
    } // class...

    public class FinYearDTO
    {
        public int YearId { get; set; }
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
    } // class...
}
