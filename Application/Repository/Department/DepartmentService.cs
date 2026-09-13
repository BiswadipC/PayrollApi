using Application.DTO.Department;
using Domain.Department;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Department
{
    public class DepartmentService
    {
        private readonly IDepartment idepartment;

        public DepartmentService(IDepartment idepartment)
        {
            this.idepartment = idepartment;
        } // constructor...

        public async Task<List<DepartmentDTO>> GetDepartments()
        {
            var departments = (await idepartment.GetDepartments()).Select(x => new DepartmentDTO()
            {
                IdNo = x.IdNo,
                Name = x.Name,
            }).ToList();

            return departments;
        } // GetDepartments...

        public async Task<DepartmentDTO> GetDepartmentById(int id)
        {
            var department = await idepartment.GetDepartmentById(id);
            return new DepartmentDTO()
            {
                IdNo = department.IdNo,
                Name = department.Name
            };
        } // GetDepartmentById...

        public async Task Save(DepartmentDTO department)
        {
            DepartmentResponse response = new DepartmentResponse(department.IdNo, department.Name);
            await idepartment.Save(response);
        } // Save...
    } // class...
}
