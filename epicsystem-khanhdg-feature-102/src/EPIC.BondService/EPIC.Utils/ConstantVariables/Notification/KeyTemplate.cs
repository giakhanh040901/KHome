using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Notification
{
    public class KeyTemplate
    {
        public const string TK_CAP_LAI_MK_THANH_CONG = "TK_CAP_LAI_MK_THANH_CONG";
        public const string TK_CAP_LAI_MA_PIN = "TK_CAP_LAI_MA_PIN";
        /// <summary>
        /// Gửi sms otp
        /// </summary>
        public const string TK_THONGBAO_OTP = "TK_THONGBAO_OTP";
        public const string TK_DANGKY_TK_OK = "TK_DANGKY_TK_OK";
        public const string TK_XAC_MINH_THANH_CONG = "TK_XAC_MINH_THANH_CONG";
        public const string TK_GIAY_TO_THAY_DOI_OK = "TK_GIAY_TO_THAY_DOI_OK";
        public const string TK_GIAY_TO_CA_NHAN_PHE_DUYET_OK = "TK_GIAY_TO_CA_NHAN_PHE_DUYET_OK";
        public const string TK_EMAIL_XACMINH_OK = "TK_EMAIL_XACMINH_OK";
        public const string TK_THAYDOI_DIACHI_GIAO_DICH_MACDINH = "TK_THAYDOI_DIACHI_GIAO_DICH_MACDINH";
        public const string TK_THIETBI_TRUYCAP_TK = "TK_THIETBI_TRUYCAP_TK";
        public const string TK_KHOI_TAO_THANH_CONG = "TK_KHOI_TAO_THANH_CONG";
        /// <summary>
        /// Gửi otp email
        /// </summary>
        public const string TK_THONG_BAO_OTP_EMAIL = "TK_THONG_BAO_OTP_EMAIL";
        public const string TK_XAC_MINH_EMAIL = "TK_XAC_MINH_EMAIL";
        public const string TK_THAY_DOI_TK_THU_HUONG = "TK_THAY_DOI_TK_THU_HUONG";
        public const string TK_NGUOI_NHAP_MA_GIOI_THIEU = "TK_NGUOI_NHAP_MA_GIOI_THIEU";

        public const string TB_CHUC_MUNG_SINH_NHAT_KHACH_HANG = "TB_CHUC_MUNG_SINH_NHAT_KHACH_HANG";
        public const string TK_CHUNG_MINH_NHA_DAU_TU_CHUYEN_NGHIEP = "TK_CHUNG_MINH_NHA_DAU_TU_CHUYEN_NGHIEP";

        #region Đầu tư
        public const string DAU_TU_CHUYEN_TIEN_THANH_CONG_BOND = "DAU_TU_CHUYEN_TIEN_THANH_CONG";
        public const string DAU_TU_THANH_CONG_BOND = "DAU_TU_DAU_TU_THANH_CONG";

        public const string DAU_TU_CHUYEN_TIEN_THANH_CONG_INVEST = "INVEST_CHUYEN_TIEN_THANH_CONG";
        public const string DAU_TU_THANH_CONG_INVEST = "INVEST_INVEST_THANH_CONG";
        public const string BOND_ADMIN_KHACH_VAO_TIEN = "BOND_ADMIN_KHACH_VAO_TIEN";
        public const string BOND_ADMIN_KHACH_RUT_TIEN = "BOND_ADMIN_KHACH_RUT_TIEN";
        #endregion

        #region Tất toán Invest
        public const string INVEST_HOP_DONG_DEN_HAN = "INVEST_HOP_DONG_DEN_HAN";
        #endregion
        #region tái tục, chi trả, rút vốn
        public const string TAI_TUC_THANH_CONG_BOND = "TAI_TUC_THANH_CONG_BOND";
        public const string DAU_TU_TAI_TUC_DAU_TU = "DAU_TU_TAI_TUC_DAU_TU";
        public const string DAU_TU_TAT_TOAN_TRUOC_HAN = "DAU_TU_TAT_TOAN_TRUOC_HAN";
        public const string INVEST_RUT_VON_THANH_CONG = "INVEST_RUT_VON_THANH_CONG";
        public const string INVEST_CHI_TRA_LOI_TUC = "INVEST_CHI_TRA_LOI_TUC";
        public const string INVEST_TAT_TOAN_TRUOC_HAN = "INVEST_TAT_TOAN_TRUOC_HAN";
        public const string INVEST_TAT_TOAN_DUNG_HAN = "INVEST_TAT_TOAN_DUNG_HAN";
        public const string INVEST_TAI_TUC_INVEST = "INVEST_TAI_TUC_INVEST";
        public const string INVEST_YEU_CAU_TAI_TUC = "INVEST_YEU_CAU_TAI_TUC";
        #endregion

        #region saler
        public const string TB_YEU_CAU_DUYET_TU_VAN_VIEN = "TB_YEU_CAU_DUYET_TU_VAN_VIEN";
        public const string TB_DANG_KY_TU_VAN_VIEN_OK = "TB_DANG_KY_TU_VAN_VIEN_OK";
        public const string TB_DIEU_HUONG_KENH_BAN = "TB_DIEU_HUONG_KENH_BAN";
        public const string TB_SINH_NHAT_KHACH_HANG_DEN_SALE = "TB_SINH_NHAT_KHACH_HANG_DEN_SALE";
        #endregion

        #region garner
        public const string GARNER_TICH_LUY_THANH_CONG = "GARNER_TICH_LUY_THANH_CONG";
        public const string GARNER_CHUYEN_TIEN_THANH_CONG = "GARNER_CHUYEN_TIEN_THANH_CONG";
        public const string GARNER_RUT_TIEN_THANH_CONG = "GARNER_RUT_TIEN_THANH_CONG";
        public const string GARNER_CHI_TRA_THANH_CONG = "GARNER_CHI_TRA_THANH_CONG";
        public const string GARNER_ADMIN_KHACH_VAO_TIEN = "GARNER_ADMIN_KHACH_VAO_TIEN";
        public const string GARNER_ADMIN_KHACH_RUT_TIEN = "GARNER_ADMIN_KHACH_RUT_TIEN";
        public const string GARNER_KHACH_QUET_QR_GIAO_NHAN_HD = "GARNER_KHACH_QUET_QR_GIAO_NHAN_HD";
        #endregion

        #region Invest
        public const string INVEST_TK_INVEST_SAP_HET_HAN = "INVEST_TK_INVEST_SAP_HET_HAN";
        public const string INVEST_ADMIN_KHACH_VAO_TIEN = "INVEST_ADMIN_KHACH_VAO_TIEN";
        public const string INVEST_ADMIN_KHACH_RUT_TIEN = "INVEST_ADMIN_KHACH_RUT_TIEN";
        public const string INVEST_KHACH_QUET_QR_GIAO_NHAN_HD = "INVEST_KHACH_QUET_QR_GIAO_NHAN_HD";
        public const string INVEST_SALE_KHACH_DAU_TU_THANH_CONG = "INVEST_SALE_KHACH_DAU_TU_THANH_CONG";
        #endregion

        #region loyalty
        public const string THONG_BAO_GAN_VOUCHER_OK = "LOYALTY_GAN_VOUCHER_OK";
        public const string LOYALTY_TAO_YEU_CAU_DOI_DIEM_VOUCHER = "LOYALTY_TAO_YEU_CAU_DOI_DIEM";
        public const string LOYALTY_TICH_DIEM_OK = "LOYALTY_TICH_DIEM_OK";
        public const string LOYALTY_NHAN_VOUCHER_OK = "LOYALTY_NHAN_VOUCHER_OK";
        public const string LOYALTY_ADMIN_KHACH_TAO_YEU_CAU_DOI_DIEM_VOUCHER = "LOYALTY_ADMIN_KHACH_TAO_YEU_CAU_DOI_DIEM";
        #endregion

        #region RealEstate
        public const string REAL_ESTATE_DAT_COC_THANH_CONG = "REAL_ESTATE_DAT_COC_THANH_CONG";
        public const string REAL_ESTATE_CHUYEN_TIEN_THANH_CONG = "REAL_ESTATE_CHUYEN_TIEN_THANH_CONG";
        public const string REAL_ESTATE_ADMIN_KHACH_VAO_TIEN = "REAL_ESTATE_ADMIN_KHACH_VAO_TIEN";
        public const string REAL_ESTATE_SALE_KHACH_DAT_COC_THANH_CONG = "REAL_ESTATE_SALE_KHACH_DAT_COC_THANH_CONG";

        #endregion

        #region rocketchat
        public const string CHAT_RECEIVE_MSG = "CHAT_RECEIVE_MSG";
        public const string CHAT_RECEIVE_DIRECT_MSG = "CHAT_RECEIVE_DIRECT_MSG";
        #endregion

        #region Event
        public const string EVENT_DANG_KY_VE_THANH_CONG = "EVENT_DANG_KY_VE_THANH_CONG";
        public const string EVENT_TAM_DUNG_SU_KIEN = "EVENT_TAM_DUNG_SU_KIEN";
        public const string EVENT_THANH_TOAN_THANH_CONG = "EVENT_THANH_TOAN_THANH_CONG";
        public const string EVENT_THAM_GIA_SU_KIEN_THANH_CONG = "EVENT_THAM_GIA_SU_KIEN_THANH_CONG";
        public const string EVENT_ADMIN_CHUYEN_TIEN_MUA_VE_THANH_CONG = "EVENT_ADMIN_CHUYEN_TIEN_MUA_VE_THANH_CONG";
        public const string EVENT_SU_KIEN_SAP_DIEN_RA = "EVENT_SU_KIEN_SAP_DIEN_RA";
        public const string EVENT_ADMIN_KHACH_HANG_NHAN_VE_BAN_CUNG = "EVENT_ADMIN_KH_NHAN_VE_BAN_CUNG";
        public const string EVENT_ADMIN_KHACH_HANG_NHAN_HOA_DON = "EVENT_ADMIN_KHACH_HANG_NHAN_HOA_DON";
        public const string EVENT_ADMIN_SU_KIEN_KET_THUC = "EVENT_ADMIN_SU_KIEN_KET_THUC";
        #endregion
    }
}
