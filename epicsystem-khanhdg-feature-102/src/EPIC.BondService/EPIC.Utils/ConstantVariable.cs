using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils
{
    /// <summary>
    /// Loại đăng nhập cho investor
    /// </summary>
    public static class LoginType
    {
        /// <summary>
        /// Đăng nhập bằng mật khẩu
        /// </summary>
        public const string PASSWORD = "password";
        public const string TOKEN = "token";
    }

    /// <summary>
    /// Các key cho custom response của identity server
    /// </summary>
    public static class CustomResponseKey
    {
        public const string CODE = "code";
        public const string MESSAGE = "message";
        public const string EXCEPTION = "exception";
        public const string STEP = "step";
        public const string IS_FIRST_TIME = "is_first_time";
        public const string IS_TEMP_PASSWORD = "is_temp_password";
        public const string IS_EKYC = "is_ekyc";
        public const string IS_HAVE_PIN = "is_have_pin";
        public const string IS_VERIFIED_EMAIL = "is_verified_email";
        public const string IS_VERIFIED_FACE = "is_verified_face";
        public const string IS_TEMP_PIN = "is_temp_pin";
        public const string IS_TEMP_USER = "is_temp_user";

    }

    public static class GrantTypeExtend
    {
        public const string EmailPhone = "email_phone";
    }

    /// <summary>
    /// Key lấy trường tự định nghĩa khi gọi api đăng nhập
    /// </summary>
    public static class RawRequestKey
    {
        public const string EMAIL = "email";
        public const string PHONE = "phone";
        public const string User = "user";
        public const string PASSWORD = "password";
        public const string FCM_TOKEN = "fcm_token";
    }

    public static class ForgotPasswordType
    {
        public const string SEND_MAIL = "mail";
        public const string SEND_SMS = "sms";
    }

    public static class CardTypesInput
    {
        public const string CCCD = "cccd";
        public const string CMND = "cmnd";
        public const string PASSPORT = "passport";
    }

    public static class IDTypes
    {
        public const string CCCD = "CCCD";
        public const string CMND = "CMND";
        public const string PASSPORT = "PASSPORT";
    }

    public static class Genders
    {
        public const string MALE = "M";
        public const string FEMALE = "F";
    }

    /// <summary>
    /// Trạng thái Investor
    /// </summary>
    public static class InvestorStatus
    {
        public const string ACTIVE = "A";
        public const string DEACTIVE = "D";
        public const string CANCEL = "C";
        public const string TEMP = "T";
        public const string LOCKED = "L";
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CustomerType
    {
        public const string IS_INVETOR = "Cá nhân";
        public const string IS_BUSINESSCUSTOMER = "Doanh nghiệp";

    }
    /// <summary>
    /// Trạng thái Investor
    /// </summary>
    public static class InvestorIdentificationStatus
    {
        public const string ACTIVE = "A";
        public const string TEMP = "T";
        public const string DEACTIVE = "D";
    }

    public static class EKYCStatus
    {
        /// <summary>
        /// Chưa định danh
        /// </summary>
        public const string NOT = "N";
        /// <summary>
        /// Đã định danh nhưng có thông tin còn sai
        /// </summary>
        public const string IDENTIFIED = "I";
        /// <summary>
        /// Đã duyệt định danh có thông tin ok
        /// </summary>
        public const string APPROVED_IDENTIFIER = "A";
    }

    public static class UserStatus
    {
        public const string ACTIVE = "A";
        public const string DEACTIVE = "D";
        public const string TEMP = "T";
        public const string LOCKED = "L";
    }
    
    public static class YesNo
    {
        public const string YES = "Y";
        public const string NO = "N";
        public static readonly List<string> All = new List<string>()
        {
            YES, NO
        };

        public static string GetYesNoText(bool input)
        {
            var yesNoText = input switch
            {
                true => YesNo.YES,
                false => YesNo.NO,
            };
            return yesNoText;
        }
    }

    public static class TrueOrFalseNum
    {
        public const int TRUE = 1;
        public const int FALSE = 0;
        public const int EXPIRE = -1;
    }

    public static class Status
    {
        public const string ACTIVE = "A";
        public const string INACTIVE = "D";
        public const string CLOSED = "C";
        public const string ChoDuyet = "P";
        public const string DaDuyet = "A";
        public const string TuChoiDuyet = "R";
        public const string TAM = "T";
        public const string DA_XOA = "X";
    }

    /// <summary>
    /// Nguồn đặt lệnh trả về FE
    /// </summary>
    public static class SourceOrderFE
    {
        public const int QUAN_TRI_VIEN = 1;
        public const int KHACH_HANG = 2;
        public const int SALE = 3;
    }

    public static class INTEREST_TYPE
    {
        public const string Coupon = "1";
        public const string ZeroCoupon = "0";
    }

    public static class COUPON_TYPE
    {
        public const string Coupon = "1";
        public const string ZeroCoupon = "0";
    }

    public static class INTEREST_Coupon_TYPE
    {
        public const string ThaNoi = "1";
        public const string DinhKy = "2";
    }

    /// <summary>
    /// Kiểu nhà đầu tư
    /// </summary>
    public static class InvestorType
    {
        /// <summary>
        /// Chuyên nghiệp
        /// </summary>
        public const string PROFESSIONAL = "P";
        /// <summary>
        /// Tất cả cả chuyên và không chuyên
        /// </summary>
        public const string ALL = "A";
    }

    public static class InvestorAppoveStatus
    {
        public const int EPIC_APPOVED = 2;
        public const int TRADING_PROVIDER_APPROVED = 1;
        public const int DO_NOTHING = 0;
    }

    /// <summary>
    /// Kiểu chính sách sản phẩm
    /// </summary>
    public static class BondPolicyType
    {
        /// <summary>
        /// Fix ngày bán cố định
        /// </summary>
        public const int FIX = 1;
        /// <summary>
        /// Ngày bán thay đổi
        /// </summary>
        public const int FLEXIBLE = 2;
    }

    /// <summary>
    /// Phân loại chính sách
    /// </summary>
    public static class BondPolicyClassify
    {
        public const int PRO = 1;
        public const int PROA = 2;
        public const int PNOTE = 3;

        public static Dictionary<int, string> KeyValues = new()
        {
            { 0, "" },
            { BondPolicyClassify.PNOTE, "PNOTE" },
            { BondPolicyClassify.PRO, "PRO" },
            { BondPolicyClassify.PROA, "PROA" },
        };
    }

    /// <summary>
    /// Trạng thái phát hành sơ cấp
    /// </summary>
    public static class BondSecondaryStatus
    {
        /// <summary>
        /// Nháp
        /// </summary>
        public const int TEMP = 1;
        /// <summary>
        /// Trình duyệt
        /// </summary>
        public const int SUBMIT = 2;
        /// <summary>
        /// dlsc duyệt
        /// </summary>
        public const int TRADING_PROVIDER_APPROVE = 3;
        /// <summary>
        /// EPIC duyệt
        /// </summary>
        public const int SUPER_ADMIN_APPROVE = 4;
    }

    /// <summary>
    /// Trạng thái chính sách mẫu
    /// </summary>
    public static class BondPolicyTemplate
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Trạng thái chính sách mẫu
    /// </summary>
    public static class ContractTemplateStatus
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Phân loại mẫu hợp đồng
    /// </summary>
    public static class ContractTemplateClassify
    {
        public const int PRO = 1;
        public const int PROA = 2;
        public const int PNOTE = 3;
    }

    /// <summary>
    /// Trạng thái mẫu hợp đồng sale
    /// </summary>
    public static class CollapContractTemplateStatus
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    public static class ContractTemplateType
    {
        public const int HDDTTP = 1;
        public const int BNHS = 2;
        public const int GXNGDTP = 3;
        public const int PDNTHGD = 4;
        public const int HDDMTP = 5;
        public const int HDDBTP = 6;
        public const int BNHD = 7;
        public const int GXNSDTP = 8;
        public const int BMHTNTTPVKQDT = 9;
    }

    /// <summary>
    /// Trạng thái kỳ hạn của chính sách mẫu
    /// </summary>
    public static class BondPolicyDetailTemplate
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Loại ký pdf (chữ ký số)
    /// </summary>
    public static class SignPdfType
    {
        /// <summary>
        /// Không hiện chữ ký
        /// </summary>
        public const int TYPE_PDFSIGNATURE_DONTSHOW = 0;
        /// <summary>
        /// Hiện text
        /// </summary>
        public const int TYPE_PDFSIGNATURE_TEXT = 1;
        /// <summary>
        /// Hiện ảnh
        /// </summary>
        public const int TYPE_PDFSIGNATURE_IMA = 2;
        /// <summary>
        /// Hiện của text và ảnh
        /// </summary>
        public const int TYPE_PDFSIGNATURE_TEXTIMA = 3;
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IsSignPdf
    {
        public const string Yes = "Y";
        public const string No = "N";
    }

    /// <summary>
    /// Trạng thái thanh toán order
    /// </summary>
    public static class OrderPaymentStatus
    {
        public const int NHAP = 1;
        public const int DA_THANH_TOAN = 2;
        public const int HUY_THANH_TOAN = 3;
    }
    public static class PeriodType
    {
        public const string NAM = "Y";
        public const string THANG = "M";
        public const string NGAY = "D";
        public const string QUY = "Q";
    }

    public static class PeriodDisplay
    {
        public const string NAM = " Năm";
        public const string THANG = " Tháng";
        public const string NGAY = " Ngày";
        public const string QUY = " Quý";
    }

    /// <summary>
    /// Các trạng thái của bảng Core Approve
    /// </summary>
    public static class CoreApproveStatus
    {
        /// <summary>
        /// Trang thai = 0 không lưu trong db
        /// </summary>
        public const int KHOI_TAO = 0;
        public const int TRINH_DUYET = 1;
        public const int DUYET = 2;
        public const int HUY = 3;
    }

    /// <summary>
    /// Làm việc với bảng nào
    /// </summary>
    public class CoreApproveDataType
    {
        public const int USERS = 1;
        public const int INVESTOR = 2;
        public const int BUSINESS_CUSTOMER = 3;
        public const int PRODUCT_BOND_INFO = 4;
        public const int PRODUCT_BOND_SECONDARY = 5;
        public const int DISTRIBUTION_CONTRACT_FILE = 6;
        public const int PRODUCT_BOND_PRIMARY = 7;
        public const int SALE = 8;
        public const int BUSINESS_CUSTOMER_BANK = 9;
        public const int INVESTOR_PROFESSIONAL = 10;
        public const int INV_RENEWALS_REQUEST = 11;
        public const int INVESTOR_PHONE = 12;
        public const int INVESTOR_EMAIL = 13;
        public const int LOY_HIS_ACCUMULATE_POINT = 14;
    }

    /// <summary>
    /// Làm việc với bảng nào (Update Investor)
    /// </summary>
    public static class CoreHistoryUpdateTable
    {
        public const int INVESTOR = 1;
        public const int INVESTOR_IDENTIFICATION = 2;
        public const int INVESTOR_BANK = 3;
        public const int INVESTOR_CONTACT_ADDRESS = 4;
        public const int BUSINESS_CUSTOMER = 5;
        public const int BUSINESS_CUSTOMER_BANK = 6;
        public const int INVESTOR_STOCK = 7;
    }

    /// <summary>
    /// Trạng thái kỳ hạn của chính sách mẫu
    /// </summary>
    public static class INVPolicyDetailTemplateStatus
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Trạng thái của chính sách mẫu INV
    /// </summary>
    public static class INVPolicyTemplateStatus
    {
        /// <summary>
        /// Kích hoạt
        /// </summary>
        public const string ACTIVE = "A";
        /// <summary>
        /// Hủy kích hoạt
        /// </summary>
        public const string DEACTIVE = "D";
    }

    /// <summary>
    /// Phân loại chính sách INV
    /// </summary>
    public static class INVPolicyClassify
    {
        public const int FLEX = 1;
        public const int FLASH = 2;
        public const int FIX = 3;
    }


    /// <summary>
    /// Kiểu chính sách sản phẩm INV
    /// </summary>
    public static class InvPolicyType
    {
        /// <summary>
        /// Fix ngày bán cố định
        /// </summary>
        public const int FIX = 1;
        /// <summary>
        /// Ngày bán thay đổi
        /// </summary>
        public const int FLEXIBLE = 2;
        /// <summary>
        /// Giới hạn
        /// </summary>
        public const int LIMIT = 3;
        /// <summary>
        /// Chi trả cố định theo ngày
        /// </summary>
        public const int FIXED_PAYMENT_DATE = 4;
    }

    /// <summary>
    /// Trạng thái mẫu hợp đồng giao nhận
    /// </summary>
    public static class DeliveryStatus
    {
        /// <summary>
        /// chờ xử lý
        /// </summary>
        public const int WAITING = 1;
        /// <summary>
        /// đang giao
        /// </summary>
        public const int DELIVERY = 2;

        /// <summary>
        /// đã nhận
        /// </summary>
        public const int RECEIVE = 3;

        /// <summary>
        /// Hoàn thành
        /// </summary>
        public const int COMPLETE = 4;
    }

    /// <summary>
    /// Hình thức nhận hợp đồng giao nhận
    /// </summary>
    public static class DeliverySource
    {
        public const int ONLINE = 1;
        public const int OFFLINE = 2;
    }

    /// <summary>
    /// Nguồn tạo investor
    /// </summary>
    public static class InvestorSource
    {
        public const int ONLINE = 1;
        public const int OFFLINE = 2;
        public const int SALE = 3;
    }

    /// <summary>
    /// Loại hiển thị của mẫu hợp đồng
    /// </summary>
    public static class DisplayType
    {
        public const string TRUOC_KHI_DUYET = "B";
        public const string SAU_KHI_DUYET = "A";
        public static class TaxCode
        {
            public const string MA_SO_THUE = "Mã số thế";
        }

        public static class IsBlockage
        {
            public const string BLOCKAGE = "Đang phong toả";
            public const string NOT_BLOCKAGE = "Chưa phong toả";
        }
    }
}

