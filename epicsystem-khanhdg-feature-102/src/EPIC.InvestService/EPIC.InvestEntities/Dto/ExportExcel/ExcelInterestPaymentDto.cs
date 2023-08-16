using EPIC.InvestEntities.Dto.RenewalsRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    public class ExcelInterestPaymentDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Kỳ chi trả
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        public long OrderId { get; set; }
        public int? DistributionId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AmountMoney { get; set; }
        public string CifCode { get; set; }
        public string IdNo { get; set; }
        public int PolicyDetailId { get; set; }
        public int SoNgay { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Nếu là kỳ cuối thì sẽ là Tổng giá trị đang đầu tư (để lập chi + thêm tiền lãi để chi trả)
        /// </summary>
        public decimal? TotalValueInvestment { get; set; }

        /// <summary>
        /// Thuế TN
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Name { get; set; }

        public string Phone { get; set; }
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        public string PolicyDetailName { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }

        /// <summary>
        /// Trạng thái của hợp đồng
        /// </summary>
        public int? OrderStatus { get; set; }

        #region Đã lập chi trả mới có
        /// <summary>
        /// Trạng thái ngân hàng khi chi tiền
        /// </summary>
        public int? StatusBank { get; set; }

        /// <summary>
        /// Trạng thái của chi tiền
        /// </summary>
        public int? InterestPaymentStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveIp { get; set; }
        #endregion

        /// <summary>
        /// trường này check là kì cuối
        /// </summary>
        public string IsLastPeriod { get; set; }

        /// <summary>
        /// Nếu là kỳ cuối và có tồn tại yêu cầu tái tục rút vốn
        /// </summary>
        public InvRenewalsRequestDto RenewalsRequest { get; set; }

        /// <summary>
        /// Mã dự án - sản phẩm đầu tư
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Tên dự án - sản phẩm đầu tư
        /// </summary>
        public string InvName { get; set; }

        /// <summary>
        /// Tài khoản thụ hưởng
        /// </summary>
        public string CustomerBankAccount { get; set; }

        /// <summary>
        /// Ngân hàng thụ hưởng
        /// </summary>
        public string CustomerBank { get; set; }

        /// <summary>
        /// Chủ tài khoản thụ hưởng
        /// </summary>
        public string OwnerCustomerBankAccount { get; set; }

        /// <summary>
        /// Tiền nhận cuối kì
        /// </summary>
        public decimal ReceivePaymentLastPeriod { get; set; }

        /// <summary>
        /// Loại tất toán
        /// </summary>
        public string SettlementType { get; set; }

        /// <summary>
        /// Lợi nhuận hiển thị
        /// </summary>
        public decimal ProfitDisplay { get; set; }
    }
}
