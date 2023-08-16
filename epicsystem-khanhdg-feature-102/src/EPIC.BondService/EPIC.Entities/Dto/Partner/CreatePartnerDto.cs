using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Partner
{
    public class CreatePartnerDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã doanh nghiệp không được bỏ trống")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên doanh nghiệp không được bỏ trống")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _shortName;
        [Required(ErrorMessage = "Tên viết tắt không được bỏ trống")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        private string _address;
        [Required(ErrorMessage = "Địa chỉ đăng ký không được bỏ trống")]
        public string Address
        {
            get => _address;    
            set => _address = value?.Trim();
        }

        private string _phone;
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set => _mobile = value?.Trim();
        }

        private string _email;
        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        private string _taxCode;
        [Required(ErrorMessage = "Mã số thuế không được bỏ trống")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }
        public DateTime? LicenseDate { get; set; }

        private string _licenseIssuer;
        [Required(ErrorMessage = "Nơi cấp không được bỏ trống")]
        public string LicenseIssuer
        {
            get => _licenseIssuer;
            set => _licenseIssuer = value?.Trim();
        }

        [Range(0, double.MaxValue, ErrorMessage = "Vốn điều lệ phải lớn hơn 0")]
        [Required(ErrorMessage = "Vốn điều lệ không được bỏ trống")]
        public double? Capital { get; set; }

        private string _repName;
        [Required(ErrorMessage = "Họ tên người đại diện không được bỏ trống")]
        public string RepName
        {
            get => _repName;
            set => _repName = value?.Trim();
        }

        private string _repPosition;
        [Required(ErrorMessage = "Vị trí người đại diện không được bỏ trống")]
        public string RepPosition
        {
            get => _repPosition;
            set => _repPosition = value?.Trim();
        }

        private string _tradingAddress;
        public string TradingAddress
        {
            get => _tradingAddress;
            set => _tradingAddress = value?.Trim();
        }

        private string _nation;
        [Required(ErrorMessage = "Quốc gia không được bỏ trống")]
        public string Nation
        {
            get => _nation;
            set => _nation = value?.Trim();
        }

        private string _decisionNo;
        public string DecisionNo
        {
            get => _decisionNo;
            set => _decisionNo = value?.Trim();
        }

        public int? NumberModified;

        public DateTime? DateModified { get; set; }
        public DateTime? DecisionDate { get; set; }
    }
}
