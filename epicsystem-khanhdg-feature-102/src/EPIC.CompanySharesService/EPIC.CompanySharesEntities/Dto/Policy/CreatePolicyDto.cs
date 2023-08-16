using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Policy
{
    public class CreatePolicyDto
    {
        private string _policyCode;
        [Required(ErrorMessage = "Mã chính sách không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mã chính sách không được dài hơn 50 ký tự")]
        public string PolicyCode
        {
            get => _policyCode;
            set => _policyCode = value?.Trim();
        }

        private string _policyName;
        [Required(ErrorMessage = "Tên chính sách không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 256 ký tự")]
        public string PolicyName
        {
            get => _policyName;
            set => _policyName = value?.Trim();
        }

        private string _policyType;
        [StringLength(50, ErrorMessage = "Loại chính sách không được dài hơn 50 ký tự")]
        public string PolicyType
        {
            get => _policyType;
            set => _policyType = value?.Trim();
        }

        private string _policyDesc;
        [StringLength(50, ErrorMessage = "Mô tả không được dài hơn 256 ký tự")]
        public string PolicyDesc
        {
            get => _policyDesc;
            set => _policyDesc = value?.Trim();
        }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Vui lòng chọn Cho phép chuyển nhượng hay không?")]
        public string AllowTransfer { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        public decimal TransferTaxRate { get; set; }

        [StringRange(AllowableValues = new string[] { InvestorType.PROFESSIONAL, InvestorType.ALL }, ErrorMessage = "Vui lòng chọn Loại khách hàng")]
        public string CustType { get; set; }

        [Required(ErrorMessage = "Số ngày chốt phát hành không được bỏ trống")]
        public int? CallDay { get; set; }

        [Required(ErrorMessage = "Giá trị đầu tư tối thiểu không được bỏ trống")]
        public decimal? MinValue { get; set; }
    }
}
