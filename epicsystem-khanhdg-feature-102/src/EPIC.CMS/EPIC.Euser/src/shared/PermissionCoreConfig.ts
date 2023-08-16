import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionCoreConfig = {};
export class PermissionCoreConst {
    
    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly CoreModule: string = "core.";
    
    //
    public static readonly CorePageDashboard: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}dashboard`;
    //
    public static readonly CoreThongTinDoanhNghiep: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}thong_tin_doanh_nghiep`;
    //
    public static readonly CoreTTDN_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}ttdn_thong_tin_chung`;
    public static readonly CoreTTDN_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_cap_nhat`;
    public static readonly CoreTTDN_CauHinhChuKySo: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_cau_hinh_chu_ki_so`;
    //
    public static readonly CoreTTDN_TKNganHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}ttdn_tk_ngan_hang`;
    public static readonly CoreTTDN_TKNH_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_tknh_them_moi`;
    public static readonly CoreTTDN_TKNH_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_tknh_set_default`;
    //
    public static readonly CoreTTDN_GiayPhepDKKD: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}ttdn_giay_phep_dkkd`;
    public static readonly CoreTTDN_GiayPhepDKKD_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_giay_phep_dkkd_them_moi`;
    public static readonly CoreTTDN_GiayPhepDKKD_Sua: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_giay_phep_dkkd_sua`;
    public static readonly CoreTTDN_GiayPhepDKKD_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}ttdn_giay_phep_dkkd_xoa`;
    
    //
    public static readonly Core_Menu_TK_UngDung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}tk_ung_dung`;

    public static readonly Core_TK_ChuaXacMinh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}tk_chua_xac_minh`;
    public static readonly Core_TK_ChuaXacMinh_XacMinh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tk_chua_xac_minh_xac_minh`;
    public static readonly Core_TK_ChuaXacMinh_ResetMatKhau: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tk_chua_xac_minh_reset_mat_khau`;
    public static readonly Core_TK_ChuaXacMinh_XoaTaiKhoan: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tk_chua_xac_minh_xoa_tai_khoan`;

    public static readonly CorePageInvestorAccount: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}investor_account`;
    public static readonly CorePageInvestorAccount_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}investor_account_chi_tiet`;
    public static readonly CorePageInvestorAccount_ChangeStatus: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}investor_account_change_status`;
    public static readonly CorePageInvestorAccount_ResetMatKhau: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}investor_account_reset_mat_khau`;
    public static readonly CorePageInvestorAccount_DatMaPin: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}investor_account_dat_ma_pin`;
    public static readonly CorePageInvestorAccount_XoaTaiKhoan: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}investor_account_xoa_tai_khoan`;
    
    // khcn = KHCN = Khách hàng cá nhân; ds = danh sách; TKNH = Tài khoản ngân hàng
    public static readonly CoreMenuKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}khach_hang`;
    
    // Module Duyệt khách hàng cá nhân
    public static readonly CoreMenuDuyetKHCN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}duyet_khcn`;
    public static readonly CoreDuyetKHCN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_danh_sach`;
    public static readonly CoreDuyetKHCN_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_them_moi`;
    
    //
    public static readonly CoreDuyetKHCN_ThongTinKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}duyet_khcn_thong_tin_khach_hang`;
    public static readonly CoreDuyetKHCN_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_thong_tin_chung`;
    public static readonly CoreDuyetKHCN_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}duyet_khcn_chi_tiet`;
    public static readonly CoreDuyetKHCN_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_cap_nhat`;
    public static readonly CoreDuyetKHCN_TrinhDuyet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_trinh_duyet`;
    public static readonly CoreDuyetKHCN_CheckFace: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_check_face`;
    
    // tab TKNH = Tài khoản ngân hàng
    public static readonly CoreDuyetKHCN_TKNH: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_tknh`;
    public static readonly CoreDuyetKHCN_TKNH_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_tknh_danh_sach`;
    public static readonly CoreDuyetKHCN_TKNH_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tknh_them_moi`;
    public static readonly CoreDuyetKHCN_TKNH_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tknh_set_default`;
    public static readonly CoreDuyetKHCN_TKNH_Sua: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tknh_sua`;
    public static readonly CoreDuyetKHCN_TKNH_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tknh_xoa`;

     // tab TKCK
     public static readonly CoreDuyetKHCN_TKCK: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_tkck`;
     public static readonly CoreDuyetKHCN_TKCK_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_tkck_danh_sach`;
     public static readonly CoreDuyetKHCN_TKCK_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tkck_them_moi`;
     public static readonly CoreDuyetKHCN_TKCK_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tkck_set_default`;

    // tab Quản lý giấy tờ tùy thân
    public static readonly CoreDuyetKHCN_GiayTo: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_giay_to`;
    public static readonly CoreDuyetKHCN_GiayTo_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_giay_to_danh_sach`;
    public static readonly CoreDuyetKHCN_GiayTo_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_giay_to_them_moi`;
    public static readonly CoreDuyetKHCN_GiayTo_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_giay_to_cap_nhat`;
    public static readonly CoreDuyetKHCN_GiayTo_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_giay_to_set_default`;
    
    // tab Quản lý địa chỉ liên hệ
    public static readonly CoreDuyetKHCN_DiaChi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_dia_chi`;
    public static readonly CoreDuyetKHCN_DiaChi_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_dia_chi_danh_sach`;
    public static readonly CoreDuyetKHCN_DiaChi_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_dia_chi_them_moi`;
    public static readonly CoreDuyetKHCN_DiaChi_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_dia_chi_set_default`;

    // Module Khách hàng cá nhân
    // khcn = KHCN = Khách hàng cá nhân; ds = danh sách; TKNH = Tài khoản ngân hàng
    public static readonly CoreMenuKHCN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}khcn`;
    public static readonly CoreKHCN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_danh_sach`;
    public static readonly CoreKHCN_XacMinh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_xac_minh`;
    
    //
    public static readonly CoreKHCN_ThongTinKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}khcn_thong_tin_khach_hang`;
    public static readonly CoreKHCN_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_thong_tin_chung`;
    public static readonly CoreKHCN_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}khcn_chi_tiet`;
    public static readonly CoreKHCN_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_cap_nhat`;
    public static readonly CoreKHCN_CheckFace: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_check_face`;
    public static readonly CoreKHCN_DoiSDT: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_doi_sdt`;
    public static readonly CoreKHCN_DoiEmail: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_doi_email`;

    // tab TKNH = Tài khoản ngân hàng
    public static readonly CoreKHCN_TKNH: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_tknh`;
    public static readonly CoreKHCN_TKNH_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_tknh_danh_sach`;
    public static readonly CoreKHCN_TKNH_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tknh_them_moi`;
    public static readonly CoreKHCN_TKNH_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tknh_set_default`;
    public static readonly CoreKHCN_TKNH_Sua: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tknh_sua`;
    public static readonly CoreKHCN_TKNH_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tknh_xoa`;
    
    // tab TKCK
    public static readonly CoreKHCN_TKCK: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_tkck`;
    public static readonly CoreKHCN_TKCK_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_tkck_danh_sach`;
    public static readonly CoreKHCN_TKCK_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tkck_them_moi`;
    public static readonly CoreKHCN_TKCK_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tkck_set_default`;
    
    // tab tài khoản đăng nhập
    public static readonly CoreKHCN_Account: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_account`;
    public static readonly CoreKHCN_Account_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_account_danh_sach`;
    public static readonly CoreKHCN_Account_ResetPassword: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_account_reset_password`;
    public static readonly CoreKHCN_Account_ResetPin: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_account_reset_pin`;
    public static readonly CoreKHCN_Account_ChangeStatus: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_account_change_status`;
    public static readonly CoreKHCN_Account_Delete: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_account_delete`;
    
    // tab Quản lý giấy tờ tùy thân
    public static readonly CoreKHCN_GiayTo: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_giay_to`;
    public static readonly CoreKHCN_GiayTo_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_giay_to_danh_sach`;
    public static readonly CoreKHCN_GiayTo_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_giay_to_them_moi`;
    public static readonly CoreKHCN_GiayTo_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_giay_to_cap_nhat`;
    public static readonly CoreKHCN_GiayTo_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_giay_to_set_default`;
    
    // tab Quản lý địa chỉ liên hệ
    public static readonly CoreKHCN_DiaChi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_dia_chi`;
    public static readonly CoreKHCN_DiaChi_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_dia_chi_danh_sach`;
    public static readonly CoreKHCN_DiaChi_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_dia_chi_them_moi`;
    public static readonly CoreKHCN_DiaChi_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_dia_chi_set_default`;
    
    // tab lịch sử chứng minh nhà đầu tư chuyên nghiệp ; NDTCN = Nhà Đầu Tư Chuyên Nghiệp
    public static readonly CoreKHCN_NDTCN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_ndtcn`;
    public static readonly CoreKHCN_NDTCN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_ndtcn_danh_sach`;
    
    // tab quản lý tư vấn viên ; TVV = Tư vấn viên
    public static readonly CoreKHCN_TuVanVien: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_tu_van_vien`;
    public static readonly CoreKHCN_TuVanVien_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_tu_van_vien_danh_sach`;
    public static readonly CoreKHCN_TuVanVien_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tu_van_vien_them_moi`;
    public static readonly CoreKHCN_TuVanVien_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tu_van_vien_set_default`;
    
    // tab quản lý người giới thiệu
    public static readonly CoreKHCN_NguoiGioiThieu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_nguoi_gioi_thieu`;
    public static readonly CoreKHCN_NguoiGioiThieu_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_nguoi_gioi_thieu_danh_sach`;

    // Module Duyệt KHDN = Khách hàng doanh nghiệp
    public static readonly CoreMenuDuyetKHDN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}duyet_khdn`;
    public static readonly CoreDuyetKHDN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khdn_danh_sach`;
    public static readonly CoreDuyetKHDN_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_them_moi`;
        //
    public static readonly CoreDuyetKHDN_ThongTinKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}duyet_khdn_thong_tin_khach_hang`;
    public static readonly CoreDuyetKHDN_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khdn_thong_tin_chung`;
    public static readonly CoreDuyetKHDN_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}duyet_khdn_chi_tiet`;
    public static readonly CoreDuyetKHDN_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_cap_nhat`;
    public static readonly CoreDuyetKHDN_TrinhDuyet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_trinh_duyet`;
        
    // tab TKNH = Tài khoản ngân hàng
    public static readonly CoreDuyetKHDN_TKNH: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khdn_tknh`;
    public static readonly CoreDuyetKHDN_TKNH_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khdn_tknh_danh_sach`;
    public static readonly CoreDuyetKHDN_TKNH_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_tknh_them_moi`;
    public static readonly CoreDuyetKHDN_TKNH_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_tknh_set_default`;

    // tab CKS = Chữ ký số
    public static readonly CoreDuyetKHDN_CKS: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khdn_chu_ky_so`;
    public static readonly CoreDuyetKHDN_CauHinhChuKySo: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_cau_hinh_chu_ky_so`;

    // tab DKKD = Đăng ký kinh doanh
    public static readonly CoreDuyetKHDN_DKKD: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khdn_dkkd`;
    public static readonly CoreDuyetKHDN_DKKD_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khdn_dkkd_danh_sach`;
    public static readonly CoreDuyetKHDN_DKKD_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_them_moi`;
    public static readonly CoreDuyetKHDN_DKKD_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_cap_nhat`;
    public static readonly CoreDuyetKHDN_DKKD_XemFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_xem_file`;
    public static readonly CoreDuyetKHDN_DKKD_TaiFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_tai_file`;
    public static readonly CoreDuyetKHDN_DKKD_XoaFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_xoa_file`;

    //=========================
    
    // Module KHDN = Khách hàng doanh nghiệp
    public static readonly CoreMenuKHDN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}khdn`;
    public static readonly CoreKHDN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khdn_danh_sach`;
    public static readonly CoreKHDN_XacMinh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_xac_minh`;
    
    // Thông tin chung
    public static readonly CoreKHDN_ThongTinKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}khdn_thong_tin_khach_hang`;
    public static readonly CoreKHDN_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khdn_thong_tin_chung`;
    public static readonly CoreKHDN_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}khdn_chi_tiet`;
    public static readonly CoreKHDN_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_cap_nhat`;
    
    // tab TKNH = Tài khoản ngân hàng
    public static readonly CoreKHDN_TKNH: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khdn_tknh`;
    public static readonly CoreKHDN_TKNH_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khdn_tknh_danh_sach`;
    public static readonly CoreKHDN_TKNH_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_tknh_them_moi`;
    public static readonly CoreKHDN_TKNH_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_tknh_set_default`;
    
    // tab CKS = Chữ ký số
    public static readonly CoreKHDN_CKS: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khdn_chu_ky_so`;
    public static readonly CoreKHDN_CauHinhChuKySo: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_cau_hinh_chu_ky_so`;
    
    // tab DKKD = Đăng ký kinh doanh
    public static readonly CoreKHDN_DKKD: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khdn_dkkd`;
    public static readonly CoreKHDN_DKKD_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khdn_dkkd_danh_sach`;
    public static readonly CoreKHDN_DKKD_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_dkkd_them_moi`;
    public static readonly CoreKHDN_DKKD_XemFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_dkkd_xem_file`;
    public static readonly CoreKHDN_DKKD_TaiFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_dkkd_tai_file`;
    public static readonly CoreKHDN_DKKD_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_dkkd_cap_nhat`;
    public static readonly CoreKHDN_DKKD_XoaFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khdn_dkkd_xoa_file`;


    // Menu Sale
    public static readonly CoreMenuSale: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}sale`;
    
    // Menu duyệt sale -----------
    public static readonly CoreMenuDuyetSale: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}duyet_sale`;
    public static readonly CoreDuyetSale_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_sale_danh_sach`;
    public static readonly CoreDuyetSale_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_sale_them_moi`;
    public static readonly CoreDuyetSale_ThongTinSale: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}duyet_sale_thong_tin_sale`;
    
    // Thông tin chung
    public static readonly CoreDuyetSale_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_sale_thong_tin_chung`;
    public static readonly CoreDuyetSale_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}duyet_sale_chi_tiet`;
    public static readonly CoreDuyetSale_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_sale_cap_nhat`;
    public static readonly CoreDuyetSale_TrinhDuyet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_sale_trinh_duyet`;
    
    // Menu sale đã duyệt ---------
    public static readonly CoreMenuSaleActive: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}sale_active`;
    public static readonly CoreSaleActive_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}sale_active_danh_sach`;
    public static readonly CoreSaleActive_KichHoat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_kich_hoat`;
    public static readonly CoreSaleActive_ThongTinSale: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}sale_active_thong_tin_sale`;
    
    // Thông tin chung
    public static readonly CoreSaleActive_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}sale_active_thong_tin_chung`;
    public static readonly CoreSaleActive_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}sale_active_chi_tiet`;
    public static readonly CoreSaleActive_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_cap_nhat`;
    
    // Hợp đồng cộng tác = HDCT
    public static readonly CoreSaleActive_HDCT: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}sale_active_hdct`;
    public static readonly CoreSaleActive_HDCT_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}sale_active_hdct_danh_sach`;
    // public static readonly CoreSaleActive_HDCT_UploadFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_upload_ho_so`;
    public static readonly CoreSaleActive_HDCT_UpdateFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_cap_nhat_ho_so`;
    // public static readonly CoreSaleActive_HDCT_Sign: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_ky_dien_tu`;
    // public static readonly CoreSaleActive_HDCT_Preview: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_xem_ho_so`;
    // public static readonly CoreSaleActive_HDCT_Download: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_download_hop_dong`;
    // public static readonly CoreSaleActive_HDCT_Download_Sign: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_download_hop_dong_sign`;
    
    // Menu sale App
    public static readonly CoreMenuSaleApp: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}sale_app`;
    public static readonly CoreSaleApp_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}sale_app_danh_sach`;
    public static readonly CoreSaleApp_DieuHuong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_app_dieu_huong`;
    
    // Mẫu Hợp đồng cộng tác = HDCT
    public static readonly CoreMenu_HDCT_Template: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}hdct_template`;
    public static readonly CoreHDCT_Template_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}hdct_template_danh_sach`;
    public static readonly CoreHDCT_Template_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hdct_template_them_moi`;
    public static readonly CoreHDCT_Template_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hdct_template_cap_nhat`;
    public static readonly CoreHDCT_Template_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hdct_template_xoa`;
    public static readonly CoreHDCT_Template_Preview: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hdct_template_preview`;
    public static readonly CoreHDCT_Template_Download: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hdct_template_download`;
    
    // Quản lý đối tác = qldt
    public static readonly CoreMenu_QLDoiTac: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}quan_ly_doi_tac`;
    
    public static readonly CoreMenu_DoiTac: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}doi_tac`;
    public static readonly CoreDoiTac_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}doi_tac_danh_sach`;
    public static readonly CoreDoiTac_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}doi_tac_them_moi`;
    public static readonly CoreDoiTac_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}doi_tac_xoa`;   
    //
    public static readonly CoreDoiTac_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}doi_tac_thong_tin_chi_tiet`;
    public static readonly CoreDoiTac_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}doi_tac_thong_tin_chung`;
    public static readonly CoreDoiTac_XemChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}doi_tac_xem_chi_tiet`;
    public static readonly CoreDoiTac_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}doi_tac_cap_nhat`;
    
    public static readonly CoreMenu_DaiLy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}dai_ly`;
    public static readonly CoreDaiLy_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}dai_ly_danh_sach`;
    public static readonly CoreDaiLy_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}dai_ly_them_moi`;
    //
    public static readonly CoreDaiLy_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}dai_ly_thong_tin_chi_tiet`;
    public static readonly CoreDaiLy_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}dai_ly_thong_tin_chung`;
    public static readonly CoreDaiLy_XemChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}dai_ly_xem_chi_tiet`;
    //  
    public static readonly CoreDoiTac_Account: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}doi_tac_account`;
    public static readonly CoreDoiTac_Account_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}doi_tac_account_danh_sach`;
    public static readonly CoreDoiTac_Account_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}doi_tac_account_them_moi`;
    
    // Truyền thông
    public static readonly CoreMenu_TruyenThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}truyen_thong`;

    // Truyền thông - Tin tức
    public static readonly CoreMenu_TinTuc: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}tin_tuc`;
    public static readonly CoreTinTuc_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}tin_tuc_danh_sach`;
    public static readonly CoreTinTuc_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tin_tuc_them_moi`;
    public static readonly CoreTinTuc_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tin_tuc_cap_nhat`;
    public static readonly CoreTinTuc_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tin_tuc_xoa`;
    public static readonly CoreTinTuc_PheDuyetDang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}tin_tuc_phe_duyet_dang`;
  
    // Truyền thông - hình ảnh
    public static readonly CoreMenu_HinhAnh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}hinh_anh`;
    public static readonly CoreHinhAnh_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}hinh_anh_danh_sach`;
    public static readonly CoreHinhAnh_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hinh_anh_them_moi`;
    public static readonly CoreHinhAnh_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hinh_anh_cap_nhat`;
    public static readonly CoreHinhAnh_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hinh_anh_xoa`;
    public static readonly CoreHinhAnh_PheDuyetDang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}hinh_anh_phe_duyet_dang`;

    // Truyền thông - Kiến thức đầu tư
    public static readonly CoreMenu_KienThucDauTu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}kien_thuc_dau_tu`;
    public static readonly CoreKienThucDauTu_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}kien_thuc_dau_tu_danh_sach`;
    public static readonly CoreKienThucDauTu_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}kien_thuc_dau_tu_them_moi`;
    public static readonly CoreKienThucDauTu_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}kien_thuc_dau_tu_cap_nhat`;
    public static readonly CoreKienThucDauTu_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}kien_thuc_dau_tu_xoa`;
    public static readonly CoreKienThucDauTu_PheDuyetDang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}kien_thuc_dau_tu_phe_duyet_dang`;
    
    // Truyền thông - Hòm thư góp ý 
    public static readonly CoreMenu_HomThuGopY: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}hom_thu_gop_y`;

    // Thông báo 
    public static readonly CoreMenu_ThongBao: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}thong_bao`;
    
    // Thông báo - Thông báo mặc định
    public static readonly CoreMenu_ThongBaoMacDinh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}thong_bao_mac_dinh`;
    public static readonly CoreThongBaoMacDinh_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}thong_bao_mac_dinh_cap_nhat`;
    
    // Thông báo - Cấu hình thông báo hệ thống / thông báo server (Thiết lập)
    // public static readonly CoreMenu_CauHinhThongBaoHeThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong`;
    // public static readonly CoreCauHinhThongBaoHeThong_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong_cap_nhat`;

    // Thông báo - Mẫu thông báo
    public static readonly CoreMenu_MauThongBao: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}mau_thong_bao`;
    public static readonly CoreMauThongBao_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}mau_thong_bao_danh_sach`;
    public static readonly CoreMauThongBao_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_them_moi`;
    public static readonly CoreMauThongBao_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_cap_nhat`;
    public static readonly CoreMauThongBao_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_xoa`;
    public static readonly CoreMauThongBao_KichHoatOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_kich_hoat_hoac_huy`;

    // Thông báo - Quản lý thông báo = QLTB
    public static readonly CoreMenu_QLTB: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qltb`;
    public static readonly CoreQLTB_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qltb_danh_sach`;
    public static readonly CoreQLTB_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_them_moi`;
    public static readonly CoreQLTB_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_xoa`;
    public static readonly CoreQLTB_KichHoatOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_kich_hoat_hoac_huy`;

    // // Thông báo - Cấu hình nhà cung cấp
    // public static readonly CoreMenu_CauHinhNCC: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_ncc`;
    // public static readonly CoreCauHinhNCC_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_ncc_danh_sach`;
    // public static readonly CoreCauHinhNCC_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_them_moi`;
    // public static readonly CoreCauHinhNCC_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_cap_nhat`;
    //
    public static readonly CoreQLTB_PageChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}qltb_page_chi_tiet`;
    
    //
    public static readonly CoreQLTB_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}qltb_page_chi_tiet_thong_tin_chung`;
    public static readonly CoreQLTB_PageChiTiet_ThongTin: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}qltb_page_chi_tiet_thong_tin`;
    public static readonly CoreQLTB_PageChiTiet_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_page_chi_tiet_cap_nhat`;
    
    //
    public static readonly CoreQLTB_GuiThongBao: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}qltb_page_chi_tiet_gui_thong_bao`;
    public static readonly CoreQLTB_PageChiTiet_GuiThongBao_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}qltb_page_chi_tiet_gui_thong_bao_danh_sach`;
    public static readonly CoreQLTB_PageChiTiet_GuiThongBao_CaiDat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_page_chi_tiet_gui_thong_bao_cai_dat`;

    // Thiết lập
    public static readonly CoreMenu_ThietLap: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}thiet_lap`;

    // Thiết lập - Cấu hình thông báo hệ thống
    public static readonly CoreMenu_CauHinhThongBaoHeThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong`;
    public static readonly CoreCauHinhThongBaoHeThong_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong_cap_nhat`;

    // Thiết lập - Thông báo server
    public static readonly CoreMenu_CauHinhNCC: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_ncc`;
    public static readonly CoreCauHinhNCC_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_ncc_danh_sach`;
    public static readonly CoreCauHinhNCC_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_them_moi`;
    public static readonly CoreCauHinhNCC_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_cap_nhat`;

    // Thiết lập - Cấu hình chữ ký số
    public static readonly CoreMenu_CauHinhCKS: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_cks`;
    public static readonly CoreCauHinhCKS_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_cks_danh_sach`;
    
    // public static readonly CoreCauHinhCKS_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_cks_them_moi`;
    public static readonly CoreCauHinhCKS_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_cks_cap_nhat`;

    // Thiết lập - whitelist ip
    public static readonly CoreMenu_WhitelistIp: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}whitelist_ip`;
    public static readonly CoreWhitelistIp_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}whitelist_ip_danh_sach`;
    public static readonly CoreWhitelistIp_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_them_moi`;
    public static readonly CoreWhitelistIp_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_chi_tiet`;
    public static readonly CoreWhitelistIp_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_cap_nhat`;
    public static readonly CoreWhitelistIp_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_xoa`;

    // Thiết lập - Msb Prefix
    public static readonly CoreMenu_MsbPrefix: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}msb_prefix`;
    public static readonly CoreMsbPrefix_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}msb_prefix_danh_sach`;
    public static readonly CoreMsbPrefix_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}msb_prefix_them_moi`;
    public static readonly CoreMsbPrefix_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}msb_prefix_chi_tiet`;
    public static readonly CoreMsbPrefix_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}msb_prefix_cap_nhat`;
    public static readonly CoreMsbPrefix_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}msb_prefix_xoa`;

     // Thiết lập - cau hinh he thong
     public static readonly CoreMenu_CauHinhHeThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_he_thong`;
     public static readonly CoreCauHinhHeThong_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_he_thong_danh_sach`;
     public static readonly CoreCauHinhHeThong_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_he_thong_them_moi`;
     public static readonly CoreCauHinhHeThong_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_he_thong_chi_tiet`;
     public static readonly CoreCauHinhHeThong_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_he_thong_cap_nhat`;

     // Thiết lập - cau hinh cuoc goi
     public static readonly CoreMenu_CauHinhCuocGoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_cuoc_goi`;
     public static readonly CoreCauHinhCuocGoi_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_cuoc_goi_danh_sach`;
     public static readonly CoreCauHinhCuocGoi_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_cuoc_goi_cap_nhat`;

    // Quản lý phê duyệt = QLPD, Khách hàng cá nhân = KHCN, Khách hàng doanh nghiệp = KHDN, Nhà đầu tư chuyên nghiệp = NDTCN
    public static readonly CoreMenu_QLPD: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd`;
    
    // Phê duyệt khách hàng cá nhân
    public static readonly CoreQLPD_KHCN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_khcn`;
    public static readonly CoreQLPD_KHCN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_khcn_danh_sach`;
    public static readonly CoreQLPD_KHCN_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khcn_phe_duyet_or_huy`;
    public static readonly CoreQLPD_KHCN_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khcn_xem_lich_su`;
    public static readonly CoreQLPD_KHCN_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khcn_thong_tin_chi_tiet`;
    
    // Phê duyệt khách hàng doanh nghiệp
    public static readonly CoreQLPD_KHDN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_khdn`;
    public static readonly CoreQLPD_KHDN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_khdn_danh_sach`;
    public static readonly CoreQLPD_KHDN_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khdn_phe_duyet_or_huy`;
    public static readonly CoreQLPD_KHDN_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khdn_xem_lich_su`;
    public static readonly CoreQLPD_KHDN_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_khdn_thong_tin_chi_tiet`;
    
    // Phê duyệt nhà đầu tư chuyên nghiệp -----
    public static readonly CoreQLPD_NDTCN: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_ndtcn`;
    public static readonly CoreQLPD_NDTCN_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_ndtcn_danh_sach`;
    public static readonly CoreQLPD_NDTCN_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_ndtcn_phe_duyet_or_huy`;
    public static readonly CoreQLPD_NDTCN_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_ndtcn_xem_lich_su`;
    public static readonly CoreQLPD_NDTCN_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_ndtcn_thong_tin_chi_tiet`;
    
    // Phê duyệt email -----
    public static readonly CoreQLPD_Email: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_email`;
    public static readonly CoreQLPD_Email_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_email_danh_sach`;
    public static readonly CoreQLPD_Email_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_email_phe_duyet_or_huy`;
    public static readonly CoreQLPD_Email_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_email_xem_lich_su`;
    public static readonly CoreQLPD_Email_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_email_thong_tin_chi_tiet`;
   
    // Phê duyệt phone -----
   public static readonly CoreQLPD_Phone: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_phone`;
   public static readonly CoreQLPD_Phone_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_phone_danh_sach`;
   public static readonly CoreQLPD_Phone_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_phone_phe_duyet_or_huy`;
   public static readonly CoreQLPD_Phone_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_phone_xem_lich_su`;
   public static readonly CoreQLPD_Phone_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_phone_thong_tin_chi_tiet`;
  
    // Phê duyệt Sale ----
    public static readonly CoreQLPD_Sale: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qlpd_sale`;
    public static readonly CoreQLPD_Sale_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qlpd_sale_danh_sach`;
    public static readonly CoreQLPD_Sale_PheDuyetOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_sale_phe_duyet_or_huy`;
    public static readonly CoreQLPD_Sale_XemLichSu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_sale_xem_lich_su`;
    public static readonly CoreQLPD_Sale_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qlpd_sale_thong_tin_chi_tiet`;
    // Quản lý phòng ban
    public static readonly CoreMenu_PhongBan: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}phong_ban`;
    public static readonly CorePhongBan_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}phong_ban_danh_sach`;
    public static readonly CorePhongBan_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_them_moi`;
    public static readonly CorePhongBan_ThemQuanLy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_them_quan_ly`;
    public static readonly CorePhongBan_ThemQuanLyDoanhNgiep: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_them_quan_ly_doanh_nghiep`;
    public static readonly CorePhongBan_XoaQuanLy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_xoa_quan_ly`;
    public static readonly CorePhongBan_XoaQuanLyDoanhNgiep: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_xoa_quan_ly_doanh_nghiep`;
    public static readonly CorePhongBan_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_cap_nhat`;
    public static readonly CorePhongBan_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_xoa`;

    // Menu báo cáo
    public static readonly Core_Menu_BaoCao: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}bao_cao`;

    public static readonly Core_BaoCao_QuanTri: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}bao_cao_quan_tri`;

    public static readonly Core_BaoCao_QuanTri_DSSaler: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_ds_saler`;
    public static readonly Core_BaoCao_QuanTri_DSKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_ds_khach_hang`;
    public static readonly Core_BaoCao_QuanTri_DSKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_ds_khach_hang_root`;
    public static readonly Core_BaoCao_QuanTri_DSNguoiDung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_ds_nguoi_dung`;
    public static readonly Core_BaoCao_QuanTri_DSKhachHangHVF: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_ds_khach_hang_hvf`;
    // public static readonly Core_BaoCao_QuanTri_SKTKNhaDauTu: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_sktk_nha_dau_tu`;
    public static readonly Core_BaoCao_QuanTri_TDTTKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_thay_doi_tt_khach_hang`;
    public static readonly Core_BaoCao_QuanTri_TDTTKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_quan_tri_tdtt_khach_hang_root`;

    public static readonly Core_BaoCao_VanHanh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}bao_cao_van_hanh`;

    public static readonly Core_BaoCao_VanHanh_DSSaler: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_ds_saler`;
    public static readonly Core_BaoCao_VanHanh_DSKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_ds_khach_hang`;
    public static readonly Core_BaoCao_VanHanh_DSKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_ds_khach_hang_root`;
    public static readonly Core_BaoCao_VanHanh_DSNguoiDung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_ds_nguoi_dung`;
    public static readonly Core_BaoCao_VanHanh_TDTTKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_thay_doi_tt_khach_hang`;
    public static readonly Core_BaoCao_VanHanh_TDTTKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_van_hanh_tdtt_khach_hang_root`;

    public static readonly Core_BaoCao_KinhDoanh: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}bao_cao_kinh_doanh`;

    public static readonly Core_BaoCao_KinhDoanh_DSSaler: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_kinh_doanh_ds_saler`;
    public static readonly Core_BaoCao_KinhDoanh_DSKhachHang: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_kinh_doanh_ds_khach_hang`;
    public static readonly Core_BaoCao_KinhDoanh_DSKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_kinh_doanh_ds_khach_hang_root`;
    public static readonly Core_BaoCao_KinhDoanh_DSNguoiDung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_kinh_doanh_ds_nguoi_dung`;

    public static readonly Core_BaoCao_HeThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}bao_cao_he_thong`;
    
    public static readonly Core_BaoCao_HeThong_DSKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_he_thong_ds_khach_hang_root`;
    public static readonly Core_BaoCao_HeThong_TDTTKhachHangRoot: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}bao_cao_he_thong_tdtt_khach_hang_root`;
}
    // Dashboard
    PermissionCoreConfig[PermissionCoreConst.CorePageDashboard] = { type: PermissionTypes.Menu, name: 'Dashboard tổng quan', parentKey: null, icon: 'pi pi-fw pi-home' };
    //
    PermissionCoreConfig[PermissionCoreConst.CoreThongTinDoanhNghiep] = { type: PermissionTypes.Menu, name: 'Thông tin doanh nghiệp', parentKey: null, icon: 'pi pi-user' };
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreThongTinDoanhNghiep};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreTTDN_ThongTinChung};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_CauHinhChuKySo] = { type: PermissionTypes.ButtonAction, name: 'Cấu hình chữ ký số', parentKey: PermissionCoreConst.CoreTTDN_ThongTinChung};
    //
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_TKNganHang] = { type: PermissionTypes.Tab, name: 'Tài khoản ngân hàng', parentKey: PermissionCoreConst.CoreThongTinDoanhNghiep};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_TKNH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreTTDN_TKNganHang};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_TKNH_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Chọn mặc định', parentKey: PermissionCoreConst.CoreTTDN_TKNganHang};
    //
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_GiayPhepDKKD] = { type: PermissionTypes.Tab, name: 'Giấy phép ĐKKD', parentKey: PermissionCoreConst.CoreThongTinDoanhNghiep};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_GiayPhepDKKD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreTTDN_GiayPhepDKKD};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_GiayPhepDKKD_Sua] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreTTDN_GiayPhepDKKD};
    PermissionCoreConfig[PermissionCoreConst.CoreTTDN_GiayPhepDKKD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionCoreConst.CoreTTDN_GiayPhepDKKD};
    //  Tài khoản người dùng
    PermissionCoreConfig[PermissionCoreConst.Core_Menu_TK_UngDung] = { type: PermissionTypes.Menu, name: 'Tài khoản ứng dụng', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-id-card' };

    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount] = { type: PermissionTypes.Page, name: 'Tài khoản người dùng', parentKey: PermissionCoreConst.Core_Menu_TK_UngDung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount_ChiTiet] = { type: PermissionTypes.Menu, name: 'Chi tiết', parentKey: PermissionCoreConst.CorePageInvestorAccount };
    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount_ChangeStatus] = { type: PermissionTypes.Menu, name: 'Đóng/ Mở tài khoản', parentKey: PermissionCoreConst.CorePageInvestorAccount };
    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount_ResetMatKhau] = { type: PermissionTypes.Menu, name: 'Reset mật khẩu', parentKey: PermissionCoreConst.CorePageInvestorAccount };
    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount_DatMaPin] = { type: PermissionTypes.Menu, name: 'Đặt lại mã pin', parentKey: PermissionCoreConst.CorePageInvestorAccount };
    PermissionCoreConfig[PermissionCoreConst.CorePageInvestorAccount_XoaTaiKhoan] = { type: PermissionTypes.Menu, name: 'Xóa tài khoản', parentKey: PermissionCoreConst.CorePageInvestorAccount };
    
    PermissionCoreConfig[PermissionCoreConst.Core_TK_ChuaXacMinh] = { type: PermissionTypes.Page, name: 'Chưa xác minh', parentKey: PermissionCoreConst.Core_Menu_TK_UngDung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_TK_ChuaXacMinh_XacMinh] = { type: PermissionTypes.ButtonAction, name: 'Xác minh thông tin', parentKey: PermissionCoreConst.Core_TK_ChuaXacMinh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_TK_ChuaXacMinh_ResetMatKhau] = { type: PermissionTypes.ButtonAction, name: 'Reset mật khẩu', parentKey: PermissionCoreConst.Core_TK_ChuaXacMinh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_TK_ChuaXacMinh_XoaTaiKhoan] = { type: PermissionTypes.ButtonAction, name: 'Xóa tài khoản', parentKey: PermissionCoreConst.Core_TK_ChuaXacMinh, webKey: WebKeys.Core };

    // Quản lý khách hàng
    PermissionCoreConfig[PermissionCoreConst.CoreMenuKhachHang] = { type: PermissionTypes.Menu, name: 'Quản lý khách hàng', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-users', };

    // Quản lý khách hàng cá nhân chưa duyệt --------- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenuDuyetKHCN] = { type: PermissionTypes.Menu, name: 'Duyệt KH cá nhân', parentKey: PermissionCoreConst.CoreMenuKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuDuyetKHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenuDuyetKHCN, webKey: WebKeys.Core };
    
    // Chi tiết khách hàng
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang] = { type: PermissionTypes.Page, name: 'Thông tin khách hàng', parentKey: PermissionCoreConst.CoreMenuDuyetKHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_CheckFace] = { type: PermissionTypes.ButtonAction, name: 'Kiểm tra mặt', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinChung, webKey: WebKeys.Core };
    
    // Tab tài khoản ngân hàng
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH] = { type: PermissionTypes.Tab, name: 'Tài khoản ngân hàng', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKNH_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKNH, webKey: WebKeys.Core };
    
    // Tab tài khoản chứng khoán
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKCK] = { type: PermissionTypes.Tab, name: 'Tài khoản chứng khoán', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKCK_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKCK, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKCK_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKCK, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_TKCK_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreDuyetKHCN_TKCK, webKey: WebKeys.Core };
   
    // Tab quản lý giấy tờ
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_GiayTo] = { type: PermissionTypes.Tab, name: 'Quản lý giấy tờ', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_GiayTo_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_GiayTo_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_GiayTo_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDuyetKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_GiayTo_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreDuyetKHCN_GiayTo, webKey: WebKeys.Core };
    
    // Tab quản lý địa chỉ liên hệ
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_DiaChi] = { type: PermissionTypes.Tab, name: 'Địa chỉ liên hệ', parentKey: PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_DiaChi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHCN_DiaChi, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_DiaChi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHCN_DiaChi, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHCN_DiaChi_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreDuyetKHCN_DiaChi, webKey: WebKeys.Core };
    // --------------- End

    // Quản lý khách hàng cá nhân đã duyệt ------- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenuKHCN] = { type: PermissionTypes.Menu, name: 'Khách hàng cá nhân', parentKey: PermissionCoreConst.CoreMenuKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuKHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_XacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionCoreConst.CoreMenuKHCN, webKey: WebKeys.Core };
    
    // Chi tiết khách hàng (Thông tin chung)
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_ThongTinKhachHang] = { type: PermissionTypes.Page, name: 'Thông tin khách hàng', parentKey: PermissionCoreConst.CoreMenuKHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_CheckFace] = { type: PermissionTypes.ButtonAction, name: 'Kiểm tra mặt', parentKey: PermissionCoreConst.CoreKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DoiSDT] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu đổi số điện thoại', parentKey: PermissionCoreConst.CoreKHCN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DoiEmail] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu đổi email', parentKey: PermissionCoreConst.CoreKHCN_ThongTinChung, webKey: WebKeys.Core };
    
    // Tab tài khoản ngân hàng
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH] = { type: PermissionTypes.Tab, name: 'Tài khoản ngân hàng', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionCoreConst.CoreKHCN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKNH_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreKHCN_TKNH, webKey: WebKeys.Core };
  
    // Tab tài khoản chứng khoán
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKCK] = { type: PermissionTypes.Tab, name: 'Tài khoản chứng khoán', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKCK_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_TKCK, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKCK_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHCN_TKCK, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TKCK_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHCN_TKCK, webKey: WebKeys.Core };

    // Tab tài khoản đăng nhập
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account] = { type: PermissionTypes.Tab, name: 'Tài khoản đăng nhập', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_Account, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account_ResetPassword] = { type: PermissionTypes.ButtonAction, name: 'Reset mật khẩu', parentKey: PermissionCoreConst.CoreKHCN_Account, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account_ResetPin] = { type: PermissionTypes.ButtonAction, name: 'Reset mã Pin', parentKey: PermissionCoreConst.CoreKHCN_Account, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account_ChangeStatus] = { type: PermissionTypes.ButtonAction, name: 'Đóng/ Mở tài khoản', parentKey: PermissionCoreConst.CoreKHCN_Account, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_Account_Delete] = { type: PermissionTypes.ButtonAction, name: 'Xóa tài khoản', parentKey: PermissionCoreConst.CoreKHCN_Account, webKey: WebKeys.Core };
    
    // Tab quản lý giấy tờ
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_GiayTo] = { type: PermissionTypes.Tab, name: 'Quản lý giấy tờ', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_GiayTo_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_GiayTo_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_GiayTo_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreKHCN_GiayTo, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_GiayTo_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHCN_GiayTo, webKey: WebKeys.Core };
    
    // Tab quản lý địa chỉ liên hệ
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DiaChi] = { type: PermissionTypes.Tab, name: 'Địa chỉ liên hệ', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DiaChi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_DiaChi, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DiaChi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHCN_DiaChi, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_DiaChi_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHCN_DiaChi, webKey: WebKeys.Core };
    
    // Tab Chứng minh Nhà đầu tư chuyên nghiệp
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_NDTCN] = { type: PermissionTypes.Tab, name: 'Chứng minh NĐTCN', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_NDTCN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_NDTCN, webKey: WebKeys.Core };
    
    // Tab Chứng minh Nhà đầu tư chuyên nghiệp
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TuVanVien] = { type: PermissionTypes.Tab, name: 'Tư vấn viên', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TuVanVien_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_TuVanVien, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TuVanVien_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHCN_TuVanVien, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_TuVanVien_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHCN_TuVanVien, webKey: WebKeys.Core };
    
    // Tab Chứng minh Nhà đầu tư chuyên nghiệp
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_NguoiGioiThieu] = { type: PermissionTypes.Tab, name: 'Người giới thiệu', parentKey: PermissionCoreConst.CoreKHCN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHCN_NguoiGioiThieu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHCN_NguoiGioiThieu, webKey: WebKeys.Core };
    // ------------ End

    // Quản lý khách hàng doanh nghiệp chưa duyệt ----- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenuDuyetKHDN] = { type: PermissionTypes.Menu, name: 'Duyệt KH doanh nghiệp', parentKey: PermissionCoreConst.CoreMenuKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuDuyetKHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenuDuyetKHDN, webKey: WebKeys.Core };
    
    // Chi tiết khách hàng
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang] = { type: PermissionTypes.Page, name: 'Thông tin khách hàng', parentKey: PermissionCoreConst.CoreMenuDuyetKHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinChung, webKey: WebKeys.Core };
    
    // Tab tài khoản ngân hàng
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_TKNH] = { type: PermissionTypes.Tab, name: 'Tài khoản ngân hàng', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_TKNH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHDN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_TKNH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHDN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_TKNH_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreDuyetKHDN_TKNH, webKey: WebKeys.Core };
    
    // Tab Chữ ký số
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_CKS] = { type: PermissionTypes.Tab, name: 'Chữ ký số', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_CauHinhChuKySo] = { type: PermissionTypes.Table, name: 'Cấu hình chữ ký số', parentKey: PermissionCoreConst.CoreDuyetKHDN_CKS, webKey: WebKeys.Core };
    
    // Tab Đăng ký kinh doanh
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD] = { type: PermissionTypes.Tab, name: 'Đăng ký kinh doanh', parentKey: PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_XemFile] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_TaiFile] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetKHDN_DKKD_XoaFile] = { type: PermissionTypes.ButtonAction, name: 'Xóa file', parentKey: PermissionCoreConst.CoreDuyetKHDN_DKKD, webKey: WebKeys.Core };
    // ------------ End

    // Quản lý khách hàng doanh nghiệp đã duyệt ---------- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenuKHDN] = { type: PermissionTypes.Menu, name: 'Khách hàng doanh nghiệp', parentKey: PermissionCoreConst.CoreMenuKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuKHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_XacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionCoreConst.CoreMenuKHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_ThongTinKhachHang] = { type: PermissionTypes.Page, name: 'Thông tin khách hàng', parentKey: PermissionCoreConst.CoreMenuKHDN, webKey: WebKeys.Core };
    
    // Chi tiết khách hàng
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreKHDN_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreKHDN_ThongTinChung, webKey: WebKeys.Core };
    
    // Tab tài khoản ngân hàng
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_TKNH] = { type: PermissionTypes.Tab, name: 'Tài khoản ngân hàng', parentKey: PermissionCoreConst.CoreKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_TKNH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHDN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_TKNH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHDN_TKNH, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_TKNH_SetDefault] = { type: PermissionTypes.ButtonAction, name: 'Set mặc định', parentKey: PermissionCoreConst.CoreKHDN_TKNH, webKey: WebKeys.Core };
    
    // Tab Chữ ký số
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_CKS] = { type: PermissionTypes.Tab, name: 'Chữ ký số', parentKey: PermissionCoreConst.CoreKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_CauHinhChuKySo] = { type: PermissionTypes.Table, name: 'Cấu hình chữ ký số', parentKey: PermissionCoreConst.CoreKHDN_CKS, webKey: WebKeys.Core };

    // Tab Đăng ký kinh doanh
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD] = { type: PermissionTypes.Tab, name: 'Đăng ký kinh doanh', parentKey: PermissionCoreConst.CoreKHDN_ThongTinKhachHang, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_XemFile] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_TaiFile] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKHDN_DKKD_XoaFile] = { type: PermissionTypes.ButtonAction, name: 'Xóa file', parentKey: PermissionCoreConst.CoreKHDN_DKKD, webKey: WebKeys.Core };
    // ------------ End

    // Quản lý Sale 
    PermissionCoreConfig[PermissionCoreConst.CoreMenuSale] = { type: PermissionTypes.Menu, name: 'Quản lý Sale', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-users' };

    // Danh sách Sale chưa duyệt ------- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenuDuyetSale] = { type: PermissionTypes.Menu, name: 'Duyệt Sale', parentKey: PermissionCoreConst.CoreMenuSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuDuyetSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenuDuyetSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_ThongTinSale] = { type: PermissionTypes.Page, name: 'Thông tin Sale', parentKey: PermissionCoreConst.CoreMenuDuyetSale, webKey: WebKeys.Core };
    
    // Chi tiết sale chưa duyệt
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreDuyetSale_ThongTinSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreDuyetSale_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDuyetSale_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDuyetSale_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionCoreConst.CoreDuyetSale_ThongTinChung, webKey: WebKeys.Core };

    // Danh sách Sale đã duyệt
    PermissionCoreConfig[PermissionCoreConst.CoreMenuSaleActive] = { type: PermissionTypes.Menu, name: 'Sale', parentKey: PermissionCoreConst.CoreMenuSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuSaleActive, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_KichHoat] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/ Khóa Sale', parentKey: PermissionCoreConst.CoreMenuSaleActive, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_ThongTinSale] = { type: PermissionTypes.Page, name: 'Thông tin Sale', parentKey: PermissionCoreConst.CoreMenuSaleActive, webKey: WebKeys.Core };
    
    // Chi tiết sale đã duyệt
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreSaleActive_ThongTinSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreSaleActive_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreSaleActive_ThongTinChung, webKey: WebKeys.Core };
    
    // Tab Hợp đồng đầu tư = HDDT
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT] = { type: PermissionTypes.Tab, name: 'Hợp đồng cộng tác', parentKey: PermissionCoreConst.CoreSaleActive_ThongTinSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_UpdateFile] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hồ sơ', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_Sign] = { type: PermissionTypes.ButtonAction, name: 'Ký điện tử', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_UploadFile] = { type: PermissionTypes.ButtonAction, name: 'Tải lên hợp đồng', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_Preview] = { type: PermissionTypes.ButtonAction, name: 'Xem hồ sơ tải lên', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_Download] = { type: PermissionTypes.ButtonAction, name: 'Tải hợp đồng mẫu', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreSaleActive_HDCT_Download_Sign] = { type: PermissionTypes.ButtonAction, name: 'Tải hợp đồng chữ ký điện tử', parentKey: PermissionCoreConst.CoreSaleActive_HDCT, webKey: WebKeys.Core };
    // ------------- End

    // Danh sách Sale App 
    PermissionCoreConfig[PermissionCoreConst.CoreMenuSaleApp] = { type: PermissionTypes.Menu, name: 'Sale App', parentKey: PermissionCoreConst.CoreMenuSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleApp_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenuSaleApp, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreSaleApp_DieuHuong] = { type: PermissionTypes.Table, name: 'Điều hướng / Hủy', parentKey: PermissionCoreConst.CoreMenuSaleApp, webKey: WebKeys.Core };

    // Mẫu hợp đồng cộng tác -----Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_HDCT_Template] = { type: PermissionTypes.Menu, name: 'Mẫu hợp đồng cộng tác', parentKey: PermissionCoreConst.CoreMenuSale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_Preview] = { type: PermissionTypes.ButtonAction, name: 'Xem mẫu hợp đồng', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_Download] = { type: PermissionTypes.ButtonAction, name: 'Tải mẫu hợp đồng', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHDCT_Template_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa mẫu', parentKey: PermissionCoreConst.CoreMenu_HDCT_Template, webKey: WebKeys.Core };
    
    //  Phòng ban
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_PhongBan] = { type: PermissionTypes.Menu, name: 'Phòng ban', parentKey: PermissionCoreConst.CoreMenuSale, webKey: WebKeys.Core};
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_ThemQuanLy] = { type: PermissionTypes.ButtonAction, name: 'Thêm quản lý', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_ThemQuanLyDoanhNgiep] = { type: PermissionTypes.ButtonAction, name: 'Thêm quản lý doanh nghiệp', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_XoaQuanLy] = { type: PermissionTypes.ButtonAction, name: 'Xóa quản lý', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_XoaQuanLyDoanhNgiep] = { type: PermissionTypes.ButtonAction, name: 'Xóa quản lý doanh nghiệp', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CorePhongBan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_PhongBan, webKey: WebKeys.Core };

    // ---------- End

    //QL Đối tác ------Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_QLDoiTac] = { type: PermissionTypes.Menu, name: 'Quản lý đối tác', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-sitemap' };
    
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_DoiTac] = { type: PermissionTypes.Menu, name: 'Đối tác', parentKey: PermissionCoreConst.CoreMenu_QLDoiTac, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_DoiTac, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_DoiTac, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_DoiTac, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreMenu_DoiTac, webKey: WebKeys.Core };
    
    // Tab thông tin chung
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreDoiTac_ThongTinChiTiet, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_XemChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreDoiTac_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreDoiTac_ThongTinChung, webKey: WebKeys.Core };
    
    //dai ly
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_DaiLy] = { type: PermissionTypes.Menu, name: 'Đại lý', parentKey: PermissionCoreConst.CoreMenu_QLDoiTac, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDaiLy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_DaiLy, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDaiLy_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_DaiLy, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDaiLy_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreMenu_DaiLy, webKey: WebKeys.Core };
    
    // Tab thông tin chung
    PermissionCoreConfig[PermissionCoreConst.CoreDaiLy_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreDaiLy_ThongTinChiTiet, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDaiLy_XemChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreDaiLy_ThongTinChung, webKey: WebKeys.Core };

    // Tab quản lý tài khoản đăng nhập
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_Account] = { type: PermissionTypes.Tab, name: 'Tài khoản', parentKey: PermissionCoreConst.CoreDoiTac_ThongTinChiTiet, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_Account_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreDoiTac_Account, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreDoiTac_Account_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreDoiTac_Account, webKey: WebKeys.Core };
    // --------- End

    // Truyền thông ------- Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_TruyenThong] = { type: PermissionTypes.Menu, name: 'Truyền thông', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-send' };

    // Truyền thông - Tin Tức
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_TinTuc] = { type: PermissionTypes.Menu, name: 'Tin tức', parentKey: PermissionCoreConst.CoreMenu_TruyenThong, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreTinTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_TinTuc, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreTinTuc_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_TinTuc, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreTinTuc_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_TinTuc, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreTinTuc_PheDuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt đăng', parentKey: PermissionCoreConst.CoreMenu_TinTuc, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreTinTuc_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_TinTuc, webKey: WebKeys.Core };


    // Truyền thông - Hình ảnh
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_HinhAnh] = { type: PermissionTypes.Menu, name: 'Hình ảnh', parentKey: PermissionCoreConst.CoreMenu_TruyenThong, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHinhAnh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_HinhAnh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_HinhAnh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_HinhAnh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHinhAnh_PheDuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt đăng', parentKey: PermissionCoreConst.CoreMenu_HinhAnh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_HinhAnh, webKey: WebKeys.Core };


    // Truyền thông - Kiến thức đầu tư
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_KienThucDauTu] = { type: PermissionTypes.Menu, name: 'Kiến thức đầu tư', parentKey: PermissionCoreConst.CoreMenu_TruyenThong, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKienThucDauTu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_KienThucDauTu, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKienThucDauTu_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_KienThucDauTu, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKienThucDauTu_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_KienThucDauTu, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKienThucDauTu_PheDuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt đăng', parentKey: PermissionCoreConst.CoreMenu_KienThucDauTu, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreKienThucDauTu_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_KienThucDauTu, webKey: WebKeys.Core };


    // Truyền thông - Hòm thư góp ý
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_HomThuGopY] = { type: PermissionTypes.Menu, name: 'Hòm thư góp ý', parentKey: PermissionCoreConst.CoreMenu_TruyenThong, webKey: WebKeys.Core };
    // -----------End

    // Meu Thông báo -------Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_ThongBao] = { type: PermissionTypes.Menu, name: 'Thông báo', parentKey: null, webKey: WebKeys.Core , icon: 'pi pi-comment'};

    // // Thông báo - Thông báo mặc định
    // PermissionCoreConfig[PermissionCoreConst.CoreMenu_ThongBaoMacDinh] = { type: PermissionTypes.Menu, name: 'Thông báo mặc định', parentKey: PermissionCoreConst.CoreMenu_ThongBao, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreThongBaoMacDinh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật cài đặt', parentKey: PermissionCoreConst.CoreMenu_ThongBaoMacDinh, webKey: WebKeys.Core };

    // // Thông báo - Cấu hình thông báo hệ thống
    // PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Cấu hình thông báo', parentKey: PermissionCoreConst.CoreMenu_ThongBao, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreCauHinhThongBaoHeThong_CapNhat] = { type: PermissionTypes.Menu, name: 'Cập nhật cài đặt', parentKey: PermissionCoreConst.CoreMenu_CauHinhThongBaoHeThong, webKey: WebKeys.Core };

    // Thông báo - Mẫu thông báo
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_MauThongBao] = { type: PermissionTypes.Menu, name: 'Mẫu thông báo', parentKey: PermissionCoreConst.CoreMenu_ThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreMauThongBao_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_MauThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreMauThongBao_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_MauThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreMauThongBao_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_MauThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreMauThongBao_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy kích hoạt', parentKey: PermissionCoreConst.CoreMenu_MauThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreMauThongBao_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_MauThongBao, webKey: WebKeys.Core };


    // Thông báo - Quản lý thông báo
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_QLTB] = { type: PermissionTypes.Menu, name: 'Quản lý thông báo', parentKey: PermissionCoreConst.CoreMenu_ThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_QLTB, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_QLTB, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy kích hoạt', parentKey: PermissionCoreConst.CoreMenu_QLTB, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_QLTB, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_PageChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreMenu_QLTB, webKey: WebKeys.Core };
    
    // // Thông báo - Cấu hình nhà cung cấp
    // PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhNCC] = { type: PermissionTypes.Menu, name: 'Cấu hình nhà cung cấp', parentKey: PermissionCoreConst.CoreMenu_ThongBao, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };
    
    // Chi tiết thông báo
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionCoreConst.CoreQLTB_PageChiTiet, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_PageChiTiet_ThongTin] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionCoreConst.CoreQLTB_ThongTinChung, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_PageChiTiet_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreQLTB_ThongTinChung, webKey: WebKeys.Core };

    // Tab Gửi thông báo
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_GuiThongBao] = { type: PermissionTypes.Menu, name: 'Gửi thông báo', parentKey: PermissionCoreConst.CoreQLTB_PageChiTiet, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_PageChiTiet_GuiThongBao_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách thông báo', parentKey: PermissionCoreConst.CoreQLTB_GuiThongBao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLTB_PageChiTiet_GuiThongBao_CaiDat] = { type: PermissionTypes.ButtonAction, name: 'Cài đặt danh sách thông báo', parentKey: PermissionCoreConst.CoreQLTB_GuiThongBao, webKey: WebKeys.Core };

    // Thiết lập -------Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_ThietLap] = { type: PermissionTypes.Menu, name: 'Thiết lập', parentKey: null, webKey: WebKeys.Core , icon: 'pi pi-th-large'};

    // Thiết lập - Thông báo mặc định
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_ThongBaoMacDinh] = { type: PermissionTypes.Menu, name: 'Thông báo mặc định', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreThongBaoMacDinh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật cài đặt', parentKey: PermissionCoreConst.CoreMenu_ThongBaoMacDinh, webKey: WebKeys.Core };

    // Thiết lập - Cấu hình thông báo hệ thống
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Cấu hình thông báo', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhThongBaoHeThong_CapNhat] = { type: PermissionTypes.Menu, name: 'Cập nhật cài đặt', parentKey: PermissionCoreConst.CoreMenu_CauHinhThongBaoHeThong, webKey: WebKeys.Core };
    
    // Thiết lập - Thống báo server (Cấu hình nhà cung cấp)
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhNCC] = { type: PermissionTypes.Menu, name: 'Cấu hình nhà cung cấp', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_CauHinhNCC, webKey: WebKeys.Core };

    // Thiết lập - Cấu hình chữ ký số
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhCKS] = { type: PermissionTypes.Menu, name: 'Cấu hình chữ ký số', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhCKS_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_CauHinhCKS, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.CoreCauHinhNCC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_CauHinhCKS, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreCauHinhCKS_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_CauHinhCKS, webKey: WebKeys.Core };

    // Thiết lập - Cấu hình chữ ký số
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_WhitelistIp] = { type: PermissionTypes.Menu, name: 'Whitelist IP', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreWhitelistIp_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_WhitelistIp, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreWhitelistIp_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_WhitelistIp, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreWhitelistIp_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionCoreConst.CoreMenu_WhitelistIp, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreWhitelistIp_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_WhitelistIp, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreWhitelistIp_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_WhitelistIp, webKey: WebKeys.Core };

      // Thiết lập - MsbPrefix
      PermissionCoreConfig[PermissionCoreConst.CoreMenu_MsbPrefix] = { type: PermissionTypes.Menu, name: 'Tiền số giao dịch MSB', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreMsbPrefix_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_MsbPrefix, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreMsbPrefix_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_MsbPrefix, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreMsbPrefix_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionCoreConst.CoreMenu_MsbPrefix, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreMsbPrefix_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_MsbPrefix, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreMsbPrefix_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionCoreConst.CoreMenu_MsbPrefix, webKey: WebKeys.Core };

      // Thiết lập - Cau hinh he thong
      PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhHeThong] = { type: PermissionTypes.Menu, name: 'Cấu hình hệ thống', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhHeThong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_CauHinhHeThong, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhHeThong_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionCoreConst.CoreMenu_CauHinhHeThong, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhHeThong_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionCoreConst.CoreMenu_CauHinhHeThong, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhHeThong_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_CauHinhHeThong, webKey: WebKeys.Core };

      // Thiết lập - Cau hinh cuoc goi
      PermissionCoreConfig[PermissionCoreConst.CoreMenu_CauHinhCuocGoi] = { type: PermissionTypes.Menu, name: 'Cấu hình cuộc gọi', parentKey: PermissionCoreConst.CoreMenu_ThietLap, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhCuocGoi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreMenu_CauHinhCuocGoi, webKey: WebKeys.Core };
      PermissionCoreConfig[PermissionCoreConst.CoreCauHinhCuocGoi_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionCoreConst.CoreMenu_CauHinhCuocGoi, webKey: WebKeys.Core };

    // Quản lý phê duyệt --------Start
    PermissionCoreConfig[PermissionCoreConst.CoreMenu_QLPD] = { type: PermissionTypes.Menu, name: 'Quản lý phê duyệt', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-check-circle' };

    // Phê duyệt khách hàng cá nhân
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHCN] = { type: PermissionTypes.Menu, name: 'Phê duyệt khách hàng cá nhân', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHCN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_KHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHCN_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_KHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHCN_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_KHCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHCN_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_KHCN, webKey: WebKeys.Core };

    //  Phê duyệt khách hàng doanh nghiệp
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHDN] = { type: PermissionTypes.Menu, name: 'Phê duyệt khách hàng doanh nghiệp', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_KHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHDN_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_KHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHDN_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_KHDN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_KHDN_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_KHDN, webKey: WebKeys.Core };

    //  Phê duyệt nhà đầu tư chuyên nghiệp -----
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_NDTCN] = { type: PermissionTypes.Menu, name: 'Phê duyệt nhà đầu tư chuyên nghiệp', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_NDTCN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_NDTCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_NDTCN_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_NDTCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_NDTCN_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_NDTCN, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_NDTCN_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_NDTCN, webKey: WebKeys.Core };

    //  Phê duyệt email -----
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Email] = { type: PermissionTypes.Menu, name: 'Phê duyệt Email', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Email_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_Email, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Email_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_Email, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Email_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_Email, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Email_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_Email, webKey: WebKeys.Core };

    //  Phê duyệt phone -----
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Phone] = { type: PermissionTypes.Menu, name: 'Phê duyệt số điện thoại', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Phone_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_Phone, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Phone_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_Phone, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Phone_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_Phone, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Phone_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_Phone, webKey: WebKeys.Core };

    //  Phê duyệt Sale
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Sale] = { type: PermissionTypes.Menu, name: 'Phê duyệt Sale', parentKey: PermissionCoreConst.CoreMenu_QLPD, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Sale_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionCoreConst.CoreQLPD_Sale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Sale_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionCoreConst.CoreQLPD_Sale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Sale_XemLichSu] = { type: PermissionTypes.ButtonAction, name: 'Xem lịch sử', parentKey: PermissionCoreConst.CoreQLPD_Sale, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.CoreQLPD_Sale_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionCoreConst.CoreQLPD_Sale, webKey: WebKeys.Core };
    // ------------ End

    // Báo cáo
    PermissionCoreConfig[PermissionCoreConst.Core_Menu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.Core, icon: 'pi pi-file-export' };

    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionCoreConst.Core_Menu_BaoCao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_DSSaler] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách saler', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_DSKhachHang] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_DSKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_DSNguoiDung] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách người dùng', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_DSKhachHangHVF] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng HVF', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    // PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_SKTKNhaDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách sao kê TK nhà đầu tư', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_TDTTKhachHang] = { type: PermissionTypes.ButtonAction, name: 'B.C thay đổi thông tin khách hàng', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_QuanTri_TDTTKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C thay đổi thông tin khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_QuanTri, webKey: WebKeys.Core };

    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh] = { type: PermissionTypes.Page, name: 'Báo cáo vận hành', parentKey: PermissionCoreConst.Core_Menu_BaoCao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_DSSaler] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách saler', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_DSKhachHang] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_DSKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_DSNguoiDung] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách người dùng', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_TDTTKhachHang] = { type: PermissionTypes.ButtonAction, name: 'B.C Thay đổi thông tin khách hàng', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_VanHanh_TDTTKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Thay đổi thông tin khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_VanHanh, webKey: WebKeys.Core };

    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_KinhDoanh] = { type: PermissionTypes.Page, name: 'Báo cáo kinh doanh', parentKey: PermissionCoreConst.Core_Menu_BaoCao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_KinhDoanh_DSSaler] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách saler', parentKey: PermissionCoreConst.Core_BaoCao_KinhDoanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_KinhDoanh_DSKhachHang] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng', parentKey: PermissionCoreConst.Core_BaoCao_KinhDoanh, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_KinhDoanh_DSKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_KinhDoanh, webKey: WebKeys.Core };

    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_HeThong] = { type: PermissionTypes.Page, name: 'Báo cáo hệ thống', parentKey: PermissionCoreConst.Core_Menu_BaoCao, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_HeThong_DSKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_HeThong, webKey: WebKeys.Core };
    PermissionCoreConfig[PermissionCoreConst.Core_BaoCao_HeThong_TDTTKhachHangRoot] = { type: PermissionTypes.ButtonAction, name: 'B.C Thay đổi thông tin khách hàng root', parentKey: PermissionCoreConst.Core_BaoCao_HeThong, webKey: WebKeys.Core };


//
export default PermissionCoreConfig;