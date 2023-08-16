using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using System;
using System.Collections.Generic;

namespace EPIC.Entities.Dto.Order
{
    public class ViewOrderDto
    {
        public int OrderId { get; set; }
        public string CifCode { get; set; }
        public int TradingProviderId { get; set; }
        public int SecondaryId { get; set; }
        public int? DepartmentId { get; set; }
        public int PolicyId { get; set; }
        public int PolicyDetailId { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public string IsInterest { get; set; }
        public string SaleReferralCode { get; set; }
        public int BondId { get; set; }
        public int Source { get; set; }
        public int BusinessCustomerBankAccId { get; set; }
        public int InvestorBankAccId { get; set; }
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
        /// <summary>
        /// Ngày đầu tư
        /// </summary>
        public DateTime? InvestDate { get; set; }
        public string CustomerType { get; set; }
        public int? DeliveryStatus { get; set; }

        /// <summary>
        /// Giá bán lấy theo bảng giá của ngày thanh toán full
        /// </summary>
        public decimal? BuyPrice { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? Status { get; set; }
        public decimal? Price { get; set; }
        public long? Quantity { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public string ContractCode { get; set; }
        public string PaymentNote { get; set; }

        /// <summary>
        /// Phòng giao dịch quản lý hợp đồng
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Phòng giao dịch quản lý /Mà sale đang tham gia
        /// </summary>
        public string ManagerDepartmentName { get; set; }

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
        /// Id phong toả giải toả
        /// </summary>
        public int? BlockadeLiberationId { get; set; }

        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Loại kỳ hạn sau khi tái tục
        /// </summary>
        public int? RenewalsPolicyDetailId { get; set; }

        /// <summary>
        /// Id của yêu cầu tái tục nếu có
        /// </summary>
        public int? RenewalsRequestId { get; set; }
        /// <summary>
        /// Có yêu cầu chọn tái tục vốn hay không?
        /// </summary>
        public bool IsRenewalsRequest { get; set; }

        /// <summary>
        /// 1: Quản trị viên đặt; 2: Khách đặt; 3: Sale đặt
        /// </summary>
        public int? OrderSource { get; set; }

        public ViewProductBondSecondaryDto BondSecondary { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
        public BusinessCustomerBankDto TradingProviderBank { get; set; }
        public InvestorBankAccount InvestorBank { get; set; }
        public ProductBondInfoDto BondInfo { get; set; }
        public ViewProductBondPolicyDto BondPolicy { get; set; }
        public List<InvestorContactAddress> ListContactAddress { get; set; }
        public ViewProductBondPolicyDetailDto BondPolicyDetail { get; set; }
        public List<ViewProductBondPolicyDto> BondPolicies { get; set; }
        public ViewSaleDto Sale { get; set; }
    }
}
