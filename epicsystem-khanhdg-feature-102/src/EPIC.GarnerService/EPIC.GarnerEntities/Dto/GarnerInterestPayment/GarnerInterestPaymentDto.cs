using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    /// <summary>
    /// Thông tin chi trả
    /// </summary>
    public class GarnerInterestPaymentDto
    {
        public long Id { get; set; }

        public int PolicyId { get; set; }

        public string CifCode { get; set; }

        public decimal AmountMoney { get; set; }

        public DateTime PayDate { get; set; }

        public int Status { get; set; }

        public int? StatusBank { get; set; }

        public string ApproveBy { get; set; }

        public DateTime? ApproveDate { get; set; }

        public string ApproveIp { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal ActuallyProfit { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal AllTotalValue { get; set; }

        /// <summary>
        /// Tổng thuế TNCN
        /// </summary>
        public decimal Tax { get; set; }
        public List<GarnerInterestPaymentDetailDto> Details { get; set; }
        public GarnerPolicyDto Policy { get; set; }
        public GarnerProductDto Product { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public InvestorDto Investor { get; set; }
    }
}
