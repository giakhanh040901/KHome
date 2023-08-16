using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class PrepareApproveRequestWithdrawalDto
    {
        public int? TradingBankAccId { get; set; }
        public List<long> WithdrawalIds { get; set; }
    }
}
