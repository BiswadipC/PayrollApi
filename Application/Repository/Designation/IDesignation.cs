using Domain.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Designation
{
    public interface IDesignation
    {
        Task<List<DesignationResponse>> GetDesignations();
        Task<DesignationResponse> GetDesignationById(int id);
        Task Save(DesignationResponse response);
    } // IDesignation...
}
