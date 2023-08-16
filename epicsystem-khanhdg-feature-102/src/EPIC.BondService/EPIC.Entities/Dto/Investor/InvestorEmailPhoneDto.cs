using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    /// <summary>
    /// Ràng buộc có email khoặc phone
    /// </summary>
    public class InvestorEmailPhoneDto
    {
        private string _email;
        private string _phone;

        [RequiredWithOtherFields(ErrorMessage = "Email không được bỏ trống", OtherFields = new string[] { "Phone" })]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        [RequiredWithOtherFields(ErrorMessage = "Số điện thoại không được bỏ trống", OtherFields = new string[] { "Email" })]
        [MaxLength(10, ErrorMessage = "Số điện thoại không dài quá 10 ký tự")]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = "Bắt đầu bằng số 0 và chỉ được phép nhập số")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }
}
