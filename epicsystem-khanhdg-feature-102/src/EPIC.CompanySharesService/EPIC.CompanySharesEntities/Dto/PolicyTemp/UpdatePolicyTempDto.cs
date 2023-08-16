using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.PolicyTemp
{
    public class UpdatePolicyTempDto
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

        public decimal? TransferTax { get; set; }

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
    }
}
