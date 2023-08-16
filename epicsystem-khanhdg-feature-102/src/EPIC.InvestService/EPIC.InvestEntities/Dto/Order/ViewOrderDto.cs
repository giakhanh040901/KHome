using EPIC.CoreEntities.Dto.Sale;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class ViewOrderDto
    {
        public long Id { get; set; }
        public int? TradingProviderId { get; set; }
        public string CifCode { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectId { get; set; }
        public int? DistributionId { get; set; }
        public int? PolicyId { get; set; }
        public int? PolicyDetailId { get; set; }
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal InitTotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }
        public string SaleReferralCode { get; set; }
        public int? Source { get; set; }
        public string ContractCode { get; set; }
        public string GenContractCode { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public DateTime? PaymentFullDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PendingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public string PendingDateModifiedBy { get; set; }
        public string DeliveryDateModifiedBy { get; set; }
        public string ReceivedDateModifiedBy { get; set; }
        public string FinishedDateModifiedBy { get; set; }
        public DateTime? SettlementDate { get; set; }


        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public int? Status { get; set; }
        public string PaymentNote { get; set; }
        public int? DeliveryStatus { get; set; }
        public string CustomerType { get; set; }
        public DateTime? RequestContractDate { get; set; }

        /// <summary>
        /// Id sale đặt lệnh hộ
        /// </summary>
        public int? SaleOrderId { get; set; }

        /// <summary>
        /// Phòng ban bán hộ
        /// </summary>
        public int? DepartmentIdSub { get; set; }

        /// <summary>
        /// Mã sale bán hộ
        /// </summary>
        public string SaleReferralCodeSub { get; set; }

        /// <summary>
        /// Tên Phòng ban bán hộ
        /// </summary>
        public string DepartmentNameSub { get; set; }

        /// <summary>
        /// Tên sale bán hộ
        /// </summary>
        public string SaleSubName { get; set; }

        /// <summary>
        /// Id phong toả giải toả
        /// </summary>
        public int? BlockadeLiberationId { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }
        /// <summary>
        /// Hình thức chi trả lợi tức/ đáo hạn
        /// </summary>
        public int? MethodInterest { get; set; }

        /// <summary>
        /// Loại kỳ hạn sau khi tái tục
        /// </summary>
        public int? RenewalsPolicyDetailId { get; set; }

        /// <summary>
        /// Có yên cầu thay đổi phương thức tái tục hay không?
        /// </summary>
        public bool IsRenewalsRequest { get; set; }

        /// <summary>
        /// Có yêu cầu rút vốn hay không?
        /// </summary>
        public bool IsWithdrawalRequest { get; set; }

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

        /// <summary>
        /// Hợp đồng cũ
        /// </summary>
        public int? RenewalsReferId { get; set; }

        /// <summary>
        /// Hợp đồng gốc
        /// </summary>
        public int? RenewalsReferIdOriginal { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        public string CustomerName { get; set; }

        /// <summary>
        /// Mã hợp đồng gốc nếu đã tái tục
        /// </summary>
        public string ContractCodeOriginal { get; set; }

        /// <summary>
        /// Loại hợp đồng tái tục của chính sách: 1: Tạo hợp đồng mới, 2 giữ hợp đồng cũ
        /// </summary>
        public int RenewalsType { get; set; }
        public ProjectDto Project { get; set; }
        public InvestorDto Investor { get; set; }
        public List<InvestorContactAddress> ListContactAddress { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public ViewPolicyDto Policy { get; set; }
        public ViewPolicyDetailDto PolicyDetail { get; set; }
        public List<ViewPolicyDto> BondPolicies { get; set; }
        public BusinessCustomerBankDto TradingProviderBank { get; set; }
        public string IsFirstPayment { get; set; }
        public ViewDistributionDto Distribution { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }

        /// <summary>
        /// Thông tin Sale theo mã giới thiệu
        /// </summary>
        public ViewSaleDto Sale { get; set; }

        /// <summary>
        /// Thông tin sale đặt hộ cho khách hàng
        /// </summary>
        public SaleInfoDetailDto SaleOrder { get; set; }

        /// <summary>
        /// Thông tin Investor đang dùng hợp đồng này
        /// </summary>
        public int? InvestorId { get; set; }
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Trạng thái của Lịch sử đầu tư: 1: Tất toán đúng hạn, 2: Tất toán trước hạn, 3: Tái tục gốc, 4: Tái tục gốc và lợi tức
        /// </summary>
        public int? InvestHistoryStatus { get; set; }
    }
}
