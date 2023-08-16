using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerSharedEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    /// <summary>
    /// Thêm thông tin chi tiết
    /// </summary>
    public class GarnerOrderMoreInfoDto : GarnerOrderDto
    {
        /// <summary>
        /// Phòng giao dịch quản lý hợp đồng
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Phòng giao dịch quản lý /Mà sale đang tham gia
        /// </summary>
        public string ManagerDepartmentName { get; set; }
        /// <summary>
        /// 1: Quản trị viên đặt; 2: Khách đặt; 3: Sale đặt
        /// </summary>
        public int? OrderSource { get; set; }
        public new DateTime? SettlementDate { get; set; }
        public decimal? InitTotalValueGroup { get; set; }
        public decimal? TotalValueGroup { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public GarnerProductDto Product { get; set; }
        public GarnerPolicyMoreInfoDto Policy { get; set; }
        public GarnerPolicyDetailDto PolicyDetail { get; set; }
        public ViewSaleDto Sale { get; set; }
        public FirstPaymentBankDto FirstPaymentBankDto { get; set; }
        public string IsFirstPayment { get; set; }
        public int? Orderer { get; set; }
        public List<GarnerOrderMoreInfoDto> ListOrder { get; set; }
    }
}
