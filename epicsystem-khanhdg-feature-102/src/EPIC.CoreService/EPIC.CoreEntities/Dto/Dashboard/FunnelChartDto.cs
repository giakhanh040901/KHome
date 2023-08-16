using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class FunnelChartDto
    {
        /// <summary>
        /// Đăng ký ngay
        /// </summary>
        public int RegisterNow { get; set; }
        /// <summary>
        /// OTP sent
        /// </summary>
        public int OtpSent { get; set; }
        /// <summary>
        /// Nhập OTP thành công
        /// </summary>
        public int SuccessfulOtp { get; set; }
        /// <summary>
        /// Thêm giấy tờ thành công
        /// </summary>
        public int SuccessfulIdentification { get; set; }
        public int StartEkyc { get; set; }
        /// <summary>
        /// eKYC thành công
        /// </summary>
        public int SuccessfulEkyc { get; set; }
        /// <summary>
        /// Thêm ngân hàng thành công
        /// 
        /// </summary>
        public int SuccessfulBank { get; set; }
        /// <summary>
        /// Hoàn thành đăng ký
        /// </summary>
        public int CompleteRegistration { get; set; }
    }
}
