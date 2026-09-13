using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Designation
{
    public class DesignationResponse
    {
        public int IdNo {  get; private set; }
        public string Name {  get; private set; } = string.Empty;

        public DesignationResponse(int idNo, string name)
        {
            List<string> errors = new List<string>();            
            if(string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Designation Name cannot be blank.");
            }
            if (name.Length < 3)
            {
                errors.Add("Designation Name cannot be less than 3 characters length.");
            }

            if(errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>()
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray()  }
                });
            }

            IdNo = idNo;
            Name = name;
        } // constructor...
    } // class...
}
