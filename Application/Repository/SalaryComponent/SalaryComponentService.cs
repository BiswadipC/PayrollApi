using Application.DTO.SalaryComponent;
using AutoMapper;
using Domain.SalaryComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.SalaryComponent
{
    public class SalaryComponentService
    {
        private readonly ISalaryComponent icomponent;
        private readonly IMapper mapper;

        public SalaryComponentService(ISalaryComponent icomponent, IMapper mapper)
        {
            this.icomponent = icomponent;
            this.mapper = mapper;
        } // constructor...

        public async Task<List<SalaryComponentDTO>> GetSalaryComponents()
        {
            var components = await icomponent.GetSalaryComponents();
            var dtos = mapper.Map<List<SalaryComponentDTO>>(components);
            return dtos;
        } // GetSalaryComponents...

        public async Task<SalaryComponentDTO> GetSalaryComponentByComponentId(int componentId)
        {
            var component = await icomponent.getSalaryComponentByComponentId(componentId);
            var dto = mapper.Map<SalaryComponentDTO>(component);
            return dto;
        } // GetSalaryComponentByComponentId...

        public async Task Save(SalaryComponentDTO dto)
        {
            var salaryComponent = mapper.Map<SalaryComponentResponse>(dto);
            await icomponent.Save(salaryComponent);
        } // Save...
    } // class...
}
