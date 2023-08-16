using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.AppOrder
{
    public class ThoiGianChiTraThucDto
    {
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Ngày của kỳ hiện tại
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Ngày của kỳ trước đấy
        /// </summary>
        public DateTime LastPayDate { get; set; }

        /// <summary>
        /// Kỳ thứ bao nhiêu
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// Id của sổ lệnh
        /// </summary>
        public long OrderId { get; set; }
        public int PolicyDetailId { get; set; }
        public int SoNgay { get; set; }

        /// <summary>
        /// Mã hợp đồng 
        /// </summary>
        public string ContractCode { get; set; }
        public string PolicyDetailName { get; set; }

        /// <summary>
        /// Đơn vị kì hạn
        /// </summary>
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal? Profit { get; set; }
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Id giấy tờ của investor
        /// </summary>
        public int? InvestorIdenId { get; set; }
        public int? PolicyId { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư của sổ lệnh
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Trạng thái của hợp đồng
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// check là kì cuối
        /// </summary>
        public bool IsLastPeriod { get; set; }
    }
}
