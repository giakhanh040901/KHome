using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Partner;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityEntities.Dto.UsersPartner
{
    public class UsersPartnerDto
    {
        public decimal UserId { get; set; }
        public decimal TradingProviderId { get; set; }
        public decimal PartnerId { get; set; }
        public MyInfoDto UserInfo { get; set; }
        public InvestorDto Investor { get; set; }
        public TradingProviderDto TradingProvider { get; set; }
        public PartnerDto Partner { get; set; }
    }
}
