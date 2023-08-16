using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    /// <summary>
    /// màn Quản lý đầu tư
    /// </summary>
    public class InvestManagerDto
    {
        /// <summary>
        /// Tổng lợi tức nhận được
        /// </summary>
        public decimal AllProfit { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal TotalValue { get; set; }
        public AssetManagerBondDto AssetManagerBond { get; set; }
        public AssetManagerInvestDto AssetManagerInvest { get; set; }
        public AssetManagerStockDto AssetManagerStock { get; set; }
        public AssetManagerDepositDto AssetManagerDeposit { get; set; }

        /// <summary>
        /// Đầu tư tích lũy
        /// </summary>
        public AssetManagerGarnerDto AssetManagerGarner { get; set; }
    }

    /// <summary>
    /// Đầu tư trái phiếu
    /// </summary>
    public class AssetManagerBondDto
    {
        /// <summary>
        /// Tổng giá trị
        /// </summary>
        public decimal TotalValue { get; set; }
        /// <summary>
        /// Lợi tức tính đến thời điểm hiện tại
        /// </summary>
        public decimal ProfitNow { get; set; }
    }

    /// <summary>
    /// Đầu tư bất động sản
    /// </summary>
    public class AssetManagerInvestDto
    {
        public decimal TotalValue { get; set; }
        public decimal ProfitNow { get; set; }
    }

    /// <summary>
    /// Đầu tư tích lũy
    /// </summary>
    public class AssetManagerGarnerDto
    {
        public decimal TotalValue { get; set; }
        public decimal ProfitNow { get; set; }
    }

    /// <summary>
    /// Đầu tư chứng khoán
    /// </summary>
    public class AssetManagerStockDto
    {
        public decimal TotalValue { get; set; }
        public decimal ProfitNow { get; set; }
    }

    /// <summary>
    /// Chứng chỉ tiền gửi
    /// </summary>
    public class AssetManagerDepositDto
    {
        public decimal TotalValue { get; set; }
        public decimal ProfitNow { get; set; }
    }
}
