using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using System.Collections.Generic;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;

namespace EPIC.Utils.DataUtils
{
    public static class ExcelDataUtils
    {

        /// <summary>
        /// Trạng thái của lệnh đặt
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string StatusOrder(int? status)
        {
            return status switch
            {
                OrderStatus.KHOI_TAO => "Khởi tạo",
                OrderStatus.CHO_THANH_TOAN => "Chờ thanh toán",
                OrderStatus.CHO_KY_HOP_DONG => "Chờ ký hợp đồng",
                OrderStatus.CHO_DUYET_HOP_DONG => "Chờ duyệt hợp đồng",
                OrderStatus.DANG_DAU_TU => "Đang đầu tư",
                OrderStatus.PHONG_TOA => "Phong toả",
                OrderStatus.GIAI_TOA => "Giải toả",
                OrderStatus.TAT_TOAN => "Tất toán",
                _ => string.Empty,
            };
        }

        public static string StatusProductItem(int? status)
        {
            return status switch
            {
                RstProductItemStatus.KHOI_TAO => "Khởi tạo",
                RstProductItemStatus.GIU_CHO => "Giữ chỗ",
                RstProductItemStatus.KHOA_CAN => "Khóa căn",
                RstProductItemStatus.DA_COC => "Đã cọc",
                RstProductItemStatus.DA_BAN => "Đã bán",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// giới tính của khách hàng cá nhân
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string GenderDisplay(string gender)
        {
            return gender switch
            {
                GenderSymbol.Female => "Nữ",
                GenderSymbol.Male => "Nam",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Loại thời hạn: ngày, tháng, năm
        /// </summary>
        /// <param name="periodType"></param>
        /// <returns></returns>
        public static string PeriodTypeDisplay(string periodType)
        {
            return periodType switch
            {
                PeriodType.NGAY => " Ngày",
                PeriodType.THANG => " Tháng",
                PeriodType.NAM => " Năm",
                PeriodType.QUY => " Quý",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Trạng thái giao dịch offline hoặc online
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string TradingTypeDisplay(int? source)
        {
            return source switch
            {
                SourceOrder.ONLINE => "Online",
                SourceOrder.OFFLINE => "Offline",
                _ => string.Empty
            };
        }

        public static string ApproveStatusDisplay(int? source)
        {
            return source switch
            {
                ExcelReport.ApproveStatus.TRINH_DUYET => "Trình duyệt",
                ExcelReport.ApproveStatus.DA_DUYET => "Đã duyệt",
                ExcelReport.ApproveStatus.HUY => "Hủy",
                ExcelReport.ApproveStatus.DONG => "Đóng",
                ExcelReport.ApproveStatus.EPIC_DUYET => "EPIC duyệt",
                _ => string.Empty
            };
        }

        public static string UserStatusDisplay(string status)
        {
            return status.Trim() switch
            {
                UserStatus.LOCKED => "Khóa",
                UserStatus.ACTIVE => "Kích hoạt",
                UserStatus.DEACTIVE => "Không kích hoạt",
                UserStatus.TEMP => "Tạm",
                _ => string.Empty
            };
        }
        public static string CaculateTypeDisplay(int? caculateType)
        {
            return caculateType switch
            {
                CalculateTypes.NET => "NET",
                CalculateTypes.GROSS => "GROSS",
                _ => string.Empty
            };
        }

        public static string FieldNameDisplay(string fieldName)
        {
            return fieldName switch
            {
                "Fullname" => "Họ và tên",
                "IdNo" => "Số giấy giờ",
                "IdType" => "Loại giấy tờ",
                "DateOfBirth" => "Ngày sinh",
                "Nationality" => "Quốc tịch",
                "PersonalIdentification" => "Đặc điểm nhận dạng",
                "IdIssuer" => "Nơi cấp",
                "IdDate" => "Ngày cấp",
                "IdExpiredDate" => "Ngày hết hạn",
                "PlaceOfOrigin" => "Quê quán",
                "PlaceOfResidence" => "Địa chỉ thường trú",
                "IdFrontImageUrl" => "Đường dẫn ảnh mặt trước",
                "IdBackImageUrl" => "Đường dẫn ảnh mặt sau",
                "IdExtraImageUrl" => "Đường dẫn ảnh thêm",
                "FaceImageUrl" => "Đường dẫn ảnh khuôn mặt ",
                "FaceVideoUrl" => "Đường dẫn video xác thực",
                "Status" => "Trạng thái",
                "IsDefault" => "Mặc định",
                "IsVerifiedFace" => "Đã xác nhận thông tin nhà đầu tư",
                "IsVerifiedIdentification" => "Đã xác nhận mặt nhà đầu tư",
                "StatusApproved" => "Trình tự duyệt",
                "Sex" => "Giới tính",
                "EkycIncorrectFields" => "Các trường OCR sai",
                "EkycInfoIsConfirmed" => "Nhà đầu tư đã xác nhận đúng thông tin",
                "ReferralCodeSelf" => "Mã giới thiệu của khách hàng",
                "ReferallCode" => "Mã giới thiệu khách hàng nhập",
                _ => fieldName
            };
        }

        public static string StatusActive(string status)
        {
            return status switch
            {
                InvestorStatus.ACTIVE => "Kích hoạt",
                InvestorStatus.DEACTIVE => "Khóa",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Nguồn tạo tài khoản
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SourceDisplay(int? source)
        {
            return source switch
            {
                InvestorAccountSource.APP => "APP",
                InvestorAccountSource.CMS => "CMS",
                InvestorAccountSource.SALER => "Saler",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Kiểu sale
        /// </summary>
        /// <param name="saleType"></param>
        /// <returns></returns>
        public static string SaleTypeDisplay(int? saleType)
        {
            return saleType switch
            {
                SaleType.MANAGER => "Quản lý",
                SaleType.COLLABORATOR => "Cộng tác viên",
                SaleType.EMPLOYEE => "Nhân viên",
                _ => string.Empty
            };
        }

        public static string SecurityCompanyDisplay(int? securityCompany)
        {
            return securityCompany switch
            {
                1 => "TPS",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Hiển thị có không
        /// </summary>
        /// <param name="yesNo"></param>
        /// <returns></returns>
        public static string YesNoDisplay(string yesNo)
        {
            return yesNo switch
            {
                YesNo.YES => "Có",
                YesNo.NO => "Không",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Xác minh hay chưa xác minh
        /// </summary>
        /// <param name="isConfirm"></param>
        /// <returns></returns>
        public static string IsConfirmDisplay(string isConfirm)
        {
            return isConfirm switch
            {
                YesNo.YES => "Đã xác minh",
                YesNo.NO => "Chưa xác minh",
                _ => string.Empty
            };
        }

        public static string FieldNameDataDisplay(string fieldData)
        {
            return fieldData switch
            {
                YesNo.YES => "Có",
                YesNo.NO => "Không",
                Status.ACTIVE => "Hoạt động",
                Status.INACTIVE => "Không hoạt động",
                Status.TAM => "Tạm",
                "0" => "Chưa duyệt",
                "1" => "DLSC đã duyệt",
                "2" => "EPIC đã duyệt",
                GenderSymbol.Male => Gender.Male,
                GenderSymbol.Female => Gender.Female,
                _ => fieldData
            };
        }

        public static string OrderSourceDisplay(int source)
        {
            return source switch
            {
                SourceOrderFE.KHACH_HANG => "Khách hàng",
                SourceOrderFE.QUAN_TRI_VIEN => "Quản trị viên",
                SourceOrderFE.SALE => "Sale",
                _ => string.Empty
            };
        }

        public static string SettlementTypeDisplay(int? settlementType)
        {
            return settlementType switch
            {
                SettlementTypes.NHAN_GOC_VA_LOI_TUC => "Nhận gốc và lợi tức",
                SettlementTypes.TAI_TUC_GOC => "Tái tục gốc",
                SettlementTypes.TAI_TUC_GOC_VA_LOI_NHUAN => "Tái tục gốc và lợi nhuận",
                _ => "Nhận gốc và lợi tức",
            };
        }

        public static string TranTypeDisplay(int? tranType)
        {
            return tranType switch
            {
                TranTypes.THU => "Thu",
                TranTypes.CHI => "Chi",
                _ => string.Empty
            };
        }

        public static string TranClassify(int? tranClassify)
        {
            return tranClassify switch
            {
                TranClassifies.THANH_TOAN => "Gốc",
                TranClassifies.CHI_TRA_LOI_TUC => "Gốc",
                TranClassifies.RUT_VON => "Lợi tức",
                TranClassifies.TAI_TUC_HOP_DONG => "Gốc",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Loại sản phẩm 
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public static string GarnerProductType(int? productType)
        {
            return productType switch
            {
                GarnerProductTypes.CO_PHAN => "Cổ phần",
                GarnerProductTypes.BAT_DONG_SAN => "Bất động sản",
                _ => string.Empty
            };
        }

        /// <summary>
        /// Loại kỳ hạn
        /// </summary>
        /// <param name="garnerType"></param>
        /// <returns></returns>
        public static string GarnerType(int? garnerType)
        {
            return garnerType switch
            {
                PolicyGarnerTypes.LINH_HOAT => "Không kỳ hạn",
                PolicyGarnerTypes.DINH_KY => "Có kỳ hạn",
                _ => string.Empty
            };
        }

        public static string SourceOrderer(int Orderer)
        {
            return Orderer switch
            {
                SourceOrderFE.QUAN_TRI_VIEN => "Quản trị viên",
                SourceOrderFE.SALE => "Tư vấn viên",
                SourceOrderFE.KHACH_HANG => "Khách hàng",
                _ => string.Empty
            };
        }

        public static string RstBuildingDensityType(int? classifyType)
        {
            return classifyType switch
            {
                RstBuildingDensityTypes.PhanKhu => "Phân khu",
                RstBuildingDensityTypes.Toa => "Tòa",
                RstBuildingDensityTypes.ODat => "Ô đất",
                RstBuildingDensityTypes.Tang => "Tầng",
                RstBuildingDensityTypes.Lo => "Lô",
                _ => string.Empty
            };
        }
    }
}
