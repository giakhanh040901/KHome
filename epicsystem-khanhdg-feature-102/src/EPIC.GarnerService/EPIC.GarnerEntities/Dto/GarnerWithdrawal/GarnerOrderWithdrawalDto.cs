using EPIC.GarnerEntities.DataEntities;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerOrderWithdrawalDto
    {
        public EPIC.GarnerEntities.DataEntities.GarnerOrder Order { get; set; }

        /// <summary>
        /// Số tiền rút
        /// </summary>
        public decimal AmountMoney { get; set; }
        public decimal Profit { get; set; }
        public decimal DeductibleProfit { get; set; }
        public decimal Tax { get; set; }
        public decimal ActuallyProfit { get; set; }
        public decimal WithdrawalFee { get; set; }
        public decimal AmountReceived { get; set; }
    }
}
