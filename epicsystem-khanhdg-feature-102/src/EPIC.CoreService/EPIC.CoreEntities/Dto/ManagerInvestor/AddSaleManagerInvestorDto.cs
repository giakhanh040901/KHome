using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class AddSaleManagerInvestorDto
    {
        private string _referralCode { get; set; }

        public int InvestorId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã giới thiệu không được bỏ trống")]
        public string ReferralCode { get => _referralCode; set => _referralCode = value?.Trim(); }
    }
}
