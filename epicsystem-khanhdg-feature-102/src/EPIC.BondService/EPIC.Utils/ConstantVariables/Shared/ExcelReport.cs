using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    public class ExcelReport
    {
        public static class Gender
        {
            public const string Female = "Nữ";
            public const string Male = "Nam";
        }

        public static class GenderSymbol
        {
            public const string Female = "F";
            public const string Male = "M";
        }

        public static class StatusSymbol
        {
            public const string ACTIVE = "A";
            public const string DEACTIVE = "D";
        }

        public static class StatusDisplay
        {
            public const string ACTIVE = "Hoạt động";
            public const string DEACTIVE = "Chưa hoạt động";
        }

        public static class SaleType
        {
            public const int MANAGER = 1;
            public const int EMPLOYEE = 2;
            public const int COLLABORATOR = 3;
        }

        public static class SaleTypeDisplay
        {
            public const string MANAGER = "Quản lý";
            public const string EMPLOYEE = "Nhân viên";
            public const string COLLABORATOR = "Cộng tác viên";
        }

        public static class ApproveStatus
        {
            public const int TRINH_DUYET = 1;
            public const int DA_DUYET = 2;
            public const int HUY = 3;
            public const int DONG = 4;
            public const int EPIC_DUYET = 5;
        }

        public static class YesOrNoDisplay
        {
            public const string YES = "Có";
            public const string NO = "Không";
        }

        public static class YesOrNo
        {
            public const string YES = "Y";
            public const string NO = "N";
        }

        public static class InvestorInfo
        {
            /// <summary>
            /// Thông tin được phép update
            /// </summary>
            public static List<String> InfoUpdate = new List<String>() { "NameEn", "ShortName", "Bori", "Dorf",
                "EduLevel", "Occupation","Address", "EduLevel", "ContactAddress", "Nationality", "Phone", "Fax",
                "Mobile", "Email", "TaxCode", "SecurityCompany", "StockTradingAccount", "RepresentativePhone",
                "RepresentativeEmail", "ReferralCodeSelf", "IdNo", "IdDate", "IdExpiredDate", "IdIssuer",
                "PlaceOfOrigin", "PlaceOfResidence", "Fullname", "Sex", "DateOfBirth", "BankAccount", "OwnerAccount", "OwnerAccount"};
        }

        public static class OrderStatusDisplay
        {
            public const string KHOI_TAO = "Khởi tạo";
            public const string CHO_THANH_TOAN = "Chờ thanh toán";
            public const string CHO_KY_HOP_DONG = "Chờ ký hợp đồng";
            public const string CHO_DUYET_HOP_DONG = "Chờ duyệt hợp đồng";
            public const string DANG_DAU_TU = "Đang đầu tư";
            public const string PHONG_TOA = "Phong tỏa";
            public const string GIAI_TOA = "Giải tỏa";
            public const string TAT_TOAN = "Tất toán";
        }

        /// <summary>
        /// Kiểu loại chi
        /// </summary>
        public static class ExpendTypes
        {
            public const int RUT_VON = 1;
            public const int TAT_TOAN = 2;
        }

        public static class ClassifyType
        {
            public const string PRO = "PRO";
            public const string PROA = "PROA";
            public const string PNOTE = "PNOTE";
        }

        public static class TranType
        {
            public const string THU = "Thu";
            public const string CHI = "Chi";
        }

        public static class TaxCode
        {
            public const string MA_SO_THUE = "Mã số thuế";
        }

        public static class IsBlockage
        {
            public const string BLOCKAGE = "Đang phong toả";
            public const string NOT_BLOCKAGE = "Chưa phong toả";
        }

        public static class IsConfirm
        {
            public const string DA_XAC_MINH = "Đã xác minh";
            public const string CHUA_XAC_MINH = "Chưa xác minh";
        }

        /// <summary>
        /// Kiểu thanh toán
        /// </summary>
        public static class PaymentType
    
        {
            public const int LAP_CHI = 1;
            public const int DA_CHI_TRA = 2;
        }

        /// <summary>
        /// Kiểu loại chi trả
        /// </summary>
        public static class PaymentTypeStatus 
        {
            public const int DEN_HAN_CHI_TRA = 0;
            public const int DA_LAP_CHUA_CHI_TRA = 1;
            public const int DA_CHI_TRA = 2;
        }

        public static class InterestPaymentStatusType
        {
            public const int CHI_TU_DONG = 2;
            public const int CHI_THU_CONG = 4;
        }
    }
}
