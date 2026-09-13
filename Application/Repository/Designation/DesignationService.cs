using Application.DTO.Designation;
using Domain.Designation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Designation
{
    public class DesignationService
    {
        private readonly IDesignation designation;

        public DesignationService(IDesignation designation)
        {
            this.designation = designation;
        } // constructor...

        public async Task<List<DesignationDTO>> GetDesignations()
        {
            var designations = (await designation.GetDesignations()).Select(x => new DesignationDTO()
            {
                IdNo = x.IdNo,
                Name = x.Name
            }).ToList();

            return designations;
        } // GetDesignations...

        public async Task<DesignationDTO> GetDesignationById(int id)
        {
            var desig = (await designation.GetDesignationById(id));
            DesignationDTO dto = new DesignationDTO()
            {
                IdNo = desig.IdNo,
                Name = desig.Name
            };

            return dto;
        } // GetDesignationById...

        public async Task Save(DesignationDTO response)
        {
            DesignationResponse desig = new DesignationResponse(response.IdNo, response.Name);
            await designation.Save(desig);
        } // Save...
    } // DesignationService...
}
