export class AppConsts {
    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;
    static nodeBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    static redicrectHrefOpenDocs = "https://docs.google.com/viewerng/viewer?url=";
    static baseUrlHome: string;

    static readonly grantType = {
        password: 'password',
        refreshToken: 'refresh_token',
    };

    static readonly messageError = "Có lỗi xảy ra. Vui lòng thử lại sau ít phút!";

    static clientId: string;
    static clientSecret: string;

    static keyCrypt = 'idCrypt';

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly authorization = {
        accessToken: 'access_token',
        refreshToken: 'refresh_token',
        encryptedAuthTokenName: 'enc_auth_token'
    };

    static readonly localRefreshAction = {
        setToken: 'setToken',
        doNothing: 'doNothing',
    }
    static ApproveConst: any;

    static readonly folder = 'real-esate';
    static tradingProviderIds: [];
    static listAPIUseTradingProviderIds = [
        '/api/real-estate/order/find-all',
        '/api/real-estate/project/get-all',
        '/api/real-estate/order/processing/find-all',
        '/api/real-estate/order/active/find-all',
        '/api/invest/order/find-cancel',
        '/api/invest/order/renewals-request',
        '/api/invest/order/invest-history'
    ]
    static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";
}

export class ReplaceIdentificationConst { 

    public static reason = [
        {
            code: 1,
            name: 'Giấy tờ bị mờ',
        },
        {
            code: 2,
            name: 'Giấy tờ bị rách',
        },
        {
            code: 3,
            name: 'Giấy tờ hết hạn',
        },
        {
            code: 4,
            name: 'Khác',
        },
    ];
    public static KHAC = 4;

}

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
    public static readonly CoreWeb: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Web}`;
    
    //
    public static readonly CorePageDashboard: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}dashboard`;

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
   
    // tab TKCK = Tài khoản chứng khoán
    public static readonly CoreDuyetKHCN_TKCK: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}duyet_khcn_tkck`;
    public static readonly CoreDuyetKHCN_TKCK_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}duyet_khcn_tkck_danh_sach`;
    public static readonly CoreDuyetKHCN_TKCK_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tkck_them_moi`;
    public static readonly CoreDuyetKHCN_TKCK_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khcn_tkck_set_default`;
   
    // tab TKCK = Tài khoản chứng khoán
    // public static readonly CoreKHCN_TKCK: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}khcn_tkck`;
    // public static readonly CoreKHCN_TKCK_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}khcn_tkck_danh_sach`;
    // public static readonly CoreKHCN_TKCK_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tkck_them_moi`;
    // public static readonly CoreKHCN_TKCK_SetDefault: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}khcn_tkck_set_default`;

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
   
    // tab TKCK = Tài khoản chứng khoán
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
    //

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
    public static readonly CoreDuyetKHDN_DKKD_XemFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_xem_file`;
    public static readonly CoreDuyetKHDN_DKKD_TaiFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_tai_file`;
    public static readonly CoreDuyetKHDN_DKKD_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_cap_nhat`;
    public static readonly CoreDuyetKHDN_DKKD_XoaFile: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}duyet_khdn_dkkd_xoa_file`;

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
    public static readonly CoreSaleActive_HDCT_Sign: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}sale_active_hdct_ky_dien_tu`;
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
        
    //dai ly
    public static readonly CoreMenu_DaiLy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}doi_tac`;
    public static readonly CoreDaiLy_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}doi_tac_danh_sach`;
    public static readonly CoreDaiLy_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}doi_tac_them_moi`;
        
    //
    public static readonly CoreDaiLy_ThongTinChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Page}doi_tac_thong_tin_chi_tiet`;
    public static readonly CoreDaiLy_ThongTinChung: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Tab}doi_tac_thong_tin_chung`;
    public static readonly CoreDaiLy_XemChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Form}doi_tac_xem_chi_tiet`;
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
    
    // Thiết lập
    public static readonly CoreMenu_ThietLap: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}thiet_lap`;

    // Thông báo - Cấu hình thông báo hệ thống
    public static readonly CoreMenu_CauHinhThongBaoHeThong: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong`;
    public static readonly CoreCauHinhThongBaoHeThong_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_thong_bao_he_thong_cap_nhat`;

    // Thông báo - Mẫu thông báo
    public static readonly CoreMenu_MauThongBao: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}mau_thong_bao`;
    public static readonly CoreMauThongBao_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}mau_thong_bao_danh_sach`;
    public static readonly CoreMauThongBao_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_them_moi`;
    public static readonly CoreMauThongBao_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_cap_nhat`;
    public static readonly CoreMauThongBao_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_xoa`;
    public static readonly CoreMauThongBao_KichHoatOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}mau_thong_bao_kich_hoat_hoac_huy`;

    // Thông báo - Cấu hình nhà cung cấp
    public static readonly CoreMenu_CauHinhNCC: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_ncc`;
    public static readonly CoreCauHinhNCC_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_ncc_danh_sach`;
    public static readonly CoreCauHinhNCC_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_them_moi`;
    public static readonly CoreCauHinhNCC_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_ncc_cap_nhat`;
   
    // Thông báo - Cấu hình chữ ký số
    public static readonly CoreMenu_CauHinhCKS: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}cau_hinh_cks`;
    public static readonly CoreCauHinhCKS_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}cau_hinh_cks_danh_sach`;
    
    // public static readonly CoreCauHinhCKS_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_cks_them_moi`;
    public static readonly CoreCauHinhCKS_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}cau_hinh_cks_cap_nhat`;
   
    // Thiết lập - Whitelist IP
    public static readonly CoreMenu_WhitelistIp: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}whitelist_ip`;
    public static readonly CoreWhitelistIp_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}whitelist_ip_danh_sach`;
    public static readonly CoreWhitelistIp_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_them_moi`;
    public static readonly CoreWhitelistIp_ChiTiet: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_chi_tiet`;
    public static readonly CoreWhitelistIp_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_cap_nhat`;
    public static readonly CoreWhitelistIp_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}whitelist_ip_xoa`;

    // Thiết lập - Whitelist IP
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

    // Thông báo - Quản lý thông báo = QLTB
    public static readonly CoreMenu_QLTB: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Menu}qltb`;
    public static readonly CoreQLTB_DanhSach: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.Table}qltb_danh_sach`;
    public static readonly CoreQLTB_ThemMoi: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_them_moi`;
    public static readonly CoreQLTB_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_xoa`;
    public static readonly CoreQLTB_KichHoatOrHuy: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}qltb_kich_hoat_hoac_huy`;
    
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
    public static readonly CorePhongBan_ThemQuanLyDoanhNghiep: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_them_quan_ly_doanh_nghiep`;
    public static readonly CorePhongBan_CapNhat: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_cap_nhat`;
    public static readonly CorePhongBan_Xoa: string = `${PermissionCoreConst.CoreModule}${PermissionCoreConst.ButtonAction}phong_ban_xoa`;

    // Báo cáo
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
}

export class ProductBondInfoConst {

