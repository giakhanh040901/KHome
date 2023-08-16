using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerWithdrawalOrderDetailDto
    {
        public GarnerOrderDto GarnerOrder { get; set; }
        public decimal AmountMoney { get; set; }
        public decimal Profit { get; set; }
        public decimal DeductibleProfit { get; set; }
        public decimal Tax { get; set; }
        public decimal ActuallyProfit { get; set; }
        public decimal WithdrawalFee { get; set; }
        public decimal AmountReceived { get; set; }
    }
}
