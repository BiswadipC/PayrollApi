using Domain.EmployeeType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EmployeeType
{
    public interface IEmployeeType
    {
        Task<List<EmployeeTypeResponse>> GetEmployeeTypes();
        Task<EmployeeTypeResponse> GetEmployeeTypeByTypeId(int typeId);
        Task<string> Save(EmployeeTypeResponse response);
    } // interface...
}
