using EPIC.CoreEntities.Dto.Sale;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class RstOrderMoreInfoDto : RstOrderDto
    {
        /// <summary>
        /// Hết thời gian giữ cọc khi Order ở khởi tạo hoặc chờ thanh toán hay chưa
        /// </summary>
        public bool TimeOutDeposit { get; set; }
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
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public RstProductItemDto ProductItem { get; set; }
        public RstDistributionPolicyDto DistributionPolicy { get; set; }
        public ViewSaleDto Sale { get; set; }
        public int? Orderer { get; set; }
        public List<RstOrderMoreInfoDto> ListOrder { get; set; }
        
        /// <summary>
        /// Loại hợp đồng (1: Sở hữu, 2: Đồng sở hữu)
        /// </summary>
        public int? OrderType { get; set; }
        public RstProjectDto Project { get; set; }
        public int? OpenSellId { get; set; }
        public int? KeepTime { get; set; }
        /// <summary>
        /// Ngày đặt cọc
        /// </summary>
        public DateTime? DepositDate { get; set; }
        /// <summary>
        /// Ngày đặt lệnh
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        public RstProductItemPriceDto ProductItemPrice { get; set; }
        public List<RstOrderCoOwnerDto> RstOrderCoOwner { get; set; }

        /// <summary>
        /// Thông tin ngân hàng nhận tiền của mở bán
        /// </summary>
        public List<RstOpenSellBankDto> OrderPaymentBanks { get; set; }

        /// <summary>
        /// Thông tin ngân hàng thanh toán lần đầu tiên
        /// </summary>
        public RstOrderPaymentBankDto OrderPaymentFirstBank { get; set; }

        /// <summary>
        /// Thông tin sale đặt hộ cho khách hàng
        /// </summary>
        public SaleInfoDetailDto SaleOrder { get; set; }
    }
}
