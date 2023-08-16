using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class CustomerHVFListExcelReportDto
    {
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng
        /// </summary>
        public string BankAcc { get; set; }

        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string BankAccHolder { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string BankBranch { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// Tên người giới thiệu
        /// </summary>
        public string Presenter { get; set; }

        /// <summary>
        /// Phòng ban
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Giá trị đã đầu tư
        /// </summary>
        public string InvestmentValue { get; set; }

        /// <summary>
        /// Email khách hàng
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// số điện thoại khách hàng
        /// </summary>
        public string Phone { get; set; }
    }
}
