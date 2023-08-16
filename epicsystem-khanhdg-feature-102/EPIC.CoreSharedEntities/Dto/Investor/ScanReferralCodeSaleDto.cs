using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class ScanReferralCodeSaleDto
    {
        private string _refCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được bỏ trống mã giới thiệu")]
        public string ReferralCode { get => _refCode; set => _refCode = value?.Trim(); }
    }
}
