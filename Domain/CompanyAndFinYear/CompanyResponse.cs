using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CompanyAndFinYear
{
    public class CompanyResponse
    {
        public int CompanyId {  get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string RegistrationNo {  get; set; } = string.Empty;
        public string? Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; } = string.Empty;
        public string? City {  get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string Pin {  get; set; } = string.Empty;        
    } // class...
}
