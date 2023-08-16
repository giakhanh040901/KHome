using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerSharedEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    /// <summary>
    /// Lập danh sách đến ngày chi trả
    /// </summary>
    public class GarnerInterestPaymentSetUpDto
    {
        public int? Id { get; set; }
        
        public string CifCode { get; set; }

        /// <summary>
        /// Id hợp đồng
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Ngày tích lũy
        /// </summary>
        public DateTime InvestDate { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Số tiền tích lũy
        /// </summary>
        public decimal InitTotalValue { get; set; }

        /// <summary>
        /// Số tiền tích lũy hiện hữu
        /// </summary>
        public decimal ExistingAmount { get; set; }

        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Số ngày đầu tư
        /// </summary>
        public int InvestDays { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Thuế TN
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// % lợi nhuận
        /// </summary>
        public decimal ProfitRate { get; set; }

        /// <summary>
        /// Số ngày
        /// </summary>
        public int NumberOfDays { get; set; }
        public GarnerOrderDto Order { get; set; }
    }
}
