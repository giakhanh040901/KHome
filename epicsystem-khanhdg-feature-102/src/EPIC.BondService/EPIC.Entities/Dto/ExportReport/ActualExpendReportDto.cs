using System;

namespace EPIC.Entities.Dto.ExportReport
{
    public class ActualExpendReportDto
    {
        /// <summary>
        /// Ngày chi
        /// </summary>
        public DateTime? ExpendDate { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã dự án 
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal TotalInvestment { get; set; }

        /// <summary>
        /// Giá trị đầu tư hiện tại
        /// </summary>
        public decimal CurrentInvestment { get; set; }

        /// <summary>
        /// Loại chi
        /// </summary>
        public string ExpendType { get; set; }

        /// <summary>
        /// Giá trị rút vốn, tất toán
        /// </summary>
        public decimal WithdrawalSettlement { get; set; }

        /// <summary>
        /// Số tiền chi
        /// </summary>
        public decimal ExpendAmount { get; set; }

        /// <summary>
        /// Tiền lãi
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Lợi nhuận khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }

        /// <summary>
        /// Số tài khoản đại lý nhận tiền
        /// </summary>
        public string ProjectBankAccount { get; set; }

        /// <summary>
        /// Số tiền gốc
        /// </summary>
        public decimal PrincipalAmount { get; set; }

        /// <summary>
        /// Giá trị đầu tư theo hợp đồng
        /// </summary>
        public decimal InitTotalValue { get; set; }
        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal ReceivedMoney { get; set; }
        public ActualExpendReportDto() { }

        public ActualExpendReportDto(ActualExpendReportDto input)
        {
            ExpendDate = input.ExpendDate;
            CustomerName = input.CustomerName;
            IdType = input.IdType;
            IdNo = input.IdNo;
            CifCode = input.CifCode;
            ProjectCode = input.ProjectCode;
            TotalInvestment = input.TotalInvestment;
            CurrentInvestment = input.CurrentInvestment;
            ExpendType = input.ExpendType;
            WithdrawalSettlement = input.WithdrawalSettlement;
            ExpendAmount = input.ExpendAmount;
            Interest = input.Interest;
            IncomeTax = input.IncomeTax;
            ContractCode = input.ContractCode;
            DeductibleProfit = input.DeductibleProfit;
            ProjectBankAccount = input.ProjectBankAccount;
            PrincipalAmount = input.PrincipalAmount;
            ReceivedMoney = input.ReceivedMoney;
            InitTotalValue = input.InitTotalValue;
        }
    }
}
