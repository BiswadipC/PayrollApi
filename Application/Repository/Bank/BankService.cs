using Application.DTO.Bank;
using AutoMapper;
using Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Repository.Bank
{
    public class BankService
    {
        private readonly IBank ibank;
        private readonly IMapper mapper;

        public BankService(IBank ibank, IMapper mapper)
        {
            this.ibank = ibank;
            this.mapper = mapper;
        } // constructor...

        public async Task<List<BankDTO>> GetBanks()
        {
            var banks = await ibank.GetBanks();
            var dtos = mapper.Map<List<BankDTO>>(banks);
            return dtos;
        } // GetBanks...

        public async Task<List<BranchDTO>> GetBranchesByBankId(int bankId)
        {
            var branches = await ibank.GetBranchesByBankId(bankId);
            var dtos = mapper.Map<List<BranchDTO>>(branches);
            return dtos.ToList();
        } // GetBranchesByBankId...

        public async Task<BankDTO> GetBankByBankId(int bankId)
        {
            var bank = await ibank.GetBankByBankId(bankId);
            var dto = mapper.Map<BankDTO>(bank);
            return dto;
        } // GetBankByBankId...

        public async Task Save(BankDTO bank)
        {
            var bankResponse = mapper.Map<BankResponse>(bank);
            await ibank.Save(bankResponse);
        } // Save...
    } // class...
}
