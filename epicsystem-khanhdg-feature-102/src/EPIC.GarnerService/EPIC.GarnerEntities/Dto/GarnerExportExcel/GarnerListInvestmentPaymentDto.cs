using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerExportExcel
{
    public class GarnerListInvestmentPaymentDto
    {

        /// <summary>
        /// Ngay chi tra
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        public decimal AmountMoney { get; set; }

        public int TradingProviderId { get; set; }
    }
}
