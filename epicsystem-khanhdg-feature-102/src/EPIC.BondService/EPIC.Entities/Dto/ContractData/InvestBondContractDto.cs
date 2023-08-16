using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    public class InvestBondContractDto
    {
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Ngày lập hợp đồng
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Tên nhà đầu tư
        /// </summary>
        public string CustomerName { get; set; } 

        /// <summary>
        /// Số CMND
        /// </summary>
        public string CustomerIdNo { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string CustomerIdIssuer { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? CustomerIdDate { get; set; }

        /// <summary>
        /// Địa chỉ liên lạc
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Chủ tài khoản
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Tên ngân hàng full
        /// </summary>
        public string FullBankName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string BankBranch{ get; set; }

        /// <summary>
        /// Mã trái phiếu
        /// </summary>
        public string BondCode { get; set; }
        
        /// <summary>
        /// Tên tổ chức phát hành
        /// </summary>
        public string BusinessCustomerName { get; set; }

        /// <summary>
        /// Ngày phát hành
        /// </summary>
        public DateTime? IssueDate{ get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Mệnh giá
        /// </summary>
        public decimal? ParValue { get; set; }

        /// <summary>
        /// Mệnh giá (chữ)
        /// </summary>
        public string ParValueText { get; set; }

        /// <summary>
        /// Số lượng trái phiếu hợp tác
        /// </summary>
        public decimal? BondQuantity { get; set; }
        
        /// <summary>
        /// Giá trị mỗi trái phiếu
        /// </summary>
        public decimal? BondPrice { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        public decimal? InvestMoney { get; set; }

        /// <summary>
        /// Số tiền đầu tư (bằng chữ)
        /// </summary>
        public string InvestMoneyText { get; set; }

        /// <summary>
        /// Thời hạn đầu tư
        /// </summary>
        public DateTime? ContractEnd { get; set; }

        /// <summary>
        /// Lợi tức (lãi)
        /// </summary>
        public decimal? Interest { get; set; }

        /// <summary>
        /// Ngày chuyển tiền (ngày giao dịch)
        /// </summary>
        public DateTime? TranDate { get; set; }

        /// <summary>
        /// Nội dung giao dịch
        /// </summary>
        public string TranContent { get; set; }

        /// <summary>
        /// Thu nhập định kỳ
        /// </summary>
        public int? RecurringIncome { get; set; }

        /// <summary>
        /// thu nhập cuối kỳ
        /// </summary>
        public decimal? FinalIncome { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyClassifyName { get; set; }

        public ContractTradingProviderDto TradingProvider { get; set; } = new();

        public ContractDepositProviderDto DepositProvider { get; set; } = new();

        public ContractTradingProviderBankAccDto TradingProviderBankAcc { get; set; } = new();
        
    }
}
