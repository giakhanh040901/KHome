using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Loại tài khoản: 1: Đăng ký ngay, 2: OTP sent, 3: Nhập OTP thành công, 4: Thêm giấy tờ thành công
    /// 5: Start eKYC, 6: eKYC thành công, 7: Thêm ngân hàng thành công, 8: Hoàn thành đăng ký
    /// </summary>
    public static class InvestorRegisterLogTypes
    {
        public const int RegisterNow  = 1;
        public const int OtpSent = 2;
        public const int SuccessfulOtp = 3;
        public const int SuccessfulIdentification = 4;
        public const int StartEkyc = 5;
        public const int SuccessfulEkyc = 6;
        public const int SuccessfulBank = 7;
        public const int CompleteRegistration = 8;
    }
}
