using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Identity
{
    public static class Permissions
    {
        //các loại permission
        private const string Web = "web_";
        private const string Menu = "menu_";
        private const string Tab = "tab_";
        private const string Page = "page_";
        private const string Table = "table_";
        private const string Form = "form_";
        private const string ButtonTable = "btn_table_";
        private const string ButtonForm = "btn_form_";
        private const string ButtonAction = "btn_action_";

        #region core
        private const string CoreModule = "core.";
        public const string CorePageDashboard = CoreModule + Page + "dashboard";
        public const string CoreWeb = CoreModule + Web;
        public const string CoreMenu = CoreModule + Menu;

        public const string CoreThongTinDoanhNghiep = CoreModule + Page + "thong_tin_doanh_nghiep";
        public const string CoreTTDN_ThongTinChung = CoreModule + Tab + "ttdn_thong_tin_chung";
        public const string CoreTTDN_CapNhat = CoreModule + ButtonAction + "ttdn_cap_nhat";
        public const string CoreTTDN_CauHinhChuKySo = CoreModule + ButtonAction + "ttdn_cau_hinh_chu_ki_so";
        //
        public const string CoreTTDN_TKNganHang = CoreModule + Tab + "ttdn_tk_ngan_hang";
        public const string CoreTTDN_TKNH_ThemMoi = CoreModule + ButtonAction + "ttdn_tknh_them_moi";
        public const string CoreTTDN_TKNH_SetDefault = CoreModule + ButtonAction + "ttdn_tknh_set_default";
        //
        public const string CoreTTDN_GiayPhepDKKD = CoreModule + Tab + "ttdn_giay_phep_dkkd";
        public const string CoreTTDN_GiayPhepDKKD_ThemMoi = CoreModule + ButtonAction + "ttdn_giay_phep_dkkd_them_moi";
        public const string CoreTTDN_GiayPhepDKKD_Sua = CoreModule + ButtonAction + "ttdn_giay_phep_dkkd_sua";
        public const string CoreTTDN_GiayPhepDKKD_Xoa = CoreModule + ButtonAction + "ttdn_giay_phep_dkkd_xoa";


        public const string Core_Menu_TK_UngDung = CoreModule + Menu + "tk_ung_dung";

        public const string CoreMenuInvestorAccount = CoreModule + Page + "investor_account";
        public const string CoreMenuInvestorAccount_ChiTiet = CoreModule + ButtonAction + "investor_account_chi_tiet";
        public const string CoreMenuInvestorAccount_ChangeStatus = CoreModule + ButtonAction + "investor_account_change_status";
        public const string CoreMenuInvestorAccount_ResetMatKhau = CoreModule + ButtonAction + "investor_account_reset_mat_khau";
        public const string CoreMenuInvestorAccount_DatMaPin = CoreModule + ButtonAction + "investor_account_dat_ma_pin";
        public const string CoreMenuInvestorAccount_XoaTaiKhoan = CoreModule + ButtonAction + "investor_account_xoa_tai_khoan";

        public const string Core_TK_ChuaXacMinh = CoreModule + Page + "tk_chua_xac_minh";
        public const string Core_TK_ChuaXacMinh_XacMinh = CoreModule + ButtonAction + "tk_chua_xac_minh_xac_minh";
        public const string Core_TK_ChuaXacMinh_ResetMatKhau = CoreModule + ButtonAction + "tk_chua_xac_minh_reset_mat_khau";
        public const string Core_TK_ChuaXacMinh_XoaTaiKhoan = CoreModule + ButtonAction + "tk_chua_xac_minh_xoa_tai_khoan";

        public const string CoreMenuKhachHang = CoreModule + Menu + "khach_hang";
        public const string CoreMenuDuyetKHCN = CoreModule + Menu + "duyet_khcn";
        public const string CoreDuyetKHCN_DanhSach = CoreModule + Table + "duyet_khcn_danh_sach";
        public const string CoreDuyetKHCN_ThemMoi = CoreModule + ButtonAction + "duyet_khcn_them_moi";
        //Chi tiết khách hàng
        public const string CoreDuyetKHCN_ThongTinKhachHang = CoreModule + Page + "duyet_khcn_thong_tin_khach_hang";
        public const string CoreDuyetKHCN_ThongTinChung = CoreModule + Tab + "duyet_khcn_thong_tin_chung";
        public const string CoreDuyetKHCN_ChiTiet = CoreModule + Form + "duyet_khcn_chi_tiet";
        public const string CoreDuyetKHCN_CapNhat = CoreModule + ButtonAction + "duyet_khcn_cap_nhat";
        public const string CoreDuyetKHCN_TrinhDuyet = CoreModule + ButtonAction + "duyet_khcn_trinh_duyet";
        public const string CoreDuyetKHCN_CheckFace = CoreModule + ButtonAction + "duyet_khcn_check_face";
        // tab TKNH = Tài khoản ngân hàng
        public const string CoreDuyetKHCN_TKNH = CoreModule + Tab + "duyet_khcn_tknh";
        public const string CoreDuyetKHCN_TKNH_DanhSach = CoreModule + Table + "duyet_khcn_tknh_danh_sach";
        public const string CoreDuyetKHCN_TKNH_ThemMoi = CoreModule + ButtonAction + "duyet_khcn_tknh_them_moi";
        public const string CoreDuyetKHCN_TKNH_SetDefault = CoreModule + ButtonAction + "duyet_khcn_tknh_set_default";
        public const string CoreDuyetKHCN_TKNH_Sua = CoreModule + ButtonAction + "duyet_khcn_tknh_sua";
        public const string CoreDuyetKHCN_TKNH_Xoa = CoreModule + ButtonAction + "duyet_khcn_tknh_xoa";
        // Tab quản lý giấy tờ
        public const string CoreDuyetKHCN_GiayTo = CoreModule + Tab + "duyet_khcn_giay_to";
        public const string CoreDuyetKHCN_GiayTo_DanhSach = CoreModule + Table + "duyet_khcn_giay_to_danh_sach";
        public const string CoreDuyetKHCN_GiayTo_ThemMoi = CoreModule + ButtonAction + "duyet_khcn_giay_to_them_moi";
        public const string CoreDuyetKHCN_GiayTo_CapNhat = CoreModule + ButtonAction + "duyet_khcn_giay_to_cap_nhat";
        public const string CoreDuyetKHCN_GiayTo_SetDefault = CoreModule + ButtonAction + "duyet_khcn_giay_to_set_default";
        // tab Quản lý địa chỉ liên hệ
        public const string CoreDuyetKHCN_DiaChi = CoreModule + Tab + "duyet_khcn_dia_chi";
        public const string CoreDuyetKHCN_DiaChi_DanhSach = CoreModule + Table + "duyet_khcn_dia_chi_danh_sach";
        public const string CoreDuyetKHCN_DiaChi_ThemMoi = CoreModule + ButtonAction + "duyet_khcn_dia_chi_them_moi";
        public const string CoreDuyetKHCN_DiaChi_SetDefault = CoreModule + ButtonAction + "duyet_khcn_dia_chi_set_default";
        // tab TKCK 
        public const string CoreDuyetKHCN_TKCK = CoreModule + Tab + "duyet_khcn_tkck";
        public const string CoreDuyetKHCN_TKCK_DanhSach = CoreModule + Table + "duyet_khcn_tkck_danh_sach";
        public const string CoreDuyetKHCN_TKCK_ThemMoi = CoreModule + ButtonAction + "duyet_khcn_tkck_them_moi";
        public const string CoreDuyetKHCN_TKCK_SetDefault = CoreModule + ButtonAction + "duyet_khcn_tkck_set_default";
        // Module Khách hàng cá nhân
        // khcn = KHCN = Khách hàng cá nhân; ds = danh sách; TKNH = Tài khoản ngân hàng
        public const string CoreMenuKHCN = CoreModule + Menu + "khcn";
        public const string CoreKHCN_DanhSach = CoreModule + Table + "khcn_danh_sach";
        public const string CoreKHCN_XacMinh = CoreModule + ButtonAction + "khcn_xac_minh";

        // Chi tiết khách hàng (Thông tin chung)
        public const string CoreKHCN_ThongTinKhachHang = CoreModule + Page + "khcn_thong_tin_khach_hang";
        public const string CoreKHCN_ThongTinChung = CoreModule + Tab + "khcn_thong_tin_chung";
        public const string CoreKHCN_ChiTiet = CoreModule + Form + "khcn_chi_tiet";
        public const string CoreKHCN_CapNhat = CoreModule + ButtonAction + "khcn_cap_nhat";
        public const string CoreKHCN_CheckFace = CoreModule + ButtonAction + "khcn_check_face";
        public const string CoreKHCN_Phone_TrinhDuyet = CoreModule + ButtonAction + "khcn_doi_sdt";
        public const string CoreKHCN_Email_TrinhDuyet = CoreModule + ButtonAction + "khcn_doi_email";
        // Tab tài khoản ngân hàng
        public const string CoreKHCN_TKNH = CoreModule + Tab + "khcn_tknh";
        public const string CoreKHCN_TKNH_DanhSach = CoreModule + Table + "khcn_tknh_danh_sach";
        public const string CoreKHCN_TKNH_ThemMoi = CoreModule + ButtonAction + "khcn_tknh_them_moi";
        public const string CoreKHCN_TKNH_SetDefault = CoreModule + ButtonAction + "khcn_tknh_set_default";
        public const string CoreKHCN_TKNH_Sua = CoreModule + ButtonAction + "khcn_tknh_sua";
        public const string CoreKHCN_TKNH_Xoa = CoreModule + ButtonAction + "khcn_tknh_xoa";
        // Tab tài khoản đăng nhập
        public const string CoreKHCN_Account = CoreModule + Tab + "khcn_account";
        public const string CoreKHCN_Account_DanhSach = CoreModule + Table + "khcn_account_danh_sach";
        public const string CoreKHCN_Account_ResetPassword = CoreModule + ButtonAction + "khcn_account_reset_password";
        public const string CoreKHCN_Account_ResetPin = CoreModule + ButtonAction + "khcn_account_reset_pin";
        public const string CoreKHCN_Account_ChangeStatus = CoreModule + ButtonAction + "khcn_account_change_status";
        public const string CoreKHCN_Account_Delete = CoreModule + ButtonAction + "khcn_account_delete";
        // Tab quản lý giấy tờ
        public const string CoreKHCN_GiayTo = CoreModule + Tab + "khcn_giay_to";
        public const string CoreKHCN_GiayTo_DanhSach = CoreModule + Table + "khcn_giay_to_danh_sach";
        public const string CoreKHCN_GiayTo_ThemMoi = CoreModule + ButtonAction + "khcn_giay_to_them_moi";
        public const string CoreKHCN_GiayTo_CapNhat = CoreModule + ButtonAction + "khcn_giay_to_cap_nhat";
        public const string CoreKHCN_GiayTo_SetDefault = CoreModule + ButtonAction + "khcn_giay_to_set_default";
        // tab Quản lý địa chỉ liên hệ
        public const string CoreKHCN_DiaChi = CoreModule + Tab + "khcn_dia_chi";
        public const string CoreKHCN_DiaChi_DanhSach = CoreModule + Table + "khcn_dia_chi_danh_sach";
        public const string CoreKHCN_DiaChi_ThemMoi = CoreModule + ButtonAction + "khcn_dia_chi_them_moi";
        public const string CoreKHCN_DiaChi_SetDefault = CoreModule + ButtonAction + "khcn_dia_chi_set_default";

        //
        // tab lịch sử chứng minh nhà đầu tư chuyên nghiệp ; NDTCN = Nhà Đầu Tư Chuyên Nghiệp
        public const string CoreKHCN_NDTCN = CoreModule + Tab + "khcn_ndtcn";
        public const string CoreKHCN_NDTCN_DanhSach = CoreModule + Table + "khcn_ndtcn_danh_sach";
        // tab quản lý tư vấn viên ; TVV = Tư vấn viên
        public const string CoreKHCN_TuVanVien = CoreModule + Tab + "khcn_tu_van_vien";
        public const string CoreKHCN_TuVanVien_DanhSach = CoreModule + Table + "khcn_tu_van_vien_danh_sach";
        public const string CoreKHCN_TuVanVien_ThemMoi = CoreModule + ButtonAction + "khcn_tu_van_vien_them_moi";
        public const string CoreKHCN_TuVanVien_SetDefault = CoreModule + ButtonAction + "khcn_tu_van_vien_set_default";
        // tab quản lý người giới thiệu
        public const string CoreKHCN_NguoiGioiThieu = CoreModule + Tab + "khcn_nguoi_gioi_thieu";
        public const string CoreKHCN_NguoiGioiThieu_DanhSach = CoreModule + Table + "khcn_nguoi_gioi_thieu_danh_sach";
        // Tab tài khoản chứng khoán
        public const string CoreKHCN_TKCK = CoreModule + Tab + "khcn_tkck";
        public const string CoreKHCN_TKCK_DanhSach = CoreModule + Table + "khcn_tkck_danh_sach";
        public const string CoreKHCN_TKCK_ThemMoi = CoreModule + ButtonAction + "khcn_tkck_them_moi";
        public const string CoreKHCN_TKCK_SetDefault = CoreModule + ButtonAction + "khcn_tkck_set_default";
        // Module Duyệt KHDN = Khách hàng doanh nghiệp
        public const string CoreMenuDuyetKHDN = CoreModule + Menu + "duyet_khdn";
        public const string CoreDuyetKHDN_DanhSach = CoreModule + Table + "duyet_khdn_danh_sach";
        public const string CoreDuyetKHDN_ThemMoi = CoreModule + ButtonAction + "duyet_khdn_them_moi";
        //
        public const string CoreDuyetKHDN_ThongTinKhachHang = CoreModule + Page + "duyet_khdn_thong_tin_khach_hang";
        public const string CoreDuyetKHDN_ThongTinChung = CoreModule + Tab + "duyet_khdn_thong_tin_chung";
        public const string CoreDuyetKHDN_ChiTiet = CoreModule + Form + "duyet_khdn_chi_tiet";
        public const string CoreDuyetKHDN_CapNhat = CoreModule + ButtonAction + "duyet_khdn_cap_nhat";
        public const string CoreDuyetKHDN_TrinhDuyet = CoreModule + ButtonAction + "duyet_khdn_trinh_duyet";

        // Tab tài khoản ngân hàng
        public const string CoreDuyetKHDN_TKNH = CoreModule + Tab + "duyet_khdn_tknh";
        public const string CoreDuyetKHDN_TKNH_DanhSach = CoreModule + Table + "duyet_khdn_tknh_danh_sach";
        public const string CoreDuyetKHDN_TKNH_ThemMoi = CoreModule + ButtonAction + "duyet_khdn_tknh_them_moi";
        public const string CoreDuyetKHDN_TKNH_SetDefault = CoreModule + ButtonAction + "duyet_khdn_tknh_set_default";

        // Tab tài khoản ngân hàng
        public const string CoreDuyetKHDN_CKS = CoreModule + Tab + "duyet_khdn_chu_ky_so";
        public const string CoreDuyetKHDN_CauHinhChuKySo = CoreModule + ButtonAction + "duyet_khdn_cau_hinh_chu_ky_so";

        // Tab Đăng ký kinh doanh
        public const string CoreDuyetKHDN_DKKD = CoreModule + Tab + "duyet_khdn_dkkd";
        public const string CoreDuyetKHDN_DKKD_DanhSach = CoreModule + Table + "duyet_khdn_dkkd_danh_sach";
        public const string CoreDuyetKHDN_DKKD_ThemMoi = CoreModule + ButtonAction + "duyet_khdn_dkkd_them_moi";
        public const string CoreDuyetKHDN_DKKD_XemFile = CoreModule + ButtonAction + "duyet_khdn_dkkd_xem_file";
        public const string CoreDuyetKHDN_DKKD_TaiFile = CoreModule + ButtonAction + "duyet_khdn_dkkd_tai_file";
        public const string CoreDuyetKHDN_DKKD_CapNhat = CoreModule + ButtonAction + "duyet_khdn_dkkd_cap_nhat";
        public const string CoreDuyetKHDN_DKKD_XoaFile = CoreModule + ButtonAction + "duyet_khdn_dkkd_xoa_file";

        //===========================

        // Quản lý khách hàng doanh nghiệp đã duyệt
        public const string CoreMenuKHDN = CoreModule + Menu + "khdn";
        public const string CoreKHDN_DanhSach = CoreModule + Table + "khdn_danh_sach";
        public const string CoreKHDN_XacMinh = CoreModule + ButtonAction + "khdn_xac_minh";

        // Thông tin chung
        public const string CoreKHDN_ThongTinKhachHang = CoreModule + Page + "khdn_thong_tin_khach_hang";
        public const string CoreKHDN_ThongTinChung = CoreModule + Tab + "khdn_thong_tin_chung";
        public const string CoreKHDN_ChiTiet = CoreModule + Form + "khdn_chi_tiet";
        public const string CoreKHDN_CapNhat = CoreModule + ButtonAction + "khdn_cap_nhat";

        // Tab tài khoản ngân hàng
        public const string CoreKHDN_TKNH = CoreModule + Tab + "khdn_tknh";
        public const string CoreKHDN_TKNH_DanhSach = CoreModule + Table + "khdn_tknh_danh_sach";
        public const string CoreKHDN_TKNH_ThemMoi = CoreModule + ButtonAction + "khdn_tknh_them_moi";
        public const string CoreKHDN_TKNH_SetDefault = CoreModule + ButtonAction + "khdn_tknh_set_default";

        // Tab tài khoản ngân hàng
        public const string CoreKHDN_CKS = CoreModule + Tab + "khdn_chu_ky_so";
        public const string CoreKHDN_CauHinhChuKySo = CoreModule + ButtonAction + "khdn_cau_hinh_chu_ky_so";

        // Tab Đăng ký kinh doanh
        public const string CoreKHDN_DKKD = CoreModule + Tab + "khdn_dkkd";
        public const string CoreKHDN_DKKD_DanhSach = CoreModule + Table + "khdn_dkkd_danh_sach";
        public const string CoreKHDN_DKKD_ThemMoi = CoreModule + ButtonAction + "khdn_dkkd_them_moi";
        public const string CoreKHDN_DKKD_XemFile = CoreModule + ButtonAction + "khdn_dkkd_xem_file";
        public const string CoreKHDN_DKKD_TaiFile = CoreModule + ButtonAction + "khdn_dkkd_tai_file";
        public const string CoreKHDN_DKKD_CapNhat = CoreModule + ButtonAction + "khdn_dkkd_cap_nhat";
        public const string CoreKHDN_DKKD_XoaFile = CoreModule + ButtonAction + "khdn_dkkd_xoa_file";

        // Quản lý Sale 
        public const string CoreMenuSale = CoreModule + Menu + "sale";

        // Danh sách Sale chưa duyệt
        public const string CoreMenuDuyetSale = CoreModule + Menu + "duyet_sale";
        public const string CoreDuyetSale_DanhSach = CoreModule + Table + "duyet_sale_danh_sach";
        public const string CoreDuyetSale_ThemMoi = CoreModule + ButtonAction + "duyet_sale_them_moi";
        public const string CoreDuyetSale_ThongTinSale = CoreModule + Page + "duyet_sale_thong_tin_sale";

        // Chi tiết sale chưa duyệt
        public const string CoreDuyetSale_ThongTinChung = CoreModule + Tab + "duyet_sale_thong_tin_chung";
        public const string CoreDuyetSale_ChiTiet = CoreModule + Form + "duyet_sale_chi_tiet";
        public const string CoreDuyetSale_CapNhat = CoreModule + ButtonAction + "duyet_sale_cap_nhat";
        public const string CoreDuyetSale_TrinhDuyet = CoreModule + ButtonAction + "duyet_sale_trinh_duyet";

        // Danh sách Sale đã duyệt
        public const string CoreMenuSaleActive = CoreModule + Menu + "sale_active";
        public const string CoreSaleActive_DanhSach = CoreModule + Table + "sale_active_danh_sach";
        public const string CoreSaleActive_KichHoat = CoreModule + ButtonAction + "sale_active_kich_hoat";
        public const string CoreSaleActive_ThongTinSale = CoreModule + Page + "sale_active_thong_tin_sale";

        // Chi tiết sale đã duyệt
        public const string CoreSaleActive_ThongTinChung = CoreModule + Tab + "sale_active_thong_tin_chung";
        public const string CoreSaleActive_ChiTiet = CoreModule + Form + "sale_active_chi_tiet";
        public const string CoreSaleActive_CapNhat = CoreModule + ButtonAction + "sale_active_cap_nhat";

        // Hợp đồng cộng tác = HDCT
        public const string CoreSaleActive_HDCT = CoreModule + Tab + "sale_active_hdct";
        public const string CoreSaleActive_HDCT_DanhSach = CoreModule + Table + "sale_active_hdct_danh_sach";
        public const string CoreSaleActive_HDCT_UpdateFile = CoreModule + ButtonAction + "sale_active_hdct_upload_ho_so";
        public const string CoreSaleActive_HDCT_Sign = CoreModule + ButtonAction + "sale_active_hdct_cap_nhat_ho_so";
        public const string CoreSaleActive_HDCT_UploadFile = CoreModule + ButtonAction + "sale_active_hdct_ky_dien_tu";
        //public const string CoreSaleActive_HDCT_Preview = CoreModule + ButtonAction + "sale_active_hdct_xem_ho_so";
        //public const string CoreSaleActive_HDCT_Download = CoreModule + ButtonAction + "sale_active_hdct_download_hop_dong";
        //public const string CoreSaleActive_HDCT_Download_Sign = CoreModule + ButtonAction + "sale_active_hdct_download_hop_dong_sign";

        // Danh sách Sale App 
        public const string CoreMenuSaleApp = CoreModule + Menu + "sale_app";
        public const string CoreSaleApp_DanhSach = CoreModule + Table + "sale_app_danh_sach";
        public const string CoreSaleApp_DieuHuong = CoreModule + ButtonAction + "sale_app_dieu_huong";

        // Mẫu Hợp đồng cộng tác = HDCT
        public const string CoreMenu_HDCT_Template = CoreModule + Menu + "hdct_template";
        public const string CoreHDCT_Template_DanhSach = CoreModule + Table + "hdct_template_danh_sach";
        public const string CoreHDCT_Template_ThemMoi = CoreModule + ButtonAction + "hdct_template_them_moi";
        public const string CoreHDCT_Template_CapNhat = CoreModule + ButtonAction + "hdct_template_cap_nhat";
        public const string CoreHDCT_Template_Xoa = CoreModule + ButtonAction + "hdct_template_xoa";
        public const string CoreHDCT_Template_Preview = CoreModule + ButtonAction + "hdct_template_preview";
        public const string CoreHDCT_Template_Download = CoreModule + ButtonAction + "hdct_template_download";

        // Quản lý đối tác = qldt
        public const string CoreMenu_QLDoiTac = CoreModule + Menu + "quan_ly_doi_tac";

        public const string CoreMenu_DoiTac = CoreModule + Menu + "doi_tac";
        public const string CoreDoiTac_DanhSach = CoreModule + Table + "doi_tac_danh_sach";
        public const string CoreDoiTac_ThemMoi = CoreModule + ButtonAction + "doi_tac_them_moi";
        public const string CoreDoiTac_Xoa = CoreModule + ButtonAction + "doi_tac_xoa";
        public const string CoreDoiTac_ThongTinChiTiet = CoreModule + Page + "doi_tac_thong_tin_chi_tiet";

        // Tab thông tin chung
        public const string CoreDoiTac_ThongTinChung = CoreModule + Tab + "doi_tac_thong_tin_chung";
        public const string CoreDoiTac_XemChiTiet = CoreModule + Form + "doi_tac_xem_chi_tiet";
        public const string CoreDoiTac_CapNhat = CoreModule + ButtonAction + "doi_tac_cap_nhat";

        //dai ly
        public const string CoreMenu_DaiLy = CoreModule + Menu + "dai_ly";
        public const string CoreDaiLy_DanhSach = CoreModule + Table + "dai_ly_danh_sach";
        public const string CoreDaiLy_ThemMoi = CoreModule + ButtonAction + "dai_ly_them_moi";
        public const string CoreDaiLy_ThongTinChiTiet = CoreModule + Page + "dai_ly_thong_tin_chi_tiet";

        // Tab thông tin chung
        public const string CoreDaiLy_ThongTinChung = CoreModule + Tab + "dai_ly_thong_tin_chung";
        public const string CoreDaiLy_XemChiTiet = CoreModule + Form + "dai_ly_xem_chi_tiet";

        // Tab quản lý tài khoản đăng nhập
        public const string CoreDoiTac_Account = CoreModule + Tab + "doi_tac_account";
        public const string CoreDoiTac_Account_DanhSach = CoreModule + Table + "doi_tac_account_danh_sach";
        public const string CoreDoiTac_Account_ThemMoi = CoreModule + ButtonAction + "doi_tac_account_them_moi";

        //Truyền thông
        public const string CoreMenu_TruyenThong = CoreModule + Menu + "truyen_thong";

        // Truyền thông - Tin Tức
        public const string CoreMenu_TinTuc = CoreModule + Menu + "tin_tuc";
        public const string CoreTinTuc_DanhSach = CoreModule + Table + "tin_tuc_danh_sach";
        public const string CoreTinTuc_ThemMoi = CoreModule + ButtonAction + "tin_tuc_them_moi";
        public const string CoreTinTuc_Xoa = CoreModule + ButtonAction + "tin_tuc_cap_nhat";
        public const string CoreTinTuc_CapNhat = CoreModule + ButtonAction + "tin_tuc_xoa";
        public const string CoreTinTuc_PheDuyetDang = CoreModule + ButtonAction + "tin_tuc_phe_duyet_dang";

        //Truyền thông -  Hình ảnh
        public const string CoreMenu_HinhAnh = CoreModule + Menu + "hinh_anh";
        public const string CoreHinhAnh_DanhSach = CoreModule + Table + "hinh_anh_danh_sach";
        public const string CoreHinhAnh_ThemMoi = CoreModule + ButtonAction + "hinh_anh_them_moi";
        public const string CoreHinhAnh_Xoa = CoreModule + ButtonAction + "hinh_anh_cap_nhat";
        public const string CoreHinhAnh_CapNhat = CoreModule + ButtonAction + "hinh_anh_xoa";
        public const string CoreHinhAnh_PheDuyetDang = CoreModule + ButtonAction + "hinh_anh_phe_duyet_dang";

        // Truyền thông - Kiến thức đầu tư
        public const string CoreMenu_KienThucDauTu = CoreModule + Menu + "kien_thuc_dau_tu";
        public const string CoreKienThucDauTu_DanhSach = CoreModule + Table + "kien_thuc_dau_tu_danh_sach";
        public const string CoreKienThucDauTu_ThemMoi = CoreModule + ButtonAction + "kien_thuc_dau_tu_them_moi";
        public const string CoreKienThucDauTu_CapNhat = CoreModule + ButtonAction + "kien_thuc_dau_tu_cap_nhat";
        public const string CoreKienThucDauTu_Xoa = CoreModule + ButtonAction + "kien_thuc_dau_tu_xoa";
        public const string CoreKienThucDauTu_PheDuyetDang = CoreModule + ButtonAction + "kien_thuc_dau_tu_phe_duyet_dang";

        //Truyền thông - Hòm thư góp ý
        public const string CoreMenu_HomThuGopY = CoreModule + Menu + "hom_thu_gop_y";

        // Menu Thông báo -------Start
        public const string CoreMenu_ThongBao = CoreModule + Menu + "thong_bao";

        //// Thông báo - Thông báo mặc định
        //public const string CoreMenu_ThongBaoMacDinh = CoreModule + Menu + "thong_bao_mac_dinh";
        //public const string CoreThongBaoMacDinh_CapNhat = CoreModule + Table + "thong_bao_mac_dinh_cap_nhat";

        // Thông báo - Cấu hình thông báo hệ thống
        //public const string CoreMenu_CauHinhThongBaoHeThong = CoreModule + Menu + "cau_hinh_thong_bao_he_thong";
        //public const string CoreCauHinhThongBaoHeThong_CapNhat = CoreModule + Menu + "cau_hinh_thong_bao_he_thong_cap_nhat";

        // Thông báo - Mẫu thông báo
        public const string CoreMenu_MauThongBao = CoreModule + Menu + "mau_thong_bao";
        public const string CoreMauThongBao_DanhSach = CoreModule + Table + "mau_thong_bao_danh_sach";
        public const string CoreMauThongBao_ThemMoi = CoreModule + ButtonAction + "mau_thong_bao_them_moi";
        public const string CoreMauThongBao_CapNhat = CoreModule + ButtonAction + "mau_thong_bao_cap_nhat";
        public const string CoreMauThongBao_Xoa = CoreModule + ButtonAction + "mau_thong_bao_xoa";
        public const string CoreMauThongBao_KichHoatOrHuy = CoreModule + ButtonAction + "mau_thong_bao_kich_hoat_hoac_huy";

        // Thông báo - Quản lý thông báo
        public const string CoreMenu_QLTB = CoreModule + Menu + "qltb";
        public const string CoreQLTB_DanhSach = CoreModule + Table + "qltb_danh_sach";
        public const string CoreQLTB_ThemMoi = CoreModule + ButtonAction + "qltb_them_moi";
        public const string CoreQLTB_Xoa = CoreModule + ButtonAction + "qltb_xoa";
        public const string CoreQLTB_KichHoatOrHuy = CoreModule + ButtonAction + "qltb_kich_hoat_hoac_huy";
        public const string CoreQLTB_PageChiTiet = CoreModule + Page + "qltb_page_chi_tiet";

        ////Thông báo - Cấu hình nhà cung cấp
        //public const string CoreMenu_CauHinhNCC = CoreModule + Menu + "cau_hinh_ncc";
        //public const string CoreCauHinhNCC_DanhSach = CoreModule + Table + "cau_hinh_ncc_danh_sach";
        //public const string CoreCauHinhNCC_ThemMoi = CoreModule + ButtonAction + "cau_hinh_ncc_them_moi";
        //public const string CoreCauHinhNCC_CapNhat = CoreModule + ButtonAction + "cau_hinh_ncc_cap_nhat";

        // Chi tiết thông báo
        public const string CoreQLTB_ThongTinChung = CoreModule + Tab + "qltb_page_chi_tiet_thong_tin_chung";
        public const string CoreQLTB_PageChiTiet_ThongTin = CoreModule + Form + "qltb_page_chi_tiet_thong_tin";
        public const string CoreQLTB_PageChiTiet_CapNhat = CoreModule + ButtonAction + "qltb_page_chi_tiet_cap_nhat";

        // Tab Gửi thông báo
        public const string CoreQLTB_GuiThongBao = CoreModule + Tab + "qltb_page_chi_tiet_gui_thong_bao";
        public const string CoreQLTB_PageChiTiet_GuiThongBao_DanhSach = CoreModule + Form + "qltb_page_chi_tiet_gui_thong_bao_danh_sach";
        public const string CoreQLTB_PageChiTiet_GuiThongBao_CaiDat = CoreModule + ButtonAction + "qltb_page_chi_tiet_gui_thong_bao_cai_dat";

        // Menu Thiết lập -------Start
        public const string CoreMenu_ThietLap = CoreModule + Menu + "thiet_lap";

        // Thiết lập - Thông báo mặc định
        public const string CoreMenu_ThongBaoMacDinh = CoreModule + Menu + "thong_bao_mac_dinh";
        public const string CoreThongBaoMacDinh_CapNhat = CoreModule + Table + "thong_bao_mac_dinh_cap_nhat";


        // Thông báo - Cấu hình thông báo hệ thống
        public const string CoreMenu_CauHinhThongBaoHeThong = CoreModule + Menu + "cau_hinh_thong_bao_he_thong";
        public const string CoreCauHinhThongBaoHeThong_CapNhat = CoreModule + Menu + "cau_hinh_thong_bao_he_thong_cap_nhat";

        //Thông báo - Cấu hình nhà cung cấp
        public const string CoreMenu_CauHinhNCC = CoreModule + Menu + "cau_hinh_ncc";
        public const string CoreCauHinhNCC_DanhSach = CoreModule + Table + "cau_hinh_ncc_danh_sach";
        public const string CoreCauHinhNCC_ThemMoi = CoreModule + ButtonAction + "cau_hinh_ncc_them_moi";
        public const string CoreCauHinhNCC_CapNhat = CoreModule + ButtonAction + "cau_hinh_ncc_cap_nhat";

        //Thông báo - Cấu hình chữ ký số
        public const string CoreMenu_CauHinhCKS = CoreModule + Menu + "cau_hinh_cks";
        public const string CoreCauHinhCKS_DanhSach = CoreModule + Table + "cau_hinh_cks_danh_sach";
        public const string CoreCauHinhCKS_CapNhat = CoreModule + ButtonAction + "cau_hinh_cks_cap_nhat";

        //Thiết lập - Cấu hình chữ ký số
        public const string CoreMenu_WhitelistIp = CoreModule + Menu + "whitelist_ip";
        public const string CoreWhitelistIp_DanhSach = CoreModule + Table + "whitelist_ip_danh_sach";
        public const string CoreWhitelistIp_ThemMoi = CoreModule + ButtonAction + "whitelist_ip_them_moi";
        public const string CoreWhitelistIp_ChiTiet = CoreModule + ButtonAction + "whitelist_ip_chi_tiet";
        public const string CoreWhitelistIp_CapNhat = CoreModule + ButtonAction + "whitelist_ip_cap_nhat";
        public const string CoreWhitelistIp_Xoa = CoreModule + ButtonAction + "whitelist_ip_xoa";

        //Thiết lập - Msb Prefix
        public const string CoreMenu_MsbPrefix = CoreModule + Menu + "msb_prefix";
        public const string CoreMsbPrefix_DanhSach = CoreModule + Table + "msb_prefix_danh_sach";
        public const string CoreMsbPrefix_ThemMoi = CoreModule + ButtonAction + "msb_prefix_them_moi";
        public const string CoreMsbPrefix_ChiTiet = CoreModule + ButtonAction + "msb_prefix_chi_tiet";
        public const string CoreMsbPrefix_CapNhat = CoreModule + ButtonAction + "msb_prefix_cap_nhat";
        public const string CoreMsbPrefix_Xoa = CoreModule + ButtonAction + "msb_prefix_xoa";

        //Thiết lập - Cau hinh he thong
        public const string CoreMenu_CauHinhHeThong = CoreModule + Menu + "cau_hinh_he_thong";
        public const string CoreCauHinhHeThong_DanhSach = CoreModule + Table + "cau_hinh_he_thong_danh_sach";
        public const string CoreCauHinhHeThong_ThemMoi = CoreModule + ButtonAction + "cau_hinh_he_thong_them_moi";
        public const string CoreCauHinhHeThong_ChiTiet = CoreModule + ButtonAction + "cau_hinh_he_thong_chi_tiet";
        public const string CoreCauHinhHeThong_CapNhat = CoreModule + ButtonAction + "cau_hinh_he_thong_cap_nhat";

        // thiet lap - cau hinh cuoc goi
        public const string CoreMenu_CauHinhCuocGoi = CoreModule + Menu + "cau_hinh_cuoc_goi";
        public const string CoreCauHinhCuocGoi_DanhSach = CoreModule + Table + "cau_hinh_cuoc_goi_danh_sach";
        public const string CoreCauHinhCuocGoi_CapNhat = CoreModule + ButtonAction + "cau_hinh_cuoc_goi_cap_nhat";

        // Quản lý phê duyệt = QLPD, Khách hàng cá nhân = KHCN, Khách hàng doanh nghiệp = KHDN, Nhà đầu tư chuyên nghiệp = NDTCN
        public const string CoreMenu_QLPD = CoreModule + Menu + "qlpd";

        // Phê duyệt khách hàng cá nhân
        public const string CoreQLPD_KHCN = CoreModule + Menu + "qlpd_khcn";
        public const string CoreQLPD_KHCN_DanhSach = CoreModule + Table + "qlpd_khcn_danh_sach";
        public const string CoreQLPD_KHCN_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_khcn_phe_duyet_or_huy";
        public const string CoreQLPD_KHCN_XemLichSu = CoreModule + ButtonAction + "qlpd_khcn_xem_lich_su";
        public const string CoreQLPD_KHCN_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_khcn_thong_tin_chi_tiet";

        // Phê duyệt khách hàng doanh nghiệp
        public const string CoreQLPD_KHDN = CoreModule + Menu + "qlpd_khdn";
        public const string CoreQLPD_KHDN_DanhSach = CoreModule + Table + "qlpd_khdn_danh_sach";
        public const string CoreQLPD_KHDN_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_khdn_phe_duyet_or_huy";
        public const string CoreQLPD_KHDN_XemLichSu = CoreModule + ButtonAction + "qlpd_khdn_xem_lich_su";
        public const string CoreQLPD_KHDN_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_khdn_thong_tin_chi_tiet";

        //  Phê duyệt nhà đầu tư chuyên nghiệp -----
        public const string CoreQLPD_NDTCN = CoreModule + Menu + "qlpd_ndtcn";
        public const string CoreQLPD_NDTCN_DanhSach = CoreModule + Table + "qlpd_ndtcn_danh_sach";
        public const string CoreQLPD_NDTCN_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_ndtcn_phe_duyet_or_huy";
        public const string CoreQLPD_NDTCN_XemLichSu = CoreModule + ButtonAction + "qlpd_ndtcn_xem_lich_su";
        public const string CoreQLPD_NDTCN_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_ndtcn_thong_tin_chi_tiet";

        //  Phê duyệt email -----
        public const string CoreQLPD_Email = CoreModule + Menu + "qlpd_email";
        public const string CoreQLPD_Email_DanhSach = CoreModule + Table + "qlpd_email_danh_sach";
        public const string CoreQLPD_Email_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_email_phe_duyet_or_huy";
        public const string CoreQLPD_Email_XemLichSu = CoreModule + ButtonAction + "qlpd_email_xem_lich_su";
        public const string CoreQLPD_Email_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_email_thong_tin_chi_tiet";

        //  Phê duyệt số điện thoại -----
        public const string CoreQLPD_Phone = CoreModule + Menu + "qlpd_phone";
        public const string CoreQLPD_Phone_DanhSach = CoreModule + Table + "qlpd_phone_danh_sach";
        public const string CoreQLPD_Phone_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_phone_phe_duyet_or_huy";
        public const string CoreQLPD_Phone_XemLichSu = CoreModule + ButtonAction + "qlpd_phone_xem_lich_su";
        public const string CoreQLPD_Phone_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_phone_thong_tin_chi_tiet";

        // Phê duyệt Sale ----
        public const string CoreQLPD_Sale = CoreModule + Menu + "qlpd_sale";
        public const string CoreQLPD_Sale_DanhSach = CoreModule + Table + "qlpd_sale_danh_sach";
        public const string CoreQLPD_Sale_PheDuyetOrHuy = CoreModule + ButtonAction + "qlpd_sale_phe_duyet_or_huy";
        public const string CoreQLPD_Sale_XemLichSu = CoreModule + ButtonAction + "qlpd_sale_xem_lich_su";
        public const string CoreQLPD_Sale_ThongTinChiTiet = CoreModule + ButtonAction + "qlpd_sale_thong_tin_chi_tiet";

        //Quản lý phòng ban
        public const string CoreMenu_PhongBan = CoreModule + Menu + "phong_ban";
        public const string CorePhongBan_DanhSach = CoreModule + Table + "phong_ban_danh_sach";
        public const string CorePhongBan_ThemMoi = CoreModule + ButtonAction + "phong_ban_them_moi";
        public const string CorePhongBan_ThemQuanLy = CoreModule + ButtonAction + "phong_ban_them_quan_ly";
        public const string CorePhongBan_ThemQuanLyDoanhNghiep = CoreModule + ButtonAction + "phong_ban_them_quan_ly_doanh_nghiep";
        public const string CorePhongBan_XoaQuanLy = CoreModule + ButtonAction + "phong_ban_xoa_quan_ly";
        public const string CorePhongBan_XoaQuanLyDoanhNghiep = CoreModule + ButtonAction + "phong_ban_xoa_quan_ly_doanh_nghiep";
        public const string CorePhongBan_CapNhat = CoreModule + ButtonAction + "phong_ban_cap_nhat";
        public const string CorePhongBan_Xoa = CoreModule + ButtonAction + "phong_ban_xoa";

        // Báo cáo
        public const string Core_Menu_BaoCao = CoreModule + Menu + "bao_cao";

        public const string Core_BaoCao_QuanTri = CoreModule + Page + "bao_cao_quan_tri";
        public const string Core_BaoCao_QuanTri_DSSaler = CoreModule + ButtonAction + "bao_cao_quan_tri_ds_saler";
        public const string Core_BaoCao_QuanTri_DSKhachHang = CoreModule + ButtonAction + "bao_cao_quan_tri_ds_khach_hang";
        public const string Core_BaoCao_QuanTri_DSKhachHangRoot = CoreModule + ButtonAction + "bao_cao_quan_tri_ds_khach_hang_root";
        public const string Core_BaoCao_QuanTri_DSNguoiDung = CoreModule + ButtonAction + "bao_cao_quan_tri_ds_nguoi_dung";
        public const string Core_BaoCao_QuanTri_DSKhachHangHVF = CoreModule + ButtonAction + "bao_cao_quan_tri_ds_khach_hang_hvf";
        public const string Core_BaoCao_QuanTri_SKTKNhaDauTu = CoreModule + ButtonAction + "bao_cao_quan_tri_sktk_nha_dau_tu";
        public const string Core_BaoCao_QuanTri_TDTTKhachHang = CoreModule + ButtonAction + "bao_cao_quan_tri_thay_doi_tt_khach_hang";
        public const string Core_BaoCao_QuanTri_TDTTKhachHangRoot = CoreModule + ButtonAction + "bao_cao_quan_tri_tdtt_khach_hang_root";

        public const string Core_BaoCao_VanHanh = CoreModule + Page + "bao_cao_van_hanh";
        public const string Core_BaoCao_VanHanh_DSSaler = CoreModule + ButtonAction + "bao_cao_van_hanh_ds_saler";
        public const string Core_BaoCao_VanHanh_DSKhachHang = CoreModule + ButtonAction + "bao_cao_van_hanh_ds_khach_hang";
        public const string Core_BaoCao_VanHanh_DSKhachHangRoot = CoreModule + ButtonAction + "bao_cao_van_hanh_ds_khach_hang_root";
        public const string Core_BaoCao_VanHanh_DSNguoiDung = CoreModule + ButtonAction + "bao_cao_van_hanh_ds_nguoi_dung";
        public const string Core_BaoCao_VanHanh_TDTTKhachHang = CoreModule + ButtonAction + "bao_cao_van_hanh_thay_doi_tt_khach_hang";
        public const string Core_BaoCao_VanHanh_TDTTKhachHangRoot = CoreModule + ButtonAction + "bao_cao_van_hanh_tdtt_khach_hang_root";

        public const string Core_BaoCao_KinhDoanh = CoreModule + Page + "bao_cao_kinh_doanh";
        public const string Core_BaoCao_KinhDoanh_DSSaler = CoreModule + ButtonAction + "bao_cao_kinh_doanh_ds_saler";
        public const string Core_BaoCao_KinhDoanh_DSKhachHang = CoreModule + ButtonAction + "bao_cao_kinh_doanh_ds_khach_hang";
        public const string Core_BaoCao_KinhDoanh_DSKhachHangRoot = CoreModule + ButtonAction + "bao_cao_kinh_doanh_ds_khach_hang_root";
        public const string Core_BaoCao_KinhDoanh_DSNguoiDung = CoreModule + ButtonAction + "bao_cao_kinh_doanh_ds_nguoi_dung";

        public const string Core_BaoCao_HeThong = CoreModule + Page + "bao_cao_he_thong";
        public const string Core_BaoCao_HeThong_DSKhachHangRoot = CoreModule + ButtonAction + "bao_cao_he_thong_ds_khach_hang_root";
        public const string Core_BaoCao_HeThong_TDTTKhachHangRoot = CoreModule + ButtonAction + "bao_cao_he_thong_tdtt_khach_hang_root";
        #endregion

        #region Bond
        // bond
        private const string BondModule = "bond.";
        public const string BondWeb = BondModule + Web;
        // 
        public const string BondPageDashboard = BondModule + Page + "dashboard";
        // Cai dat    
        public const string BondMenuCaiDat = BondModule + Menu + "cai_dat";
        // CHNNL: cấu hình ngày nghỉ lễ
        public const string BondMenuCaiDat_CHNNL = BondModule + Menu + "caidat_chnnl";
        public const string BondCaiDat_CHNNL_DanhSach = BondModule + Table + "caidat_chnnl_danh_sach";
        public const string BondCaiDat_CHNNL_CapNhat = BondModule + ButtonAction + "caidat_chnnl_cap_nhat";
        // Cài đặt -> Tổ chức phát hành
        // TCPH: Tổ chức phát hành
        public const string BondMenuCaiDat_TCPH = BondModule + Menu + "caidat_tcph";
        public const string BondCaiDat_TCPH_DanhSach = BondModule + Table + "caidat_tcph_danh_sach";
        public const string BondCaiDat_TCPH_ThemMoi = BondModule + ButtonAction + "caidat_tcph_them_moi";

        // TCPH - tab thông tin chi tiết
        public const string Bond_TCPH_ThongTinChiTiet = BondModule + Page + "tcph_thong_tin_chi_tiet";
        public const string Bond_TCPH_ThongTinChung = BondModule + Tab + "tcph_thong_tin_chung";
        public const string Bond_TCPH_ChiTiet = BondModule + Form + "tcph_chi_tiet";
        public const string Bond_TCPH_CapNhat = BondModule + ButtonAction + "tcph_cap_nhat";

        public const string Bond_TCPH_Xoa = BondModule + ButtonAction + "tcph_xoa";

        // Cài đặt -> Đại lý sơ cấp
        // DLSC: Đại lý sơ cấp
        public const string BondMenuCaiDat_DLSC = BondModule + Menu + "caidat_dlsc";
        public const string BondCaiDat_DLSC_DanhSach = BondModule + Table + "caidat_dlsc_danh_sach";
        public const string BondCaiDat_DLSC_ThemMoi = BondModule + ButtonAction + "caidat_dlsc_them_moi";

        // DLSC - tab thông tin chi tiết
        // TTC: Thông tin chung
        public const string Bond_DLSC_ThongTinChiTiet = BondModule + Page + "dlsc_thong_tin_chi_tiet";
        public const string Bond_DLSC_ThongTinChung = BondModule + Tab + "dlsc_thong_tin_chung";
        public const string Bond_DLSC_TTC_ChiTiet = BondModule + Form + "dlsc_ttc_chi_tiet";

        // TKDN: Tài khoản đăng nhập
        public const string Bond_DLSC_TaiKhoanDangNhap = BondModule + Tab + "dlsc_tkdn";
        public const string Bond_DLSC_TKDN_DanhSach = BondModule + Table + "dlsc_tkdn_danh_sach";
        public const string Bond_DLSC_TKDN_ThemMoi = BondModule + ButtonAction + "dlsc_tkdn_them_moi";

        // Cài đặt -> Đại lý lưu ký
        // DLLK: Đại lý lưu ký
        public const string BondMenuCaiDat_DLLK = BondModule + Menu + "caidat_dllk";
        public const string BondCaiDat_DLLK_DanhSach = BondModule + Table + "caidat_dllk_danh_sach";
        public const string BondCaiDat_DLLK_ThemMoi = BondModule + ButtonAction + "caidat_dllk_them_moi";

        // TCPH - tab thông tin chi tiết đại lý lưu ký
        public const string Bond_DLLK_ThongTinChiTiet = BondModule + Page + "dllk_thong_tin_chi_tiet";
        public const string Bond_DLLK_ThongTinChung = BondModule + Tab + "dllk_thong_tin_chung";
        public const string Bond_DLLK_ChiTiet = BondModule + Form + "dllk_chi_tiet";
        public const string Bond_DLLK_Xoa = BondModule + ButtonAction + "dllk_xoa";

        // Cài đặt -> Chính sách mẫu
        // CSM: Chính sách mẫu
        public const string BondMenuCaiDat_CSM = BondModule + Menu + "caidat_csm";
        public const string BondCaiDat_CSM_DanhSach = BondModule + Table + "caidat_csm_danh_sach";
        public const string BondCaiDat_CSM_ThemChinhSach = BondModule + ButtonAction + "caidat_csm_them_chinh_sach";
        public const string BondCaiDat_CSM_CapNhatChinhSach = BondModule + ButtonAction + "caidat_csm_cap_nhat_chinh_sach";
        public const string BondCaiDat_CSM_XoaChinhSach = BondModule + ButtonAction + "caidat_csm_xoa_chinh_sach";
        public const string BondCaiDat_CSM_KichHoatOrHuy = BondModule + ButtonAction + "caidat_csm_kich_hoat_or_huy";
        public const string BondCaiDat_CSM_ThemKyHan = BondModule + ButtonAction + "caidat_csm_them_ky_han";
        public const string BondCaiDat_CSM_CapNhatKyHan = BondModule + ButtonAction + "caidat_csm_cap_nhat_ky_han";
        public const string BondCaiDat_CSM_XoaKyHan = BondModule + ButtonAction + "caidat_csm_xoa_ky_han";
        public const string BondCaiDat_CSM_KyHan_KichHoatOrHuy = BondModule + ButtonAction + "caidat_csm_ky_han_kich_hoat_or_huy";

        // Cài đặt -> Mẫu thông báo
        // MTB: Mẫu thông báo
        public const string BondMenuCaiDat_MTB = BondModule + Menu + "caidat_mtb";
        public const string BondCaiDatMTB_CapNhat = BondModule + Form + "caidat_mtb_cap_nhat";
        // Cài đặt -> Hình ảnh
        public const string BondMenuCaiDat_HinhAnh = BondModule + Menu + "caidat_hinhanh";


        public const string BondCaiDat_HinhAnh_DanhSach = BondModule + Table + "caidat_hinhanh_danh_sach";
        public const string BondCaiDat_HinhAnh_ThemMoi = BondModule + ButtonAction + "caidat_hinhanh_them_moi";
        public const string BondCaiDat_HinhAnh_Sua = BondModule + ButtonAction + "caidat_hinhanh_sua";
        public const string BondCaiDat_HinhAnh_Xoa = BondModule + ButtonAction + "caidat_hinhanh_xoa";
        public const string BondCaiDat_HinhAnh_DuyetDang = BondModule + ButtonAction + "caidat_hinhanh_duyet_dang";

        // Quản lý trái phiếu
        // QLTP: Quản lý trái phiếu
        public const string BondMenuQLTP = BondModule + Menu + "qltp";

        // QLTP -> Lô trái phiếu
        // LTP: Lô trái phiếu
        // TTCT: Thông tin chi tiết
        public const string BondMenuQLTP_LTP = BondModule + Menu + "qltp_ltp";
        public const string BondMenuQLTP_LTP_DanhSach = BondModule + Table + "qltp_ltp_danh_sach";
        public const string BondMenuQLTP_LTP_ThemMoi = BondModule + ButtonAction + "qltp_ltp_them_moi";
        public const string BondMenuQLTP_LTP_TrinhDuyet = BondModule + ButtonAction + "qltp_ltp_dong_trinh_duyet";
        public const string BondMenuQLTP_LTP_TTCT = BondModule + Page + "qltp_ltp_thong_tin_chi_tiet";
        public const string BondMenuQLTP_LTP_PheDuyet = BondModule + ButtonAction + "qltp_ltp_phe_duyet";
        public const string BondMenuQLTP_LTP_DongTraiPhieu = BondModule + ButtonAction + "qltp_ltp_dong_trai_phieu";
        public const string BondMenuQLTP_LTP_Xoa = BondModule + ButtonAction + "qltp_ltp_dong_xoa";
        public const string BondMenuQLTP_LTP_EpicXacMinh = BondModule + ButtonAction + "qltp_ltp_epic_phe_duyet";

        // Thông tin chi tiết lô trái phiếu
        // LTP: Lô trái phiếu
        // TTC: Thông tin chung
        // TSDB: Tài sản đảm bảo, HSPL: Hồ sơ pháp lý, TTTT: Thông tin trái tức
        public const string Bond_LTP_TTC = BondModule + Tab + "ltp_thong_tin_chung";
        public const string Bond_LTP_TTC_ChiTiet = BondModule + Form + "ltp_ttc_chi_tiet";
        public const string Bond_LTP_TTC_CapNhat = BondModule + Form + "ltp_ttc_cap_nhat";
        //public const string Bond_LTP_MoTa = BondModule + Tab + "ltp_mo_ta";
        public const string Bond_LTP_TSDB = BondModule + Tab + "ltp_tsdb";
        public const string Bond_LTP_TSDB_DanhSach = BondModule + Table + "ltp_ttdb_danh_sach";
        public const string Bond_LTP_TSDB_Them = BondModule + ButtonAction + "ltp_ttdb_them";
        public const string Bond_LTP_TSDB_Sua = BondModule + ButtonAction + "ltp_ttdb_sua";
        public const string Bond_LTP_TSDB_Xoa = BondModule + ButtonAction + "ltp_ttdb_xoa";
        public const string Bond_LTP_TSDB_TaiXuong = BondModule + ButtonAction + "ltp_ttdb_tai_xuong";

        public const string Bond_LTP_HSPL = BondModule + Tab + "ltp_hspl";
        public const string Bond_LTP_HSPL_DanhSach = BondModule + Table + "ltp_hsbl_danh_sach";
        public const string Bond_LTP_HSPL_ThemMoi = BondModule + ButtonAction + "ltp_hspl_them_moi";
        public const string Bond_LTP_HSPL_XemHoSo = BondModule + ButtonAction + "ltp_hspl_xem_ho_so";
        public const string Bond_LTP_HSPL_Download = BondModule + ButtonAction + "ltp_hspl_download";
        public const string Bond_LTP_HSPL_Xoa = BondModule + ButtonAction + "ltp_hspl_xoa";

        public const string Bond_LTP_TTTT = BondModule + Tab + "ltp_tttt";
        public const string Bond_LTP_TTTT_DanhSach = BondModule + Table + "ltp_tttt_danh_sach";

        // QLTP -> Phát hành sơ cấp
        // PHSC: Phát hành sơ cấp
        public const string BondMenuQLTP_PHSC = BondModule + Menu + "qltp_phsc";
        public const string BondMenuQLTP_PHSC_DanhSach = BondModule + Table + "qltp_phsc_danh_sach";
        public const string BondMenuQLTP_PHSC_ThemMoi = BondModule + ButtonAction + "qltp_phsc_them_moi";
        public const string BondMenuQLTP_PHSC_TrinhDuyet = BondModule + ButtonAction + "qltp_phsc_trinh_duyet";
        public const string BondMenuQLTP_PHSC_PheDuyetOrHuy = BondModule + ButtonAction + "qltp_phsc_phe_duyet_or_huy";
        public const string BondMenuQLTP_PHSC_Xoa = BondModule + ButtonAction + "qltp_phsc_xoa";

        public const string BondMenuQLTP_PHSC_TTCT = BondModule + Page + "qltp_phsc_thong_tin_chi_tiet";
        public const string Bond_PHSC_TTC = BondModule + Tab + "phsc_thong_tin_chung";
        public const string Bond_PHSC_TTC_ChiTiet = BondModule + Form + "phsc_ttc_chi_tiet";
        public const string Bond_PHSC_TTC_ChinhSua = BondModule + ButtonAction + "phsc_ttc_chinh_sua";

        //public const string Bond_PHSC_CSL = BondModule + Tab + "phsc_chinh_sach_lai";

        // QLTP -> Hợp đồng phân phối
        // HDPP: Hợp đồng phân phối
        public const string BondMenuQLTP_HDPP = BondModule + Menu + "qltp_hdpp";
        public const string BondMenuQLTP_HDPP_DanhSach = BondModule + Table + "qltp_hdpp_danh_sach";
        public const string BondMenuQLTP_HDPP_ThemMoi = BondModule + ButtonAction + "qltp_hdpp_them_moi";
        public const string BondMenuQLTP_HDPP_Xoa = BondModule + ButtonAction + "qltp_hdpp_xoa";

        public const string BondMenuQLTP_HDPP_TTCT = BondModule + Page + "qltp_hdpp_thong_tin_chi_tiet";
        public const string Bond_HDPP_TTC = BondModule + Tab + "hdpp_thong_tin_chung";
        public const string Bond_HDPP_TTC_ChiTiet = BondModule + Form + "hdpp_ttc_chi_tiet";

        // Tab thông tin thanh toán = tttt
        public const string Bond_HDPP_TTThanhToan = BondModule + Tab + "hdpp_thong_tin_thanh_toan";
        public const string Bond_HDPP_TTTT_DanhSach = BondModule + Table + "hdpp_tttt_danh_sach";
        public const string Bond_HDPP_TTTT_ThemMoi = BondModule + ButtonAction + "hdpp_tttt_them_moi";
        public const string Bond_HDPP_TTTT_CapNhat = BondModule + ButtonAction + "hdpp_tttt_cap_nhat";
        public const string Bond_HDPP_TTTT_Xoa = BondModule + ButtonAction + "hdpp_tttt_xoa";
        public const string Bond_HDPP_TTTT_ChiTietThanhToan = BondModule + Form + "hdpp_tttt_chi_tiet_thanh_toan";
        public const string Bond_HDPP_TTTT_PheDuyet = BondModule + ButtonAction + "hdpp_tttt_phe_duyet";
        public const string Bond_HDPP_TTTT_HuyPheDuyet = BondModule + ButtonAction + "hdpp_tttt_huy_phe_duyet";

        public const string Bond_HDPP_DMHSKHK = BondModule + Tab + "hdpp_danh_muc_ho_so_hang_ky";
        public const string Bond_HDPP_DMHSKHK_DanhSach = BondModule + Table + "hdpp_danh_muc_ho_so_hang_ky_danh_sach";
        public const string Bond_HDPP_DMHSKHK_ThemMoi = BondModule + ButtonAction + "hdpp_danh_muc_ho_so_hang_ky_them_moi";
        public const string Bond_HDPP_DMHSKHK_PheDuyet = BondModule + ButtonAction + "hdpp_danh_muc_ho_so_hang_ky_phe_duyet";
        public const string Bond_HDPP_DMHSKHK_HuyPheDuyet = BondModule + ButtonAction + "hdpp_danh_muc_ho_so_hang_ky_huy_phe_duyet";
        public const string Bond_HDPP_DMHSKHK_Xoa = BondModule + ButtonAction + "hdpp_danh_muc_ho_so_hang_ky_xoa";

        public const string Bond_HDPP_TTTraiTuc = BondModule + Tab + "hdpp_thong_tin_trai_tuc";
        public const string Bond_HDPP_TTTraiTuc_DanhSach = BondModule + Table + "hdpp_thong_tin_trai_tuc_danh_sach";

        // QLTP -> Bán theo kỳ hạn
        // BTKH: Bán theo kỳ hạn
        public const string BondMenuQLTP_BTKH = BondModule + Menu + "qltp_btkh";
        public const string BondMenuQLTP_BTKH_DanhSach = BondModule + Table + "qltp_btkh_danh_sach";
        public const string BondMenuQLTP_BTKH_ThemMoi = BondModule + ButtonAction + "qltp_btkh_them_moi";
        public const string BondMenuQLTP_BTKH_TrinhDuyet = BondModule + ButtonAction + "qltp_btkh_trinh_duyet";
        public const string BondMenuQLTP_BTKH_DongTam = BondModule + ButtonAction + "qltp_btkh_dong_tam";
        public const string BondMenuQLTP_BTKH_BatShowApp = BondModule + ButtonAction + "qltp_btkh_bat_tat_show_app";
        public const string BondMenuQLTP_BTKH_EpicXacMinh = BondModule + ButtonAction + "qltp_btkh_epic_phe_duyet";
        public const string BondMenuQLTP_BTKH_ThongTinChiTiet = BondModule + Page + "qltp_btkh_thong_tin_chi_tiet";

        public const string Bond_BTKH_TTCT_ThongTinChung = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_ttc";
        public const string Bond_BTKH_TTCT_ThongTinChung_XemChiTiet = BondModule + Form + "qltp_btkh_thong_tin_chi_tiet_ttc_xem_chi_tiet";
        public const string Bond_BTKH_TTCT_ThongTinChung_CapNhat = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ttc_cap_nhat";

        //Tab Tổng quan
        public const string Bond_BTKH_TTCT_TongQuan = BondModule + Tab + "btkh_tong_quan";
        public const string Bond_BTKH_TTCT_TongQuan_CapNhat = BondModule + ButtonAction + "btkh_tong_quan_cap_nhat";
        public const string Bond_BTKH_TTCT_TongQuan_ChonAnh = BondModule + ButtonAction + "btkh_tong_quan_chon_anh";
        public const string Bond_BTKH_TTCT_TongQuan_ThemToChuc = BondModule + ButtonAction + "btkh_tong_quan_them_to_chuc";
        public const string Bond_BTKH_TTCT_TongQuan_UploadFile = BondModule + ButtonAction + "btkh_tong_quan_upload_file";
        public const string Bond_BTKH_TTCT_TongQuan_DanhSach_ToChuc = BondModule + Table + "btkh_tong_quan_danh_sach_to_chuc";
        public const string Bond_BTKH_TTCT_TongQuan_DanhSach_File = BondModule + Table + "btkh_tong_quan_danh_sach_file";

        public const string Bond_BTKH_TTCT_ChinhSach = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_cs";
        public const string Bond_BTKH_TTCT_ChinhSach_DanhSach = BondModule + Table + "qltp_btkh_thong_tin_chi_tiet_cs_ds";
        public const string Bond_BTKH_TTCT_ChinhSach_ThemMoi = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_cs_themcs";
        public const string Bond_BTKH_TTCT_ChinhSach_CapNhat = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_cs_cap_nhat";
        public const string Bond_BTKH_TTCT_ChinhSach_Xoa = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_cs_xoa";
        public const string Bond_BTKH_TTCT_ChinhSach_BatTatShowApp = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_cs_bat_tat_show_app";
        public const string Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_cs_huy_kich_hoat_or_huy";
        public const string Bond_BTKH_TTCT_KyHan_ThemMoi = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ky_han_them_moi";
        public const string Bond_BTKH_TTCT_KyHan_CapNhat = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ky_han_cap_nhat";
        public const string Bond_BTKH_TTCT_KyHan_Xoa = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ky_han_Xoa";
        public const string Bond_BTKH_TTCT_KyHan_BatTatShowApp = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ky_han_bat_tat_show_app";
        public const string Bond_BTKH_TTCT_KyHan_KichHoatOrHuy = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_ky_han_kich_hoat_or_huy";

        public const string Bond_BTKH_TTCT_BangGia = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_bg";
        public const string Bond_BTKH_TTCT_BangGia_DanhSach = BondModule + Table + "qltp_btkh_thong_tin_chi_tiet_bg_danh_sach";
        public const string Bond_BTKH_TTCT_BangGia_ImportExcelBG = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_bg_import_excel_bg";
        public const string Bond_BTKH_TTCT_BangGia_DownloadFileMau = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_bg_download_file_mau";
        public const string Bond_BTKH_TTCT_BangGia_XoaBangGia = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_bg_xoa_bang_gia";

        public const string Bond_BTKH_TTCT_FileChinhSach = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_filecs";
        public const string Bond_BTKH_TTCT_FileChinhSach_DanhSach = BondModule + Table + "qltp_btkh_thong_tin_chi_tiet_filecs_danhsach";
        public const string Bond_BTKH_TTCT_FileChinhSach_UploadFileChinhSach = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_filecs_uploadfilecs";
        public const string Bond_BTKH_TTCT_FileChinhSach_Sua = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_filecs_sua";
        public const string Bond_BTKH_TTCT_FileChinhSach_Xoa = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_filecs_xoa";
        public const string Bond_BTKH_TTCT_FileChinhSach_Download = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_filecs_download";
        public const string Bond_BTKH_TTCT_FileChinhSach_XemFile = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_filecs_xem_file";


        public const string Bond_BTKH_TTCT_MauHopDong = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong";
        public const string Bond_BTKH_TTCT_MauHopDong_DanhSach = BondModule + Table + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_ds";
        public const string Bond_BTKH_TTCT_MauHopDong_ThemMoi = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_them_moi";
        public const string Bond_BTKH_TTCT_MauHopDong_CapNhat = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_cap_nhat";
        public const string Bond_BTKH_TTCT_MauHopDong_Xoa = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_xoa";
        public const string Bond_BTKH_TTCT_MauHopDong_KichHoatOrHuy = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_kich_hoat_or_huy";

        public const string Bond_BTKH_TTCT_MauGiaoNhanHD = BondModule + Tab + "qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd";
        public const string Bond_BTKH_TTCT_MauGiaoNhanHD_DanhSach = BondModule + Table + "qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_ds";
        public const string Bond_BTKH_TTCT_MauGiaoNhanHD_ThemMoi = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_giao_nhan_hd_them_moi";
        public const string Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat = BondModule + ButtonAction + "qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_cap_nhat";



        public const string BondMenuHDPP = BondModule + Menu + "hdpp";

        // Hợp đồng phân phối -> Sổ lệnh
        public const string BondHDPP_SoLenh = BondModule + Menu + "hdpp_solenh";
        public const string BondHDPP_SoLenh_DanhSach = BondModule + Table + "hdpp_solenh_ds";
        public const string BondHDPP_SoLenh_ThemMoi = BondModule + ButtonAction + "hdpp_solenh_them_moi";
        public const string BondHDPP_SoLenh_Xoa = BondModule + ButtonAction + "hdpp_solenh_xoa";

        // TT Chi tiết
        public const string BondHDPP_SoLenh_TTCT = BondModule + Page + "hdpp_solenh_ttct";

        //Tab TT chung
        public const string BondHDPP_SoLenh_TTCT_TTC = BondModule + Tab + "hdpp_solenh_ttct_ttc";
        public const string BondHDPP_SoLenh_TTCT_TTC_XemChiTiet = BondModule + Form + "hdpp_xlhd_ttct_ttc_xem_chi_tiet";
        public const string BondHDPP_SoLenh_TTCT_TTC_CapNhat = BondModule + ButtonAction + "hdpp_solenh_ttct_ttc_cap_nhat";

        //
        public const string BondHDPP_SoLenh_TTCT_TTC_DoiKyHan = BondModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ky_han";
        public const string BondHDPP_SoLenh_TTCT_TTC_DoiMaGT = BondModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ma_gt";
        public const string BondHDPP_SoLenh_TTCT_TTC_DoiTKNganHang = BondModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_tk_ngan_hang";
        public const string BondHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu = BondModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu";

        public const string BondHDPP_SoLenh_TTCT_TTThanhToan = BondModule + Tab + "hdpp_solenh_ttct_ttthanhtoan";

        //TT Thanh Toan
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach = BondModule + Table + "hdpp_solenh_ttct_ttthanhtoan_ds";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_themtt";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_chi_tiettt";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_cap_nhat";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_phe_duyet";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao";
        public const string BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa = BondModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_xoa";

        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy = BondModule + Tab + "hdpp_solenh_ttct_hskh_dangky";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach = BondModule + Table + "hdpp_solenh_ttct_hskh_dangky_ds";

        //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsm";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsckdt";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_chuyen_online";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_ky_dien_tu";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_gui_thong_bao";
        public const string BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy = BondModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy";

        public const string BondHDPP_SoLenh_TTCT_LoiTuc = BondModule + Tab + "hdpp_solenh_ttct_loituc";
        public const string BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach = BondModule + Table + "hdpp_solenh_ttct_loituc_ds";

        //
        public const string BondHDPP_SoLenh_TTCT_TraiTuc = BondModule + Tab + "hdpp_solenh_ttct_traituc";
        public const string BondHDPP_SoLenh_TTCT_TraiTuc_DanhSach = BondModule + Table + "hdpp_solenh_ttct_traituc_ds";

        // HDPP -> Xử lý hợp đồng
        // XLHD: Xử lý hợp đồng
        public const string BondHDPP_XLHD = BondModule + Menu + "hdpp_xlhd";
        public const string BondHDPP_XLHD_DanhSach = BondModule + Table + "hdpp_xlhd_ds";

        // HDPP -> Hợp đồng
        public const string BondHDPP_HopDong = BondModule + Menu + "hdpp_hopdong";
        public const string BondHDPP_HopDong_DanhSach = BondModule + Table + "hdpp_hopdong_ds";
        public const string BondHDPP_HopDong_YeuCauTaiTuc = BondModule + ButtonAction + "hdpp_hopdong_yc_tai_tuc";
        //public const string BondHDPP_HopDong_YeuCauRutVon = BondModule + ButtonAction + "hdpp_hopdong_yc_rut_von";
        public const string BondHDPP_HopDong_PhongToaHopDong = BondModule + ButtonAction + "hdpp_hopdong_phong_toa_hd";


        // HDPP -> Hợp đồng
        public const string BondHDPP_GiaoNhanHopDong = BondModule + Menu + "hdpp_giaonhanhopdong";
        public const string BondHDPP_GiaoNhanHopDong_DanhSach = BondModule + Table + "hdpp_giaonhanhopdong_ds";
        public const string BondHDPP_GiaoNhanHopDong_DoiTrangThai = BondModule + ButtonAction + "hdpp_giaonhanhopdong_doitrangthai";
        public const string BondHDPP_GiaoNhanHopDong_XuatHopDong = BondModule + ButtonAction + "hdpp_giaonhanhopdong_xuathopdong";
        public const string BondHDPP_GiaoNhanHopDong_ThongTinChiTiet = BondModule + Page + "hdpp_giaonhanhopdong_ttct";
        public const string BondHDPP_GiaoNhanHopDong_TTC = BondModule + Tab + "hdpp_giaonhanhopdong_ttc";

        // HDPP -> Phong tỏa giải tóa
        // PTGT: Phong tỏa giải tỏa
        public const string BondHDPP_PTGT = BondModule + Menu + "hdpp_ptgt";
        public const string BondHDPP_PTGT_DanhSach = BondModule + Table + "hdpp_ptgt_ds";
        public const string BondHDPP_PTGT_GiaiToaHopDong = BondModule + ButtonAction + "hdpp_ptgt_phong_toa_hd";
        public const string BondHDPP_PTGT_XemChiTiet = BondModule + ButtonAction + "hdpp_ptgt_xem_chi_tiet";

        // Hợp đồng đáo hạn
        public const string BondHDPP_HopDongDaoHan = BondModule + Menu + "hdpp_hddh";
        public const string BondHDPP_HopDongDaoHan_DanhSach = BondModule + Table + "hdpp_hddh_ds";
        public const string BondHDPP_HopDongDaoHan_ThongTinDauTu = BondModule + ButtonAction + "hdpp_hddh_thong_tin_dau_tu";
        public const string BondHDPP_HopDongDaoHan_LapDSChiTra = BondModule + ButtonAction + "hdpp_hddh_lap_ds_chi_tra";
        public const string BondHDPP_HopDongDaoHan_DuyetKhongChi = BondModule + ButtonAction + "hdpp_hddh_duyet_khong_chi";

        //============================================================

        // Quản lý phê duyệt
        // QLPD: Quản lý phê duyệt
        public const string BondMenuQLPD = BondModule + Menu + "qlpd";

        // Quản lý phê duyệt -> Phê duyệt lô trái phiếu
        // PDLTP: Phê duyệt lô trái phiếu
        public const string BondQLPD_PDLTP = BondModule + Menu + "hdpp_pdltp";
        public const string BondQLPD_PDLTP_DanhSach = BondModule + Table + "hdpp_pdltp_ds";
        public const string BondQLPD_PDLTP_PheDuyetOrHuy = BondModule + ButtonAction + "hdpp_pdltp_phe_duyet_or_huy";


        // Quản lý phê duyệt -> Phê duyệt bán theo kỳ hạn
        // PDLTP: Phê duyệt bán theo kỳ hạn
        public const string BondQLPD_PDBTKH = BondModule + Menu + "hdpp_pdbtkh";
        public const string BondQLPD_PDBTKH_DanhSach = BondModule + Table + "hdpp_pdbtkh_ds";
        public const string BondQLPD_PDBTKH_PheDuyetOrHuy = BondModule + ButtonAction + "hdpp_pdbtkh_phe_duyet_or_huy";

        // Quản lý phê duyệt -> Phê duyệt yêu cầu tái tục
        // PDLTP: Phê duyệt yêu cầu tái tục
        public const string BondQLPD_PDYCTT = BondModule + Menu + "hdpp_pdyctt";
        public const string BondQLPD_PDYCTT_DanhSach = BondModule + Table + "hdpp_pdyctt_ds";
        public const string BondQLPD_PDYCTT_PheDuyetOrHuy = BondModule + ButtonAction + "hdpp_pdyctt_phe_duyet_or_huy";

        public const string Bond_Menu_BaoCao = BondModule + Menu + "bao_cao";

        public const string Bond_BaoCao_QuanTri = BondModule + Page + "bao_cao_quan_tri";
        public const string Bond_BaoCao_QuanTri_THCGTraiPhieu = BondModule + ButtonAction + "bao_cao_quan_tri_thcg_trai_phieu";
        public const string Bond_BaoCao_QuanTri_THCGDauTu = BondModule + ButtonAction + "bao_cao_quan_tri_thcg_dau_tu";
        public const string Bond_BaoCao_QuanTri_TGLDenHan = BondModule + ButtonAction + "bao_cao_quan_tri_tgl_den_han";

        public const string Bond_BaoCao_VanHanh = BondModule + Page + "bao_cao_van_hanh";
        public const string Bond_BaoCao_VanHanh_THCGTraiPhieu = BondModule + ButtonAction + "bao_cao_van_hanh_thcg_trai_phieu";
        public const string Bond_BaoCao_VanHanh_THCGDauTu = BondModule + ButtonAction + "bao_cao_van_hanh_thcg_dau_tu";
        public const string Bond_BaoCao_VanHanh_TGLDenHan = BondModule + ButtonAction + "bao_cao_van_hanh_tgl_den_han";

        public const string Bond_BaoCao_KinhDoanh = BondModule + Page + "bao_cao_kinh_doanh";
        public const string Bond_BaoCao_KinhDoanh_THCGTraiPhieu = BondModule + ButtonAction + "bao_cao_kinh_doanh_thcg_trai_phieu";
        public const string Bond_BaoCao_KinhDoanh_THCGDauTu = BondModule + ButtonAction + "bao_cao_kinh_doanh_thcg_dau_tu";
        public const string Bond_BaoCao_KinhDoanh_TGLDenHan = BondModule + ButtonAction + "bao_cao_kinh_doanh_tgl_den_han";
        #endregion

        #region invest

        //invest
        private const string InvestModule = "invest.";
        public const string InvestWeb = InvestModule + Web;
        // Menu Dashboard
        public const string InvestPageDashboard = InvestModule + Page + "dashboard";

        //Menu Cài đặt
        public const string InvestMenuSetting = InvestModule + Menu + "setting";

        //Module chủ đầu tư
        // dt = Đầu tư 
        public const string InvestMenuChuDT = InvestModule + Menu + "chu_dau_tu";
        public const string InvestChuDT_DanhSach = InvestModule + Table + "chu_dt_danh_sach";
        public const string InvestChuDT_ThemMoi = InvestModule + ButtonAction + "chu_dt_them_moi";
        public const string InvestChuDT_ThongTinChuDauTu = InvestModule + Page + "chu_dt_thong_tin_chu_dau_tu";
        public const string InvestChuDT_Xoa = InvestModule + ButtonAction + "chu_dt_xoa";

        //Tab thông tin chung
        public const string InvestChuDT_ThongTinChung = InvestModule + Tab + "chu_dt_thong_tin_chung";
        public const string InvestChuDT_ChiTiet = InvestModule + Form + "chu_dt_chi_tiet";
        public const string InvestChuDT_CapNhat = InvestModule + ButtonAction + "chu_dt_cap_nhat";

        //Module cấu hình ngày nghỉ lễ
        // nnl = NNL = Ngày nghỉ lễ
        public const string InvestMenuCauHinhNNL = InvestModule + Menu + "cau_hinh_nnl";
        public const string InvestCauHinhNNL_DanhSach = InvestModule + Table + "cau_hinh_nnl_danh_sach";
        public const string InvestCauHinhNNL_CapNhat = InvestModule + ButtonAction + "cau_hinh_nnl_cap_nhat";

        //Module Chính sách mẫu
        public const string InvestMenuChinhSachMau = InvestModule + Menu + "csm";
        public const string InvestCSM_DanhSach = InvestModule + Table + "csm_danh_sach";
        public const string InvestCSM_ThemMoi = InvestModule + ButtonAction + "csm_them_moi";
        public const string InvestCSM_CapNhat = InvestModule + ButtonAction + "csm_cap_nhat";
        public const string InvestCSM_KichHoatOrHuy = InvestModule + ButtonAction + "csm_kich_hoat_or_huy";
        public const string InvestCSM_Xoa = InvestModule + ButtonAction + "csm_xoa";
        public const string InvestCSM_KyHan_ThemMoi = InvestModule + ButtonAction + "csm_ky_han_them_moi";
        public const string InvestCSM_KyHan_CapNhat = InvestModule + ButtonAction + "csm_ky_han_cap_nhat";
        public const string InvestCSM_KyHan_KichHoatOrHuy = InvestModule + ButtonAction + "csm_ky_han_kich_hoat_or_huy";
        public const string InvestCSM_KyHan_Xoa = InvestModule + ButtonAction + "csm_ky_han_xoa";

        //Module cấu hình mã hợp đồng
        public const string InvestMenuCauHinhMaHD = InvestModule + Menu + "cau_hinh_ma_hd";
        public const string InvestCauHinhMaHD_DanhSach = InvestModule + Table + "cau_hinh_ma_hd_danh_sach";
        public const string InvestCauHinhMaHD_ThemMoi = InvestModule + ButtonAction + "cau_hinh_ma_hd_them_moi";
        public const string InvestCauHinhMaHD_CapNhat = InvestModule + ButtonAction + "cau_hinh_ma_hd_cap_nhat";
        public const string InvestCauHinhMaHD_Xoa = InvestModule + ButtonAction + "cau_hinh_ma_hd_xoa";

        //Module mẫu hợp đồng
        public const string InvestMenuMauHD = InvestModule + Menu + "mau_hd";
        public const string InvestMauHD_DanhSach = InvestModule + Table + "mau_hd_danh_sach";
        public const string InvestMauHD_ThemMoi = InvestModule + ButtonAction + "mau_hd_them_moi";
        public const string InvestMauHD_TaiFileCaNhan = InvestModule + ButtonAction + "mau_hd_tai_file_ca_nhan";
        public const string InvestMauHD_TaiFileDoanhNghiep = InvestModule + ButtonAction + "mau_hd_tai_file_doanh_nghiep";
        public const string InvestMauHD_CapNhat = InvestModule + ButtonAction + "mau_hd_cap_nhat";
        public const string InvestMauHD_Xoa = InvestModule + ButtonAction + "mau_hd_xoa";

        //Module Tổng thầu
        public const string InvestMenuTongThau = InvestModule + Menu + "tong_thau";
        public const string InvestTongThau_DanhSach = InvestModule + Table + "tong_thau_danh_sach";
        public const string InvestTongThau_ThemMoi = InvestModule + ButtonAction + "tong_thau_them_moi";
        public const string InvestTongThau_ThongTinTongThau = InvestModule + Page + "tong_thau_thong_tin_tong_thau";
        public const string InvestTongThau_Xoa = InvestModule + ButtonAction + "tong_thau_xoa";

        //Tab thông tin chung
        public const string InvestTongThau_ThongTinChung = InvestModule + Tab + "tong_thau_thong_tin_chung";
        public const string InvestTongThau_ChiTiet = InvestModule + Form + "tong_thau_chi_tiet";

        //Module Đại lý
        public const string InvestMenuDaiLy = InvestModule + Menu + "dai_ly";
        public const string InvestDaiLy_DanhSach = InvestModule + Table + "dai_ly_danh_sach";
        public const string InvestDaiLy_ThemMoi = InvestModule + ButtonAction + "dai_ly_them_moi";
        public const string InvestDaiLy_KichHoatOrHuy = InvestModule + ButtonAction + "dai_ly_kich_hoat_or_huy";
        public const string InvestDaiLy_Xoa = InvestModule + ButtonAction + "dai_ly_xoa";
        public const string InvestDaiLy_ThongTinDaiLy = InvestModule + Page + "dai_ly_thong_tin_dai_ly";

        //Tab thông tin chung
        public const string InvestDaiLy_ThongTinChung = InvestModule + Tab + "dai_ly_thong_tin_chung";
        public const string InvestDaiLy_ChiTiet = InvestModule + Form + "dai_ly_chi_tiet";
        //public const string InvestDaiLy_CapNhat = InvestModule + ButtonAction + "dai_ly_cap_nhat";

        //Tab tài khoản đăng nhập
        public const string InvestDaiLy_TKDN = InvestModule + Tab + "dai_ly_tkdn";
        public const string InvestDaiLy_TKDN_ThemMoi = InvestModule + ButtonAction + "dai_ly_tkdn_them_moi";
        public const string InvestDaiLy_TKDN_DanhSach = InvestModule + Table + "dai_ly_tkdn_danh_sach";

        //Module Hình ảnh
        public const string InvestMenuHinhAnh = InvestModule + Menu + "hinh_anh";
        public const string InvestHinhAnh_DanhSach = InvestModule + Table + "hinh_anh_danh_sach";
        public const string InvestHinhAnh_ThemMoi = InvestModule + ButtonAction + "hinh_anh_them_moi";
        public const string InvestHinhAnh_Sua = InvestModule + ButtonAction + "hinh_anh_sua";
        public const string InvestHinhAnh_Xoa = InvestModule + ButtonAction + "hinh_anh_xoa";
        public const string InvestHinhAnh_DuyetDang = InvestModule + ButtonAction + "hinh_anh_duyet_dang";
        public const string InvestHinhAnh_ChiTiet = InvestModule + ButtonAction + "hinh_anh_chi_tiet";

        //Module Thông báo hệ thống
        public const string InvestMenuThongBaoHeThong = InvestModule + Menu + "thong_bao_he_thong";
        public const string InvestMenuThongBaoHeThong_CaiDat = InvestModule + ButtonAction + "thong_bao_he_thong_cai_dat";

        //Menu Hợp đồng phân phối
        public const string InvestMenuHDPP = InvestModule + Menu + "hdpp";

        //Module Sổ lệnh
        public const string InvestHDPP_SoLenh = InvestModule + Menu + "hdpp_solenh";
        public const string InvestHDPP_SoLenh_DanhSach = InvestModule + Form + "hdpp_solenh_ds";
        public const string InvestHDPP_SoLenh_ThemMoi = InvestModule + ButtonAction + "hdpp_solenh_them_moi";
        public const string InvestHDPP_SoLenh_Xoa = InvestModule + ButtonAction + "hdpp_solenh_xoa";
        // TT Chi tiết = TTCT
        public const string InvestHDPP_SoLenh_TTCT = InvestModule + Page + "hdpp_solenh_ttct";

        //Tab TT Chung = TTC
        public const string InvestHDPP_SoLenh_TTCT_TTC = InvestModule + Tab + "hdpp_solenh_ttct_ttc";
        public const string InvestHDPP_SoLenh_TTCT_TTC_XemChiTiet = InvestModule + Form + "hdpp_solenh_ttct_ttc_xem_chi_tiet";
        public const string InvestHDPP_SoLenh_TTCT_TTC_CapNhat = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttc_cap_nhat";
        public const string InvestHDPP_SoLenh_TTCT_TTC_DoiKyHan = InvestModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ky_han";
        public const string InvestHDPP_SoLenh_TTCT_TTC_DoiMaGT = InvestModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ma_gt";
        public const string InvestHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang = InvestModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_tt_khach_hang";
        public const string InvestHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu = InvestModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu";

        //TT Thanh Toan
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan = InvestModule + Tab + "hdpp_solenh_ttct_ttthanhtoan";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_DanhSach = InvestModule + Tab + "hdpp_solenh_ttct_ttthanhtoan_ds";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_themtt";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_cap_nhat";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_chi_tiettt";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_phe_duyet";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao";
        public const string InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa = InvestModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_xoa";

        //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy = InvestModule + Tab + "hdpp_solenh_ttct_hskh_dangky";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach = InvestModule + Table + "hdpp_solenh_ttct_hskh_dangky_ds";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsm";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsckdt";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_chuyen_online";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_ky_dien_tu";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_gui_thong_bao";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy";
        public const string InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung = InvestModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_nhan_hd_cung";

        //Tab Lợi nhuận
        public const string InvestSoLenh_LoiNhuan = InvestModule + Tab + "hdpp_solenh_ttct_loi_nhuan";
        public const string InvestSoLenh_LoiNhuan_DanhSach = InvestModule + Table + "hdpp_solenh_ttct_loi_nhuan_ds";

        //Tab Lịch sử hợp đồng
        public const string InvestSoLenh_LichSuHD = InvestModule + Tab + "hdpp_solenh_ttct_lich_su_hd";
        public const string InvestSoLenh_LichSuHD_DanhSach = InvestModule + Table + "hdpp_solenh_ttct_lich_su_hd_ds";

        //Trái tức
        public const string InvestHDPP_SoLenh_TTCT_TraiTuc = InvestModule + Tab + "hdpp_solenh_ttct_traituc";
        public const string InvestHDPP_SoLenh_TTCT_TraiTuc_DanhSach = InvestModule + Table + "hdpp_solenh_ttct_traituc_ds";


        // HDPP -> Xử lý hợp đồng
        // XLHD: Xử lý hợp đồng
        public const string InvestHDPP_XLHD = InvestModule + Menu + "hdpp_xlhd";
        public const string InvestHDPP_XLHD_DanhSach = InvestModule + Table + "hdpp_xlhd_ds";

        // HDPP -> Hợp đồng
        public const string InvestHDPP_HopDong = InvestModule + Menu + "hdpp_hopdong";
        public const string InvestHDPP_HopDong_DanhSach = InvestModule + Table + "hdpp_hopdong_ds";
        public const string InvestHDPP_HopDong_YeuCauTaiTuc = InvestModule + ButtonAction + "hdpp_hopdong_yc_tai_tuc";
        public const string InvestHDPP_HopDong_YeuCauRutVon = InvestModule + ButtonAction + "hdpp_hopdong_yc_rut_von";
        public const string InvestHDPP_HopDong_PhongToaHopDong = InvestModule + ButtonAction + "hdpp_hopdong_phong_toa_hd";

        // HDPP -> Giao nhận hợp đồng
        public const string InvestHDPP_GiaoNhanHopDong = InvestModule + Menu + "hdpp_giaonhanhopdong";
        public const string InvestHDPP_GiaoNhanHopDong_DanhSach = InvestModule + Table + "hdpp_giaonhanhopdong_ds";
        public const string InvestHDPP_GiaoNhanHopDong_DoiTrangThai = InvestModule + ButtonAction + "hdpp_giaonhanhopdong_doitrangthai";
        public const string InvestHDPP_GiaoNhanHopDong_XuatHopDong = InvestModule + ButtonAction + "hdpp_giaonhanhopdong_xuat_hd";
        public const string InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet = InvestModule + Page + "hdpp_giaonhanhopdong_ttct";
        public const string InvestHDPP_GiaoNhanHopDong_TTC = InvestModule + Tab + "hdpp_giaonhanhopdong_ttc";

        // Module phong toả, giải toả
        // HD = Hợp đồng
        public const string InvestHopDong_PhongToaGiaiToa = InvestModule + Menu + "hop_dong_phong_toa_giai_toa";
        public const string InvestHopDong_PhongToaGiaiToa_DanhSach = InvestModule + Table + "hop_dong_phong_toa_giai_toa_danh_sach";
        public const string InvestHopDong_PhongToaGiaiToa_GiaiToaHD = InvestModule + ButtonAction + "hop_dong_phong_toa_giai_toa_giai_toa_HD";
        public const string InvestHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa = InvestModule + Form + "hop_dong_phong_toa_giai_toa_thong_tin";

        // Xử lý rút vốn
        public const string InvestHDPP_XLRT = InvestModule + Menu + "hdpp_xlrt";
        public const string InvestHDPP_XLRT_DanhSach = InvestModule + Table + "hdpp_xlrt_danh_sach";
        public const string InvestHDPP_XLRT_DuyetChiTuDong = InvestModule + ButtonAction + "hdpp_xlrt_duyet_chi_tu_dong";
        public const string InvestHDPP_XLRT_DuyetChiThuCong = InvestModule + ButtonAction + "hdpp_xlrt_duyet_chi_thu_cong";
        public const string InvestHDPP_XLRT_HuyYeuCau = InvestModule + ButtonAction + "hdpp_xlrt_huy_yeu_cau";
        public const string InvestHDPP_XLRT_ThongTinDauTu = InvestModule + ButtonAction + "hdpp_xlrt_thong_tin_dau_tu";

        // Hợp đồng đáo hạn
        public const string InvestHopDong_HopDongDaoHan = InvestModule + Menu + "hdpp_hddh";
        public const string InvestHopDong_HopDongDaoHan_DanhSach = InvestModule + Table + "hdpp_hddh_ds";
        public const string InvestHopDong_HopDongDaoHan_ThongTinDauTu = InvestModule + ButtonAction + "hdpp_hddh_thong_tin_dau_tu";
        public const string InvestHopDong_HopDongDaoHan_LapDSChiTra = InvestModule + ButtonAction + "hdpp_hddh_lap_ds_chi_tra";
        public const string InvestHopDong_HopDongDaoHan_DuyetChiTuDong = InvestModule + ButtonAction + "hdpp_hddh_duyet_chi_tu_dong";
        public const string InvestHopDong_HopDongDaoHan_DuyetChiThuCong = InvestModule + ButtonAction + "hdpp_hddh_duyet_chi_thu_cong";
        public const string InvestHopDong_HopDongDaoHan_DuyetTaiTuc = InvestModule + ButtonAction + "hdpp_hddh_duyet_tai_tuc";
        public const string InvestHopDong_HopDongDaoHan_ExportExcel = InvestModule + ButtonAction + "hdpp_hddh_export_excel";

        // HDPP: Chi trả lợi tức
        public const string InvestHDPP_CTLT = InvestModule + Menu + "hdpp_ctlt";
        public const string InvestHDPP_CTLT_DanhSach = InvestModule + Table + "hdpp_ctlt_danh_sach";
        public const string InvestHDPP_CTLT_ThongTinDauTu = InvestModule + ButtonAction + "hdpp_ctlt_thong_tin_dau_tu";
        public const string InvestHDPP_CTLT_LapDSChiTra = InvestModule + ButtonAction + "hdpp_ctlt_lap_ds_chi_tra";
        public const string InvestHDPP_CTLT_DuyetChiTuDong = InvestModule + ButtonAction + "hdpp_ctlt_duyet_chi_tu_dong";
        public const string InvestHDPP_CTLT_DuyetChiThuCong = InvestModule + ButtonAction + "hdpp_ctlt_duyet_chi_thu_cong";
        public const string InvestHDPP_CTLT_ExportExcel = InvestModule + ButtonAction + "hdpp_ctlt_export_excel";

        // HDPP: Tất toán hợp đồng
        /*public const string InvestHDPP_TTHD = InvestModule + Menu + "hdpp_tthd";
        public const string InvestHDPP_TTHD_DanhSach = InvestModule + Table + "hdpp_tthd_ds";*/

        // HDPP: Lịch sử đầu tư
        public const string InvestHDPP_LSDT = InvestModule + Menu + "hdpp_lsdt";
        public const string InvestHDPP_LSDT_DanhSach = InvestModule + Table + "hdpp_lsdt_danh_sach";
        public const string InvestHDPP_LSDT_ThongTinDauTu = InvestModule + ButtonAction + "hdpp_lsdt_thong_tin_dau_tu";

        // HDPP: HỢP ĐỒNG TÁI TỤC
        public const string InvestHDPP_HDTT = InvestModule + Menu + "hdpp_hdtt";
        public const string InvestHDPP_HDTT_DanhSach = InvestModule + Table + "hdpp_hdtt_ds";
        public const string InvestHDPP_HDTT_ThongTinDauTu = InvestModule + ButtonAction + "hdpp_hdtt_thong_tin_dau_tu";
        public const string InvestHDPP_HDTT_HuyYeuCau = InvestModule + ButtonAction + "hdpp_hdtt_huy_yeu_cau";

        //=======================================

        // Menu Quản lý đầu tư
        // qldt = Quản lý đầu tư
        public const string InvestMenuQLDT = InvestModule + Menu + "qldt";
        // Module Sản phẩm đầu tư
        // SPDT = Sản phẩm đầu tư
        public const string InvestMenuSPDT = InvestModule + Menu + "spdt";
        public const string InvestSPDT_DanhSach = InvestModule + Table + "spdt_danh_sach";
        public const string InvestSPDT_ThemMoi = InvestModule + ButtonAction + "spdt_them_moi";
        public const string InvestSPDT_TrinhDuyet = InvestModule + ButtonAction + "spdt_trinh_duyet";
        public const string InvestSPDT_Dong = InvestModule + ButtonAction + "spdt_dong_san_pham";
        public const string InvestSPDT_EpicXacMinh = InvestModule + ButtonAction + "spdt_epic_xac_minh";
        public const string InvestSPDT_Xoa = InvestModule + ButtonAction + "spdt_xoa";
        public const string InvestSPDT_ThongTinSPDT = InvestModule + Page + "spdt_thong_tin_spdt";

        // Tab Thông tin chung
        public const string InvestSPDT_ThongTinChung = InvestModule + Tab + "spdt_thong_tin_chung";
        public const string InvestSPDT_ChiTiet = InvestModule + Form + "spdt_chi_tiet";
        public const string InvestSPDT_CapNhat = InvestModule + ButtonAction + "spdt_cap_nhat";

        // tab hinh anh dau tu
        public const string InvestSPDT_HADT = InvestModule + Tab + "spdt_hadt";
        public const string InvestSPDT_HADT_DanhSach = InvestModule + Table + "spdt_hadt_danh_sach";
        public const string InvestSPDT_HADT_ThemMoi = InvestModule + ButtonAction + "spdt_hadt_them_moi";
        public const string InvestSPDT_HADT_Xoa = InvestModule + ButtonAction + "spdt_hadt_xoa";

        // Tab Hồ sơ pháp lý
        // HSPL = Hồ sơ pháp lý
        public const string InvestSPDT_HSPL = InvestModule + Tab + "spdt_hspl";
        public const string InvestSPDT_HSPL_DanhSach = InvestModule + Table + "spdt_hspl_danh_sach";
        public const string InvestSPDT_HSPL_ThemMoi = InvestModule + ButtonAction + "spdt_hspl_them_moi";
        public const string InvestSPDT_HSPL_Preview = InvestModule + ButtonAction + "spdt_hspl_xem_file";
        public const string InvestSPDT_HSPL_DownloadFile = InvestModule + ButtonAction + "spdt_hspl_download_file";
        public const string InvestSPDT_HSPL_DeleteFile = InvestModule + ButtonAction + "spdt_hspl_xoa_file";

        // Tab Tin tức sản phẩm
        // TTSP = Tin tức sản phẩm
        public const string InvestSPDT_TTSP = InvestModule + Tab + "spdt_ttsp";
        public const string InvestSPDT_TTSP_DanhSach = InvestModule + Table + "spdt_ttsp_danh_sach";
        public const string InvestSPDT_TTSP_ThemMoi = InvestModule + ButtonAction + "spdt_ttsp_them_moi";
        public const string InvestSPDT_TTSP_CapNhat = InvestModule + ButtonAction + "spdt_ttsp_CapNhat";
        public const string InvestSPDT_TTSP_PheDuyetOrHuy = InvestModule + ButtonAction + "spdt_ttsp_phe_duyet_or_huy";
        public const string InvestSPDT_TTSP_Xoa = InvestModule + ButtonAction + "spdt_ttsp_xoa";

        // Module Phân phối đầu tư
        // PPDT = phân phối đầu tư
        public const string InvestMenuPPDT = InvestModule + Menu + "ppdt";
        public const string InvestPPDT_DanhSach = InvestModule + Table + "ppdt_danh_sach";
        public const string InvestPPDT_ThemMoi = InvestModule + ButtonAction + "ppdt_them_moi";
        public const string InvestPPDT_DongTam = InvestModule + ButtonAction + "ppdt_xoa";
        public const string InvestPPDT_TrinhDuyet = InvestModule + ButtonAction + "ppdt_trinh_duyet";
        public const string InvestPPDT_BatTatShowApp = InvestModule + ButtonAction + "ppdt_tat_show_app";
        public const string InvestPPDT_EpicXacMinh = InvestModule + ButtonAction + "ppdt_epic_xac_minh";
        public const string InvestPPDT_ThongTinPPDT = InvestModule + Page + "ppdt_thong_tin_ppdt";

        // Tab Thông tin chung
        public const string InvestPPDT_ThongTinChung = InvestModule + Tab + "ppdt_thong_tin_chung";
        public const string InvestPPDT_ChiTiet = InvestModule + Form + "ppdt_chi_tiet";
        public const string InvestPPDT_CapNhat = InvestModule + ButtonAction + "ppdt_cap_nhat";

        //Tab Tổng quan
        public const string InvestPPDT_TongQuan = InvestModule + Tab + "ppdt_tong_quan";
        public const string InvestPPDT_TongQuan_CapNhat = InvestModule + ButtonAction + "ppdt_tong_quan_cap_nhat";
        public const string InvestPPDT_TongQuan_ChonAnh = InvestModule + ButtonAction + "ppdt_tong_quan_chon_anh";
        public const string InvestPPDT_TongQuan_ThemToChuc = InvestModule + ButtonAction + "ppdt_tong_quan_them_to_chuc";
        public const string InvestPPDT_TongQuan_DanhSach_ToChuc = InvestModule + Table + "ppdt_tong_quan_danh_sach_to_chuc";
        public const string InvestPPDT_TongQuan_UploadFile = InvestModule + ButtonAction + "ppdt_tong_quan_upload_file";
        public const string InvestPPDT_TongQuan_DanhSach_File = InvestModule + Table + "ppdt_tong_quan_danh_sach_file";


        // Tab Chính sách
        // Chính sách
        public const string InvestPPDT_ChinhSach = InvestModule + Tab + "ppdt_chinh_sach";
        public const string InvestPPDT_ChinhSach_DanhSach = InvestModule + Table + "ppdt_chinh_sach_danh_sach";
        public const string InvestPPDT_ChinhSach_ThemMoi = InvestModule + ButtonAction + "ppdt_chinh_sach_them_moi";
        public const string InvestPPDT_ChinhSach_CapNhat = InvestModule + ButtonAction + "ppdt_chinh_sach_CapNhat";
        public const string InvestPPDT_ChinhSach_KichHoatOrHuy = InvestModule + ButtonAction + "ppdt_chinh_sach_kich_hoat_or_huy";
        public const string InvestPPDT_ChinhSach_BatTatShowApp = InvestModule + ButtonAction + "ppdt_chinh_sach_bat_tat_show_app";
        public const string InvestPPDT_ChinhSach_Xoa = InvestModule + ButtonAction + "ppdt_chinh_sach_xoa";

        public const string InvestPPDT_KyHan_ThemMoi = InvestModule + ButtonAction + "ppdt_ky_han_them_moi";
        public const string InvestPPDT_KyHan_CapNhat = InvestModule + ButtonAction + "ppdt_ky_han_cap_nhat";
        public const string InvestPPDT_KyHan_KichHoatOrHuy = InvestModule + ButtonAction + "ppdt_ky_han_kich_hoat_or_huy";
        public const string InvestPPDT_KyHan_BatTatShowApp = InvestModule + ButtonAction + "ppdt_ky_han_bat_tat_show_app";
        public const string InvestPPDT_KyHan_Xoa = InvestModule + ButtonAction + "ppdt_ky_han_xoa";

        // Tab File chính sách
        public const string InvestPPDT_FileChinhSach = InvestModule + Tab + "ppdt_file_chinh_sach";
        public const string InvestPPDT_FileChinhSach_DanhSach = InvestModule + Table + "ppdt_file_chinh_sach_danh_sach";
        public const string InvestPPDT_FileChinhSach_ThemMoi = InvestModule + ButtonAction + "ppdt_file_chinh_sach_them_moi";
        public const string InvestPPDT_FileChinhSach_CapNhat = InvestModule + ButtonAction + "ppdt_file_chinh_sach_cap_nhat";
        public const string InvestPPDT_FileChinhSach_Xoa = InvestModule + ButtonAction + "ppdt_file_chinh_sach_xoa";

        // Tab Mẫu hợp đồng
        public const string InvestPPDT_MauHopDong = InvestModule + Tab + "ppdt_mau_hop_dong";
        public const string InvestPPDT_MauHopDong_DanhSach = InvestModule + Table + "ppdt_mau_hop_dong_danh_sach";
        public const string InvestPPDT_MauHopDong_ThemMoi = InvestModule + ButtonAction + "ppdt_mau_hop_dong_them_moi";
        public const string InvestPPDT_MauHopDong_CapNhat = InvestModule + ButtonAction + "ppdt_mau_hop_dong_cap_nhat";
        public const string InvestPPDT_MauHopDong_Xoa = InvestModule + ButtonAction + "ppdt_mau_hop_dong_xoa";

        // Tab Hợp đồng phân phối   
        public const string InvestPPDT_HopDongPhanPhoi = InvestModule + Tab + "ppdt_hop_dong_phan_phoi";
        public const string InvestPPDT_HopDongPhanPhoi_DanhSach = InvestModule + Table + "ppdt_hop_dong_phan_phoi_danh_sach";
        public const string InvestPPDT_HopDongPhanPhoi_ThemMoi = InvestModule + ButtonAction + "ppdt_hop_dong_phan_phoi_them_moi";
        public const string InvestPPDT_HopDongPhanPhoi_CapNhat = InvestModule + ButtonAction + "ppdt_hop_dong_phan_phoi_cap_nhat";
        public const string InvestPPDT_HopDongPhanPhoi_Xoa = InvestModule + ButtonAction + "ppdt_hop_dong_phan_phoi_xoa";

        // Tab Mẫu giao nhận hợp đồng
        public const string InvestPPDT_MauGiaoNhanHD = InvestModule + Tab + "ppdt_mau_giao_nhan_hd";
        public const string InvestPPDT_MauGiaoNhanHD_DanhSach = InvestModule + Table + "ppdt_mau_giao_nhan_hd_danh_sach";
        public const string InvestPPDT_MauGiaoNhanHD_ThemMoi = InvestModule + ButtonAction + "ppdt_mau_giao_nhan_hd_them_moi";
        public const string InvestPPDT_MauGiaoNhanHD_CapNhat = InvestModule + ButtonAction + "ppdt_mau_giao_nhan_hd_cap_nhat";
        public const string InvestPPDT_MauGiaoNhanHD_KichHoat = InvestModule + ButtonAction + "ppdt_mau_giao_nhan_hd_kich_hoat";
        public const string InvestPPDT_MauGiaoNhanHD_Xoa = InvestModule + ButtonAction + "ppdt_mau_giao_nhan_hd_xoa";

        // Menu Quản lý phê duyệt
        // qlpd = Quản lý phê duyệt 
        public const string InvestMenuQLPD = InvestModule + Menu + "qlpd";
        // Module Phê duyệt sản phẩm đầu tư
        // PDSPDT = Phê duyệt sản phẩm đầu tư
        public const string InvestMenuPDSPDT = InvestModule + Menu + "pdspdt";
        public const string InvestPDSPDT_DanhSach = InvestModule + Table + "pdspdt_danh_sach";
        public const string InvestPDSPDT_PheDuyetOrHuy = InvestModule + ButtonAction + "pdspdt_phe_duyet";

        // Module Phê duyệt phân phối đầu tư
        // PDPPDT = Phê duyệt phân phối đầu tư
        public const string InvestMenuPDPPDT = InvestModule + Menu + "pdppdt";
        public const string InvestPDPPDT_DanhSach = InvestModule + Table + "pdppdt_danh_sach";
        public const string InvestPDPPDT_PheDuyetOrHuy = InvestModule + ButtonAction + "pdppdt_phe_duyet";

        // Module Phê duyệt yêu cầu tái tục
        // PDPPDT = Phê duyệt yêu cầu tái tục
        public const string InvestMenuPDYCTT = InvestModule + Menu + "pdyctt";
        public const string InvestPDYCTT_DanhSach = InvestModule + Table + "pdyctt_danh_sach";
        public const string InvestPDYCTT_PheDuyetOrHuy = InvestModule + ButtonAction + "pdyctt_phe_duyet";

        // Module Phê duyệt yêu cầu rút vốn
        // PDPPDT = Phê duyệt yêu cầu rút vốn
        public const string InvestMenuPDYCRV = InvestModule + Menu + "pdycrv";
        public const string InvestPDYCRV_DanhSach = InvestModule + Table + "pdycrv_danh_sach";
        public const string InvestPDYCRV_PheDuyetOrHuy = InvestModule + ButtonAction + "pdycrv_phe_duyet";
        public const string InvestPDYCRV_ChiTietHD = InvestModule + ButtonAction + "pdycrv_chi_tiet_hd";

        public const string Invest_Menu_BaoCao = InvestModule + Menu + "bao_cao";

        public const string Invest_BaoCao_QuanTri = InvestModule + Page + "bao_cao_quan_tri";
        public const string Invest_BaoCao_QuanTri_THCKDauTu = InvestModule + ButtonAction + "bao_cao_quan_tri_thck_dau_tu";
        public const string Invest_BaoCao_QuanTri_THCMaBDS = InvestModule + ButtonAction + "bao_cao_quan_tri_thc_ma_bds";
        public const string Invest_BaoCao_QuanTri_DCDenHan = InvestModule + ButtonAction + "bao_cao_quan_tri_dc_den_han";
        public const string Invest_BaoCao_QuanTri_ThucChi = InvestModule + ButtonAction + "bao_cao_quan_tri_thuc_chi";
        public const string Invest_BaoCao_QuanTri_TCTDauTu = InvestModule + ButtonAction + "bao_cao_quan_tri_tct_dau_tu";
        public const string Invest_BaoCao_QuanTri_THCKDTVanHanhHVF = InvestModule + ButtonAction + "bao_cao_quan_tri_thckdt_van_hanh_hvf";
        public const string Invest_BaoCao_QuanTri_SKTKNhaDauTu = InvestModule + ButtonAction + "bao_cao_quan_tri_sktk_nha_dau_tu";
        public const string Invest_BaoCao_QuanTri_THCKDTBanHo = InvestModule + ButtonAction + "bao_cao_quan_tri_thckdt_ban_ho";

        public const string Invest_BaoCao_VanHanh = InvestModule + Page + "bao_cao_van_hanh";
        public const string Invest_BaoCao_VanHanh_THCKDauTu = InvestModule + ButtonAction + "bao_cao_van_hanh_thck_dau_tu";
        public const string Invest_BaoCao_VanHanh_THCMaBDS = InvestModule + ButtonAction + "bao_cao_van_hanh_thc_ma_bds";
        public const string Invest_BaoCao_VanHanh_DCDenHan = InvestModule + ButtonAction + "bao_cao_van_hanh_dc_den_han";
        public const string Invest_BaoCao_VanHanh_ThucChi = InvestModule + ButtonAction + "bao_cao_van_hanh_thuc_chi";
        public const string Invest_BaoCao_VanHanh_TCTDauTu = InvestModule + ButtonAction + "bao_cao_van_hanh_tct_dau_tu";

        public const string Invest_BaoCao_KinhDoanh = InvestModule + Page + "bao_cao_kinh_doanh";
        public const string Invest_BaoCao_KinhDoanh_THCKDauTu = InvestModule + ButtonAction + "bao_cao_kinh_doanh_thck_dau_tu";
        public const string Invest_BaoCao_KinhDoanh_THCMaBDS = InvestModule + ButtonAction + "bao_cao_kinh_doanh_thc_ma_bds";
        public const string Invest_BaoCao_KinhDoanh_DCDenHan = InvestModule + ButtonAction + "bao_cao_kinh_doanh_dc_den_han";
        public const string Invest_BaoCao_KinhDoanh_ThucChi = InvestModule + ButtonAction + "bao_cao_kinh_doanh_thuc_chi";
        public const string Invest_BaoCao_KinhDoanh_TCTDauTu = InvestModule + ButtonAction + "bao_cao_kinh_doanh_tct_dau_tu";

        public const string Invest_Menu_TruyVan = InvestModule + Menu + "truy_van";
        public const string Invest_TruyVan_ThuTien_NganHang = InvestModule + Page + "truy_van_thu_tien_ngan_hang";
        public const string Invest_TruyVan_ChiTien_NganHang = InvestModule + Page + "truy_van_chi_tien_ngan_hang";
        #endregion

        #region garner

        //garner
        private const string GarnerModule = "garner.";
        public const string GarnerWeb = GarnerModule + Web;
        // Menu Dashboard
        public const string GarnerPageDashboard = GarnerModule + Page + "dashboard";

        //Menu Cài đặt
        public const string GarnerMenuSetting = GarnerModule + Menu + "setting";

        //Module chủ đầu tư
        // dt = Đầu tư 
        public const string GarnerMenuChuDT = GarnerModule + Menu + "chu_dau_tu";
        public const string GarnerChuDT_DanhSach = GarnerModule + Table + "chu_dt_danh_sach";
        public const string GarnerChuDT_ThemMoi = GarnerModule + ButtonAction + "chu_dt_them_moi";
        public const string GarnerChuDT_ThongTinChuDauTu = GarnerModule + Page + "chu_dt_thong_tin_chu_dau_tu";
        public const string GarnerChuDT_Xoa = GarnerModule + ButtonAction + "chu_dt_xoa";

        //Tab thông tin chung
        public const string GarnerChuDT_ThongTinChung = GarnerModule + Tab + "chu_dt_thong_tin_chung";
        public const string GarnerChuDT_ChiTiet = GarnerModule + Form + "chu_dt_chi_tiet";
        public const string GarnerChuDT_CapNhat = GarnerModule + ButtonAction + "chu_dt_cap_nhat";

        //Module cấu hình ngày nghỉ lễ
        // nnl = NNL = Ngày nghỉ lễ
        public const string GarnerMenuCauHinhNNL = GarnerModule + Menu + "cau_hinh_nnl";
        public const string GarnerCauHinhNNL_DanhSach = GarnerModule + Table + "cau_hinh_nnl_danh_sach";
        public const string GarnerCauHinhNNL_CapNhat = GarnerModule + ButtonAction + "cau_hinh_nnl_cap_nhat";

        //Module Chính sách mẫu
        public const string GarnerMenuChinhSachMau = GarnerModule + Menu + "csm";
        public const string GarnerCSM_DanhSach = GarnerModule + Table + "csm_danh_sach";
        public const string GarnerCSM_ThemMoi = GarnerModule + ButtonAction + "csm_them_moi";
        public const string GarnerCSM_CapNhat = GarnerModule + ButtonAction + "csm_cap_nhat";
        public const string GarnerCSM_KichHoatOrHuy = GarnerModule + ButtonAction + "csm_kich_hoat_or_huy";
        public const string GarnerCSM_Xoa = GarnerModule + ButtonAction + "csm_xoa";
        public const string GarnerCSM_KyHan_ThemMoi = GarnerModule + ButtonAction + "csm_ky_han_them_moi";
        public const string GarnerCSM_KyHan_CapNhat = GarnerModule + ButtonAction + "csm_ky_han_cap_nhat";
        public const string GarnerCSM_KyHan_KichHoatOrHuy = GarnerModule + ButtonAction + "csm_ky_han_kich_hoat_or_huy";
        public const string GarnerCSM_KyHan_Xoa = GarnerModule + ButtonAction + "csm_ky_han_xoa";
        public const string GarnerCSM_HopDongMau_ThemMoi = GarnerModule + ButtonAction + "csm_hop_dong_mau_them_moi";
        public const string GarnerCSM_HopDongMau_CapNhat = GarnerModule + ButtonAction + "csm_hop_dong_mau_cap_nhat";
        public const string GarnerCSM_HopDongMau_KichHoatOrHuy = GarnerModule + ButtonAction + "csm_hop_dong_mau_kich_hoat_or_huy";
        public const string GarnerCSM_HopDongMau_Xoa = GarnerModule + ButtonAction + "csm_hop_dong_mau_xoa";

        //Module Mô tả chung
        public const string GarnerMenuMoTaChung = GarnerModule + Menu + "mtc";
        public const string GarnerMTC_DanhSach = GarnerModule + Table + "mtc_danh_sach";
        public const string GarnerMTC_ThemMoi = GarnerModule + ButtonAction + "mtc_them_moi";
        public const string GarnerMTC_ThongTinMTC = GarnerModule + Page + "mtc_thong_tin_mtc";
        public const string GarnerMTC_ThongTinMTC_Sua = GarnerModule + ButtonAction + "mtc_thong_tin_mtc_sua";
        public const string GarnerMTC_ThongTinMTC_NoiBat = GarnerModule + ButtonAction + "mtc_thong_tin_mtc_noi_bat";

        public const string GarnerMTC_ThongTinMTC_Xem = GarnerModule + ButtonAction + "mtc_thong_tin_mtc_xem";
        public const string GarnerMTC_ThongTinMTC_Xoa = GarnerModule + ButtonAction + "mtc_thong_tin_mtc_xoa";

        //Module cấu hình mã hợp đồng
        public const string GarnerMenuCauHinhMaHD = GarnerModule + Menu + "cau_hinh_ma_hd";
        public const string GarnerCauHinhMaHD_DanhSach = GarnerModule + Table + "cau_hinh_ma_hd_danh_sach";
        public const string GarnerCauHinhMaHD_ThemMoi = GarnerModule + ButtonAction + "cau_hinh_ma_hd_them_moi";
        public const string GarnerCauHinhMaHD_CapNhat = GarnerModule + ButtonAction + "cau_hinh_ma_hd_cap_nhat";
        public const string GarnerCauHinhMaHD_Xoa = GarnerModule + ButtonAction + "cau_hinh_ma_hd_xoa";
        //Module mẫu hợp đồng
        public const string GarnerMenuMauHD = GarnerModule + Menu + "mau_hd";
        public const string GarnerMauHD_DanhSach = GarnerModule + Table + "mau_hd_danh_sach";
        public const string GarnerMauHD_ThemMoi = GarnerModule + ButtonAction + "mau_hd_them_moi";
        public const string GarnerMauHD_TaiFileCaNhan = GarnerModule + ButtonAction + "mau_hd_tai_file_ca_nhan";
        public const string GarnerMauHD_TaiFileDoanhNghiep = GarnerModule + ButtonAction + "mau_hd_tai_file_doanh_nghiep";
        public const string GarnerMauHD_CapNhat = GarnerModule + ButtonAction + "mau_hd_cap_nhat";
        public const string GarnerMauHD_Xoa = GarnerModule + ButtonAction + "mau_hd_xoa";
        //Module Tổng thầu
        public const string GarnerMenuTongThau = GarnerModule + Menu + "tong_thau";
        public const string GarnerTongThau_DanhSach = GarnerModule + Table + "tong_thau_danh_sach";
        public const string GarnerTongThau_ThemMoi = GarnerModule + ButtonAction + "tong_thau_them_moi";
        public const string GarnerTongThau_ThongTinTongThau = GarnerModule + Page + "tong_thau_thong_tin_tong_thau";
        public const string GarnerTongThau_Xoa = GarnerModule + ButtonAction + "tong_thau_xoa";

        //Tab thông tin chung
        public const string GarnerTongThau_ThongTinChung = GarnerModule + Tab + "tong_thau_thong_tin_chung";
        public const string GarnerTongThau_ChiTiet = GarnerModule + Form + "tong_thau_chi_tiet";

        //Module Đại lý
        public const string GarnerMenuDaiLy = GarnerModule + Menu + "dai_ly";
        public const string GarnerDaiLy_DanhSach = GarnerModule + Table + "dai_ly_danh_sach";
        public const string GarnerDaiLy_ThemMoi = GarnerModule + ButtonAction + "dai_ly_them_moi";
        public const string GarnerDaiLy_DoiTrangThai = GarnerModule + ButtonAction + "dai_ly_doi_trang_thai";
        public const string GarnerDaiLy_Xoa = GarnerModule + ButtonAction + "dai_ly_xoa";
        public const string GarnerDaiLy_ThongTinDaiLy = GarnerModule + Page + "dai_ly_thong_tin_dai_ly";

        //Tab thông tin chung
        public const string GarnerDaiLy_ThongTinChung = GarnerModule + Tab + "dai_ly_thong_tin_chung";
        public const string GarnerDaiLy_ChiTiet = GarnerModule + Form + "dai_ly_chi_tiet";
        //public const string GarnerDaiLy_CapNhat = GarnerModule + ButtonAction + "dai_ly_cap_nhat";

        //Tab tài khoản đăng nhập
        public const string GarnerDaiLy_TKDN = GarnerModule + Tab + "dai_ly_tkdn";
        public const string GarnerDaiLy_TKDN_ThemMoi = GarnerModule + ButtonAction + "dai_ly_tkdn_them_moi";
        public const string GarnerDaiLy_TKDN_DanhSach = GarnerModule + Table + "dai_ly_tkdn_danh_sach";

        //Module Hình ảnh
        public const string GarnerMenuHinhAnh = GarnerModule + Menu + "hinh_anh";
        public const string GarnerHinhAnh_DanhSach = GarnerModule + Table + "hinh_anh_danh_sach";
        public const string GarnerHinhAnh_ThemMoi = GarnerModule + ButtonAction + "hinh_anh_them_moi";
        public const string GarnerHinhAnh_Sua = GarnerModule + ButtonAction + "hinh_anh_sua";
        public const string GarnerHinhAnh_Xoa = GarnerModule + ButtonAction + "hinh_anh_xoa";
        public const string GarnerHinhAnh_DuyetDang = GarnerModule + ButtonAction + "hinh_anh_duyet_dang";
        public const string GarnerHinhAnh_ChiTiet = GarnerModule + ButtonAction + "hinh_anh_chi_tiet";

        //Module Thông báo hệ thống
        public const string GarnerMenuThongBaoHeThong = GarnerModule + Menu + "thong_bao_he_thong";
        public const string GarnerMenuThongBaoHeThong_CaiDat = GarnerModule + ButtonAction + "thong_bao_he_thong_cai_dat";

        //Menu Hợp đồng phân phối
        public const string GarnerMenuHDPP = GarnerModule + Menu + "hdpp";

        //Module Sổ lệnh
        public const string GarnerHDPP_SoLenh = GarnerModule + Menu + "hdpp_solenh";
        public const string GarnerHDPP_SoLenh_DanhSach = GarnerModule + Form + "hdpp_solenh_ds";
        public const string GarnerHDPP_SoLenh_ThemMoi = GarnerModule + ButtonAction + "hdpp_solenh_them_moi";
        public const string GarnerHDPP_SoLenh_Xoa = GarnerModule + ButtonAction + "hdpp_solenh_xoa";
        // TT Chi tiết = TTCT
        public const string GarnerHDPP_SoLenh_TTCT = GarnerModule + Page + "hdpp_solenh_ttct";

        //Tab TT Chung = TTC
        public const string GarnerHDPP_SoLenh_TTCT_TTC = GarnerModule + Tab + "hdpp_solenh_ttct_ttc";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_XemChiTiet = GarnerModule + Form + "hdpp_solenh_ttct_ttc_xem_chi_tiet";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_CapNhat = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttc_cap_nhat";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_DoiKyHan = GarnerModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ky_han";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT = GarnerModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_ma_gt";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang = GarnerModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_tt_khach_hang";
        public const string GarnerHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu = GarnerModule + ButtonAction + "hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu";

        //TT Thanh Toan
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan = GarnerModule + Tab + "hdpp_solenh_ttct_ttthanhtoan";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_DanhSach = GarnerModule + Tab + "hdpp_solenh_ttct_ttthanhtoan_ds";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_themtt";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_cap_nhat";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_chi_tiettt";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_phe_duyet";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao";
        public const string GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa = GarnerModule + ButtonAction + "hdpp_solenh_ttct_ttthanhtoan_xoa";

        //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy = GarnerModule + Tab + "hdpp_solenh_ttct_hskh_dangky";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach = GarnerModule + Table + "hdpp_solenh_ttct_hskh_dangky_ds";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsm";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_hsckdt";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_chuyen_online";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_ky_dien_tu";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_gui_thong_bao";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy";
        public const string GarnerHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung = GarnerModule + ButtonAction + "hdpp_solenh_ttct_hskh_dangky_nhan_hd_cung";

        //Tab Lợi nhuận
        public const string GarnerSoLenh_LoiNhuan = GarnerModule + Tab + "hdpp_solenh_ttct_loi_nhuan";
        public const string GarnerSoLenh_LoiNhuan_DanhSach = GarnerModule + Table + "hdpp_solenh_ttct_loi_nhuan_ds";

        //Tab Lịch sử hợp đồng
        public const string GarnerSoLenh_LichSuHD = GarnerModule + Tab + "hdpp_solenh_ttct_lich_su_hd";
        public const string GarnerSoLenh_LichSuHD_DanhSach = GarnerModule + Table + "hdpp_solenh_ttct_lich_su_hd_ds";

        //Trái tức
        public const string GarnerHDPP_SoLenh_TTCT_TraiTuc = GarnerModule + Tab + "hdpp_solenh_ttct_traituc";
        public const string GarnerHDPP_SoLenh_TTCT_TraiTuc_DanhSach = GarnerModule + Table + "hdpp_solenh_ttct_traituc_ds";


        // HDPP -> Xử lý hợp đồng
        // XLHD: Xử lý hợp đồng
        public const string GarnerHDPP_XLHD = GarnerModule + Menu + "hdpp_xlhd";
        public const string GarnerHDPP_XLHD_DanhSach = GarnerModule + Table + "hdpp_xlhd_ds";

        // HDPP -> Hợp đồng
        public const string GarnerHDPP_HopDong = GarnerModule + Menu + "hdpp_hopdong";
        public const string GarnerHDPP_HopDong_DanhSach = GarnerModule + Table + "hdpp_hopdong_ds";
        public const string GarnerHDPP_HopDong_YeuCauTaiTuc = GarnerModule + ButtonAction + "hdpp_hopdong_yc_tai_tuc";
        public const string GarnerHDPP_HopDong_YeuCauRutVon = GarnerModule + ButtonAction + "hdpp_hopdong_yc_rut_von";
        public const string GarnerHDPP_HopDong_PhongToaHopDong = GarnerModule + ButtonAction + "hdpp_hopdong_phong_toa_hd";

        // HDPP -> Giao nhận hợp đồng
        public const string GarnerHDPP_GiaoNhanHopDong = GarnerModule + Menu + "hdpp_giaonhanhopdong";
        public const string GarnerHDPP_GiaoNhanHopDong_DanhSach = GarnerModule + Table + "hdpp_giaonhanhopdong_ds";
        public const string GarnerHDPP_GiaoNhanHopDong_DoiTrangThai = GarnerModule + ButtonAction + "hdpp_giaonhanhopdong_doitrangthai";
        public const string GarnerHDPP_GiaoNhanHopDong_XuatHopDong = GarnerModule + ButtonAction + "hdpp_giaonhanhopdong_xuat_hd";
        public const string GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet = GarnerModule + Page + "hdpp_giaonhanhopdong_ttct";
        public const string GarnerHDPP_GiaoNhanHopDong_TTC = GarnerModule + Tab + "hdpp_giaonhanhopdong_ttc";

        // Module phong toả, giải toả
        // HD = Hợp đồng
        public const string GarnerHopDong_PhongToaGiaiToa = GarnerModule + Menu + "hop_dong_phong_toa_giai_toa";
        public const string GarnerHopDong_PhongToaGiaiToa_DanhSach = GarnerModule + Table + "hop_dong_phong_toa_giai_toa_danh_sach";
        public const string GarnerHopDong_PhongToaGiaiToa_GiaiToaHD = GarnerModule + ButtonAction + "hop_dong_phong_toa_giai_toa_giai_toa_HD";
        public const string GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa = GarnerModule + Form + "hop_dong_phong_toa_giai_toa_thong_tin";

        // Hợp đồng đáo hạn
        public const string GarnerHopDong_HopDongDaoHan = GarnerModule + Menu + "hdpp_hddh";
        public const string GarnerHopDong_HopDongDaoHan_DanhSach = GarnerModule + Table + "hdpp_hddh_ds";
        public const string GarnerHopDong_HopDongDaoHan_ThongTinDauTu = GarnerModule + ButtonAction + "hdpp_hddh_thong_tin_dau_tu";
        public const string GarnerHopDong_HopDongDaoHan_LapDSChiTra = GarnerModule + ButtonAction + "hdpp_hddh_lap_ds_chi_tra";
        public const string GarnerHopDong_HopDongDaoHan_DuyetKhongChi = GarnerModule + ButtonAction + "hdpp_hddh_duyet_khong_chi";

        // Xử lý rút tiền
        public const string GarnerHDPP_XLRutTien = GarnerModule + Menu + "hdpp_xlruttien";
        public const string GarnerHDPP_XLRutTien_DanhSach = GarnerModule + Table + "hdpp_xlruttien_ds";
        public const string GarnerHDPP_XLRutTien_ThongTinChiTiet = GarnerModule + ButtonAction + "hdpp_xlruttien_thong_tin_chi_tiet";
        public const string GarnerHDPP_XLRutTien_ChiTienTD = GarnerModule + ButtonAction + "hdpp_xlruttien_chi_tien_td";
        public const string GarnerHDPP_XLRutTien_ChiTienTC = GarnerModule + ButtonAction + "hdpp_xlruttien_chi_tien_tc";
        public const string GarnerHDPP_XLRutTien_HuyYeuCau = GarnerModule + ButtonAction + "hdpp_xlruttien_huy_yeu_cau";

        // Chi tra loi tuc
        public const string GarnerHDPP_CTLC = GarnerModule + Menu + "hdpp_ctlc";
        public const string GarnerHDPP_CTLC_DanhSach = GarnerModule + Table + "hdpp_ctlc_ds";
        public const string GarnerHDPP_CTLC_ThongTinChiTiet = GarnerModule + ButtonAction + "hdpp_ctlc_thong_tin_chi_tiet";
        public const string GarnerHDPP_CTLC_DuyetChiTD = GarnerModule + ButtonAction + "hdpp_ctlc_duyet_chi_td";
        public const string GarnerHDPP_CTLC_DuyetChiTC = GarnerModule + ButtonAction + "hdpp_ctlc_duyet_chi_tc";
        public const string GarnerHDPP_CTLC_LapDSChiTra = GarnerModule + ButtonAction + "hdpp_ctlc_lap_ds_chi_tra";

        // Lịch sử tích lũy
        public const string GarnerHDPP_LSTL = GarnerModule + Menu + "hdpp_lstl";
        public const string GarnerHDPP_LSTL_DanhSach = GarnerModule + Table + "hdpp_lstl_danh_sach";
        public const string GarnerHDPP_LSTL_ThongTinChiTiet = GarnerModule + ButtonAction + "hdpp_lstl_thong_tin_chi_tiet";

        //=======================================

        // Menu Quản lý đầu tư
        // qldt = Quản lý đầu tư
        public const string GarnerMenuQLDT = GarnerModule + Menu + "qldt";
        // Module Sản phẩm đầu tư
        // SPDT = Sản phẩm đầu tư
        public const string GarnerMenuSPTL = GarnerModule + Menu + "spdt";
        public const string GarnerSPDT_DanhSach = GarnerModule + Table + "spdt_danh_sach";
        public const string GarnerSPDT_ThemMoi = GarnerModule + ButtonAction + "spdt_them_moi";
        public const string GarnerSPDT_TrinhDuyet = GarnerModule + ButtonAction + "spdt_trinh_duyet";
        public const string GarnerSPDT_DongOrMo = GarnerModule + ButtonAction + "spdt_dong_or_mo";
        public const string GarnerSPDT_EpicXacMinh = GarnerModule + ButtonAction + "spdt_epic_xac_minh";
        public const string GarnerSPDT_Xoa = GarnerModule + ButtonAction + "spdt_xoa";
        public const string GarnerSPDT_ThongTinSPDT = GarnerModule + Page + "spdt_thong_tin_spdt";

        // Tab Thông tin chung
        public const string GarnerSPDT_ThongTinChung = GarnerModule + Tab + "spdt_thong_tin_chung";
        public const string GarnerSPDT_ChiTiet = GarnerModule + Form + "spdt_chi_tiet";
        public const string GarnerSPDT_CapNhat = GarnerModule + ButtonAction + "spdt_cap_nhat";

        // Tab Dai ly phan phoi
        public const string GarnerSPDT_DLPP = GarnerModule + Tab + "spdt_dlpp";
        public const string GarnerSPDT_DLPP_DanhSach = GarnerModule + Table + "spdt_dlpp_danh_sach";
        public const string GarnerSPDT_DLPP_ThemMoi = GarnerModule + ButtonAction + "spdt_dlpp_them_moi";
        public const string GarnerSPDT_DLPP_CapNhat = GarnerModule + ButtonAction + "spdt_dlpp_CapNhat";
        //public const string GarnerSPDT_DLPP_PheDuyetOrHuy = GarnerModule + ButtonAction + "spdt_dlpp_phe_duyet_or_huy";
        public const string GarnerSPDT_DLPP_Xoa = GarnerModule + ButtonAction + "spdt_dlpp_xoa";

        //tai san dam bao
        public const string GarnerSPDT_TSDB = GarnerModule + Tab + "spdt_tsdb";
        public const string GarnerSPDT_TSDB_DanhSach = GarnerModule + Table + "spdt_tsdb_danh_sach";
        public const string GarnerSPDT_TSDB_ThemMoi = GarnerModule + ButtonAction + "spdt_tsdb_them_moi";
        public const string GarnerSPDT_TSDB_CapNhat = GarnerModule + ButtonAction + "spdt_tsdb_cap_nhat";
        public const string GarnerSPDT_TSDB_Preview = GarnerModule + ButtonAction + "spdt_tsdb_xem_file";
        public const string GarnerSPDT_TSDB_DownloadFile = GarnerModule + ButtonAction + "spdt_tsdb_download_file";
        public const string GarnerSPDT_TSDB_DeleteFile = GarnerModule + ButtonAction + "spdt_tsdb_xoa_file";
        // Tab Hồ sơ pháp lý
        // TSDB = Hồ sơ pháp lý
        public const string GarnerSPDT_HSPL = GarnerModule + Tab + "spdt_hspl";
        public const string GarnerSPDT_HSPL_DanhSach = GarnerModule + Table + "spdt_hspl_danh_sach";
        public const string GarnerSPDT_HSPL_ThemMoi = GarnerModule + ButtonAction + "spdt_hspl_them_moi";
        public const string GarnerSPDT_HSPL_CapNhat = GarnerModule + ButtonAction + "spdt_hspl_cap_nhat";
        public const string GarnerSPDT_HSPL_Preview = GarnerModule + ButtonAction + "spdt_hspl_xem_file";
        public const string GarnerSPDT_HSPL_DownloadFile = GarnerModule + ButtonAction + "spdt_hspl_download_file";
        public const string GarnerSPDT_HSPL_DeleteFile = GarnerModule + ButtonAction + "spdt_hspl_xoa_file";

        // Tab Tin tức sản phẩm
        // TTSP = Tin tức sản phẩm
        public const string GarnerSPDT_TTSP = GarnerModule + Tab + "spdt_ttsp";
        public const string GarnerSPDT_TTSP_DanhSach = GarnerModule + Table + "spdt_ttsp_danh_sach";
        public const string GarnerSPDT_TTSP_ThemMoi = GarnerModule + ButtonAction + "spdt_ttsp_them_moi";
        public const string GarnerSPDT_TTSP_CapNhat = GarnerModule + ButtonAction + "spdt_ttsp_CapNhat";
        public const string GarnerSPDT_TTSP_PheDuyetOrHuy = GarnerModule + ButtonAction + "spdt_ttsp_phe_duyet_or_huy";
        public const string GarnerSPDT_TTSP_Xoa = GarnerModule + ButtonAction + "spdt_ttsp_xoa";

        // Module Phân phối đầu tư
        // PPDT = phân phối đầu tư
        public const string GarnerMenuPPDT = GarnerModule + Menu + "ppdt";
        public const string GarnerPPDT_DanhSach = GarnerModule + Table + "ppdt_danh_sach";
        public const string GarnerPPDT_ThemMoi = GarnerModule + ButtonAction + "ppdt_them_moi";
        public const string GarnerPPDT_DongOrMo = GarnerModule + ButtonAction + "ppdt_dong_or_mo";
        public const string GarnerPPDT_TrinhDuyet = GarnerModule + ButtonAction + "ppdt_trinh_duyet";
        public const string GarnerPPDT_BatTatShowApp = GarnerModule + ButtonAction + "ppdt_tat_show_app";
        public const string GarnerPPDT_EpicXacMinh = GarnerModule + ButtonAction + "ppdt_epic_xac_minh";
        public const string GarnerPPDT_ThongTinPPDT = GarnerModule + Page + "ppdt_thong_tin_ppdt";

        // Tab Thông tin chung
        public const string GarnerPPDT_ThongTinChung = GarnerModule + Tab + "ppdt_thong_tin_chung";
        public const string GarnerPPDT_ChiTiet = GarnerModule + Form + "ppdt_chi_tiet";
        public const string GarnerPPDT_CapNhat = GarnerModule + ButtonAction + "ppdt_cap_nhat";

        //Tab Tổng quan
        public const string GarnerPPDT_TongQuan = GarnerModule + Tab + "ppdt_tong_quan";
        public const string GarnerPPDT_TongQuan_CapNhat = GarnerModule + ButtonAction + "ppdt_tong_quan_cap_nhat";
        public const string GarnerPPDT_TongQuan_ChonAnh = GarnerModule + ButtonAction + "ppdt_tong_quan_chon_anh";
        public const string GarnerPPDT_TongQuan_ThemToChuc = GarnerModule + ButtonAction + "ppdt_tong_quan_them_to_chuc";
        public const string GarnerPPDT_TongQuan_DanhSach_ToChuc = GarnerModule + Table + "ppdt_tong_quan_danh_sach_to_chuc";
        public const string GarnerPPDT_TongQuan_UploadFile = GarnerModule + ButtonAction + "ppdt_tong_quan_upload_file";
        public const string GarnerPPDT_TongQuan_DanhSach_File = GarnerModule + Table + "ppdt_tong_quan_danh_sach_file";


        // Tab Chính sách
        // Chính sách
        public const string GarnerPPDT_ChinhSach = GarnerModule + Tab + "ppdt_chinh_sach";
        public const string GarnerPPDT_ChinhSach_DanhSach = GarnerModule + Table + "ppdt_chinh_sach_danh_sach";
        public const string GarnerPPDT_ChinhSach_ThemMoi = GarnerModule + ButtonAction + "ppdt_chinh_sach_them_moi";
        public const string GarnerPPDT_ChinhSach_CapNhat = GarnerModule + ButtonAction + "ppdt_chinh_sach_CapNhat";
        public const string GarnerPPDT_ChinhSach_KichHoatOrHuy = GarnerModule + ButtonAction + "ppdt_chinh_sach_kich_hoat_or_huy";
        public const string GarnerPPDT_ChinhSach_BatTatShowApp = GarnerModule + ButtonAction + "ppdt_chinh_sach_bat_tat_show_app";
        public const string GarnerPPDT_ChinhSach_Xoa = GarnerModule + ButtonAction + "ppdt_chinh_sach_xoa";

        public const string GarnerPPDT_KyHan_ThemMoi = GarnerModule + ButtonAction + "ppdt_ky_han_them_moi";
        public const string GarnerPPDT_KyHan_CapNhat = GarnerModule + ButtonAction + "ppdt_ky_han_cap_nhat";
        public const string GarnerPPDT_KyHan_KichHoatOrHuy = GarnerModule + ButtonAction + "ppdt_ky_han_kich_hoat_or_huy";
        public const string GarnerPPDT_KyHan_BatTatShowApp = GarnerModule + ButtonAction + "ppdt_ky_han_bat_tat_show_app";
        public const string GarnerPPDT_KyHan_Xoa = GarnerModule + ButtonAction + "ppdt_ky_han_xoa";

        public const string GarnerPPDT_HopDongMau_ThemMoi = GarnerModule + ButtonAction + "ppdt_hop_dong_mau_them_moi";
        public const string GarnerPPDT_HopDongMau_CapNhat = GarnerModule + ButtonAction + "ppdt_hop_dong_mau_cap_nhat";
        public const string GarnerPPDT_HopDongMau_KichHoatOrHuy = GarnerModule + ButtonAction + "ppdt_hop_dong_mau_kich_hoat_or_huy";
        public const string GarnerPPDT_HopDongMau_BatTatShowApp = GarnerModule + ButtonAction + "ppdt_hop_dong_mau_bat_tat_show_app";
        public const string GarnerPPDT_HopDongMau_Xoa = GarnerModule + ButtonAction + "ppdt_hop_dong_mau_xoa";

        // Tab File chính sách
        public const string GarnerPPDT_FileChinhSach = GarnerModule + Tab + "ppdt_file_chinh_sach";
        public const string GarnerPPDT_FileChinhSach_DanhSach = GarnerModule + Table + "ppdt_file_chinh_sach_danh_sach";
        public const string GarnerPPDT_FileChinhSach_ThemMoi = GarnerModule + ButtonAction + "ppdt_file_chinh_sach_them_moi";
        public const string GarnerPPDT_FileChinhSach_CapNhat = GarnerModule + ButtonAction + "ppdt_file_chinh_sach_cap_nhat";
        public const string GarnerPPDT_FileChinhSach_Xoa = GarnerModule + ButtonAction + "ppdt_file_chinh_sach_xoa";

        // Tab Mẫu hợp đồng
        public const string GarnerPPDT_MauHopDong = GarnerModule + Tab + "ppdt_mau_hop_dong";
        public const string GarnerPPDT_MauHopDong_DanhSach = GarnerModule + Table + "ppdt_mau_hop_dong_danh_sach";
        public const string GarnerPPDT_MauHopDong_ThemMoi = GarnerModule + ButtonAction + "ppdt_mau_hop_dong_them_moi";
        public const string GarnerPPDT_MauHopDong_CapNhat = GarnerModule + ButtonAction + "ppdt_mau_hop_dong_cap_nhat";
        public const string GarnerPPDT_MauHopDong_Xoa = GarnerModule + ButtonAction + "ppdt_mau_hop_dong_xoa";

        public const string Garner_TTCT_BangGia = GarnerModule + Tab + "ppdt_thong_tin_chi_tiet_bg";
        public const string Garner_TTCT_BangGia_DanhSach = GarnerModule + Table + "ppdt_thong_tin_chi_tiet_bg_danh_sach";
        public const string Garner_TTCT_BangGia_ImportExcelBG = GarnerModule + ButtonAction + "ppdt_thong_tin_chi_tiet_bg_import_excel_bg";
        public const string Garner_TTCT_BangGia_DownloadFileMau = GarnerModule + ButtonAction + "ppdt_thong_tin_chi_tiet_bg_download_file_mau";
        public const string Garner_TTCT_BangGia_XoaBangGia = GarnerModule + ButtonAction + "ppdt_thong_tin_chi_tiet_bg_xoa_bang_gia";


        // Tab Hợp đồng phân phối   
        public const string GarnerPPDT_HopDongPhanPhoi = GarnerModule + Tab + "ppdt_hop_dong_phan_phoi";
        public const string GarnerPPDT_HopDongPhanPhoi_DanhSach = GarnerModule + Table + "ppdt_hop_dong_phan_phoi_danh_sach";
        public const string GarnerPPDT_HopDongPhanPhoi_ThemMoi = GarnerModule + ButtonAction + "ppdt_hop_dong_phan_phoi_them_moi";
        public const string GarnerPPDT_HopDongPhanPhoi_CapNhat = GarnerModule + ButtonAction + "ppdt_hop_dong_phan_phoi_cap_nhat";
        public const string GarnerPPDT_HopDongPhanPhoi_Xoa = GarnerModule + ButtonAction + "ppdt_hop_dong_phan_phoi_xoa";

        // Tab Mẫu giao nhận hợp đồng
        public const string GarnerPPDT_MauGiaoNhanHD = GarnerModule + Tab + "ppdt_mau_giao_nhan_hd";
        public const string GarnerPPDT_MauGiaoNhanHD_DanhSach = GarnerModule + Table + "ppdt_mau_giao_nhan_hd_danh_sach";
        public const string GarnerPPDT_MauGiaoNhanHD_ThemMoi = GarnerModule + ButtonAction + "ppdt_mau_giao_nhan_hd_them_moi";
        public const string GarnerPPDT_MauGiaoNhanHD_CapNhat = GarnerModule + ButtonAction + "ppdt_mau_giao_nhan_hd_cap_nhat";
        public const string GarnerPPDT_MauGiaoNhanHD_KichHoat = GarnerModule + ButtonAction + "ppdt_mau_giao_nhan_hd_kich_hoat";
        public const string GarnerPPDT_MauGiaoNhanHD_Xoa = GarnerModule + ButtonAction + "ppdt_mau_giao_nhan_hd_xoa";

        // Menu Quản lý phê duyệt
        // qlpd = Quản lý phê duyệt 
        public const string GarnerMenuQLPD = GarnerModule + Menu + "qlpd";
        // Module Phê duyệt sản phẩm đầu tư
        // PDSPDT = Phê duyệt sản phẩm đầu tư
        public const string GarnerMenuPDSPDT = GarnerModule + Menu + "pdspdt";
        public const string GarnerPDSPTL_DanhSach = GarnerModule + Table + "pdspdt_danh_sach";
        public const string GarnerPDSPTL_PheDuyetOrHuy = GarnerModule + ButtonAction + "pdspdt_phe_duyet";

        // Module Phê duyệt phân phối đầu tư
        // PDPPDT = Phê duyệt phân phối đầu tư
        public const string GarnerMenuPDPPDT = GarnerModule + Menu + "pdppdt";
        public const string GarnerPDPPDT_DanhSach = GarnerModule + Table + "pdppdt_danh_sach";
        public const string GarnerPDPPDT_PheDuyetOrHuy = GarnerModule + ButtonAction + "pdppdt_phe_duyet";

        // Module Phê duyệt yêu cầu tái tục
        // PDPPDT = Phê duyệt yêu cầu tái tục
        public const string GarnerMenuPDYCTT = GarnerModule + Menu + "pdyctt";
        public const string GarnerPDYCTT_DanhSach = GarnerModule + Table + "pdyctt_danh_sach";
        public const string GarnerPDYCTT_PheDuyetOrHuy = GarnerModule + ButtonAction + "pdyctt_phe_duyet";

        // Module Phê duyệt yêu cầu rút vốn
        // PDPPDT = Phê duyệt yêu cầu rút vốn
        public const string GarnerMenuPDYCRV = GarnerModule + Menu + "pdycrv";
        public const string GarnerPDYCRV_DanhSach = GarnerModule + Table + "pdycrv_danh_sach";
        public const string GarnerPDYCRV_PheDuyetOrHuy = GarnerModule + ButtonAction + "pdycrv_phe_duyet";
        public const string GarnerPDYCRV_ChiTietHD = GarnerModule + ButtonAction + "pdycrv_chi_tiet_hd";

        public const string Garner_Menu_BaoCao = GarnerModule + Menu + "bao_cao";

        public const string Garner_BaoCao_QuanTri = GarnerModule + Page + "bao_cao_quan_tri";
        public const string Garner_BaoCao_QuanTri_TCTDauTu = GarnerModule + ButtonAction + "bao_cao_quan_tri_tct_dau_tu";
        public const string Garner_BaoCao_QuanTri_THCKDauTu = GarnerModule + ButtonAction + "bao_cao_quan_tri_thck_dau_tu";
        public const string Garner_BaoCao_QuanTri_THSanPhamTichLuy = GarnerModule + ButtonAction + "bao_cao_quan_tri_thsp_tich_luy";
        public const string Garner_BaoCao_QuanTri_QuanTriTH = GarnerModule + ButtonAction + "bao_cao_quan_tri_quan_tri_th";
        public const string Garner_BaoCao_QuanTri_DauTuBanHo = GarnerModule + ButtonAction + "bao_cao_quan_tri_dau_tu_ban_ho";

        public const string Garner_BaoCao_VanHanh = GarnerModule + Page + "bao_cao_van_hanh";
        public const string Garner_BaoCao_VanHanh_TCTDauTu = GarnerModule + ButtonAction + "bao_cao_van_hanh_tct_dau_tu";
        public const string Garner_BaoCao_VanHanh_THCKDauTu = GarnerModule + ButtonAction + "bao_cao_van_hanh_thck_dau_tu";
        public const string Garner_BaoCao_VanHanh_THSanPhamTichLuy = GarnerModule + ButtonAction + "bao_cao_van_hanh_thsp_tich_luy";
        public const string Garner_BaoCao_VanHanh_QuanTriTH = GarnerModule + ButtonAction + "bao_cao_van_hanh_quan_tri_th";
        public const string Garner_BaoCao_VanHanh_DauTuBanHo = GarnerModule + ButtonAction + "bao_cao_van_hanh_quan_dau_tu_ban_ho";


        public const string Garner_BaoCao_KinhDoanh = GarnerModule + Page + "bao_cao_kinh_doanh";
        public const string Garner_BaoCao_KinhDoanh_TCTDauTu = GarnerModule + ButtonAction + "bao_cao_kinh_doanh_tct_dau_tu";
        public const string Garner_BaoCao_KinhDoanh_THCKDauTu = GarnerModule + ButtonAction + "bao_cao_kinh_doanh_thck_dau_tu";
        public const string Garner_BaoCao_KinhDoanh_THSanPhamTichLuy = GarnerModule + ButtonAction + "bao_cao_kinh_doanh_thsp_tich_luy";
        public const string Garner_BaoCao_KinhDoanh_QuanTriTH = GarnerModule + ButtonAction + "bao_cao_kinh_doanh_quan_tri_th";
        public const string Garner_BaoCao_KinhDoanh_DauTuBanHo = GarnerModule + ButtonAction + "bao_cao_kinh_doanh_quan_dau_tu_ban_ho";


        public const string Garner_Menu_TruyVan = GarnerModule + Menu + "truy_van";

        public const string Garner_TruyVan_ThuTien_NganHang = GarnerModule + Page + "truy_van_thu_tien_ngan_hang";
        public const string Garner_TruyVan_ChiTien_NganHang = GarnerModule + Page + "truy_van_chi_tien_ngan_hang";

        #endregion

        //support
        #region support
        private const string SupportModule = "support.";
        public const string SupportWeb = SupportModule + Web;

        #endregion

        //euser
        #region user
        private const string UserModule = "user.";
        public const string UserWeb = UserModule + Web;

        // PHÂN QUYỀN WEBSTIE
        public const string UserPhanQuyen_Websites = UserModule + Menu + "phan_quyen_websites";
        public const string UserPhanQuyen_Website_CauHinhVaiTro = UserModule + ButtonAction + "phan_quyen_website_cau_hinh_vaitro";
        public const string UserPhanQuyen_Website_ThemVaiTro = UserModule + ButtonAction + "phan_quyen_website_them_vaitro";
        public const string UserPhanQuyen_Website_CapNhatVaiTro = UserModule + ButtonAction + "phan_quyen_website_cap_nhat_vaitro";

        // QUẢN LÝ TÀI KHOẢN, CẤU HÌNH QUYỀN CHO TÀI KHOẢN
        public const string UserQL_TaiKhoan = UserModule + Menu + "ql_taikhoan";
        public const string UserQL_TaiKhoan_ThemMoi = UserModule + ButtonAction + "ql_taikhoan_them_moi";
        public const string UserQL_TaiKhoan_CapNhatVaiTro = UserModule + ButtonAction + "ql_taikhoan_cap_nhat_vai_tro";
        public const string UserQL_TaiKhoan_SetMatKhau = UserModule + ButtonAction + "ql_taikhoan_set_matkhau";
        public const string UserQL_TaiKhoan_ChiTiet = UserModule + ButtonAction + "ql_taikhoan_chitiet";
        public const string UserQL_TaiKhoan_Xoa = UserModule + ButtonAction + "ql_taikhoan_xoa";

        //  CẤU HÌNH QUYỀN CHO TÀI KHOẢN
        public const string UserQL_TaiKhoan_CauHinhQuyen = UserModule + ButtonAction + "ql_taikhoan_cau_hinh_quyen";
        public const string UserQL_TaiKhoan_PhanQuyenWebsite = UserModule + ButtonAction + "ql_taikhoan_phan_quyen_website";
        public const string UserQL_TaiKhoan_PhanQuyenChiTietWebsite = UserModule + ButtonAction + "ql_taikhoan_phan_quyen_chi_tiet_website";

        #endregion

        //euser

        #region Company_shares
        #endregion

        #region RealState
        private const string RealStateModule = "real_state.";
        public const string RealStateWeb = RealStateModule + Web;

        // Menu Dashboard
        public const string RealStatePageDashboard = RealStateModule + Page + "dashboard";

        //Menu Cài đặt
        public const string RealStateMenuSetting = RealStateModule + Menu + "setting";

        // Module chủ đầu tư
        public const string RealStateMenuChuDT = RealStateModule + Menu + "chu_dau_tu";
        public const string RealStateChuDT_ThongTinChuDauTu = RealStateModule + Page + "chu_dt_thong_tin_chu_dau_tu";
        public const string RealStateChuDT_DanhSach = RealStateModule + Table + "chu_dt_danh_sach";
        public const string RealStateChuDT_ThemMoi = RealStateModule + ButtonAction + "chu_dt_them_moi";
        public const string RealStateChuDT_KichHoatOrHuy = RealStateModule + ButtonAction + "chu_dt_kich_hoat_or_huy";
        public const string RealStateChuDT_Xoa = RealStateModule + ButtonAction + "chu_dt_xoa";
        // Tab thông tin chung
        public const string RealStateChuDT_ThongTinChung = RealStateModule + Tab + "chu_dt_thong_tin_chung";
        public const string RealStateChuDT_ChiTiet = RealStateModule + Form + "chu_dt_chi_tiet";
        public const string RealStateChuDT_CapNhat = RealStateModule + ButtonAction + "chu_dt_cap_nhat";

        // Module Đại lý
        public const string RealStateMenuDaiLy = RealStateModule + Menu + "dai_ly";
        public const string RealStateDaiLy_DanhSach = RealStateModule + Table + "dai_ly_danh_sach";
        public const string RealStateDaiLy_ThemMoi = RealStateModule + ButtonAction + "dai_ly_them_moi";
        public const string RealStateDaiLy_KichHoatOrHuy = RealStateModule + ButtonAction + "dai_ly_kich_hoat_or_huy";
        public const string RealStateDaiLy_Xoa = RealStateModule + ButtonAction + "dai_ly_xoa";
        public const string RealStateDaiLy_ThongTinDaiLy = RealStateModule + Page + "dai_ly_thong_tin_dai_ly";
        // Tab thông tin chung
        public const string RealStateDaiLy_ThongTinChung = RealStateModule + Tab + "dai_ly_thong_tin_chung";
        public const string RealStateDaiLy_ChiTiet = RealStateModule + Form + "dai_ly_chi_tiet";


        // Module Chính sách phân phối
        public const string RealStateMenuCSPhanPhoi = RealStateModule + Menu + "cs_phan_phoi";
        public const string RealStateCSPhanPhoi_DanhSach = RealStateModule + Table + "cs_phan_phoi_danh_sach";
        public const string RealStateCSPhanPhoi_ThemMoi = RealStateModule + ButtonAction + "cs_phan_phoi_them_moi";
        public const string RealStateCSPhanPhoi_CapNhat = RealStateModule + ButtonAction + "cs_phan_phoi_cap_nhat";
        public const string RealStateCSPhanPhoi_DoiTrangThai = RealStateModule + ButtonAction + "cs_phan_phoi_doi_trang_thai";
        public const string RealStateCSPhanPhoi_Xoa = RealStateModule + ButtonAction + "cs_phan_phoi_xoa";

        // Module Chính sách bán hàng
        public const string RealStateMenuCSBanHang = RealStateModule + Menu + "cs_ban_hang";
        public const string RealStateCSBanHang_DanhSach = RealStateModule + Table + "cs_ban_hang_danh_sach";
        public const string RealStateCSBanHang_ThemMoi = RealStateModule + ButtonAction + "cs_ban_hang_them_moi";
        public const string RealStateCSBanHang_CapNhat = RealStateModule + ButtonAction + "cs_ban_hang_cap_nhat";
        public const string RealStateCSBanHang_DoiTrangThai = RealStateModule + ButtonAction + "cs_ban_hang_doi_trang_thai";
        public const string RealStateCSBanHang_Xoa = RealStateModule + ButtonAction + "cs_ban_hang_xoa";

        // Cấu trúc hợp đồng giao dịch
        public const string RealStateMenuCTMaHDGiaoDich = RealStateModule + Menu + "ct_ma_hd_giao_dich";
        public const string RealStateCTMaHDGiaoDich_DanhSach = RealStateModule + Table + "ct_ma_hd_giao_dich_danh_sach";
        public const string RealStateCTMaHDGiaoDich_ThemMoi = RealStateModule + ButtonAction + "ct_ma_hd_giao_dich_them_moi";
        public const string RealStateCTMaHDGiaoDich_CapNhat = RealStateModule + ButtonAction + "ct_ma_hd_giao_dich_cap_nhat";
        public const string RealStateCTMaHDGiaoDich_DoiTrangThai = RealStateModule + ButtonAction + "ct_ma_hd_giao_dich_doi_trang_thai";
        public const string RealStateCTMaHDGiaoDich_Xoa = RealStateModule + ButtonAction + "ct_ma_hd_giao_dich_xoa";

        // Module cấu trúc hợp đồng cọc
        public const string RealStateMenuCTMaHDCoc = RealStateModule + Menu + "ct_ma_hd_coc";
        public const string RealStateCTMaHDCoc_DanhSach = RealStateModule + Table + "ct_ma_hd_coc_danh_sach";
        public const string RealStateCTMaHDCoc_ThemMoi = RealStateModule + ButtonAction + "ct_ma_hd_coc_them_moi";
        public const string RealStateCTMaHDCoc_CapNhat = RealStateModule + ButtonAction + "ct_ma_hd_coc_cap_nhat";
        public const string RealStateCTMaHDCoc_DoiTrangThai = RealStateModule + ButtonAction + "ct_ma_hd_coc_doi_trang_thai";
        public const string RealStateCTMaHDCoc_Xoa = RealStateModule + ButtonAction + "ct_ma_hd_coc_xoa";

        // Module mẫu hợp đồng chủ đầu tư
        public const string RealStateMenuMauHDCDT = RealStateModule + Menu + "mau_hdcdt";
        public const string RealStateMauHDCDT_DanhSach = RealStateModule + Table + "mau_hdcdt_danh_sach";
        public const string RealStateMauHDCDT_ThemMoi = RealStateModule + ButtonAction + "mau_hdcdt_them_moi";
        public const string RealStateMauHDCDT_CapNhat = RealStateModule + ButtonAction + "mau_hdcdt_cap_nhat";
        public const string RealStateMauHDCDT_DoiTrangThai = RealStateModule + ButtonAction + "mau_hdcdt_doi_trang_thai";
        public const string RealStateMauHDCDT_TaiFileDoanhNghiep = RealStateModule + ButtonAction + "mau_hdcdt_tai_file_doanh_nghiep";
        public const string RealStateMauHDCDT_TaiFileCaNhan = RealStateModule + ButtonAction + "mau_hdcdt_tai_file_ca_nhan";
        public const string RealStateMauHDCDT_Xoa = RealStateModule + ButtonAction + "mau_hdcdt_xoa";

        // Module mẫu hợp đồng đại lý
        public const string RealStateMenuMauHDDL = RealStateModule + Menu + "mau_hddl";
        public const string RealStateMauHDDL_DanhSach = RealStateModule + Table + "mau_hddl_danh_sach";
        public const string RealStateMauHDDL_ThemMoi = RealStateModule + ButtonAction + "mau_hddl_them_moi";
        public const string RealStateMauHDDL_CapNhat = RealStateModule + ButtonAction + "mau_hddl_cap_nhat";
        public const string RealStateMauHDDL_DoiTrangThai = RealStateModule + ButtonAction + "mau_hddl_doi_trang_thai";
        public const string RealStateMauHDDL_TaiFileDoanhNghiep = RealStateModule + ButtonAction + "mau_hddl_tai_file_doanh_nghiep";
        public const string RealStateMauHDDL_TaiFileCaNhan = RealStateModule + ButtonAction + "mau_hddl_tai_file_ca_nhan";
        public const string RealStateMauHDDL_Xoa = RealStateModule + ButtonAction + "mau_hddl_xoa";

        // Thông báo hệ thống
        public const string RealStateThongBaoHeThong = RealStateModule + Menu + "thong_bao_he_thong";

        // Menu quản lý dự án 
        public const string RealStateMenuProjectManager = RealStateModule + Menu + "project_manager";

        public const string RealStateMenuProjectOverview = RealStateModule + Menu + "project_overview";
        public const string RealStateProjectOverview_DanhSach = RealStateModule + Table + "project_overview_danh_sach";
        public const string RealStateProjectOverview_ThemMoi = RealStateModule + ButtonAction + "project_overview_them_moi";
        public const string RealStateProjectOverview_Xoa = RealStateModule + ButtonAction + "project_overview_xoa";

        public const string RealStateProjectOverview_ThongTinProjectOverview = RealStateModule + Page + "project_overview_thong_tin_project_overview";
        public const string RealStateProjectOverview_TrinhDuyet = RealStateModule + ButtonAction + "project_overview_trinh_duyet";
        public const string RealStateProjectOverview_PheDuyet = RealStateModule + ButtonAction + "project_overview_phe_duyet";

        // Tab  thông tin chung
        public const string RealStateProjectOverview_ThongTinChung = RealStateModule + Tab + "project_overview_thong_tin_chung";
        public const string RealStateProjectOverview_ThongTinChung_ChiTiet = RealStateModule + Form + "project_overview_thong_tin_chung_chi_tiet";
        public const string RealStateProjectOverview_ThongTinChung_CapNhat = RealStateModule + ButtonAction + "project_overview_thong_tin_chung_cap_nhat";

        // Tab Mô tả dự án
        public const string RealStateProjectOverview_MoTa = RealStateModule + Tab + "project_overview_mo_ta";
        public const string RealStateProjectOverview_MoTa_ChiTiet = RealStateModule + Form + "project_overview_mo_ta_chi_tiet";
        public const string RealStateProjectOverview_MoTa_CapNhat = RealStateModule + ButtonAction + "project_overview_mo_ta_cap_nhat";

        // Tab Tiện ích
        public const string RealStateProjectOverview_TienIch = RealStateModule + Tab + "project_overview_tien_ich";
        public const string RealStateProjectOverview_TienIchHeThong = RealStateModule + Table + "project_overview_tien_ich_ht";
        public const string RealStateProjectOverview_TienIchHeThong_DanhSach = RealStateModule + Table + "project_overview_tien_ich_ht_ds";
        public const string RealStateProjectOverview_TienIchHeThong_QuanLy = RealStateModule + ButtonAction + "project_overview_tien_ich_ht_quan_ly";

        public const string RealStateProjectOverview_TienIchKhac = RealStateModule + Table + "project_overview_tien_ich_khac";
        public const string RealStateProjectOverview_TienIchKhac_DanhSach = RealStateModule + Table + "project_overview_tien_ich_khac_ds";
        public const string RealStateProjectOverview_TienIchKhac_ThemMoi = RealStateModule + ButtonAction + "project_overview_tien_ich_khac_them_moi";
        public const string RealStateProjectOverview_TienIchKhac_CapNhat = RealStateModule + ButtonAction + "project_overview_tien_ich_khac_cap_nhat";
        public const string RealStateProjectOverview_TienIchKhac_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_tien_ich_khac_doi_trang_thai";
        public const string RealStateProjectOverview_TienIchKhac_DoiTrangThaiNoiBat = RealStateModule + ButtonAction + "project_overview_tien_ich_khac_doi_trang_thai_noi_bat";
        public const string RealStateProjectOverview_TienIchKhac_Xoa = RealStateModule + ButtonAction + "project_overview_tien_ich_khac_xoa";


        public const string RealStateProjectOverview_TienIchMinhHoa = RealStateModule + Table + "project_overview_tien_ich_minh_hoa";
        public const string RealStateProjectOverview_TienIchMinhHoa_DanhSach = RealStateModule + Table + "project_overview_tien_ich_minh_hoa_ds";
        public const string RealStateProjectOverview_TienIchMinhHoa_ThemMoi = RealStateModule + ButtonAction + "project_overview_tien_ich_minh_hoa_them_moi";
        public const string RealStateProjectOverview_TienIchMinhHoa_CapNhat = RealStateModule + ButtonAction + "project_overview_tien_ich_minh_hoa_cap_nhat";
        public const string RealStateProjectOverview_TienIchMinhHoa_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_tien_ich_minh_hoa_doi_trang_thai";
        public const string RealStateProjectOverview_TienIchMinhHoa_Xoa = RealStateModule + ButtonAction + "project_overview_tien_ich_minh_hoa_xoa";

        // Tab cấu trúc dự án
        public const string RealStateProjectOverview_CauTruc = RealStateModule + Tab + "project_overview_cau_truc";
        public const string RealStateProjectOverview_CauTruc_DanhSach = RealStateModule + Table + "project_overview_cau_truc_ds";
        public const string RealStateProjectOverview_CauTruc_Them = RealStateModule + ButtonAction + "project_overview_cau_truc_them";
        public const string RealStateProjectOverview_CauTruc_Sua = RealStateModule + ButtonAction + "project_overview_cau_truc_sua";
        public const string RealStateProjectOverview_CauTruc_Xoa = RealStateModule + ButtonAction + "project_overview_cau_truc_xoa";

        // Tab hình ảnh
        public const string RealStateProjectOverview_HinhAnhDuAn = RealStateModule + Tab + "project_overview_hinh_anh_du_an";

        public const string RealStateProjectOverview_HinhAnh = RealStateModule + Tab + "project_overview_hinh_anh";
        public const string RealStateProjectOverview_HinhAnh_ChiTiet = RealStateModule + Form + "project_overview_hinh_anh_chi_tiet";
        public const string RealStateProjectOverview_HinhAnh_ThemMoi = RealStateModule + ButtonAction + "project_overview_hinh_anh_them_moi";
        public const string RealStateProjectOverview_HinhAnh_CapNhat = RealStateModule + ButtonAction + "project_overview_hinh_anh_cap_nhat";
        public const string RealStateProjectOverview_HinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_hinh_anh_doi_trang_thai";
        public const string RealStateProjectOverview_HinhAnh_Xoa = RealStateModule + ButtonAction + "project_overview_hinh_anh_xoa";

        public const string RealStateProjectOverview_NhomHinhAnh = RealStateModule + Tab + "project_overview_nhom_hinh_anh";
        public const string RealStateProjectOverview_NhomHinhAnh_ChiTiet = RealStateModule + Form + "project_overview_nhom_hinh_anh_chi_tiet";
        public const string RealStateProjectOverview_NhomHinhAnh_ThemMoi = RealStateModule + ButtonAction + "project_overview_nhom_hinh_anh_them_moi";
        public const string RealStateProjectOverview_NhomHinhAnh_CapNhat = RealStateModule + ButtonAction + "project_overview_nhom_hinh_anh_cap_nhat";
        public const string RealStateProjectOverview_NhomHinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_nhom_hinh_anh_doi_trang_thai";
        public const string RealStateProjectOverview_NhomHinhAnh_Xoa = RealStateModule + ButtonAction + "project_overview_nhom_hinh_anh_xoa";

        // Tab Chính sách dự án
        public const string RealStateProjectOverview_ChinhSach = RealStateModule + Tab + "project_overview_chinh_sach";
        public const string RealStateProjectOverview_ChinhSach_DanhSach = RealStateModule + Table + "project_overview_chinh_sach_ds";
        public const string RealStateProjectOverview_ChinhSach_ThemMoi = RealStateModule + ButtonAction + "project_overview_chinh_sach_them_moi";
        public const string RealStateProjectOverview_ChinhSach_CapNhat = RealStateModule + ButtonAction + "project_overview_chinh_sach_cap_nhat";
        public const string RealStateProjectOverview_ChinhSach_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_chinh_sach_doi_trang_thai";
        public const string RealStateProjectOverview_ChinhSach_Xoa = RealStateModule + ButtonAction + "project_overview_chinh_sach_xoa";

        // Tab hồ sơ pháp lý
        public const string RealStateProjectOverview_HoSo = RealStateModule + Tab + "project_overview_ho_so";
        public const string RealStateProjectOverview_HoSo_DanhSach = RealStateModule + Table + "project_overview_ho_so_ds";
        public const string RealStateProjectOverview_HoSo_ThemMoi = RealStateModule + ButtonAction + "project_overview_ho_so_them_moi";
        public const string RealStateProjectOverview_HoSo_CapNhat = RealStateModule + ButtonAction + "project_overview_ho_so_cap_nhat";
        public const string RealStateProjectOverview_HoSo_XemFile = RealStateModule + ButtonAction + "project_overview_ho_so_xem_file";
        public const string RealStateProjectOverview_HoSo_TaiXuong = RealStateModule + ButtonAction + "project_overview_ho_so_tai_xuong";
        public const string RealStateProjectOverview_HoSo_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_ho_so_doi_trang_thai";
        public const string RealStateProjectOverview_HoSo_Xoa = RealStateModule + ButtonAction + "project_overview_ho_so_xoa";

        // Tab Facebook
        public const string RealStateProjectOverview_Facebook_Post = RealStateModule + Tab + "project_overview_facebook_post";
        public const string RealStateProjectOverview_Facebook_Post_DanhSach = RealStateModule + Table + "project_overview_facebook_post_ds";
        public const string RealStateProjectOverview_Facebook_Post_ThemMoi = RealStateModule + ButtonAction + "project_overview_facebook_post_them_moi";
        public const string RealStateProjectOverview_Facebook_Post_CapNhat = RealStateModule + ButtonAction + "project_overview_facebook_post_cap_nhat";
        public const string RealStateProjectOverview_Facebook_Post_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_facebook_post_doi_trang_thai";
        public const string RealStateProjectOverview_Facebook_Post_Xoa = RealStateModule + ButtonAction + "project_overview_facebook_post_xoa";

        // Tab Chia sẻ dự án
        public const string RealStateProjectOverview_ChiaSeDuAn = RealStateModule + Tab + "project_overview_chia_se_du_an";
        public const string RealStateProjectOverview_ChiaSeDuAn_DanhSach = RealStateModule + Table + "project_overview_chia_se_du_an_ds";
        public const string RealStateProjectOverview_ChiaSeDuAn_ThemMoi = RealStateModule + ButtonAction + "project_overview_chia_se_du_an_them_moi";
        public const string RealStateProjectOverview_ChiaSeDuAn_CapNhat = RealStateModule + ButtonAction + "project_overview_chia_se_du_an_cap_nhat";
        public const string RealStateProjectOverview_ChiaSeDuAn_DoiTrangThai = RealStateModule + ButtonAction + "project_overview_chia_se_du_an_doi_trang_thai";
        public const string RealStateProjectOverview_ChiaSeDuAn_Xoa = RealStateModule + ButtonAction + "project_overview_chia_se_du_an_xoa";

        // Bảng hàng dự án
        public const string RealStateMenuProjectList = RealStateModule + Menu + "project_list";
        public const string RealStateMenuProjectList_DanhSach = RealStateModule + Table + "project_list_danh_sach";

        public const string RealStateMenuProjectListDetail = RealStateModule + Page + "project_list_detail";
        public const string RealStateMenuProjectListDetail_DanhSach = RealStateModule + Table + "project_list_detail_danh_sach";
        public const string RealStateMenuProjectListDetail_ThemMoi = RealStateModule + ButtonAction + "project_list_detail_them_moi";
        public const string RealStateMenuProjectListDetail_UploadFile = RealStateModule + ButtonAction + "project_list_detail_upload_file";
        public const string RealStateMenuProjectListDetail_TaiFileMau = RealStateModule + ButtonAction + "project_list_detail_tai_file_mau";
        public const string RealStateMenuProjectListDetail_KhoaCan = RealStateModule + ButtonAction + "project_list_detail_khoa_can";
        public const string RealStateMenuProjectListDetail_NhanBan = RealStateModule + ButtonAction + "project_list_detail_nhan_ban";

        public const string RealStateMenuProjectListDetail_ChiTiet = RealStateModule + Page + "project_list_detail_chi_tiet";

        // Tab Thông tin chung - Bảng hàng
        public const string RealStateMenuProjectListDetail_ThongTinChung = RealStateModule + Tab + "project_list_thong_tin_chung";
        public const string RealStateMenuProjectListDetail_ThongTinChung_ChiTiet = RealStateModule + Form + "project_list_thong_tin_chung_chi_tiet";
        public const string RealStateMenuProjectListDetail_ThongTinChung_CapNhat = RealStateModule + ButtonAction + "project_list_thong_tin_chung_cap_nhat";

        // Tab Chính sách ưu đãi CĐT - Bảng hàng
        public const string RealStateMenuProjectListDetail_ChinhSach = RealStateModule + Tab + "project_list_chinh_sach";
        public const string RealStateMenuProjectListDetail_ChinhSach_ChiTiet = RealStateModule + Form + "project_list_chinh_sach_chi_tiet";
        public const string RealStateMenuProjectListDetail_ChinhSach_CapNhat = RealStateModule + ButtonAction + "project_list_chinh_sach_cap_nhat";
        public const string RealStateMenuProjectListDetail_ChinhSach_DoiTrangThai = RealStateModule + ButtonAction + "project_list_chinh_sach_doi_trang_thai";

        // Tab Tiện ích - Bảng hàng
        public const string RealStateMenuProjectListDetail_TienIch = RealStateModule + Tab + "project_list_tien_ich";
        public const string RealStateMenuProjectListDetail_TienIch_ChiTiet = RealStateModule + Form + "project_list_tien_ich_chi_tiet";
        public const string RealStateMenuProjectListDetail_TienIch_CapNhat = RealStateModule + ButtonAction + "project_list_tien_ich_cap_nhat";
        public const string RealStateMenuProjectListDetail_TienIch_DoiTrangThai = RealStateModule + ButtonAction + "project_list_tien_ich_doi_trang_thai";

        // Tab Vật liệu - Bảng hàng
        public const string RealStateMenuProjectListDetail_VatLieu = RealStateModule + Tab + "project_list_vat_lieu";
        public const string RealStateMenuProjectListDetail_VatLieu_ChiTiet = RealStateModule + Form + "project_list_vat_lieu_chi_tiet";
        public const string RealStateMenuProjectListDetail_VatLieu_CapNhat = RealStateModule + ButtonAction + "project_list_vat_lieu_cap_nhat";

        // Tab Sơ đồ thiết kế - Bảng hàng
        public const string RealStateMenuProjectListDetail_SoDoTK = RealStateModule + Tab + "project_list_so_do_tk";
        public const string RealStateMenuProjectListDetail_SoDoTK_ChiTiet = RealStateModule + Form + "project_list_so_do_tk_chi_tiet";
        public const string RealStateMenuProjectListDetail_SoDoTK_CapNhat = RealStateModule + ButtonAction + "project_list_so_do_tk_cap_nhat";

        // Tab Hình ảnh - Bảng hàng
        public const string RealStateProjectListDetail_HinhAnhDuAn = RealStateModule + Tab + "project_list_detail_hinh_anh_du_an";

        public const string RealStateProjectListDetail_HinhAnh = RealStateModule + Tab + "project_list_detail_hinh_anh";
        public const string RealStateProjectListDetail_HinhAnh_ChiTiet = RealStateModule + Form + "project_list_detail_hinh_anh_chi_tiet";
        public const string RealStateProjectListDetail_HinhAnh_ThemMoi = RealStateModule + ButtonAction + "project_list_detail_hinh_anh_them_moi";
        public const string RealStateProjectListDetail_HinhAnh_CapNhat = RealStateModule + ButtonAction + "project_list_detail_hinh_anh_cap_nhat";
        public const string RealStateProjectListDetail_HinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "project_list_detail_hinh_anh_doi_trang_thai";
        public const string RealStateProjectListDetail_HinhAnh_Xoa = RealStateModule + ButtonAction + "project_list_detail_hinh_anh_xoa";

        public const string RealStateProjectListDetail_NhomHinhAnh = RealStateModule + Tab + "project_list_detail_nhom_hinh_anh";
        public const string RealStateProjectListDetail_NhomHinhAnh_ChiTiet = RealStateModule + Form + "project_list_detail_nhom_hinh_anh_chi_tiet";
        public const string RealStateProjectListDetail_NhomHinhAnh_ThemMoi = RealStateModule + ButtonAction + "project_list_detail_nhom_hinh_anh_them_moi";
        public const string RealStateProjectListDetail_NhomHinhAnh_CapNhat = RealStateModule + ButtonAction + "project_list_detail_nhom_hinh_anh_cap_nhat";
        public const string RealStateProjectListDetail_NhomHinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "project_list_detail_nhom_hinh_anh_doi_trang_thai";
        public const string RealStateProjectListDetail_NhomHinhAnh_Xoa = RealStateModule + ButtonAction + "project_list_detail_nhom_hinh_anh_xoa";

        // Tab Lịch sử - Bảng hàng
        public const string RealStateProjectListDetail_LichSu = RealStateModule + Tab + "project_list_detail_lich_su";
        public const string RealStateProjectListDetail_LichSu_DanhSach = RealStateModule + Table + "project_list_detail_lich_su_danh_sach";

        // Menu Phân phối
        public const string RealStateMenuPhanPhoi = RealStateModule + Menu + "phan_phoi";
        public const string RealStatePhanPhoi_DanhSach = RealStateModule + Table + "phan_phoi_danh_sach";
        public const string RealStatePhanPhoi_ThemMoi = RealStateModule + ButtonAction + "phan_phoi_them_moi";
        public const string RealStatePhanPhoi_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_doi_trang_thai";
        public const string RealStatePhanPhoi_Xoa = RealStateModule + ButtonAction + "phan_phoi_xoa";
        public const string RealStatePhanPhoi_ChiTiet = RealStateModule + Page + "phan_phoi_chi_tiet";
        // Tab Thông tin chung - Bảng hàng
        public const string RealStateMenuPhanPhoi_ThongTinChung = RealStateModule + Tab + "phan_phoi_thong_tin_chung";
        public const string RealStateMenuPhanPhoi_ThongTinChung_ChiTiet = RealStateModule + Form + "phan_phoi_thong_tin_chung_chi_tiet";
        public const string RealStateMenuPhanPhoi_ThongTinChung_CapNhat = RealStateModule + ButtonAction + "phan_phoi_thong_tin_chung_cap_nhat";
        public const string RealStateMenuPhanPhoi_ThongTinChung_TrinhDuyet = RealStateModule + ButtonAction + "phan_phoi_thong_tin_chung_trinh_duyet";
        public const string RealStateMenuPhanPhoi_ThongTinChung_PheDuyet = RealStateModule + ButtonAction + "phan_phoi_thong_tin_chung_phe_duyet";

        // Tab Danh sách sản phẩm
        public const string RealStatePhanPhoi_DSSP = RealStateModule + Tab + "phan_phoi_dssp";
        public const string RealStatePhanPhoi_DSSP_ThemMoi = RealStateModule + Table + "phan_phoi_dssp_them_moi";
        public const string RealStatePhanPhoi_DSSP_DanhSach = RealStateModule + Table + "phan_phoi_dssp_ds";
        public const string RealStatePhanPhoi_DSSP_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_dssp_doi_trang_thai";
        public const string RealStatePhanPhoi_DSSP_Xoa = RealStateModule + ButtonAction + "phan_phoi_dssp_xoa";
        public const string RealStatePhanPhoi_DSSP_ChiTiet = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet";

        // Tab Thông tin chung - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_thong_tin_chung";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_thong_tin_chung_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_thong_tin_chung_cap_nhat";

        // Tab Chính sách ưu đãi CĐT - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_chinh_sach";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_chinh_sach_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_chinh_sach_cap_nhat";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_chinh_sach_doi_trang_thai";

        // Tab Tiện ích - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_TienIch = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_tien_ich";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_TienIch_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_tien_ich_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_TienIch_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_tien_ich_cap_nhat";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_TienIch_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_tien_ich_doi_trang_thai";

        // Tab Vật liệu - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_VatLieu = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_vat_lieu";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_vat_lieu_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_vat_lieu_cap_nhat";

        // Tab Sơ đồ thiết kế - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_so_do_tk";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_so_do_tk_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_so_do_tk_cap_nhat";

        // Tab Hình ảnh - Chi tiết căn hộ phân phối
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_du_an";

        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_detail_hinh_anh";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ThemMoi = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_them_moi";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_cap_nhat";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_doi_trang_thai";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_Xoa = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_hinh_anh_xoa";

        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ChiTiet = RealStateModule + Form + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_chi_tiet";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ThemMoi = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_them_moi";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_CapNhat = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_cap_nhat";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_doi_trang_thai";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_Xoa = RealStateModule + ButtonAction + "phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_xoa";

        // Tab Lịch sử - Bảng hàng
        public const string RealStatePhanPhoi_DSSP_ChiTiet_LichSu = RealStateModule + Tab + "phan_phoi_dssp_chi_tiet_detail_lich_su";
        public const string RealStatePhanPhoi_DSSP_ChiTiet_LichSu_DanhSach = RealStateModule + Table + "phan_phoi_dssp_chi_tiet_detail_lich_su_danh_sach";


        // Tab Chính sách phân phối
        public const string RealStatePhanPhoi_ChinhSach = RealStateModule + Tab + "phan_phoi_chinh_sach";
        public const string RealStatePhanPhoi_ChinhSach_DanhSach = RealStateModule + Table + "phan_phoi_chinh_sach_ds";
        public const string RealStatePhanPhoi_ChinhSach_ThemMoi = RealStateModule + ButtonAction + "phan_phoi_chinh_sach_them_moi";
        public const string RealStatePhanPhoi_ChinhSach_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_chinh_sach_doi_trang_thai";
        public const string RealStatePhanPhoi_ChinhSach_Xoa = RealStateModule + ButtonAction + "phan_phoi_chinh_sach_xoa";
        public const string RealStatePhanPhoi_ChinhSach_ChiTiet = RealStateModule + ButtonAction + "phan_phoi_chinh_sach_chi_tiet";

        // Tab mẫu biểu hợp đồng
        public const string RealStatePhanPhoi_MauBieu = RealStateModule + Tab + "phan_phoi_mau_bieu";
        public const string RealStatePhanPhoi_MauBieu_DanhSach = RealStateModule + Table + "phan_phoi_mau_bieu_ds";
        public const string RealStatePhanPhoi_MauBieu_ThemMoi = RealStateModule + ButtonAction + "phan_phoi_mau_bieu_them_moi";
        public const string RealStatePhanPhoi_MauBieu_XuatWord = RealStateModule + ButtonAction + "phan_phoi_mau_bieu_xuat_word";
        public const string RealStatePhanPhoi_MauBieu_XuatPdf = RealStateModule + ButtonAction + "phan_phoi_mau_bieu_xuat_pdf";
        public const string RealStatePhanPhoi_MauBieu_ChinhSua = RealStateModule + ButtonAction + "phan_phoi_mau_bieu_chinh_sua";
        public const string RealStatePhanPhoi_MauBieu_DoiTrangThai = RealStateModule + ButtonAction + "phan_phoi_mau_bieu_doi_trang_thai";

        // Menu Mở bán
        public const string RealStateMenuMoBan = RealStateModule + Menu + "mo_ban";
        public const string RealStateMoBan_DanhSach = RealStateModule + Table + "mo_ban_danh_sach";
        public const string RealStateMoBan_ThemMoi = RealStateModule + ButtonAction + "mo_ban_them_moi";
        public const string RealStateMoBan_DoiTrangThai = RealStateModule + ButtonAction + "mo_ban_doi_trang_thai";
        public const string RealStateMoBan_Xoa = RealStateModule + ButtonAction + "mo_ban_xoa";
        public const string RealStateMoBan_DungBan = RealStateModule + ButtonAction + "mo_ban_mau_dung_ban";
        public const string RealStateMoBan_DoiNoiBat = RealStateModule + ButtonAction + "mo_ban_mau_bieu_doi_mo_ban";
        public const string RealStateMoBan_DoiShowApp = RealStateModule + ButtonAction + "mo_ban_mau_bieu_doi_show_app";

        public const string RealStateMoBan_ChiTiet = RealStateModule + Page + "mo_ban_chi_tiet";

        // Tab Thông tin chung - Mở bán
        public const string RealStateMoBan_ThongTinChung = RealStateModule + Tab + "mo_ban_thong_tin_chung";
        public const string RealStateMoBan_ThongTinChung_ChiTiet = RealStateModule + Form + "mo_ban_thong_tin_chung_chi_tiet";
        public const string RealStateMoBan_ThongTinChung_CapNhat = RealStateModule + ButtonAction + "mo_ban_thong_tin_chung_cap_nhat";
        public const string RealStateMoBan_ThongTinChung_TrinhDuyet = RealStateModule + ButtonAction + "mo_ban_thong_tin_chung_trinh_duyet";
        public const string RealStateMoBan_ThongTinChung_PheDuyet = RealStateModule + ButtonAction + "mo_ban_thong_tin_chung_phe_duyet";

        // Tab Danh sách sản phẩm - Mở bán
        public const string RealStateMoBan_DSSP = RealStateModule + Tab + "mo_ban_dssp";
        public const string RealStateMoBan_DSSP_DanhSach = RealStateModule + Table + "mo_ban_dssp_ds";
        public const string RealStateMoBan_DSSP_Them = RealStateModule + ButtonAction + "mo_ban_dssp_them_moi";
        public const string RealStateMoBan_DSSP_Xoa = RealStateModule + ButtonAction + "mo_ban_dssp_xoa";
        public const string RealStateMoBan_DSSP_DoiTrangThai = RealStateModule + ButtonAction + "mo_ban_dssp_doi_trang_thai";
        public const string RealStateMoBan_DSSP_DoiShowApp = RealStateModule + ButtonAction + "mo_ban_dssp_doi_show_app";
        public const string RealStateMoBan_DSSP_DoiShowPrice = RealStateModule + ButtonAction + "mo_ban_dssp_doi_show_price";
        public const string RealStateMoBan_DSSP_ChiTiet = RealStateModule + ButtonAction + "mo_ban_dssp_chi_tiet";
        // Tab Thông tin chung - Chi tiết căn hộ mở bán
        public const string RealStateMoBan_DSSP_ChiTiet_ThongTinChung = RealStateModule + Tab + "mo_ban_dssp_chi_tiet_thong_tin_chung";
        public const string RealStateMoBan_DSSP_ChiTiet_ThongTinChung_ChiTiet = RealStateModule + Form + "mo_ban_dssp_chi_tiet_thong_tin_chung_chi_tiet";
        // Tab Lịch sử - Chi tiết căn hộ mở bán
        public const string RealStateMoBan_DSSP_ChiTiet_LichSu = RealStateModule + Tab + "mo_ban_dssp_chi_tiet_lich_su";
        public const string RealStateMoBan_DSSP_ChiTiet_LichSu_DanhSach = RealStateModule + Table + "mo_ban_dssp_chi_tiet_lich_su_danh_sach";


        // Tab Chính sách mở bán
        public const string RealStateMoBan_ChinhSach = RealStateModule + Tab + "mo_ban_chinh_sach";
        public const string RealStateMoBan_ChinhSach_DanhSach = RealStateModule + Table + "mo_ban_chinh_sach_ds";
        public const string RealStateMoBan_ChinhSach_ChiTiet = RealStateModule + ButtonAction + "mo_ban_chinh_sach_chi_tiet";
        public const string RealStateMoBan_ChinhSach_CapNhat = RealStateModule + ButtonAction + "mo_ban_chinh_sach_cap_nhat";
        public const string RealStateMoBan_ChinhSach_DoiTrangThai = RealStateModule + ButtonAction + "mo_ban_chinh_sach_doi_trang_thai";

        // Tab mẫu biểu hợp đồng - Mở bán
        public const string RealStateMoBan_MauBieu = RealStateModule + Tab + "mo_ban_mau_bieu";
        public const string RealStateMoBan_MauBieu_DanhSach = RealStateModule + Table + "mo_ban_mau_bieu_ds";
        public const string RealStateMoBan_MauBieu_ThemMoi = RealStateModule + ButtonAction + "mo_ban_mau_bieu_them_moi";
        public const string RealStateMoBan_MauBieu_ChinhSua = RealStateModule + ButtonAction + "mo_ban_mau_bieu_chinh_sua";
        public const string RealStateMoBan_MauBieu_DoiTrangThai = RealStateModule + ButtonAction + "mo_ban_mau_bieu_doi_trang_thai";

        // Tab hồ sơ dự án - Mở bán
        public const string RealStateMoBan_HoSo = RealStateModule + Tab + "mo_ban_ho_so";
        public const string RealStateMoBan_HoSo_DanhSach = RealStateModule + Table + "mo_ban_ho_so_ds";
        public const string RealStateMoBan_HoSo_ThemMoi = RealStateModule + ButtonAction + "mo_ban_ho_so_them_moi";
        public const string RealStateMoBan_HoSo_ChinhSua = RealStateModule + ButtonAction + "mo_ban_ho_so_chinh_sua";
        public const string RealStateMoBan_HoSo_DoiTrangThai = RealStateModule + ButtonAction + "mo_ban_ho_so_doi_trang_thai";
        public const string RealStateMoBan_HoSo_XemFile = RealStateModule + ButtonAction + "mo_ban_ho_so_xem_file";
        public const string RealStateMoBan_HoSo_Tai = RealStateModule + ButtonAction + "mo_ban_ho_so_tai";
        public const string RealStateMoBan_HoSo_Xoa = RealStateModule + ButtonAction + "mo_ban_ho_so_xoa";

        // Menu Quản lý giao dịch cọc
        public const string RealStateMenuQLGiaoDichCoc = RealStateModule + Menu + "ql_ho_so_giao_dich_coc";
        // Module Sổ lệnh
        public const string RealStateMenuSoLenh = RealStateModule + Menu + "so_lenh";
        public const string RealStateMenuSoLenh_DanhSach = RealStateModule + Table + "so_lenh_ds";
        public const string RealStateMenuSoLenh_ThemMoi = RealStateModule + ButtonAction + "so_lenh_them_moi";
        public const string RealStateMenuSoLenh_GiaHanGiuCho = RealStateModule + ButtonAction + "so_lenh_gia_han_giu_cho";
        public const string RealStateMenuSoLenh_Xoa = RealStateModule + ButtonAction + "so_lenh_xoa";

        public const string RealStateMenuSoLenh_ChiTiet = RealStateModule + Page + "so_lenh_chi_tiet";
        // Tab Thông tin chung
        public const string RealStateMenuSoLenh_ThongTinChung = RealStateModule + Tab + "so_lenh_thong_tin_chung";
        public const string RealStateMenuSoLenh_ThongTinChung_ChiTiet = RealStateModule + Form + "so_lenh_thong_tin_chung_chi_tiet";
        public const string RealStateMenuSoLenh_ThongTinChung_ChinhSua = RealStateModule + ButtonAction + "so_lenh_thong_tin_chung_chinh_sua";
        public const string RealStateMenuSoLenh_ThongTinChung_DoiLoaiHD = RealStateModule + ButtonAction + "so_lenh_thong_tin_chung_doi_loai_hop_dong";
        public const string RealStateMenuSoLenh_ThongTinChung_DoiHinhThucThanhToan = RealStateModule + ButtonAction + "so_lenh_thong_tin_chung_doi_hinh_thuc_thanh_toan";
        // Tab Thanh toán
        public const string RealStateMenuSoLenh_ThongTinThanhToan = RealStateModule + Tab + "so_lenh_thong_tin_thanh_toan";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_DanhSach = RealStateModule + Table + "so_lenh_thong_tin_thanh_toan_ds";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_ThemMoi = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_them_moi";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_ChiTiet = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_chi_tiet";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_ChinhSua = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_chinh_sua";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_phe_duyet_or_huy";
        public const string RealStateMenuSoLenh_ThongTinThanhToan_Xoa = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_xoa";
        // Tab Chính sách ưu đãi
        public const string RealStateMenuSoLenh_CSUuDai = RealStateModule + Tab + "so_lenh_cs_uu_dai";
        public const string RealStateMenuSoLenh_CSUuDai_DanhSach = RealStateModule + Table + "so_lenh_cs_uu_dai_ds";
        // Tab HSKH đăng ký
        public const string RealStateMenuSoLenh_HSKHDangKy = RealStateModule + Tab + "so_lenh_hskh_dang_ky";
        public const string RealStateMenuSoLenh_HSKHDangKy_DanhSach = RealStateModule + Table + "so_lenh_hskh_dang_ky_ds";
        public const string RealStateMenuSoLenh_HSKHDangKy_ChuyenOnline = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_chuyen_online";
        public const string RealStateMenuSoLenh_HSKHDangKy_CapNhatHS = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_cap_nhat_hs";
        public const string RealStateMenuSoLenh_HSKHDangKy_DuyetHS = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_duyet_hs";
        public const string RealStateMenuSoLenh_HSKHDangKy_HuyDuyetHS = RealStateModule + ButtonAction + "so_lenh_thong_tin_thanh_toan_huy_duyet_hs";
        // Tab Lịch sử - Sổ lệnh
        public const string RealStateMenuSoLenh_LichSu = RealStateModule + Tab + "so_lenh_lich_su";
        public const string RealStateMenuSoLenh_LichSu_DanhSach = RealStateModule + Table + "so_lenh_lich_su_danh_sach";
        // Xử lý đặt cọc 
        public const string RealStateGDC_XLDC = RealStateModule + Menu + "gdc_xldc";
        public const string RealStateGDC_XLDC_DanhSach = RealStateModule + Table + "gdc_xldc_ds";
        // Hợp đồng đặt cọc 
        public const string RealStateGDC_HDDC = RealStateModule + Menu + "gdc_hddc";
        public const string RealStateGDC_HDDC_DanhSach = RealStateModule + Table + "gdc_hddc_ds";

        // Menu Quản lý phê duyệt
        public const string RealStateMenuQLPD = RealStateModule + Menu + "qlpd";
        // Phê duyệt dự án
        public const string RealStateMenuPDDA = RealStateModule + Menu + "pdda";
        public const string RealStatePDDA_DanhSach = RealStateModule + Table + "pdda_ds";
        public const string RealStatePDDA_PheDuyetOrHuy = RealStateModule + ButtonAction + "pdda_phe_duyet";
        // Phê duyệt phân phối
        public const string RealStateMenuPDPP = RealStateModule + Menu + "pdpp";
        public const string RealStatePDPP_DanhSach = RealStateModule + Table + "pdpp_ds";
        public const string RealStatePDPP_PheDuyetOrHuy = RealStateModule + ButtonAction + "pdpp_phe_duyet";
        // Phê duyệt mở bán
        public const string RealStateMenuPDMB = RealStateModule + Menu + "pdmb";
        public const string RealStatePDMB_DanhSach = RealStateModule + Table + "pdmb_ds";
        public const string RealStatePDMB_PheDuyetOrHuy = RealStateModule + ButtonAction + "pdmb_phe_duyet";
        // Báo cáo
        public const string RealState_Menu_BaoCao = RealStateModule + Menu + "bao_cao";
        public const string RealState_BaoCao_QuanTri = RealStateModule + Page + "bao_cao_quan_tri";
        public const string RealState_BaoCao_QuanTri_TQBangHangDuAn = RealStateModule + ButtonAction + "bao_cao_quan_tri_tq_bang_hang_du_an";
        public const string RealState_BaoCao_QuanTri_TH_TienVeDA = RealStateModule + ButtonAction + "bao_cao_quan_tri_th_tien_ve_da";
        public const string RealState_BaoCao_QuanTri_TH_CacKhoanGD = RealStateModule + ButtonAction + "bao_cao_quan_tri_th_cac_khoan_gd";

        public const string RealState_BaoCao_VanHanh = RealStateModule + Page + "bao_cao_van_hanh";
        public const string RealState_BaoCao_VanHanh_TQBangHangDuAn = RealStateModule + ButtonAction + "bao_cao_van_hanh_tq_bang_hang_du_an";
        public const string RealState_BaoCao_VanHanh_TH_TienVeDA = RealStateModule + ButtonAction + "bao_cao_van_hanh_th_tien_ve_da";
        public const string RealState_BaoCao_VanHanh_TH_CacKhoanGD = RealStateModule + ButtonAction + "bao_cao_van_hanh_th_cac_khoan_gd";

        public const string RealState_BaoCao_KinhDoanh = RealStateModule + Page + "bao_cao_kinh_doanh";
        public const string RealState_BaoCao_KinhDoanh_TQBangHangDuAn = RealStateModule + ButtonAction + "bao_cao_kinh_doanh_tq_bang_hang_du_an";
        public const string RealState_BaoCao_KinhDoanh_TH_TienVeDA = RealStateModule + ButtonAction + "bao_cao_kinh_doanh_th_tien_ve_da";
        public const string RealState_BaoCao_KinhDoanh_TH_CacKhoanGD = RealStateModule + ButtonAction + "bao_cao_kinh_doanh_th_cac_khoan_gd";

        #endregion

        #region loyalty
        private const string LoyaltyModule = "loyalty.";
        public const string LoyaltyWeb = LoyaltyModule + Web;

        // Quản lý khách hàng
        public const string LoyaltyMenu_QLKhachHang = LoyaltyModule + Menu + "quan_ly_khach_hang";

        public const string LoyaltyMenu_KhachHangCaNhan = LoyaltyModule + Menu + "khach_hang_ca_nhan";
        public const string Loyalty_KhachHangCaNhan_DanhSach = LoyaltyModule + Table + "khach_hang_ca_nhan_danh_sach";
        public const string Loyalty_KhachHangCaNhan_CapNhat = LoyaltyModule + ButtonAction + "khach_hang_ca_nhan_cap_nhat";
        public const string LoyaltyKhachHangCaNhan_XuatDuLieu = LoyaltyModule + ButtonAction + "khach_hang_ca_nhan_xuat_du_lieu";
        public const string Loyalty_KhachHangCaNhan_ChiTiet = LoyaltyModule + Page + "khach_hang_ca_nhan_chi_tiet";

        // Tab thông tin chung
        public const string Loyalty_KhachHangCaNhan_ThongTinChung = LoyaltyModule + Tab + "khach_hang_ca_nhan_thong_tin_chung";
        public const string Loyalty_KhachHangCaNhan_ThongTinChung_ChiTiet = LoyaltyModule + Form + "khach_hang_ca_nhan_thong_tin_chung_chi_tiet";

        // Tab danh sach uu dai
        public const string Loyalty_KhachHangCaNhan_UuDai = LoyaltyModule + Tab + "khach_hang_ca_nhan_uu_dai";
        public const string Loyalty_KhachHangCaNhan_UuDai_DanhSach = LoyaltyModule + Table + "khach_hang_ca_nhan_uu_dai_danh_sach";
        public const string Loyalty_KhachHangCaNhan_UuDai_ThemMoi = LoyaltyModule + ButtonAction + "khach_hang_ca_nhan_uu_dai_them_moi";

        // Tab SuKienThamGia
        public const string Loyalty_KhachHangCaNhan_SuKienThamGia = LoyaltyModule + Tab + "khach_hang_ca_nhan_su_kien_tham_gia";

        //Tab lịch sử tham gia
        public const string Loyalty_KhachHangCaNhan_LichSuThamGia = LoyaltyModule + Tab + "khach_hang_ca_nhan_lich_su_tham_gia";
        public const string Loyalty_KhachHangCaNhan_LichSuThamGia_DanhSach = LoyaltyModule + Table + "khach_hang_ca_nhan_lich_su_tham_gia_danh_sach";

        // Quản lý uu dai
        public const string LoyaltyMenu_QLUuDai = LoyaltyModule + Menu + "quan_ly_uu_dai";

        //DanhSachUuDai
        public const string LoyaltyMenu_DanhSachUuDai = LoyaltyModule + Menu + "danh_sach_uu_dai";
        public const string Loyalty_DanhSachUuDai_DanhSach = LoyaltyModule + Table + "danh_sach_uu_dai_danh_sach";
        public const string Loyalty_DanhSachUuDai_UploadVoucher = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_upload_voucher";
        public const string Loyalty_DanhSachUuDai_DownloadMau = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_download_mau";
        public const string Loyalty_DanhSachUuDai_ThemMoi = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_them_moi";
        public const string Loyalty_DanhSachUuDai_ChiTiet = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_chi_tiet";
        public const string Loyalty_DanhSachUuDai_ChinhSua = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_chinh_sua";
        public const string Loyalty_DanhSachUuDai_DanhDauOrTatNoiBat = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_danh_dau_or_tat_noi_bat";
        public const string Loyalty_DanhSachUuDai_BatOrTatShowApp = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_bat_or_tat_show_app";
        public const string Loyalty_DanhSachUuDai_KichHoatOrHuy = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_kich_hoat_or_huy";
        public const string Loyalty_DanhSachUuDai_Xoa = LoyaltyModule + ButtonAction + "danh_sach_uu_dai_xoa";

        //LichSuCapPhat
        public const string LoyaltyMenu_LichSuCapPhat = LoyaltyModule + Menu + "lich_su_cap_phat";
        public const string Loyalty_LichSuCapPhat_DanhSach = LoyaltyModule + Table + "lich_su_cap_phat_danh_sach";

        // Quản lý diem
        public const string LoyaltyMenu_QLDiem = LoyaltyModule + Menu + "quan_ly_diem";

        //QL tich diem
        public const string LoyaltyMenu_TichDiem = LoyaltyModule + Menu + "quan_ly_tich_diem";
        public const string LoyaltyMenu_TichDiem_ThemMoi = LoyaltyModule + ButtonAction + "tich_diem_them_moi";
        public const string LoyaltyMenu_TichDiem_UploadDSDiem = LoyaltyModule + ButtonAction + "tich_diem_upload_ds_diem";
        public const string LoyaltyMenu_TichDiem_DownloadMau = LoyaltyModule + ButtonAction + "tich_diem_download_mau";
        public const string LoyaltyMenu_TichDiem_DanhSach = LoyaltyModule + Table + "tich_diem_danh_sach";
        public const string LoyaltyMenu_TichDiem_ChiTiet = LoyaltyModule + ButtonAction + "tich_diem_chi_tiet";
        public const string LoyaltyMenu_TichDiem_ChinhSua = LoyaltyModule + ButtonAction + "tich_diem_chinh_sua";
        public const string LoyaltyMenu_TichDiem_Huy = LoyaltyModule + ButtonAction + "tich_diem_huy";

        //QuanLyHangThanhVien
        public const string LoyaltyMenu_HangThanhVien = LoyaltyModule + Menu + "quan_ly_hang_thanh_vien";
        public const string LoyaltyMenu_HangThanhVien_ThemMoi = LoyaltyModule + ButtonAction + "hang_thanh_vien_them_moi";
        public const string LoyaltyMenu_HangThanhVien_DanhSach = LoyaltyModule + Table + "hang_thanh_vien_danh_sach";
        public const string LoyaltyMenu_HangThanhVien_ChiTiet = LoyaltyModule + ButtonAction + "hang_thanh_vien_chi_tiet";
        public const string LoyaltyMenu_HangThanhVien_ChinhSua = LoyaltyModule + ButtonAction + "hang_thanh_vien_chinh_sua";
        public const string LoyaltyMenu_HangThanhVien_KichHoatOrHuy = LoyaltyModule + ButtonAction + "hang_thanh_vien_kich_hoat_or_huy";

        // Quản lý yeu cau
        public const string LoyaltyMenu_QLYeuCau = LoyaltyModule + Menu + "quan_ly_yeu_cau";

        //Yêu cầu đổi voucher
        public const string LoyaltyMenu_YeuCauDoiVoucher = LoyaltyModule + Menu + "yeu_cau_doi_voucher";
        public const string Loyalty_YeuCauDoiVoucher_DanhSach = LoyaltyModule + Table + "yeu_cau_doi_voucher_danh_sach";
        public const string Loyalty_YeuCauDoiVoucher_ThemMoi = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_them_moi";
        public const string Loyalty_YeuCauDoiVoucher_ChiTiet = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_chi_tiet";
        public const string Loyalty_YeuCauDoiVoucher_ChinhSua = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_chinh_sua";
        public const string Loyalty_YeuCauDoiVoucher_BanGiao = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_ban_giao";
        public const string Loyalty_YeuCauDoiVoucher_HoanThanh = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_hoan_thanh";
        public const string Loyalty_YeuCauDoiVoucher_HuyYeuCau = LoyaltyModule + ButtonAction + "yeu_cau_doi_voucher_huy_yeu_cau";

        //Truyền thông
        public const string LoyaltyMenu_TruyenThong = LoyaltyModule + Menu + "truyen_thong";
        //Truyền thông -  Hình ảnh
        public const string LoyaltyMenu_HinhAnh = LoyaltyModule + Menu + "hinh_anh";
        public const string LoyaltyHinhAnh_DanhSach = LoyaltyModule + Table + "hinh_anh_danh_sach";
        public const string LoyaltyHinhAnh_ThemMoi = LoyaltyModule + ButtonAction + "hinh_anh_them_moi";
        public const string LoyaltyHinhAnh_Xoa = LoyaltyModule + ButtonAction + "hinh_anh_cap_nhat";
        public const string LoyaltyHinhAnh_CapNhat = LoyaltyModule + ButtonAction + "hinh_anh_xoa";
        public const string LoyaltyHinhAnh_PheDuyetOrHuyDang = LoyaltyModule + ButtonAction + "hinh_anh_phe_duyet_or_huy_dang";

        // Truyền thông - Tin Tức
        public const string LoyaltyMenu_TinTuc = LoyaltyModule + Menu + "tin_tuc";
        public const string LoyaltyTinTuc_DanhSach = LoyaltyModule + Table + "tin_tuc_danh_sach";
        public const string LoyaltyTinTuc_ThemMoi = LoyaltyModule + ButtonAction + "tin_tuc_them_moi";
        public const string LoyaltyTinTuc_CapNhat = LoyaltyModule + ButtonAction + "tin_tuc_cap_nhat";
        public const string LoyaltyTinTuc_PheDuyetOrHuyDang = LoyaltyModule + ButtonAction + "tin_tuc_phe_duyet_or_huy_dang";
        public const string LoyaltyTinTuc_Xoa = LoyaltyModule + ButtonAction + "tin_tuc_xoa";

        // Menu Thông báo -------Start
        public const string LoyaltyMenu_ThongBao = LoyaltyModule + Menu + "thong_bao";

        public const string LoyaltyMenu_ThongBaoHeThong = LoyaltyModule + Menu + "thong_bao_he_thong";
        public const string LoyaltyThongBaoHeThong_CapNhat = LoyaltyModule + Menu + "thong_bao_he_thong_cap_nhat";

        // Thông báo - Mẫu thông báo
        public const string LoyaltyMenu_MauThongBao = LoyaltyModule + Menu + "mau_thong_bao";
        public const string LoyaltyMauThongBao_DanhSach = LoyaltyModule + Table + "mau_thong_bao_danh_sach";
        public const string LoyaltyMauThongBao_ThemMoi = LoyaltyModule + ButtonAction + "mau_thong_bao_them_moi";
        public const string LoyaltyMauThongBao_CapNhat = LoyaltyModule + ButtonAction + "mau_thong_bao_cap_nhat";
        public const string LoyaltyMauThongBao_Xoa = LoyaltyModule + ButtonAction + "mau_thong_bao_xoa";
        public const string LoyaltyMauThongBao_KichHoatOrHuy = LoyaltyModule + ButtonAction + "mau_thong_bao_kich_hoat_hoac_huy";

        // Thông báo - Quản lý thông báo
        public const string LoyaltyMenu_QLTB = LoyaltyModule + Menu + "qltb";
        public const string LoyaltyQLTB_DanhSach = LoyaltyModule + Table + "qltb_danh_sach";
        public const string LoyaltyQLTB_ThemMoi = LoyaltyModule + ButtonAction + "qltb_them_moi";
        public const string LoyaltyQLTB_Xoa = LoyaltyModule + ButtonAction + "qltb_xoa";
        public const string LoyaltyQLTB_KichHoatOrHuy = LoyaltyModule + ButtonAction + "qltb_kich_hoat_hoac_huy";
        public const string LoyaltyQLTB_PageChiTiet = LoyaltyModule + Page + "qltb_page_chi_tiet";


        // Chi tiết thông báo
        public const string LoyaltyQLTB_ThongTinChung = LoyaltyModule + Tab + "qltb_page_chi_tiet_thong_tin_chung";
        public const string LoyaltyQLTB_PageChiTiet_ThongTin = LoyaltyModule + Form + "qltb_page_chi_tiet_thong_tin";
        public const string LoyaltyQLTB_PageChiTiet_CapNhat = LoyaltyModule + ButtonAction + "qltb_page_chi_tiet_cap_nhat";

        // Tab Gửi thông báo
        public const string LoyaltyQLTB_GuiThongBao = LoyaltyModule + Tab + "qltb_page_chi_tiet_gui_thong_bao";
        public const string LoyaltyQLTB_PageChiTiet_GuiThongBao_DanhSach = LoyaltyModule + Form + "qltb_page_chi_tiet_gui_thong_bao_danh_sach";
        public const string LoyaltyQLTB_PageChiTiet_GuiThongBao_CaiDat = LoyaltyModule + ButtonAction + "qltb_page_chi_tiet_gui_thong_bao_cai_dat";
        public const string LoyaltyQLTB_PageChiTiet_GuiThongBao_GuiThongBao = LoyaltyModule + ButtonAction + "qltb_page_chi_tiet_gui_thong_bao_gui_thong_bao";
        public const string LoyaltyQLTB_PageChiTiet_GuiThongBao_Xoa = LoyaltyModule + ButtonAction + "qltb_page_chi_tiet_gui_thong_bao_xoa";

        // Báo cáo
        public const string LoyaltyMenu_BaoCao = LoyaltyModule + Menu + "bao_cao";

        public const string Loyalty_BaoCao_QuanTri = LoyaltyModule + Page + "bao_cao_quan_tri";
        public const string Loyalty_BaoCao_QuanTri_DSYeuCauDoiVoucher = LoyaltyModule + ButtonAction + "bao_cao_quan_tri_ds_yeu_cau_doi_voucher";
        public const string Loyalty_BaoCao_QuanTri_XuatNhapTonVoucher = LoyaltyModule + ButtonAction + "bao_cao_quan_tri_xuat_nhap_ton_voucher";

        // Chương trình trúng thưởng
        public const string LoyaltyMenu_CT_TrungThuong = LoyaltyModule + Menu + "ct_trung_thuong";
        public const string LoyaltyCT_TrungThuong_DanhSach = LoyaltyModule + Table + "ct_trung_thuong_danh_sach";
        public const string LoyaltyCT_TrungThuong_ThemMoi = LoyaltyModule + ButtonAction + "ct_trung_thuong_them_moi";
        public const string LoyaltyCT_TrungThuong_CaiDatThamGia = LoyaltyModule + ButtonAction + "ct_trung_thuong_cai_dat_tham_gia";
        public const string LoyaltyCT_TrungThuong_DoiTrangThai = LoyaltyModule + ButtonAction + "ct_trung_thuong_doi_trang_thai";
        public const string LoyaltyCT_TrungThuong_Xoa = LoyaltyModule + ButtonAction + "ct_trung_thuong_xoa";
        public const string LoyaltyCT_TrungThuong_PageChiTiet = LoyaltyModule + Page + "ct_trung_thuong_page_chi_tiet";
        public const string LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh = LoyaltyModule + Tab + "ct_trung_thuong_page_chi_tiet_thong_tin_chuong_trinh";
        public const string LoyaltyCT_TrungThuong_ThongTinChuongTrinh_ChinhSua = LoyaltyModule + ButtonAction + "ct_trung_thuong_thong_tin_chuong_trinh_chinh_sua";
        public const string LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh = LoyaltyModule + Tab + "ct_trung_thuong_page_chi_tiet_cau_hinh_chuong_trinh";
        public const string LoyaltyCT_TrungThuong_CauHinhChuongTrinh_ChinhSua = LoyaltyModule + ButtonAction + "ct_trung_thuong_cau_hinh_chuong_trinh_chinh_sua";
        public const string LoyaltyCT_TrungThuong_PageChiTiet_LichSu = LoyaltyModule + Tab + "ct_trung_thuong_page_chi_tiet_lich_su";
        public const string LoyaltyCT_TrungThuong_LichSu_DanhSach = LoyaltyModule + Table + "ct_trung_thuong_lich_su_danh_sach";

        #endregion

        #region event

        private const string EventModule = "event.";
        public const string EventWeb = EventModule + Web;

        // Cài đặt
        public const string Event_Menu_CaiDat = EventModule + Menu + "CaiDat";

        // Cấu trúc mã hợp đồng
        public const string Event_Menu_CauTrucMaHD = EventModule + Menu + "CauTrucMaHD";
        public const string Event_CauTrucMaHD_DanhSach = EventModule + Table + "CauTrucMaHD_DanhSach";
        public const string Event_CauTrucMaHD_ThemMoi = EventModule + ButtonAction + "CauTrucMaHD_ThemMoi";
        public const string Event_CauTrucMaHD_ChiTiet = EventModule + ButtonAction + "CauTrucMaHD_ChiTiet";
        public const string Event_CauTrucMaHD_CapNhat = EventModule + ButtonAction + "CauTrucMaHD_CapNhat";
        public const string Event_CauTrucMaHD_DoiTrangThai = EventModule + ButtonAction + "CauTrucMaHD_DoiTrangThai";

        // Thông báo hệ thống
        public const string Event_Menu_ThongBaoHeThong = EventModule + Menu + "ThongBaoHeThong";
        public const string Event_ThongBaoHeThong_DanhSach = EventModule + Table + "ThongBaoHeThong_DanhSach";
        public const string Event_ThongBaoHeThong_CapNhat = EventModule + ButtonAction + "ThongBaoHeThong_CapNhat";

        // Quản lý sự kiện
        public const string Event_Menu_QuanLySuKien = EventModule + Menu + "QuanLySuKien";

        // Tổng quan sự kiện
        public const string Event_Menu_TongQuanSuKien = EventModule + Menu + "TongQuanSuKien";
        public const string Event_TongQuanSuKien_DanhSach = EventModule + Table + "TongQuanSuKien_DanhSach";
        public const string Event_TongQuanSuKien_ThemMoi = EventModule + ButtonAction + "TongQuanSuKien_ThemMoi";
        public const string Event_TongQuanSuKien_MoBanVe = EventModule + ButtonAction + "TongQuanSuKien_MoBanVe";
        public const string Event_TongQuanSuKien_BatTatShowApp = EventModule + ButtonAction + "TongQuanSuKien_BatTatShowApp";
        public const string Event_TongQuanSuKien_TamDung_HuySuKien = EventModule + ButtonAction + "TongQuanSuKien_TamDungOrHuySuKien";
        public const string Event_TongQuanSuKien_ChiTiet = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet";

        // TỔNG QUAN SỰ KIỆN - THÔNG TIN CHUNG
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinChung = EventModule + Tab + "TongQuanSuKien_ChiTiet_ThongTinChung";
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet";
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat";

        // TỔNG QUAN SỰ KIỆN - QUẢN LÝ
        public const string Event_TongQuanSuKien_ChiTiet_QuanLy = EventModule + Tab + "TongQuanSuKien_ChiTiet_QuanLy";
        public const string Event_TongQuanSuKien_ChiTiet_QuanLy_DanhSach = EventModule + Table + "TongQuanSuKien_ChiTiet_QuanLy_DanhSach";
        public const string Event_TongQuanSuKien_ChiTiet_QuanLy_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_QuanLy_CapNhat";

        // TỔNG QUAN SỰ KIỆN - THÔNG TIN MÔ TẢ
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinMoTa = EventModule + Tab + "TongQuanSuKien_ChiTiet_ThongTinMoTa";
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet = EventModule + Table + "TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet";
        public const string Event_TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat";

        // // TỔNG QUAN SỰ KIỆN - THỜI GIAN VÀ LOẠI VÉ
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe = EventModule + Tab + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe";
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach = EventModule + Table + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach";
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi";
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet";
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat";
        public const string Event_TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai";

        // TỔNG QUAN SỰ KIỆN - HÌNH ẢNH SỰ KIỆN
        public const string Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien = EventModule + Tab + "TongQuanSuKien_ChiTiet_HinhAnhSuKien";
        public const string Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh = EventModule + Page + "TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh";
        public const string Event_TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat";

        // TỔNG QUAN SỰ KIỆN - MẪU GIAO NHẬN VÉ
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe = EventModule + Tab + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach = EventModule + Table + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe";
        public const string Event_TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai";

        // TỔNG QUAN SỰ KIỆN - MẪU VÉ
        public const string Event_TongQuanSuKien_ChiTiet_MauVe = EventModule + Tab + "TongQuanSuKien_ChiTiet_MauVe";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_DanhSach = EventModule + Table + "TongQuanSuKien_ChiTiet_MauVe_DanhSach";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_ThemMoi = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_ThemMoi";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_ChiTiet = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_ChiTiet";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_CapNhat = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_CapNhat";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_XemThuMau = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_XemThuMau";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_TaiMauVe = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_TaiMauVe";
        public const string Event_TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai = EventModule + ButtonAction + "TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai";

        // QUẢN LÝ BÁN VÉ
        public const string Event_Menu_QuanLyBanVe = EventModule + Menu + "QuanLyBanVe";

        // SỔ LỆNH
        public const string Event_Menu_SoLenh = EventModule + Menu + "SoLenh";
        public const string Event_SoLenh_DanhSach = EventModule + Table + "SoLenh_DanhSach";
        public const string Event_SoLenh_ThemMoi = EventModule + ButtonAction + "SoLenh_ThemMoi";
        public const string Event_SoLenh_GiaHanGiulenh = EventModule + ButtonAction + "SoLenh_GiaHanGiuLenh";
        public const string Event_SoLenh_Xoa = EventModule + ButtonAction + "SoLenh_Xoa";

        // CHI TIẾT SỔ LỆNH
        public const string Event_SoLenh_ChiTiet = EventModule + Page + "SoLenh_ChiTiet";

        // SỔ LỆNH - THÔNG TIN CHUNG
        public const string Event_SoLenh_ChiTiet_ThongTinChung = EventModule + Tab + "SoLenh_ChiTiet_ThongTinChung";
        public const string Event_SoLenh_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "SoLenh_ChiTiet_ThongTinChung_ChiTiet";
        public const string Event_SoLenh_ChiTiet_ThongTinChung_CapNhat = EventModule + ButtonAction + "SoLenh_ChiTiet_ThongTinChung_CapNhat";

        // SỔ LỆNH - GIAO DỊCH
        public const string Event_SoLenh_ChiTiet_GiaoDich = EventModule + Tab + "SoLenh_ChiTiet_GiaoDich";
        public const string Event_SoLenh_ChiTiet_GiaoDich_DanhSach = EventModule + Table + "SoLenh_ChiTiet_GiaoDich_DanhSach";
        public const string Event_SoLenh_ChiTiet_GiaoDich_ThemMoi = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_ThemMoi";
        public const string Event_SoLenh_ChiTiet_GiaoDich_ChiTiet = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_ChiTiet";
        public const string Event_SoLenh_ChiTiet_GiaoDich_CapNhat = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_CapNhat";
        public const string SoLenh_ChiTiet_GiaoDich_GuiThongBao = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_GuiThongBao";
        public const string Event_SoLenh_ChiTiet_GiaoDich_PheDuyet = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_PheDuyet";
        public const string Event_SoLenh_ChiTiet_GiaoDich_HuyThanhToan = EventModule + ButtonAction + "SoLenh_ChiTiet_GiaoDich_HuyThanhToan";

        // SỔ LỆNH - DANH SÁCH VÉ
        public const string Event_SoLenh_ChiTiet_DanhSachVe = EventModule + Tab + "SoLenh_ChiTiet_DanhSachVe";
        public const string Event_SoLenh_ChiTiet_DanhSachVe_DanhSach = EventModule + Tab + "SoLenh_ChiTiet_DanhSachVe_DanhSach";
        public const string Event_SoLenh_ChiTiet_DanhSachVe_TaiVe = EventModule + ButtonAction + "SoLenh_ChiTiet_DanhSachVe_TaiVe";
        public const string Event_SoLenh_ChiTiet_DanhSachVe_XemVe = EventModule + ButtonAction + "SoLenh_ChiTiet_DanhSachVe_XemVe";

        // SỔ LỆNH - LỊCH SỬ
        public const string Event_SoLenh_ChiTiet_LichSu = EventModule + Table + "SoLenh_ChiTiet_LichSu";

        // XỬ LÝ GIAO DỊCH
        public const string Event_Menu_XuLyGiaoDich = EventModule + Menu + "XuLyGiaoDich";
        public const string Event_XuLyGiaoDich_DanhSach = EventModule + Table + "XuLyGiaoDich_DanhSach";
        public const string Event_XuLyGiaoDich_PheDuyetMuaVe = EventModule + ButtonAction + "XuLyGiaoDich_PheDuyetMuave";

        // CHI TIẾT XỬ LÝ GIAO DỊCH
        public const string Event_XuLyGiaoDich_ChiTiet = EventModule + Page + "XuLyGiaoDich_ChiTiet";

        // XỬ LÝ GIAO DỊCH - THÔNG TIN CHUNG
        public const string Event_XuLyGiaoDich_ChiTiet_ThongTinChung = EventModule + Tab + "XuLyGiaoDich_ChiTiet_ThongTinChung";
        public const string Event_XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet";
        public const string Event_XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat";

        // XỬ LÝ GIAO DỊCH - GIAO DỊCH
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich = EventModule + Page + "XuLyGiaoDich_ChiTiet_GiaoDich";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach = EventModule + Page + "XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet";
        public const string Event_XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan";

        // XỬ LÝ GIAO DỊCH - DANH SÁCH VÉ
        public const string Event_XuLyGiaoDich_ChiTiet_DanhSachVe = EventModule + Tab + "XuLyGiaoDich_ChiTiet_DanhSachVe";
        public const string Event_XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach = EventModule + Table + "XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach";
        public const string Event_XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe";
        public const string Event_XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe = EventModule + ButtonAction + "XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe";

        // XỬ LÝ GIAO DỊCH - LỊCH SỬ
        public const string Event_XuLyGiaoDich_ChiTiet_LichSu = EventModule + Table + "XuLyGiaoDich_ChiTiet_LichSu";

        // VÉ BÁN HỢP LỆ
        public const string Event_Menu_VeBanHopLe = EventModule + Menu + "VeBanHopLe";
        public const string Event_VeBanHopLe_DanhSach = EventModule + Table + "VeBanHopLe_DanhSach";
        public const string Event_VeBanHopLe_YeuCauHoaDon = EventModule + ButtonAction + "VeBanHopLe_YeuCauHoaDon";
        public const string Event_VeBanHopLe_YeuCauVeCung = EventModule + ButtonAction + "VeBanHopLe_YeuCauVeCung";
        public const string Event_VeBanHopLe_ThongBaoVeHopLe = EventModule + ButtonAction + "VeBanHopLe_ThongBaoVeHopLe";
        public const string Event_VeBanHopLe_KhoaYeuCau = EventModule + ButtonAction + "VeBanHopLe_KhoaYeuCau";

        // CHI TIẾT VÉ BÁN HỢP LỆ
        public const string Event_VeBanHopLe_ChiTiet = EventModule + Page + "VeBanHopLe_ChiTiet";

        // VÉ BÁN HỢP LỆ - THÔNG TIN CHUNG
        public const string Event_VeBanHopLe_ChiTiet_ThongTinChung = EventModule + Tab + "VeBanHopLe_ChiTiet_ThongTinChung";
        public const string Event_VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet";
        public const string Event_VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu";

        // VÉ BÁN HỢP LỆ - GIAO DỊCH
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich = EventModule + Page + "VeBanHopLe_ChiTiet_GiaoDich";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_DanhSach = EventModule + Table + "VeBanHopLe_ChiTiet_GiaoDich_DanhSach";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_ThemMoi = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_GiaoDich_ThemMoi";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_ChiTiet = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_GiaoDich_ChiTiet";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_CapNhat = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_GiaoDich_CapNhat";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_PheDuyet = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_GiaoDich_PheDuyet";
        public const string Event_VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan";

        // VÉ BÁN HỢP LỆ - DANH SÁCH VÉ
        public const string Event_VeBanHopLe_ChiTiet_DanhSachVe = EventModule + Tab + "VeBanHopLe_ChiTiet_DanhSachVe";
        public const string Event_VeBanHopLe_ChiTiet_DanhSachVe_DanhSach = EventModule + Table + "VeBanHopLe_ChiTiet_DanhSachVe_DanhSach";
        public const string Event_VeBanHopLe_ChiTiet_DanhSachVe_TaiVe = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_DanhSachVe_TaiVe";
        public const string Event_VeBanHopLe_ChiTiet_DanhSachVe_XemVe = EventModule + ButtonAction + "VeBanHopLe_ChiTiet_DanhSachVe_XemVe";

        // VÉ BÁN HỢP LỆ - LỊCH SỬ
        public const string Event_VeBanHopLe_ChiTiet_LichSu = EventModule + Tab + "VeBanHopLe_ChiTiet_LichSu";

        // QUẢN LÝ THAM GIA
        public const string Event_Menu_QuanLyThamGia = EventModule + Menu + "QuanLyThamGia";
        public const string Event_QuanLyThamGia_DanhSach = EventModule + Table + "QuanLyThamGia_DanhSach";
        public const string Event_QuanLyThamGia_XemVe = EventModule + ButtonAction + "QuanLyThamGia_XemVe";
        public const string Event_QuanLyThamGia_TaiVe = EventModule + ButtonAction + "QuanLyThamGia_TaiVe";
        public const string Event_QuanLyThamGia_XacNhanThamGia = EventModule + ButtonAction + "QuanLyThamGia_XacNhanThamGia";
        public const string Event_QuanLyThamGia_MoKhoaVe = EventModule + ButtonAction + "QuanLyThamGia_MoKhoaVe";

        // YÊU CẦU VÉ CỨNG
        public const string Event_Menu_YeuCauVeCung = EventModule + Menu + "YeuCauVeCung";
        public const string Event_YeuCauVeCung_DanhSach = EventModule + Table + "YeuCauVeCung_DanhSach";
        public const string Event_YeuCauVeCung_XuatMauGiaoVe = EventModule + ButtonAction + "YeuCauVeCung_XuatMauGiaoVe";
        public const string Event_YeuCauVeCung_XuatVeCung = EventModule + ButtonAction + "YeuCauVeCung_XuatVeCung";
        public const string Event_YeuCauVeCung_DoiTrangThai_DangGiao = EventModule + ButtonAction + "YeuCauVeCung_DoiTrangThai_DangGiao";
        public const string Event_YeuCauVeCung_DoiTrangThai_HoanThanh = EventModule + ButtonAction + "YeuCauVeCung_DoiTrangThai_HoanThanh";

        public const string Event_YeuCauVeCung_ChiTiet = EventModule + Page + "YeuCauVeCung_ChiTiet";
        // YÊU CẦU VÉ CỨNG - CHI TIẾT - THÔNG TIN CHUNG
        public const string Event_YeuCauVeCung_ChiTiet_ThongTinChung = EventModule + Tab + "YeuCauVeCung_ChiTiet_ThongTinChung";
        public const string Event_YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet";
        // public const string Event_YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauHoaDon = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauHoaDon";
        // public const string Event_YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauVeCung = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauVeCung";
        // public const string Event_YeuCauVeCung_ChiTiet_ThongTinChung_CapNhat = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_ThongTinChung_CapNhat";

        // YÊU CẦU VÉ CỨNG - CHI TIẾT - GIAO DỊCH
        public const string Event_YeuCauVeCung_ChiTiet_GiaoDich = EventModule + Tab + "YeuCauVeCung_ChiTiet_GiaoDich";
        public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_DanhSach = EventModule + Table + "YeuCauVeCung_ChiTiet_GiaoDich_DanhSach";
        // public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_ThemMoi = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_GiaoDich_ThemMoi";
        public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet";
        // public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_CapNhat = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_GiaoDich_CapNhat";
        // public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_PheDuyet = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_GiaoDich_PheDuyet";
        // public const string Event_YeuCauVeCung_ChiTiet_GiaoDich_HuyThanhToan = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_GiaoDich_HuyThanhToan";

        // YÊU CẦU VÉ CỨNG - CHI TIẾT - DANH SÁCH VÉ
        public const string Event_YeuCauVeCung_ChiTiet_DanhSachVe = EventModule + Tab + "YeuCauVeCung_ChiTiet_DanhSachVe";
        public const string Event_YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach = EventModule + Tab + "YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach";
        public const string Event_YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe";
        public const string Event_YeuCauVeCung_ChiTiet_DanhSachVe_XemVe = EventModule + ButtonAction + "YeuCauVeCung_ChiTiet_DanhSachVe_XemVe";

        // YÊU CẦU VÉ CỨNG - CHI TIẾT - LỊCH SỬ
        public const string Event_YeuCauVeCung_ChiTiet_LichSu = EventModule + Table + "YeuCauVeCung_ChiTiet_LichSu";

        // YÊU CẦU HÓA ĐƠN
        public const string Event_Menu_YeuCauHoaDon = EventModule + Menu + "YeuCauHoaDon";
        public const string Event_YeuCauHoaDon_DanhSach = EventModule + Table + "YeuCauHoaDon_DanhSach";
        public const string Event_YeuCauHoaDon_DoiTrangThai_DangGiao = EventModule + ButtonAction + "YeuCauHoaDon_DoiTrangThai_DangGiao";
        public const string Event_YeuCauHoaDon_DoiTrangThai_HoanThanh = EventModule + ButtonAction + "YeuCauHoaDon_DoiTrangThai_HoanThanh";

        public const string Event_YeuCauHoaDon_ChiTiet = EventModule + Page + "YeuCauHoaDon_ChiTiet";
        // YÊU CẦU HÓA ĐƠN - CHI TIẾT - THÔNG TIN CHUNG
        public const string Event_YeuCauHoaDon_ChiTiet_ThongTinChung = EventModule + Tab + "YeuCauHoaDon_ChiTiet_ThongTinChung";
        public const string Event_YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet = EventModule + Page + "YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet";
        // public const string Event_YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauHoaDon = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauHoaDon";
        // public const string Event_YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauVeCung = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauVeCung";
        // public const string Event_YeuCauHoaDon_ChiTiet_ThongTinChung_CapNhat = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_ThongTinChung_CapNhat";

        // YÊU CẦU HÓA ĐƠN - CHI TIẾT - GIAO DỊCH
        public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich = EventModule + Tab + "YeuCauHoaDon_ChiTiet_GiaoDich";
        public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach = EventModule + Table + "YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach";
        // public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_ThemMoi = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_GiaoDich_ThemMoi";
        public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet";
        // public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_CapNhat = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_GiaoDich_CapNhat";
        // public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_PheDuyet = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_GiaoDich_PheDuyet";
        // public const string Event_YeuCauHoaDon_ChiTiet_GiaoDich_HuyThanhToan = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_GiaoDich_HuyThanhToan";

        // YÊU CẦU HÓA ĐƠN - CHI TIẾT - DANH SÁCH VÉ
        public const string Event_YeuCauHoaDon_ChiTiet_DanhSachVe = EventModule + Tab + "YeuCauHoaDon_ChiTiet_DanhSachVe";
        public const string Event_YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach = EventModule + Table + "YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach";
        public const string Event_YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe";
        public const string Event_YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe = EventModule + ButtonAction + "YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe";

        // YÊU CẦU HÓA ĐƠN - CHI TIẾT - LỊCH SỬ
        public const string Event_YeuCauHoaDon_ChiTiet_LichSu = EventModule + Table + "YeuCauHoaDon_ChiTiet_LichSu";


        #endregion

        private const string FileModule = "file.";
    }
}
 