    public static periodUnits = [
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Năm',
            code: 'Y'
        }
    ];

    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static HOAT_DONG = 3;
    public static HUY_DUYET = 4;
    public static DONG = 5;

    public static statusConst = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'help',
        },
        {
            name: 'Trình duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.HUY_DUYET,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.DONG,
            severity: 'secondary',
        }
    ]
    public static interestRateTypes = [
        {
            name: 'Định kỳ',
            code: 1,
        },
        {
            name: 'Cuối kỳ',
            code: 2,
        }
    ];

    public static countType = [
        {
            name: 'Tính từ ngày phát hành',
            code: 1,
        },
        {
            name: 'Tính từ ngày thanh toán',
            code: 2,
        }
    ];

    public static bondPeriodUnits = [
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Năm',
            code: 'Y'
        }
    ]
    public static booleans = [
        {
            name: 'Có',
            code: 'Y'
        },
        {
            name: 'Không',
            code: 'N'
        },
    ];
    public static unitDates = [
        {
            name: 'Năm',
            code: 'Y',
        },
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Ngày',
            code: 'D'
        }

    ];

    public static status = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'help',
        },
        {
            name: 'Trình duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.HUY_DUYET,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.DONG,
            severity: 'secondary',
        }
    ];

    // public static STATUS = {
    //     KHOI_TAO: 1,
    //     CHO_DUYET: 2,
    //     HOAT_DONG: 3,
    //     CLOSE: 4,
    // }



    public static STATUS_ACTIVE = 'A';
    public static STATUS_DISABLE = 'D';
    //
    public static QUESTION_YES = 'Y';
    public static QUESTION_NO = 'N';
    //
    public static INTEREST_RATE_TYPE_PERIODIC = 1;
    public static INTEREST_RATE_TYPE_PERIOD_END = 2;
    //
    public static UNIT_DATE_YEAR = 'Y';
    //

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }

    public static getPeriodUnits(code) {
        for (let item of this.periodUnits) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getInterestRateTypes(code) {
        for (let item of this.interestRateTypes) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getBoolean(code) {
        for (let item of this.booleans) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getUnitDates(code) {
        for (let item of this.unitDates) {
            if (item.code == code) return item.name;
        }
        return '';
    }
    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class DeliveryContractConst {

    public static fieldFilters = [
        {
            name: 'Ngày yêu cầu',
            code: 1,
            field: 'pendingDate',
        },
        {
            name: 'Ngày giao',
            code: 2,
            field: 'deliveryDate',
        },
        {
            name: 'Ngày nhận',
            code: 3,
            field: 'receivedDate',
        },
        {
            name: 'Ngày hoàn thành',
            code: 4,
            field: 'finishedDate',
        },
    ];

    public static deliveryStatus = [
        {
            name: "Chờ xử lý",
            code: '1',
            severity: 'danger',
        },
        {
            name: "Đang giao",
            code: '2',
            severity: 'warning',
        },
        {
            name: "Đã nhận",
            code: '3',
            severity: 'info',
        },
        {
            name: "Hoàn thành",
            code: '4',
            severity: 'success',
        },
    ];
    public static CHO_XU_LY = 1;
    public static DANG_GIAO = 2;
    public static DA_NHAN = 3;
    public static HOAN_THANH = 4;

    public static getSeverityStatus(code) {
        const deliveryStatus = this.deliveryStatus.find(p => p.code == code);
        return deliveryStatus ? deliveryStatus.severity : '-';
    }

    public static getNameStatus(code) {
        let type = this.deliveryStatus.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class PermissionRealStateConst {
    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly RealStateModule: string = "real_state.";

    public static readonly RealStatePageDashboard: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}dashboard`;

    // dt = DT = Đầu tư
    // Menu Cài đặt
    public static readonly RealStateMenuSetting: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}setting`;
    // Module chủ đầu tư
    public static readonly RealStateMenuChuDT: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}chu_dau_tu`;
    public static readonly RealStateChuDT_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}chu_dt_them_moi`;
    public static readonly RealStateChuDT_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}chu_dt_danh_sach`;
    public static readonly RealStateChuDT_ThongTinChuDauTu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}chu_dt_thong_tin_chu_dau_tu`;
    public static readonly RealStateChuDT_KichHoatOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}chu_dt_kich_hoat_or_huy`;
    public static readonly RealStateChuDT_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}chu_dt_xoa`;
    // Tab thông tin chung
    public static readonly RealStateChuDT_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}chu_dt_thong_tin_chung`;
    public static readonly RealStateChuDT_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}chu_dt_chi_tiet`;
    public static readonly RealStateChuDT_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}chu_dt_cap_nhat`;
   
    // Module Đại lý
    public static readonly RealStateMenuDaiLy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}dai_ly`;
    public static readonly RealStateDaiLy_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}dai_ly_danh_sach`;
    public static readonly RealStateDaiLy_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}dai_ly_them_moi`;
    public static readonly RealStateDaiLy_KichHoatOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}dai_ly_kich_hoat_or_huy`;
    public static readonly RealStateDaiLy_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}dai_ly_xoa`;
    public static readonly RealStateDaiLy_ThongTinDaiLy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}dai_ly_thong_tin_dai_ly`;
    //Tab Thông tin chung
    public static readonly RealStateDaiLy_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}dai_ly_thong_tin_chung`;
    public static readonly RealStateDaiLy_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}dai_ly_chi_tiet`;
    
    // Module Chính sách phân phối
    public static readonly RealStateMenuCSPhanPhoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}cs_phan_phoi`;
    public static readonly RealStateCSPhanPhoi_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}cs_phan_phoi_danh_sach`;
    public static readonly RealStateCSPhanPhoi_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_phan_phoi_them_moi`;
    public static readonly RealStateCSPhanPhoi_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_phan_phoi_cap_nhat`;
    public static readonly RealStateCSPhanPhoi_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_phan_phoi_doi_trang_thai`;
    public static readonly RealStateCSPhanPhoi_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_phan_phoi_xoa`;

    // Chính sách bán hàng
    public static readonly RealStateMenuCSBanHang: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}cs_ban_hang`;
    public static readonly RealStateCSBanHang_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}cs_ban_hang_danh_sach`;
    public static readonly RealStateCSBanHang_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_ban_hang_them_moi`;
    public static readonly RealStateCSBanHang_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_ban_hang_cap_nhat`;
    public static readonly RealStateCSBanHang_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_ban_hang_doi_trang_thai`;
    public static readonly RealStateCSBanHang_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}cs_ban_hang_xoa`;

    // Cấu trúc hợp đồng giao dịch
    public static readonly RealStateMenuCTMaHDGiaoDich: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}ct_ma_hd_giao_dich`;
    public static readonly RealStateCTMaHDGiaoDich_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}ct_ma_hd_giao_dich_danh_sach`;
    public static readonly RealStateCTMaHDGiaoDich_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_giao_dich_them_moi`;
    public static readonly RealStateCTMaHDGiaoDich_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_giao_dich_cap_nhat`
    public static readonly RealStateCTMaHDGiaoDich_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_giao_dich_doi_trang_thai`
    public static readonly RealStateCTMaHDGiaoDich_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_giao_dich_xoa`;

    // Cấu trúc hợp đồng cọc
    public static readonly RealStateMenuCTMaHDCoc: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}ct_ma_hd_coc`;
    public static readonly RealStateCTMaHDCoc_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}ct_ma_hd_coc_danh_sach`;
    public static readonly RealStateCTMaHDCoc_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_coc_them_moi`;
    public static readonly RealStateCTMaHDCoc_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_coc_cap_nhat`
    public static readonly RealStateCTMaHDCoc_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_coc_doi_trang_thai`
    public static readonly RealStateCTMaHDCoc_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}ct_ma_hd_coc_xoa`;

    // Module mẫu hợp đồng chủ đầu tư
    public static readonly RealStateMenuMauHDCDT: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}mau_hdcdt`;
    public static readonly RealStateMauHDCDT_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mau_hdcdt_danh_sach`;
    public static readonly RealStateMauHDCDT_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_them_moi`;
    public static readonly RealStateMauHDCDT_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_cap_nhat`
    public static readonly RealStateMauHDCDT_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_doi_trang_thai`
    public static readonly RealStateMauHDCDT_TaiFileDoanhNghiep: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_tai_file_doanh_nghiep`;
    public static readonly RealStateMauHDCDT_TaiFileCaNhan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_tai_file_ca_nhan`
    public static readonly RealStateMauHDCDT_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hdcdt_xoa`;

    // Module mẫu hợp đồng đại lý
    public static readonly RealStateMenuMauHDDL: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}mau_hddl`;
    public static readonly RealStateMauHDDL_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mau_hddl_danh_sach`;
    public static readonly RealStateMauHDDL_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_them_moi`;
    public static readonly RealStateMauHDDL_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_cap_nhat`
    public static readonly RealStateMauHDDL_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_doi_trang_thai`
    public static readonly RealStateMauHDDL_TaiFileDoanhNghiep: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_tai_file_doanh_nghiep`;
    public static readonly RealStateMauHDDL_TaiFileCaNhan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_tai_file_ca_nhan`
    public static readonly RealStateMauHDDL_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mau_hddl_xoa`;
    
    // Thông báo hệ thống
    public static readonly RealStateThongBaoHeThong: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}thong_bao_he_thong`;

    // Menu Quản lý dự án
    public static readonly RealStateMenuProjectManager: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}project_manager`;

    public static readonly RealStateMenuProjectOverview: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}project_overview`;
    public static readonly RealStateProjectOverview_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_danh_sach`;
    public static readonly RealStateProjectOverview_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_them_moi`;

    public static readonly RealStateProjectOverview_ThongTinProjectOverview: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}project_overview_thong_tin_project_overview`;
    public static readonly RealStateProjectOverview_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_xoa`;
    public static readonly RealStateProjectOverview_TrinhDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_trinh_duyet`;
    public static readonly RealStateProjectOverview_PheDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_phe_duyet`;
    
    // Tab Thông tin chung
    public static readonly RealStateProjectOverview_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_thong_tin_chung`;
    public static readonly RealStateProjectOverview_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_overview_thong_tin_chung_chi_tiet`;
    public static readonly RealStateProjectOverview_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_thong_tin_chung_cap_nhat`;
    // Tab Mô tả dự án
    public static readonly RealStateProjectOverview_MoTa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_mo_ta`;
    public static readonly RealStateProjectOverview_MoTa_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_overview_mo_ta_chi_tiet`;
    public static readonly RealStateProjectOverview_MoTa_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_mo_ta_cap_nhat`;
    // Tab Tiện ích
    public static readonly RealStateProjectOverview_TienIch: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_tien_ich`;
    public static readonly RealStateProjectOverview_TienIchHeThong: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_ht`;
    public static readonly RealStateProjectOverview_TienIchHeThong_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_ht_ds`;
    public static readonly RealStateProjectOverview_TienIchHeThong_QuanLy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_ht_quan_ly`;

    // TIỆN ÍCH KHÁC
    public static readonly RealStateProjectOverview_TienIchKhac: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_khac`;
    public static readonly RealStateProjectOverview_TienIchKhac_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_khac_ds`;
    public static readonly RealStateProjectOverview_TienIchKhac_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_khac_them_moi`;
    public static readonly RealStateProjectOverview_TienIchKhac_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_khac_cap_nhat`;
    public static readonly RealStateProjectOverview_TienIchKhac_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_khac_doi_trang_thai`;
    public static readonly RealStateProjectOverview_TienIchKhac_DoiTrangThaiNoiBat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_khac_doi_trang_thai_noi_bat`;
    public static readonly RealStateProjectOverview_TienIchKhac_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_khac_xoa`;

    public static readonly RealStateProjectOverview_TienIchMinhHoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_minh_hoa`;
    public static readonly RealStateProjectOverview_TienIchMinhHoa_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_tien_ich_minh_hoa_ds`;
    public static readonly RealStateProjectOverview_TienIchMinhHoa_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_minh_hoa_them_moi`;
    public static readonly RealStateProjectOverview_TienIchMinhHoa_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_minh_hoa_cap_nhat`;
    public static readonly RealStateProjectOverview_TienIchMinhHoa_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_minh_hoa_doi_trang_thai`;
    public static readonly RealStateProjectOverview_TienIchMinhHoa_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_tien_ich_minh_hoa_xoa`;

    // Tab Cấu trúc dự án
    public static readonly RealStateProjectOverview_CauTruc: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_cau_truc`;
    public static readonly RealStateProjectOverview_CauTruc_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_cau_truc_ds`;
    public static readonly RealStateProjectOverview_CauTruc_Them: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_cau_truc_them`;
    public static readonly RealStateProjectOverview_CauTruc_Sua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_cau_truc_sua`;
    public static readonly RealStateProjectOverview_CauTruc_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_cau_truc_xoa`;

    // Tab Hình ảnh
    public static readonly RealStateProjectOverview_HinhAnhDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_hinh_anh_du_an`;

    public static readonly RealStateProjectOverview_HinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_hinh_anh`;
    public static readonly RealStateProjectOverview_HinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_overview_hinh_anh_chi_tiet`;
    public static readonly RealStateProjectOverview_HinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_hinh_anh_them_moi`;
    public static readonly RealStateProjectOverview_HinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_hinh_anh_cap_nhat`;
    public static readonly RealStateProjectOverview_HinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_hinh_anh_doi_trang_thai`;
    public static readonly RealStateProjectOverview_HinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_hinh_anh_xoa`;

    public static readonly RealStateProjectOverview_NhomHinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_nhom_hinh_anh`;
    public static readonly RealStateProjectOverview_NhomHinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_overview_nhom_hinh_anh_chi_tiet`;
    public static readonly RealStateProjectOverview_NhomHinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_nhom_hinh_anh_them_moi`;
    public static readonly RealStateProjectOverview_NhomHinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_nhom_hinh_anh_cap_nhat`;
    public static readonly RealStateProjectOverview_NhomHinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_nhom_hinh_anh_doi_trang_thai`;
    public static readonly RealStateProjectOverview_NhomHinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_nhom_hinh_anh_xoa`;

    // Tab Chính sách dự án
    public static readonly RealStateProjectOverview_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_chinh_sach`;
    public static readonly RealStateProjectOverview_ChinhSach_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_chinh_sach_ds`;
    public static readonly RealStateProjectOverview_ChinhSach_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chinh_sach_them_moi`;
    public static readonly RealStateProjectOverview_ChinhSach_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chinh_sach_cap_nhat`;
    public static readonly RealStateProjectOverview_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chinh_sach_doi_trang_thai`;
    public static readonly RealStateProjectOverview_ChinhSach_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chinh_sach_xoa`;

    // Tab Hồ sơ pháp lý
    public static readonly RealStateProjectOverview_HoSo: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_ho_so`;
    public static readonly RealStateProjectOverview_HoSo_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_ho_so_ds`;
    public static readonly RealStateProjectOverview_HoSo_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_them_moi`;
    public static readonly RealStateProjectOverview_HoSo_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_cap_nhat`;
    public static readonly RealStateProjectOverview_HoSo_XemFile: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_xem_file`;
    public static readonly RealStateProjectOverview_HoSo_TaiXuong: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_tai_xuong`;
    public static readonly RealStateProjectOverview_HoSo_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_doi_trang_thai`;
    public static readonly RealStateProjectOverview_HoSo_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_ho_so_xoa`;
    
    // Bài đăng facebook
    public static readonly RealStateProjectOverview_Facebook_Post: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_facebook_post`;
    public static readonly RealStateProjectOverview_Facebook_Post_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_facebook_post_ds`;
    public static readonly RealStateProjectOverview_Facebook_Post_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_facebook_post_them_moi`;
    public static readonly RealStateProjectOverview_Facebook_Post_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_facebook_post_cap_nhat`;
    public static readonly RealStateProjectOverview_Facebook_Post_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_facebook_post_doi_trang_thai`;
    public static readonly RealStateProjectOverview_Facebook_Post_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_facebook_post_xoa`;
    
    // Chia sẻ dự án
    public static readonly RealStateProjectOverview_ChiaSeDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_overview_chia_se_du_an`;
    public static readonly RealStateProjectOverview_ChiaSeDuAn_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_chia_se_du_an_ds`;
    public static readonly RealStateProjectOverview_ChiaSeDuAn_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chia_se_du_an_them_moi`;
    public static readonly RealStateProjectOverview_ChiaSeDuAn_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chia_se_du_an_cap_nhat`;
    public static readonly RealStateProjectOverview_ChiaSeDuAn_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chia_se_du_an_doi_trang_thai`;
    public static readonly RealStateProjectOverview_ChiaSeDuAn_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_chia_se_du_an_xoa`;

    // Menu Bảng hàng dự án
    public static readonly RealStateMenuProjectList: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}project_list`;
    public static readonly RealStateMenuProjectList_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_list_danh_sach`;

    public static readonly RealStateMenuProjectListDetail: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}project_list_detail`;
    public static readonly RealStateMenuProjectListDetail_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_list_detail_danh_sach`;
    public static readonly RealStateMenuProjectListDetail_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_them_moi`;
    public static readonly RealStateMenuProjectListDetail_UploadFile: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_upload_file`;
    public static readonly RealStateMenuProjectListDetail_TaiFileMau: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_tai_file_mau`;
    public static readonly RealStateMenuProjectListDetail_KhoaCan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_khoa_can`;
    public static readonly RealStateMenuProjectListDetail_NhanBan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_nhan_ban`;

    public static readonly RealStateMenuProjectListDetail_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}project_list_detail_chi_tiet`;

    // Tab Thông tin chung - Bảng hàng
    public static readonly RealStateMenuProjectListDetail_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_thong_tin_chung`;
    public static readonly RealStateMenuProjectListDetail_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_thong_tin_chung_chi_tiet`;
    public static readonly RealStateMenuProjectListDetail_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_thong_tin_chung_cap_nhat`;

    // Tab Chính sách ưu đãi CĐT - Bảng hàng
    public static readonly RealStateMenuProjectListDetail_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_chinh_sach`;
    public static readonly RealStateMenuProjectListDetail_ChinhSach_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_chinh_sach_chi_tiet`;
    public static readonly RealStateMenuProjectListDetail_ChinhSach_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_chinh_sach_cap_nhat`;
    public static readonly RealStateMenuProjectListDetail_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_chinh_sach_doi_trang_thai`;

    // Tab Tiện ích - Bảng hàng
    public static readonly RealStateMenuProjectListDetail_TienIch: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_tien_ich`;
    public static readonly RealStateMenuProjectListDetail_TienIch_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_tien_ich_chi_tiet`;
    public static readonly RealStateMenuProjectListDetail_TienIch_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_tien_ich_cap_nhat`;
    public static readonly RealStateMenuProjectListDetail_TienIch_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_tien_ich_doi_trang_thai`;

    // Tab Vật liệu - Bảng hàng
    public static readonly RealStateMenuProjectListDetail_VatLieu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_vat_lieu`;
    public static readonly RealStateMenuProjectListDetail_VatLieu_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_vat_lieu_chi_tiet`;
    public static readonly RealStateMenuProjectListDetail_VatLieu_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_vat_lieu_cap_nhat`;

    // Tab Sơ đồ thiết kế - Bảng hàng
    public static readonly RealStateMenuProjectListDetail_SoDoTK: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_so_do_tk`;
    public static readonly RealStateMenuProjectListDetail_SoDoTK_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_so_do_tk_chi_tiet`;
    public static readonly RealStateMenuProjectListDetail_SoDoTK_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_so_do_tk_cap_nhat`;

    // Tab Hình ảnh - Bảng hàng
    public static readonly RealStateProjectListDetail_HinhAnhDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_detail_hinh_anh_du_an`;

    public static readonly RealStateProjectListDetail_HinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_detail_hinh_anh`;
    public static readonly RealStateProjectListDetail_HinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_detail_hinh_anh_chi_tiet`;
    public static readonly RealStateProjectListDetail_HinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_hinh_anh_them_moi`;
    public static readonly RealStateProjectListDetail_HinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_hinh_anh_cap_nhat`;
    public static readonly RealStateProjectListDetail_HinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_hinh_anh_doi_trang_thai`;
    public static readonly RealStateProjectListDetail_HinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_hinh_anh_xoa`;

    public static readonly RealStateProjectListDetail_NhomHinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_detail_nhom_hinh_anh`;
    public static readonly RealStateProjectListDetail_NhomHinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}project_list_detail_nhom_hinh_anh_chi_tiet`;
    public static readonly RealStateProjectListDetail_NhomHinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_nhom_hinh_anh_them_moi`;
    public static readonly RealStateProjectListDetail_NhomHinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_nhom_hinh_anh_cap_nhat`;
    public static readonly RealStateProjectListDetail_NhomHinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_nhom_hinh_anh_doi_trang_thai`;
    public static readonly RealStateProjectListDetail_NhomHinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_list_detail_nhom_hinh_anh_xoa`;

    // Tab Lịch sử - Bảng hàng
    public static readonly RealStateProjectListDetail_LichSu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}project_list_detail_lich_su`;
    public static readonly RealStateProjectListDetail_LichSu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_list_detail_lich_su_danh_sach`;


    // Menu Phân phối
    public static readonly RealStateMenuPhanPhoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}phan_phoi`;
    public static readonly RealStatePhanPhoi_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_danh_sach`;
    public static readonly RealStatePhanPhoi_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_them_moi`;
    public static readonly RealStatePhanPhoi_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_doi_trang_thai`;
    public static readonly RealStatePhanPhoi_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_xoa`;
    public static readonly RealStatePhanPhoi_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}phan_phoi_chi_tiet`;

    // Tab Thông tin chung - Phân phối
    public static readonly RealStateMenuPhanPhoi_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_thong_tin_chung`;
    public static readonly RealStateMenuPhanPhoi_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_thong_tin_chung_chi_tiet`;
    public static readonly RealStateMenuPhanPhoi_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_thong_tin_chung_cap_nhat`;
    public static readonly RealStateMenuPhanPhoi_ThongTinChung_TrinhDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_thong_tin_chung_trinh_duyet`;
    public static readonly RealStateMenuPhanPhoi_ThongTinChung_PheDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_thong_tin_chung_phe_duyet`;

    // Tab Danh sách sản phẩm
    public static readonly RealStatePhanPhoi_DSSP: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp`;
    public static readonly RealStatePhanPhoi_DSSP_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_dssp_them_moi`;
    public static readonly RealStatePhanPhoi_DSSP_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_dssp_ds`;
    public static readonly RealStatePhanPhoi_DSSP_Mo_KhoaCan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_doi_trang_thai`;
    public static readonly RealStatePhanPhoi_DSSP_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_xoa`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet`;
   
    // // Tab Thông tin chung - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_thong_tin_chung`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_thong_tin_chung_them_moi`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_thong_tin_chung_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_thong_tin_chung_cap_nhat`;

    // // Tab Chính sách ưu đãi CĐT - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_chinh_sach`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_chinh_sach_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_chinh_sach_cap_nhat`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_chinh_sach_doi_trang_thai`;

    // // Tab Tiện ích - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_tien_ich`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_tien_ich_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_tien_ich_cap_nhat`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_tien_ich_doi_trang_thai`;

    // // Tab Vật liệu - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_vat_lieu`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_vat_lieu_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_vat_lieu_cap_nhat`;

    // // Tab Sơ đồ thiết kế - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_so_do_tk`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_so_do_tk_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_so_do_tk_cap_nhat`;

    // // Tab Hình ảnh - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_hinh_anh_du_an`;

    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_hinh_anh`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_detail_hinh_anh_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_them_moi`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_cap_nhat`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_doi_trang_thai`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_xoa`;

    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_chi_tiet`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_them_moi`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_cap_nhat`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_doi_trang_thai`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_xoa`;

    // // Tab Lịch sử - Chi tiết căn hộ phân phối
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_LichSu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_lich_su`;
    // public static readonly RealStatePhanPhoi_DSSP_ChiTiet_LichSu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_dssp_chi_tiet_detail_lich_su_danh_sach`;
    
    // Tab Chính sách phân phối
    public static readonly RealStatePhanPhoi_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_chinh_sach`;
    public static readonly RealStatePhanPhoi_ChinhSach_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_chinh_sach_ds`;
    public static readonly RealStatePhanPhoi_ChinhSach_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_chinh_sach_them_moi`;
    public static readonly RealStatePhanPhoi_ChinhSach_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_chinh_sach_chi_tiet`;
    public static readonly RealStatePhanPhoi_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_chinh_sach_doi_trang_thai`;
    public static readonly RealStatePhanPhoi_ChinhSach_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_chinh_sach_xoa`;

    // Tab mẫu biểu hợp đồng
    public static readonly RealStatePhanPhoi_MauBieu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_mau_bieu`;
    public static readonly RealStatePhanPhoi_MauBieu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_mau_bieu_ds`;
    public static readonly RealStatePhanPhoi_MauBieu_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_mau_bieu_them_moi`;
    public static readonly RealStatePhanPhoi_MauBieu_XuatWord: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_mau_bieu_xuat_word`;
    public static readonly RealStatePhanPhoi_MauBieu_XuatPdf: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_mau_bieu_xuat_pdf`;
    public static readonly RealStatePhanPhoi_MauBieu_ChinhSua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_mau_bieu_chinh_sua`;
    public static readonly RealStatePhanPhoi_MauBieu_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_mau_bieu_doi_trang_thai`;

    // Menu Mở bán
    public static readonly RealStateMenuMoBan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}mo_ban`;
    public static readonly RealStateMoBan_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_danh_sach`;
    public static readonly RealStateMoBan_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_them_moi`;
    public static readonly RealStateMoBan_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_doi_trang_thai`;
    public static readonly RealStateMoBan_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_xoa`;
    public static readonly RealStateMoBan_DungBan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_dung_ban`;
    public static readonly RealStateMoBan_DoiNoiBat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_bieu_doi_mo_ban`;
    public static readonly RealStateMoBan_DoiShowApp: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_bieu_doi_show_app`;

    public static readonly RealStateMoBan_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}mo_ban_chi_tiet`;
   
    // Tab Thông tin chung - Mở bán
    public static readonly RealStateMoBan_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_thong_tin_chung`;
    public static readonly RealStateMoBan_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}mo_ban_thong_tin_chung_chi_tiet`;
    public static readonly RealStateMoBan_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_thong_tin_chung_cap_nhat`;
    public static readonly RealStateMoBan_ThongTinChung_TrinhDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_thong_tin_chung_trinh_duyet`;
    public static readonly RealStateMoBan_ThongTinChung_PheDuyet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_thong_tin_chung_phe_duyet`;
    // Tab Danh sách sản phẩm - Mở bán
    public static readonly RealStateMoBan_DSSP: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_dssp`;
    public static readonly RealStateMoBan_DSSP_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_dssp_ds`;
    public static readonly RealStateMoBan_DSSP_Them: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_them_moi`;
    public static readonly RealStateMoBan_DSSP_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_xoa`;
    public static readonly RealStateMoBan_DSSP_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_doi_trang_thai`;
    public static readonly RealStateMoBan_DSSP_DoiShowApp: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_doi_show_app`;
    public static readonly RealStateMoBan_DSSP_DoiShowPrice: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_doi_show_price`;
    public static readonly RealStateMoBan_DSSP_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_dssp_chi_tiet`;
    // Tab Thông tin chung - Chi tiết căn hộ mở bán
    public static readonly RealStateMoBan_DSSP_ChiTiet_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_dssp_chi_tiet_thong_tin_chung`;
    public static readonly RealStateMoBan_DSSP_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}mo_ban_dssp_chi_tiet_thong_tin_chung_chi_tiet`;
    // Tab Lịch sử - Chi tiết căn hộ mở bán
    public static readonly RealStateMoBan_DSSP_ChiTiet_LichSu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_dssp_chi_tiet_lich_su`;
    public static readonly RealStateMoBan_DSSP_ChiTiet_LichSu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_dssp_chi_tiet_lich_su_danh_sach`;


    // Tab Chính sách mở bán - Mở bán
    public static readonly RealStateMoBan_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_chinh_sach`;
    public static readonly RealStateMoBan_ChinhSach_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_chinh_sach_ds`;
    public static readonly RealStateMoBan_ChinhSach_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_chinh_sach_cap_nhat`;
    public static readonly RealStateMoBan_ChinhSach_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_chinh_sach_chi_tiet`;
    public static readonly RealStateMoBan_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_chinh_sach_doi_trang_thai`;

    // Tab mẫu biểu hợp đồng - Mở bán
    public static readonly RealStateMoBan_MauBieu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_mau_bieu`;
    public static readonly RealStateMoBan_MauBieu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_mau_bieu_ds`;
    public static readonly RealStateMoBan_MauBieu_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_bieu_them_moi`;
    public static readonly RealStateMoBan_MauBieu_ChinhSua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_bieu_chinh_sua`;
    public static readonly RealStateMoBan_MauBieu_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_mau_bieu_doi_trang_thai`;

    // Tab hồ sơ dự án - Mở bán
    public static readonly RealStateMoBan_HoSo: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}mo_ban_ho_so`;
    public static readonly RealStateMoBan_HoSo_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}mo_ban_ho_so_ds`;
    public static readonly RealStateMoBan_HoSo_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_them_moi`;
    public static readonly RealStateMoBan_HoSo_ChinhSua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_chinh_sua`;
    public static readonly RealStateMoBan_HoSo_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_doi_trang_thai`;
    public static readonly RealStateMoBan_HoSo_XemFile: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_xem_file`;
    public static readonly RealStateMoBan_HoSo_Tai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_tai`;
    public static readonly RealStateMoBan_HoSo_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}mo_ban_ho_so_xoa`;

    // Menu Quản lý giao dịch cọc
    public static readonly RealStateMenuQLGiaoDichCoc: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}ql_ho_so_giao_dich_coc`;
    // Module Sổ lệnh
    public static readonly RealStateMenuSoLenh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}so_lenh`;
    public static readonly RealStateMenuSoLenh_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}so_lenh_ds`;
    public static readonly RealStateMenuSoLenh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_them_moi`;
    public static readonly RealStateMenuSoLenh_GiaHanGiuCho: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_gia_han_giu_cho`;
    public static readonly RealStateMenuSoLenh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_xoa`;

    public static readonly RealStateMenuSoLenh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}so_lenh_chi_tiet`;
    // Tab Thông tin chung
    public static readonly RealStateMenuSoLenh_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}so_lenh_thong_tin_chung`;
    public static readonly RealStateMenuSoLenh_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}so_lenh_thong_tin_chung_chi_tiet`;
    public static readonly RealStateMenuSoLenh_ThongTinChung_ChinhSua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_chung_chinh_sua`;
    public static readonly RealStateMenuSoLenh_ThongTinChung_DoiLoaiHD: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_chung_doi_loai_hop_dong`;
    public static readonly RealStateMenuSoLenh_ThongTinChung_DoiHinhThucThanhToan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_chung_doi_hinh_thuc_thanh_toan`;
    // Tab Thanh toán
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}so_lenh_thong_tin_thanh_toan`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}so_lenh_thong_tin_thanh_toan_ds`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_them_moi`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_chi_tiet`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_ChinhSua: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_chinh_sua`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_phe_duyet_or_huy`;
    public static readonly RealStateMenuSoLenh_ThongTinThanhToan_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_xoa`;
    // Tab Chính sách ưu đãi
    public static readonly RealStateMenuSoLenh_CSUuDai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}so_lenh_cs_uu_dai`;
    public static readonly RealStateMenuSoLenh_CSUuDai_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}so_lenh_cs_uu_dai_ds`;
    // Tab HSKH đăng ký
    public static readonly RealStateMenuSoLenh_HSKHDangKy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}so_lenh_hskh_dang_ky`;
    public static readonly RealStateMenuSoLenh_HSKHDangKy_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}so_lenh_hskh_dang_ky_ds`;
    public static readonly RealStateMenuSoLenh_HSKHDangKy_ChuyenOnline: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_chuyen_online`;
    public static readonly RealStateMenuSoLenh_HSKHDangKy_CapNhatHS: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_cap_nhat_hs`;
    public static readonly RealStateMenuSoLenh_HSKHDangKy_DuyetHS: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_duyet_hs`;
    public static readonly RealStateMenuSoLenh_HSKHDangKy_HuyDuyetHS: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}so_lenh_thong_tin_thanh_toan_huy_duyet_hs`;
    // Tab Lịch sử - Sổ lệnh
    public static readonly RealStateMenuSoLenh_LichSu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}so_lenh_lich_su`;
    public static readonly RealStateMenuSoLenh_LichSu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}so_lenh_lich_su_danh_sach`;
    
    // Xử lý đặt cọc 
    public static readonly RealStateGDC_XLDC: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}gdc_xldc`;
    public static readonly RealStateGDC_XLDC_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}gdc_xldc_ds`;

    // Hợp đồng đặt cọc 
    public static readonly RealStateGDC_HDDC: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}gdc_hddc`;
    public static readonly RealStateGDC_HDDC_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}gdc_hddc_ds`;
    
    // Menu Quản lý phê duyệt
    public static readonly RealStateMenuQLPD: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}qlpd`;
    // Phê duyệt dự án
    public static readonly RealStateMenuPDDA: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}pdda`;  
    public static readonly RealStatePDDA_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}pdda_ds`;  
    public static readonly RealStatePDDA_PheDuyetOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}pdda_phe_duyet`;  
    // Phê duyệt phân phối
    public static readonly RealStateMenuPDPP: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}pdpp`;  
    public static readonly RealStatePDPP_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}pdpp_ds`;  
    public static readonly RealStatePDPP_PheDuyetOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}pdpp_phe_duyet`;  
    // Phê duyệt mở bán
    public static readonly RealStateMenuPDMB: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}pdmb`;  
    public static readonly RealStatePDMB_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}pdmb_ds`;  
    public static readonly RealStatePDMB_PheDuyetOrHuy: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}pdmb_phe_duyet`;  
    // Báo cáo
    public static readonly RealState_Menu_BaoCao: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}bao_cao`;
   
    public static readonly RealState_BaoCao_QuanTri: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}bao_cao_quan_tri`;
    public static readonly RealState_BaoCao_QuanTri_TQBangHangDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_quan_tri_tq_bang_hang_du_an`;
    public static readonly RealState_BaoCao_QuanTri_TH_TienVeDA: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_quan_tri_th_tien_ve_da`;
    public static readonly RealState_BaoCao_QuanTri_TH_CacKhoanGD: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_quan_tri_th_cac_khoan_gd`;

    public static readonly RealState_BaoCao_VanHanh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}bao_cao_van_hanh`;
    public static readonly RealState_BaoCao_VanHanh_TQBangHangDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_van_hanh_tq_bang_hang_du_an`;
    public static readonly RealState_BaoCao_VanHanh_TH_TienVeDA: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_van_hanh_th_tien_ve_da`;
    public static readonly RealState_BaoCao_VanHanh_TH_CacKhoanGD: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_van_hanh_th_cac_khoan_gd`;

    public static readonly RealState_BaoCao_KinhDoanh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}bao_cao_kinh_doanh`;
    public static readonly RealState_BaoCao_KinhDoanh_TQBangHangDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_kinh_doanh_tq_bang_hang_du_an`;
    public static readonly RealState_BaoCao_KinhDoanh_TH_TienVeDA: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_kinh_doanh_th_tien_ve_da`;
    public static readonly RealState_BaoCao_KinhDoanh_TH_CacKhoanGD: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}bao_cao_kinh_doanh_th_cac_khoan_gd`;
}

export class AppPermissionNames {
    // user
    public readonly Pages_Users = "Pages.Users";
    public readonly Actions_Users_Create = "Actions.Users.Create";
    public readonly Actions_Users_Update = "Actions.Users.Update";
    public readonly Actions_Users_Delete = "Actions.Users.Delete";
    public readonly Actions_Users_Activation = "Actions.Users.Activation";

    // role
    public readonly Pages_Roles = "Pages.Roles";
    public readonly Actions_Roles_Create = "Actions.Roles.Create";
    public readonly Actions_Roles_Update = "Actions.Roles.Update";
    public readonly Actions_Roles_Delete = "Actions.Roles.Delete";
}

export class DataTableConst {
    public static message = {
        emptyMessage: 'Không có dữ liệu',
        totalMessage: 'bản ghi',
        selectedMessage: 'chọn'
    }
}

export class UserTypes {
    //
    public static list = [
        {
            name: 'Epic',
            code: 'E',
            severity: '',
        },
        {
            name: 'Đối tác root',
            code: 'RP',
            severity: '',
        },
        {
            name: 'Đối tác',
            code: 'P',
            severity: '',
        },
        {
            name: 'Đại lý Root',
            code: 'RT',
            severity: '',
        },
        {
            name: 'User',
            code: 'T',
            severity: '',
        },
    ];

    public static EPIC = 'E';
    public static PARTNER_ROOT = 'RP';
    public static PARTNER = 'P';
    public static TRADING_PROVIDER_ROOT = 'RT';
    public static TRADING_PROVIDER = 'T'; // User thuộc đại lý (Do đại lý tạo)
    // TYPE GROUP
    public static TYPE_PARTNERS = ['P', 'RP'];  // PARTNERROOT HOẶC PARTNER
    public static TYPE_ROOTS = ['RP', 'RT'];  // PARTNERROOT HOẶC TRADINGROOT
    public static TYPE_TRADING_PROVIDERS = ['T', 'RT'];  // TRADING HOẶC TRADINGROOT
    
    

    public static getUserTypeInfo(code, property) {
        let type = this.list.find(t => t.code == code);
        if (type) return type[property];
        return '';
    }

}

export class DepositProviderConst {
    public static statusConst = [
        {
            name: 'Kích hoạt',
            code: 'A'
        },
        {
            name: 'Chưa kích hoạt',
            code: 'D'
        }
    ]
    public static types = [
        {
            name: 'Tổ chức',
            code: 'B',
        },
        {
            name: 'Cá nhân',
            code: 'I',
        },

    ];

    public static getNameType(code) {
        let type = this.types.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}
export class ActiveDeactiveConst {
    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static list = [
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success'
        },
        {
            name: 'Khóa',
            code: this.DEACTIVE,
            severity: 'secondary'
        }
    ];

    public static listStatus = [
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success'
        },
        {
            name: 'Huỷ kích hoạt',
            code: this.DEACTIVE,
            severity: 'danger'
        } 
    ];

    public static getInfo(code, atribution = 'name', isOption2:boolean = false) {
        const listFilter = isOption2 ? this.listStatus : this.list;
        let status = listFilter.find(type => type.code == code);
        return status ? status[atribution] : null;
    }
   
}

export class UnitDateConst {
    public static list = {
        D: 'd',
        M: 'M',
        Y: 'y',
    }
}

export class StatusBondInfoFileConst {
    public static list = [
        {
            name: "Trình duyệt",
            code: 0,
        },
        {
            name: "Duyệt",
            code: 1,
        },
    ]

    public static RESPONSE_TRUE = 1;
    public static RESPONSE_FALSE = 0;

    public static getStatusName(code) {
        let list = this.list.find(list => list.code == code);
        if (list) return list.name;
        return '';
    }

    // public static getStatusSeverity(code) {
    //     let list = this.list.find(list => list.code == code);
    //     if(list) return list.severity;
    //     return '';
    // }
}

export class StatusResponseConst {
    public static list = [
        {
            value: false,
            status: 0,
        },
        {
            value: true,
            status: 1,
        },
    ]

    public static RESPONSE_TRUE = 1;
    public static RESPONSE_FALSE = 0;

}

export class ContractTemplateConst {

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static list = [
        {
            name: 'Còn hiệu lực',
            code: this.ACTIVE,
            severity: 'success',

        },
        {
            name: 'Hết hiệu lực',
            code: this.DEACTIVE,
            severity: 'help',
                
        }
    ];

    public static classify = [
        { name: "PRO", code: 1 },
        { name: "PRO A", code: 2 },
        { name: "PNOTE", code: 3 }
    ]

    public static contractType = [
        { name: "Hợp đồng đầu tư trái phiếu", code: 1 },
        { name: "Biên nhận hồ sơ", code: 2 },
        { name: "Giấy xác nhận giao dịch trái phiếu", code: 3 },
        { name: "Phiếu đề nghị thực hiện giao dịch", code: 4 },
        { name: "Hợp đồng đặt mua trái phiếu", code: 5 },
        { name: "Hợp đồng đặt bán trái phiếu", code: 6 },
        { name: "Biên nhận hợp đồng", code: 7 },
        { name: "Giấy xác nhận số dư trái phiếu", code: 8 },
        { name: "Bảng minh họa thu nhập từ trái phiếu và kết quả đầu tư", code: 9 }
    ]

    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }

    public static getInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class DistributionConst {
    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static HOAT_DONG = 3;
    public static CANCEL = 4
    public static CLOSED = '';

    public static statusConst = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'help',
        },
        {
            name: 'Trình duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.CANCEL,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.CLOSED,
            severity: 'secondary'
        }
    ]

    public static getStatusName(code, isClose) {
        if (isClose == 'Y') return 'Đóng';
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code, isClose) {
        if (isClose == 'Y') return 'secondary';
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }
    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class DistributionContractTemplateConst {

    public static status = {
        ACTIVE: "A",
        DEACTIVE: "D"
    }

    public static classify = [
        { name: "FLEX", code: 1 },
        { name: "FLASH", code: 2 },
        { name: "FIX", code: 3 }
    ]

    public static type = [
        { name: "Cá nhân", code: "I" },
        { name: "Doanh nghiệp", code: "B" },
    ]
    public static displayType = [
        { name: "Trước khi duyệt hợp đồng", code: "B" },
        { name: "Sau khi duyệt hợp đồng", code: "A" },
    ]

    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }

    public static getDisplayType(code) {
        for (let item of this.displayType) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getType(code) {
        for (let item of this.type) {
            if (item.code == code) return item.name;
        }
        return '';
    }
}

export class InvestorAccountConst {
    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }
}
export class IssuerConst {
    //
    public static status = [
        {
            name: 'Kích hoạt',
            code: 1,
        },
        {
            name: 'Không kích hoạt',
            code: 2,
        },
        {
            name: 'Đóng',
            code: 3,
        },
    ];
    public static STATUS_ACTIVE = 1;
    public static STATUS_DISABLE = 2;
    public static STATUS_CLOSE = 3;

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    //
    public static types = [
        {
            name: 'Tổ chức',
            code: 'B',
        },
        {
            name: 'Cá nhân',
            code: 'I',
        },

    ];

    public static getNameType(code) {
        let type = this.types.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    //
}

export class StatusDeleteConst {
    public static list = [
        {
            name: 'Đã xóa',
            code: 'Y',
        },
        {
            name: 'Chưa xóa',
            code: 'N',
        },
    ]

    public static DELETE_TRUE = 'Y';
    public static DELETE_FALSE = 'N';
}

export class ContractTypeConst {
    public static list = [
        {
            name: 'PNOTE',
            code: 1,
        },
        {
            name: 'PRO',
            code: 2,
        },
        {
            name: 'PROA',
            code: 3,
        }
    ];

    public static PNOTE = 1;
    public static PRO = 2;
    public static PROA = 3;

    public static getName(code: Number) {
        const rslt = this.list.find(e => e.code.toString() === code.toString());
        return rslt ? rslt.name : '-';
    }
}

export class OrderSellingPolicyConst {
    public static policyTypes = [
        {
            name: 'Chủ đầu tư phân phối',
            code: 1,
        },
        {
            name: 'Đại lý phân phối',
            code: 2,
        },
    ];

    public static getName(code: number){
        const type = this.policyTypes.find( e => e.code === code);
        return type ? type.name : '-';
    }
}

export class OrderPaymentConst {
    public static transactionTypes = [
        {
            name: 'Thu',
            code: 1,
        },
        {
            name: 'Chi',
            code: 2,
        },
    ];

    public static THU = 1;
    public static CHI = 2;

    public static getNameTransactionType(code: number) {
        const transactionType = this.transactionTypes.find(t => t.code == code);
        return transactionType ? transactionType.name : '-';
    }

    public static paymentTypes = [
        {
            name: 'Chuyển khoản',
            code: 2,
        },
        {
            name: 'Tiền mặt',
            code: 1,
        },
    ];

    public static getNamePaymentType(code: number) {
        const paymentType = this.paymentTypes.find(p => p.code == code);
        return paymentType ? paymentType.name : '-';
    }

    public static paymentStatus = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning'
        },
        {
            name: 'Đã thanh toán',
            code: 2,
            severity: 'success'
        },
        {
            name: 'Hủy thanh toán',
            code: 3,
            severity: 'danger'
        },
    ];

    public static PAYMENT_TEMP = 1;
    public static PAYMENT_SUCCESS = 2;
    public static PAYMENT_CLOSE = 3;

    public static getNamePaymentStatus(code: number) {
        const status = this.paymentStatus.find(p => p.code == code);
        return status ? status.name : '-';
    }

    public static getSeverityPaymentStatus(code: number) {
        const status = this.paymentStatus.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static fileStatus = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning'
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success'
        },
        // {
        //     name: 'Từ chối',
        //     code: 3,
        //     severity: 'danger'
        // },
    ];
}


export class DistributionContractConst {

    public static DAT_DAU_TU = 1;
    public static XY_LY_HOP_DONG = 2;
    public static DANG_DAU_TU = 3;

    public static status = [
        {
            name: 'Đặt đầu tư',
            code: 1,
            severity: 'help'
        },
        {
            name: 'Xử lý hợp đồng',
            code: 2,
            severity: 'warning'
        },
        {
            name: 'Đang đầu tư',
            code: 3,
            severity: 'success'
        }
    ];

    public static ORDER = 1;
    public static PENDING = 1;
    public static SUCCESS = 2;

    public static getNameStatus(code: number) {
        const status = this.status.find(s => s.code == code);
        return status ? status.name : '-';
    }

    public static getSeverityStatus(code: number) {
        const status = this.status.find(s => s.code == code);
        return status ? status.severity : '-';
    }

    public static transactionTypes = [
        {
            name: 'Thu',
            code: 1,
        },
        {
            name: 'Chi',
            code: 2,
        },
    ];

    public static getNameTransactionType(code: number) {
        const transactionType = this.transactionTypes.find(t => t.code == code);
        return transactionType ? transactionType.name : '-';
    }

    public static paymentStatus = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning'
        },
        {
            name: 'Đã thanh toán',
            code: 2,
            severity: 'success'
        },
        {
            name: 'Hủy thanh toán',
            code: 3,
            severity: 'danger'
        },
    ];

    public static PAYMENT_TEMP = 1;
    public static PAYMENT_SUCCESS = 2;
    public static PAYMENT_CLOSE = 3;

    public static getNamePaymentStatus(code: number) {
        const status = this.paymentStatus.find(p => p.code == code);
        return status ? status.name : '-';
    }

    public static getSeverityPaymentStatus(code: number) {
        const status = this.paymentStatus.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static fileStatus = [
        {
            name: 'Chờ duyệt',
            code: 1,
            severity: 'warning'
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success'
        },
        {
            name: 'Huỷ duyệt',
            code: 3,
            severity: 'danger'
        },
    ];

    public static FILE_PENDING = 1;
    public static FILE_APPROVE = 2;
    public static FILE_CANCEL = 3;


    public static getNameFileStatus(code: number) {
        const status = this.fileStatus.find(p => p.code == code);
        return status ? status.name : '-';
    }

    public static getSeverityFileStatus(code: number) {
        const status = this.fileStatus.find(p => p.code == code);
        return status ? status.severity : '-';
    }
}

export class BusinessTypeConst {
    public static list = [
        {
            name: 'B2B',
            code: 'B2B',
        },
        {
            name: 'B2C',
            code: 'B2C',
        }
    ];

    public static B2B = 'B2B';
    public static B2C = 'B2C';
}

export class YesNoConst {
    public static list = [
        {
            name: 'Có',
            code: 'Y',
        },
        {
            name: 'Không',
            code: 'N',
        },
    ]

    public static getName(code) {
        for (let item of this.list) {
            if (item.code == code) return item.name;
        }
        return '-';
    }

    public static YES = 'Y';
    public static NO = 'N';
}
export class ProjectConst {
    public static projectTypes = [
        {
            name: 'Nhà riêng',
            type: 1,
        },
        {
            name: 'Căn hộ chung cư',
            type: 2,
        },
        {
            name: 'Biệt thự',
            type: 3,
        },
        {
            name: 'Đất nền',
            type: 4,
        },
        {
            name: 'Biệt thự nghỉ dưỡng',
            type: 5,
        },
        {
            name: 'Condotel',
            type: 6,
        },
        {
            name: 'Shophouse',
            type: 7,
        },
        {
            name: 'Officetel',
            type: 8,
        },
    ];
    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static HOAT_DONG = 3;
    public static HUY_DUYET = 4;
    public static DONG = 5;
    public static statusConst = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'help',
        },
        {
            name: 'Trình duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.HUY_DUYET,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.DONG,
            severity: 'secondary',
        }
    ]
    public static interestRateTypes = [
        {
            name: 'Định kỳ',
            code: 1,
        },
        {
            name: 'Cuối kỳ',
            code: 2,
        }
    ];

    public static getNameProjectType(type) {
        for (let item of this.projectTypes) {
            if (item.type == type) return item.name;
        }
        return '-';
    }

    public static getStatusName(code) {
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code) {
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }
    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

}

export class SearchConst {
    public static DEBOUNCE_TIME = 800;
}

export class ApproveConst {
    //
    public static statusConst = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Hủy duyêt',
            code: 3,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: 4,
            severity: 'secondary',
        },
    ];
    //
    public static DATA_TYPE_PROJECT = 1;
    public static DATA_TYPE_DISTRIBUTION = 2;
    public static DATA_TYPE_OPEN_SELL = 3;
    public static STATUS_WITHDRAWAL = 11;

    public static LINK_PROJECT = "/project-manager/project-overview/detail/";
    public static LINK_DISTRIBUTION = "/project-manager/product-distribution/detail/";
    public static LINK_OPEN_SELL = "/project-manager/open-sell/detail/";

    //
    public static dataType = [
        {
            name: 'Dự án đầu tư',
            code: this.DATA_TYPE_PROJECT,
        },
        {
            name: 'Phân phối đầu tư',
            code: this.DATA_TYPE_DISTRIBUTION,
        },
        {
            name: 'Mở bán',
            code: this.DATA_TYPE_OPEN_SELL,
        },
    ];
    
    

    public static getDataTypesName(code) {
        let dataType = this.dataType.find(dataType => dataType.code == code);
        if (dataType) return dataType.name;
        return '';
    }

    public static status = [
        {
            name: 'Trình duyệt',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Đã duyệt',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: 3,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: 4,
            severity: 'secondary',
        },
    ];

    public static STATUS_REQUEST = 1;
    public static STATUS_APPROVE = 2;
    public static STATUS_CANCEL = 3;

    public static STATUS_ACTIVE = 1;
    public static STATUS_DISABLE = 2;
    public static STATUS_CLOSE = 3;

    public static getStatusSeverity(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static actionType = [
        {
            name: 'Thêm',
            code: 1,
            severity: 'success',
        },
        {
            name: 'Sửa',
            code: 2,
            severity: 'warning',
        },
        {
            name: 'Xoá',
            code: 3,
            severity: 'danger',
        },
    ];

    public static ACTION_ADD = 1;
    public static ACTION_UPDATE = 2;
    public static ACTION_DELETE = 3;

    public static getActionTypeName(code) {
        let actionType = this.actionType.find(actionType => actionType.code == code);
        if (actionType) return actionType.name;
        return '';
    }

    //
    // public static types = [
    //     {
    //         name: 'Tổ chức',
    //         code: 'B',
    //     },
    //     {
    //         name: 'Cá nhân',
    //         code: 'I',
    //     },

    // ];

    // public static getNameType(code) {
    //     let type = this.types.find(type => type.code == code);
    //     if(type) return type.name;
    //     return '';
    // }
}


export class PartnerConst {
    public static status = [
        {
            name: 'Kích hoạt',
            code: 'A',
        },
        {
            name: 'Không kích hoạt',
            code: 'D',
        },
        {
            name: 'Đóng',
            code: 'C',
        },
    ];

    public static STATUS_ACTIVE = 'A';
    public static STATUS_DISABLE = 'D';
    public static STATUS_CLOSE = 'C';

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }
}

export class BusinessCustomerConst {
    public static status = {
        HOAT_DONG: 2,
        HUY_DUYET: 3,
    }

    public static statusList = [

        {
            name: 'Hoạt động',
            code: this.status.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.status.HUY_DUYET,
            severity: 'danger',
        },
    ];

    public static isCheckConst = [
        {
            name: 'Đã kiểm tra',
            code: 'Y'

        },
        {
            name: "Chưa kiểm tra",
            code: 'N'
        }
    ]

    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }
}

export class BusinessCustomerApproveConst {
    public static status = {
        KHOI_TAO: 1,
        CHO_DUYET: 2,
        DA_DUYET: 3,
        HUY_DUYET: 4,
    }

    public static statusList = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.status.KHOI_TAO,
        },
        {
            name: 'Trình duyệt',
            severity: 'warning',
            code: this.status.CHO_DUYET,
        },
        {
            name: 'Đã duyệt',
            code: this.status.DA_DUYET,
            severity: 'success',
        },
        {
            name: 'Huỷ duyệt',
            code: this.status.HUY_DUYET,
            severity: 'danger',
        }
    ];

    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    // Bộ lọc tìm kiếm nhập keyword theo loại 
    public static fieldSearchs = [
        {
            name: 'Mã số thuế',
            code: 2,
            field: 'keyword',
            placeholder: 'Nhập mã số thuế...',
        },
        {
            name: 'Số điện thoại',
            code: 3,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
        {
            name: 'Email',
            code: 4,
            field: 'email',
            placeholder: 'Nhập email...',
        },
        {
            name: 'Tên doanh nghiệp',
            code: 1,
            field: 'name',
            placeholder: 'Nhập tên doanh nghiệp...',
        },
    ];

    public static getFieldSearchInfo(field, attribute: string) {
        const fieldSearch = this.fieldSearchs.find(fields => fields.field == field);
        return fieldSearch ? fieldSearch[attribute] : null;
    }
}
export class PolicyTemplateConst {
    //
    public static NO = 'N';
    public static YES = 'Y';
    public static types = [
        {
            name: 'Thanh toán thông thường',
            code: 1,
        },
        {
            name: 'Trả góp ngân hàng',
            code: 2,
        },
        {
            name: 'Trả trước',
            code: 3,
        },
    ];

    public static THEO_GIA_TRI_CAN_HO = 1;
    public static GIA_CO_DINH = 2;
    
    public static depositTypes = [
        {
            name: 'Theo giá trị căn hộ',
            code: this.THEO_GIA_TRI_CAN_HO,
        },
        {
            name: 'Giá cố định',
            code: this.GIA_CO_DINH,
        },
    ]

    public static renewalsTypes = [
        {
            name: "Tạo hợp đồng mới",
            code: 1,
        },
        {
            name: "Giữ hợp đồng cũ",
            code: 2,
        }
    ]

    public static classifyPolicy = [
       
        {
            name: 'Hợp tác',
            code: 1,
        },
        {
            name: 'Mua bán',
            code: 2,
        },
    ];
    //
    public static classify = [
        {
            name: 'Fix',
            code: 3,
        },
        {
            name: 'Flex',
            code: 1,
        },
        {
            name: 'Flash',
            code: 2,
        },
    ];
    public static classifyFix = [
        {
            name: 'Fix',
            code: 3,
        }
    ];
    public static classifyNoFix = [
        {
            name: 'Flex',
            code: 1,
        },
        {
            name: 'Flash',
            code: 2,
        },
    ];
    //
    public static isTransfer = [
        {
            name: 'Có',
            code: 'Y',
        },
        {
            name: 'Không',
            code: 'N',
        }
    ];

    public static calculateType = [
        {
            name: 'Gross',
            code: 2,
        },
        {
            name: 'Net',
            code: 1,
        },
        
    ];

    public static exitFeeType = [
        {
            name: 'Theo số tiền',
            code: 1,
        },
        {
            name: 'Theo năm',
            code: 2,
        }
    ];
    //

    public static policyDisplayOrder = [
        {
            name: 'Kỳ hạn ngắn - Kỳ hạn dài',
            code: 1,
        },
        {
            name: 'Kỳ hạn dài - Kỳ hạn ngắn',
            code: 2,
        }
    ];

    public static statuses = [
        {
            name: "Kích hoạt",
            code: "A",
            severity: 'success'
        },
        {
            name: "Hủy kích hoạt",
            code: "D",
            severity: 'danger'
        }
    ];
    //
    public static KICH_HOAT = 'A';
    public static KHOA = 'D';
    //
    public static TYPE_CO_DINH = 1;
    public static TYPE_LINH_HOAT = 2;
    public static TYPE_GIOI_HAN = 3;
    public static TYPE_CHI_TRA_CO_DINH_THEO_NGAY = 4;
    //
    public static CLASSIFY_FLEX = 1;
    public static CLASSIFY_FLASH = 2;
    public static CLASSIFY_FIX = 3;

    public static getSeverityStatus(code) {
        const status = this.statuses.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static getPolicyDisplayOrderName(code) {
        let type = this.policyDisplayOrder.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameStatus(code) {
        let type = this.statuses.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameType(code) {
        let type = this.types.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameDepositType(code) {
        let type = this.depositTypes.find(type => type.code == code);
        return type.name ?? '';
    }

    public static getNameClassify(code) {
        let type = this.classify.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameIsTransfer(code) {
        let type = this.isTransfer.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class PolicyDetailTemplateConst {
    public static interestPeriodType = [
        {
            name: 'Ngày',
            code: 'D'
        },
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Năm',
            code: 'Y'
        }
    ];
    public static interestPeriodTypeQuarter = [
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Quý',
            code: 'Q'
        }
    ];

    public static interestType = [
        {
            name: 'Định kỳ',
            code: 1,
        },
        {
            name: 'Cuối kỳ',
            code: 2,
        }
    ];

    public static INTEREST_RATE_TYPE_PERIODIC = 1;
    public static INTEREST_RATE_TYPE_PERIOD_END = 2;

    public static INTEREST_PERIOD_TYPE_MONTH = 'M';
    public static INTEREST_PERIOD_TYPE_YEAR = 'Y';

    public static KICH_HOAT = 'A';
    public static KHOA = 'D';

    public static status = [
        {
            name: "Kích hoạt",
            code: "A",
            severity: 'success'
        },
        {
            name: "Khóa",
            code: "D",
            severity: 'secondary'
        }
    ];

    public static getSeverityStatus(code) {
        const status = this.status.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameInterestPeriodType(code) {
        let type = this.interestPeriodType.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
    public static getNameInterestPeriodTypeQuarter(code) {
        let type = this.interestPeriodTypeQuarter.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameInterestType(code) {
        let type = this.interestType.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

}

export class OrderConst {

    // STATUS ORDER
    public static KHOI_TAO = 1;
    public static CHO_THANH_TOAN = 2;
    public static CHO_KY_HOP_DONG = 3;
    public static CHO_DUYET_HOP_DONG = 4;
    public static DANG_DAU_TU = 5;
    public static PHONG_TOA = 6;
    public static DA_XOA = 7;

    public static TRA_THANG = 1;
    public static TRA_GOP = 2;

    public static reasonOptions = [   
        {
            name: 'Khách ngoại giao',
            code: 8,
        },
        {
            name: 'Khách xin gia hạn thời gian',
            code: 9,
        },
        {
            name: 'Khác',
            code: 10,
        }
    ];

    public static paymentOptions = [   
        {
            name: 'Thanh toán thường',
            code: 1,
        },
        {
            name: 'Thanh toán sớm',
            code: 2,
        },
        {
            name: 'Trả góp ngân hàng',
            code: 3,
        }
    ];
    public static getpaymentOptions(code, attribute = 'name') {
        const type = this.paymentOptions.find(type => type.code == code);
        return type ? type[attribute] : null;
    }
    public static SO_HUU = 1;
    public static DONG_SO_HUU = 2;
    public static typeOfcontract = [ 
        {
            name: 'Sở hữu',
            code: this.SO_HUU,
        },
        {
            name: 'Đồng sở hữu',
            code: this.DONG_SO_HUU,
        }
    ];
    //
    public static searchFields = [
        {
            name: 'Mã hợp đồng',
            code: 3,
            field: 'contractCode',
            placeholder: 'Nhập mã hợp đồng...',
        },
        {
            name: 'Số điện thoại',
            code: 1,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
        
        {
            name: 'Mã khách hàng',
            code: 2,
            field: 'cifCode',
            placeholder: 'Nhập mã khách hàng...',
        },
       
        {
            name: 'CMND/CCCD',
            code: 4,
            field: 'idNo',
            placeholder: 'Nhập số CMND hoặc CCCD ...'
        }
    ];
    
    public static getInfoFieldFilter(field, attribute: string) {
        const searchField = this.searchFields.find(fieldFilter => fieldFilter.field == field);
        return searchField ? searchField[attribute] : null;
    }
    //
    public static statusOrder = [
        {
            name: "Khởi tạo",
            code: this.KHOI_TAO,
            severity: 'help',
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ thanh toán",
            code: this.CHO_THANH_TOAN,
            severity: 'warning',
            backLink: '/trading-contract/order',
        },
        // {
        //     name: "Chờ ký hợp đồng",
        //     code: this.CHO_KY_HOP_DONG,
        //     severity: "help",
        //     backLink: '/trading-contract/order',
        // },
        {
            name: "Đã xóa",
            code: this.DA_XOA,
            severity: 'secondary',
        }
    ];

    public static statusProcessing = [
        // {
        //     name: "Chờ ký hợp đồng",
        //     code: 3,
        //     severity: "help",
        //     backLink: '/trading-contract/contract-processing',
        // },
        {
            name: "Chờ duyệt hợp đồng",
            code: this.CHO_DUYET_HOP_DONG,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
        },

    ];

    public static statusActive = [
        {
            name: "Đang đầu tư",
            code: this.DANG_DAU_TU,
            severity: "help",
            backLink: '/trading-contract/contract-active',
        },
        {
            name: "Phong tỏa",
            code: this.PHONG_TOA,
            severity: 'danger',
            backLink: '/trading-contract/contract-active',
        },
    ];

    public static statusBlockage = [
        {
            name: "Phong toả",
            code: 6,
            severity: 'danger',
            backLink: '/trading-contract/contract-blockage',
        },
        {
            name: "Giải tỏa",
            code: 7,
            severity: 'success',
            backLink: '/trading-contract/contract-blockage',
        },
    ];

    public static interestTypes = [
        {
            name: 'Có',
            code: 'Y',
        },
        {
            name: 'Không',
            code: 'N',
        }
    ];

    public static INTEREST_TYPE_YES = 'Y';
    public static INTEREST_TYPE_NO = 'N';

    public static sources = [
        {
            name: 'Online',
            code: 1,
            severity: 'success'

        },
        {
            name: 'Offline',
            code: 2,
            severity: 'secondary'

        },
        // {
        //     name: 'Sale đặt lệnh',
        //     code: 3,
        // }
    ];

    public static getOrderSource(code, atribution = 'name') {
        let source = this.sources.find(s => s.code == code);
        return source ? source[atribution] : null;
    }

    public static orderSources = [
        {
            name: 'Quản trị viên',
            code: 1,
            severity: 'success'
        },
        {
            name: 'Khách hàng',
            code: 2,
            severity: 'info'
        },
        {
            name: 'Tư vấn viên',
            code: 3,
            severity: 'warning'
        }
    ];

    public static RENEWAL_PENDING = 1;
    public static RENEWAL_SUCCESS = 2;
    public static RENEWAL_CANCEL = 3;


    public static statusRenewal = [
        {
            name: 'Khởi tạo',
            code: this.RENEWAL_PENDING,
            severity: 'help'

        },
        {
            name: 'Đã tái tục',
            code: this.RENEWAL_SUCCESS,
            severity: 'success'

        },
        {
            name: 'Hủy yêu cầu',
            code: this.RENEWAL_CANCEL,
            severity: 'danger'
        }
    ];

    
    public static investHistoryStatus = [
        {
            name: 'Tất toán đúng hạn',
            code: 1,
            severity: 'success'
        },
        {
            name: 'Tất toán trước hạn',
            code: 2,
            severity: 'info'
        },
        {
            name: 'Tái tục gốc',
            code: 3,
            severity: 'warning'
        },
        {
            name: 'Tái tục gốc và lợi tức',
            code: 4,
            severity: 'danger'
        }
    ];

    public static getHistoryStatus(code, field) {
        const source = this.investHistoryStatus.find(type => type.code == code);
        return source ? source[field] : null;
    }

    public static SOURCE_ONLINE = 1;
    public static SOURCE_OFFLINE = 2;

    public static getStatusRenewal(code, field) {
        const source = this.statusRenewal.find(type => type.code == code);
        return source ? source[field] : null;
    }

    public static getInfoSource(code, field) {
        const source = this.sources.find(type => type.code == code);
        return source ? source[field] : null;
    }

    public static getInfoOrderSource(code, field) {
        const source = this.orderSources.find(type => type.code == code);
        return source ? source[field] : null;
    }

    public static getNameInterestType(code) {
        let type = this.interestTypes.find(type => type.code == code);
        return type ? type.name : '';
    }

    public static status = [
        {
            name: "Khởi tạo",
            code: this.KHOI_TAO,
            severity: 'help',
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ thanh toán",
            code: this.CHO_THANH_TOAN,
            severity: 'warning',
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ ký hợp đồng",
            code: this.CHO_KY_HOP_DONG,
            severity: "help",
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ duyệt hợp đồng",
            code: this.CHO_DUYET_HOP_DONG,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
        },
        {
            name: "Đã đặt cọc",
            code: this.DANG_DAU_TU,
            severity: 'success',
            backLink: '/trading-contract/contract-active',
        },
        {
            name: "Phong toả",
            code: this.PHONG_TOA,
            severity: 'danger',
            backLink: '/trading-contract/contract-blockage',
        },
        {
            name: "Đã xóa",
            code: this.DA_XOA,
            severity: 'secondary',
        }
    ];

    public static getSeverityStatus(code) {
        const status = this.status.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getBackLink(code) {
        const status = this.status.find(type => type.code == code);
        return status ? status.backLink : '/';
    }

    // LOẠI TÁI TỤC
    public static REINSTATEMENT_TYPES = [
        {
            name: 'Tái tục gốc',
            shortName: 'Gốc',
            code: 2,
        },
        {
            name: 'Tái tục gốc + lợi nhuận',
            shortName: 'Gốc + LN',
            code: 3,
        }
    ];

    public static ORIGINAL_REINSTATEMENT = 2;
    public static ALL_REINSTATEMENT = 3;

    public static getReinstatementType(code, attribute = 'name') {
        const type = this.REINSTATEMENT_TYPES.find(type => type.code == code);
        return type ? type[attribute] : null;
    }

    // LOẠI RÚT VỐN (1 PHẦN HOẶC TOÀN BỘ)
    public static WITHDRAWAL_VALUES = [
        {
            name: 'Rút một phần',
            code: 1,
        },
        {
            name: 'Rút toàn bộ',
            code: 2,
        }
    ];

    public static WITHDRAWAL_PARTIAL = 1;
    public static WITHDRAWAL_ALL = 2;

    public static orderers = [
        {
            name: 'Quản trị viên',
            code: 1,
            severity: 'success'
        },
        {
            name: 'Khách hàng',
            code: 2,
            severity: 'info'
        },
        {
            name: 'Tư vấn viên',
            code: 3,
            severity: 'warning'
        }
    ];
    
    public static getInfoOrderer(code, atribution = 'name') {
        const orderer = this.orderers.find(type => type.code == code);
        return orderer ? orderer[atribution] : null;
    }
}


export class InterestPaymentConst {
    public static statusInterests = [
        {
            name: 'Đến hạn chi trả',
            code: 0,
        },
        {
            name: 'Đã lập chưa chi trả',
            code: 1,
        },
        {
            name: 'Đã chi trả',
            code: 2,
        },
        {
            name: 'Đã chi trả (Tự động)',
            code: 3,
        },
        {
            name: 'Đã chi trả (Thủ công)',
            code: 4,
        },
    ];

    public static STATUS_DUEDATE = 0;
    public static STATUS_CREATED_LIST = 1;
    public static STATUS_DONE = 2;
    public static STATUS_DONE_ONLINE = 3;
    public static STATUS_DONE_OFFLINE = 4;


    public static INTEREST_PAYMENT_ONLINE = 2;  // CHI TỰ ĐỘNG
    public static INTEREST_PAYMENT_OFFLINE = 4; // CHI THỦ CÔNG

    public static typeInterests = {
        [this.STATUS_DONE_ONLINE] : this.INTEREST_PAYMENT_ONLINE,
        [this.STATUS_DONE_OFFLINE] : this.INTEREST_PAYMENT_OFFLINE,
    }

    public static statusExpires = [
        {
            name: 'Nhận gốc và lợi tức',
            code: 1,
        },
        {
            name: 'Tái tục gốc',
            code: 2,
        },
        {
            name: 'Tái tục gốc + LN',
            code: 3,
        },
    ];

    public static EXPIRE_DONE = 1;
    public static EXPIRE_RENEWAL_ORIGINAL = 2;
    public static EXPIRE_RENEWAL_PROFIT = 3;

    public static typeExactDates = [
        {
            name: 'Ngày đã chọn',
            code: 'Y',
        },
        {
            name: 'Tất cả ngày',
            code: 'N',
        },
    ];
}

export class InvestorConst {

    public static sourcesCreate = [
        {
            name: 'Online',
            code: 1,
            severity: 'success'

        },
        {
            name: 'Offline',
            code: 2,
            severity: 'secondary'
        },
        {
            name: 'Sale',
            code: 3,
            severity: 'help'
        },
    ];
    public static getSourceCreate(code, field) {
        const source = this.sourcesCreate.find(type => type.code == code);
        return source ? source[field] : null;
    }
    public static getSexName(code) {
        const found = this.ListSex.find(o => o.code === code);
        return found ? found.name : '';
    }
    public static TEMP = {
        YES: 1,
        NO: 0
    };

    public static ID_TYPES = {
        PASSPORT: 'PASSPORT',
        CMND: 'CMND',
        CCCD: 'CCCD',
    }

    public static SEX = {
        MALE: 'M',
        FEMALE: 'F',
    }

    public static SEX_NAME = {
        [this.SEX.MALE]: 'Nam',
        [this.SEX.FEMALE]: 'Nữ',
    }


    public static ListSex = [
        {
            name: this.SEX_NAME[this.SEX.MALE],
            code: this.SEX.MALE,
        },
        {
            name: this.SEX_NAME[this.SEX.FEMALE],
            code: this.SEX.FEMALE,
        },
    ]

    public static IdTypes = [
        {
            name: 'Hộ chiếu',
            code: this.ID_TYPES.PASSPORT
        },
        {
            name: 'Chứng minh nhân dân',
            code: this.ID_TYPES.CMND
        },
        {
            name: 'Căn cước công dân',
            code: this.ID_TYPES.CCCD
        },
    ]

    public static STATUS = {
        KHOI_TAO: 0,
        TRINH_DUYET: 1,
        DUYET: 2,
        HUY: 3,
        XAC_MINH: 4,
    }

    public static statusList = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.STATUS.KHOI_TAO,
        },
        {
            name: 'Trình duyệt',
            code: this.STATUS.TRINH_DUYET,
            severity: 'warning',
        },
        {
            name: 'Duyệt',
            code: this.STATUS.DUYET,
            severity: 'success',
        },
        {
            name: 'Huỷ',
            code: this.STATUS.HUY,
            severity: 'cancel',
        },
        {
            name: 'Duyệt',
            code: this.STATUS.XAC_MINH,
            severity: 'success',
        },
    ];

    public static statusListApprove = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.STATUS.KHOI_TAO,
        },
        {
            name: 'Trình duyệt',
            code: this.STATUS.TRINH_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hủy duyệt',
            code: this.STATUS.HUY,
            severity: 'danger',
        },
        {
            name: 'Đã duyệt',
            code: this.STATUS.DUYET,
            severity: 'success',
        },
    ];

    /**
     * GET TEN TRANG THAI
     * @param code 
     * @returns 
     */
    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    /**
     * GET SEVERITY CUA TRANG THAI
     * @param code 
     * @returns 
     */
    public static getStatusSeverity(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    /**
     * GET NAME LOAI GIAY TO THEO CODE
     * @param code 
     * @returns 
     */
    public static getIdTypeName(code: string) {
        const found = this.IdTypes.find(o => o.code === code);
        return found ? found.name : '';
    }

    public static fieldFilters = [
        {
            name: 'Số điện thoại',
            code: 3,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
        
        {
            name: 'Mã khách hàng',
            code: 2,
            field: 'cifCode',
            placeholder: 'Nhập mã khách hàng...',
        },
        {
            name: 'Số CMND/CCCD',
            code: 4,
            field: 'idNo',
            placeholder: 'Nhập số cmnd/cccd...',
        },
        {
            name: 'Email',
            code: 5,
            field: 'email',
            placeholder: 'Nhập email...',
        },
        {
            name: 'Họ tên',
            code: 1,
            field: 'fullname',
            placeholder: 'Nhập họ tên...',
        },
    ];

    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }

    public static statusApproved = [
        {
            name: 'Hoạt động',
            severity: 'success',
            code: 'A',
        },
        {
            name: 'Sai thông tin',
            severity: 'secondary',
            code: 'D',
        },
    ];

    public static getInfoStatusApproved(code, property) {
        let status = this.statusApproved.find(s => s.code == code);
        return status ? status[property] : null;
    }

    public static listStock = [
        {
            name: 'Công ty cổ phần chứng khoán Tiên Phong - TPS',
            severity: 'help',
            code: 1,
        },
        // {
        //     name: 'Trình duyệt',
        //     code: this.STATUS.TRINH_DUYET,
        //     severity: 'warning',
        // },
        // {
        //     name: 'Hủy duyệt',
        //     code: this.STATUS.HUY,
        //     severity: 'danger',
        // },
    ];
    public static getListStockName(code) {
        for (let item of this.listStock) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static userType  = [
        {
            name: "Epic Center",
            code: 'I',
            severity: 'success'
        },
        {
            name: "Epic CMS",
            code: 'E',
            severity: 'help'
        },
        {
            name: "Epic CMS",
            code: 'P',
            severity: 'help'
        },
        {
            name: "Epic CMS",
            code: 'T',
            severity: 'help'
        },
        {
            name: "Epic CMS",
            code: 'RP',
            severity: 'help'
        },
        {
            name: "Epic CMS",
            code: 'RT',
            severity: 'help'
        },
        {
            name: "Sale",
            code: 'S',
            severity: 'help'
        }
    ];
    
    public static getUserTypeSeverity(code) {
        for (let item of this.userType) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    public static getUserTypeName(code) {
        for (let item of this.userType) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusApproveSeverity(code) {
        for (let item of this.statusListApprove) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    public static getStatusApproveName(code) {
        for (let item of this.statusListApprove) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getInfoSource(code, field) {
        const source = this.sources.find(type => type.code == code);
        return source ? source[field] : null;
    }

    public static sources = [
        {
            name: 'Online',
            code: 1,
            severity: 'success'

        },
        {
            name: 'Offline',
            code: 2,
            severity: 'secondary'
        },
    ];
}

export class MSBPrefixAccountConst {

    public static ID_MSB_BANK = 26

    public static type = [
        {
            name: '1',
            code: 1,
        },
        {
            name: '2',
            code: 2
        },
        {
            name: '3',
            code: 3,
        },
    ];

    public static getName(code) {
        const status = this.type.find(item => item.code == code);
        return status ? status.name : '';
    }
}

export class ErrorBankConst { 

    public static LOI_KET_NOI_MSB = 1505;
    public static SO_TK_KHONG_TON_TAI = 2036;
}

export class MediaConst {

    public static ACTIVE = 'ACTIVE';
    public static TRINH_DUYET = 'PENDING';
    public static NHAP = 'DRAFT';

    public static status = {
        ACTIVE: "ACTIVE",
        PENDING: "PENDING",
        DELETED: "DELETED",
        DRAFT: "DRAFT",
    }
    public static mediaStatus = {
        ACTIVE: 'Đã đăng',
        PENDING: 'Đang chờ',
        DELETED: 'Đã xoá',
        DRAFT: 'Bản nháp'
    }
    public static statusSeverity = {
        ACTIVE: 'success',
        PENDING: 'warning',
        DELETED: 'danger',
        DRAFT: 'secondary'
    }

    public static newsTypes = {
        PURE_NEWS: 'Tin tức', //Tin tức thuần
        SHARING: 'Chia sẻ từ KOL', //Chia sẻ từ KOL
        PROMOTION: 'Ưu đãi' //Ưu đãi
    }

    public static getStatusNews(code) {
        return this.mediaStatus[code];
    }

    public static getStatusSeverity(code) {
        return this.statusSeverity[code]
    }

    public static getNewsType(code) {
        return this.newsTypes[code]
    }
}


export class MediaNewsConst {

    public static positionList = [
        {
            name: "Banner popup",
            code: 'banner_popup'
        },
        {
            name: "Slide ảnh",
            code: 'slide_image'
        },
        {
            name: "Banner trên top",
            code: 'banner_top'
        },
        {
            name: "Hôm nay có gì hot",
            code: 'hot_today'
        },
        {
            name: "Dành cho bạn",
            code: 'just_for_you'
        },
        {
            name: "Video",
            code: 'videos'

        }
    ];
    public static typeList = [
        {
            name: "Trang khám phá",
            code: 'kham_pha'
        },
        {
            name: "Trang đầu tư",
            code: 'dau_tu'
        },
        {
            name: "Trang tài sản",
            code: 'tai_san'
        },
        {
            name: "Sản phẩm",
            code: 'san_pham'
        },
        {
            name: "Sản phẩm đầu tư",
            code: 'san_pham_dau_tu'
        },
        {
            name: "Trang mua BĐS",
            code: 'mua_bds'
        },
        {
            name: "Trang thuê BĐS",
            code: 'thue_bds'
        }
    ];
    public static statusList = [
        {
            name: "Đã đăng",
            code: 'ACTIVE',
            severity: 'success'
        },
        {
            name: "Trình duyệt",
            code: 'PENDING',
            severity: 'warning'
        },
        {
            name: "Đã xóa",
            code: 'DELETED',
            severity: 'danger'
        },
        {
            name: "Bản nháp",
            code: 'DRAFT',
            severity: 'secondary'
        }
    ];

}

export class StatusCoupon {
    public static list = [
        {
            name: 'Đến hạn',
            code: 1,
        },
        {
            name: 'Chưa đến hạn',
            code: 2,
        },
    ]

    public static fieldName = [
        {
            name: 'Tài khoản ngân hàng',
            code: 'BUSINESS_CUSTOMER_BANK_ACC_ID',
        },
        {
            name: 'Số tiền đầu tư',
            code: 'TOTAL_VALUE',
        },
        {
            name: 'Kỳ hạn',
            code: 'POLICY_DETAIL_ID',
        },
        {
            name: 'Mã giới thiệu',
            code: 'SALE_REFERRAL_CODE',
        },
        {
            name: 'Địa chỉ',
            code: 'CONTACT_ADDRESS_ID',
        },
        {
            name: 'Giấy tờ',
            code: 'INVESTOR_IDEN_ID',
        },
        {
            name: 'Hình thức thanh toán',
            code: 'IS_BANK_LOAN'
        },
        {
            name: 'Loại',
            code: 'SOURCE'
        },
        {
            name: 'Loại thanh toán',
            code: 'PAYMNET_TYPE'
        },
        {
            name: 'Số tiền',
            code: 'PAYMNET_AMOUNT'
        },
        {
            name: 'Mô tả thanh toán',
            code: 'DESCRIPTION'
        },
        {
            name: 'Ngày giao dịch',
            code: 'TRAN_DATE'
        },
        {
            name: 'Hủy duyệt hợp đồng',
            code: 'CANCEL'
        },
        {
            name: 'Duyệt hợp đồng',
            code: 'APPROVE'
        }
    ]

    public static getName(code) {
        const status = this.list.find(item => item.code == code);
        return status ? status.name : '';
    }

    public static getFieldName(code) {
        const status = this.fieldName.find(item => item.code == code);
        return status ? status.name : '';
    }
}
//
export class SaleConst {

    public static searchList = [
        {
            field: 'referralCode',
            placeholder: 'Tìm kiếm theo mã giới thiệu...',
            name: 'Mã giới thiệu',
        },
        {
            field: 'phone',
            placeholder: 'Tìm kiếm theo số điện thoại...',
            name: 'Số điện thoại',
        }
    ];
    
    public static getInfoSearch(field, atribution = 'name') {
        
        const fieldSearch = this.searchList.find(item => item.field == field);
        console.log(field, fieldSearch);

        return fieldSearch ? fieldSearch[atribution] : '';
    }
}

export const KeyFilter = {
    blockSpecial: new RegExp(/^[^~!@#$%^&*><:;+=_]+$/),
    numberOnlyBlockSpecial: new RegExp(/^[^\sA-Za-záàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,/-]+$/),
    stringOnlyBlockSpecial: new RegExp(/^[^0-9~!@#$%^&*><:;+=_,/-]+$/),
    decisionNoBlockSpecial: new RegExp(/^[^\sáàảãạâấầẩẫậăắằẳẵặóòỏõọôốồổỗộơớờởỡợéèẻẽẹêếềểễệúùủũụưứừửữựíìỉĩịýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÉÈẺẼẸÊẾỀỂỄỆÚÙỦŨỤƯỨỪỬỮỰÍÌỈĨỊÝỲỶỸỴĐ~!@#$%^&*><:;+=_,]+$/),
}

export class UserConst {

    public static STATUS = {
        DEACTIVE: 'D',
        ACTIVE: 'A',
        TEMPORARY: 'T',
    }

    public static STATUS_NAME = {
        [this.STATUS.DEACTIVE]: 'Bị khoá',
        [this.STATUS.ACTIVE]: 'Hoạt động',
        [this.STATUS.TEMPORARY]: 'Tạm',
    }

    public static STATUS_SEVERITY = {
        [this.STATUS.DEACTIVE]: 'cancel',
        [this.STATUS.ACTIVE]: 'success',
        [this.STATUS.TEMPORARY]: 'warning',
    }

}

export class ContractorConst {

    public static KICH_HOAT = 'A';
    public static HUY_KICH_HOAT = 'D';
    public static DONG = 'C';

    public static status = [
        {
            name: 'Kích hoạt',
            code: this.KICH_HOAT,
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: this.HUY_KICH_HOAT,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.DONG,
            severity: 'secondary',
        }
    ];

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code) {
        let status = this.status.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }
}
export class IsSignPdfConst {
    public static YES = 'Y';
    public static NO = 'N';
}

export class FormNotificationConst {
    public static IMAGE_APPROVE = "IMAGE_APPROVE";
    public static IMAGE_CLOSE = "IMAGE_CLOSE";
}

export class BlockageLiberationConst {

    public static blockageTypes = [
        {
            name: "Khác",
            code: 1
        },
        {
            name: "Cầm cố khoản vay",
            code: 2
        },
        {
            name: "Ứng vốn",
            code: 3
        }
    ];

    public static status = [
        {
            name: "Phong toả",
            code: 1,
            severity: 'danger',
        },
        {
            name: "Giải toả",
            code: 2,
            severity: 'success'
        }
    ];

    public static PHONG_TOA = 1;
    public static GIAI_TOA = 2

    public static getSeverityStatus(code) {
        const status = this.status.find(p => p.code == code);
        return status ? status.severity : '-';
    }

    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class ContractFormConst {

    public static customerType = [
        {
            name: 'Cá nhân',
            code: 'I',
           
        },
        {
            name: 'Doanh nghiệp',
            code: 'B',
            
        },
    ];

    public static statusConst = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Khóa',
            code: 'D',
            severity: 'secondary',
        },
    ];

    public static StatusActive = 'A';
    public static StatusDeactive = 'D';

    public static KyTu = 'FIX_TEXT';

    public static valueList = [
        {
            name: 'Tên dự án',
            code: 'PRODUCT_NAME',
        },
        {
            name: 'Mã dự án',
            code: 'PRODUCT_CODE',
        },
        {
            name: 'Mã căn hộ',
            code: 'RST_PRODUCT_ITEM_CODE',
        },
        {
            name: 'Tên viết tắt nhà đầu tư',
            code: 'SHORT_NAME',
        },
        {
            name: 'Ngày đặt lệnh',
            code: 'BUY_DATE',
        },
        {
            name: 'Ngày thanh toán đủ',
            code: 'PAYMENT_FULL_DATE',
        },
        {
            name: 'Ký tự',
            code: 'FIX_TEXT',
        },
        {
            name: 'Số thứ tự rút gọn',
            code: 'ORDER_ID',
        },
        {
            name: 'Số thứ tự đầy đủ',
            code: 'ORDER_ID_PREFIX_0',
        },
        {
            name: 'Tên chính sách',
            code: 'POLICY_NAME',
        },
        {
            name: 'Mã chính sách',
            code: 'POLICY_CODE',
        },
        // {
        //     name: 'Tên viết tắt sản phẩm',
        //     code: 'PRODUCT_TYPE',
            
        // },
    ];

    public static getStatusInfo(code, atribution = 'name') {
        let status = this.statusConst.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static getCustomerInfo(code, atribution = 'name') {
        let status = this.customerType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

}

export class SampleContractConst {

    public static DAI_LY = 1;
    public static DOI_TAC = 2;

    public static depositContractFormType = [
        {
            name: 'Sử dụng mẫu của đại lý',
            code: 1,
           
        },
        {
            name: ' Sử dụng mẫu của Chủ đầu tư',
            code: 2,
            
        },
    ]

    public static contractType = [
        // {
        //     name: 'Hợp đồng bán BĐS',
        //     code: 1,
           
        // },
        // {
        //     name: 'Hợp đồng mua BĐS',
        //     code: 2,
            
        // },
        {
            name: 'Hợp đồng thanh lý',
            code: 3,
        },
        {
            name: 'Hợp đồng đặt cọc',
            code: 4,
        },
        {
            name: 'Hợp đồng mua bán',
            code: 5
        }
    ];



    public static getContractType(code, atribution = 'name') {
        let status = this.contractType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static ONLINE = 1;
    public static OFFLINE = 2;
    public static TAT_CA = 3;

    public static contractSource = [
        {
            name: 'Tất cả',
            code: 3,
            
        },
        {
            name: 'Online',
            code: 1,
           
        },
        {
            name: 'Offline',
            code: 2,
            
        },
        
    ];

    public static contractSourceDistribution = [
        {
            name: 'Online',
            code: 1,
           
        },
        {
            name: 'Offline',
            code: 2,
            
        },
    ];

    public static getContractSource(code, atribution = 'name') {
        let status = this.contractSource.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static customerType = [
        {
            name: 'Cá nhân',
            code: 'I',
           
        },
        {
            name: 'Doanh nghiệp',
            code: 'B',
            
        },
    ];

    public static statusConst = [
        {
            name: 'Đang sử dụng',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Đã xóa',
            code: 'D',
            severity: 'secondary',
        },
    ];
    
    public static getStatusInfo(code, atribution = 'name') {
        let status = this.statusConst.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static getCustomerInfo(code, atribution = 'name') {
        let status = this.customerType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static statusFilters = [
        {
            name: 'Kích hoạt',
            code: 'A',
        },
        {
            name: 'Khóa',
            code: 'D',
        },
    ] as IDropdown[];
}

export class StatusPaymentBankConst { 

    public static INTEREST_CONTRACT = 'INTEREST_CONTRACT';
    public static RENEWAL_CONTRACT = 'RENEWAL_CONTRACT';
    public static MANAGER_WITHDRAW = 'MANAGER_WITHDRAW';

    public static PENDING = 1;
    public static APPROVE_ONLINE = 2;
    public static CANCEL = 3;
    public static APPROVE_OFFLINE= 4;
    public static END = [this.APPROVE_ONLINE, this.APPROVE_OFFLINE, this.CANCEL];

    public static list = [
        {
            name: 'Chờ xử lý',
            code: this.PENDING,
            severity: 'warning',
        },
        {
            name: 'Chi tự động',
            code: this.APPROVE_ONLINE,
            severity: 'success',
        },
        {
            name: 'Chi thủ công',
            code: this.APPROVE_OFFLINE,
            severity: 'success'
        },
        {
            name: 'Hủy yêu cầu',
            code: this.CANCEL,
            severity: 'danger',
        },
    ];

    public static getInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null; 
    }
    //

    public static RESPONSE_PENDING = 1;
    public static RESPONSE_SUCCESS = 2;
    public static RESPONSE_FAILD = 3;

    public static responses = [
        {
            name: 'Chờ phản hồi',
            code: this.RESPONSE_PENDING,
            severity: 'warning',
        },
        {
            name: 'Thành công',
            code: this.RESPONSE_SUCCESS,
            severity: 'success',
        },
        {
            name: 'Thất bại',
            code: this.RESPONSE_FAILD,
            severity: 'danger'
        }
    ];

    public static getInfoResponse(code, atribution = 'name') {
        let response = this.responses.find(s => s.code == code);
        return response ? response[atribution] : null; 
    }
}
export class MessageBankResponse {
    public static MSB = [
        {
            message: 'Receive account name not correct',
            customMessage: 'Tên chủ tài khoản, số tài khoản hoặc mã BIN không chính xác!'
        },
        //
        {
            message: 'Receive account is lock or not found',
            customMessage: 'Tài khoản nhận bị khóa hoặc không tìm thấy!'
        },
    ];

    public static getMessageMsb(message) {
        let msb = this.MSB.find(m => m.message === message);
        return msb ? msb.customMessage : msb.message;
    }
}

export class StatusActualCashFlow {

    public static KHOI_TAO = 1;
    public static DA_THANH_TOAN = 2;
    public static HUY_YEU_CAU = 3;

    public static list = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'warning'
        },
        {
            name: 'Đã thanh toán',
            code: this.DA_THANH_TOAN,
            severity: 'success'
        },
        {
            name: 'Hủy yêu cầu',
            code: this.HUY_YEU_CAU,
            severity: 'danger'
        },
    ];

    public static getInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : '';
    }
}

export class CollectMoneyBankConst {

    public static paymentType = [
        {
            name: 'Trả lãi',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Rút tiền',
            code: 2,
            severity: 'success',
        },
    ];
    public static getTypes(code, atribution = 'name') {
        const status = this.paymentType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static statusSearchConst = [
        {
            name: 'Khởi tạo',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Thành công',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Thất bại',
            code: 3,
            severity: 'danger',
        },
        
    ];

    public static statusConst = [
        {
            name: 'Khởi tạo',
            code: 1,
            severity: 'warning',
        },
        {
            name: 'Thành công',
            code: 2,
            severity: 'success',
        },
        {
            name: 'Thất bại',
            code: 3,
            severity: 'danger',
        },
    ];

    public static getStatusName(code) {
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code) {
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }
    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class MessageErrorConst {

    public static message = {
        Error: "ACTIVE",
        Validate: "Vui lòng nhập đủ thông tin cho các trường có dấu (*)",
        DataNotFound: "Không tìm thấy dữ liệu!"
    }
    
}

export const sloganWebConst = [
    // "Việc gì khó, đã có EMIR lo!",
    "Đừng bao giờ sợ thất bại. Bạn chỉ cần đúng có một lần trong đời thôi - CEO của Starbucks!",
    "Luôn bắt đầu với mong đợi những điều tốt đẹp sẽ xảy ra – Tom Hopkins!",
    "Nơi nào không có cạnh tranh, nơi đó không có thị trường!",
    "Kẻ chiến thắng không bao giờ bỏ cuộc; kẻ bỏ cuộc không bao giờ chiến thắng!",
    // "Nhất cận thị, nhì cận giang, muốn giàu sang thì…cận sếp!",
    "Công việc quan trọng nhất luôn ở phía trước, không bao giờ ở phía sau bạn!",
    "Chúng ta có thể gặp nhiều thất bại, nhưng chúng ta không được bị đánh bại!",
    "Điều duy nhất vượt qua được vận may là sự chăm chỉ. – Harry Golden!",
    "Muốn đi nhanh thì đi một mình. Muốn đi xa thì đi cùng nhau!",
    "Đôi lúc bạn đối mặt với khó khăn không phải vì bạn làm điều gì đó sai mà bởi vì bạn đang đi đúng hướng!",
    "Điều quan trọng không phải vị trí đứng mà hướng đi. Mỗi khi có ý định từ bỏ, hãy nghĩ đến lý do mà bạn bắt đầu!",
    "Cách tốt nhất để dự đoán tương lai là hãy tạo ra nó!",
    "Kỷ luật là cầu nối giữa mục tiêu và thành tựu!",
    "Di chuyển nhanh và phá vỡ các quy tắc. Nếu vẫn chưa phá vỡ cái gì, chứng tỏ bạn di chuyển chưa đủ nhanh!",
    "Đằng nào thì bạn cũng phải nghĩ, vì vậy sao không nghĩ lớn luôn? - Donald Trump!",
    "Những khách hàng không hài lòng sẽ là bài học tuyệt vời cho bạn - Bill Gates!",
    "Nếu mọi người thích bạn, họ sẽ lắng nghe bạn, nhưng nếu họ tin tưởng bạn, họ sẽ làm kinh doanh với bạn!",
    "Hoàn cảnh thuận lợi luôn chứa đựng những yếu tố nguy hiểm. Hoàn cảnh khó khăn luôn giúp ta vững vàng hơn!",
];

export interface IHeaderColumn {
    field: string,
    header: string,
    width?: string,
    isPin?: boolean,
    isResize?: boolean
    class?: string,
    position?: number,
    fieldSort?: string,
};

export interface IDropdown {
    name: string,
    code: number | string,
    severity?: string,
    rawData?: any,
};

export interface ISelectButton {
    name: string,
    value: number | string,
};

export interface IConstant {
    id: number,
    value: string,
};

export class ProjectOverviewConst {
    public static productTypes = [
        {
            id: 1,
            value: "Chung cư"
        },
        {
            id: 2,
            value: "Biệt thự"
        },
        {
            id: 3,
            value: "Liền kề"
        },
        {
            id: 4,
            value: "Khách sạn"
        },
        {
            id: 5,
            value: "Condotel"
        },
        {
            id: 6,
            value: "Shophouse"
        },
        {
            id: 7,
            value: "Đất nền"
        },
    ] as IConstant[];

    public static getNameProductTypes(types: number[]) {
        if (types.length == 1){
            let type = this.productTypes.find(item => item.id == types[0]);
            return type.value ?? "";
        } else {
            let result = [];
            types.forEach(value => {
                let type = this.productTypes.find(item => item.id == value);
                result.push(type?.value ?? "");
            })
            return result.join(", ");
        }
    }

    public static projectTypeFilters = [
        {
            name: 'Đất đấu giá',
            code: 1,
        },
        {
            name: 'Đất BT',
            code: 2,
        },
        {
            name: 'Đất giao',
            code: 3,
        },
    ] as IDropdown[];

    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static HOAT_DONG = 3;
    public static HUY_DUYET = 4;
    public static DONG = 5;

    public static statusFilters = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'help',
        },
        {
            name: 'Chờ duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Hủy duyệt',
            code: this.HUY_DUYET,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.DONG,
            severity: 'secondary',
        },
    ] as IDropdown[];

    public static getStatusSeverity(code: number) {
        let status = this.statusFilters.find(status => status.code === code);
        if (status) return status.severity;
        return '';
    }

    public static getStatusName(code: number) {
        let status = this.statusFilters.find(status => status.code === code);
        if (status) return status.name;
        return '';
    }

    public static distributionTypes = [
        {
            name: 'Chủ đầu tư phân phối',
            code: 1,
        },
        {
            name: 'Đại lý phân phối',
            code: 2,
        },
    ] as IDropdown[];

    public static projectStatuses = [
        // {
        //     name: 'Đang xây dựng',
        //     code: 1,
        // },
        {
            name: 'Đang bán',
            code: 2,
        },
        {
            name: 'Đã hết hàng',
            code: 3,
        },
        {
            name: 'Tạm dừng bán',
            code: 4,
        },
        {
            name: 'Sắp mở bán',
            code: 5,
        },
    ] as IDropdown[];

    public static utilitiProjectType = [
        {
            id: 1,
            value: 'Nội khu',
        },
        {
            id: 2,
            value: 'Ngoại khu',
        }
    ] as IConstant[];

    public static statusActive = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: 'D',
            severity: 'danger',
        },
    ] as IDropdown[];

    public static getStatusActiveSeverity(code: number) {
        let status = this.statusActive.find(status => status.code === code);
        if (status) return status.severity;
        return '';
    }

    public static getStatusActiveName(code: number) {
        let status = this.statusActive.find(status => status.code === code);
        if (status) return status.name;
        return '';
    }

    public static hightLights = [
        {
            name: 'Nổi bật',
            code: 1,
        }
    ] as IDropdown[];
}

export class ProjectStructureConst {
    //
    public static TOA = 1;
    public static PHAN_KHU = 2;
    public static O_DAT = 3;
    public static LO = 4;
    public static TANG = 5;

    public static buildingDensityTypes = [
        {
            name: 'Tòa',
            code: this.TOA,
        },
        {
            name: 'Phân khu',
            code: this.PHAN_KHU,
        },
        // {
        //     name: 'Ô đất',
        //     code: this.O_DAT,
        // },
        {
            name: 'Lô',
            code: this.LO,
        },
        {
            name: 'Tầng',
            code: this.TANG,
        },
    ]

    public static buildingDensityTypesLevel1 = [
        {
            name: 'Tòa',
            code: this.TOA,
        },
        {
            name: 'Phân khu',
            code: this.PHAN_KHU,
        },
        // {
        //     name: 'Ô đất',
        //     code: this.O_DAT,
        // }, 
    ]
    public static buildingDensityTypesLevel2 = [
        {
            name: 'Tòa',
            code: this.TOA,
        },
        {
            name: 'Lô',
            code: this.LO,
        },
        {
            name: 'Tầng',
            code: this.TANG,
        },
    ]

    public static buildingDensityTypesLevel1Toa = [
        {
            name: 'Tầng',
            code: this.TANG,
        },
    ];
    public static buildingDensityTypesLevel1PhanKhu = [
        {
            name: 'Tòa',
            code: this.TOA,
        },
        {
            name: 'Lô',
            code: this.LO,
        },
    ]

    public static getBuildingDensityTypeName(code, atribution = 'name') {
        let status = this.buildingDensityTypes.find(s => s.code == code);
        return status ? status[atribution] : '';
    }

    public static getBuildingDensitys(parentType?: number, hasChild?: false) {
        let data = [];
        if(!parentType) {
            data = this.buildingDensityTypes.filter(b => ([this.TOA, this.PHAN_KHU, this.O_DAT].includes(b.code) && !hasChild) || (this.PHAN_KHU == b.code));
        } else {
            if(parentType == this.TOA) {
                data = this.buildingDensityTypes.filter(b => b.code == this.TANG);
            } else if(parentType == this.PHAN_KHU) {
                data = this.buildingDensityTypes.filter(b => [this.TOA, this.LO].includes(b.code));
            }
        }
        return data;
        // return this.buildingDensityTypes;
    }

    public static NODE_1 = 1;
    public static NODE_2 = 2;

    public static CREATE = 1;
    public static UPDATE = 2;
}


export class ProjectMedia {
    public static LOGO_DU_AN = "LogoDuAn";
    public static ANH_DAI_DIEN_DU_AN = "AnhDaiDienDuAn";
    public static BANNER_QUANG_CAO_DU_AN = "BannerQuangCaoDuAn";
    public static SLIDE_HINH_ANH_DU_AN = "SlideHinhAnhDuAn";
    public static TVC = "TVC";
    public static ANH_360 = "Anh360";
    public static CAN_HO_MAU_DU_AN = "CanHoMauDuAn";
    public static ANH_MAT_BANG_DU_AN = "AnhMatBangDuAn";
    public static TIEN_ICH = "TienIch";
    public static ANH_DAI_DIEN_CAN_HO = "AnhDaiDienCanHo";
    public static SLIDE_HINH_ANH_CAN_HO = "SlideHinhAnhCanHo";
    public static MAT_BANG_CAN_HO = "MatBangCanHo";
    public static VAT_LIEU = "VatLieu";

    public static types = [
        {
            name: 'Logo dự án',
            code: this.LOGO_DU_AN,
            description: '',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Ảnh đại diện dự án',
            code: this.ANH_DAI_DIEN_DU_AN,
            description: '',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Banner quảng cáo dự án',
            code: this.BANNER_QUANG_CAO_DU_AN,
            description: 'Hình ảnh hiển thị tại các màn tổng quan danh sách các dự án trên ứng dụng',
            typeFile: 'image',
            isMultiple: true,
            maxSize: 10
        },
        {
            name: 'Slide hình ảnh dự án',
            code: this.SLIDE_HINH_ANH_DU_AN,
            description: 'Hình ảnh hiển thị màn hình xem thông tin chi tiết dự án trên ứng dụng',
            typeFile: 'image',
            isMultiple: true,
            maxSize: 10
        },
        {
            name: 'TVC',
            code: this.TVC,
            description: 'Video ngắn giới thiệu khi xem thông tin dự án',
            typeFile: 'video',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Ảnh 360/VR',
            code: this.ANH_360,
            description: 'Hình ảnh toàn cảnh khi xem thông tin dự án, có thể đính kèm liên kết điều hướng tới trang thứ 3',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Ảnh mặt bằng dự án',
            code: this.ANH_MAT_BANG_DU_AN,
            description: 'Danh sách hình ảnh về mặt bằng tổng thể khi xem chi tiết thiết kế dự án',
            typeFile: 'image',
            isMultiple: true,
        },
        {
            name: 'Ảnh tiện ích',
            code: this.TIEN_ICH,
            description: 'Danh sách hình ảnh tiện ích nổi bật khi xem chi tiết thiết kế dự án',
            typeFile: 'image',
            isMultiple: true,
        },
    ];

    public static productTypes = [
        {
            name: 'Ảnh đại diện căn hộ',
            code: this.ANH_DAI_DIEN_CAN_HO,
            description: '',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Slide hình ảnh',
            code: this.SLIDE_HINH_ANH_CAN_HO,
            description: 'Slide hình ảnh giới thiệu tổng quan về căn hộ',
            typeFile: 'image',
            isMultiple: true,
        },
        {
            name: 'Mặt bằng căn hộ',
            code: this.MAT_BANG_CAN_HO,
            description: 'Hình ảnh đại diện cho sơ đồ mặt bằng căn hộ',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
        {
            name: 'Vật liệu',
            code: this.VAT_LIEU,
            description: 'Hình ảnh đại diện cho vật liệu thi công căn hộ',
            typeFile: 'image',
            isMultiple: false,
            maxSize: 1
        },
    ];

    public static statusConst = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: 'D',
            severity: 'danger',
        },
    ];

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static getInfoType(code, atribution = 'name') {
        const type = this.types.find(type => type.code == code);
        return type ? type[atribution] : 'None';
    }
}

export class ProjectPolicyConst{

    public static CHINH_SACH_QUA_TANG = 1;

    public static PolicyTypes = [
        {
            name: 'Chính sách quà tặng',
            code: this.CHINH_SACH_QUA_TANG
        },
        // {
        //     name: 'Chính sách thanh toán sớm',
        //     code: 2
        // },
        // {
        //     name: 'Chính sách mua nhiều',
        //     code: 3
        // },
        // {
        //     name: 'Chính sách ngoại giao',
        //     code: 4
        // },
    ];

    public static ONLINE = 1;
    public static OFFLINE = 2;
    public static TAT_CA = 3;

    public static sources = [
        {
            name: 'Tất cả',
            code: 3,
            
        },
        {
            name: 'Online',
            code: 1,
           
        },
        {
            name: 'Offline',
            code: 2,
            
        },
    ];

    public static getSource(code, atribution = 'name') {
        let status = this.sources.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static getNamePolicyType(code){
        let type = this.PolicyTypes.find(type => type.code == code);
        return type.name ?? '';
    }

    public static statusConst = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Khóa',
            code: 'D',
            severity: 'danger',
        },
    ];

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static getInfo(code, atribution = 'name') {
        let status = this.statusConst.find(type => type.code == code);
        return status ? status[atribution] : null;
    }
    
} 

export class ProjectFileConst{
    public static JuridicalFileTypes = [
        {
            name: 'Hồ sơ pháp lý',
            code: 1
        },
    ]

    public static getNameTypeFile(code){
        let type = this.JuridicalFileTypes.find( type => type.code == code);
        return type.name ?? '';
    }

    public static statuses = [
        {
            name: 'Kích hoạt',
            code: 'A'
        },
        {
            name: 'Hủy kích hoạt',
            code: 'D'
        },
    ];
}

export class SellingPolicyConst {
    //
    public static NO = 'N';
    public static YES = 'Y';
    public static types = [
        {
            name: 'Online',
            code: 1,
        },
        {
            name: 'Offline',
            code: 2,
        },
        {
            name: 'Tất cả',
            code: 3,
        },
    ];


    public static AP_DUNG_THEO_GIA_TRI_CAN_HO = 1;
    public static AP_DUNG_TAT_CA_CAC_CAN = 2;
    public static sellingPolicyType = [
        {
            name: "Theo giá trị căn hộ",
            code: this.AP_DUNG_THEO_GIA_TRI_CAN_HO,
        },
        {
            name: "Áp dụng tất cả các căn",
            code: this.AP_DUNG_TAT_CA_CAC_CAN,
        }
    ]

    public static classifyPolicy = [
       
        {
            name: 'Hợp tác',
            code: 1,
        },
        {
            name: 'Mua bán',
            code: 2,
        },
    ];

    public static getNameType(code) {
        let type = this.types.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getSellingPolicy(code) {
        let type = this.sellingPolicyType.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export interface IActionTable {
    data: any;
    label: string;
    icon: string;
    command: Function,
};

export class ProductDistributionConst {

    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static DANG_PHAN_PHOI = 3;
    public static TAM_DUNG = 4;
    public static HET_HANG = 5;
    public static DA_BAN = 5;
    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';
    public static KHOA_CAN = 3;
    //
    public static statuses = [
        {
            name: 'Khởi tạo',
            code: 1,
            severity: 'danger',
        },
        {
            name: 'Chờ duyệt',
            code: 2,
            severity: 'warning',
        },
        {
            name: 'Đang phân phối',
            code: 3,
            severity: 'success',
        },
        {
            name: 'Tạm dừng',
            code: 4,
            severity: 'help',
        },
        {
            name: 'Hết hàng',
            code: 5,
            severity: 'secondary',
        },
    ] as IDropdown[];

    public static getStatus(code: number, atribution = 'name') {
        let status = this.statuses.find(status => status.code === code);
        return status ? status[atribution] : '';
    }

    public static bookTypes = [
        {
            value: 'Có sổ đổ',
            id: 1,
        },
        {
            value: 'Sổ đỏ 50 năm',
            id: 2,
        },
        {
            value: 'Sổ lâu dài',
            id: 3,
        },
        {
            value: 'Chưa có sổ',
            id: 4,
        },
    ] as IConstant[];

    public static listPayType = [
        {
            name: 'Thanh toán thường',
            code: 1,
        },
        {
            name: 'Trả góp ngân hàng',
            code: 2,
        },
        {
            name: 'Trả trước',
            code: 3,
        },
    ] as IDropdown[];

    public static listDepositType = [
        {
            name: 'Theo giá trị căn hộ',
            code: 1,
        },
        {
            name: 'Giá cố định',
            code: 2,
        },
    ] as IDropdown[];

    public static listLockType = [
        {
            name: 'Theo giá trị căn hộ',
            code: 1,
        },
        {
            name: 'Giá cố định',
            code: 2,
        },
    ] as IDropdown[];

    public static listStatusDetail = [
        {
            name: 'Khởi tạo',
            code: 1,
            severity: 'help',
        },
        {
            name: 'Giữ chỗ',
            code: 2,
            severity: 'warning',
        },
        {
            name: 'Khóa căn',
            code: 3,
            severity: 'secondary',
        },
        {
            name: 'Đã cọc',
            code: 4,
            severity: 'success',
        },
        {
            name: 'Đã bán',
            code: 5,
            severity: 'primary',
        },
        {
            name: 'Chưa mở bán',
            code: 6,
            severity: 'danger',
        },
        {
            name: 'Đang bán',
            code: 7,
            severity: 'info',
        },
    ] as IDropdown[];

    public static getStatusDetail(code: number, atribution = 'name') {
        let status = this.listStatusDetail.find(status => status.code === code);
        return status ? status[atribution] : '';
    }

    public static listStatusActive = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: 'D',
            severity: 'danger',
        },
    ]

    public static getStatusActive(code: string, atribution = 'name') {
        let status = this.listStatusActive.find(status => status.code === code);
        return status ? status[atribution] : '';
    }

    public static keyStorage = 'productDistribution';
}
export class ProductConst {
    //type 
    public static PROJECT_LIST = 'PROJECT_LIST';
    public static PRODUCT_LIST = 'PRODUCT_LIST';
    public static OPEN_SELL = 'OPEN_SELL';
    // Phân loại sản phẩm
    public static CAN_HO_THONG_THUONG = 1;
    public static CAN_HO_STUDIO = 2;
    public static CAN_HO_OFFICETEL = 3;
    public static CAN_HO_SHOPHOUSE = 4;
    public static CAN_HO_PENTHOUSE = 5;
    public static CAN_HO_DUPLEX = 6;
    public static CAN_HO_SKY_VILLA= 7;
    public static NHA_O_NONG_THON = 8;
    public static BIET_THU_NHA_O = 9;
    public static LIEN_KE = 10;
    public static CHUNG_CU_THAP_TANG = 11;
    public static CAN_SHOPHOUSE = 12;
    public static BIET_THU_NGHI_DUONG = 13;
    public static VILLA = 14;
    public static DUPLEX_POOL = 15;
    public static BOUTIQUE_HOTEL = 16;

    public static classifyTypes = [
        {
            name: 'Căn hộ thông thường',
            code: this.CAN_HO_THONG_THUONG,
        },
        {
            name: 'Studio',
            code: this.CAN_HO_STUDIO,
        },
        {
            name: 'Căn hộ Officetel',
            code: this.CAN_HO_OFFICETEL,
        },
        {
            name: 'Shophouse',
            code: this.CAN_HO_SHOPHOUSE,
        },
        {
            name: 'Penthouse',
            code: this.CAN_HO_PENTHOUSE,
        },
        {
            name: 'Duplex',
            code: this.CAN_HO_DUPLEX,
        },
        {
            name: 'Sky Villa',
            code: this.CAN_HO_SKY_VILLA,
        },
        {
            name: 'Nhà ở nông thôn',
            code: this.NHA_O_NONG_THON,
        },
        {
            name: 'Biệt thự nhà ở',
            code: this.BIET_THU_NHA_O,
        },
        {
            name: 'Liền kề',
            code: this.LIEN_KE,
        },
        {
            name: 'Chung cư thấp tầng',
            code: this.CHUNG_CU_THAP_TANG,
        },
        {
            name: 'Căn Shophouse',
            code: this.CAN_SHOPHOUSE,
        },
        {
            name: 'Biệt thự nghỉ dưỡng',
            code: this.BIET_THU_NGHI_DUONG,
        },
        {
            name: 'Villa',
            code: this.VILLA,
        },
        {
            name: 'Duplex Pool',
            code: this.DUPLEX_POOL
        },
        {
            name: 'Boutique Hotel',
            code: this.BOUTIQUE_HOTEL
        }
    ]

    public static classifyTypesToa = [
        {
            name: 'Căn hộ thông thường',
            code: this.CAN_HO_THONG_THUONG,
        },
        {
            name: 'Chung cư thấp tầng',
            code: this.CHUNG_CU_THAP_TANG,
        },
        {
            name: 'Studio',
            code: this.CAN_HO_STUDIO,
        },
        {
            name: 'Shophouse',
            code: this.CAN_HO_SHOPHOUSE,
        },
        {
            name: 'Penthouse',
            code: this.CAN_HO_PENTHOUSE,
        },
        {
            name: 'Duplex',
            code: this.CAN_HO_DUPLEX,
        },
        {
            name: 'Duplex Pool',
            code: this.DUPLEX_POOL
        },
        {
            name: 'Sky Villa',
            code: this.CAN_HO_SKY_VILLA,
        },
    ]

    public static classifyTypesLo = [
        {
            name: 'Nhà ở nông thôn',
            code: this.NHA_O_NONG_THON,
        },
        {
            name: 'Biệt thự nhà ở',
            code: this.BIET_THU_NHA_O,
        },
        {
            name: 'Liền kề',
            code: this.LIEN_KE,
        },
        {
            name: 'Căn Shophouse',
            code: this.CAN_SHOPHOUSE,
        },
        {
            name: 'Biệt thự nghỉ dưỡng',
            code: this.BIET_THU_NGHI_DUONG,
        },
        {
            name: 'Villa',
            code: this.VILLA,
        },
        {
            name: 'Boutique Hotel',
            code: this.BOUTIQUE_HOTEL
        }
    ]

    public static getclassifyTypesName(code, atribution = 'name') {
        const type = this.classifyTypes.find(rt => rt.code == code);
        return type ? type[atribution] : '';
    }

    public static classifyTypeGroupFirst = [
        this.CAN_HO_THONG_THUONG,
        this.CAN_HO_STUDIO,
        this.CAN_HO_OFFICETEL,
        this.CAN_HO_SHOPHOUSE,
        this.CAN_HO_PENTHOUSE,
        this.CAN_HO_DUPLEX,
        this.DUPLEX_POOL,
        this.CAN_HO_SKY_VILLA,
    ];

    public static classifyTypeGroupSecond = [
        this.NHA_O_NONG_THON,
        this.BIET_THU_NHA_O,
        this.LIEN_KE,
        this.CHUNG_CU_THAP_TANG,
        this.CAN_SHOPHOUSE,
        this.BIET_THU_NGHI_DUONG,
        this.VILLA,
        this.BOUTIQUE_HOTEL
    ];
    
    // Loại sổ đỏ
    public static CHUA_CO_SO = 4;
    public static redBookTypes = [
        {
            name: 'Có sổ đỏ',
            code: 1,
        },
        {
            name: 'Sổ 50 năm',
            code: 2,
        },
        {
            name: 'Sổ lâu dài',
            code: 3,
        },
        {
            name: 'Chưa có số',
            code: 4,
        },
    ];

    public static getRedBookTypeName(code, atribution = 'name') {
        const type = this.redBookTypes.find(rt => rt.code == code);
        return type ? type[atribution] : '';
    }

    // Hướng cửa | Hướng ban công
    public static doorDirectionNames = [
        "Đông", 
        "Tây",
        "Nam", 
        "Bắc",
        "Đông Nam",
        "Đông Bắc",
        "Tây Nam",
        "Tây Bắc",
        "Đông Nam & Tây Nam",
        "Đông Nam & Đông Bắc",
        "Tây Nam & Tây Bắc",
        "Đông Nam & Tây Bắc",
        "Đông Bắc & Tây Bắc",
        "Đông Bắc & Tây Nam",
    ]

    public static doorDirections = this.doorDirectionNames.map((name, index) => {
        return {
            name: name,
            code: index + 1,
        }
    });

    // Số lượng phòng
    public static roomQuantityName = [
        "1 PN", 
        "2 PN", 
        "3 PN", 
        "4 PN", 
        "5 PN", 
        "6 PN", 
        "7 PN", 
        "8 PN", 
        "1 PN + 1 WC", 
        "2 PN + 1 WC", 
        "2 PN + 2 WC", 
        "3 PN + 2 WC", 
    ]

    public static roomTypes = this.roomQuantityName.map((name, index) => {
        return {
            name: name,
            code: index + 1,
        }
    });

    public static getRoomTypeName(code, atribution = 'name') {
        const type = this.roomTypes.find(rt => rt.code == code);
        return type ? type[atribution] : '';
    }

    // Vị trí căn
    public static locationNames = [
        'Căn giữa',
        'Căn góc',
        'Cổng chính',
        'Tòa riêng',
        'Căn thông tầng',
    ];

    public static locations = this.locationNames.map((name, index) => {
        return {
            name: name,
            code: index + 1,
        }
    });

    public static getLocationName(code, atribution = 'name') {
        const location = this.locations.find(location => location.code == code);
        return location ? location[atribution] : '';
    }

    public static SINGLE_TYPE = 1;
    public static GRAFT_TYPE = 2;

    public static types = [
        {
            name: 'Căn đơn',
            code: this.SINGLE_TYPE,
        },
        {
            name: 'Căn ghép',
            code: this.GRAFT_TYPE,
        },
    ];

    public static getType(code, atribution) {
        const type = this.types.find(type => type.code == code);
        return type ? type[atribution] : '';
    }
    //
    public static handingTypes = [
        {
            name: 'Bàn giao thô',
            code: 1,
        },
        {
            name: 'Nội thất cơ bản',
            code: 2,
        },
        {
            name: 'Nội thất liền tường',
            code: 3,
        },
        {
            name: 'Nội thất cao cấp',
            code: 4,
        },
        {
            name: 'Full nội thất',
            code: 5
        }
    ];

    public static getHandingType(code, atribution) {
        const type = this.handingTypes.find(type => type.code == code);
        return type ? type[atribution] : '';
    }

    public static KHOI_TAO = 1;
    public static GIU_CHO = 2;
    public static KHOA_CAN = 3;
    public static DA_COC = 4;
    public static DA_BAN = 5;
    public static CHUA_MO_BAN = 6;
    public static DANG_MO_BAN = 7;

    public static statuses = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'warning',
        },
        {
            name: 'Giữ chỗ',
            code: this.GIU_CHO,
            severity: 'secondary',
        },
        {
            name: 'Khóa căn',
            code: this.KHOA_CAN,
            severity: 'secondary',
        },
        {
            name: 'Đã đặt cọc',
            code: this.DA_COC,
            severity: 'warning',
        },
        {
            name: 'Đã bán',
            code: this.DA_BAN,
            severity: 'success',
        },
        {
            name: 'Chưa mở bán',
            code: this.CHUA_MO_BAN,
            severity: 'secondary',
        },
        {
            name: 'Đang mở bán',
            code: this.DANG_MO_BAN,
            severity: 'success',
        },
    ];



    public static getStatus(code, atribution = 'name') {
        const status = this.statuses.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static listCard = [
        {
            backgroundColor: '#F7F0FA',
            numberColor: '#55008C',
            backgroundColorFull: '#55008C',
            numberColorFull: '#FFF',
            code: this.CHUA_MO_BAN,
            title: 'Chưa mở bán',
            countName: 'notOpenCount'
        },
        {
            backgroundColor: '#F4FCF9',
            numberColor: '#0FB579',
            backgroundColorFull: '#0FB579',
            numberColorFull: '#FFF',
            code: this.DANG_MO_BAN,
            title: 'Đang mở bán',
            countName: 'openCount'
        },
        {
            backgroundColor: '#EAEBFA',
            numberColor: '#4C95EB',
            backgroundColorFull: '#4C95EB',
            numberColorFull: '#FFF',
            code: this.GIU_CHO,
            title: 'Giữ chỗ',
            countName: 'holdCount'
        },
        {
            backgroundColor: '#F7F7F7',
            numberColor: '#716F6E',
            backgroundColorFull: '#C9C9C9',
            numberColorFull: '#FFF',
            code: this.KHOA_CAN,
            title: 'Khóa căn',
            countName: 'lockCount'
        },
        {
            backgroundColor: '#FEF9F5',
            numberColor: '#F08F35',
            backgroundColorFull: '#F08F35',
            numberColorFull: '#FFF',
            code: this.DA_COC,
            title: 'Đã đặt cọc',
            countName: 'depositCount'
        },
        {
            backgroundColor: '#FEF5F8',
            numberColor: '#EB4C60',
            backgroundColorFull: '#EB4C60',
            numberColorFull: '#FFF',
            code: this.DA_BAN,
            title: 'Đã bán',
            countName: 'soldCount'
        },
    ]

    public static statusListProduct = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'warning',
        },
        {
            name: 'Chưa mở bán',
            code: this.CHUA_MO_BAN,
            severity: 'purple-fade',
        },
        {
            name: 'Đang mở bán',
            code: this.DANG_MO_BAN,
            severity: 'green-fade',
        },
        {
            name: 'Giữ chỗ',
            code: this.GIU_CHO,
            severity: 'blue-fade',
        },
        {
            name: 'Khóa căn',
            code: this.KHOA_CAN,
            severity: 'silver-fade',
        },
        {
            name: 'Đã cọc',
            code: this.DA_COC,
            severity: 'orange-fade',
        },
        {
            name: 'Đã bán',
            code: this.DA_BAN,
            severity: 'red-fade',
        },
    ] as IDropdown[];

    public static getStatusListProduct(code, atribution = 'name') {
        const status = this.statusListProduct.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class ProjectSale {
    public static statusFilters = [
        {
            name: 'Đang phân phối',
            code: 1,
            severity: 'success',
        },
        {
            name: 'Tạm dừng',
            code: 2,
            severity: 'danger',
        },
        {
            name: 'Khởi tạo',
            code: 3,
            severity: 'warning',
        },
        {
            name: 'Hết hàng',
            code: 4,
            severity: 'secondary',
        },
    ] as IDropdown[];

    public static getStatusSeverity(code: number) {
        let status = this.statusFilters.find(status => status.code === code);
        if (status) return status.severity;
        return '';
    }

    public static getStatusName(code: number) {
        let status = this.statusFilters.find(status => status.code === code);
        if (status) return status.name;
        return '';
    }

    public static keyStorage = 'projectSale';
}
export class OpenSellConst {

    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static DANG_BAN = 3;
    public static TAM_DUNG = 4;
    public static HET_HANG = 5;
    public static HUY_DUYET = 6;
    public static DUNG_BAN = 7;

    public static statuses = [
        {
            name: 'Khởi tạo',
            code: this.KHOI_TAO,
            severity: 'orange-fade',
        },
        {
            name: 'Chờ duyệt',
            code: this.CHO_DUYET,
            severity: 'warning',
        },
        {
            name: 'Đang bán',
            code: this.DANG_BAN,
            severity: 'green-fade',
        },
        {
            name: 'Tạm dừng',
            code: this.TAM_DUNG,
            severity: 'help',
        },
        {
            name: 'Hết hàng',
            code: this.HET_HANG,
            severity: 'secondary',
        },
        {
            name: 'Huỷ duyệt',
            code: this.HUY_DUYET,
            severity: 'danger',
        },
        {
            name: 'Dừng bán',
            code: this.DUNG_BAN,
            severity: 'red-fade',
        },
    ];

    public static getStatus(code, atribution = 'name') {
        const status = this.statuses.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

    public static TK_DAI_LY = 1;
    public static TK_DOI_TAC = 2;
    public static TAT_CA_TK = 3;

    public static bankTypes = [
        {
            name: 'Sử dụng tài khoản đại lý',
            code: this.TK_DAI_LY,
            severity: 'help',
        },
        {
            name: 'Sử dụng tài khoản chủ đầu tư',
            code: this.TK_DOI_TAC,
            severity: 'warning',
        },   
        {
            name: 'Sử dụng tài khoản đại lý và chủ đầu tư',
            code: this.TAT_CA_TK,
            severity: 'warning',
        },   
    ];
}

export class OpenSellFileConst {
    public static types = [
        {
            name: 'Tài liệu phân phối',
            code: 1,
            severity: 'danger',
        },
        {
            name: 'Chính sách ưu đãi',
            code: 2,
            severity: 'pending',
        },
        {
            name: 'Chương trình bán hàng',
            code: 3,
            severity: 'success',
        },
        {
            name: 'Tài liệu bán hàng',
            code: 4,
            severity: 'success',
        },
    ];
    public static getTypes(code, atribution = 'name') {
        const status = this.types.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class LockApartmentConst {
    public static typeLock = {
        PRODUCT_ITEM: 'PRODUCT_ITEM',
        OPEN_SELL: 'OPEN_SELL',
    }
    public static listReason = [
        {
            name: 'Khác',
            code: 1,
            type: [this.typeLock.PRODUCT_ITEM,this.typeLock.OPEN_SELL],
        },
        {
            name: 'Đã bán offline',
            code: 2,
            type: [this.typeLock.PRODUCT_ITEM,this.typeLock.OPEN_SELL],
        },
        {
            name: 'Giữ căn cho khách ngoại giao',
            code: 3,
            type: [this.typeLock.PRODUCT_ITEM,this.typeLock.OPEN_SELL],
        },
        {
            name: 'Khóa làm tài sản đảm bảo',
            code: 4,
            type: [this.typeLock.PRODUCT_ITEM,this.typeLock.OPEN_SELL],
        },
        {
            name: 'Hủy phân phối',
            code: 5,
            type: [this.typeLock.PRODUCT_ITEM],
        },
        {
            name: 'Hủy mở bán',
            code: 6,
            type: [this.typeLock.OPEN_SELL],
        },
    ]
}

export class HistoryConst {
    public static THEM_MOI = 1;
    public static SUA = 2;
    public static XOA = 3;

    public static actions = [
        {
            name: 'Thêm mới',
            code: this.THEM_MOI
        },
        {
            name: 'Chỉnh sửa',
            code: this.SUA
        },
        {
            name: 'Xóa',
            code: this.XOA
        }
    ];

    public static getActionName(code){
        const action = this.actions.find(s => s.code == code);
        return action ? action.name : null;
    }
}

export class SortConst {
    public static ASCENDING = 1;
    public static DESCENDING = -1;

    public static listSort = [
        {
            code: this.ASCENDING,
            value: 'asc',
        },
        {
            code: this.DESCENDING,
            value: 'desc',
        },
    ];

    public static getValueSort(code){
        const sort = this.listSort.find(s => s.code == code);
        return sort ? sort.value : null;
    }
}

export class TabView {
    //
    public static FIRST = 0;
    public static SECOND = 1;
    public static THIRD = 2;
    public static FOURTH = 3;
    public static FIFTH = 4;
    public static SIXTH = 5;
    public static SEVENTH = 6;
    public static EIGHTH = 7;
    public static NINTH = 8;
    public static TENTH = 9;
}

export const COMPARE_TYPE = {
    EQUAL: 1,
    GREATER: 2,
    LESS: 3,
    GREATER_EQUAL: 4,
    LESS_EQUAL: 5,
}

export class TypeFormatDateConst {
    public static DMY       = 'DD/MM/YYYY';
    public static DMYHm     = 'DD/MM/YYYY HH:mm';
    public static DMYHms    = 'DD/MM/YYYY HH:mm:ss';
    public static YMD       = 'YYYY/MM/DD';
    public static YMDHm     = 'YYYY/MM/DD HH:mm';
    public static YMDHms    = 'YYYY/MM/DD HH:mm:ss';
    public static YMDTHms    = 'YYYY-MM-DDTHH:mm:ss';
    public static YMDTHmsZ    = 'YYYY-MM-DDTHH:mm:ssZ';
}

export class DashboardConst {
    public static DAT_LENH_MOI_GIAO_DICH_MOI = 1;

    public static listAction = [
        {
            value: 'Đặt lệnh mới giao dịch mới',
            id: this.DAT_LENH_MOI_GIAO_DICH_MOI,
        },
    ] as IConstant[]; 
}

export const TRADING_CONTRACT_ROUTER = 'trading-contract';

export class QueryMoneyBankCost{
    public static GARNER = 1;
    public static INVEST = 2;
    public static COMPANY_SHARE = 3;
    public static REAL_ESTATE = 4;
}

// export class TradingProviderConst {
//     public static KICH_HOAT = 1;
//     public static DONG = 2;

//     public static listStatus = [
//         {
//             name: 'Kích hoạt',
//             code: this.KICH_HOAT
//         },
//         {
//             name: "Đóng",
//             code: this.DONG
//         }
//     ]
// }

export class ProjectShareConst{
    public static TEMP = 'T';
    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static listStatus = [
        {
            name: 'Tạm',
            code: this.TEMP,
            severity: 'secondary',
        },
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: this.DEACTIVE,
            severity: 'danger',
        },
    ];

    public static getInfo(code, atribution = 'name') {
        let status = this.listStatus.find(type => type.code == code);
        return status ? status[atribution] : null;
    }
}

export class TradingProviderConst {
    public static KICH_HOAT = 1;
    public static HUY_KICH_HOAT = 2;

    public static listStatus = [
        {
            name: 'Kích hoạt',
            code: this.KICH_HOAT,
            severity: 'success',
        },
        {
            name: "Hủy kích hoạt",
            code: this.HUY_KICH_HOAT,
            severity: 'secondary',
        }
    ];

    public static getStatus(code, attribute = 'name') {
        let status = this.listStatus.find(status => status.code == code);
        return status ? status[attribute] : null;
    }
}

export const AtributionConfirmConst = {
    header: 'Thông báo!',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Đồng ý',
    rejectLabel: 'Hủy bỏ',
}

export class ExportExcelConst{
    public static TONG_QUAN_BANG_HANG_DU_AN = "product-project-overview";
    public static TONG_HOP_TIEN_VE_DU_AN = "synthetic-money-project";
    public static TONG_HOP_CAC_KHOAN_GIAO_DICH = "synthetic-trading";
}

export class ProductItemFileConst{
    public static MATERIAL = 'product-item-material';
    public static DESIGN_DIAGRAM = 'product-item-design-diagram';
}