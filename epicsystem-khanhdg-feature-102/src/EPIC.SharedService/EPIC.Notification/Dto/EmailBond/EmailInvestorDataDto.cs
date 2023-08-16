using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using System.Collections.Generic;

namespace EPIC.Notification.Dto.EmailBond
{
    public class EmailInvestorDataDto
    {
        public CifCodes CifCode { get; set; }
        public Investor Investor { get; set; }
        public InvestorIdentification InvestorIdentification { get; set; }
        public UserDto Users { get; set; }
        public AuthOtp Otp { get; set; }
        public InvestorContactAddress InvestorContactAddress { get; set; }
        public InvestorBankAccount InvestorBankAccount { get; set; }
        public ParamsChooseTemplate OtherParams { get; set; }
        public List<string> FcmTokens { get; set; }
    }

    public class EmailDataBond
    {
        public Investor Investor { get; set; }
        public InvestorIdentification InvestorIdentification { get; set; }

        public BusinessCustomer BusinessCustomer { get; set; }
        public ParamsChooseTemplate OtherParams { get; set; }
        public UserDto Users { get; set; }
        public List<string> FcmTokens { get; set; }
    }
}
