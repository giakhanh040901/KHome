using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.PolicyTemp
{
    public class CreatePolicyTempDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã Chính sách không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Mã chính sách không được dài hơn 50 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên chính sách không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 50 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }


        [Required(ErrorMessage = "Vui lòng chọn kiểu chính sách sản phẩm")]
        public int Type { get; set; }

        private string _investorType;
        [Required(ErrorMessage = "Loại nhà đầu tư không được bỏ trống")]
        [StringLength(1, ErrorMessage = "Vui lòng chọn Loại nhà đầu tư")]
        public string InvestorType
        {
            get => _investorType;
            set => _investorType = value?.Trim();
        }

        [Required(ErrorMessage = "Thuế lợi nhuận % không được bỏ trống")]
        public decimal IncomeTax { get; set; }

        [Required(ErrorMessage = "Thuế chuyển nhượng không được bỏ trống")]
        public decimal TransferTax { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        public decimal Classify { get; set; }

        [Required(ErrorMessage = "Số tiền đầu tư tối thiểu không được bỏ trống")]
        public decimal MinMoney { get; set; }

        private string _isTransfer;
        [StringLength(1, ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
        [Required(ErrorMessage = "Vui lòng chọn Có cho phép chuyển nhượng")]
        public string IsTransfer
        {
            get => _isTransfer;
            set => _isTransfer = value?.Trim();
        }

        public List<CreatePolicyDetailTempDto> PolicyDetailTemp { get; set; }
    }

    public class CreatePolicyDetailTempDto
    {
        [Required(ErrorMessage = "Số thứ tự không được bỏ trống")]
        public int? Stt { get; set; }

        private string _shortName;
        [StringLength(50, ErrorMessage = "Tên viết tắt kỳ hạn không được dài qua 50 ký tự")]
        [Required(ErrorMessage = "Tên viết tắt kỳ hạn không được bỏ trống")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        private string _name;
        [StringLength(256, ErrorMessage = "Tên kỳ hạn không được dài qua 256 ký tự")]
        [Required(ErrorMessage = "Tên kỳ hạn không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }

        [Required(ErrorMessage = "Đơn vị không được bỏ trống")]
        public string PeriodType { get; set; }

        [Required(ErrorMessage = "Số kỳ đầu tư không được bỏ trống")]
        public int? PeriodQuantity { get; set; }

        [Required(ErrorMessage = "Lợi tức không được bỏ trống")]
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }

        [Required(ErrorMessage = "Kiểu trả lợi tức không được bỏ trống")]
        public int? InterestType { get; set; }
    }

    public class PolicyDetailTempDto : CreatePolicyDetailTempDto
    {
        public int CPSPolicyTempId { get; set; }
    }
}
