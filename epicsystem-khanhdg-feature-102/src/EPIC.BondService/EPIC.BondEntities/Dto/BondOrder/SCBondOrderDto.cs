using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondEntities.Dto.BondPolicy;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.Entities.Dto.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondOrder
{
    public class SCBondOrderDto
    {
        public string CifCode { get; set; }
        public decimal? TotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }

        public string SaleReferralCode { get; set; }
        public int? Source { get; set; }
        public string ContractCode { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public decimal? Price { get; set; }
        public long? Quantity { get; set; }
        public int? Status { get; set; }
        public string DepartmentName { get; set; }
        public string DeliveryCode { get; set; }
        public int? DeliveryStatus { get; set; }

        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Mã sale bán hộ
        /// </summary>
        public string SaleReferralCodeSub { get; set; }

        public SCBondInfoDto BondInfo { get; set; }
        public SCBondSecondaryDto BondSecondary { get; set; }
        public SCBondPolicyDto BondPolicy { get; set; }
        public SCBondPolicyDetailDto BondPolicyDetail { get; set; }
        /// <summary>
        /// Thông tin phòng ban
        /// </summary>
        public SCDepartmentDto Department { get; set; }
    }
}
