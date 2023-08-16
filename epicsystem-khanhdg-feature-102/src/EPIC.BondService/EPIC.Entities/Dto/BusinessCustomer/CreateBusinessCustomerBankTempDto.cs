using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class CreateBusinessCustomerBankTempDto
    {
        public int? BusinessCustomerId { get; set; }
        [Required(ErrorMessage = "Tên ngân hàng không được bỏ trống")]
        public int BankId { get; set; }
        private string _bankaccno;
        [Required(ErrorMessage = "Số tài khoản ngân hàng không được bỏ trống")]
        [StringLength(100, ErrorMessage = "Số tài khoản ngân hàng không được dài hơn 100 ký tự")]
        public string BankAccNo
        {
            get => _bankaccno;
            set => _bankaccno = value?.Trim();
        }

        private string _bankAccName;
        [Required(ErrorMessage = "Tên chủ ngân hàng không được bỏ trống")]
        [StringLength(128, ErrorMessage = "Số tài khoản ngân hàng không được dài hơn 128 ký tự")]
        public string BankAccName
        {
            get => _bankAccName;
            set => _bankAccName = value?.Trim();
        }

        private string _bankName;
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
        public string IsDefault { get; set; }
        public bool IsTemp { get; set; }
        public int? BusinessCustomerTempId { get; set; }
    }
}
