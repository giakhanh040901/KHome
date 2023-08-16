using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class ChangePinDto
    {
        private string _oldPin;
        private string _newPin;
        //private string _phone;

        [Required(ErrorMessage = "Mã pin cũ không được bỏ trống")]
        [RegularExpression(RegexPatterns.OnlyNumber, ErrorMessage = "Mã pin chỉ được phép nhập số")]
        [MaxLength(6, ErrorMessage = "Mã pin cũ không dài quá 6 ký tự")]
        public string OldPin
        { 
            get => _oldPin; 
            set => _oldPin = value?.Trim();
        }

        [Required(ErrorMessage = "Mã pin mới không được bỏ trống")]
        [RegularExpression(RegexPatterns.OnlyNumber, ErrorMessage = "Mã pin chỉ được phép nhập số")]
        [MaxLength(6, ErrorMessage = "Mã pin mới không dài quá 6 ký tự")]
        public string NewPin 
        { 
            get => _newPin;
            set => _newPin = value?.Trim();
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        //public string Phone
        //{
        //    get => _phone;
        //    set => _phone = value?.Trim();
        //}
    }
}
