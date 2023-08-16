using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    public class CreateWithdrawalDto
    {
        public long? OrderId { get; set; }
        public decimal? AmountMoney { get; set; }
        public DateTime WithdrawalDate { get; set; }
    }
}
