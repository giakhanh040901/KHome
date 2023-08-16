using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class ViewCalculateGarnerWithdrawalDto : CalculateGarnerWithdrawalDto
    {
        public List<string> OrderSources { get; set;}

        /// <summary>
        /// Danh sách lệnh, kèm theo danh sách mẫu hợp đồng rút
        /// </summary>
        public List<GarnerContractTemplatesWithdrawalDto> OrderContractFileWithdrawals { get; set; }
        /// <summary>
        /// Danh sách ngân hàng thụ hưởng của nhà đầu tư
        /// </summary>
        public List<BankAccountInfoDto> ListBanks { get; set; }
    }
}
