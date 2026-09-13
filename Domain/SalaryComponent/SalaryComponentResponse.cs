using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SalaryComponent
{
    public class SalaryComponentResponse
    {
        public int ComponentId { get; private set; }
        public int CompanyId { get; private set; }
        public string ComponentCode { get; private set; } = string.Empty;
        public string ComponentName { get; private set; } = string.Empty;
        public string ComponentType { get; private set; } = string.Empty;
        public string CalculationType { get; private set; } = string.Empty;
        public string Taxable { get; private set; } = string.Empty;
        public string IsActive { get; private set; } = string.Empty;

        public SalaryComponentResponse(int componentId, int companyId, string componentCode, string componentName, string componentType, 
                    string calculationType, string Taxable, string IsActive)
        {
            List<string> errors = new List<string>();

            if(string.IsNullOrWhiteSpace(componentCode))
            {
                errors.Add("Component Code cannot be blank.");
            }
            if (string.IsNullOrWhiteSpace(componentName))
            {
                errors.Add("Component Name cannot be blank.");
            }
            else if (componentName.Length < 3)
            {
                errors.Add("Component Name must be of minimum 3 characters length.");
            }
            if(componentType != "EARNING" && componentType != "DEDUCTION" && componentType != "TAX" && componentType != "EMPLOYER_CONTRIBUTION")
            {
                errors.Add("Invalid \'Component Type\'. Specify a valid \'Component Type\'.");
            }
            if (calculationType != "FIXED" && calculationType != "PERCENTAGE" && calculationType != "FORMULA" && calculationType != "ATTENDANCE_BASED")
            {
                errors.Add("Invalid \'Calculation Type\'. Specify a valid \'Calculation Type\'.");
            }

            if(errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>()
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray() }
                });
            }

            ComponentId = componentId;
            CompanyId = companyId;
            ComponentCode = componentCode;
            ComponentName = componentName;
            ComponentType = componentType;
            CalculationType = calculationType;
            this.Taxable = Taxable;
            this.IsActive = IsActive;
        } // constructor...
    } // class...
}
