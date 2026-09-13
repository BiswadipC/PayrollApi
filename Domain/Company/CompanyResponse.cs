using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Company
{
    public class CompanyResponse
    {
        public int CompanyId {  get; private set; }
        public string CompanyCode { get; private set; } = string.Empty;
        public string CompanyName { get; private set; } = string.Empty;
        public string GSTIN { get; private set; } = string.Empty;
        public string RegNo { get; private set; } = string.Empty;
        public string Address1 { get; private set; } = string.Empty;
        public string Address2 { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public string Pin { get; private set; } = string.Empty;
        public string CurrencyCode { get; private set; } = string.Empty;
        public List<FinYearResponse> Years { get; private set; } = new List<FinYearResponse>();
        public string Verb { get; private set;  } = string.Empty;

        public CompanyResponse(int companyId, string companyCode, string companyName, string gstin, string regNo, string address1, string address2, string city, string state,
                    string country, string pin, string currencyCode, List<FinYearResponse> years, string verb)
        {
            CompanyId = companyId;
            CompanyCode = companyCode;
            CompanyName = companyName;
            GSTIN = gstin;
            RegNo = regNo;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Country = country;
            Pin = pin;
            CurrencyCode = currencyCode;
            Years = years;
            Verb = verb;
        } // constructor...
    } // class...

    public class FinYearResponse
    {
        public int YearId {  get; private set; }
        public string FromDate { get; private set; } = string.Empty;
        public string ToDate { get; private set; } = string.Empty;

        public FinYearResponse(int yearId, string fromDate, string toDate)
        {
            YearId = yearId;
            FromDate = fromDate;
            ToDate = toDate;
        } // constructor...
    } // class...
}
