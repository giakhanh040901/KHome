using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class CustomerRootListDto
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Họ và tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Mã giới thiệu của khách hàng nhập khi đăng kí
        /// </summary>
        public string ReferralCode { get;set; }

        /// <summary>
        /// Trạng thái xác minh
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Nguồn tạo
        /// </summary>
        public string Source { get;set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; } 

        /// <summary>
        /// Xác minh EPIC
        /// </summary>
        public string EpicIsConfirm { get; set; }

        /// <summary>
        /// Thông tin NDTCN
        /// </summary>
        public string IsProf { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Ngày phát hành
        /// </summary>
        public DateTime? IssueDate { get;set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? IdExpireDate { get; set; }

        /// <summary>
        /// Nguyên quán
        /// </summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfResidence { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Ngân hàng
        /// </summary>
        public string CustomerBankName { get; set; }
        /// <summary>
        /// Điện thoại
        /// </summary>
        public string Phone { get; set; }
    }
}
