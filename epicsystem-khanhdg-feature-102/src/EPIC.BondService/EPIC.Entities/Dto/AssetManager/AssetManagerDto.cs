using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.AssetManager
{
    public class AssetManagerDto
    {
        public int InvestorId { get; set; }
        public long ProductTotal { get; set; }
        public long InterestDay { get; set; }
        public DistributionAssetDto Distribution { get; set; }
    }

    public class TransectionHistoryDto
    {
        public int InvestorId { get; set; }
        public string Name { get; set; }

        public DateTime Time { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Tiền ra (Rút tiền)
        /// </summary>
        public string IsCashOutflows { get; set; }

        public int Status { get; set; }
    }

    /// <summary>
    /// DTO biến động
    /// </summary>
    public class FluctuationsDto
    {
        public int InvestorId { get; set; }
        public List<CashFlowsDto> CashOutflows { get; set; }
        public List<CashFlowsDto> CashInflows { get; set; }
    }

    public class CashFlowsDto 
    {
        public DateTime Time { get; set; }
        public long Amount { get; set; }
    }

    /// <summary>
    /// Sản phẩm đầu tư
    /// </summary>
    public class InvestProduct {
        public int InvestorId { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }

        /// <summary>
        /// dòng tiền
        /// </summary>
        public long CashFlows { get; set; }

        public double Interest { get; set; }
    }

    public class InvestBondDto
    {
        public int InvestorId { get; set; }
        public List<InvestDto> Invests { get; set; }
        public List<OrderDto> Orders { get; set; }
        public List<HistoryDto> Historys { get; set; }
    }

    public class InvestDto
    {
        public long Amount { get; set; }
        public string Name { get; set; }
        public double Interest { get; set; }
        /// <summary>
        /// Sản phẩm
        /// </summary>
        public int Classify { get; set; }

        /// <summary>
        /// Ngày tất toán
        /// </summary>
        public DateTime FinalSettlementDay { get; set; }

        /// <summary>
        /// Thòi gian còn lại (ngày)
        /// </summary>
        public int RemainTime { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
        public int Classify { get; set; }
        public double Interest { get; set; }
        public int Status { get; set; }
    }

    public class HistoryDto
    {
        public string Name { get; set; }
        public long Amount { get; set; }
        public int Classify { get; set; }
        public double Interest { get; set; }
        public double Profit { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// Ngày tất toán
        /// </summary>
        public DateTime FinalSettlementDay { get; set; }
    }

    public class Contract
    {
        public string ContractName { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string LinkPDF { get; set; }
    }

    /// <summary>
    /// DTO đặt lệnh
    /// </summary>
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        /// <summary>
        /// Trạng thái đầu tư
        /// </summary>
        public int InvestStatus { get; set; }
        public long InvestMoney { get; set; }
        public long InterestDay { get; set; }
        public InvestOrderDto InvestOrder { get; set; }

        public Promotion Promotion { get; set; }
        public Contract Contract { get; set; }
        public BankAccountDto BankAccount { get; set; }

        public MoreInformationDto MoreInformation { get; set; }

    }

    /// <summary>
    /// DTO Ưu đãi
    /// </summary>
    public class Promotion
    {
        public string Title { get; set; }
        public long MoneyRequired { get; set; }
    }

    public class InvestOrderDto
    {
        /// <summary>
        /// Tiền đầu tư
        /// </summary>
        public long InvestMoney { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int InvestQuantity { get; set; }

        /// <summary>
        /// Tên đầu tư
        /// </summary>
        public string BondName { get; set; }

        /// <summary>
        /// lợi nhuận
        /// </summary>
        public double Profit { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public double Interest { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Đơn vị kỳ hạn
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public int Classify { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        public long ParValue { get; set; }
        /// <summary>
        /// chu kỳ nhận lợi tức
        /// </summary>
        public int InterestType { get; set; }

        /// <summary>
        /// Tổng thu nhập
        /// </summary>
        public double IncomeTotal { get; set; }

        public DateTime IssuerDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    /// <summary>
    /// Tài khoản hưởng thụ
    /// </summary>
    public class BankAccountDto
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Owner { get; set; }
    }

    /// <summary>
    /// Giấy tờ tùy thân
    /// </summary>
    public class IdentificationDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    /// <summary>
    /// Mã Giới thiệu
    /// </summary>
    public class ReferralCodeDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    /// <summary>
    /// Địa chỉ
    /// </summary>
    public class AddressDto
    {
        public string Alias { get; set; }
        public string Address { get; set; }
    }

    public class MoreInformationDto
    {
        public IdentificationDto Identification { get; set; }
        public ReferralCodeDto ReferralCode { get; set; }
        public AddressDto Address { get; set; }

    }

    public class TransectionDto
    {
        public string Name { get; set; }

        public DateTime Time { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Tiền ra (Rút tiền)
        /// </summary>
        public string IsCashOutflows { get; set; }

        public int Status { get; set; }
    }
}
