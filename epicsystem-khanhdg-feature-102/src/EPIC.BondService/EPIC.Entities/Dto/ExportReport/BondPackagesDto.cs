using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ExportReport
{
    public class BondPackagesDto
    {
        /// <summary>
        /// Mã TP
        /// </summary>
        public string BondCode { get; set; }
        /// <summary>
        /// Tổ chức phát hành
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ngày phát hành
        /// </summary>
        public DateTime? IssueDate { get; set; }
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Ngày mua sơ cấp
        /// </summary>
        public DateTime? DateBuy { get; set; }
        /// <summary>
        /// Tổng số lượng trái phiếu mua sơ cấp
        /// </summary>
        public decimal? PrimaryPurchaseTotal { get; set; }
        /// <summary>
        /// Tổng số tiền mua sơ cấp
        /// </summary>
        public decimal? PrimaryPurchaseTotalValue { get; set; }
        /// <summary>
        /// Tổng số lượng bán trái phiếu PNOTE
        /// </summary>
        public decimal PNOTESaleTotal { get; set; }
        /// <summary>
        /// Tổng số lượng bán trái phiếu PRO hoặc PROA
        /// </summary>
        public decimal? PROSaleTotal { get; set; }
        /// <summary>
        /// Tổng số tiền đã bán
        /// </summary>
        public decimal? SaleTotal { get; set; }
        /// <summary>
        /// Tổng số lượng trái phiếu đã mua lại khách hàng
        /// </summary>
        public decimal? CustomerPurchaseTotal { get; set; }
        /// <summary>
        /// Tổng số tiền mua lại khách hàng
        /// </summary>
        public decimal? CustomerPurchaseTotalValue { get; set; }
        /// <summary>
        /// Tổng số lượng trái phiếu còn lại 
        /// </summary>
        public decimal? RemainTotal { get; set; }
        /// <summary>
        /// Tổng giá trị trái phiếu còn lại
        /// </summary>
        public decimal? RemainTotalValue { get; set; }
    }
}
