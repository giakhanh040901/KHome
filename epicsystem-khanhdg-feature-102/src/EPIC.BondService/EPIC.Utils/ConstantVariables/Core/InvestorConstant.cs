using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils
{
    public static class InvestorAddressDefault
    {
        public const string YES = "Y";
        public const string NO = "N";
    }

    /// <summary>
    /// Các step khi khách đăng ký tài khoản
    /// </summary>
    public static class InvestorAppStep
    {
        public const int BAT_DAU = 1;
        public const int DA_DANG_KY = 2;
        public const int DA_EKYC = 3;
        public const int DA_ADD_BANK = 4;
    }

    /// <summary>
    /// Phân loại ảnh mặt truyền lên
    /// </summary>
    public static class FaceMatchImageTypes
    {
        public const int ANH_MAT_TRAI = 1;
        public const int ANH_MAT_PHAI = 2;
        public const int ANH_MAT_NHAY_MAT = 3;
        public const int ANH_MAT_CUOI = 4;
    }

    public class EkycFields
    {
        public static Dictionary<string, string> Dict = new Dictionary<string, string>
        {
            { "DateOfBirth", "Ngày sinh" },
            { "Fullname", "Họ và tên" },
            { "Sex", "Giới tính" },
            { "IdNo", "Mã số giấy tờ" },
            { "IdDate", "Ngày cấp" },
            { "IdExpiredDate", "Ngày hết hạn" },
            { "IdIssuer", "Nơi cấp" },
            { "PlaceOfOrigin", "Nguyên quán" },
            { "PlaceOfResidence", "Nơi cư trú" },
            { "Nationality", "Quốc tịch" },
            { "IdType", "Loại giấy tờ" },
            { "IdFrontImageUrl", "Ảnh mặt trước" },
        };
    }

    public class ProfStatus
    {
        public const int KHONG_CHUYEN = 1;
        public const int DANG_CHO_DUYET = 2;
        public const int CHUYEN_NGHIEP = 3;
    }

}
