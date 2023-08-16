using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleInvestor
{
    public class SaleRegisterInvestorDto
    {
        private string _phone { get; set; }
        private string _email { get; set; }
        private string _referralCode { get; set; }

        [PhoneEpic(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }

        [Email(ErrorMessage = "Email không hợp lệ")]
        public string Email { get => _email; set => _email = value?.Trim(); }

        public string ReferralCode { get => _referralCode; set => _referralCode = value?.Trim(); }
    }
}
