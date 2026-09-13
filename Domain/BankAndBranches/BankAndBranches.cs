using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.BankAndBranches
{
    public class BankResponse
    {
        public int BankId {  get; set; }
        public string BankName { get; set; } = string.Empty;
        public List<BranchResponse> Branches {  get; set; } = new List<BranchResponse>();
    } // class...

    public class BranchResponse
    {
        public int BranchId { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;
        public string PhoneNo {  get; set; } = string.Empty;
    } // class...
}
