using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class ValidatePinDto
    {
        //private string _phone;
        private string _pin;

        [Required(ErrorMessage = "Mã pin không được bỏ trống")]
        [RegularExpression(RegexPatterns.OnlyNumber, ErrorMessage = "Mã pin chỉ được phép nhập số")]
        public string Pin
        {
            get => _pin;
            set => _pin = value?.Trim();
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        //public string Phone
        //{
        //    get => _phone;
        //    set => _phone = value?.Trim();
        //}
    }
}
