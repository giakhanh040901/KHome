using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class UpdateDefaultBankDto
    {
        private string _bankAccount { get; set; }
        private string _ownerAccount { get; set; }

        public int InvestorBankId { get; set; }
        public int BankId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số tài khoản không được để trống")]
        public string BankAccount { get => _bankAccount; set => _bankAccount = value?.Trim(); }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên chủ tài khoản không được để trống")]
        public string OwnerAccount { get => _ownerAccount; set => _ownerAccount = value?.Trim(); }
    }
}
