using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.RolePermissions.Constant;
using System.Collections.Generic;

namespace EPIC.Utils.RolePermissions
{
    /// <summary>
    /// Cấu hình các permission trong hệ thống
    /// </summary>
    public static class PermissionConfig
    {
        public static readonly Dictionary<string, PermissionContent> Configs = new()
        {
            #region ecore
            { Permissions.CoreWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "" } },
            { Permissions.CorePageDashboard, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Dashboard tổng thể" } },

            { Permissions.CoreThongTinDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Thông tin doanh nghiệp" } },
            { Permissions.CoreTTDN_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreThongTinDoanhNghiep, Description = "Thông tin chung" } },
            { Permissions.CoreTTDN_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_ThongTinChung, Description = "Cập nhật" } },
            { Permissions.CoreTTDN_CauHinhChuKySo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_ThongTinChung, Description = "Cấu hình chữ kí số" } },

            { Permissions.CoreTTDN_TKNganHang, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreThongTinDoanhNghiep, Description = "Tài khoản ngân hàng" } },
            { Permissions.CoreTTDN_TKNH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_TKNganHang, Description = "Thêm mới" } },
            { Permissions.CoreTTDN_TKNH_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_TKNganHang, Description = "Set mặc định" } },

            { Permissions.CoreTTDN_GiayPhepDKKD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreThongTinDoanhNghiep, Description = "Giấy phép ĐKKD" } },
            { Permissions.CoreTTDN_GiayPhepDKKD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_GiayPhepDKKD, Description = "Thêm mới" } },
            { Permissions.CoreTTDN_GiayPhepDKKD_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_GiayPhepDKKD, Description = "Cập nhật" } },
            { Permissions.CoreTTDN_GiayPhepDKKD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreTTDN_GiayPhepDKKD, Description = "Xoá" } },

            { Permissions.Core_Menu_TK_UngDung, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Tài khoản ứng dụng" } },

            { Permissions.Core_TK_ChuaXacMinh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_TK_UngDung, Description = "Tài khoản chưa xác minh" } },
            { Permissions.Core_TK_ChuaXacMinh_XacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_TK_ChuaXacMinh, Description = "Xác minh thông tin" } },
            { Permissions.Core_TK_ChuaXacMinh_ResetMatKhau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_TK_ChuaXacMinh, Description = "Reset mật khẩu" } },
            { Permissions.Core_TK_ChuaXacMinh_XoaTaiKhoan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_TK_ChuaXacMinh, Description = "Xóa tài khoản" } },

            { Permissions.CoreMenuInvestorAccount, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_TK_UngDung, Description = "Tài khoản người dùng" } },
             { Permissions.CoreMenuInvestorAccount_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuInvestorAccount, Description = "Chi tiết" } },
            { Permissions.CoreMenuInvestorAccount_ChangeStatus, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuInvestorAccount, Description = "Đóng/ Mở tài khoản" } },
            { Permissions.CoreMenuInvestorAccount_ResetMatKhau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuInvestorAccount, Description = "Reset mật khẩu" } },
            { Permissions.CoreMenuInvestorAccount_DatMaPin, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuInvestorAccount, Description = "Đặt mã pin" } },
            { Permissions.CoreMenuInvestorAccount_XoaTaiKhoan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuInvestorAccount, Description = "Xóa tài khoản" } },
            //Quản lý khách hàng cá nhân
            { Permissions.CoreMenuKhachHang, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Quản lý khách hàng" } },
            
           //Quản lý khách hàng cá nhân chưa duyệt
            { Permissions.CoreMenuDuyetKHCN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKhachHang, Description = "Duyệt KH cá nhân" } },
            { Permissions.CoreDuyetKHCN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHCN, Description = "Danh sách" } },
            { Permissions.CoreDuyetKHCN_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHCN, Description = "Thêm mới" } },
                
                //Chi tiết khách hàng
                { Permissions.CoreDuyetKHCN_ThongTinKhachHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHCN, Description = "Thông tin khách hàng" } },
                { Permissions.CoreDuyetKHCN_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinKhachHang, Description = "Thông tin chung" } },
                { Permissions.CoreDuyetKHCN_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreDuyetKHCN_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinChung, Description = "Cập nhật" } },
                { Permissions.CoreDuyetKHCN_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinChung, Description = "Trình duyệt" } },
                { Permissions.CoreDuyetKHCN_CheckFace, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinChung, Description = "Kiểm tra mặt" } },
                
                // tab TKNH = Tài khoản ngân hàng
                { Permissions.CoreDuyetKHCN_TKNH, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinKhachHang, Description = "Tài khoản ngân hàng" } },
                { Permissions.CoreDuyetKHCN_TKNH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKNH, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHCN_TKNH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKNH, Description = "Thêm mới" } },
                { Permissions.CoreDuyetKHCN_TKNH_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKNH, Description = "Set mặc định" } },
                { Permissions.CoreDuyetKHCN_TKNH_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKNH, Description = "Sửa" } },
                { Permissions.CoreDuyetKHCN_TKNH_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKNH, Description = "Xóa" } },
                
                // Tab quản lý giấy tờ
                { Permissions.CoreDuyetKHCN_GiayTo, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinKhachHang, Description = "Quản lý giấy tờ" } },
                { Permissions.CoreDuyetKHCN_GiayTo_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_GiayTo, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHCN_GiayTo_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_GiayTo, Description = "Thêm mới" } },
                 { Permissions.CoreDuyetKHCN_GiayTo_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_GiayTo, Description = "Cập nhật" } },
                { Permissions.CoreDuyetKHCN_GiayTo_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_GiayTo, Description = "Set mặc định" } },
                
                // tab Quản lý địa chỉ liên hệ
                { Permissions.CoreDuyetKHCN_DiaChi, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinKhachHang, Description = "Địa chỉ liên hệ" } },
                { Permissions.CoreDuyetKHCN_DiaChi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_DiaChi, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHCN_DiaChi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_DiaChi, Description = "Thêm mới" } },
                { Permissions.CoreDuyetKHCN_DiaChi_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_DiaChi, Description = "Set mặc định" } },
                
                // tab TKCK
                { Permissions.CoreDuyetKHCN_TKCK, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_ThongTinKhachHang, Description = "Tài khoản chứng khoán" } },
                { Permissions.CoreDuyetKHCN_TKCK_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKCK, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHCN_TKCK_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKCK, Description = "Thêm mới" } },
                { Permissions.CoreDuyetKHCN_TKCK_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHCN_TKCK, Description = "Set mặc định" } },
               
                // Quản lý khách hàng đã duyệt
                { Permissions.CoreMenuKHCN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKhachHang, Description = "Khách hàng cá nhân" } },
                { Permissions.CoreKHCN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKHCN, Description = "Danh sách" } },
                { Permissions.CoreKHCN_XacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKHCN, Description = "Epic xác minh" } },
                
                // Chi tiết khách hàng (Thông tin chung)
                { Permissions.CoreKHCN_ThongTinKhachHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKHCN, Description = "Thông tin khách hàng" } },
                { Permissions.CoreKHCN_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Thông tin chung" } },
                { Permissions.CoreKHCN_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreKHCN_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinChung, Description = "Cập nhật" } },
                { Permissions.CoreKHCN_CheckFace, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinChung, Description = "Kiểm tra mặt" } },
                { Permissions.CoreKHCN_Phone_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinChung, Description = "Yêu cầu đổi số điện thoại" } },
                { Permissions.CoreKHCN_Email_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinChung, Description = "Yêu cầu đổi email" } },
                
                // Tab tài khoản ngân hàng
                { Permissions.CoreKHCN_TKNH, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Tài khoản ngân hàng" } },
                { Permissions.CoreKHCN_TKNH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKNH, Description = "Danh sách" } },
                { Permissions.CoreKHCN_TKNH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKNH, Description = "Thêm mới" } },
                { Permissions.CoreKHCN_TKNH_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKNH, Description = "Set mặc định" } },
                { Permissions.CoreKHCN_TKNH_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKNH, Description = "Sửa" } },
                { Permissions.CoreKHCN_TKNH_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKNH, Description = "Xóa" } },
                // Tab tài khoản đăng nhập
                { Permissions.CoreKHCN_Account, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Tài khoản đăng nhập" } },
                { Permissions.CoreKHCN_Account_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_Account, Description = "Danh sách" } },
                { Permissions.CoreKHCN_Account_ResetPassword, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_Account, Description = "Reset mật khẩu" } },
                { Permissions.CoreKHCN_Account_ResetPin, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_Account, Description = "Reset mã Pin" } },
                { Permissions.CoreKHCN_Account_ChangeStatus, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_Account, Description = "Đóng/ Mở tài khoản" } },
                { Permissions.CoreKHCN_Account_Delete, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_Account, Description = "Xóa tài khoản" } },
                // Tab quản lý giấy tờ
                { Permissions.CoreKHCN_GiayTo, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Quản lý giấy tờ" } },
                { Permissions.CoreKHCN_GiayTo_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_GiayTo, Description = "Danh sách" } },
                { Permissions.CoreKHCN_GiayTo_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_GiayTo, Description = "Thêm mới" } },
                { Permissions.CoreKHCN_GiayTo_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_GiayTo, Description = "Cập nhật" } },
                { Permissions.CoreKHCN_GiayTo_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_GiayTo, Description = "Set mặc định" } },
                
                // Tab quản lý địa chỉ liên hệ
                { Permissions.CoreKHCN_DiaChi, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Địa chỉ liên hệ" } },
                { Permissions.CoreKHCN_DiaChi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_DiaChi, Description = "Danh sách" } },
                { Permissions.CoreKHCN_DiaChi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_DiaChi, Description = "Thêm mới" } },
                { Permissions.CoreKHCN_DiaChi_SetDefault, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_DiaChi, Description = "Set mặc định" } },
               
                 // Tab Chứng minh Nhà đầu tư chuyên nghiệp
                { Permissions.CoreKHCN_NDTCN, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Chứng minh NĐTCN" } },
                { Permissions.CoreKHCN_NDTCN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_NDTCN, Description = "Danh sách" } },
                
                // tab quản lý tư vấn viên ; TVV = Tư vấn viên
                { Permissions.CoreKHCN_TuVanVien, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Tư vấn viên" } },
                { Permissions.CoreKHCN_TuVanVien_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TuVanVien, Description = "Danh sách" } },
                { Permissions.CoreKHCN_TuVanVien_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TuVanVien, Description = "Thêm mới" } },
                { Permissions.CoreKHCN_TuVanVien_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TuVanVien, Description = "Set mặc định" } },

                // tab quản lý người giới thiệu
                { Permissions.CoreKHCN_NguoiGioiThieu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Người giới thiệu" } },
                { Permissions.CoreKHCN_NguoiGioiThieu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_NguoiGioiThieu, Description = "Danh sách" } },
                 
                // Tab tài khoản chứng khoán
                { Permissions.CoreKHCN_TKCK, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_ThongTinKhachHang, Description = "Tài khoản chứng khoán" } },
                { Permissions.CoreKHCN_TKCK_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKCK, Description = "Danh sách" } },
                { Permissions.CoreKHCN_TKCK_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKCK, Description = "Thêm mới" } },
                { Permissions.CoreKHCN_TKCK_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHCN_TKCK, Description = "Set mặc định" } },
                
                // Module Duyệt KHDN = Khách hàng doanh nghiệp
                { Permissions.CoreMenuDuyetKHDN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKhachHang, Description = "Duyệt KH doanh nghiệp" } },
                { Permissions.CoreDuyetKHDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHDN, Description = "Danh sách" } },
                  { Permissions.CoreDuyetKHDN_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHDN, Description = "Thêm mới" } },
                
                // Chi tiết khách hàng
                { Permissions.CoreDuyetKHDN_ThongTinKhachHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetKHDN, Description = "Thông tin khách hàng" } },
                { Permissions.CoreDuyetKHDN_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinKhachHang, Description = "Thông tin chung" } },
                { Permissions.CoreDuyetKHDN_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinChung, Description = "Chi tiết" } },
                { Permissions.CoreDuyetKHDN_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinChung, Description = "Cập nhật" } },
               { Permissions.CoreDuyetKHDN_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinChung, Description = "Trình duyệt" } },
                
                // Tab tài khoản ngân hàng
                { Permissions.CoreDuyetKHDN_TKNH, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinKhachHang, Description = "Tài khoản ngân hàng" } },
                { Permissions.CoreDuyetKHDN_TKNH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHDN_TKNH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Thêm mới" } },
                { Permissions.CoreDuyetKHDN_TKNH_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Set mặc định" } },

                // Tab Chữ ký số
                { Permissions.CoreDuyetKHDN_CKS, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinKhachHang, Description = "Chữ ký số" } },
                { Permissions.CoreDuyetKHDN_CauHinhChuKySo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_CKS, Description = "Cấu hình chữ ký số" } },
                
                // Tab Đăng ký kinh doanh
                { Permissions.CoreDuyetKHDN_DKKD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_ThongTinKhachHang, Description = "'Đăng ký kinh doanh" } },
                { Permissions.CoreDuyetKHDN_DKKD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Danh sách" } },
                { Permissions.CoreDuyetKHDN_DKKD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Thêm mới" } },
                { Permissions.CoreDuyetKHDN_DKKD_XemFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Xem file" } },
                { Permissions.CoreDuyetKHDN_DKKD_TaiFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Tải file" } },
                { Permissions.CoreDuyetKHDN_DKKD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Cập nhật" } },
                { Permissions.CoreDuyetKHDN_DKKD_XoaFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_DKKD, Description = "Xóa file" } },
                
                //=======================================

                // Quản lý khách hàng doanh nghiệp đã duyệt
                { Permissions.CoreMenuKHDN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Khách hàng doanh nghiệp" } },
                { Permissions.CoreKHDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Danh sách" } },
                { Permissions.CoreKHDN_XacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetKHDN_TKNH, Description = "Xác minh" } },
                 
                // Chi tiết khách hàng 
                { Permissions.CoreKHDN_ThongTinKhachHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuKHDN, Description = "Thông tin khách hàng" } },
                { Permissions.CoreKHDN_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinKhachHang, Description = "Thông tin chung" } },
                { Permissions.CoreKHDN_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreKHDN_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinChung, Description = "Cập nhật" } },
               
                // Tab tài khoản ngân hàng
                { Permissions.CoreKHDN_TKNH, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinKhachHang, Description = "Tài khoản ngân hàng" } },
                { Permissions.CoreKHDN_TKNH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_TKNH, Description = "Danh sách" } },
                { Permissions.CoreKHDN_TKNH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_TKNH, Description = "Thêm mới" } },
                { Permissions.CoreKHDN_TKNH_SetDefault, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_TKNH, Description = "Set mặc định" } },
                
                // Tab Chữ ký số
                { Permissions.CoreKHDN_CKS, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinKhachHang, Description = "Chữ ký số" } },
                { Permissions.CoreKHDN_CauHinhChuKySo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_CKS, Description = "Cấu hình chữ ký số" } },

                 // Tab Đăng ký kinh doanh
                { Permissions.CoreKHDN_DKKD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_ThongTinKhachHang, Description = "'Đăng ký kinh doanh" } },
                { Permissions.CoreKHDN_DKKD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Danh sách" } },
                { Permissions.CoreKHDN_DKKD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Thêm mới" } },
                { Permissions.CoreKHDN_DKKD_XemFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Xem file" } },
                { Permissions.CoreKHDN_DKKD_TaiFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Tải file" } },
                { Permissions.CoreKHDN_DKKD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Cập nhật" } },
                { Permissions.CoreKHDN_DKKD_XoaFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreKHDN_DKKD, Description = "Xóa file" } },
                
                // Quản lý Sale 
                { Permissions.CoreMenuSale, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Quản lý Sale" } },
               
                // Danh sách Sale chưa duyệt ------- Start
                { Permissions.CoreMenuDuyetSale, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSale, Description = "Duyệt Sale" } },
                { Permissions.CoreDuyetSale_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetSale, Description = "Danh sách" } },
                { Permissions.CoreDuyetSale_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetSale, Description = "Thêm mới" } },
                { Permissions.CoreDuyetSale_ThongTinSale, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuDuyetSale, Description = "Thông tin Sale" } },
               
                // Chi tiết sale chưa duyệt
                { Permissions.CoreDuyetSale_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetSale_ThongTinSale, Description = "Thông tin chung" } },
                { Permissions.CoreDuyetSale_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetSale_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreDuyetSale_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetSale_ThongTinChung, Description = "Cập nhật" } },
                { Permissions.CoreDuyetSale_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDuyetSale_ThongTinChung, Description = "Trình duyệt" } },

                // Danh sách sale đã duyệt
                { Permissions.CoreMenuSaleActive, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSale, Description = "Sale" } },
                { Permissions.CoreSaleActive_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSaleActive, Description = "Danh sách" } },
                { Permissions.CoreSaleActive_KichHoat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSaleActive, Description = "Kích hoạt/ Khóa Sale" } },
                { Permissions.CoreSaleActive_ThongTinSale, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSaleActive, Description = "Thông tin Sale" } },

                // Chi tiết sale đã duyệt
                { Permissions.CoreSaleActive_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_ThongTinSale, Description = "Thông tin chung" } },
                { Permissions.CoreSaleActive_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreSaleActive_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_ThongTinChung, Description = "Cập nhật" } },
 
                // Tab Hợp đồng đầu tư = HDDT
                { Permissions.CoreSaleActive_HDCT, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_ThongTinSale, Description = "Hợp đồng cộng tác" } },
                { Permissions.CoreSaleActive_HDCT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Danh sách" } },
                { Permissions.CoreSaleActive_HDCT_UpdateFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Cập nhật hồ sơ" } },
                { Permissions.CoreSaleActive_HDCT_Sign, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Ký điện tử" } },
                //{ Permissions.CoreSaleActive_HDCT_UploadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Tải lên hợp đồng" } },
                //{ Permissions.CoreSaleActive_HDCT_Preview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Xem hồ sơ tải lên" } },
                //{ Permissions.CoreSaleActive_HDCT_Download, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Tải hợp đồng" } },
                //{ Permissions.CoreSaleActive_HDCT_Download_Sign, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreSaleActive_HDCT, Description = "Tải hợp đồng chữ ký điện tử" } },

                // Danh sách Sale App
                { Permissions.CoreMenuSaleApp, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSale, Description = "Sale App" } },
                { Permissions.CoreSaleApp_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSaleApp, Description = "Danh sách" } },
                { Permissions.CoreSaleApp_DieuHuong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSaleApp, Description = "Điều hướng / Hủy" } },
                
                // Mẫu hợp đồng cộng tác 
                { Permissions.CoreMenu_HDCT_Template, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSale, Description = "Mẫu hợp đồng cộng tác" } },
                { Permissions.CoreHDCT_Template_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Danh sách" } },
                { Permissions.CoreHDCT_Template_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Thêm mới" } },
                { Permissions.CoreHDCT_Template_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Cập nhật" } },
                { Permissions.CoreHDCT_Template_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Xóa mẫu" } },
                { Permissions.CoreHDCT_Template_Preview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Xem mẫu hợp đồng" } },
                { Permissions.CoreHDCT_Template_Download, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HDCT_Template, Description = "Tải mẫu hợp đồng" } },
                
                // Phong ban
                { Permissions.CoreMenu_PhongBan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenuSale, Description = "Menu phòng ban" } },
                { Permissions.CorePhongBan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Danh sách" } },
                { Permissions.CorePhongBan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Thêm mới" } },
                { Permissions.CorePhongBan_ThemQuanLy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Thêm quản lý" } },
                { Permissions.CorePhongBan_ThemQuanLyDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Thêm quản lý doanh nghiệp" } },
                { Permissions.CorePhongBan_XoaQuanLy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Xóa quản lý" } },
                { Permissions.CorePhongBan_XoaQuanLyDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Xóa quản lý doanh nghiệp" } },
                { Permissions.CorePhongBan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Phê duyệt / Hủy" } },
                { Permissions.CorePhongBan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_PhongBan, Description = "Xóa" } },

                //Ql đối tác ------- Start
                { Permissions.CoreMenu_QLDoiTac, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Quản lý đối tác" } },
                //Đối tác
                { Permissions.CoreMenu_DoiTac, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLDoiTac, Description = "Đối tác" } },
                { Permissions.CoreDoiTac_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DoiTac, Description = "Danh sách" } },
                { Permissions.CoreDoiTac_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DoiTac, Description = "Thêm mới" } },
                { Permissions.CoreDoiTac_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DoiTac, Description = "Xóa" } },
                { Permissions.CoreDoiTac_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DoiTac, Description = "Thông tin chi tiết" } },

                // Tab thông tin chung
                { Permissions.CoreDoiTac_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_ThongTinChiTiet, Description = "Thông tin chung" } },
                { Permissions.CoreDoiTac_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreDoiTac_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_ThongTinChung, Description = "Cập nhật" } },
                
                //Đại lý
                { Permissions.CoreMenu_DaiLy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLDoiTac, Description = "Đối tác" } },
                { Permissions.CoreDaiLy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DaiLy, Description = "Danh sách" } },
                { Permissions.CoreDaiLy_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DaiLy, Description = "Thêm mới" } },
                { Permissions.CoreDaiLy_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_DaiLy, Description = "Thông tin chi tiết" } },

                // Tab thông tin chung
                { Permissions.CoreDaiLy_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDaiLy_ThongTinChiTiet, Description = "Thông tin chung" } },
                { Permissions.CoreDaiLy_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDaiLy_ThongTinChung, Description = "Xem chi tiết" } },
               
                // Tab quản lý tài khoản đăng nhập
                { Permissions.CoreDoiTac_Account, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_ThongTinChiTiet, Description = "Tài khoản" } },
                { Permissions.CoreDoiTac_Account_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_Account, Description = "Danh sách" } },
                { Permissions.CoreDoiTac_Account_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreDoiTac_Account, Description = "Thêm mới" } },
 
                // Truyền thông ------- Start
                { Permissions.CoreMenu_TruyenThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Truyền thông" } },

                //Truyền thông -tin tức
                { Permissions.CoreMenu_TinTuc, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TruyenThong, Description = "Tin tức" } },
                { Permissions.CoreTinTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TinTuc, Description = "Danh sách" } },
                { Permissions.CoreTinTuc_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TinTuc, Description = "Thêm mới" } },
                { Permissions.CoreTinTuc_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TinTuc, Description = "Xóa" } },
                { Permissions.CoreTinTuc_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TinTuc, Description = "Cập nhật" } },
                { Permissions.CoreTinTuc_PheDuyetDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TinTuc, Description = "Phê duyệt đăng" } },
                
                // Truyền thông - Hình ảnh
                { Permissions.CoreMenu_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TruyenThong, Description = "Hình ảnh" } },
                { Permissions.CoreHinhAnh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HinhAnh, Description = "Danh sách" } },
                { Permissions.CoreHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HinhAnh, Description = "Thêm mới" } },
                { Permissions.CoreHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HinhAnh, Description = "Xóa" } },
                { Permissions.CoreHinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HinhAnh, Description = "Cập nhật" } },
                { Permissions.CoreHinhAnh_PheDuyetDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_HinhAnh, Description = "Phê duyệt đăng" } },

                // Truyền thông - Kiến thức đầu tư
                { Permissions.CoreMenu_KienThucDauTu, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TruyenThong, Description = "Kiến thức đầu tư" } },
                { Permissions.CoreKienThucDauTu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_KienThucDauTu, Description = "Danh sách" } },
                { Permissions.CoreKienThucDauTu_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_KienThucDauTu, Description = "Thêm mới" } },
                { Permissions.CoreKienThucDauTu_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_KienThucDauTu, Description = "Xóa" } },
                { Permissions.CoreKienThucDauTu_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_KienThucDauTu, Description = "Cập nhật" } },
                { Permissions.CoreKienThucDauTu_PheDuyetDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_KienThucDauTu, Description = "Phê duyệt đăng" } },

                //Truyền thông - Hòm thư góp ý
                { Permissions.CoreMenu_HomThuGopY, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_TruyenThong, Description = "Hòm thư góp ý" } },

                // Menu Thông báo -------Start
                { Permissions.CoreMenu_ThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Thông báo" } },

                //// Thông báo - Thông báo mặc định
                //{ Permissions.CoreMenu_ThongBaoMacDinh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBao, Description = "Thông báo mặc định" } },
                //{ Permissions.CoreThongBaoMacDinh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBaoMacDinh, Description = "Cập nhật cài đặt" } },

               // // Thông báo - Cấu hình thông báo hệ thống
               //{ Permissions.CoreMenu_CauHinhThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBao, Description = "Cấu hình thông báo" } },
               // { Permissions.CoreCauHinhThongBaoHeThong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhThongBaoHeThong, Description = "Cập nhật cài đặt" } },

                // Thông báo - Mẫu thông báo
                { Permissions.CoreMenu_MauThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBao, Description = "Mẫu thông báo" } },
                { Permissions.CoreMauThongBao_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MauThongBao, Description = "Danh sách" } },
                { Permissions.CoreMauThongBao_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MauThongBao, Description = "Thêm mới" } },
                { Permissions.CoreMauThongBao_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MauThongBao, Description = "Cập nhật" } },
                { Permissions.CoreMauThongBao_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MauThongBao, Description = "Xóa" } },
                { Permissions.CoreMauThongBao_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MauThongBao, Description = "Kích hoạt / Hủy kích hoạt" } },

                // Thông báo - Quản lý thông báo
                { Permissions.CoreMenu_QLTB, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBao, Description = "Quản lý thông báo" } },
                { Permissions.CoreQLTB_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLTB, Description = "Danh sách" } },
                { Permissions.CoreQLTB_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLTB, Description = "Thêm mới" } },
                { Permissions.CoreQLTB_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLTB, Description = "Xóa" } },
                { Permissions.CoreQLTB_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLTB, Description = "Kích hoạt / Hủy kích hoạt" } },
                { Permissions.CoreQLTB_PageChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLTB, Description = "Thông tin chi tiết" } },

                ////Thông báo - Cấu hình nhà cung cấp
                //{ Permissions.CoreMenu_CauHinhNCC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBao, Description = "Thông báo" } },
                //{ Permissions.CoreCauHinhNCC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Danh sách" } },
                //{ Permissions.CoreCauHinhNCC_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Thêm mới" } },
                //{ Permissions.CoreCauHinhNCC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Cập nhật" } },

                // Chi tiết thông báo
                { Permissions.CoreQLTB_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_PageChiTiet, Description = "Thông tin chung" } },
                { Permissions.CoreQLTB_PageChiTiet_ThongTin, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.CoreQLTB_PageChiTiet_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_ThongTinChung, Description = "Cập nhật" } },
    
                //Tab gửi thông báo
                { Permissions.CoreQLTB_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_PageChiTiet, Description = "Gửi thông báo" } },
                { Permissions.CoreQLTB_PageChiTiet_GuiThongBao_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_GuiThongBao, Description = "Danh sách thông báo" } },
                { Permissions.CoreQLTB_PageChiTiet_GuiThongBao_CaiDat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLTB_GuiThongBao, Description = "Cài đặt danh sách thông báo" } },

                // Menu Thiết lập -------Start
                { Permissions.CoreMenu_ThietLap, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Thiết lập" } },

                 // Thiết lập - Thông báo mặc định
                { Permissions.CoreMenu_ThongBaoMacDinh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Thông báo mặc định" } },
                { Permissions.CoreThongBaoMacDinh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThongBaoMacDinh, Description = "Cập nhật cài đặt" } },


                 // Thiết lập - Cấu hình thông báo hệ thống
               { Permissions.CoreMenu_CauHinhThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Cấu hình thông báo" } },
                { Permissions.CoreCauHinhThongBaoHeThong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhThongBaoHeThong, Description = "Cập nhật cài đặt" } },

                //Thiết lập - Cấu hình nhà cung cấp
                { Permissions.CoreMenu_CauHinhNCC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Thông báo" } },
                { Permissions.CoreCauHinhNCC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Danh sách" } },
                { Permissions.CoreCauHinhNCC_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Thêm mới" } },
                { Permissions.CoreCauHinhNCC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhNCC, Description = "Cập nhật" } },

                 //Thiết lập - Cấu hình chữ ký số
                { Permissions.CoreMenu_CauHinhCKS, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Thiết lập" } },
                { Permissions.CoreCauHinhCKS_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhCKS, Description = "Danh sách" } },
                { Permissions.CoreCauHinhCKS_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhCKS, Description = "Cập nhật" } },

                //Thiết lập - whitelist ip
                { Permissions.CoreMenu_WhitelistIp, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Thiết lập" } },
                { Permissions.CoreWhitelistIp_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_WhitelistIp, Description = "Danh sách" } },
                { Permissions.CoreWhitelistIp_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_WhitelistIp, Description = "Thêm mới" } },
                { Permissions.CoreWhitelistIp_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_WhitelistIp, Description = "Thông tin chi tiết" } },
                { Permissions.CoreWhitelistIp_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_WhitelistIp, Description = "Cập nhật" } },
                { Permissions.CoreWhitelistIp_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_WhitelistIp, Description = "Xóa" } },

                 //Thiết lập - Msb Prefix
                { Permissions.CoreMenu_MsbPrefix, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Thiết lập" } },
                { Permissions.CoreMsbPrefix_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MsbPrefix, Description = "Danh sách" } },
                { Permissions.CoreMsbPrefix_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MsbPrefix, Description = "Thêm mới" } },
                { Permissions.CoreMsbPrefix_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MsbPrefix, Description = "Thông tin chi tiết" } },
                { Permissions.CoreMsbPrefix_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MsbPrefix, Description = "Cập nhật" } },
                { Permissions.CoreMsbPrefix_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_MsbPrefix, Description = "Xóa" } },

                  //Thiết lập - Cau hinh he thong
                { Permissions.CoreMenu_CauHinhHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Cau hinh he thong" } },
                { Permissions.CoreCauHinhHeThong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhHeThong, Description = "Danh sách" } },
                { Permissions.CoreCauHinhHeThong_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhHeThong, Description = "Thêm mới" } },
                { Permissions.CoreCauHinhHeThong_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhHeThong, Description = "Thông tin chi tiết" } },
                { Permissions.CoreCauHinhHeThong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhHeThong, Description = "Cập nhật" } },

                 //Thiết lập - Cau hinh cuoc goi
                { Permissions.CoreMenu_CauHinhCuocGoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_ThietLap, Description = "Cau hinh cuoc goi" } },
                { Permissions.CoreCauHinhCuocGoi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhCuocGoi, Description = "Danh sách" } },
                { Permissions.CoreCauHinhCuocGoi_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_CauHinhCuocGoi, Description = "Cập nhật" } },


                // Quản lý phê duyệt --------Start
                { Permissions.CoreMenu_QLPD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Quản lý phê duyệt" } },

                //  Phê duyệt khách hàng cá nhân
                { Permissions.CoreQLPD_KHCN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt khách hàng cá nhân" } },
                { Permissions.CoreQLPD_KHCN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHCN, Description = "Danh sách" } },
                { Permissions.CoreQLPD_KHCN_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHCN, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_KHCN_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHCN, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_KHCN_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHCN, Description = "Thông tin chi tiết" } },
               
                //  Phê duyệt khách hàng doanh nghiệp
                { Permissions.CoreQLPD_KHDN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt khách hàng doanh nghiệp" } },
                { Permissions.CoreQLPD_KHDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHDN, Description = "Danh sách" } },
                { Permissions.CoreQLPD_KHDN_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHDN, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_KHDN_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHDN, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_KHDN_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_KHDN, Description = "Thông tin chi tiết" } },

                //Phe duyet sale
                { Permissions.CoreQLPD_Sale, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt Sale" } },
                { Permissions.CoreQLPD_Sale_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Sale, Description = "Danh sách" } },
                { Permissions.CoreQLPD_Sale_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Sale, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_Sale_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Sale, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_Sale_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Sale, Description = "Thông tin chi tiết" } },

                // Phe duyet nha dau tu chuyen nghiep
                { Permissions.CoreQLPD_NDTCN, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt nhà đầu tư chuyên nghiệp" } },
                { Permissions.CoreQLPD_NDTCN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_NDTCN, Description = "Danh sách" } },
                { Permissions.CoreQLPD_NDTCN_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_NDTCN, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_NDTCN_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_NDTCN, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_NDTCN_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_NDTCN, Description = "Thông tin chi tiết" } },

                // Phe duyet email
                { Permissions.CoreQLPD_Email, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt email" } },
                { Permissions.CoreQLPD_Email_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Email, Description = "Danh sách" } },
                { Permissions.CoreQLPD_Email_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Email, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_Email_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Email, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_Email_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Email, Description = "Thông tin chi tiết" } },

                // Phe duyet so dien thoai
                { Permissions.CoreQLPD_Phone, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreMenu_QLPD, Description = "Phê duyệt số điện thoại" } },
                { Permissions.CoreQLPD_Phone_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Phone, Description = "Danh sách" } },
                { Permissions.CoreQLPD_Phone_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Phone, Description = "Phê duyệt / Hủy" } },
                { Permissions.CoreQLPD_Phone_XemLichSu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Phone, Description = "Xem lịch sử" } },
                { Permissions.CoreQLPD_Phone_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.CoreQLPD_Phone, Description = "Thông tin chi tiết" } },


                // Báo cáo
                { Permissions.Core_Menu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Core, ParentKey = null, Description = "Menu báo cáo" } },

                { Permissions.Core_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_BaoCao, Description = "Báo cáo quản trị" } },
                { Permissions.Core_BaoCao_QuanTri_DSSaler, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách saler" } },
                { Permissions.Core_BaoCao_QuanTri_DSKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách khách hàng" } },
                { Permissions.Core_BaoCao_QuanTri_DSKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách khách hàng root" } },
                { Permissions.Core_BaoCao_QuanTri_DSNguoiDung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách người dùng" } },
                { Permissions.Core_BaoCao_QuanTri_DSKhachHangHVF, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách khách hàng HVF" } },
                { Permissions.Core_BaoCao_QuanTri_SKTKNhaDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C Danh sách sao kê tài khoản nhà đầu tư" } },
                { Permissions.Core_BaoCao_QuanTri_TDTTKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C thay đổi thông tin khách hàng" } },
                { Permissions.Core_BaoCao_QuanTri_TDTTKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_QuanTri, Description = "B.C thay đổi thông tin khách hàng" } },

                { Permissions.Core_BaoCao_VanHanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_BaoCao, Description = "Báo cáo vận hành" } },
                { Permissions.Core_BaoCao_VanHanh_DSSaler, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C Danh sách saler" } },
                { Permissions.Core_BaoCao_VanHanh_DSKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C Danh sách khách hàng" } },
                { Permissions.Core_BaoCao_VanHanh_DSKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C Danh sách khách hàng root" } },
                { Permissions.Core_BaoCao_VanHanh_DSNguoiDung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C Danh sách người dùng" } },
                { Permissions.Core_BaoCao_VanHanh_TDTTKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C thay đổi thông tin khách hàng" } },
                { Permissions.Core_BaoCao_VanHanh_TDTTKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_VanHanh, Description = "B.C thay đổi thông tin khách hàng" } },

                { Permissions.Core_BaoCao_KinhDoanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_BaoCao, Description = "Báo cáo kinh doanh" } },
                { Permissions.Core_BaoCao_KinhDoanh_DSSaler, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_KinhDoanh, Description = "B.C Danh sách saler" } },
                { Permissions.Core_BaoCao_KinhDoanh_DSKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_KinhDoanh, Description = "B.C Danh sách khách hàng" } },
                { Permissions.Core_BaoCao_KinhDoanh_DSKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_KinhDoanh, Description = "B.C Danh sách khách hàng root" } },
                { Permissions.Core_BaoCao_KinhDoanh_DSNguoiDung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_KinhDoanh, Description = "B.C Danh sách người dùng" } },

                { Permissions.Core_BaoCao_HeThong, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_Menu_BaoCao, Description = "Báo cáo hệ thống" } },
                { Permissions.Core_BaoCao_HeThong_DSKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_HeThong, Description = "B.C Danh sách khách hàng root" } },
                { Permissions.Core_BaoCao_HeThong_TDTTKhachHangRoot, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Core, ParentKey = Permissions.Core_BaoCao_HeThong, Description = "B.C thay đổi thông tin khách hàng root" } },
            #endregion  

            #region trang ebond
            { Permissions.BondWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "" } },
            // Dashboard
            { Permissions.BondPageDashboard, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Dashboard tổng quan" } },
            // Menu cài đặt
            { Permissions.BondMenuCaiDat, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Cài đặt" } },
            // CHNNL: cấu hình ngày nghỉ lễ
            { Permissions.BondMenuCaiDat_CHNNL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Cấu hình ngày nghỉ lễ" } },
            { Permissions.BondCaiDat_CHNNL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CHNNL, Description = "Danh sách" } },
            { Permissions.BondCaiDat_CHNNL_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CHNNL, Description = "Cập nhật" } },

            // Cài đặt -> Tổ chức phát hành
            // TCPH: Tổ chức phát hành
            { Permissions.BondMenuCaiDat_TCPH, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Tổ chức phát hành" } },
            { Permissions.BondCaiDat_TCPH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_TCPH, Description = "Danh sách" } },
            { Permissions.BondCaiDat_TCPH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_TCPH, Description = "Thêm mới" } },
            { Permissions.Bond_TCPH_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_TCPH, Description = "Xóa" } },

            // TCPH - tab thông tin chi tiết
            { Permissions.Bond_TCPH_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_TCPH, Description = "Thông tin chi tiết" } },
            { Permissions.Bond_TCPH_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_TCPH_ThongTinChiTiet, Description = "Thông tin chung" } },
            { Permissions.Bond_TCPH_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_TCPH_ThongTinChung, Description = "Xem chi tiết" } },
            { Permissions.Bond_TCPH_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_TCPH_ThongTinChung, Description = "Cập nhật" } },
            
            // Cài đặt -> Đại lý sơ cấp
            // DLSC: Đại lý sơ cấp
            { Permissions.BondMenuCaiDat_DLSC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Đại lý sơ cấp" } },
            { Permissions.BondCaiDat_DLSC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLSC, Description = "Danh sách" } },
            { Permissions.BondCaiDat_DLSC_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLSC, Description = "Thêm mới" } },

            // Tab thông tin đại lý sơ cấp
            // Thông tin chung
            { Permissions.Bond_DLSC_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLSC, Description = "Thông tin chi tiết" } },
            { Permissions.Bond_DLSC_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLSC_ThongTinChiTiet, Description = "Thông tin chung" } },
            { Permissions.Bond_DLSC_TTC_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLSC_ThongTinChung, Description = "Xem chi tiết" } },
            // Tài khoản đăng nhập
            { Permissions.Bond_DLSC_TaiKhoanDangNhap, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLSC, Description = "Tài khoản đăng nhập" } },
            { Permissions.Bond_DLSC_TKDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLSC_TaiKhoanDangNhap, Description = "Danh sách" } },
            { Permissions.Bond_DLSC_TKDN_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLSC_TaiKhoanDangNhap, Description = "Thêm mới" } },

            // Cài đặt -> Đại lý lưu ký
            // DLLK: Đại lý lưu ký
            { Permissions.BondMenuCaiDat_DLLK, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Đại lý lưu ký" } },
            { Permissions.BondCaiDat_DLLK_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLLK, Description = "Danh sách" } },
            { Permissions.BondCaiDat_DLLK_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLLK, Description = "Thêm mới" } },
            { Permissions.Bond_DLLK_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLLK, Description = "Thêm mới" } },

            // Tab thông tin đại lý lưu ký
            // Thông tin chung
            { Permissions.Bond_DLLK_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_DLLK, Description = "Thông tin chi tiết" } },
            { Permissions.Bond_DLLK_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLLK_ThongTinChiTiet, Description = "Thông tin chung" } },
            { Permissions.Bond_DLLK_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_DLLK_ThongTinChung, Description = "Xem chi tiết" } },

            // Cài đặt -> Chính sách mẫu
            // CSM: Chính sách mẫu
            { Permissions.BondMenuCaiDat_CSM, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Chính sách mẫu" } },
            { Permissions.BondCaiDat_CSM_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Danh sách" } },
            { Permissions.BondCaiDat_CSM_ThemChinhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Thêm chính sách" } },
            { Permissions.BondCaiDat_CSM_CapNhatChinhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Cập nhật chính sách" } },
            { Permissions.BondCaiDat_CSM_XoaChinhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Xóa chính sách" } },
            { Permissions.BondCaiDat_CSM_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Kích hoạt / Hủy (Chính sách)" } },
            { Permissions.BondCaiDat_CSM_ThemKyHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Thêm kỳ hạn" } },
            { Permissions.BondCaiDat_CSM_CapNhatKyHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Cập nhật kỳ hạn" } },
            { Permissions.BondCaiDat_CSM_XoaKyHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Xóa kỳ hạn" } },
            { Permissions.BondCaiDat_CSM_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_CSM, Description = "Kích hoạt / Hủy (Kỳ hạn)" } },

            // Cài đặt -> Mẫu thông báo
            // MTB: Mẫu thông báo
            { Permissions.BondMenuCaiDat_MTB, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Mẫu thông báo" } },
            { Permissions.BondCaiDatMTB_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_MTB, Description = "Cài đặt mẫu thông báo" } },

            // Cài đặt -> Hình ảnh
            { Permissions.BondMenuCaiDat_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat, Description = "Hình ảnh" } },

            { Permissions.BondCaiDat_HinhAnh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_HinhAnh, Description = "Danh sách" } },
            { Permissions.BondCaiDat_HinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_HinhAnh, Description = "Thêm mới" } },
            { Permissions.BondCaiDat_HinhAnh_Sua, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_HinhAnh, Description = "Sửa" } },
            { Permissions.BondCaiDat_HinhAnh_DuyetDang, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_HinhAnh, Description = "Duyệt đăng" } },
            { Permissions.BondCaiDat_HinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuCaiDat_HinhAnh, Description = "Xóa" } },

            // Quản lý trái phiếu
            { Permissions.BondMenuQLTP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Quản lý trái phiếu" } },

            // QLTP -> Lô trái phiếu
            // LTP: Lô trái phiếu
            // TTCT: Thông tin chi tiết
            { Permissions.BondMenuQLTP_LTP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP, Description = "Lô trái phiếu" } },

            { Permissions.BondMenuQLTP_LTP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Danh sách" } },
            { Permissions.BondMenuQLTP_LTP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Thêm mới" } },
            { Permissions.BondMenuQLTP_LTP_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Trình duyệt" } },
            { Permissions.BondMenuQLTP_LTP_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Phê duyệt / Hủy" } },
            { Permissions.BondMenuQLTP_LTP_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Epic xác minh" } },
            { Permissions.BondMenuQLTP_LTP_DongTraiPhieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Đóng / Mở trái phiếu" } },
            { Permissions.BondMenuQLTP_LTP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Xóa" } },
            { Permissions.BondMenuQLTP_LTP_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP, Description = "Thông tin chi tiết" } },
            
            // Thông tin chi tiết
            { Permissions.Bond_LTP_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP_TTCT, Description = "Thông tin chung" } },
            { Permissions.Bond_LTP_TTC_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TTC, Description = "Xem chi tiết" } },
            { Permissions.Bond_LTP_TTC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TTC, Description = "Cập nhật" } },

            //{ Permissions.Bond_LTP_MoTa, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TTC, Description = "Mô tả" } },
            { Permissions.Bond_LTP_TSDB, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP_TTCT, Description = "Tài sản đảm bảo" } },
            { Permissions.Bond_LTP_TSDB_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TSDB, Description = "Danh sách" } },
            { Permissions.Bond_LTP_TSDB_Them, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TSDB, Description = "Thêm" } },
            { Permissions.Bond_LTP_TSDB_Sua, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TSDB, Description = "Sửa" } },
            { Permissions.Bond_LTP_TSDB_TaiXuong, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TSDB, Description = "Tải xuống" } },
            { Permissions.Bond_LTP_TSDB_Xoa, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TSDB, Description = "Xóa" } },


            { Permissions.Bond_LTP_HSPL, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP_TTCT, Description = "Hồ sơ pháp lý" } },
            { Permissions.Bond_LTP_HSPL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_HSPL, Description = "Danh sách" } },
            { Permissions.Bond_LTP_HSPL_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_HSPL, Description = "Thêm mới" } },
            { Permissions.Bond_LTP_HSPL_XemHoSo, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_HSPL, Description = "Xem hồ sơ" } },
            { Permissions.Bond_LTP_HSPL_Download, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_HSPL, Description = "Tải hồ sơ" } },
            { Permissions.Bond_LTP_HSPL_Xoa, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_HSPL, Description = "Xóa" } },

            { Permissions.Bond_LTP_TTTT, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_LTP_TTCT, Description = "Thông tin trái tức" } },
            { Permissions.Bond_LTP_TTTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_LTP_TTTT, Description = "Danh sách" } },

            // QLTP -> Phát hành sơ cấp
            // PHSC: Phát hành sơ cấp
            // TTCT: Thông tin chi tiết

            { Permissions.BondMenuQLTP_PHSC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP, Description = "Phát hành sơ cấp" } },

            { Permissions.BondMenuQLTP_PHSC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Danh sách" } },
            { Permissions.BondMenuQLTP_PHSC_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Thêm mới" } },
            { Permissions.BondMenuQLTP_PHSC_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Trình duyệt" } },
            { Permissions.BondMenuQLTP_PHSC_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Phê duyệt / Hủy" } },
            { Permissions.BondMenuQLTP_PHSC_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Xóa" } },

            // Thông tin chi tiết
            { Permissions.BondMenuQLTP_PHSC_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC, Description = "Thông tin chi tiết" } },
            { Permissions.Bond_PHSC_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC_TTCT, Description = "Thông tin chung" } },
            { Permissions.Bond_PHSC_TTC_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_PHSC_TTC, Description = "Xem chi tiết" } },
            { Permissions.Bond_PHSC_TTC_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_PHSC_TTC, Description = "Chỉnh sửa" } },


            //{ Permissions.Bond_PHSC_CSL, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_PHSC_TTCT, Description = "Chính sách lãi" } },

            // QLTP -> Hợp đồng phân phối
            // HDPP: Hợp đồng phân phối
            { Permissions.BondMenuQLTP_HDPP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP, Description = "Hợp đồng phân phối" } },

            { Permissions.BondMenuQLTP_HDPP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP, Description = "Danh sách" } },
            { Permissions.BondMenuQLTP_HDPP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP, Description = "Thêm mới" } },
            { Permissions.BondMenuQLTP_HDPP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP, Description = "Xóa" } },

            { Permissions.BondMenuQLTP_HDPP_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP, Description = "Thông tin chi tiết" } },
            { Permissions.Bond_HDPP_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP_TTCT, Description = "Thông tin chung" } },
            { Permissions.Bond_HDPP_TTC_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTC, Description = "Xem chi tiết" } },

            { Permissions.Bond_HDPP_TTThanhToan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP_TTCT, Description = "Thông tin thanh toán" } },
            { Permissions.Bond_HDPP_TTTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Danh sách" } },
            { Permissions.Bond_HDPP_TTTT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Thêm mới" } },
            { Permissions.Bond_HDPP_TTTT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Cập nhật" } },
            { Permissions.Bond_HDPP_TTTT_ChiTietThanhToan, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Chi tiết thanh toán" } },
            { Permissions.Bond_HDPP_TTTT_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Phê duyệt" } },
            { Permissions.Bond_HDPP_TTTT_HuyPheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Hủy phê duyệt" } },
            { Permissions.Bond_HDPP_TTTT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTThanhToan, Description = "Xóa" } },

            { Permissions.Bond_HDPP_DMHSKHK, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP_TTCT, Description = "Danh mục hồ sơ khách hàng ký" } },
            { Permissions.Bond_HDPP_DMHSKHK_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_DMHSKHK, Description = "Danh sách" } },
            { Permissions.Bond_HDPP_DMHSKHK_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_DMHSKHK, Description = "Thêm mới" } },
            { Permissions.Bond_HDPP_DMHSKHK_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_DMHSKHK, Description = "Phê duyệt" } },
            { Permissions.Bond_HDPP_DMHSKHK_HuyPheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_DMHSKHK, Description = "Hủy phê duyệt" } },
            { Permissions.Bond_HDPP_DMHSKHK_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_DMHSKHK, Description = "Xóa" } },

            { Permissions.Bond_HDPP_TTTraiTuc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_HDPP_TTCT, Description = "Thông tin trái tức" } },
            { Permissions.Bond_HDPP_TTTraiTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_HDPP_TTTraiTuc, Description = "Danh sách" } },

            // Bán theo kỳ hạn

            { Permissions.BondMenuQLTP_BTKH, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP, Description = "Bán theo kỳ hạn" } },
            { Permissions.BondMenuQLTP_BTKH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Danh sách" } },
            { Permissions.BondMenuQLTP_BTKH_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Thêm mới" } },
            { Permissions.BondMenuQLTP_BTKH_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Thêm mới" } },
            { Permissions.BondMenuQLTP_BTKH_DongTam, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Đóng tạm" } },
            { Permissions.BondMenuQLTP_BTKH_BatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Bật show app" } },
            { Permissions.BondMenuQLTP_BTKH_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Epic xác minh" } },
            { Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH, Description = "Thông tin chi tiết" } },

            { Permissions.Bond_BTKH_TTCT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Thông tin chung" } },
            { Permissions.Bond_BTKH_TTCT_ThongTinChung_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ThongTinChung, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ThongTinChung, Description = "Chỉnh sửa" } },
            //Tổng quan
            { Permissions.Bond_BTKH_TTCT_TongQuan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Tổng quan" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Chỉnh sửa" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_ChonAnh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Chọn ảnh" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_ThemToChuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Thêm tổ chức" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_DanhSach_ToChuc, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Danh sách tổ chức" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_UploadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Upload file" } },
            { Permissions.Bond_BTKH_TTCT_TongQuan_DanhSach_File, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_TongQuan, Description = "Danh sách file" } },

            //
            { Permissions.Bond_BTKH_TTCT_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Chính sách" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Thêm chính sách" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Cập nhật chính sách" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Bật tắt show App (Chính sách)" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Kích hoạt / Hủy (Chính sách)" } },
            { Permissions.Bond_BTKH_TTCT_ChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Xóa chính sách" } },
            //
            { Permissions.Bond_BTKH_TTCT_KyHan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Thêm kỳ hạn" } },
            { Permissions.Bond_BTKH_TTCT_KyHan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Cập nhật kỳ hạn" } },
            { Permissions.Bond_BTKH_TTCT_KyHan_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Bật tắt show App (Kỳ hạn)" } },
            { Permissions.Bond_BTKH_TTCT_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Kích hoạt / Hủy (Kỳ hạn)" } },
            { Permissions.Bond_BTKH_TTCT_KyHan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_ChinhSach, Description = "Xóa kỳ hạn" } },

            { Permissions.Bond_BTKH_TTCT_BangGia, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Bảng giá" } },
            { Permissions.Bond_BTKH_TTCT_BangGia_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_BangGia, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_BangGia_ImportExcelBG, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_BangGia, Description = "Import file excel bảng giá" } },
            { Permissions.Bond_BTKH_TTCT_BangGia_DownloadFileMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_BangGia, Description = "Download file mẫu" } },
            { Permissions.Bond_BTKH_TTCT_BangGia_XoaBangGia, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_BangGia, Description = "Xóa bảng giá" } },

            { Permissions.Bond_BTKH_TTCT_FileChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "File chính sách" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_UploadFileChinhSach, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Upload file chính sách" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Sửa" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_Download, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Tải file" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_XemFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Xem file" } },
            { Permissions.Bond_BTKH_TTCT_FileChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_FileChinhSach, Description = "Xóa" } },

            { Permissions.Bond_BTKH_TTCT_MauHopDong, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Mẫu hợp đồng" } },
            { Permissions.Bond_BTKH_TTCT_MauHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauHopDong, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_MauHopDong_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauHopDong, Description = "Thêm mới" } },
            { Permissions.Bond_BTKH_TTCT_MauHopDong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauHopDong, Description = "Cập nhật" } },
            { Permissions.Bond_BTKH_TTCT_MauHopDong_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauHopDong, Description = "Kích hoạt / Hủy kích hoạt" } },
            { Permissions.Bond_BTKH_TTCT_MauHopDong_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauHopDong, Description = "Xóa" } },

            { Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet, Description = "Mẫu giao nhận HĐ" } },
            { Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD, Description = "Danh sách" } },
            { Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD, Description = "Thêm mới" } },
            { Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BTKH_TTCT_MauGiaoNhanHD, Description = "Cập nhật" } },


            // Hợp đồng phân phối
            { Permissions.BondMenuHDPP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Hợp đồng phân phối" } },

            // Hợp đồng phân phối -> Sổ lệnh
            { Permissions.BondHDPP_SoLenh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Sổ lệnh" } },
            { Permissions.BondHDPP_SoLenh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh, Description = "Danh sách" } },
            { Permissions.BondHDPP_SoLenh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh, Description = "Thêm mới" } },
            { Permissions.BondHDPP_SoLenh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh, Description = "Xóa" } },
            { Permissions.BondHDPP_SoLenh_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Thông tin chi tiết" } },

            { Permissions.BondHDPP_SoLenh_TTCT_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Thông tin chung" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTC_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Xem chi tiết" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật sổ lệnh" } },

            { Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiKyHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT, Description = "Cập nhật kỳ hạn" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiMaGT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật mã giới thiệu" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiTKNganHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật TK ngân hàng" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật Số tiền đầu tư" } },


            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT, Description = "Thông tin thanh toán" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Danh sách" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Thêm mới" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Chi tiết thanh toán" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Sửa" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Phê duyệt" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Hủy phê duyệt" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Gửi thông báo" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TTThanhToan, Description = "Xóa" } },

            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT, Description = "HSKH đăng ký" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Danh sách" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ mẫu" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ chữ ký điện tử" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải lên hồ sơ" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Xem hồ sơ tải lên" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Chuyển online" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Cập nhật hồ sơ" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Ký điện tử" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Hủy ký điện tử" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Gửi thông báo" } },
            { Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Duyệt / Hủy hồ sơ" } },

            { Permissions.BondHDPP_SoLenh_TTCT_LoiTuc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT, Description = "Lợi tức" } },
            { Permissions.BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_LoiTuc, Description = "Danh sách" } },

            { Permissions.BondHDPP_SoLenh_TTCT_TraiTuc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT, Description = "Trái tức" } },
            { Permissions.BondHDPP_SoLenh_TTCT_TraiTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_SoLenh_TTCT_TraiTuc, Description = "Danh sách" } },

            // Hợp đồng phân phối -> Xử lý hợp đồng
            { Permissions.BondHDPP_XLHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Xử lý hợp đồng" } },
            { Permissions.BondHDPP_XLHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_XLHD, Description = "Danh sách" } },

            // Hợp đồng phân phối -> Hợp đồng
            { Permissions.BondHDPP_HopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Hợp đồng" } },
            { Permissions.BondHDPP_HopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDong, Description = "Danh sách" } },
            { Permissions.BondHDPP_HopDong_YeuCauTaiTuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDong, Description = "Yêu cầu tái tục" } },
            // { Permissions.BondHDPP_HopDong_YeuCauRutVon, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDong, Description = "Yêu cầu rút vốn" } },
            { Permissions.BondHDPP_HopDong_PhongToaHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDong, Description = "Phong tỏa hợp đồng" } },
            
            // Hợp đồng phân phối -> Giao nhận hợp đồng
            { Permissions.BondHDPP_GiaoNhanHopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Giao nhận hợp đồng" } },
            { Permissions.BondHDPP_GiaoNhanHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_GiaoNhanHopDong, Description = "Danh sách" } },
            { Permissions.BondHDPP_GiaoNhanHopDong_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_GiaoNhanHopDong, Description = "Đổi trạng thái" } },
            { Permissions.BondHDPP_GiaoNhanHopDong_XuatHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_GiaoNhanHopDong, Description = "Xuất hợp đồng" } },
            { Permissions.BondHDPP_GiaoNhanHopDong_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_GiaoNhanHopDong, Description = "Thông tin chi tiết" } },
            { Permissions.BondHDPP_GiaoNhanHopDong_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_GiaoNhanHopDong_ThongTinChiTiet, Description = "Thông tin chung" } },

            // Hợp đồng phân phối -> Phong tỏa, giải tỏa
            { Permissions.BondHDPP_PTGT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Phong tỏa giải tỏa" } },
            { Permissions.BondHDPP_PTGT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_PTGT, Description = "Danh sách" } },
            { Permissions.BondHDPP_PTGT_GiaiToaHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_PTGT, Description = "Giải tỏa hợp đồng" } },
            { Permissions.BondHDPP_PTGT_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_PTGT, Description = "Thông tin chi tiết" } },

            // Hợp đồng đáo hạn 
            { Permissions.BondHDPP_HopDongDaoHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuHDPP, Description = "Hợp đồng đáo hạn" } },
            { Permissions.BondHDPP_HopDongDaoHan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDongDaoHan, Description = "Danh sách" } },
            { Permissions.BondHDPP_HopDongDaoHan_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDongDaoHan, Description = "Thông tin đầu tư" } },
            { Permissions.BondHDPP_HopDongDaoHan_LapDSChiTra, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDongDaoHan, Description = "Lập danh sách chi trả" } },
            { Permissions.BondHDPP_HopDongDaoHan_DuyetKhongChi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondHDPP_HopDongDaoHan, Description = "Duyệt không chi" } },

            //===============================================================

            //Quản lý phê duyệt
            { Permissions.BondMenuQLPD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Quản lý phê duyệt" } },

            // QLPD -> Phê duyệt lô trái phiếu
            { Permissions.BondQLPD_PDLTP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLPD, Description = "Phê duyệt lô trái phiếu" } },
            { Permissions.BondQLPD_PDLTP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDLTP, Description = "Danh sách" } },
            { Permissions.BondQLPD_PDLTP_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDLTP, Description = "Phê duyệt / Hủy" } },

            // QLPD -> Phê duyệt bán theo kỳ hạn
            { Permissions.BondQLPD_PDBTKH, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLPD, Description = "Phê duyệt bán theo kỳ hạn" } },
            { Permissions.BondQLPD_PDBTKH_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDBTKH, Description = "Danh sách" } },
            { Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDBTKH, Description = "Phê duyệt / Hủy" } },

            // QLPD -> Phê duyệt yêu cầu tái tục
            { Permissions.BondQLPD_PDYCTT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondMenuQLPD, Description = "Phê duyệt yêu cầu tái tục" } },
            { Permissions.BondQLPD_PDYCTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDYCTT, Description = "Danh sách" } },
            { Permissions.BondQLPD_PDYCTT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.BondQLPD_PDYCTT, Description = "Phê duyệt / Hủy" } },

            // Báo cáo
            { Permissions.Bond_Menu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Bond, ParentKey = null, Description = "Menu báo cáo" } },

            { Permissions.Bond_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_Menu_BaoCao, Description = "Báo cáo quản trị" } },
             { Permissions.Bond_BaoCao_QuanTri_THCGTraiPhieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_QuanTri, Description = "B.C tổng hợp các gói trái phiếu" } },
                { Permissions.Bond_BaoCao_QuanTri_THCGDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_QuanTri, Description = "B.C tổng hợp các gói đầu tư" } },
                { Permissions.Bond_BaoCao_QuanTri_TGLDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_QuanTri, Description = "B.C tính gốc lãi đến hạn" } },

            { Permissions.Bond_BaoCao_VanHanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_Menu_BaoCao, Description = "Báo cáo vận hành" } },
            { Permissions.Bond_BaoCao_VanHanh_THCGTraiPhieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_VanHanh, Description = "B.C tổng hợp các gói trái phiếu" } },
                { Permissions.Bond_BaoCao_VanHanh_THCGDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_VanHanh, Description = "B.C tổng hợp các gói đầu tư" } },
                { Permissions.Bond_BaoCao_VanHanh_TGLDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_VanHanh, Description = "B.C tính gốc lãi đến hạn" } },

            { Permissions.Bond_BaoCao_KinhDoanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_Menu_BaoCao, Description = "Báo cáo kinh doanh" } },
            { Permissions.Bond_BaoCao_KinhDoanh_THCGTraiPhieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_KinhDoanh, Description = "B.C tổng hợp các gói trái phiếu" } },
            { Permissions.Bond_BaoCao_KinhDoanh_THCGDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_KinhDoanh, Description = "B.C tổng hợp các gói đầu tư" } },
            { Permissions.Bond_BaoCao_KinhDoanh_TGLDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Bond, ParentKey = Permissions.Bond_BaoCao_KinhDoanh, Description = "B.C tính gốc lãi đến hạn" } },
            #endregion

            #region trang einvest
            { Permissions.InvestWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "" } },
            { Permissions.InvestPageDashboard, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Dashboard tổng quan" } },
            //
            { Permissions.InvestMenuSetting, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Cài đặt" } },

                // Quản lý chủ đầu tư
                { Permissions.InvestMenuChuDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Chủ đầu tư" } },
                { Permissions.InvestChuDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChuDT, Description = "Danh sách" } },
                { Permissions.InvestChuDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChuDT, Description = "Thêm mới" } },
                { Permissions.InvestChuDT_ThongTinChuDauTu, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChuDT, Description = "Thông tin chủ đầu tư" } },
                { Permissions.InvestChuDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChuDT, Description = "Xoá" } },

                    // Thông tin chi tiết chủ đầu tư
                    { Permissions.InvestChuDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestChuDT_ThongTinChuDauTu, Description = "Thông tin chung" } },
                    { Permissions.InvestChuDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestChuDT_ThongTinChung, Description = "Chi tiết" } },
                    { Permissions.InvestChuDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestChuDT_ThongTinChung, Description = "Cập nhật" } },

                // Cấu hình ngày nghỉ lễ
                { Permissions.InvestMenuCauHinhNNL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Cấu hình ngày nghỉ lễ" } },
                { Permissions.InvestCauHinhNNL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhNNL, Description = "Danh sách" } },
                { Permissions.InvestCauHinhNNL_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhNNL, Description = "Cập nhật" } },

                // Chính sách mẫu = CMS
                { Permissions.InvestMenuChinhSachMau, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Chính sách mẫu" } },
                { Permissions.InvestCSM_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Danh sách" } },
                { Permissions.InvestCSM_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Thêm chính sách" } },
                { Permissions.InvestCSM_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Cập nhật chính sách" } },
                { Permissions.InvestCSM_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Kích hoạt / Huỷ (Chính sách)" } },
                { Permissions.InvestCSM_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Xoá (Chính sách)" } },

                { Permissions.InvestCSM_KyHan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Thêm kỳ hạn" } },
                { Permissions.InvestCSM_KyHan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Cập nhật kỳ hạn" } },
                { Permissions.InvestCSM_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Kích hoạt / Hủy (Kỳ hạn)" } },
                { Permissions.InvestCSM_KyHan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuChinhSachMau, Description = "Xóa (Kỳ hạn)" } },

                 // Cấu hình mã hd
                { Permissions.InvestMenuCauHinhMaHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Cấu hình mã hợp đồng" } },
                { Permissions.InvestCauHinhMaHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhMaHD, Description = "Danh sách" } },
                { Permissions.InvestCauHinhMaHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhMaHD, Description = "Thêm cấu hình" } },
                { Permissions.InvestCauHinhMaHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhMaHD, Description = "Cập nhật cấu hình" } },
                { Permissions.InvestCauHinhMaHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuCauHinhMaHD, Description = "Xoá cấu hình" } },
                
                // mau hop dong
                { Permissions.InvestMenuMauHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Mẫu hợp đồng" } },
                { Permissions.InvestMauHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Danh sách" } },
                { Permissions.InvestMauHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Thêm mới" } },
                 { Permissions.InvestMauHD_TaiFileDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Cập nhật" } },
                  { Permissions.InvestMauHD_TaiFileCaNhan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Cập nhật" } },
                { Permissions.InvestMauHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Cập nhật" } },
                { Permissions.InvestMauHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuMauHD, Description = "Xoá" } },
                // Tổng thầu
                { Permissions.InvestMenuTongThau, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Tổng thầu" } },
                { Permissions.InvestTongThau_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuTongThau, Description = "Danh sách" } },
                { Permissions.InvestTongThau_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuTongThau, Description = "Thêm mới" } },
                { Permissions.InvestTongThau_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuTongThau, Description = "Xoá" } },
                { Permissions.InvestTongThau_ThongTinTongThau, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuTongThau, Description = "Thông tin tổng thầu" } },
                    // Tab thông tin chung
                    { Permissions.InvestTongThau_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestTongThau_ThongTinTongThau, Description = "Thông tin chung" } },
                    { Permissions.InvestTongThau_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestTongThau_ThongTinChung, Description = "Chi tiết" } },
                
                // Đại lý
                { Permissions.InvestMenuDaiLy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Đại lý" } },
                { Permissions.InvestDaiLy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuDaiLy, Description = "Danh sách" } },
                { Permissions.InvestDaiLy_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuDaiLy, Description = "Thêm mới" } },
                { Permissions.InvestDaiLy_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuDaiLy, Description = "Kích hoạt/ Đóng" } },
                { Permissions.InvestDaiLy_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuDaiLy, Description = "Xóa" } },
                { Permissions.InvestDaiLy_ThongTinDaiLy, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuDaiLy, Description = "Thông tin đại lý" } },
                    // Tab thông tin chung
                    { Permissions.InvestDaiLy_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestDaiLy_ThongTinDaiLy, Description = "Thông tin chung" } },
                    { Permissions.InvestDaiLy_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestDaiLy_ThongTinChung, Description = "Chi tiết" } },

                    // Tab Tài khoản đăng nhập
                    { Permissions.InvestDaiLy_TKDN, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestDaiLy_ThongTinDaiLy, Description = "Tài khoản đăng nhập" } },
                    { Permissions.InvestDaiLy_TKDN_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestDaiLy_TKDN, Description = "Thêm mới" } },
                    { Permissions.InvestDaiLy_TKDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestDaiLy_TKDN, Description = "Danh sách" } },

                // Hình ảnh
                { Permissions.InvestMenuHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Hình ảnh" } },
                { Permissions.InvestHinhAnh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Danh sách" } },
                { Permissions.InvestHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Thêm mới" } },
                { Permissions.InvestHinhAnh_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Sửa" } },
                { Permissions.InvestHinhAnh_DuyetDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Duyệt đăng/Huỷ duyệt đăng" } },
                { Permissions.InvestHinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Chi tiết" } },
                { Permissions.InvestHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHinhAnh, Description = "Xoá" } },

                // Thông báo hệ thống
                { Permissions.InvestMenuThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Thông báo hệ thống" } },
                { Permissions.InvestMenuThongBaoHeThong_CaiDat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSetting, Description = "Cài đặt" } },


        // Hợp đồng phân phối
        { Permissions.InvestMenuHDPP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Hợp đồng phân phối" } },
            
            // Sổ lệnh
            { Permissions.InvestHDPP_SoLenh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Sổ lệnh" } },
            { Permissions.InvestHDPP_SoLenh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh, Description = "Danh sách" } },
            { Permissions.InvestHDPP_SoLenh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh, Description = "Thêm mới" } },
            { Permissions.InvestHDPP_SoLenh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh, Description = "Xóa" } },
            //
            { Permissions.InvestHDPP_SoLenh_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Thông tin sổ lệnh" } },

            // Tab thông tin chung
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "Thông tin chung" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Chi tiết" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật" } },

                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiKyHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật kỳ hạn" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiMaGT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật mã giới thiệu" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật thông tin khách hàng" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật Số tiền đầu tư" } },

                // Tab thông tin thanh toán
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "Thông tin thanh toán" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Danh sách" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Thêm mới" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Cập nhật" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Chi tiết thanh toán" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Phê duyệt" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Huỷ phê duyệt" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Gửi thông báo" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TTThanhToan, Description = "Xoá" } },

                // Tab HSKH đăng ký
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "HSKH đăng ký" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Danh sách" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ mẫu" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ chữ ký điện tử" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Upload hồ sơ" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Xem hồ sơ tải lên" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Chuyển Online" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Cập nhật hồ sơ" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Ký điện tử" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Hủy ký điện tử" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Gửi thông báo" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Duyệt hồ sơ / Hủy hồ sơ" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Nhận hợp đồng bản cứng" } },
                
                // Tab Lợi nhuận
                { Permissions.InvestSoLenh_LoiNhuan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "Lợi nhuận" } },
                { Permissions.InvestSoLenh_LoiNhuan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSoLenh_LoiNhuan, Description = "Danh sách" } },
                
                // Tab Lịch sử hợp đồng
                { Permissions.InvestSoLenh_LichSuHD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "Lịch sử hợp đồng" } },
                { Permissions.InvestSoLenh_LichSuHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSoLenh_LichSuHD, Description = "Danh sách" } },
                
                // Trái tức
                { Permissions.InvestHDPP_SoLenh_TTCT_TraiTuc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT, Description = "Trái tức" } },
                { Permissions.InvestHDPP_SoLenh_TTCT_TraiTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_SoLenh_TTCT_TraiTuc, Description = "Danh sách" } },

            // Xử lý hợp đồng
            { Permissions.InvestHDPP_XLHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Xử lý hợp đồng" } },
            { Permissions.InvestHDPP_XLHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLHD, Description = "Danh sách" } },
            
            // Hợp đồng
            { Permissions.InvestHDPP_HopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Hợp đồng" } },
            { Permissions.InvestHDPP_HopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HopDong, Description = "Danh sách" } },
            { Permissions.InvestHDPP_HopDong_YeuCauTaiTuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HopDong, Description = "Yêu cầu tái tục" } },
            { Permissions.InvestHDPP_HopDong_YeuCauRutVon, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HopDong, Description = "Yêu cầu rút vốn" } },
            { Permissions.InvestHDPP_HopDong_PhongToaHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HopDong, Description = "Phong tỏa hợp đồng" } },

            // Giao nhận hợp đồng
            { Permissions.InvestHDPP_GiaoNhanHopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Giao nhận hợp đồng" } },
            { Permissions.InvestHDPP_GiaoNhanHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_GiaoNhanHopDong, Description = "Danh sách" } },
            { Permissions.InvestHDPP_GiaoNhanHopDong_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_GiaoNhanHopDong, Description = "Đổi trạng thái" } },
            { Permissions.InvestHDPP_GiaoNhanHopDong_XuatHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_GiaoNhanHopDong, Description = "Xuất hợp đồng" } },
            { Permissions.InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_GiaoNhanHopDong, Description = "Thông tin chi tiết" } },
            { Permissions.InvestHDPP_GiaoNhanHopDong_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet, Description = "Thông tin chung" } },
            
            // Phong toả, giải toả
            { Permissions.InvestHopDong_PhongToaGiaiToa, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Phong toả giải toả" } },
            { Permissions.InvestHopDong_PhongToaGiaiToa_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_PhongToaGiaiToa, Description = "Danh sách" } },
            { Permissions.InvestHopDong_PhongToaGiaiToa_GiaiToaHD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_PhongToaGiaiToa, Description = "Giải toả hợp đồng" } },
            { Permissions.InvestHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_PhongToaGiaiToa, Description = "Thông tin phong toả giải toả" } },

            // Xử lý rút tiền
            { Permissions.InvestHDPP_XLRT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Xử lý rút vốn" } },
            { Permissions.InvestHDPP_XLRT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLRT, Description = "Danh sách" } },
            { Permissions.InvestHDPP_XLRT_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLRT, Description = "Thông tin đầu tư" } },
            { Permissions.InvestHDPP_XLRT_DuyetChiTuDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLRT, Description = "Duyệt chi tự động" } },
            { Permissions.InvestHDPP_XLRT_DuyetChiThuCong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLRT, Description = "Duyệt chi thủ công" } },
            { Permissions.InvestHDPP_XLRT_HuyYeuCau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_XLRT, Description = "Hủy yêu cầu" } },
            
            // Hợp đồng đáo hạn 
            { Permissions.InvestHopDong_HopDongDaoHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Hợp đồng đáo hạn" } },
            { Permissions.InvestHopDong_HopDongDaoHan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Danh sách" } },
            { Permissions.InvestHopDong_HopDongDaoHan_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Thông tin đầu tư" } },
            { Permissions.InvestHopDong_HopDongDaoHan_LapDSChiTra, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Lập danh sách chi trả" } },
            { Permissions.InvestHopDong_HopDongDaoHan_DuyetChiTuDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Duyệt chi tự động" } },
            { Permissions.InvestHopDong_HopDongDaoHan_DuyetChiThuCong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Duyệt chi thủ công" } },
            { Permissions.InvestHopDong_HopDongDaoHan_DuyetTaiTuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Duyệt tái tục" } },
            { Permissions.InvestHopDong_HopDongDaoHan_ExportExcel, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHopDong_HopDongDaoHan, Description = "Xuất Excel" } },
            
            // Chi trả lợi tức 
            { Permissions.InvestHDPP_CTLT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Chi trả lợi tức" } },
            { Permissions.InvestHDPP_CTLT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Danh sách" } },
            { Permissions.InvestHDPP_CTLT_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Thông tin đầu tư" } },
            { Permissions.InvestHDPP_CTLT_LapDSChiTra, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Lập danh sách chi trả" } },
            { Permissions.InvestHDPP_CTLT_DuyetChiTuDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Duyệt chi tự động" } },
            { Permissions.InvestHDPP_CTLT_DuyetChiThuCong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Duyệt chi thủ công" } },
            { Permissions.InvestHDPP_CTLT_ExportExcel, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_CTLT, Description = "Xuất Excel" } },
            
            // Lịch sử đầu tư
            { Permissions.InvestHDPP_LSDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Lịch sử đầu tư" } },
            { Permissions.InvestHDPP_LSDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_LSDT, Description = "Danh sách" } },
            { Permissions.InvestHDPP_LSDT_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_LSDT, Description = "Thông tin đầu tư" } },
            
            // Hợp đồng tái tục
            { Permissions.InvestHDPP_HDTT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuHDPP, Description = "Hợp đồng tái tục" } },
            { Permissions.InvestHDPP_HDTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HDTT, Description = "Danh sách" } },
            { Permissions.InvestHDPP_HDTT_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HDTT, Description = "Thông tin đầu tư" } },
            { Permissions.InvestHDPP_HDTT_HuyYeuCau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestHDPP_HDTT, Description = "Hủy yêu cầu" } },
        
            // Quản lý đầu tư
            { Permissions.InvestMenuQLDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Quản lý đầu tư" } },

            //Sản phẩm đầu tư
            { Permissions.InvestMenuSPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLDT, Description = "Sản phẩm đầu tư" } },
            { Permissions.InvestSPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Danh sách" } },
            { Permissions.InvestSPDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Thêm mới" } },
            { Permissions.InvestSPDT_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Trình duyệt" } },
            { Permissions.InvestSPDT_Dong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Đóng sản phẩm" } },
            { Permissions.InvestSPDT_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Epic xác minh" } },
            { Permissions.InvestSPDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Xoá" } },

            { Permissions.InvestSPDT_ThongTinSPDT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuSPDT, Description = "Thông tin sản phẩm đầu tư" } },
                //Tab Thông tin chung
                { Permissions.InvestSPDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinSPDT, Description = "Thông tin chung" } },
                { Permissions.InvestSPDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinChung, Description = "Chi tiết" } },
                { Permissions.InvestSPDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinChung, Description = "Cập nhật" } },

                 //Tab hinh anh dau tu
                { Permissions.InvestSPDT_HADT, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinSPDT, Description = "Hình ảnh đầu tư" } },
                { Permissions.InvestSPDT_HADT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HADT, Description = "Danh sách" } },
                { Permissions.InvestSPDT_HADT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HADT, Description = "Thêm mới" } },
                { Permissions.InvestSPDT_HADT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HADT, Description = "Xoá" } },
               
                //Tab Hồ sơ pháp lý
                { Permissions.InvestSPDT_HSPL, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinSPDT, Description = "Hồ sơ pháp lý" } },
                { Permissions.InvestSPDT_HSPL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HSPL, Description = "Danh sách" } },
                { Permissions.InvestSPDT_HSPL_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HSPL, Description = "Thêm mới" } },
                { Permissions.InvestSPDT_HSPL_Preview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HSPL, Description = "Xem file" } },
                { Permissions.InvestSPDT_HSPL_DownloadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HSPL, Description = "Tải file" } },
                { Permissions.InvestSPDT_HSPL_DeleteFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_HSPL, Description = "Xoá file" } },

                //Tab tin tức sản phẩm
                { Permissions.InvestSPDT_TTSP, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_ThongTinSPDT, Description = "Tin tức sản phẩm" } },
                { Permissions.InvestSPDT_TTSP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_TTSP, Description = "Danh sách" } },
                { Permissions.InvestSPDT_TTSP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_TTSP, Description = "Thêm mới" } },
                { Permissions.InvestSPDT_TTSP_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_TTSP, Description = "Cập nhật" } },
                { Permissions.InvestSPDT_TTSP_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_TTSP, Description = "Duyệt đăng / Bỏ duyệt đăng" } },
                { Permissions.InvestSPDT_TTSP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestSPDT_TTSP, Description = "Xoá" } },

            //Phân phối đầu tư
            { Permissions.InvestMenuPPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLDT, Description = "Phân phối đầu tư" } },
            { Permissions.InvestPPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Danh sách" } },
            { Permissions.InvestPPDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Thêm mới" } },
            { Permissions.InvestPPDT_DongTam, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Đóng tạm / Mở" } },
            { Permissions.InvestPPDT_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Trình duyệt" } },
            { Permissions.InvestPPDT_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Bật / Tắt show app" } },
            { Permissions.InvestPPDT_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Epic xác minh" } },

            { Permissions.InvestPPDT_ThongTinPPDT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPPDT, Description = "Thông tin phân phối đầu tư" } },

                //Tab Thông tin chung
                { Permissions.InvestPPDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Thông tin chung" } },
                { Permissions.InvestPPDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinChung, Description = "Chi tiết" } },
                { Permissions.InvestPPDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinChung, Description = "Cập nhật" } },

                //Tab tổng quan
                { Permissions.InvestPPDT_TongQuan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Tổng quan" } },
                { Permissions.InvestPPDT_TongQuan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Chỉnh sửa" } },
                { Permissions.InvestPPDT_TongQuan_ChonAnh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Chọn ảnh" } },
                { Permissions.InvestPPDT_TongQuan_ThemToChuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Thêm tổ chức" } },
                { Permissions.InvestPPDT_TongQuan_DanhSach_ToChuc, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Danh sách tổ chức" } },
                { Permissions.InvestPPDT_TongQuan_UploadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Upload file" } },
                { Permissions.InvestPPDT_TongQuan_DanhSach_File, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_TongQuan, Description = "Danh sách file" } },

                //Tab Chính sách
                { Permissions.InvestPPDT_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Chính sách" } },
                { Permissions.InvestPPDT_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Danh sách" } },
                { Permissions.InvestPPDT_ChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Thêm chính sách" } },
                { Permissions.InvestPPDT_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Cập nhật chính sách" } },
                { Permissions.InvestPPDT_ChinhSach_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Kích hoạt / Huỷ (Chính sách)" } },
                { Permissions.InvestPPDT_ChinhSach_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Bật ? Tắt show App (Chính sách)" } },
                { Permissions.InvestPPDT_ChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Xoá chính sách" } },

                { Permissions.InvestPPDT_KyHan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Thêm kỳ hạn" } },
                { Permissions.InvestPPDT_KyHan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Cập nhật kỳ hạn" } },
                { Permissions.InvestPPDT_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Kích hoạt / Huỷ (Kỳ hạn)" } },
                { Permissions.InvestPPDT_KyHan_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Bật / Tắt show App (Kỳ hạn)" } },
                { Permissions.InvestPPDT_KyHan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ChinhSach, Description = "Xoá (Kỳ hạn)" } },

                //Tab file chính sách
                { Permissions.InvestPPDT_FileChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "File chính sách" } },
                { Permissions.InvestPPDT_FileChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_FileChinhSach, Description = "Danh sách" } },
                { Permissions.InvestPPDT_FileChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_FileChinhSach, Description = "Thêm mới" } },
                { Permissions.InvestPPDT_FileChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_FileChinhSach, Description = "Cập nhật" } },
                { Permissions.InvestPPDT_FileChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_FileChinhSach, Description = "Xoá" } },

                //Tab mẫu hợp đồng
                { Permissions.InvestPPDT_MauHopDong, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Mẫu hợp đồng" } },
                { Permissions.InvestPPDT_MauHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauHopDong, Description = "Danh sách" } },
                { Permissions.InvestPPDT_MauHopDong_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauHopDong, Description = "Thêm mới" } },
                { Permissions.InvestPPDT_MauHopDong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauHopDong, Description = "Cập nhật" } },
                { Permissions.InvestPPDT_MauHopDong_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauHopDong, Description = "Xoá" } },

                //Tab Hợp đồng phân phối
                { Permissions.InvestPPDT_HopDongPhanPhoi, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Hợp đồng phân phối" } },
                { Permissions.InvestPPDT_HopDongPhanPhoi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_HopDongPhanPhoi, Description = "Danh sách" } },
                { Permissions.InvestPPDT_HopDongPhanPhoi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_HopDongPhanPhoi, Description = "Thêm mới" } },
                { Permissions.InvestPPDT_HopDongPhanPhoi_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_HopDongPhanPhoi, Description = "Cập nhật" } },
                { Permissions.InvestPPDT_HopDongPhanPhoi_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_HopDongPhanPhoi, Description = "Xoá" } },

                //Tab Mẫu giao nhận HĐ
                { Permissions.InvestPPDT_MauGiaoNhanHD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_ThongTinPPDT, Description = "Mẫu giao nhận HĐ" } },
                { Permissions.InvestPPDT_MauGiaoNhanHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauGiaoNhanHD, Description = "Danh sách" } },
                { Permissions.InvestPPDT_MauGiaoNhanHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauGiaoNhanHD, Description = "Thêm mới" } },
                { Permissions.InvestPPDT_MauGiaoNhanHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauGiaoNhanHD, Description = "Cập nhật" } },
                { Permissions.InvestPPDT_MauGiaoNhanHD_KichHoat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauGiaoNhanHD, Description = "Kích hoạt" } },
                { Permissions.InvestPPDT_MauGiaoNhanHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestPPDT_MauGiaoNhanHD, Description = "Xoá" } },

            // Quản lý phê duyệt
            { Permissions.InvestMenuQLPD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Quản lý phê duyệt" } },
            // Phê duyệt sản phẩm đầu tư
            { Permissions.InvestMenuPDSPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLPD, Description = "Phê duyệt sản phẩm đầu tư" } },
            { Permissions.InvestPDSPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDSPDT, Description = "Danh sách" } },
            { Permissions.InvestPDSPDT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDSPDT, Description = "Phê duyệt / Hủy" } },
            
            // Phê duyệt phân phối đầu tư
            { Permissions.InvestMenuPDPPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLPD, Description = "Phê duyệt phân phối đầu tư" } },
            { Permissions.InvestPDPPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDPPDT, Description = "Danh sách" } },
            { Permissions.InvestPDPPDT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDPPDT, Description = "Phê duyệt / Hủy" } },

            // Phê duyệt yêu cầu tái tục
            { Permissions.InvestMenuPDYCTT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLPD, Description = "Phê duyệt yêu cầu tái tục" } },
            { Permissions.InvestPDYCTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDYCTT, Description = "Danh sách" } },
            { Permissions.InvestPDYCTT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDYCTT, Description = "Phê duyệt / Hủy" } },

            // Phê duyệt yêu cầu rút vốn
            { Permissions.InvestMenuPDYCRV, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuQLPD, Description = "Phê duyệt yêu cầu rút vốn" } },
            { Permissions.InvestPDYCRV_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDYCRV, Description = "Danh sách" } },
            { Permissions.InvestPDYCRV_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDYCRV, Description = "Phê duyệt / Hủy" } },
            { Permissions.InvestPDYCRV_ChiTietHD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.InvestMenuPDYCRV, Description = "Chi tiết hợp đồng" } },

            // Báo cáo
            { Permissions.Invest_Menu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Menu báo cáo" } },

            { Permissions.Invest_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_Menu_BaoCao, Description = "Báo cáo quản trị" } },
            { Permissions.Invest_BaoCao_QuanTri_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Invest_BaoCao_QuanTri_THCMaBDS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C tổng hợp các mã BDS" } },
            { Permissions.Invest_BaoCao_QuanTri_DCDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C dự chi đến hạn" } },
            { Permissions.Invest_BaoCao_QuanTri_ThucChi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C thực chi" } },
            { Permissions.Invest_BaoCao_QuanTri_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C tổng chi trả đầu tư" } },
            { Permissions.Invest_BaoCao_QuanTri_THCKDTVanHanhHVF, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C tổng hợp các khoản đầu tư VH HVF" } },
            { Permissions.Invest_BaoCao_QuanTri_SKTKNhaDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C sao kê tài khoản nhà đầu tư" } },
              { Permissions.Invest_BaoCao_QuanTri_THCKDTBanHo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_QuanTri, Description = "B.C tổng hợp các khoản đầu tư bán hộ" } },

            { Permissions.Invest_BaoCao_VanHanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_Menu_BaoCao, Description = "Báo cáo vận hành" } },
            { Permissions.Invest_BaoCao_VanHanh_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_VanHanh, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Invest_BaoCao_VanHanh_THCMaBDS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_VanHanh, Description = "B.C tổng hợp các mã BDS" } },
            { Permissions.Invest_BaoCao_VanHanh_DCDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_VanHanh, Description = "B.C dự chi đến hạn" } },
            { Permissions.Invest_BaoCao_VanHanh_ThucChi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_VanHanh, Description = "B.C thực chi" } },
            { Permissions.Invest_BaoCao_VanHanh_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_VanHanh, Description = "B.C tổng chi trả đầu tư" } },

            { Permissions.Invest_BaoCao_KinhDoanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_Menu_BaoCao, Description = "Báo cáo kinh doanh" } },
            { Permissions.Invest_BaoCao_KinhDoanh_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_KinhDoanh, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Invest_BaoCao_KinhDoanh_THCMaBDS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_KinhDoanh, Description = "B.C tổng hợp các mã BDS" } },
            { Permissions.Invest_BaoCao_KinhDoanh_DCDenHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_KinhDoanh, Description = "B.C dự chi đến hạn" } },
            { Permissions.Invest_BaoCao_KinhDoanh_ThucChi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_KinhDoanh, Description = "B.C thực chi" } },
            { Permissions.Invest_BaoCao_KinhDoanh_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_BaoCao_KinhDoanh, Description = "B.C tổng chi trả đầu tư" } },

            { Permissions.Invest_Menu_TruyVan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Invest, ParentKey = null, Description = "Menu truy vấn" } },
            { Permissions.Invest_TruyVan_ThuTien_NganHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_Menu_TruyVan, Description = "Thu tiền ngân hàng" } },
            { Permissions.Invest_TruyVan_ChiTien_NganHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Invest, ParentKey = Permissions.Invest_Menu_TruyVan, Description = "Chi tiền ngân hàng" } },
            #endregion

            #region trang esupport
            { Permissions.SupportWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Support, ParentKey = null, Description = "" } },
            #endregion

            #region trang User
            { Permissions.UserWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.User, ParentKey = null, Description = "" } },

            // PHÂN QUYỀN WEBSITE THEO VAI TRÒ
            { Permissions.UserPhanQuyen_Websites, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.User, ParentKey = null, Description = "Phân quyền website" } },
            { Permissions.UserPhanQuyen_Website_CauHinhVaiTro, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserPhanQuyen_Websites, Description = "Cấu hình vai trò" } },
            { Permissions.UserPhanQuyen_Website_ThemVaiTro, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserPhanQuyen_Websites, Description = "Thêm vai trò" } },
            { Permissions.UserPhanQuyen_Website_CapNhatVaiTro, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserPhanQuyen_Websites, Description = "Cập nhật vai trò" } },

            // QUẢN LÝ TÀI KHOẢN, CẤU HÌNH QUYỀN CHO TÀI KHOẢN
            { Permissions.UserQL_TaiKhoan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.User, ParentKey = null, Description = "Quản lý tài khoản" } },
            { Permissions.UserQL_TaiKhoan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Thêm mới" } },
            { Permissions.UserQL_TaiKhoan_CapNhatVaiTro, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Cập nhật" } },
            { Permissions.UserQL_TaiKhoan_SetMatKhau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Set mật khẩu" } },
            { Permissions.UserQL_TaiKhoan_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Chi tiết tài khoản" } },
            { Permissions.UserQL_TaiKhoan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Xóa tài khoản" } },

            // CẤU HÌNH QUYỀN CHO TÀI KHOẢN
            { Permissions.UserQL_TaiKhoan_CauHinhQuyen, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Cấu hình quyền" } },
            { Permissions.UserQL_TaiKhoan_PhanQuyenWebsite, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Phân quyền website" } },
            { Permissions.UserQL_TaiKhoan_PhanQuyenChiTietWebsite, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.User, ParentKey = Permissions.UserQL_TaiKhoan, Description = "Phân quyền chi tiết website" } },

            #endregion

            #region trang egarner
            { Permissions.GarnerWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "" } },
            { Permissions.GarnerPageDashboard, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Dashboard tổng quan" } },
            //
            { Permissions.GarnerMenuSetting, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Cài đặt" } },

                // Quản lý chủ đầu tư
                { Permissions.GarnerMenuChuDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Chủ đầu tư" } },
                { Permissions.GarnerChuDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChuDT, Description = "Danh sách" } },
                { Permissions.GarnerChuDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChuDT, Description = "Thêm mới" } },
                { Permissions.GarnerChuDT_ThongTinChuDauTu, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChuDT, Description = "Thông tin chủ đầu tư" } },
                { Permissions.GarnerChuDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChuDT, Description = "Xoá" } },

                    // Thông tin chi tiết chủ đầu tư
                    { Permissions.GarnerChuDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerChuDT_ThongTinChuDauTu, Description = "Thông tin chung" } },
                    { Permissions.GarnerChuDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerChuDT_ThongTinChung, Description = "Chi tiết" } },
                    { Permissions.GarnerChuDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerChuDT_ThongTinChung, Description = "Cập nhật" } },

                // Cấu hình ngày nghỉ lễ
                { Permissions.GarnerMenuCauHinhNNL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Cấu hình ngày nghỉ lễ" } },
                { Permissions.GarnerCauHinhNNL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhNNL, Description = "Danh sách" } },
                { Permissions.GarnerCauHinhNNL_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhNNL, Description = "Cập nhật" } },

                // Chính sách mẫu = CMS
                { Permissions.GarnerMenuChinhSachMau, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Chính sách mẫu" } },
                { Permissions.GarnerCSM_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Danh sách" } },
                { Permissions.GarnerCSM_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Thêm chính sách" } },
                { Permissions.GarnerCSM_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Cập nhật chính sách" } },
                { Permissions.GarnerCSM_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Kích hoạt / Huỷ (Chính sách)" } },
                { Permissions.GarnerCSM_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Xoá (Chính sách)" } },

                { Permissions.GarnerCSM_KyHan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Thêm kỳ hạn" } },
                { Permissions.GarnerCSM_KyHan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Cập nhật kỳ hạn" } },
                { Permissions.GarnerCSM_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Kích hoạt / Hủy (Kỳ hạn)" } },
                { Permissions.GarnerCSM_KyHan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Xóa (Kỳ hạn)" } },

                 { Permissions.GarnerCSM_HopDongMau_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Thêm hợp đồng mẫu" } },
                { Permissions.GarnerCSM_HopDongMau_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Cập nhật hợp đồng mẫu" } },
                { Permissions.GarnerCSM_HopDongMau_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Kích hoạt / Hủy (Hợp đồng mẫu)" } },
                { Permissions.GarnerCSM_HopDongMau_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuChinhSachMau, Description = "Xóa (Hợp đồng mẫu)" } },

                 // Cấu hình mã hd
                { Permissions.GarnerMenuCauHinhMaHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Cấu hình mã hợp đồng" } },
                { Permissions.GarnerCauHinhMaHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhMaHD, Description = "Danh sách" } },
                { Permissions.GarnerCauHinhMaHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhMaHD, Description = "Thêm cấu hình" } },
                { Permissions.GarnerCauHinhMaHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhMaHD, Description = "Cập nhật cấu hình" } },
                { Permissions.GarnerCauHinhMaHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuCauHinhMaHD, Description = "Xoá cấu hình" } },
                
                // mau hop dong
                { Permissions.GarnerMenuMauHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Mẫu hợp đồng" } },
                { Permissions.GarnerMauHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Danh sách" } },
                { Permissions.GarnerMauHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Thêm mới" } },
                 { Permissions.GarnerMauHD_TaiFileDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Cập nhật" } },
                  { Permissions.GarnerMauHD_TaiFileCaNhan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Cập nhật" } },
                { Permissions.GarnerMauHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Cập nhật" } },
                { Permissions.GarnerMauHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMauHD, Description = "Xoá" } },

                // Module Mô tả chung
                { Permissions.GarnerMenuMoTaChung, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Mô tả chung" } },
                { Permissions.GarnerMTC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMoTaChung, Description = "Danh sách" } },
                { Permissions.GarnerMTC_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuMoTaChung, Description = "Thêm mô tả chung" } },
                { Permissions.GarnerMTC_ThongTinMTC, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Thông tin mô tả chung" } },
                { Permissions.GarnerMTC_ThongTinMTC_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMTC_ThongTinMTC, Description = "Thêm mô tả chung" } },
                { Permissions.GarnerMTC_ThongTinMTC_NoiBat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMTC_ThongTinMTC, Description = "Đặt nôi bật" } },

                { Permissions.GarnerMTC_ThongTinMTC_Xem, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMTC_ThongTinMTC, Description = "Xem chi tiết" } },
                { Permissions.GarnerMTC_ThongTinMTC_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMTC_ThongTinMTC, Description = "Xóa" } },

                // Tổng thầu
                { Permissions.GarnerMenuTongThau, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Tổng thầu" } },
                { Permissions.GarnerTongThau_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuTongThau, Description = "Danh sách" } },
                { Permissions.GarnerTongThau_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuTongThau, Description = "Thêm mới" } },
                { Permissions.GarnerTongThau_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuTongThau, Description = "Xoá" } },
                { Permissions.GarnerTongThau_ThongTinTongThau, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuTongThau, Description = "Thông tin tổng thầu" } },
                    // Tab thông tin chung
                    { Permissions.GarnerTongThau_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerTongThau_ThongTinTongThau, Description = "Thông tin chung" } },
                    { Permissions.GarnerTongThau_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerTongThau_ThongTinChung, Description = "Chi tiết" } },
                
                // Đại lý
                { Permissions.GarnerMenuDaiLy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Đại lý" } },
                { Permissions.GarnerDaiLy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Danh sách" } },
                { Permissions.GarnerDaiLy_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Thêm mới" } },
                { Permissions.GarnerDaiLy_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Kích hoạt/ Đóng" } },
                { Permissions.GarnerDaiLy_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Xóa" } },
                { Permissions.GarnerDaiLy_ThongTinDaiLy, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuDaiLy, Description = "Thông tin đại lý" } },
                    // Tab thông tin chung
                    { Permissions.GarnerDaiLy_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerDaiLy_ThongTinDaiLy, Description = "Thông tin chung" } },
                    { Permissions.GarnerDaiLy_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerDaiLy_ThongTinChung, Description = "Chi tiết" } },

                    // Tab Tài khoản đăng nhập
                    { Permissions.GarnerDaiLy_TKDN, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerDaiLy_ThongTinDaiLy, Description = "Tài khoản đăng nhập" } },
                    { Permissions.GarnerDaiLy_TKDN_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerDaiLy_TKDN, Description = "Thêm mới" } },
                    { Permissions.GarnerDaiLy_TKDN_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerDaiLy_TKDN, Description = "Danh sách" } },

                // Hình ảnh
                { Permissions.GarnerMenuHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Hình ảnh" } },
                { Permissions.GarnerHinhAnh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Danh sách" } },
                { Permissions.GarnerHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Thêm mới" } },
                { Permissions.GarnerHinhAnh_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Sửa" } },
                { Permissions.GarnerHinhAnh_DuyetDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Duyệt đăng/Huỷ duyệt đăng" } },
                { Permissions.GarnerHinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Chi tiết" } },
                { Permissions.GarnerHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHinhAnh, Description = "Xoá" } },

                // Thông báo hệ thống
                { Permissions.GarnerMenuThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Thông báo hệ thống" } },
                { Permissions.GarnerMenuThongBaoHeThong_CaiDat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSetting, Description = "Cài đặt" } },


        // Hợp đồng phân phối
        { Permissions.GarnerMenuHDPP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Hợp đồng phân phối" } },
            
            // Sổ lệnh
            { Permissions.GarnerHDPP_SoLenh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Sổ lệnh" } },
            { Permissions.GarnerHDPP_SoLenh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_SoLenh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh, Description = "Thêm mới" } },
            { Permissions.GarnerHDPP_SoLenh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh, Description = "Xóa" } },
            //
            { Permissions.GarnerHDPP_SoLenh_TTCT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Thông tin sổ lệnh" } },

            // Tab thông tin chung
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "Thông tin chung" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_XemChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Chi tiết" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật" } },

                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiKyHan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật kỳ hạn" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật mã giới thiệu" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật thông tin KH" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTC, Description = "Cập nhật Số tiền đầu tư" } },

                // Tab thông tin thanh toán
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "Thông tin thanh toán" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Danh sách" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Thêm mới" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Cập nhật" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Chi tiết thanh toán" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Phê duyệt" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Huỷ phê duyệt" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Gửi thông báo" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TTThanhToan, Description = "Xoá" } },

                // Tab HSKH đăng ký
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "HSKH đăng ký" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Danh sách" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ mẫu" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Tải hồ sơ chữ ký điện tử" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Upload hồ sơ" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Xem hồ sơ tải lên" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Chuyển Online" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Cập nhật hồ sơ" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Ký điện tử" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Hủy ký điện tử" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Gửi thông báo" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Duyệt hồ sơ / Hủy hồ sơ" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, Description = "Nhận hợp đồng bản cứng" } },
                
                // Tab Lợi nhuận
                { Permissions.GarnerSoLenh_LoiNhuan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "Lợi nhuận" } },
                { Permissions.GarnerSoLenh_LoiNhuan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSoLenh_LoiNhuan, Description = "Danh sách" } },
                
                // Tab Lịch sử hợp đồng
                { Permissions.GarnerSoLenh_LichSuHD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "Lịch sử hợp đồng" } },
                { Permissions.GarnerSoLenh_LichSuHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSoLenh_LichSuHD, Description = "Danh sách" } },
                
                // Trái tức
                { Permissions.GarnerHDPP_SoLenh_TTCT_TraiTuc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT, Description = "Trái tức" } },
                { Permissions.GarnerHDPP_SoLenh_TTCT_TraiTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_SoLenh_TTCT_TraiTuc, Description = "Danh sách" } },

            // Xử lý hợp đồng
            { Permissions.GarnerHDPP_XLHD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Xử lý hợp đồng" } },
            { Permissions.GarnerHDPP_XLHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLHD, Description = "Danh sách" } },
            
            // Hợp đồng
            { Permissions.GarnerHDPP_HopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Hợp đồng" } },
            { Permissions.GarnerHDPP_HopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_HopDong, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_HopDong_YeuCauTaiTuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_HopDong, Description = "Yêu cầu tái tục" } },
            { Permissions.GarnerHDPP_HopDong_YeuCauRutVon, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_HopDong, Description = "Yêu cầu rút vốn" } },
            { Permissions.GarnerHDPP_HopDong_PhongToaHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_HopDong, Description = "Phong tỏa hợp đồng" } },

            // Giao nhận hợp đồng
            { Permissions.GarnerHDPP_GiaoNhanHopDong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Giao nhận hợp đồng" } },
            { Permissions.GarnerHDPP_GiaoNhanHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_GiaoNhanHopDong, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_GiaoNhanHopDong_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_GiaoNhanHopDong, Description = "Đổi trạng thái" } },
            { Permissions.GarnerHDPP_GiaoNhanHopDong_XuatHopDong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_GiaoNhanHopDong, Description = "Xuất hợp đồng" } },
            { Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_GiaoNhanHopDong, Description = "Thông tin chi tiết" } },
            { Permissions.GarnerHDPP_GiaoNhanHopDong_TTC, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, Description = "Thông tin chung" } },
            
            // Phong toả, giải toả
            { Permissions.GarnerHopDong_PhongToaGiaiToa, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Phong toả giải toả" } },
            { Permissions.GarnerHopDong_PhongToaGiaiToa_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_PhongToaGiaiToa, Description = "Danh sách" } },
            { Permissions.GarnerHopDong_PhongToaGiaiToa_GiaiToaHD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_PhongToaGiaiToa, Description = "Giải toả hợp đồng" } },
            { Permissions.GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_PhongToaGiaiToa, Description = "Thông tin phong toả giải toả" } },

            // Hợp đồng đáo hạn 
            { Permissions.GarnerHopDong_HopDongDaoHan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Hợp đồng đáo hạn" } },
            { Permissions.GarnerHopDong_HopDongDaoHan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_HopDongDaoHan, Description = "Danh sách" } },
            { Permissions.GarnerHopDong_HopDongDaoHan_ThongTinDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_HopDongDaoHan, Description = "Thông tin đầu tư" } },
            { Permissions.GarnerHopDong_HopDongDaoHan_LapDSChiTra, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_HopDongDaoHan, Description = "Lập danh sách chi trả" } },
            { Permissions.GarnerHopDong_HopDongDaoHan_DuyetKhongChi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHopDong_HopDongDaoHan, Description = "Duyệt không chi" } },

            // Xử lý rút tiền
            { Permissions.GarnerHDPP_XLRutTien, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Xử lý rút tiền" } },
            { Permissions.GarnerHDPP_XLRutTien_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLRutTien, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_XLRutTien_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLRutTien, Description = "Thông tin chi tiết" } },
            { Permissions.GarnerHDPP_XLRutTien_ChiTienTD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLRutTien, Description = "Chi tiền tự động" } },
            { Permissions.GarnerHDPP_XLRutTien_ChiTienTC, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLRutTien, Description = "Chi tiền thủ công" } },
            { Permissions.GarnerHDPP_XLRutTien_HuyYeuCau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_XLRutTien, Description = "Hủy yêu cầu" } },

             // Chi tra loi tuc
            { Permissions.GarnerHDPP_CTLC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Chi trả lợi tức" } },
            { Permissions.GarnerHDPP_CTLC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_CTLC, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_CTLC_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_CTLC, Description = "Thông tin chi tiết" } },
            { Permissions.GarnerHDPP_CTLC_DuyetChiTD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_CTLC, Description = "Duyệt chi tự động" } },
            { Permissions.GarnerHDPP_CTLC_DuyetChiTC, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_CTLC, Description = "Duyệt chi thủ công" } },
            { Permissions.GarnerHDPP_CTLC_LapDSChiTra, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_CTLC, Description = "Lập danh sach chi trả" } },

            // Lịch sử tích lũy
            { Permissions.GarnerHDPP_LSTL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuHDPP, Description = "Lịch sữ tích lũy" } },
            { Permissions.GarnerHDPP_LSTL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_LSTL, Description = "Danh sách" } },
            { Permissions.GarnerHDPP_LSTL_ThongTinChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerHDPP_LSTL, Description = "Thông tin chi tiết" } },

        // Quản lý đầu tư
        { Permissions.GarnerMenuQLDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Quản lý đầu tư" } },

            //Sản phẩm đầu tư
            { Permissions.GarnerMenuSPTL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLDT, Description = "Sản phẩm đầu tư" } },
            { Permissions.GarnerSPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Danh sách" } },
            { Permissions.GarnerSPDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Thêm mới" } },
            { Permissions.GarnerSPDT_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Trình duyệt" } },
            { Permissions.GarnerSPDT_DongOrMo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Đóng/Mở" } },
            { Permissions.GarnerSPDT_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Epic xác minh" } },
            //{ Permissions.GarnerSPDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Xoá" } },

            { Permissions.GarnerSPDT_ThongTinSPDT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuSPTL, Description = "Thông tin sản phẩm đầu tư" } },
                //Tab Thông tin chung
                { Permissions.GarnerSPDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinSPDT, Description = "Thông tin chung" } },
                { Permissions.GarnerSPDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinChung, Description = "Chi tiết" } },
                { Permissions.GarnerSPDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinChung, Description = "Cập nhật" } },

                //Tab đại lý phân phối
                { Permissions.GarnerSPDT_DLPP, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinSPDT, Description = "Đại lý phân phối" } },
                { Permissions.GarnerSPDT_DLPP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_DLPP, Description = "Danh sách" } },
                { Permissions.GarnerSPDT_DLPP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_DLPP, Description = "Thêm mới" } },
                { Permissions.GarnerSPDT_DLPP_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_DLPP, Description = "Cập nhật" } },
                //{ Permissions.GarnerSPDT_DLPP_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_DLPP, Description = "Duyệt đăng / Bỏ duyệt đăng" } },
                { Permissions.GarnerSPDT_DLPP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_DLPP, Description = "Xoá" } },

                 //Tab Tai san dam bao
                { Permissions.GarnerSPDT_TSDB, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinSPDT, Description = "Tài sản đảm bảo" } },
                { Permissions.GarnerSPDT_TSDB_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Danh sách" } },
                { Permissions.GarnerSPDT_TSDB_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Thêm mới" } },
                { Permissions.GarnerSPDT_TSDB_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Cập nhật" } },
                { Permissions.GarnerSPDT_TSDB_Preview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Xem file" } },
                { Permissions.GarnerSPDT_TSDB_DownloadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Tải file" } },
                { Permissions.GarnerSPDT_TSDB_DeleteFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TSDB, Description = "Xoá file" } },
                //Tab Hồ sơ pháp lý
                { Permissions.GarnerSPDT_HSPL, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinSPDT, Description = "Hồ sơ pháp lý" } },
                { Permissions.GarnerSPDT_HSPL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Danh sách" } },
                { Permissions.GarnerSPDT_HSPL_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Thêm mới" } },
                 { Permissions.GarnerSPDT_HSPL_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Cập nhật" } },
                { Permissions.GarnerSPDT_HSPL_Preview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Xem file" } },
                { Permissions.GarnerSPDT_HSPL_DownloadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Tải file" } },
                { Permissions.GarnerSPDT_HSPL_DeleteFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_HSPL, Description = "Xoá file" } },

                //Tab tin tức sản phẩm
                { Permissions.GarnerSPDT_TTSP, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_ThongTinSPDT, Description = "Tin tức sản phẩm" } },
                { Permissions.GarnerSPDT_TTSP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TTSP, Description = "Danh sách" } },
                { Permissions.GarnerSPDT_TTSP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TTSP, Description = "Thêm mới" } },
                { Permissions.GarnerSPDT_TTSP_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TTSP, Description = "Cập nhật" } },
                { Permissions.GarnerSPDT_TTSP_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TTSP, Description = "Duyệt đăng / Bỏ duyệt đăng" } },
                { Permissions.GarnerSPDT_TTSP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerSPDT_TTSP, Description = "Xoá" } },

            //Phân phối đầu tư
            { Permissions.GarnerMenuPPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLDT, Description = "Phân phối đầu tư" } },
            { Permissions.GarnerPPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Danh sách" } },
            { Permissions.GarnerPPDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Thêm mới" } },
            { Permissions.GarnerPPDT_DongOrMo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Đóng tạm / Mở" } },
            { Permissions.GarnerPPDT_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Trình duyệt" } },
            { Permissions.GarnerPPDT_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Bật / Tắt show app" } },
            { Permissions.GarnerPPDT_EpicXacMinh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Epic xác minh" } },

            { Permissions.GarnerPPDT_ThongTinPPDT, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPPDT, Description = "Thông tin phân phối đầu tư" } },

                //Tab Thông tin chung
                { Permissions.GarnerPPDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Thông tin chung" } },
                { Permissions.GarnerPPDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinChung, Description = "Chi tiết" } },
                { Permissions.GarnerPPDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinChung, Description = "Cập nhật" } },

                //Tab tổng quan
                { Permissions.GarnerPPDT_TongQuan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Tổng quan" } },
                { Permissions.GarnerPPDT_TongQuan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Chỉnh sửa" } },
                { Permissions.GarnerPPDT_TongQuan_ChonAnh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Chọn ảnh" } },
                { Permissions.GarnerPPDT_TongQuan_ThemToChuc, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Thêm tổ chức" } },
                { Permissions.GarnerPPDT_TongQuan_DanhSach_ToChuc, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Danh sách tổ chức" } },
                { Permissions.GarnerPPDT_TongQuan_UploadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Upload file" } },
                { Permissions.GarnerPPDT_TongQuan_DanhSach_File, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_TongQuan, Description = "Danh sách file" } },

                //Tab Chính sách
                { Permissions.GarnerPPDT_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Chính sách" } },
                { Permissions.GarnerPPDT_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Danh sách" } },
                { Permissions.GarnerPPDT_ChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Thêm chính sách" } },
                { Permissions.GarnerPPDT_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Cập nhật chính sách" } },
                { Permissions.GarnerPPDT_ChinhSach_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Kích hoạt / Huỷ (Chính sách)" } },
                { Permissions.GarnerPPDT_ChinhSach_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Bật ? Tắt show App (Chính sách)" } },
                { Permissions.GarnerPPDT_ChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Xoá chính sách" } },

                { Permissions.GarnerPPDT_KyHan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Thêm kỳ hạn" } },
                { Permissions.GarnerPPDT_KyHan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Cập nhật kỳ hạn" } },
                { Permissions.GarnerPPDT_KyHan_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Kích hoạt / Huỷ (Kỳ hạn)" } },
                { Permissions.GarnerPPDT_KyHan_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Bật / Tắt show App (Kỳ hạn)" } },
                { Permissions.GarnerPPDT_KyHan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Xoá (Kỳ hạn)" } },

                { Permissions.GarnerPPDT_HopDongMau_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Thêm hợp đồng mẫu" } },
                { Permissions.GarnerPPDT_HopDongMau_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Cập nhật hợp đồng mẫu" } },
                { Permissions.GarnerPPDT_HopDongMau_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Kích hoạt / Huỷ (Hợp đồng mẫu)" } },
                { Permissions.GarnerPPDT_HopDongMau_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Bật / Tắt show App (Hợp đồng mẫu)" } },
                { Permissions.GarnerPPDT_HopDongMau_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ChinhSach, Description = "Xoá (Hợp đồng mẫu)" } },

                //Tab file chính sách
                { Permissions.GarnerPPDT_FileChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "File chính sách" } },
                { Permissions.GarnerPPDT_FileChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_FileChinhSach, Description = "Danh sách" } },
                { Permissions.GarnerPPDT_FileChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_FileChinhSach, Description = "Thêm mới" } },
                { Permissions.GarnerPPDT_FileChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_FileChinhSach, Description = "Cập nhật" } },
                { Permissions.GarnerPPDT_FileChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_FileChinhSach, Description = "Xoá" } },

                //Tab bảng giá
                { Permissions.Garner_TTCT_BangGia, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Bảng giá" } },
                { Permissions.Garner_TTCT_BangGia_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_TTCT_BangGia, Description = "Danh sách" } },
                { Permissions.Garner_TTCT_BangGia_ImportExcelBG, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_TTCT_BangGia, Description = "Import file excel bảng giá" } },
                { Permissions.Garner_TTCT_BangGia_DownloadFileMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_TTCT_BangGia, Description = "Download file mẫu" } },
                { Permissions.Garner_TTCT_BangGia_XoaBangGia, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_TTCT_BangGia, Description = "Xóa bảng giá" } },

                //Tab mẫu hợp đồng
                { Permissions.GarnerPPDT_MauHopDong, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Mẫu hợp đồng" } },
                { Permissions.GarnerPPDT_MauHopDong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauHopDong, Description = "Danh sách" } },
                { Permissions.GarnerPPDT_MauHopDong_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauHopDong, Description = "Thêm mới" } },
                { Permissions.GarnerPPDT_MauHopDong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauHopDong, Description = "Cập nhật" } },
                { Permissions.GarnerPPDT_MauHopDong_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauHopDong, Description = "Xoá" } },

                //Tab Hợp đồng phân phối
                { Permissions.GarnerPPDT_HopDongPhanPhoi, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Hợp đồng phân phối" } },
                { Permissions.GarnerPPDT_HopDongPhanPhoi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_HopDongPhanPhoi, Description = "Danh sách" } },
                { Permissions.GarnerPPDT_HopDongPhanPhoi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_HopDongPhanPhoi, Description = "Thêm mới" } },
                { Permissions.GarnerPPDT_HopDongPhanPhoi_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_HopDongPhanPhoi, Description = "Cập nhật" } },
                { Permissions.GarnerPPDT_HopDongPhanPhoi_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_HopDongPhanPhoi, Description = "Xoá" } },

                //Tab Mẫu giao nhận HĐ
                { Permissions.GarnerPPDT_MauGiaoNhanHD, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_ThongTinPPDT, Description = "Mẫu giao nhận HĐ" } },
                { Permissions.GarnerPPDT_MauGiaoNhanHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauGiaoNhanHD, Description = "Danh sách" } },
                { Permissions.GarnerPPDT_MauGiaoNhanHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauGiaoNhanHD, Description = "Thêm mới" } },
                { Permissions.GarnerPPDT_MauGiaoNhanHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauGiaoNhanHD, Description = "Cập nhật" } },
                { Permissions.GarnerPPDT_MauGiaoNhanHD_KichHoat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauGiaoNhanHD, Description = "Kích hoạt" } },
                { Permissions.GarnerPPDT_MauGiaoNhanHD_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerPPDT_MauGiaoNhanHD, Description = "Xoá" } },

            // Quản lý phê duyệt
            { Permissions.GarnerMenuQLPD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Quản lý phê duyệt" } },
            // Phê duyệt sản phẩm tích lũy
            { Permissions.GarnerMenuPDSPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLPD, Description = "Phê duyệt sản phẩm đầu tư" } },
            { Permissions.GarnerPDSPTL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDSPDT, Description = "Danh sách" } },
            { Permissions.GarnerPDSPTL_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDSPDT, Description = "Phê duyệt / Hủy" } },
            
            // Phê duyệt phân phối đầu tư
            { Permissions.GarnerMenuPDPPDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLPD, Description = "Phê duyệt phân phối đầu tư" } },
            { Permissions.GarnerPDPPDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDPPDT, Description = "Danh sách" } },
            { Permissions.GarnerPDPPDT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDPPDT, Description = "Phê duyệt / Hủy" } },
            // Phê duyệt yêu cầu tái tục
            { Permissions.GarnerMenuPDYCTT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLPD, Description = "Phê duyệt yêu cầu tái tục" } },
            { Permissions.GarnerPDYCTT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDYCTT, Description = "Danh sách" } },
            { Permissions.GarnerPDYCTT_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDYCTT, Description = "Phê duyệt / Hủy" } },

            // Phê duyệt yêu cầu rút vốn
            { Permissions.GarnerMenuPDYCRV, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuQLPD, Description = "Phê duyệt yêu cầu rút vốn" } },
            { Permissions.GarnerPDYCRV_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDYCRV, Description = "Danh sách" } },
            { Permissions.GarnerPDYCRV_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDYCRV, Description = "Phê duyệt / Hủy" } },
            { Permissions.GarnerPDYCRV_ChiTietHD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.GarnerMenuPDYCRV, Description = "Chi tiết hợp đồng" } },

            // Báo cáo
            { Permissions.Garner_Menu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Menu báo cáo" } },

            { Permissions.Garner_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_Menu_BaoCao, Description = "Báo cáo quản trị" } },
            { Permissions.Garner_BaoCao_QuanTri_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_QuanTri, Description = "B.C tổng chi trả đầu tư" } },
            { Permissions.Garner_BaoCao_QuanTri_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_QuanTri, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Garner_BaoCao_QuanTri_THSanPhamTichLuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_QuanTri, Description = "B.C Tổng hợp các sản phẩm tích luỹ" } },
            { Permissions.Garner_BaoCao_QuanTri_QuanTriTH, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_QuanTri, Description = "B.C Quản trị tổng hợp" } },
            { Permissions.Garner_BaoCao_QuanTri_DauTuBanHo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_QuanTri, Description = "B.C Các khoản đầu tư bán hộ" } },

            { Permissions.Garner_BaoCao_VanHanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_Menu_BaoCao, Description = "Báo cáo vận hành" } },
            { Permissions.Garner_BaoCao_VanHanh_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_VanHanh, Description = "B.C tổng chi trả đầu tư" } },
            { Permissions.Garner_BaoCao_VanHanh_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_VanHanh, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Garner_BaoCao_VanHanh_THSanPhamTichLuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_VanHanh, Description = "B.C Tổng hợp các sản phẩm tích luỹ" } },
            { Permissions.Garner_BaoCao_VanHanh_QuanTriTH, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_VanHanh, Description = "B.C Quản trị tổng hợp" } },
            { Permissions.Garner_BaoCao_VanHanh_DauTuBanHo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_VanHanh, Description = "B.C Các khoản đầu tư bán hộ" } },


            { Permissions.Garner_BaoCao_KinhDoanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_Menu_BaoCao, Description = "Báo cáo kinh doanh" } },
            { Permissions.Garner_BaoCao_KinhDoanh_TCTDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_KinhDoanh, Description = "B.C tổng chi trả đầu tư" } },
            { Permissions.Garner_BaoCao_KinhDoanh_THCKDauTu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_KinhDoanh, Description = "B.C tổng hợp các khoản đầu tư" } },
            { Permissions.Garner_BaoCao_KinhDoanh_THSanPhamTichLuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_KinhDoanh, Description = "B.C Tổng hợp các sản phẩm tích luỹ" } },
            { Permissions.Garner_BaoCao_KinhDoanh_QuanTriTH, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_KinhDoanh, Description = "B.C Quản trị tổng hợp" } },
            { Permissions.Garner_BaoCao_KinhDoanh_DauTuBanHo, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_BaoCao_KinhDoanh, Description = "B.C Các khoản đầu tư bán hộ" } },

            { Permissions.Garner_Menu_TruyVan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Garner, ParentKey = null, Description = "Menu truy vấn" } },
            { Permissions.Garner_TruyVan_ThuTien_NganHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_Menu_TruyVan, Description = "Thu tiền ngân hàng" } },
            { Permissions.Garner_TruyVan_ChiTien_NganHang, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Garner, ParentKey = Permissions.Garner_Menu_TruyVan, Description = "Chi tiền ngân hàng" } },
              #endregion

            #region trang realestate
            { Permissions.RealStateWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "" } },
            { Permissions.RealStatePageDashboard, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Dashboard tổng quan" } },
            
            // Menu Cài đặt
            { Permissions.RealStateMenuSetting, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Cài đặt" } },

            // Module chủ đầu tư
            { Permissions.RealStateMenuChuDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Chủ đầu tư" } },
            { Permissions.RealStateChuDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuChuDT, Description = "Danh sách" } },
            { Permissions.RealStateChuDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuChuDT, Description = "Thêm mới" } },
            { Permissions.RealStateChuDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuChuDT, Description = "Xoá" } },
            { Permissions.RealStateChuDT_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuChuDT, Description = "Kích hoạt/Huỷ" } },
            { Permissions.RealStateChuDT_ThongTinChuDauTu, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuChuDT, Description = "Thông tin chủ đầu tư" } },
            // Thông tin chung
            { Permissions.RealStateChuDT_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateChuDT_ThongTinChuDauTu, Description = "Thông tin chung" } },
            { Permissions.RealStateChuDT_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateChuDT_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStateChuDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateChuDT_ThongTinChung, Description = "Cập nhật" } },
     
            // Module Đại lý
            { Permissions.RealStateMenuDaiLy, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Đại lý" } },
            { Permissions.RealStateDaiLy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuDaiLy, Description = "Danh sách" } },
            { Permissions.RealStateDaiLy_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuDaiLy, Description = "Thêm mới" } },
            { Permissions.RealStateDaiLy_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuDaiLy, Description = "Kích hoạt/Huỷ" } },
            { Permissions.RealStateDaiLy_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuDaiLy, Description = "Xoá" } },
            { Permissions.RealStateDaiLy_ThongTinDaiLy, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuDaiLy, Description = "Thông tin đại lý" } },

            { Permissions.RealStateDaiLy_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateDaiLy_ThongTinDaiLy, Description = "Thông tin chung" } },
            { Permissions.RealStateDaiLy_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateDaiLy_ThongTinChung, Description = "Chi tiết" } },

            // Module Chính sách phân phối
            { Permissions.RealStateMenuCSPhanPhoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Chính sách phân phối" } },
            { Permissions.RealStateCSPhanPhoi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSPhanPhoi, Description = "Danh sách" } },
            { Permissions.RealStateCSPhanPhoi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSPhanPhoi, Description = "Thêm chính sách" } },
            { Permissions.RealStateCSPhanPhoi_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSPhanPhoi, Description = "Cập nhật chính sách" } },
            { Permissions.RealStateCSPhanPhoi_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSPhanPhoi, Description = "Kích hoạt / Huỷ (Chính sách)" } },
            { Permissions.RealStateCSPhanPhoi_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSPhanPhoi, Description = "Xoá (Chính sách)" } },

            // Module Chính sách bán hàng
            { Permissions.RealStateMenuCSBanHang, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Chính sách bán hàng" } },
            { Permissions.RealStateCSBanHang_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSBanHang, Description = "Danh sách" } },
            { Permissions.RealStateCSBanHang_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSBanHang, Description = "Thêm chính sách" } },
            { Permissions.RealStateCSBanHang_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSBanHang, Description = "Cập nhật chính sách" } },
            { Permissions.RealStateCSBanHang_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSBanHang, Description = "Kích hoạt / Huỷ (Chính sách)" } },
            { Permissions.RealStateCSBanHang_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCSBanHang, Description = "Xoá (Chính sách)" } },
            
            // Cấu trúc hợp đồng giao dịch
            { Permissions.RealStateMenuCTMaHDGiaoDich, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Mẫu HĐ chủ đầu tư" } },
            { Permissions.RealStateCTMaHDGiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDGiaoDich, Description = "Danh sách" } },
            { Permissions.RealStateCTMaHDGiaoDich_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDGiaoDich, Description = "Thêm mới" } },
            { Permissions.RealStateCTMaHDGiaoDich_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDGiaoDich, Description = "Cập nhật" } },
            { Permissions.RealStateCTMaHDGiaoDich_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDGiaoDich, Description = "Kích hoạt / Huỷ" } },
            { Permissions.RealStateCTMaHDGiaoDich_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDGiaoDich, Description = "Xoá" } },


            // Cấu trúc hợp đồng cọc
            { Permissions.RealStateMenuCTMaHDCoc, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Mẫu HĐ chủ đầu tư" } },
            { Permissions.RealStateCTMaHDCoc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDCoc, Description = "Danh sách" } },
            { Permissions.RealStateCTMaHDCoc_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDCoc, Description = "Thêm mới" } },
            { Permissions.RealStateCTMaHDCoc_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDCoc, Description = "Cập nhật" } },
            { Permissions.RealStateCTMaHDCoc_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDCoc, Description = "Kích hoạt / Huỷ" } },
            { Permissions.RealStateCTMaHDCoc_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuCTMaHDCoc, Description = "Xoá" } },

                // Module Mẫu hợp đồng chủ đầu tư
            { Permissions.RealStateMenuMauHDCDT, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Mẫu HĐ chủ đầu tư" } },
            { Permissions.RealStateMauHDCDT_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Danh sách" } },
            { Permissions.RealStateMauHDCDT_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Thêm mới" } },
            { Permissions.RealStateMauHDCDT_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Cập nhật" } },
            { Permissions.RealStateMauHDCDT_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Kích hoạt / Huỷ" } },
            { Permissions.RealStateMauHDCDT_TaiFileDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Tải file d/nghiệp" } },
            { Permissions.RealStateMauHDCDT_TaiFileCaNhan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Tải file cá nhân" } },
            { Permissions.RealStateMauHDCDT_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDCDT, Description = "Xoá" } },

              // Module Mẫu hợp đồng đại lý
            { Permissions.RealStateMenuMauHDDL, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Mẫu HĐ chủ đầu tư" } },
            { Permissions.RealStateMauHDDL_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Danh sách" } },
            { Permissions.RealStateMauHDDL_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Thêm mới" } },
            { Permissions.RealStateMauHDDL_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Cập nhật" } },
            { Permissions.RealStateMauHDDL_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Kích hoạt / Huỷ" } },
            { Permissions.RealStateMauHDDL_TaiFileDoanhNghiep, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Tải file d/nghiệp" } },
            { Permissions.RealStateMauHDDL_TaiFileCaNhan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Tải file cá nhân" } },
            { Permissions.RealStateMauHDDL_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuMauHDDL, Description = "Xoá" } },
            
            // Thông báo hệ thống
            { Permissions.RealStateThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSetting, Description = "Thông báo hệ thống" } },
            
            // Menu Quản lý dự án
            { Permissions.RealStateMenuProjectManager, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Quản lý dự án" } },

            { Permissions.RealStateMenuProjectOverview, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectManager, Description = "Tổng quan dự án" } },
            { Permissions.RealStateProjectOverview_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Trình duyệt" } },
            { Permissions.RealStateProjectOverview_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Phê duyệt" } },

            { Permissions.RealStateProjectOverview_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Xóa" } },
            { Permissions.RealStateProjectOverview_ThongTinProjectOverview, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Thông tin tổng quan mở bán" } },

            { Permissions.RealStateProjectOverview_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Thông tin chung" } },
            { Permissions.RealStateProjectOverview_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Chi tiết" } },
            { Permissions.RealStateProjectOverview_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectOverview, Description = "Cập nhật" } },

            { Permissions.RealStateProjectOverview_MoTa, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Mô tả dự án" } },
            { Permissions.RealStateProjectOverview_MoTa_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_MoTa, Description = "Chi tiết" } },
            { Permissions.RealStateProjectOverview_MoTa_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_MoTa, Description = "Cập nhật" } },

            // Tab tiện ích
            { Permissions.RealStateProjectOverview_TienIch, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Tiện ích" } },

            { Permissions.RealStateProjectOverview_TienIchHeThong, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIch, Description = "Danh sách tiện ích hệ thống" } },
            { Permissions.RealStateProjectOverview_TienIchHeThong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchHeThong, Description = "Danh sách tiện ích hệ thống" } },
            { Permissions.RealStateProjectOverview_TienIchHeThong_QuanLy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchHeThong, Description = "Quản lý tiện ích hệ thống" } },

            { Permissions.RealStateProjectOverview_TienIchKhac, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIch, Description = "Tab tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Danh sách tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Thêm mới tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Cập nhật tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Đổi trạng thái tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_DoiTrangThaiNoiBat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Đổi trạng thái nổi bật tiện ích khác" } },
            { Permissions.RealStateProjectOverview_TienIchKhac_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchKhac, Description = "Xoá tiện ích khác" } },

            { Permissions.RealStateProjectOverview_TienIchMinhHoa, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIch, Description = "Tab ảnh minh hoạ" } },
            { Permissions.RealStateProjectOverview_TienIchMinhHoa_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchMinhHoa, Description = "Danh sách ảnh minh hoạ" } },
            { Permissions.RealStateProjectOverview_TienIchMinhHoa_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchMinhHoa, Description = "Thêm mới ảnh minh hoạ" } },
            { Permissions.RealStateProjectOverview_TienIchMinhHoa_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchMinhHoa, Description = "Cập nhật ảnh minh hoạ" } },
            { Permissions.RealStateProjectOverview_TienIchMinhHoa_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchMinhHoa, Description = "Đổi trạng thái ảnh minh hoạ" } },
            { Permissions.RealStateProjectOverview_TienIchMinhHoa_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_TienIchMinhHoa, Description = "Xoá ảnh minh hoạ" } },

            // Tab cấu trúc dự án
            { Permissions.RealStateProjectOverview_CauTruc, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Cấu trúc dự án" } },
            { Permissions.RealStateProjectOverview_CauTruc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_CauTruc, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_CauTruc_Them, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_CauTruc, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_CauTruc_Sua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_CauTruc, Description = "Chỉnh sửa" } },
            { Permissions.RealStateProjectOverview_CauTruc_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_CauTruc, Description = "Xóa" } },

            // Tab hình ảnh
            { Permissions.RealStateProjectOverview_HinhAnhDuAn, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Hình ảnh dự án" } },

            { Permissions.RealStateProjectOverview_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnhDuAn, Description = "Hình ảnh" } },
            { Permissions.RealStateProjectOverview_HinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStateProjectOverview_HinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_HinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStateProjectOverview_HinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectOverview_HinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnh, Description = "Xoá" } },

            { Permissions.RealStateProjectOverview_NhomHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HinhAnhDuAn, Description = "Nhóm hình ảnh" } },
            { Permissions.RealStateProjectOverview_NhomHinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_NhomHinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStateProjectOverview_NhomHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_NhomHinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_NhomHinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_NhomHinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStateProjectOverview_NhomHinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_NhomHinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectOverview_NhomHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_NhomHinhAnh, Description = "Xoá" } },
            
            // Tab cấu trúc dự án
            { Permissions.RealStateProjectOverview_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Cấu trúc dự án" } },
            { Permissions.RealStateProjectOverview_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChinhSach, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_ChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChinhSach, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChinhSach, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_ChinhSach_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChinhSach, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_ChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChinhSach, Description = "Quản lý" } },

            // Tab Hồ sơ pháp lý
            { Permissions.RealStateProjectOverview_HoSo, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Cấu trúc dự án" } },
            { Permissions.RealStateProjectOverview_HoSo_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_HoSo_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_HoSo_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_HoSo_XemFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_HoSo_TaiXuong, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_HoSo_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },
            { Permissions.RealStateProjectOverview_HoSo_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Quản lý" } },

            // Tab bài đăng facebook
            { Permissions.RealStateProjectOverview_Facebook_Post, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Bài đăng facebook" } },
            { Permissions.RealStateProjectOverview_Facebook_Post_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_Facebook_Post, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_Facebook_Post_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_Facebook_Post, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_Facebook_Post_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_Facebook_Post, Description = "Chỉnh sửa" } },
            { Permissions.RealStateProjectOverview_Facebook_Post_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_Facebook_Post, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectOverview_Facebook_Post_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_Facebook_Post, Description = "Xóa" } },
            
            // Tab Chia sẻ dự án
            { Permissions.RealStateProjectOverview_ChiaSeDuAn, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Chia sẻ dự án" } },
            { Permissions.RealStateProjectOverview_ChiaSeDuAn_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChiaSeDuAn, Description = "Danh sách" } },
            { Permissions.RealStateProjectOverview_ChiaSeDuAn_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChiaSeDuAn, Description = "Thêm mới" } },
            { Permissions.RealStateProjectOverview_ChiaSeDuAn_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChiaSeDuAn, Description = "Chỉnh sửa" } },
            { Permissions.RealStateProjectOverview_ChiaSeDuAn_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChiaSeDuAn, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectOverview_ChiaSeDuAn_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ChiaSeDuAn, Description = "Xóa" } },

            // Menu Bảng hàng dự án
            { Permissions.RealStateMenuProjectList, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Bảng hàng dự án" } },
            { Permissions.RealStateMenuProjectList_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_ThongTinProjectOverview, Description = "Danh sách" } },
            
            { Permissions.RealStateMenuProjectListDetail, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Chi tiết bảng hàng" } },
            { Permissions.RealStateMenuProjectListDetail_DanhSach, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Danh sách chi tiết bảng hàng" } },
            { Permissions.RealStateMenuProjectListDetail_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Thêm mới" } },
            { Permissions.RealStateMenuProjectListDetail_UploadFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Upload File" } },
            { Permissions.RealStateMenuProjectListDetail_TaiFileMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Tải File mẫu" } },
            { Permissions.RealStateMenuProjectListDetail_KhoaCan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Khoá căn" } },
            { Permissions.RealStateMenuProjectListDetail_NhanBan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Nhân bản" } },
            
            { Permissions.RealStateMenuProjectListDetail_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectOverview_HoSo, Description = "Chi tiết căn hộ" } },
           
            // Tab Thông tin chung - Bảng hàng
            { Permissions.RealStateMenuProjectListDetail_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Thông tin chung" } },
            { Permissions.RealStateMenuProjectListDetail_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStateMenuProjectListDetail_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ThongTinChung, Description = "Cập nhật" } },
            
            // Tab Chính sách ưu đãi CĐT - Bảng hàng
            { Permissions.RealStateMenuProjectListDetail_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Chính sách ưu đãi CĐT" } },
            { Permissions.RealStateMenuProjectListDetail_ChinhSach_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChinhSach, Description = "Chi tiết" } },
            { Permissions.RealStateMenuProjectListDetail_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChinhSach, Description = "Cập nhật" } },
            { Permissions.RealStateMenuProjectListDetail_ChinhSach_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChinhSach, Description = "Đổi trạng thái" } },
               
            // Tab Tiện ích - Bảng hàng
            { Permissions.RealStateMenuProjectListDetail_TienIch, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Tiện ích" } },
            { Permissions.RealStateMenuProjectListDetail_TienIch_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_TienIch, Description = "Chi tiết" } },
            { Permissions.RealStateMenuProjectListDetail_TienIch_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_TienIch, Description = "Cập nhật" } },
            { Permissions.RealStateMenuProjectListDetail_TienIch_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_TienIch, Description = "Đổi trạng thái" } },
        
            // Tab Vật liệu - Bảng hàng
            { Permissions.RealStateMenuProjectListDetail_VatLieu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Vật liệu" } },
            { Permissions.RealStateMenuProjectListDetail_VatLieu_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_VatLieu, Description = "Chi tiết" } },
            { Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_VatLieu, Description = "Cập nhật" } },
        
            // Tab Sơ đồ thiết kế - Bảng hàng
            { Permissions.RealStateMenuProjectListDetail_SoDoTK, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Sơ đồ thiết kế" } },
            { Permissions.RealStateMenuProjectListDetail_SoDoTK_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_SoDoTK, Description = "Chi tiết" } },
            { Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_SoDoTK, Description = "Cập nhật" } },
        
            // Tab Hình ảnh - Bảng hàng
            { Permissions.RealStateProjectListDetail_HinhAnhDuAn, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Hình ảnh dự án" } },
           
            { Permissions.RealStateProjectListDetail_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnhDuAn, Description = "Hình ảnh" } },
            { Permissions.RealStateProjectListDetail_HinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStateProjectListDetail_HinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStateProjectListDetail_HinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStateProjectListDetail_HinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectListDetail_HinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnh, Description = "Xoá" } },

            { Permissions.RealStateProjectListDetail_NhomHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_HinhAnhDuAn, Description = "Nhóm hình ảnh" } },
            { Permissions.RealStateProjectListDetail_NhomHinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_NhomHinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStateProjectListDetail_NhomHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_NhomHinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStateProjectListDetail_NhomHinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_NhomHinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStateProjectListDetail_NhomHinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_NhomHinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStateProjectListDetail_NhomHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_NhomHinhAnh, Description = "Xoá" } },

            // Tab lịch sử - Bảng hàng
            { Permissions.RealStateProjectListDetail_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Lịch sử" } },
            { Permissions.RealStateProjectListDetail_LichSu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateProjectListDetail_LichSu, Description = "Danh sách" } },

            // Menu Phân phối
            { Permissions.RealStateMenuPhanPhoi, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectManager, Description = "Phân phối sản phẩm" } },
            { Permissions.RealStatePhanPhoi_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi, Description = "Danh sách" } },
            { Permissions.RealStatePhanPhoi_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi, Description = "Thêm mới" } },
            { Permissions.RealStatePhanPhoi_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi, Description = "Đổi trạng thái" } },
            { Permissions.RealStatePhanPhoi_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi, Description = "Xoá" } },
            { Permissions.RealStatePhanPhoi_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi, Description = "Chi tiết" } },

            // Tab Thông tin chung - Phân phối
            { Permissions.RealStateMenuPhanPhoi_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_ChiTiet, Description = "Thông tin chung phân phối" } },
            { Permissions.RealStateMenuPhanPhoi_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStateMenuPhanPhoi_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi_ThongTinChung, Description = "Cập nhật" } },
            { Permissions.RealStateMenuPhanPhoi_ThongTinChung_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi_ThongTinChung, Description = "Trình duyệt" } },
            { Permissions.RealStateMenuPhanPhoi_ThongTinChung_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPhanPhoi_ThongTinChung, Description = "Phê duyệt" } },
 
            // Tab Danh sách sản phẩm
            { Permissions.RealStatePhanPhoi_DSSP, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_ChiTiet, Description = "Thông tin chung" } },
            { Permissions.RealStatePhanPhoi_DSSP_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Thêm mới" } },
            { Permissions.RealStatePhanPhoi_DSSP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Danh sách" } },
            { Permissions.RealStatePhanPhoi_DSSP_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Đổi trạng thái" } },
            { Permissions.RealStatePhanPhoi_DSSP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Xoá" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Chi tiết" } },

            // Tab Thông tin chung - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Thông tin chung" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, Description = "Cập nhật" } },
            
            // Tab Chính sách ưu đãi CĐT - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Chính sách ưu đãi CĐT" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, Description = "Cập nhật" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, Description = "Đổi trạng thái" } },
               
            // Tab Tiện ích - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Tiện ích" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, Description = "Cập nhật" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, Description = "Đổi trạng thái" } },
        
             // Tab Vật liệu - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Vật liệu" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu, Description = "Cập nhật" } },
        
             // Tab Sơ đồ thiết kế - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuProjectListDetail_ChiTiet, Description = "Sơ đồ thiết kế" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK, Description = "Cập nhật" } },
        
            // Tab Hình ảnh - Chi tiết căn hộ phân phối
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Hình ảnh dự án" } },

            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn, Description = "Hình ảnh" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, Description = "Xoá" } },

            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn, Description = "Nhóm hình ảnh" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, Description = "Thêm mới" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, Description = "Cập nhật" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, Description = "Đổi trạng thái" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, Description = "Xoá" } },

            // Tab lịch sử - Bảng hàng
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet, Description = "Lịch sử" } },
            { Permissions.RealStatePhanPhoi_DSSP_ChiTiet_LichSu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP_ChiTiet_LichSu, Description = "Danh sách" } },

            // Tab Chính sách phân phối
            { Permissions.RealStatePhanPhoi_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_ChiTiet, Description = "Chính sách phân phối" } },
            { Permissions.RealStatePhanPhoi_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Danh sách" } },
            { Permissions.RealStatePhanPhoi_ChinhSach_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Chi tiết" } },
            { Permissions.RealStatePhanPhoi_ChinhSach_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Đổi trạng thái" } },
            { Permissions.RealStatePhanPhoi_ChinhSach_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Xoá" } },
            { Permissions.RealStatePhanPhoi_ChinhSach_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_DSSP, Description = "Xoá" } },


            // Tab mẫu biểu hợp đồng
            { Permissions.RealStatePhanPhoi_MauBieu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_ChiTiet, Description = "Mẫu biểu hợp đồng" } },
            { Permissions.RealStatePhanPhoi_MauBieu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Danh sách" } },
            { Permissions.RealStatePhanPhoi_MauBieu_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Thêm mới" } },
            { Permissions.RealStatePhanPhoi_MauBieu_XuatWord, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Xuất Word" } },
            { Permissions.RealStatePhanPhoi_MauBieu_XuatPdf, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Xuất PDF" } },
            { Permissions.RealStatePhanPhoi_MauBieu_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Chỉnh sửa" } },
            { Permissions.RealStatePhanPhoi_MauBieu_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Đổi trạng thái" } },

            // Menu Mở bán
            { Permissions.RealStateMenuMoBan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_ChiTiet, Description = "Mở bán" } },
            { Permissions.RealStateMoBan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Danh sách" } },
            { Permissions.RealStateMoBan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Thêm mới" } },
            { Permissions.RealStateMoBan_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Đổi trạng thái" } },
            { Permissions.RealStateMoBan_DoiNoiBat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Đổi nổi bật" } },
            { Permissions.RealStateMoBan_DoiShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Đổi ShowApp" } },
            { Permissions.RealStateMoBan_DungBan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Dừng bán" } },
            { Permissions.RealStateMoBan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Xoá" } },
            
            { Permissions.RealStateMoBan_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStatePhanPhoi_MauBieu, Description = "Chi tiết" } },
            // Tab thông tin chung
            { Permissions.RealStateMoBan_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChiTiet, Description = "Thông tin chung mở bán" } },
            { Permissions.RealStateMoBan_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStateMoBan_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ThongTinChung, Description = "Cập nhật" } },
            { Permissions.RealStateMoBan_ThongTinChung_TrinhDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ThongTinChung, Description = "Trình duyệt" } },
            { Permissions.RealStateMoBan_ThongTinChung_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ThongTinChung, Description = "Phê duyệt" } },
            
            // Tab Danh sách sản phẩm - Mở bán
            { Permissions.RealStateMoBan_DSSP, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChiTiet, Description = "Danh sách sản phẩm" } },
            { Permissions.RealStateMoBan_DSSP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Danh sách" } },
            { Permissions.RealStateMoBan_DSSP_Them, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Thêm mới" } },
            { Permissions.RealStateMoBan_DSSP_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Đổi trạng thái" } },
            { Permissions.RealStateMoBan_DSSP_DoiShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Đổi trạng thái ShowApp" } },
            { Permissions.RealStateMoBan_DSSP_DoiShowPrice, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Đổi trạng thái hiển thị giá" } },
            { Permissions.RealStateMoBan_DSSP_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Xóa" } },
            { Permissions.RealStateMoBan_DSSP_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP, Description = "Chi tiết" } },
                
            // Tab Thông tin chung - Chi tiết căn hộ mở bán
            { Permissions.RealStateMoBan_DSSP_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP_ChiTiet, Description = "Thông tin chung" } },
            { Permissions.RealStateMoBan_DSSP_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP_ChiTiet_ThongTinChung, Description = "Chi tiết" } },
            // Tab Lịch sử - Chi tiết căn hộ mở bán
            { Permissions.RealStateMoBan_DSSP_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP_ChiTiet, Description = "Lịch sử" } },
            { Permissions.RealStateMoBan_DSSP_ChiTiet_LichSu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_DSSP_ChiTiet_LichSu, Description = "Danh sách" } },

            // Tab Chính sách mở bán
            { Permissions.RealStateMoBan_ChinhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChiTiet, Description = "Chính sách" } },
            { Permissions.RealStateMoBan_ChinhSach_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Danh sách" } },
            { Permissions.RealStateMoBan_ChinhSach_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Chi tiết" } },
            { Permissions.RealStateMoBan_ChinhSach_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Cập nhật" } },
            { Permissions.RealStateMoBan_ChinhSach_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Xuất PDF" } },
  
            // Tab mẫu biểu hợp đồng - Mở bán
            { Permissions.RealStateMoBan_MauBieu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChiTiet, Description = "Mẫu biểu" } },
            { Permissions.RealStateMoBan_MauBieu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Danh sách" } },
            { Permissions.RealStateMoBan_MauBieu_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Thêm mới" } },
            { Permissions.RealStateMoBan_MauBieu_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Chỉnh sửa" } },
            { Permissions.RealStateMoBan_MauBieu_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChinhSach, Description = "Đổi trạng thái" } },
  
            // Tab hồ sơ dự án - Mở bán
            { Permissions.RealStateMoBan_HoSo, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_ChiTiet, Description = "Hồ sơ" } },
            { Permissions.RealStateMoBan_HoSo_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Danh sách" } },
            { Permissions.RealStateMoBan_HoSo_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Thêm mới" } },
            { Permissions.RealStateMoBan_HoSo_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Chỉnh sửa" } },
            { Permissions.RealStateMoBan_HoSo_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Đổi trạng thái" } },
            { Permissions.RealStateMoBan_HoSo_XemFile, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Xem file" } },
            { Permissions.RealStateMoBan_HoSo_Tai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Tải file" } },
            { Permissions.RealStateMoBan_HoSo_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMoBan_HoSo, Description = "Xoá" } },
  
            // Menu Quản lý giao dịch cọc
            { Permissions.RealStateMenuQLGiaoDichCoc, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Quản lý giao dịch cọc" } },
            // Module Sổ lệnh
            { Permissions.RealStateMenuSoLenh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLGiaoDichCoc, Description = "Sổ lệnh" } },
            { Permissions.RealStateMenuSoLenh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh, Description = "Danh sách" } },
            { Permissions.RealStateMenuSoLenh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh, Description = "Thêm mới" } },
            { Permissions.RealStateMenuSoLenh_GiaHanGiuCho, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh, Description = "Gia hạn thời gian giữ chỗ" } },
            { Permissions.RealStateMenuSoLenh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh, Description = "Xoá" } },
            
            { Permissions.RealStateMenuSoLenh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLGiaoDichCoc, Description = "Thông tin sổ lệnh" } },
            // Tab thông tin chung
            { Permissions.RealStateMenuSoLenh_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ChiTiet, Description = "Thông tin chung" } },
            { Permissions.RealStateMenuSoLenh_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinChung, Description = "Chi tiết" } },
            { Permissions.RealStateMenuSoLenh_ThongTinChung_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinChung, Description = "Chỉnh sửa" } },
            { Permissions.RealStateMenuSoLenh_ThongTinChung_DoiLoaiHD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinChung, Description = "Đổi loại hợp đồng" } },
            { Permissions.RealStateMenuSoLenh_ThongTinChung_DoiHinhThucThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinChung, Description = "Đổi hình thức thanh toán" } },
            // Tab Thanh toán
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ChiTiet, Description = "Thanh toán" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Danh sách" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Thêm mới" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Chi tiết" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Chỉnh sửa" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Phê duyệt/ huỷ" } },
            { Permissions.RealStateMenuSoLenh_ThongTinThanhToan_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ThongTinThanhToan, Description = "Xoá" } },
            // Tab Chính sách ưu đãi
            { Permissions.RealStateMenuSoLenh_CSUuDai, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ChiTiet, Description = "Chính sách ưu đãi" } },
            { Permissions.RealStateMenuSoLenh_CSUuDai_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_CSUuDai, Description = "Danh sách" } },
            // Tab HSKH đăng ký
            { Permissions.RealStateMenuSoLenh_HSKHDangKy, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ChiTiet, Description = "HSKH đăng ký" } },
            { Permissions.RealStateMenuSoLenh_HSKHDangKy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_HSKHDangKy, Description = "Danh sách" } },
            { Permissions.RealStateMenuSoLenh_HSKHDangKy_ChuyenOnline, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_HSKHDangKy, Description = "Chuyển online" } },
            { Permissions.RealStateMenuSoLenh_HSKHDangKy_CapNhatHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_HSKHDangKy, Description = "Cập nhật hồ sơ" } },
            { Permissions.RealStateMenuSoLenh_HSKHDangKy_DuyetHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_HSKHDangKy, Description = "Duyệt hồ sơ" } },
            { Permissions.RealStateMenuSoLenh_HSKHDangKy_HuyDuyetHS, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_HSKHDangKy, Description = "Hủy duyệt hồ sơ" } },

            // Tab Lịch sử - Sổ lệnh
            { Permissions.RealStateMenuSoLenh_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_ChiTiet, Description = "Lịch sử" } },
            { Permissions.RealStateMenuSoLenh_LichSu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuSoLenh_LichSu, Description = "Danh sách" } },

            // Xử lý đặt cọc 
            { Permissions.RealStateGDC_XLDC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLGiaoDichCoc, Description = "Xử lý đặt cọc" } },
            { Permissions.RealStateGDC_XLDC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateGDC_XLDC, Description = "Danh sách" } },

            // Hợp đồng
            { Permissions.RealStateGDC_HDDC, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLGiaoDichCoc, Description = "Hợp đồng đặt cọc" } },
            { Permissions.RealStateGDC_HDDC_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateGDC_HDDC, Description = "Danh sách" } },

            // Menu Quản lý phê duyệt
            { Permissions.RealStateMenuQLPD, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Quản lý phê duyệt" } },
            // Phê duyệt dự án
            { Permissions.RealStateMenuPDDA, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLPD, Description = "Phê duyệt dự án" } },
            { Permissions.RealStatePDDA_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDDA, Description = "Danh sách" } },
            { Permissions.RealStatePDDA_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDDA, Description = "Phê duyệt" } },
            // Phê duyệt phân phối
            { Permissions.RealStateMenuPDPP, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLPD, Description = "Phê duyệt phân phối" } },
            { Permissions.RealStatePDPP_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDPP, Description = "Danh sách" } },
            { Permissions.RealStatePDPP_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDPP, Description = "Phê duyệt" } },
            // Phê duyệt mở bán
            { Permissions.RealStateMenuPDMB, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuQLPD, Description = "Phê duyệt mở bán" } },
            { Permissions.RealStatePDMB_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDMB, Description = "Danh sách" } },
            { Permissions.RealStatePDMB_PheDuyetOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealStateMenuPDMB, Description = "Phê duyệt" } },
            // Báo cáo
            { Permissions.RealState_Menu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.RealState, ParentKey = null, Description = "Báo cáo" } },

            { Permissions.RealState_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_Menu_BaoCao, Description = "Báo cáo quản trị" } },
            { Permissions.RealState_BaoCao_QuanTri_TQBangHangDuAn, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_QuanTri, Description = "B.C Tổng quan bảng hàng dự án" } },
            { Permissions.RealState_BaoCao_QuanTri_TH_TienVeDA, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_QuanTri, Description = "B.C Tổng hợp tiền về dự án" } },
            { Permissions.RealState_BaoCao_QuanTri_TH_CacKhoanGD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_QuanTri, Description = "B.C Tổng hợp các khoản giao dịch" } },

            { Permissions.RealState_BaoCao_VanHanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_Menu_BaoCao, Description = "Báo cáo vận hành" } },
            { Permissions.RealState_BaoCao_VanHanh_TQBangHangDuAn, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_VanHanh, Description = "B.C Tổng quan bảng hàng dự án" } },
            { Permissions.RealState_BaoCao_VanHanh_TH_TienVeDA, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_VanHanh, Description = "B.C Tổng hợp tiền về dự án" } },
            { Permissions.RealState_BaoCao_VanHanh_TH_CacKhoanGD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_VanHanh, Description = "B.C Tổng hợp các khoản giao dịch" } },

            { Permissions.RealState_BaoCao_KinhDoanh, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_Menu_BaoCao, Description = "Báo cáo kinh doanh" } },
            { Permissions.RealState_BaoCao_KinhDoanh_TQBangHangDuAn, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_KinhDoanh, Description = "B.C Tổng quan bảng hàng dự án" } },
            { Permissions.RealState_BaoCao_KinhDoanh_TH_TienVeDA, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_KinhDoanh, Description = "B.C Tổng hợp tiền về dự án" } },
            { Permissions.RealState_BaoCao_KinhDoanh_TH_CacKhoanGD, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.RealState, ParentKey = Permissions.RealState_BaoCao_KinhDoanh, Description = "B.C Tổng hợp các khoản giao dịch" } },

            #endregion

            #region trang loyalty
                { Permissions.LoyaltyWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "" } },
                // Khách hàng ------ Start
                { Permissions.LoyaltyMenu_QLKhachHang, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Quản lý khách hàng" } },
                { Permissions.LoyaltyMenu_KhachHangCaNhan, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Khách hàng cá nhân" } },
                { Permissions.Loyalty_KhachHangCaNhan_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Danh sách khách hàng cá nhân" } },
                { Permissions.Loyalty_KhachHangCaNhan_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Cập nhật" } },
                { Permissions.LoyaltyKhachHangCaNhan_XuatDuLieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Xuất dữ liệu" } },
                { Permissions.Loyalty_KhachHangCaNhan_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Chi tiết" } },
                // Tab thông tin chung
                { Permissions.Loyalty_KhachHangCaNhan_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Thông tin chung" } },
                { Permissions.Loyalty_KhachHangCaNhan_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_KhachHangCaNhan_ThongTinChung, Description = "Chi tiết" } },
                // Tab danh sach uu dai
                { Permissions.Loyalty_KhachHangCaNhan_UuDai, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Ưu đãi" } },
                { Permissions.Loyalty_KhachHangCaNhan_UuDai_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_KhachHangCaNhan_UuDai, Description = "Danh sách ưu đãi" } },
                { Permissions.Loyalty_KhachHangCaNhan_UuDai_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_KhachHangCaNhan_UuDai, Description = "Thêm mới danh sách" } },
                // Tab SuKienThamGia
                { Permissions.Loyalty_KhachHangCaNhan_SuKienThamGia, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Sự kiện tham gia" } },
                //Tab lịch sử tham gia
                { Permissions.Loyalty_KhachHangCaNhan_LichSuThamGia, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLKhachHang, Description = "Lịch sử tham gia" } },
                { Permissions.Loyalty_KhachHangCaNhan_LichSuThamGia_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_KhachHangCaNhan_LichSuThamGia, Description = "Danh sách" } },
                
                // Quản lý uu dai
                { Permissions.LoyaltyMenu_QLUuDai, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Quản lý ưu đãi" } },
                //DanhSachUuDai
                { Permissions.LoyaltyMenu_DanhSachUuDai, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLUuDai, Description = "Danh sách ưu đãi" } },
                { Permissions.Loyalty_DanhSachUuDai_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Danh sách" } },
                { Permissions.Loyalty_DanhSachUuDai_UploadVoucher, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Upload Voucher" } },
                { Permissions.Loyalty_DanhSachUuDai_DownloadMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Download mẫu" } },
                { Permissions.Loyalty_DanhSachUuDai_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Thêm mới" } },
                { Permissions.Loyalty_DanhSachUuDai_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Chi tiết" } },
                { Permissions.Loyalty_DanhSachUuDai_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Chỉnh sửa" } },
                { Permissions.Loyalty_DanhSachUuDai_DanhDauOrTatNoiBat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Đánh dấu/Tắt nổi bật" } },
                { Permissions.Loyalty_DanhSachUuDai_BatOrTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Bật/ Tắt Showapp" } },
                { Permissions.Loyalty_DanhSachUuDai_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Kích hoạt/Huỷ" } },
                { Permissions.Loyalty_DanhSachUuDai_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_DanhSachUuDai, Description = "Xoá" } },
                //LichSuCapPhat
                { Permissions.LoyaltyMenu_LichSuCapPhat, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLUuDai, Description = "Lịch sử cấp phát" } },
                { Permissions.Loyalty_LichSuCapPhat_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_LichSuCapPhat, Description = "Danh sách" } },

                // Quản lý diem
                { Permissions.LoyaltyMenu_QLDiem, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Quản lý điểm" } },
                //QL tich diem
                { Permissions.LoyaltyMenu_TichDiem, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLDiem, Description = "Quản lý tích điểm" } },
                { Permissions.LoyaltyMenu_TichDiem_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Thêm mới" } },
                { Permissions.LoyaltyMenu_TichDiem_UploadDSDiem, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Upload danh sách điểm" } },
                { Permissions.LoyaltyMenu_TichDiem_DownloadMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Download mẫu" } },
                { Permissions.LoyaltyMenu_TichDiem_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Danh sách" } },
                { Permissions.LoyaltyMenu_TichDiem_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Chi tiết" } },
                { Permissions.LoyaltyMenu_TichDiem_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Chỉnh sửa" } },
                { Permissions.LoyaltyMenu_TichDiem_Huy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TichDiem, Description = "Huỷ" } },
                // QLHangThanhVien
                { Permissions.LoyaltyMenu_HangThanhVien, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLDiem, Description = "Quản lý hạng thành viên" } },
                { Permissions.LoyaltyMenu_HangThanhVien_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HangThanhVien, Description = "Thêm mới" } },
                { Permissions.LoyaltyMenu_HangThanhVien_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HangThanhVien, Description = "Danh sách" } },
                { Permissions.LoyaltyMenu_HangThanhVien_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HangThanhVien, Description = "Chi tiết" } },
                { Permissions.LoyaltyMenu_HangThanhVien_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HangThanhVien, Description = "Chỉnh sửa" } },
                { Permissions.LoyaltyMenu_HangThanhVien_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HangThanhVien, Description = "Kích hoạt/Huỷ" } }, 
                
                // Quản lý yeu cau
                { Permissions.LoyaltyMenu_QLYeuCau, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Quản lý yêu cầu" } },
                //Yêu cầu đổi điểm
                { Permissions.LoyaltyMenu_YeuCauDoiVoucher, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLYeuCau, Description = "Yêu cầu đổi điểm" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Danh sách" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Thêm mới" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Chi tiết" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Chỉnh sửa" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_BanGiao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Bàn giao" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_HoanThanh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Hoàn thành" } },
                { Permissions.Loyalty_YeuCauDoiVoucher_HuyYeuCau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_YeuCauDoiVoucher, Description = "Huỷ yêu cầu" } },

                // Truyền thông 
                { Permissions.LoyaltyMenu_TruyenThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Truyền thông" } },
                // Truyền thông - Hình ảnh
                { Permissions.LoyaltyMenu_HinhAnh, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TruyenThong, Description = "Hình ảnh" } },
                { Permissions.LoyaltyHinhAnh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HinhAnh, Description = "Danh sách" } },
                { Permissions.LoyaltyHinhAnh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HinhAnh, Description = "Thêm mới" } },
                { Permissions.LoyaltyHinhAnh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HinhAnh, Description = "Xóa" } },
                { Permissions.LoyaltyHinhAnh_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HinhAnh, Description = "Cập nhật" } },
                { Permissions.LoyaltyHinhAnh_PheDuyetOrHuyDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_HinhAnh, Description = "Phê duyệt đăng" } },

                //Truyền thông -tin tức
                { Permissions.LoyaltyMenu_TinTuc, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TruyenThong, Description = "Tin tức" } },
                { Permissions.LoyaltyTinTuc_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TinTuc, Description = "Danh sách" } },
                { Permissions.LoyaltyTinTuc_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TinTuc, Description = "Thêm mới" } },
                { Permissions.LoyaltyTinTuc_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TinTuc, Description = "Xóa" } },
                { Permissions.LoyaltyTinTuc_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TinTuc, Description = "Cập nhật" } },
                { Permissions.LoyaltyTinTuc_PheDuyetOrHuyDang, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_TinTuc, Description = "Phê duyệt đăng" } },
                

                // Menu Thông báo -------Start
                { Permissions.LoyaltyMenu_ThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Thông báo" } },

                
                 // thong bao - Cấu hình thông báo hệ thống
               { Permissions.LoyaltyMenu_ThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_ThongBao, Description = "Cấu hình thông báo" } },
                { Permissions.LoyaltyThongBaoHeThong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_ThongBaoHeThong, Description = "Cập nhật cài đặt" } },

              
                // Thông báo - Mẫu thông báo
                { Permissions.LoyaltyMenu_MauThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_ThongBao, Description = "Mẫu thông báo" } },
                { Permissions.LoyaltyMauThongBao_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_MauThongBao, Description = "Danh sách" } },
                { Permissions.LoyaltyMauThongBao_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_MauThongBao, Description = "Thêm mới" } },
                { Permissions.LoyaltyMauThongBao_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_MauThongBao, Description = "Cập nhật" } },
                { Permissions.LoyaltyMauThongBao_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_MauThongBao, Description = "Xóa" } },
                { Permissions.LoyaltyMauThongBao_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_MauThongBao, Description = "Kích hoạt / Hủy kích hoạt" } },

                // Thông báo - Quản lý thông báo
                { Permissions.LoyaltyMenu_QLTB, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_ThongBao, Description = "Quản lý thông báo" } },
                { Permissions.LoyaltyQLTB_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLTB, Description = "Danh sách" } },
                { Permissions.LoyaltyQLTB_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLTB, Description = "Thêm mới" } },
                { Permissions.LoyaltyQLTB_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLTB, Description = "Xóa" } },
                { Permissions.LoyaltyQLTB_KichHoatOrHuy, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLTB, Description = "Kích hoạt / Hủy kích hoạt" } },
                { Permissions.LoyaltyQLTB_PageChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_QLTB, Description = "Thông tin chi tiết" } },

                // Chi tiết thông báo
                { Permissions.LoyaltyQLTB_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_PageChiTiet, Description = "Thông tin chung" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_ThongTin, new PermissionContent { PermissionType = PermissionTypes.Form, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_ThongTinChung, Description = "Xem chi tiết" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_ThongTinChung, Description = "Cập nhật" } },
    
                //Tab gửi thông báo
                { Permissions.LoyaltyQLTB_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_PageChiTiet, Description = "Gửi thông báo" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_GuiThongBao_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_GuiThongBao, Description = "Danh sách thông báo" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_GuiThongBao_CaiDat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_GuiThongBao, Description = "Cài đặt danh sách thông báo" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_GuiThongBao_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_GuiThongBao, Description = "Gửi thông báo" } },
                { Permissions.LoyaltyQLTB_PageChiTiet_GuiThongBao_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyQLTB_GuiThongBao, Description = "Xoá" } },

                // Báo cáo
                { Permissions.LoyaltyMenu_BaoCao, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Thông báo" } },
                { Permissions.Loyalty_BaoCao_QuanTri, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_BaoCao, Description = "Báo cáo quản trị" } },
                { Permissions.Loyalty_BaoCao_QuanTri_DSYeuCauDoiVoucher, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_BaoCao_QuanTri, Description = "BC Danh sách yêu cầu đổi voucher" } },
                { Permissions.Loyalty_BaoCao_QuanTri_XuatNhapTonVoucher, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.Loyalty_BaoCao_QuanTri, Description = "BC Xuất nhập tồn voucher" } },
            
                // Chương trình trúng thưởng
                { Permissions.LoyaltyMenu_CT_TrungThuong, new PermissionContent { PermissionType = PermissionTypes.Menu, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = null, Description = "Chương trình trúng thưởng" } },
                { Permissions.LoyaltyCT_TrungThuong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Danh sách" } },
                { Permissions.LoyaltyCT_TrungThuong_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Thêm mới" } },
                { Permissions.LoyaltyCT_TrungThuong_CaiDatThamGia, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Cài đặt tham gia" } },
                { Permissions.LoyaltyCT_TrungThuong_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Đổi trạng thái" } },
                { Permissions.LoyaltyCT_TrungThuong_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Xoá" } },
                { Permissions.LoyaltyCT_TrungThuong_PageChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyMenu_CT_TrungThuong, Description = "Thông tin chi tiết" } },
                { Permissions.LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet, Description = "Thông tin chương trình" } },
                { Permissions.LoyaltyCT_TrungThuong_ThongTinChuongTrinh_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh, Description = "Chỉnh sửa" } },
                { Permissions.LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet, Description = "Cấu hình chương trình" } },
                { Permissions.LoyaltyCT_TrungThuong_CauHinhChuongTrinh_ChinhSua, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh, Description = "Chỉnh sửa" } },
                { Permissions.LoyaltyCT_TrungThuong_PageChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet, Description = "Lịch sử" } },
                { Permissions.LoyaltyCT_TrungThuong_LichSu_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, PermissionInWeb = PermissionInWebs.Loyalty, ParentKey = Permissions.LoyaltyCT_TrungThuong_PageChiTiet_LichSu, Description = "Danh sách" } },

                #endregion

            #region trang event
                { Permissions.EventWeb, new PermissionContent { PermissionType = PermissionTypes.Web, PermissionInWeb = PermissionInWebs.Event, ParentKey = null, Description = "" } },

                // CÀI ĐẶT
                { Permissions.Event_Menu_CaiDat, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Cài đặt", ParentKey = null, PermissionInWeb = PermissionInWebs.Event } },
    
                // CẤU TRÚC MÃ HỢP ĐỒNG
                { Permissions.Event_Menu_CauTrucMaHD, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Cấu trúc mã hợp đồng", ParentKey = Permissions.Event_Menu_CaiDat, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_CauTrucMaHD_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_CauTrucMaHD, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_CauTrucMaHD_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_Menu_CauTrucMaHD, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_CauTrucMaHD_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_Menu_CauTrucMaHD, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_CauTrucMaHD_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_Menu_CauTrucMaHD, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_CauTrucMaHD_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái", ParentKey = Permissions.Event_Menu_CauTrucMaHD, PermissionInWeb = PermissionInWebs.Event } },

                // THÔNG BÁO HỆ THỐNG
                { Permissions.Event_Menu_ThongBaoHeThong, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Thông báo hệ thống", ParentKey = Permissions.Event_Menu_CaiDat, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_ThongBaoHeThong_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_ThongBaoHeThong, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_ThongBaoHeThong_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_Menu_ThongBaoHeThong, PermissionInWeb = PermissionInWebs.Event } },

                // QUẢN LÝ SỰ KIỆN
                { Permissions.Event_Menu_QuanLySuKien, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Quản lý sự kiện", ParentKey = null, PermissionInWeb = PermissionInWebs.Event } },

                // TỔNG QUAN SỰ KIỆN
                { Permissions.Event_Menu_TongQuanSuKien, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Tổng quan sự kiện", ParentKey = Permissions.Event_Menu_QuanLySuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_MoBanVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Mở bán vé", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_BatTatShowApp, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Bật tắt Show App", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_TamDung_HuySuKien, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tạm dừng/ Hủy sự kiện", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },

                // TỔNG QUAN SỰ KIỆN - CHI TIẾT
                { Permissions.Event_TongQuanSuKien_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết sự kiện", ParentKey = Permissions.Event_Menu_TongQuanSuKien, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Thông tin sự kiện", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - QUẢN LÝ
                { Permissions.Event_TongQuanSuKien_ChiTiet_QuanLy, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Quản lý", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_QuanLy_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_QuanLy, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_QuanLy_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_QuanLy, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - THÔNG TIN MÔ TẢ
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinMoTa, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin mô tả", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Chi tiết", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinMoTa, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThongTinMoTa, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - THỜI GIAN VÀ LOẠI VÉ
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thời gian và loại vé", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - HÌNH ẢNH SỰ KIỆN
                { Permissions.Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Hình ảnh sự kiện", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách hình ảnh", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - MẪU GIAO NHẬN VÉ
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Mẫu giao nhận vé", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem thử mẫu", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải mẫu vé", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe, PermissionInWeb = PermissionInWebs.Event } },

                // CHI TIẾT - MẪU VÉ
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Mẫu vé", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_XemThuMau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem thử mẫu", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_TaiMauVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải mẫu vé", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái", ParentKey = Permissions.Event_TongQuanSuKien_ChiTiet_MauVe, PermissionInWeb = PermissionInWebs.Event } },

                // QUẢN LÝ BÁN VÉ
                { Permissions.Event_Menu_QuanLyBanVe, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Quản lý bán vé", ParentKey = null, PermissionInWeb = PermissionInWebs.Event } }, 

                // SỔ LỆNH
                { Permissions.Event_Menu_SoLenh, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Sổ lệnh", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_SoLenh, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_Menu_SoLenh, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_GiaHanGiulenh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Gia hạn giữ lệnh", ParentKey = Permissions.Event_Menu_SoLenh, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_Xoa, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xóa", ParentKey = Permissions.Event_Menu_SoLenh, PermissionInWeb = PermissionInWebs.Event } }, 

                // SỔ LỆNH - CHI TIẾT
                { Permissions.Event_SoLenh_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Chi tiết", ParentKey = Permissions.Event_Menu_SoLenh, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_SoLenh_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_SoLenh_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết", ParentKey = Permissions.Event_SoLenh_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_SoLenh_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - GIAO DỊCH
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Giao dịch", ParentKey = Permissions.Event_SoLenh_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.SoLenh_ChiTiet_GiaoDich_GuiThongBao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Gửi thông báo", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Phê duyệt", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_GiaoDich_HuyThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Hủy thanh toán", ParentKey = Permissions.Event_SoLenh_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - DANH SÁCH VÉ
                { Permissions.Event_SoLenh_ChiTiet_DanhSachVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách vé", ParentKey = Permissions.Event_SoLenh_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_DanhSachVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách vé", ParentKey = Permissions.Event_SoLenh_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_DanhSachVe_TaiVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải vé", ParentKey = Permissions.Event_SoLenh_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_SoLenh_ChiTiet_DanhSachVe_XemVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem vé", ParentKey = Permissions.Event_SoLenh_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - LỊCH SỬ
                { Permissions.Event_SoLenh_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Lịch sử", ParentKey = Permissions.Event_SoLenh_ChiTiet, PermissionInWeb = PermissionInWebs.Event } }, 

                // XỬ LÝ GIAO DỊCH
                { Permissions.Event_Menu_XuLyGiaoDich, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Xử lý giao dịch", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_XuLyGiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_PheDuyetMuaVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Phê duyệt mua vé", ParentKey = Permissions.Event_Menu_XuLyGiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // XỬ LÝ GIAO DỊCH - CHI TIẾT
                { Permissions.Event_XuLyGiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Chi tiết", ParentKey = Permissions.Event_Menu_XuLyGiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_XuLyGiaoDich_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - GIAO DỊCH
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Giao dịch", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Phê duyệt", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Hủy thanh toán", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - DANH SÁCH VÉ
                { Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách vé", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách vé", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải vé", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem vé", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - LỊCH SỬ
                { Permissions.Event_XuLyGiaoDich_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Lịch sử", ParentKey = Permissions.Event_XuLyGiaoDich_ChiTiet, PermissionInWeb = PermissionInWebs.Event } }, 


                // VÉ BÁN HỢP LỆ
                { Permissions.Event_Menu_VeBanHopLe, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Vé bán hợp lệ", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_YeuCauHoaDon, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Yêu cầu hóa đơn", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_YeuCauVeCung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Yêu cầu vé cứng", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ThongBaoVeHopLe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thông báo vé hợp lệ", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_KhoaYeuCau, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Khóa yêu cầu", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } }, 

                // VÉ BÁN HỢP LỆ - CHI TIẾT
                { Permissions.Event_VeBanHopLe_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Chi tiết", ParentKey = Permissions.Event_Menu_VeBanHopLe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_VeBanHopLe_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi mã giới thiệu", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - GIAO DỊCH
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Giao dịch", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_ThemMoi, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Thêm mới", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Chi tiết", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_CapNhat, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Cập nhật", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_PheDuyet, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Phê duyệt", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Hủy thanh toán", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - DANH SÁCH VÉ
                { Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách vé", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách vé", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe_TaiVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải vé", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe_XemVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem vé", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - LỊCH SỬ
                { Permissions.Event_VeBanHopLe_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Lịch sử", ParentKey = Permissions.Event_VeBanHopLe_ChiTiet, PermissionInWeb = PermissionInWebs.Event } }, 

                // QUẢN LÝ THAM GIA
                { Permissions.Event_Menu_QuanLyThamGia, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Quản lý tham gia", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_QuanLyThamGia_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_QuanLyThamGia, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_QuanLyThamGia_XemVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xem vé", ParentKey = Permissions.Event_Menu_QuanLyThamGia, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_QuanLyThamGia_TaiVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Tải vé", ParentKey = Permissions.Event_Menu_QuanLyThamGia, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_QuanLyThamGia_XacNhanThamGia, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xác nhận tham gia", ParentKey = Permissions.Event_Menu_QuanLyThamGia, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_QuanLyThamGia_MoKhoaVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Mở / Khóa vé", ParentKey = Permissions.Event_Menu_QuanLyThamGia, PermissionInWeb = PermissionInWebs.Event } }, 

                // YÊU CẦU VÉ CỨNG
                { Permissions.Event_Menu_YeuCauVeCung, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Yêu cầu vé cứng", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_XuatMauGiaoVe, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xuất mẫu giao vé", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_XuatVeCung, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Xuất vé cứng", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_DoiTrangThai_DangGiao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái Đang giao", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_DoiTrangThai_HoanThanh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái Hoàn thành", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } }, 

                // YÊU CẦU VÉ CỨNG - CHI TIẾT
                { Permissions.Event_YeuCauVeCung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết vé", ParentKey = Permissions.Event_Menu_YeuCauVeCung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_YeuCauVeCung_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Thông tin chi tiết", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT -GIAO DỊCH
                { Permissions.Event_YeuCauVeCung_ChiTiet_GiaoDich, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Giao dịch", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_GiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết giao dịch", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - DANH SÁCH VÉ
                { Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách vé", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Tải vé", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe_XemVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Xem vé", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT- LỊCH SỬ
                { Permissions.Event_YeuCauVeCung_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Lịch sử", ParentKey = Permissions.Event_YeuCauVeCung_ChiTiet, PermissionInWeb = PermissionInWebs.Event } }, 

                // YÊU CẦU HÓA ĐƠN
                { Permissions.Event_Menu_YeuCauHoaDon, new PermissionContent { PermissionType = PermissionTypes.Menu, Description = "Yêu cầu hóa đơn", ParentKey = Permissions.Event_Menu_QuanLyBanVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Table, Description = "Danh sách", ParentKey = Permissions.Event_Menu_YeuCauHoaDon, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_DoiTrangThai_DangGiao, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái Đang giao", ParentKey = Permissions.Event_Menu_YeuCauHoaDon, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_DoiTrangThai_HoanThanh, new PermissionContent { PermissionType = PermissionTypes.ButtonAction, Description = "Đổi trạng thái Hoàn thành", ParentKey = Permissions.Event_Menu_YeuCauHoaDon, PermissionInWeb = PermissionInWebs.Event } }, 

                // YÊU CẦU HÓA ĐƠN - CHI TIẾT
                { Permissions.Event_YeuCauHoaDon_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Page, Description = "Chi tiết", ParentKey = Permissions.Event_Menu_YeuCauHoaDon, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - THÔNG TIN CHUNG
                { Permissions.Event_YeuCauHoaDon_ChiTiet_ThongTinChung, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin chung", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Thông tin vé", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_ThongTinChung, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - GIAO DỊCH
                { Permissions.Event_YeuCauHoaDon_ChiTiet_GiaoDich, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Giao dịch", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Chi tiết giao dịch", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_GiaoDich, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - DANH SÁCH VÉ
                { Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách vé", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Danh sách", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Tải vé", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } },
                { Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Xem vé", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet_DanhSachVe, PermissionInWeb = PermissionInWebs.Event } }, 

                // CHI TIẾT - LỊCH SỬ
                { Permissions.Event_YeuCauHoaDon_ChiTiet_LichSu, new PermissionContent { PermissionType = PermissionTypes.Tab, Description = "Lịch sử", ParentKey = Permissions.Event_YeuCauHoaDon_ChiTiet, PermissionInWeb = PermissionInWebs.Event } },

            #endregion

            #region files
            #endregion
        };
    }
}
