using Domain.SalaryComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.SalaryComponent
{
    public interface ISalaryComponent
    {
        Task<List<SalaryComponentResponse>> GetSalaryComponents();
        Task<SalaryComponentResponse> GetSalaryComponentByComponentId(int componentId);
        Task Save(SalaryComponentResponse response);
    } // interface...
}
