using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SalaryComponent
{
    public class SalaryComponentResponse
    {
        public int ComponentId {  get; set; }
        public int CompanyId {  get; set; }
        public string ComponentName {  get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public string CalculationType {  get; set; } = string.Empty;
        public string Taxable { get; set; } = string.Empty;
        public string IsActive { get; set; } = string.Empty;
    } // SalaryComponentResponse...
}
