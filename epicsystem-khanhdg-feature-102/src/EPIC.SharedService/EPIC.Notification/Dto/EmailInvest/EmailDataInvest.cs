using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Notification.Dto.EmailInvest
{
    public class EmailDataInvest
    {
        public Investor Investor { get; set; }
        public InvestorIdentification InvestorIdentification { get; set; }
        public ParamsChooseTemplate OtherParams { get; set; }
        public UserDto Users { get; set; }
        public List<string> FcmTokens { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }
    }
}
