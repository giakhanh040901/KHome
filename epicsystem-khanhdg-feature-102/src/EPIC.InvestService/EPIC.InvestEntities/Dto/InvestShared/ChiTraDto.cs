using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestShared
{
    public class ThoiGianChiTraThucDto
    { 
        /// <summary>
        /// Ngày của kỳ hiện tại
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Ngày của kỳ trước đấy
        /// </summary>
        public DateTime LastPayDate { get; set; }

        /// <summary>
        /// Ngày lập chi của kỳ trước đấy
        /// </summary>
        public DateTime? InvestLastPayDate { get; set; }

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

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal? Profit { get; set; }
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        public int? PolicyId { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư của sổ lệnh
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// check là kì cuối
        /// </summary>
        public bool IsLastPeriod { get; set; }

        /// <summary>
        /// Ngày đáo hạn của hợp đồng
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
    }
}
