using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class CreateBusinessCustomerTempDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã Doanh nghiệp không được bỏ trống")]
        [StringLength(20, ErrorMessage = "Mã Doanh nghiệp không được dài hơn 20 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên doanh nghiệp không được bỏ trống")]
        [StringLength(200, ErrorMessage = "Tên doanh nghiệp không được dài hơn 200 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _shortName;
        [Required(ErrorMessage = "Tên viết tắt không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Tên viết tắt không được dài hơn 100 ký tự")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value?.Trim();
        }

        private string _address;
        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [StringLength(1024, ErrorMessage = "Địa chỉ không được dài hơn 1024 ký tự")]
        public string Address
        {
            get => _address;
            set => _address = value?.Trim();
        }

        private string _nation;
        [Required(ErrorMessage = "Quốc gia không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Quốc gia không được dài hơn 256 ký tự")]
        public string Nation
        {
            get => _nation;
            set => _nation = value?.Trim();
        }

        private string _tradingAddress;
        [Required(ErrorMessage = "Địa chỉ giao dịch không được bỏ trống")]
        [StringLength(1024, ErrorMessage = "Địa chỉ không được dài hơn 1024 ký tự")]
        public string TradingAddress
        {
            get => _tradingAddress;
            set => _tradingAddress = value?.Trim();
        }

        private string _phone;
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được dài hơn 20 ký tự")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _mobile;
        [StringLength(15, ErrorMessage = "Mobile không được dài hơn 15 ký tự")]
        public string Mobile
        {
            get => _mobile;
            set => _mobile = value?.Trim();
        }

        private string _email;
        [Required(ErrorMessage = "Thư điện tử không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Email không được dài hơn 50 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        private string _taxCode;
        [Required(ErrorMessage = "Mã số thuế không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Mã số thuế không được dài hơn 100 ký tự")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }

        private string _bankaccno;
        [StringLength(100, ErrorMessage = "Số tài khoản ngân hàng không được dài hơn 100 ký tự")]
        public string BankAccNo
        {
            get => _bankaccno;
            set => _bankaccno = value?.Trim();
        }

        private string _bankAccName;
        [StringLength(128, ErrorMessage = "Số tài khoản ngân hàng không được dài hơn 128 ký tự")]
        public string BankAccName
        {
            get => _bankAccName;
            set => _bankAccName = value?.Trim();
        }

        private string _bankName;
        [StringLength(200, ErrorMessage = "Tên ngân hàng không được dài hơn 200 ký tự")]
        public string BankName
        {
            get => _bankName;
            set => _bankName = value?.Trim();
        }

        private string _bankBranchName;
        [StringLength(200, ErrorMessage = "Chi nhánh ngân hàng không được dài hơn 200 ký tự")]
        public string BankBranchName
        {
            get => _bankBranchName;
            set => _bankBranchName = value?.Trim();
        }

        [Required(ErrorMessage = "Ngày cấp không được bỏ trống")]
        public DateTime? LicenseDate { get; set; }

        private string _licenseIssuer;
        [Required(ErrorMessage = "Nơi cấp giấy phép không được bỏ trống")]
        [StringLength(1024, ErrorMessage = "Nơi cấp giấy phép đăng ký kinh doanh không được dài hơn 1024 ký tự")]
        public string LicenseIssuer
        {
            get => _licenseIssuer;
            set => _licenseIssuer = value?.Trim();
        }
        public decimal? Capital { get; set; }


        private string _repName;
        [Required(ErrorMessage = "Họ tên người đại diện pháp luật không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Họ tên người đại diện pháp luật không được dài hơn 256 ký tự")]
        public string RepName
        {
            get => _repName;
            set => _repName = value?.Trim();
        }

        private string _repPosition;
        [Required(ErrorMessage = "Vị trí người đại diện không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Vị trí người đại diện không được dài hơn 256 ký tự")]
        public string RepPosition
        {
            get => _repPosition;
            set => _repPosition = value?.Trim();
        }

        [StringLength(128, ErrorMessage = "Quyết định số không được dài hơn 128 ký tự")]
        private string _decisionNo;
        public string DecisionNo
        {
            get => _decisionNo;
            set => _decisionNo = value?.Trim();
        }
        public DateTime? DecisionDate { get; set; }
        public int? NumberModified { get; set; }
        public DateTime? DateModified { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? BusinessCustomerBankId { get; set; }
        public int? BankId { get; set; }

        private string _repIdNo;
        [StringLength(50, ErrorMessage = "Số CCCD/CMND Không được dài hơn 50 ký tự")]
        public string RepIdNo
        {
            get => _repIdNo;
            set => _repIdNo = value?.Trim();
        }

        public DateTime? RepIdDate { get; set; }

        private string _repIdIssuer;
        [StringLength(256, ErrorMessage = "Nơi phát hành không được dài hơn 256 ký tự")]
        public string RepIdIssuer
        {
            get => _repIdIssuer;
            set => _repIdIssuer = value?.Trim();
        }

        private string _repAddress;
        [StringLength(1024, ErrorMessage = "Địa chỉ người đại diện không được dài hơn 1024 ký tự")]
        public string RepAddress
        {
            get => _repAddress;
            set => _repAddress = value?.Trim();
        }

        [StringRange(AllowableValues = new string[] { Genders.MALE, Genders.FEMALE}, ErrorMessage = "Vui lòng chọn Giới tính")]
        public string RepSex { get; set; }

        public DateTime? RepBirthDate { get; set; }

        private string _website;
        public string Website
        {
            get => _website;
            set => _website = value?.Trim();
        }

        private string _fanpage;
        public string Fanpage
        {
            get => _fanpage;
            set => _fanpage = value?.Trim();
        }
        private string _businessRegistrationImg;
        public string BusinessRegistrationImg
        {
            get => _businessRegistrationImg;
            set => _businessRegistrationImg = value?.Trim();
        }
        private string _server;
        public string Server
        {
            get => _server;
            set => _server = value?.Trim();
        }

        private string _key;
        public string Key
        {
            get => _key;
            set => _key = value?.Trim();
        }

        private string _secret;
        public string Secret
        {
            get => _secret;
            set => _secret = value?.Trim();
        }
        private string _avatarImageUrl;
        public string AvatarImageUrl
        {
            get => _avatarImageUrl;
            set => _avatarImageUrl = value?.Trim();
        }
        private string _stampImageUrl;
        public string StampImageUrl
        {
            get => _stampImageUrl;
            set => _stampImageUrl = value?.Trim();
        }
        public int? PartnerId { get; set; }
    }
}
