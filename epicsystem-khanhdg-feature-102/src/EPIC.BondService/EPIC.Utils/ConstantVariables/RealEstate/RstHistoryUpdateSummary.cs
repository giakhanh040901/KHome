using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstHistoryUpdateSummary
    {
        //Hợp đồng
        public const string SUMMARY_APPROVE_FILE = "Cập nhật duyệt hợp đồng ";
        public const string SUMMARY_CANCEL_FILE = "Cập nhật hủy duyệt hợp đồng ";
        public const string SUMMARY_UPDATE_SOURCE = "Cập nhật loại hình online sang offline";
        public const string SUMMARY_UPDATE_FILE_SCAN = "Cập nhật hồ sơ hợp đồng ";
        //Căn hộ
        public const string CANCEL = "Hủy cọc";
        public const string DEPOSIT = "Đã cọc";
        public const string OPEN = "Mở căn";
        public const string LOCK = "Khoá căn";
        public const string HOLD = "Căn đang được giữ chỗ";
        public const string PAY = "Căn hộ đã được thanh toán cọc";
        public const string DISTRIBUTION = "Phân phối căn hộ";
        public const string UPDATE = "Cập nhật thông tin";
        public const string INITIALIZE = "Khởi tạo căn hộ";
    }
}
