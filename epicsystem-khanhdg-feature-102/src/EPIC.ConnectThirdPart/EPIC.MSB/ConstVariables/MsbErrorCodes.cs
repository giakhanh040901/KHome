using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.ConstVariables
{
    /// <summary>
    /// Mã lỗi Msb
    /// </summary>
    public static class MsbErrorCodes
    {
        /// <summary>
        /// Thành công
        /// </summary>
        public const string Success = "0";
        /// <summary>
        /// Request Data không đúng định dạng
        /// </summary>
        public const string RequestDataIsNotCorrectFormat = "1";
        /// <summary>
        /// Thông tin merchant không tồn tại
        /// </summary>
        public const string MerchentIsNotExists = "2";
        /// <summary>
        /// Lỗi check sum
        /// </summary>
        public const string ChecksumFail = "3";
        /// <summary>
        /// Id yêu cầu chưa qua được bước validate nên không tìm thấy
        /// </summary>
        public const string RequestNotFound = "4";
        /// <summary>
        /// Yêu cầu bị lặp lại
        /// </summary>
        public const string DuplicateRequestId = "5";
        /// <summary>
        /// Lỗi valid request khi gọi api validate
        /// </summary>
        public const string ValidRequestError = "6";
        /// <summary>
        /// Merchant chưa được cấu hình dịch vụ chi hộ
        /// </summary>
        public const string MerchantIsNotConfigured = "7";
        /// <summary>
        /// Số dư không đủ để thực hiện giao dịch
        /// </summary>
        public const string AmountExceedsLimit = "8";
        /// <summary>
        /// Trùng id giao dịch trong lô
        /// </summary>
        public const string DuplicateTransId = "9";
        /// <summary>
        /// Số giao dịch không hợp lệ
        /// </summary>
        public const string InvalidTransactionNumber = "10";
        /// <summary>
        /// Submit thành công yêu cầu xác thực thêm otp
        /// </summary>
        public const string SubmitRequestTransferSuccessfully = "11";
        /// <summary>
        /// Otp hết hạn
        /// </summary>
        public const string ExpOtp = "12";
        /// <summary>
        /// Nhâp OTP quá số lần cho phép (5 lần)
        /// </summary>
        public const string OTPTooManyTimesAllowed = "13";
        /// <summary>
        /// Otp không chính xác
        /// </summary>
        public const string ErrorOtp = "14";
        /// <summary>
        /// Số tài khoản không tồn tại hoặc đã bị khóa
        /// </summary>
        public const string AccountIsNotExitsOrBlocked = "15";
        /// <summary>
        /// Số tài khoản chi không hợp lệ
        /// </summary>
        public const string InvalidSpendingAccountNumber = "16";
        /// <summary>
        /// Mã VA Code đã tồn tại
        /// </summary>
        public const string VACodeIsExists = "51";
        /// <summary>
        /// Lỗi xử lý phía MSB
        /// </summary>
        public const string InternalServerError = "99";
        /// <summary>
        /// Tài khoản VA đã tồn tại
        /// </summary>
        public const string ExistAccount = "206";


    }
}
