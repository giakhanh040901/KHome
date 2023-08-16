using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    /// <summary>
    /// Kết quả yêu cầu chi theo lô
    /// </summary>
    public class ResultRequestPayDto
    {
        /// <summary>
        /// Có phải gửi otp không?
        /// </summary>
        public bool IsSubmitOtp { get; set; }
        /// <summary>
        /// Lỗi cụ thể từng request detail nếu có
        /// </summary>
        public List<ResultRequestPayDetailDto> ErrorDetails { get; set; } = new();
    }

    /// <summary>
    /// Chi tiết lỗi yêu cầu chi nếu có, mô ta lỗi từng tài khoản trong một lô chi
    /// </summary>
    public class ResultRequestPayDetailDto
    {
        /// <summary>
        /// Request detail (từng tài khoản nhận tiền)
        /// </summary>
        public long DetailId { get; set; }
        /// <summary>
        /// Lỗi nếu có
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
