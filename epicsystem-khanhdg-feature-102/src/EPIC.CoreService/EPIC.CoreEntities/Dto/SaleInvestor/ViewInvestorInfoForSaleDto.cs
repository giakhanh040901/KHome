using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.ManagerInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    /// <summary>
    /// Sale xem chi tiết thông tin khách ở app
    /// </summary>
    public class ViewInvestorInfoForSaleDto
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AvatarImageUrl { get; set; }
        public int ProfStatus { get; set; }
        public string UserStatus { get; set; }
        public string ReferralCodeSelf { get; set; }
        public ViewIdentificationDto DefaultIdentification { get; set; }
        public List<ViewIdentificationDto> ListIdentifications { get; set; }
        public ViewInvestorBankAccountDto DefaultBank { get; set; }
        public ViewInvestorContactAddressDto DefaultContactAddress { get; set; }
    }
}
