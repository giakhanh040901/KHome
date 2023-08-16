using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CreateUserByInvestorDto
    {
        private string _password { get; set; }
        private string _username { get; set; }


        public int InvestorId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [RegularExpression(RegexPatterns.Password8Characters1Uppercase1Lowercase1Number,
            ErrorMessage = "Mật khẩu ít nhất 8 ký tự gồm 1 chữ hoa 1 chữ thường và 1 số")]
        public string Password { get => _password; set => _password = value?.Trim(); }
        public string UserName { get => _username; set => _username = value?.Trim(); }

    }
}
