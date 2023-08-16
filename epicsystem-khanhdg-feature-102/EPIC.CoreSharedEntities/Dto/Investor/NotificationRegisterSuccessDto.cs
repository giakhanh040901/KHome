using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class NotificationRegisterSuccessDto
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