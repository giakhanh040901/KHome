using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    public static class GarnerHistoryUpdateSummary
    {
        public const string SUMMARY_GENNERAL_INFORMATION = "Thông tin chung";
        public const string SUMMARY_PRODUCT_TRADING_PROVIDER = "Đại lý phân phối";
        public const string SUMMARY_COLLATERAL = "Tài sản đảm bảo";
        public const string SUMMARY_LEGAL_RECORDS = "Hồ sơ pháp lý";
        //Distribution
        public const string SUMMARY_UPDATE_DISTRIBUTION_BANK_COLLECT = "Cập nhật tài khoản nhận tiền";
        public const string SUMMARY_UPDATE_DISTRIBUTION_BANK_PAY = "Cập nhật tài khoản chi tiền";

        public const string SUMMARY_UPDATE_OPEN_CELL_DATE = "Cập nhật ngày bắt đầu bán";
        public const string SUMMARY_UPDATE_CLOSE_CELL_DATE = "Cập nhật ngày kết thúc bán";

        public const string SUMMARY_ADD_PRODUC_RPICE   = "Thêm mới bảng giá";
        public const string SUMMARY_DELETE_PRODUC_RPICE   = "Xóa bảng giá";

        public const string SUMMARY_ADD_POLICY   = "Thêm chính sách";
        public const string SUMMARY_ADD_POLICY_DETAIL   = "Thêm kỳ hạn";

        //Policy
        public const string SUMMARY_UPDATE_CODE = "Cập nhật mã chính sách";
        public const string SUMMARY_UPDATE_NAME = "Cập nhật tên chính sách";
        public const string SUMMARY_UPDATE_MIN_MONEY = "Cập nhật số tiền đầu tư tối thiểu";
        public const string SUMMARY_UPDATE_MAX_MONEY = "Cập nhật số tiền đầu tư tối đa";
        public const string SUMMARY_UPDATE_MIN_INVEST_DAY = "Cập nhật số ngày đầu tư tối thiểu";
        public const string SUMMARY_UPDATE_INCOME_TAX = "Cập nhật thuế lợi nhuận";
        public const string SUMMARY_UPDATE_INVESTOR_TYPE = "Cập nhật loại nhà đầu tư";
        public const string SUMMARY_UPDATE_CLASSIFY = "Cập nhật phân loại chính sách sản phẩm";
        public const string SUMMARY_UPDATE_GARNER_TYPE = "Cập nhật loại hình kỳ hạn";
        public const string SUMMARY_UPDATE_INTEREST_TYPE = "Cập nhật kiểu trả lợi tức";

        public const string SUMMARY_UPDATE_CALCULATE_TYPE = "Cập nhật loại hình lợi tức";
        public const string SUMMARY_UPDATE_ORDER_OF_WITHDRAWAL = "Cập nhật thứ tự rút tiền";
        public const string SUMMARY_UPDATE_MIN_WITHDRAWAL = "Cập nhật số tiền rút tối thiểu";
        public const string SUMMARY_UPDATE_MAX_WITHDRAWAL = "Cập nhật số tiền rút tối đa";
        public const string SUMMARY_UPDATE_WITHDRAWAL_FEE = "Cập nhật phí rút tiền";
        public const string SUMMARY_UPDATE_WITHDRAWAL_FEE_TYPE = "Cập nhật kiểu tính phí rút vốn";
        public const string SUMMARY_UPDATE_IS_TRANSFER_ASSETS = "Cập nhật chuyển đổi tài sản";
        public const string SUMMARY_UPDATE_TRANSFER_ASSETS_FEE = "Cập nhật phí chuyển đổi tài sản";
        public const string SUMMARY_UPDATE_SORT_ORDER = "Cập nhật thứ tự hiển thị";
        public const string SUMMARY_UPDATE_DESCRIPTION = "Cập nhật mô tả";
        public const string SUMMARY_UPDATE_START_DATE = "Cập nhật ngày bắt đầu";
        public const string SUMMARY_UPDATE_END_DATE = "Cập nhật ngày kết thúc";
        public const string SUMMARY_DELETE_POLICY = "Xóa chính sách";
        public const string SUMMARY_UPDATE_STATUS_POLICY = "Cập nhật trạng thái chính sách";
        //PolicyDetail
        public const string SUMMARY_UPDATE_POLICY_DETAIL_SORT_ORDER = "Cập nhật số thứ tự";
        public const string SUMMARY_UPDATE_POLICY_DETAIL_PERIOD_QUANTITY = "Cập nhật số kỳ đầu tư";
        public const string SUMMARY_UPDATE_SHORT_NAME = "Cập nhật tên viết tắt";
        public const string SUMMARY_UPDATE_POLICY_DETAIL_NAME = "Cập nhật tên kỳ hạn";
        public const string SUMMARY_UPDATE_PROFIT = "Cập nhật lợi nhuận";
        public const string SUMMARY_UPDATE_INTEREST_DAYS = "Cập nhật số ngày đầu tư";
        public const string SUMMARY_DELETE_POLICY_DETAIL = "Xóa kỳ hạn";
        //File Chính sách
        public const string SUMMARY_ADD_POLICY_FILE = "Thêm mới File chính sách";
        public const string SUMMARY_UPDATE_POLICY_FILE_URL = "Cập nhật File chính sách";
        public const string SUMMARY_UPDATE_POLICY_FILE_TITLE = "Cập nhật tên File chính sách";
        public const string SUMMARY_UPDATE_EFFECTIVE_DATE = "Cập nhật ngày có hiệu lực";
        public const string SUMMARY_UPDATE_EXPIRATION_DATE = "Cập nhật ngày hết hiệu lực";
        public const string SUMMARY_DELETE_POLICY_FILE = "Xóa file chính sách";
        //mẫu hợp đồng
        public const string SUMMARY_ADD_CONTRACT_TEMPLATE = "Thêm mới mẫu hợp đồng";
        public const string SUMMARY_DELETE_CONTRACT_TEMPLATE = "Xóa mẫu hợp đồng";
        public const string SUMMARY_UPDATE_CONTRACT_SOURCE = "Cập nhật kiểu hợp đồng";
        public const string SUMMARY_UPDATE_POLICY_ID = "Cập nhật chính sách";
        public const string SUMMARY_UPDATE_CONFIG_CONTRACT_CODE = "Cập nhật cấu trúc mã hợp đồng";
        public const string SUMMARY_UPDATE_DISPLAY_TYPE = "Cập nhật loại hiển thị";
        public const string SUMMARY_UPDATE_CONTRACT_TEMPLATE_TEMP = "Cập nhật hợp đồng";
        public const string SUMMARY_UPDATE_CONTRACT_TEMPLATE_START_DATE = "Cập nhật ngày hiệu lực";
        public const string SUMMARY_UPDATE_CONTRACT_TEMPLATE_STATUS = "Cập nhật trạng thái mẫu hợp đồng";
        //Giao nhận hợp đồng
        public const string SUMMARY_UPDATE_NAME_RECEIVE_CONTRACT = "Cập nhật tên giao nhận hợp đồng";
        public const string SUMMARY_UPDATE_CODE_RECEIVE_CONTRACT = "Cập nhật mã giao nhận hợp đồng";
        public const string SUMMARY_UPDATE_FILE_RECEIVE_CONTRACT = "Cập nhật file giao nhận hợp đồng";
        public const string SUMMARY_UPDATE_STATUS_RECEIVE_CONTRACT = "Cập nhật trạng thái giao nhận hợp đồng";
        public const string SUMMARY_ADD_RECEIVE_CONTRACT = "Thêm mới nhận hợp đồng";
        public const string SUMMARY_DELETE_RECEIVE_CONTRACT = "Xóa giao nhận hợp đồng";
        // Hợp đồng
        public const string SUMMARY_UPDATE_SOURCE = "Cập nhật loại hình online sang offline";
        public const string SUMMARY_APPROVE_FILE = "Cập nhật duyệt hợp đồng ";
        public const string SUMMARY_CANCEL_FILE = "Cập nhật hủy duyệt hợp đồng ";
        public const string SUMMARY_PROCESS_CONTRACT = "Cập nhật xử lý yêu cầu nhận hợp đồng ";
        public const string SUMMARY_UPDATE_CONTRACT_FILE = "Cập nhật lại file hợp đồng ";
        public const string SUMMARY_UPDATE_CONTRACT_FILE_SIGN_PDF = "Cập nhật ký điện tử ";

    }
}
