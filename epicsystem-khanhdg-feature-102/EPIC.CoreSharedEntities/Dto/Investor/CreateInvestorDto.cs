using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class CreateInvestorDto
    {
        private string _userName { get; set; }
        private string _password { get; set; }

        private string _name { get; set; }
        private string _bori { get; set; }
        private string _dorf { get; set; }
        private string _sex { get; set; }
        private string _eduLevel { get; set; }
        private string _occupation { get; set; }
        private string _address { get; set; }
        private string _contactAddress { get; set; }
        private string _nationality { get; set; }
        private string _mobile { get; set; }
        private string _email { get; set; }
        private string _taxCode { get; set; }

        private string _bankAccount { get; set; }
        private string _bankCode { get; set; }
        private string _bankName { get; set; }
        private string _bankBranch { get; set; }


        [Required(ErrorMessage = "Tài khoản không được bỏ trống")]
        public string UserName { get => _userName; set => _userName = value?.Trim(); }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get => _password; set => _password = value?.Trim(); }


        [Required(ErrorMessage = "Họ và tên không được bỏ trống")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        public string Bori { get => _bori; set => _bori = value?.Trim(); }
        public string Dorf { get => _dorf; set => _dorf = value?.Trim(); }

        [Required(ErrorMessage = "Giới tính không được bỏ trống")]
        public string Sex { get => _sex; set => _sex = value?.Trim(); }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateTime? BirthDate { get; set; }
        public string EduLevel { get => _eduLevel; set => _eduLevel = value?.Trim(); }
        public string Occupation { get => _occupation; set => _occupation = value?.Trim(); }
        public string Address { get => _address; set => _address = value?.Trim(); }
        public string ContactAddress { get => _contactAddress; set => _contactAddress = value?.Trim(); }
        public string Nationality { get => _nationality; set => _nationality = value?.Trim(); }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Mobile { get => _mobile; set => _mobile = value?.Trim(); }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Địa chỉ email không được bỏ trống")]
        public string Email { get => _email; set => _email = value?.Trim(); }
        [Required(ErrorMessage = "Mã số thuế không được bỏ trống")]
        public string TaxCode { get => _taxCode; set => _taxCode = value?.Trim(); }

        public string BankAccount { get => _bankAccount; set => _bankAccount = value?.Trim(); }
        public string BankCode { get => _bankCode; set => _bankCode = value?.Trim(); }
        public string BankName { get => _bankName; set => _bankName = value?.Trim(); }
        public string BankBranch { get => _bankBranch; set => _bankBranch = value?.Trim(); }
    }

    /// <summary>
    /// Ràng buộc cả email và phone
    /// </summary>
    public class InvestorEmailPhoneRequiredDto
    {
        private string _email;
        private string _phone;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không dài quá 10 ký tự")]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = "Bắt đầu bằng số 0 và chỉ được phép nhập số")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }

    public class VerificationCodeDto : InvestorEmailPhoneDto
    {
        public string _verificationCode;

        [Required(ErrorMessage = "Mã xác thực không được bỏ trống")]
        [MaxLength(6, ErrorMessage = "Mã xác thực không dài quá 6 ký tự")]
        public string VerificationCode
        {
            get => _verificationCode;
            set => _verificationCode = value?.Trim();
        }
    }

    public class RegisterInvestorDto : InvestorEmailPhoneRequiredDto
    {
        private string _password;
        private string _referralCode;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }

        public string ReferralCode
        {
            get => _referralCode;
            set => _referralCode = value?.Trim();
        }

        private string _fcmToken;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "FcmToken không được bỏ trống")]
        public string FcmToken
        {
            get => _fcmToken;
            set => _fcmToken = value?.Trim();
        }
    }

    public class CheckRegisteredOfflineDto
    {
        private string _phone;
        private string _email;

        [RequiredWithOtherFields(ErrorMessage = "Số điện thoại không được bỏ trống", OtherFields = new string[] { "Email" })]
        [MaxLength(10, ErrorMessage = "Số điện thoại không dài quá 10 ký tự")]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = "Bắt đầu bằng số 0 và chỉ được phép nhập số")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        [RequiredWithOtherFields(ErrorMessage = "Email không được bỏ trống", OtherFields = new string[] { "Phone" })]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }
    }

    public class CheckEmailExistDto
    {
        private string _email;

        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }
    }

    public class CheckPhoneExistDto
    {
        private string _phone;

        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không dài quá 10 ký tự")]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = "Bắt đầu bằng số 0 và chỉ được phép nhập số")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }
}
