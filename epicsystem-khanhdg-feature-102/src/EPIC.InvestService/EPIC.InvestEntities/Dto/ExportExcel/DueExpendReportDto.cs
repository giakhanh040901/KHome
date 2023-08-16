using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ExportExcel
{
    /// <summary>
    /// Dto của báo cáo dự chi đến hạn
    /// </summary>
    public class DueExpendReportDto
    {
        public int Stt { get; set; }

        /// <summary>
        /// Ngày đến hạn trả : ngày trả của kì hiện tại
        /// </summary>
        public DateTime? CurrentPeriodPaymentDate { get; set; }

        /// <summary>
        /// Ngày trả của kì trước
        /// </summary>
        public DateTime? PreviousPeriodPaymentDate { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public Decimal? ActualReceiveMoney { get; set; }
        /// <summary>
        /// Số hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Số giấy tờ tùy thân CCCD, CMND, Hộ chiếu
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ tùy thân CCCD, CMND, Hộ chiếu
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Mã dự án đầu tư
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư hiện tại
        /// </summary>
        public Decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Số tiền phải trả ở kì cuối : gốc + lợi nhuận (Gốc đến hạn trả)
        /// </summary>
        public Decimal LastPeriodPayment { get; set; }

        /// <summary>
        /// Lãi đến hạn trả
        /// </summary>
        public Decimal InterestDue { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public Decimal IncomeTax { get; set; }

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string BankAccName { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Tài khoản dự án : tài khoản của đại lý nhận tiền
        /// </summary>
        public string ProjectBankAccNo { get; set; }

        /// <summary>
        /// Kì hạn thứ bao nhiêu
        /// </summary>
        public int PeriodIndex { get; set; }

        public DateTime? InvestDate { get; set; }

        public string Status { get; set; }
        
        /// <summary>
        /// Thời hạn 
        /// </summary>
        public string PeriodTime { get; set; }

        /// <summary>
        /// Lợi tức
        /// </summary>
        public string Profit { get; set; }

        public DueExpendReportDto()
        {
        }

        public DueExpendReportDto(DueExpendReportDto input)
        {
            this.IdNo = input.IdNo;
            this.InterestDue = input.InterestDue;
            this.CurrentPeriodPaymentDate = input.CurrentPeriodPaymentDate;
            this.PreviousPeriodPaymentDate = input.PreviousPeriodPaymentDate;
            this.LastPeriodPayment = input.LastPeriodPayment;
            this.CurrentInvestment = input.CurrentInvestment;
            this.CustomerName = input.CustomerName;
            this.ContractCode = input.ContractCode;
            this.BankAccNo = input.BankAccNo;
            this.ProjectBankAccNo = input.ProjectBankAccNo;
            this.BankAccName = input.BankAccName;
            this.CifCode = input.CifCode;
            this.PeriodIndex = input.PeriodIndex;
            this.ProjectCode = input.ProjectCode;
            this.Stt = input.Stt;
            this.IdType = input.IdType;
            this.IncomeTax = input.IncomeTax;
            this.BankName = input.BankName;
            this.Status= input.Status;
            this.InvestDate = input.InvestDate;
            this.PeriodTime = input.PeriodTime;
            this.Profit = input.Profit;
        }
    }
}
