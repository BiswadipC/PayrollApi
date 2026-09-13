using Domain.SalaryComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.SalaryComponent
{
    public interface ISalaryComponent
    {
        Task<List<SalaryComponentResponse>> GetSalaryComponents();
        Task<SalaryComponentResponse> getSalaryComponentByComponentId(int componentId);
        Task Save(SalaryComponentResponse salaryComponentResponse);
    } // interface...
}
