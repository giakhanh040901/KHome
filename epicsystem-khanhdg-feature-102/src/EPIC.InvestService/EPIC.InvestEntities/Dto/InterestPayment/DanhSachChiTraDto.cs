using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class DanhSachChiTraDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Kỳ chi trả
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// OrderId
        /// </summary>
        public long OrderId { get; set; }
        public int? DistributionId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Tên dự án - sản phẩm đầu tư
        /// </summary>
        public string InvName { get; set; }

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
        /// Số tiền hợp đồng đang đầu tư tại thời điểm chi
        /// </summary>
        public decimal TotalValueCurrent { get; set; }
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

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Mã hợp đồng trong file
        /// </summary>
        public string GenContractCode { get; set; }
        /// <summary>
        /// Loại tính thuế chính sách 1: NET, 2: GROSS
        /// </summary>
        public decimal? PolicyCalculateType { get; set; }

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
        #endregion

        /// <summary>
        /// trường này check là kì cuối
        /// </summary>
        public string IsLastPeriod { get; set; }

        /// <summary>
        /// Có thể tái tục hợp đồng cuối kỳ hay không? (Y/N)
        /// </summary>
        public string CanRenewalOrder { get; set; }

        /// <summary>
        /// Nếu là kỳ cuối và có tồn tại yêu cầu tái tục rút vốn
        /// </summary>
        public InvRenewalsRequestDto RenewalsRequest { get; set; }

        /// <summary>
        /// Ngân hàng thụ hưởng của khách hàng nếu là khách hàng doanh nghiệp
        /// </summary>
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }

        /// <summary>
        /// Ngân hàng thụ hưởng của khách hàng nếu là khách hàng cá nhân
        /// </summary>
        public BankAccountInfoDto InvestorBank { get; set; }

        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn (1: có chi tiền, 2: không chi tiền)
        /// </summary>
        public int MethodInterest { get; set; }
    }
}
