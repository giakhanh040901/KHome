using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class BankAccountDto
    {
        //private string _bankBranch;
        private string _bankAccount;
        private string _ownerAccount;
        private string _phone;

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Tên ngân hàng không được bỏ trống")]
        //public string BankBranch 
        //{ 
        //    get => _bankBranch;
        //    set => _bankBranch = value?.Trim(); 
        //}
        public int BankId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số tài khoản ngân hàng không được bỏ trống")]
        [RegularExpression(RegexPatterns.OnlyNumber, ErrorMessage = "Số tài khoản ngân hàng chỉ được phép nhập số")]
        [MaxLength(25, ErrorMessage = "Số tài khoản dài tối đa 25 ký tự")]
        public string BankAccount
        {
            get => _bankAccount;
            set => _bankAccount = value?.Trim();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chủ tài khoản không được bỏ trống")]
        public string OwnerAccount
        {
            get => _ownerAccount;
            set => _ownerAccount = value?.Trim();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }
}
