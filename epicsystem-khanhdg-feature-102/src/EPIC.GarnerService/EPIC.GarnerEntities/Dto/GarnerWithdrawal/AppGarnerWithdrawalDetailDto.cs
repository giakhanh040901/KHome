using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class AppGarnerWithdrawalDetailDto
    {
        public long WithdrawalId { get; set; }

        public decimal AmountMoney { get; set; }

        public decimal WithdrawalFee { get; set; }

        public int Status { get; set; }

        public List<GarnerOrderWithdrawalDto> WithdrawalDetails { get; set; }
    }
}
