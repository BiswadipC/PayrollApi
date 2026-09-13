using Domain.Department;
using Domain.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Department
{
    public interface IDepartment
    {
        Task<List<DepartmentResponse>> GetDepartments();
        Task<DepartmentResponse> GetDepartmentById(int id);
        Task Save(DepartmentResponse response);
    } // interface...
}
