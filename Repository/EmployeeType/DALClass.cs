using Domain.EmployeeType;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.EmployeeType
{
    internal class DALClass : IEmployeeType
    {
        private readonly PayrollContext context;

        public DALClass(PayrollContext context)
        {
            this.context = context;
        } // constructor...

        public async Task<List<EmployeeTypeResponse>> GetEmployeeTypes()
        {
            var employeeTypes = await context.EmployeeTypes.Select(m => new EmployeeTypeResponse()
            {
                TypeId = m.TypeId,
                TypeName = m.TypeName
            }).ToListAsync();

            return employeeTypes;
        } // EmployeeTypeResponse...

        public async Task<EmployeeTypeResponse> GetEmployeeTypeByTypeId(int typeId)
        {
            var employeeType = await context.EmployeeTypes.Select(x => new EmployeeTypeResponse()
            {
                TypeId = x.TypeId,
                TypeName = x.TypeName
            }).FirstOrDefaultAsync(m => m.TypeId == typeId) ?? new EmployeeTypeResponse();

            return employeeType;
        } // GetEmployeeTypeByTypeId...
    } // class...
}
