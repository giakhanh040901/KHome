using EPIC.Entities.Dto.ExportReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.ExportExcel
{
    public class InterestPrincipalDueDto
    {
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Số giấy tờ tuỳ thân
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ tuỳ thân
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã trái phiếu
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// Loại trái phiếu
        /// </summary>
        public string BondType { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public string KyHan { get; set; }

        /// <summary>
        /// Tiền dự chi
        /// </summary>
        public Decimal ForecastMoney { get; set; }

        /// <summary>
        /// Thời gian kỳ hạn
        /// </summary>
        public string PeriodTime { get; set; }

        /// <summary>
        /// Phân loại
        /// </summary>
        public string ClassifyDisplay { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Chủ tài khoản đầu tư
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string CustomerBankName { get; set; }

        /// <summary>
        /// Số lượng trái phiếu
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public Decimal Profit { get; set; }
        
        /// <summary>
        /// Tổng giá trị hợp đồng
        /// </summary>
        public Decimal? TotalValue { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public int InterestPeriod { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public Decimal IncomeTax { get; set; }

        /// <summary>
        /// Ngày chi trả của kì trước
        /// </summary>
        public DateTime PreviousPeriodPaydate { get; set; }
    }
}
