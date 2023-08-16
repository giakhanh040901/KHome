using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Notification
{
    public class TitleEmail
    {
        public const string RESET_PASSWORD_SUCCESS = "Mật khẩu đã được cấp lại";
        public const string RESET_PIN_SUCCESS = "Mã PIN đã được cấp lại";
        public const string SEND_OTP_VERIFY = "Mã OTP xác thực tài khoản";
        public const string SEND_OTP_SUCCESS = "Mã OTP";
        public const string NOTIFY_OTP_EMAIL_TEMPLATE = "Mã OTP";
        public const string REGISTER_ACCOUNT_SUCCESS = "Đăng ký tài khoản thành công";
        public const string VERIFY_ACCOUNT_SUCCESS = "Thông báo xác minh tài khoản thành công";
        public const string MODIFY_BANK_ACCOUNT = "Thay đổi tài khoản thụ hưởng mặc định thành công";
        public const string MODIFY_TRADING_ADDRESS_DEFAULT_SUCCESS = "Thay đổi địa chỉ giao dịch mặc định thành công";
        public const string TK_NGUOI_NHAP_MA_GIOI_THIEU = "Mã giới thiệu của bạn đã được sử dụng";
        public const string NOTIFY_VERIFY_EMAIL_TEMPLATE = "Thông báo xác thực email";
        public const string VERIFY_EMAIL_SUCCESS = "Xác thực email thành công";
        public const string NOTIFY_INVESTOR_PROF = "Thông báo xác minh nhà đầu tư chuyên nghiệp";
        public const string TB_CHUC_MUNG_SINH_NHAT_KHACH_HANG = "Chúc mừng sinh nhật quý khách hàng";

        #region đầu tư
        public const string DAU_TU_THANH_CONG_BOND = "Đầu tư thành công";
        public const string CHUYEN_TIEN_THANH_CONG_BOND = "Chuyển tiền thành công";

        public const string DAU_TU_THANH_CONG_INVEST = "Đầu tư thành công";
        public const string CHUYEN_TIEN_THANH_CONG_INVEST = "Chuyển tiền thành công";
        #endregion

        #region Invest
        public const string INVEST_HOP_DONG_DEN_HAN = "Khoản đầu tư sắp đến hạn";
        public const string INVEST_ADMIN_KHACH_VAO_TIEN = "Thông báo khách hàng vào tiền invest";
        public const string INVEST_ADMIN_KHACH_RUT_TIEN = "Thông báo khách hàng thực hiện rút invest";
        public const string INVEST_QR_CONTRACT_DELIVERY = "Thông báo khách hàng quét QR giao nhận hợp đồng";
        public const string INVEST_SALE_KHACH_DAU_TU_THANH_CONG = "Thông báo khách hàng đầu tư thành công";

        #endregion

        #region tái tục chi trả rút vốn
        //Bond
        public const string TAI_TUC_THANH_CONG_BOND = "Tái tục thành công";
        public const string DAU_TU_TAT_TOAN_TRUOC_HAN = "Hợp đồng tất toán trước hạn";
        public const string DAU_TU_TAI_TUC_DAU_TU = "Tái tục đầu tư thành công";

        //Invest
        public const string INVEST_RUT_VON_THANH_CONG = "Rút vốn thành công";
        public const string CHI_TRA_THANH_CONG_INVEST = "Chi trả lợi tức thành công";
        public const string INVEST_TAT_TOAN_DUNG_HAN = "Tất toán đúng hạn thành công";
        public const string INVEST_TAT_TOAN_TRUOC_HAN = "Tất toán trước hạn thành công";
        public const string INVEST_TAI_TUC_INVEST = "Tái tục đầu tư thành công";
        public const string INVEST_YEU_CAU_TAI_TUC = "Yêu cầu tái tục đầu tư sản phẩm Invest thành công";
        #endregion

        #region saler
        public const string TB_YEU_CAU_DUYET_TU_VAN_VIEN = "Yêu cầu duyệt tư vấn viên";
        public const string TB_DANG_KY_TU_VAN_VIEN_OK = "Đăng ký tư vấn viên thành công";
        public const string TB_DUYET_YEU_CAU_TU_VAN_VIEN_OK = "Duyệt yêu cầu tư vấn viên thành công";
        public const string TB_SINH_NHAT_KHACH_HANG_DEN_SALE = "Thông báo sắp đến sinh nhật của khách hàng";
        #endregion

        #region Garner
        public const string GARNER_CHUYEN_TIEN_THANH_CONG = "Chuyển tiền thành công";
        public const string GARNER_TICH_LUY_THANH_CONG = "Tích lũy thành công";
        public const string GARNER_RUT_TIEN_THANH_CONG = "Rút tiền thành công";
        public const string GARNER_CHI_TRA_THANH_CONG = "Chi trả lợi tức thành công";
        public const string GARNER_ADMIN_KHACH_VAO_TIEN = "Thông báo khách hàng vào tiền tích luỹ";
        public const string GARNER_ADMIN_KHACH_RUT_TIEN = "Thông báo khách hàng thực hiện rút tích luỹ";
        public const string GARNER_QR_CONTRACT_DELIVERY = "Thông báo khách hàng quét QR giao nhận hợp đồng";
        #endregion

        #region loyalty
        public const string LOYALTY_CONVERSION_POINT_SUCCESS = "Thông báo khách hàng đã nhận được voucher";
        public const string LOYALTY_ACCUMULATE_POINT_SUCCESS = "Thông báo khách hàng đã tích điểm thành công";
        public const string LOYALTY_CONSUME_POINT = "Thông báo khách hàng gửi yêu cầu đổi điểm thành công";
        public const string LOYALTY_CUSTOMER_RECEIVED_VOUCHER = "Thông báo khách hàng đã nhận được voucher";
        #endregion

        #region RealEstate
        public const string REAL_ESTATE_DAT_COC_THANH_CONG = "Đặt cọc thành công";
        public const string REAL_ESTATE_CHUYEN_TIEN_THANH_CONG = "Chuyển tiền thành công";
        public const string REAL_ESTATE_ADMIN_KHACH_VAO_TIEN = "Thông báo khách hàng vào tiền bất động sản";
        public const string REAL_ESTATE_SALE_KHACH_DAT_COC_THANH_CONG = "Thông báo khách hàng đặt cọc thành công";
        #endregion

        #region chat
        public const string CHAT_AGENT_SENT_MSG_TO_VISITOR = "Thông báo khách hàng nhận được tin nhắn mới";
        #endregion

        #region Event
        public const string TB_DANG_KY_THAM_GIA_SU_KIEN = "Thông báo đăng ký vé tham gia sự kiện thành công";
        public const string TB_TAM_DUNG_TO_CHUC_SU_KIEN = "Thông báo tạm dừng tổ chức sự kiện";
        public const string TB_MUA_VE_THANH_CONG = "Thông báo duyệt thanh toán thành công";
        public const string TB_THAM_GIA_SU_KIEN_THANH_CONG = "Thông báo tham gia sự kiện thành công";
        public const string TB_AMDIN_KHACH_CHUYEN_TIEN_MUA_VE_THAM_GIA_SU_KIEN = "Thông báo khách hàng chuyển tiền mua vé tham gia sự kiện";
        public const string TB_SU_KIEN_SAP_DIEN_RA = "Thông báo sự kiện sắp diễn ra";
        public const string TB_ADMIN_KHACH_HANG_NHAN_VE_BAN_CUNG = "Thông báo khách hàng yêu cầu nhận vé bản cứng";
        public const string TB_ADMIN_KHACH_HANG_NHAN_HOA_DON = "Thông báo khách hàng yêu cầu nhận hóa đơn";
        public const string TB_ADMIN_SU_KIEN_KET_THUC = "Thông báo sự kiện kết thúc";
        #endregion
    }
}
