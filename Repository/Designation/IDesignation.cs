using Domain.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Designation
{
    public interface IDesignation
    {
        Task<List<DesignationResponse>> GetDesignations();
        Task<DesignationResponse> GetDesignationById(int id);
        Task<string> Save(DesignationResponse response);
    } // interface...
}
