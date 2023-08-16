using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class ForgotPasswordDto
    {
        private string _emailOrPhone;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại hoặc email không được bỏ trống")]
        public string EmailOrPhone
        {
            get => _emailOrPhone;
            set => _emailOrPhone = value?.Trim();
        }

        [Required(ErrorMessage = "Phương thức nhận OTP không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { ForgotPasswordType.SEND_MAIL, ForgotPasswordType.SEND_SMS })]
        public string SendType { get; set; }
    }
}
