using Domain.Bank;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Bank
{
    public class BankDTO
    {
        public int BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public List<BranchDTO> Branches { get; set; } = new List<BranchDTO>();
        public string Flag { set; get; } = string.Empty;
    } // class...

    public class BranchDTO
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
    } // class...
}
