using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class CustomerListExcelReportDto
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        
        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Ngày cấp giấy tờ
        /// </summary>
        public DateTime? IdDate { get; set; }

        /// <summary>
        /// Ngày hết hạn giấy tờ
        /// </summary>
        public DateTime? IdExpiredDate { get; set; }

        /// <summary>
        /// Quê quán
        /// </summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfResidence { get; set; }

        /// <summary>
        /// Công ty chứng khoán
        /// </summary>
        public string SecurityCompany { get; set; }
        
        /// <summary>
        /// Công ty chứng khoán
        /// </summary>
        public string StockTradingAccount { get; set; }

        /// <summary>
        /// Nguồn tạo
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Trạng thái : Đã xác minh/ Chưa xác minh
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Thông tin nhà đầu như chuyên nghiệp
        /// </summary>
        public string IsProf { get; set; }

        public string BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

    }
}
