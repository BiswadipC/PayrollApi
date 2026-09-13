using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bank
{
    public class BankResponse
    {
        public int BankId { get; private set; }
        public string BankName { get; private set; } = string.Empty;
        public List<BranchResponse> Branches { get; private set; } = new List<BranchResponse>();
        public string Flag { private set; get; } = string.Empty;

        public BankResponse(int bankId, string bankName, List<BranchResponse> branches, string flag)
        {
            List<string> errors = new List<string>();

            if((flag == "POST" || flag == "PUT") && string.IsNullOrWhiteSpace(bankName))
            {
                errors.Add("Bank Name cannot be blank.");
            }
            else if((flag == "POST" || flag == "PUT") && bankName.Length < 3)
            {
                errors.Add("Bank Name must be atleast 3 characters length.");
            }
            if ((flag == "POST" || flag == "PUT") && (branches == null || branches.Count() == 0))
            {
                errors.Add("A Bank must have atleast one Branch.");
            }

            if (errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>()
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray()  }
                });
            }

            BankId = bankId;
            BankName = bankName;
            Branches = branches!;
        } // constructor...
    } // class...

    public class BranchResponse
    {
        public int BranchId { get; private set; }
        public string BranchCode { get; private set; } = string.Empty;
        public string BranchName { get; private set; } = string.Empty;
        public string IFSCCode { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string PhoneNo { get; private set; } = string.Empty;

        public BranchResponse(int branchId, string branchCode, string branchName, string iFSCCode, string address, string phoneNo)
        {
            List<string> errors = new List<string>();

            if(string.IsNullOrWhiteSpace(branchCode))
            {
                errors.Add("Branch Code cannot be blank");
            }
            if (string.IsNullOrWhiteSpace(branchName))
            {
                errors.Add("Branch Name cannot be blank");
            }
            if (string.IsNullOrWhiteSpace(iFSCCode))
            {
                errors.Add("IFSC Code cannot be blank");
            }

            if(errors.Any())
            {
                throw new BadRequestClass(new Dictionary<string, string[]>()
                {
                    {GlobalConstantClass.BadRequestKey, errors.ToArray()  }
                });
            }

            BranchId = branchId;
            BranchCode = branchCode;
            BranchName = branchName;
            IFSCCode = iFSCCode;
            Address = address;
            PhoneNo = phoneNo;
        } // constructor...
    } // class...
}
