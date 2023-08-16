using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class UserReportDto
    {
        /// <summary>
        /// Tên đăng nhập của user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Ngày tạo của user
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Mã giới thiệu của user
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        /// Bước Ekyc
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Ngày xác minh tài khoản bước cuối cùng
        /// </summary>
        public DateTime? FinalStepDate { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public string Status { get; set; }
    }
}
