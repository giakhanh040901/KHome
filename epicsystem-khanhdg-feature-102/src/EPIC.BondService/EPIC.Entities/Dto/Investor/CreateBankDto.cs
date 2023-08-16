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
    public class CreateBankDto
    {
        private string _bankAccount;
        private string _ownerAccount;

        public int InvestorId { get; set; }
        public int BankId { get; set; }

        [Required(ErrorMessage = "Số tài khoản không được để trống", AllowEmptyStrings = false)]
        public string BankAccount { get => _bankAccount; set => _bankAccount = value?.Trim(); }

        [Required(ErrorMessage = "Chủ tài khoản không được để trống", AllowEmptyStrings = false)]
        public string OwnerAccount { get => _ownerAccount; set => _ownerAccount = value?.Trim(); }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsDefault { get; set; }
    }
}
