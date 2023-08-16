using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestShared
{
    public class ThoiGianChiTraDto
    {
        public List<ThoiGianTraLoiNhuanDto> ThoiGianTraLoiNhuan { get; set; }
        public List<ThoiGianKetThucGiaDinhDto> ThoiGianKetThucGiaDinh { get; set; }
    }

    /// <summary>
    /// Dòng thời gian Chi trả lợi nhuận không kèm trường hợp rút vốn
    /// </summary>
    public class ThoiGianTraLoiNhuanDto
    {
        /// <summary>
        /// Ngày trả lợi nhuận
        /// </summary>
        public DateTime? ThoiGianTraLoiNhuan { get; set; }

        /// <summary>
        /// Lợi nhuận trả lãi
        /// </summary>
        public decimal? LoiNhuanTraLai { get; set; }

        /// <summary>
        /// Số ngày tính từ ngày bắt đầu tính lãi
        /// </summary>
        public int? SoNgay { get; set; }
    }

    /// <summary>
    /// Dòng thời gian kết thúc giả định được lên theo các kỳ hạn trong chính sách đang được sử dụng
    /// Mỗi kỳ hạn trong Chính sách ứng với ngày kết thúc tương ứng tính từ ngày bắt đầu tĩnh lãi
    /// </summary>
    public class ThoiGianKetThucGiaDinhDto
    {
        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public int PolicyDetailId { get; set; }

        /// <summary>
        /// Thời gian kết thúc giả định
        /// </summary>
        public DateTime? ThoiGianKetThuc { get; set; }

        /// <summary>
        /// Số ngày tính từ ngày bắt đầu tính lãi
        /// </summary>
        public int? SoNgay { get; set; }
    }

    public class RutVonDto
    {
        /// <summary>
        /// Số ngày đã đầu tư (1)
        /// </summary>
        public int? NumberOfDaysInvested { get; set; }

        /// <summary>
        /// Ngày rút vốn
        /// </summary>
        public DateTime? WithdrawlDate { get; set; }

        /// <summary>
        /// Số tiền rút (2)
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Lợi tức (lãi suất) , tỷ lệ lợi nhuận
        /// </summary>
        public decimal? ProfitRate { get; set; }

        /// <summary>
        /// Lợi nhuận chi trả khi rút vốn (4) = (1) * (2) * (3) / 365
        /// </summary>
        public decimal? WithdrawalProfit { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân (5)
        /// </summary>
        public decimal? IncomeTax { get; set; }

        /// <summary>
        /// Chi phí rút vốn (6)
        /// </summary>
        public decimal? WithdrawalFee { get; set; }

        /// <summary>
        /// % rút vốn
        /// </summary>
        public decimal? ExitFee { get; set; }

        /// <summary>
        /// Loại phí rút vốn ( 1: Theo số tiền rút, 2: theo năm)
        /// </summary>
        public decimal? ExitFeeType { get; set; }

        /// <summary>
        /// Lợi nhuận đã nhận hoặc Lợi nhuận khấu trừ (7)
        /// </summary>
        public decimal? ProfitReceived { get; set; }

        /// <summary>
        /// Số tiền thực nhận (8) = (2) + (4) - (5) - (6) - (7)
        /// </summary>
        public decimal ActuallyAmount { get; set; }
        
        /// <summary>
        /// Lợi nhuận thực nhận
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Số dư sau khi rút
        /// </summary>
        public decimal? Surplus { get; set; }

        /// <summary>
        /// Rút vốn lần thứ bao nhiêu
        /// </summary>
        public int WithdrawalIndex { get; set; }
    }
}
