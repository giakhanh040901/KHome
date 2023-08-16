using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.ManagerInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExportData
{
    public class ExportDataInvestorDto
    {
        /// <summary>
        /// Id Nhà đầu tư
        /// </summary>
        public int InvestorId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Họ tên  
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Quốc tịch
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? IdExpiredDate { get; set; }

        /// <summary>
        /// Nguyên quán
        /// </summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfResidence { get; set; }

        // Bảng investorContactAddress lấy mặc định
        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        public string ContactAddress { get; set; }

        // Bảng investorContactAddress lấy mặc định
        /// <summary>
        /// Tên ngân hàng tài khoản của khác hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Tài khoản App
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mã giới thiệu của khách hàng
        /// </summary>
        public string ReferralCodeSelf { get; set; }

        /// <summary>
        /// Mã giới thiệu Người giới thiệu
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// Ngày đăng ký tài khoản
        /// </summary>
        public DateTime? UserCreatedDate { get; set; }

        /// <summary>
        /// Ngày khóa tài khoản
        /// </summary>
        public DateTime? UserDeactiveDate { get; set; }

        /// <summary>
        /// Người tạo tài khoản
        /// </summary>
        public string UserCreatedBy { get; set; }

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public string UserStatus { get; set; }
    }
}
