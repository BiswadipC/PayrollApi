using Domain.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Department
{
    public interface IDepartment
    {
        Task<List<DepartmentResponse>> GetDepartments();
        Task<DepartmentResponse> GetDepartmentById(int id);
        Task<string> Save(DepartmentResponse response);
    } // interface...
}
