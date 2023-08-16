using EPIC.GarnerSharedEntities.Dto;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    /// <summary>
    /// Chi tiết chi trả
    /// </summary>
    public class GarnerInterestPaymentDetailDto
    {
        public long Id { get; set; }

        public long InterestPaymentId { get; set; }

        public long OrderId { get; set; }

        /// <summary>
        /// Số tiền chi trả
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Thuế
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Phần trăm lợi nhuận
        /// </summary>
        public decimal ProfitRate { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Số tiền tích lũy hiện hữu
        /// </summary>
        public decimal ExistingAmount { get; set; }

        /// <summary>
        /// Số ngày đầu tư
        /// </summary>
        public int NumberOfDays { get; set; }

        /// <summary>
        /// Số tiền tích lũy
        /// </summary>
        public decimal InitTotalValue { get; set; }
        public GarnerOrderDto Order { get; set; }
    }
}
