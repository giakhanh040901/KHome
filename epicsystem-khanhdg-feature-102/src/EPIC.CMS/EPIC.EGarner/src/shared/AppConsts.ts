import { RepeatFixedDate } from "./model/policy-template.model";

export class AppConsts {
    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;
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

    static readonly folder = 'garner';
    static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";
}
export class DeliveryContractConst {

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

export class PermissionGarnerConst {

    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly GarnerModule: string = "garner.";
    //
    public static readonly GarnerPageDashboard: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}dashboard`;
    // dt = DT = Đầu tư
    public static readonly GarnerMenuSetting: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}setting`;
    // Menu Cài đặt
    // Module chủ đầu tư
    public static readonly GarnerMenuChuDT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}chu_dau_tu`;
    public static readonly GarnerChuDT_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}chu_dt_danh_sach`;
    public static readonly GarnerChuDT_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}chu_dt_them_moi`;
    public static readonly GarnerChuDT_ThongTinChuDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}chu_dt_thong_tin_chu_dau_tu`;
    public static readonly GarnerChuDT_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}chu_dt_xoa`;
    // Tab thông tin chung
    public static readonly GarnerChuDT_ThongTinChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}chu_dt_thong_tin_chung`;
    public static readonly GarnerChuDT_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}chu_dt_chi_tiet`;
    public static readonly GarnerChuDT_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}chu_dt_cap_nhat`;

    // nnl = NNL = Ngày nghỉ lễ
    // Module cấu hình ngày nghỉ lễ
    public static readonly GarnerMenuCauHinhNNL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}cau_hinh_nnl`;
    public static readonly GarnerCauHinhNNL_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}cau_hinh_nnl_danh_sach`;
    public static readonly GarnerCauHinhNNL_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}cau_hinh_nnl_cap_nhat`;

    // Module Chính sách mẫu = CSM
    public static readonly GarnerMenuChinhSachMau: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}csm`;
    public static readonly GarnerCSM_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}csm_danh_sach`;
    public static readonly GarnerCSM_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_them_moi`;
    public static readonly GarnerCSM_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_cap_nhat`;
    public static readonly GarnerCSM_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_kich_hoat_or_huy`;
    public static readonly GarnerCSM_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_xoa`;
    //
    public static readonly GarnerCSM_KyHan_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_ky_han_them_moi`;
    public static readonly GarnerCSM_KyHan_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_ky_han_cap_nhat`;
    public static readonly GarnerCSM_KyHan_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_ky_han_kich_hoat_or_huy`;
    public static readonly GarnerCSM_KyHan_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_ky_han_xoa`;

    public static readonly GarnerCSM_HopDongMau_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_hop_dong_mau_them_moi`;
    public static readonly GarnerCSM_HopDongMau_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_hop_dong_mau_cap_nhat`;
    public static readonly GarnerCSM_HopDongMau_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_hop_dong_mau_kich_hoat_or_huy`;
    public static readonly GarnerCSM_HopDongMau_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}csm_hop_dong_mau_xoa`;
    // Module cấu hình mã hợp đồng
    public static readonly GarnerMenuCauHinhMaHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}cau_hinh_ma_hd`;
    public static readonly GarnerCauHinhMaHD_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}cau_hinh_ma_hd_danh_sach`;
    public static readonly GarnerCauHinhMaHD_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}cau_hinh_ma_hd_them_moi`;
    public static readonly GarnerCauHinhMaHD_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}cau_hinh_ma_hd_cap_nhat`;
    public static readonly GarnerCauHinhMaHD_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}cau_hinh_ma_hd_xoa`;

    // Module mẫu hợp đồng
    public static readonly GarnerMenuMauHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}mau_hd`;
    public static readonly GarnerMauHD_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}mau_hd_danh_sach`;
    public static readonly GarnerMauHD_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mau_hd_them_moi`;
    public static readonly GarnerMauHD_TaiFileDoanhNghiep: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mau_hd_tai_file_doanh_nghiep`;
    public static readonly GarnerMauHD_TaiFileCaNhan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mau_hd_tai_file_ca_nhan`
    public static readonly GarnerMauHD_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mau_hd_cap_nhat`
    public static readonly GarnerMauHD_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mau_hd_xoa`;

    // Module Mô tả chung
    public static readonly GarnerMenuMoTaChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}mtc`;
    public static readonly GarnerMTC_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}mtc_danh_sach`;
    public static readonly GarnerMTC_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mtc_them_moi`;
    public static readonly GarnerMTC_ThongTinMTC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}mtc_thong_tin_mtc`;

    public static readonly GarnerMTC_ThongTinMTC_Sua: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mtc_thong_tin_mtc_sua`;
    public static readonly GarnerMTC_ThongTinMTC_NoiBat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mtc_thong_tin_mtc_noi_bat`;
    public static readonly GarnerMTC_ThongTinMTC_Xem: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mtc_thong_tin_mtc_xem`;
    public static readonly GarnerMTC_ThongTinMTC_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}mtc_thong_tin_mtc_xoa`;

        // Module Tổng thầu
    public static readonly GarnerMenuTongThau: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}tong_thau`;
    public static readonly GarnerTongThau_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}tong_thau_danh_sach`;
    public static readonly GarnerTongThau_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}tong_thau_them_moi`;
    public static readonly GarnerTongThau_ThongTinTongThau: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}tong_thau_thong_tin_tong_thau`;
    public static readonly GarnerTongThau_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}tong_thau_xoa`;
    //Tab Thông tin chung
    public static readonly GarnerTongThau_ThongTinChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}tong_thau_thong_tin_chung`;
    public static readonly GarnerTongThau_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}tong_thau_chi_tiet`;

    // Module Đại lý
    public static readonly GarnerMenuDaiLy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}dai_ly`;
    public static readonly GarnerDaiLy_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}dai_ly_danh_sach`;
    public static readonly GarnerDaiLy_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}dai_ly_them_moi`;
    public static readonly GarnerDaiLy_DoiTrangThai: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}dai_ly_doi_trang_thai`;
    public static readonly GarnerDaiLy_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}dai_ly_xoa`;
    public static readonly GarnerDaiLy_ThongTinDaiLy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}dai_ly_thong_tin_dai_ly`;
    //Tab Thông tin chung
    public static readonly GarnerDaiLy_ThongTinChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}dai_ly_thong_tin_chung`;
    public static readonly GarnerDaiLy_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}dai_ly_chi_tiet`;
    //     public static readonly GarnerDaiLy_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}dai_ly_cap_nhat`;
    //Tab TKDN = Tài khoản đăng nhập
    public static readonly GarnerDaiLy_TKDN: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}dai_ly_tkdn`;
    public static readonly GarnerDaiLy_TKDN_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}dai_ly_tkdn_danh_sach`;
    public static readonly GarnerDaiLy_TKDN_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}dai_ly_tkdn_them_moi`;
    
    //Module Hình ảnh
    public static readonly GarnerMenuHinhAnh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hinh_anh`;
    public static readonly GarnerHinhAnh_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hinh_anh_danh_sach`;
    public static readonly GarnerHinhAnh_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hinh_anh_them_moi`;
    public static readonly GarnerHinhAnh_Sua: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hinh_anh_sua`;
    public static readonly GarnerHinhAnh_DuyetDang: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hinh_anh_duyet_dang`;
    public static readonly GarnerHinhAnh_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hinh_anh_chi_tiet`;
    public static readonly GarnerHinhAnh_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hinh_anh_xoa`;
    //Module Thông báo hệ thống
    public static readonly GarnerMenuThongBaoHeThong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}thong_bao_he_thong`;
    public static readonly GarnerMenuThongBaoHeThong_CaiDat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}thong_bao_he_thong_cai_dat`;

    // HDPP = Hợp đồng phân phối
    // Menu Hợp đồng phân phối
    public static readonly GarnerMenuHDPP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp`;
    // Module Sổ lệnh
    public static readonly GarnerHDPP_SoLenh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_solenh`;
    public static readonly GarnerHDPP_SoLenh_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}hdpp_solenh_ds`;
    public static readonly GarnerHDPP_SoLenh_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_them_moi`;
    public static readonly GarnerHDPP_SoLenh_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_xoa`;

    // TT Chi tiết = TTCT
    public static readonly GarnerHDPP_SoLenh_TTCT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}hdpp_solenh_ttct`;
    //Tab TT Chung = TTC
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_ttc`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_XemChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}hdpp_solenh_ttct_ttc_xem_chi_tiet`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttc_cap_nhat`;
    // 
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_DoiKyHan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ky_han`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ma_gt`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_tt_khach_hang`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu`;

    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_ttthanhtoan`;
    //TT Thanh Toan
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_ttthanhtoan_ds`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_themtt`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_cap_nhat`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_chi_tiettt`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_phe_duyet`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_xoa`;

    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_hskh_dangky`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_solenh_ttct_hskh_dangky_ds`;
    
    //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsm`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsckdt`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_chuyen_online`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_ky_dien_tu`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_gui_thong_bao`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy`;

    public static readonly GarnerSoLenh_LoiNhuan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_loi_nhuan`;
    public static readonly GarnerSoLenh_LoiNhuan_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_solenh_ttct_loi_nhuan_ds`;
    
    public static readonly GarnerSoLenh_LichSuHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_lich_su_hd`;
    public static readonly GarnerSoLenh_LichSuHD_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_solenh_ttct_lich_su_hd_ds`;
    
    //
    public static readonly GarnerHDPP_SoLenh_TTCT_TraiTuc: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_solenh_ttct_traituc`;
    public static readonly GarnerHDPP_SoLenh_TTCT_TraiTuc_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_solenh_ttct_traituc_ds`;
    
    // HDPP -> Xử lý hợp đồng
    // XLHD: Xử lý hợp đồng
    public static readonly GarnerHDPP_XLHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_xlhd`;
    public static readonly GarnerHDPP_XLHD_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_xlhd_ds`;

    // HDPP -> Hợp đồng
    public static readonly GarnerHDPP_HopDong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_hopdong`;
    public static readonly GarnerHDPP_HopDong_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_hopdong_ds`;
    public static readonly GarnerHDPP_HopDong_YeuCauTaiTuc: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hopdong_yc_tai_tuc`;
    public static readonly GarnerHDPP_HopDong_YeuCauRutVon: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hopdong_yc_rut_von`;
    public static readonly GarnerHDPP_HopDong_PhongToaHopDong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hopdong_phong_toa_hd`;
    public static readonly GarnerHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_nhan_hd_cung`;

    // HDPP -> Giao nhận hợp đồng
    public static readonly GarnerHDPP_GiaoNhanHopDong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_giaonhanhopdong`;
    public static readonly GarnerHDPP_GiaoNhanHopDong_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_giaonhanhopdong_ds`;
    public static readonly GarnerHDPP_GiaoNhanHopDong_DoiTrangThai: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_giaonhanhopdong_doitrangthai`;
    public static readonly GarnerHDPP_GiaoNhanHopDong_XuatHopDong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_giaonhanhopdong_xuat_hd`;

    public static readonly GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}hdpp_giaonhanhopdong_ttct`;
    // // Thông tin chung
    // public static readonly GarnerHDPP_GiaoNhanHopDong_TTC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}hdpp_giaonhanhopdong_ttc`;

    // Module phong toả, giải toả
    // HD = Hợp đồng
    public static readonly GarnerHopDong_PhongToaGiaiToa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hop_dong_phong_toa_giai_toa`;
    public static readonly GarnerHopDong_PhongToaGiaiToa_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hop_dong_phong_toa_giai_toa_danh_sach`; 
    public static readonly GarnerHopDong_PhongToaGiaiToa_GiaiToaHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hop_dong_phong_toa_giai_toa_giai_toa_HD`; 
    public static readonly GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}hop_dong_phong_toa_giai_toa_thong_tin`; 

    // HDDH: HỢP ĐỒNG ĐÁO HẠN
    // public static readonly GarnerHDPP_HDDH: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_hddh`;
    // public static readonly GarnerHDPP_HDDH_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_hddh_ds`;
    // public static readonly GarnerHDPP_HDDH_ThongTinDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hddh_thong_tin_dau_tu`;
    // public static readonly GarnerHDPP_HDDH_LapDSChiTra: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hddh_lap_ds_chi_tra`;
    // public static readonly GarnerHDPP_HDDH_DuyetKhongChi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_hddh_duyet_khong_chi`;

    // HDPP: XỬ LÝ RÚT TIỀN
    public static readonly GarnerHDPP_XLRutTien: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_xlruttien`;
    public static readonly GarnerHDPP_XLRutTien_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_xlruttien_ds`;
    public static readonly GarnerHDPP_XLRutTien_ThongTinChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlruttien_thong_tin_chi_tiet`;
    public static readonly GarnerHDPP_XLRutTien_ChiTienTD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlruttien_chi_tien_td`;
    public static readonly GarnerHDPP_XLRutTien_ChiTienTC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlruttien_chi_tien_tc`;
    public static readonly GarnerHDPP_XLRutTien_HuyYeuCau: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_xlruttien_huy_yeu_cau`;

    // Chi tra loi tuc
    public static readonly GarnerHDPP_CTLC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_ctlc`;
    public static readonly GarnerHDPP_CTLC_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_ctlc_ds`;
    public static readonly GarnerHDPP_CTLC_ThongTinChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_ctlc_thong_tin_chi_tiet`;
    public static readonly GarnerHDPP_CTLC_DuyetChiTD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_ctlc_duyet_chi_td`;
    public static readonly GarnerHDPP_CTLC_DuyetChiTC: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_ctlc_duyet_chi_tc`;
    public static readonly GarnerHDPP_CTLC_LapDSChiTra: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_ctlc_lap_ds_chi_tra`;
    // LỊCH SỬ TÍCH LŨY
    public static readonly GarnerHDPP_LSTL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}hdpp_lstl`;
    public static readonly GarnerHDPP_LSTL_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}hdpp_lstl_danh_sach`;
    public static readonly GarnerHDPP_LSTL_ThongTinChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}hdpp_lstl_thong_tin_chi_tiet`;


    //==================================================
          
    // Menu Quản lý đầu tư
    // qldt = Quản lý đầu tư
    public static readonly GarnerMenuQLDT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}qldt`;
    // Module Sản phẩm đầu tư
    // SPDT = Sản phẩm đầu tư
    public static readonly GarnerMenuSPTL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}spdt`;  
    public static readonly GarnerSPTL_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}spdt_danh_sach`; 
    public static readonly GarnerSPTL_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_them_moi`;  
    public static readonly GarnerSPTL_TrinhDuyet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_trinh_duyet`;  
    public static readonly GarnerSPTL_DongOrMo: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_dong_or_mo`;  
    public static readonly GarnerSPTL_EpicXacMinh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_epic_xac_minh`;  
    public static readonly GarnerSPTL_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_xoa`;  

    public static readonly GarnerSPTL_ThongTinSPTL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}spdt_thong_tin_spdt`;

    // Tab Thông tin chung
    public static readonly GarnerSPTL_ThongTinChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}spdt_thong_tin_chung`;
    public static readonly GarnerSPTL_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}spdt_chi_tiet`;
    public static readonly GarnerSPTL_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_cap_nhat`;

    // Tab đại lý phân phối
    public static readonly GarnerSPTL_DLPP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}spdt_dlpp`;
    public static readonly GarnerSPTL_DLPP_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}spdt_dlpp_danh_sach`;
    public static readonly GarnerSPTL_DLPP_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_dlpp_them_moi`;
    public static readonly GarnerSPTL_DLPP_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_dlpp_CapNhat`;
    // public static readonly GarnerSPTL_DLPP_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_dlpp_phe_duyet_or_huy`;
    public static readonly GarnerSPTL_DLPP_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_dlpp_xoa`;
    
    // Tab tai san dam bao
    public static readonly GarnerSPDT_TSDB: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}spdt_tsdb`;
    public static readonly GarnerSPDT_TSDB_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}spdt_tsdb_danh_sach`;
    public static readonly GarnerSPDT_TSDB_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_tsdb_them_moi`;
    public static readonly GarnerSPDT_TSDB_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_tsdb_cap_nhat`;
    public static readonly GarnerSPDT_TSDB_Preview: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_tsdb_xem_file`;
    public static readonly GarnerSPDT_TSDB_DownloadFile: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_tsdb_download_file`;
    public static readonly GarnerSPDT_TSDB_DeleteFile: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_tsdb_xoa_file`;
    
    // Tab Hồ sơ pháp lý
    public static readonly GarnerSPDT_HSPL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}spdt_hspl`;
    public static readonly GarnerSPDT_HSPL_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}spdt_hspl_danh_sach`;
    public static readonly GarnerSPDT_HSPL_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_hspl_them_moi`;
    public static readonly GarnerSPDT_HSPL_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_hspl_cap_nhat`;
    public static readonly GarnerSPDT_HSPL_Preview: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_hspl_xem_file`;
    public static readonly GarnerSPDT_HSPL_DownloadFile: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_hspl_download_file`;
    public static readonly GarnerSPDT_HSPL_DeleteFile: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_hspl_xoa_file`;
    // Tab Tin tức sản phẩm
    // TTSP = Tin tức sản phẩm
    public static readonly GarnerSPDT_TTSP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}spdt_ttsp`;
    public static readonly GarnerSPDT_TTSP_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}spdt_ttsp_danh_sach`;
    public static readonly GarnerSPDT_TTSP_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_ttsp_them_moi`;
    public static readonly GarnerSPDT_TTSP_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_ttsp_CapNhat`;
    public static readonly GarnerSPDT_TTSP_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_ttsp_phe_duyet_or_huy`;
    public static readonly GarnerSPDT_TTSP_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}spdt_ttsp_xoa`;
    
    // Module Phân phối đầu tư
    // PPDT = phân phối đầu tư
    public static readonly GarnerMenuPPSP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}ppdt`;  
    public static readonly GarnerPPSP_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_danh_sach`; 
    public static readonly GarnerPPSP_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_them_moi`;  
    public static readonly GarnerPPSP_DongOrMo: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_dong_or_mo`;  
    public static readonly GarnerPPSP_TrinhDuyet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_trinh_duyet`;  
    public static readonly GarnerPPSP_BatTatShowApp: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_tat_show_app`;
    public static readonly GarnerPPSP_EpicXacMinh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_epic_xac_minh`;
    public static readonly GarnerPPSP_ThongTinPPSP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}ppdt_thong_tin_ppdt`;
    // Tab Thông tin chung
    public static readonly GarnerPPSP_ThongTinChung: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_thong_tin_chung`;
    public static readonly GarnerPPSP_ChiTiet: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Form}ppdt_chi_tiet`;
    public static readonly GarnerPPSP_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_cap_nhat`;
    //Tab Tổng quan
    public static readonly GarnerPPSP_TongQuan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_tong_quan`;
    public static readonly GarnerPPSP_TongQuan_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_tong_quan_cap_nhat`;
    public static readonly GarnerPPSP_TongQuan_ChonAnh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_tong_quan_chon_anh`;
    public static readonly GarnerPPSP_TongQuan_ThemToChuc: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_tong_quan_them_to_chuc`;
    public static readonly GarnerPPSP_TongQuan_DanhSach_ToChuc: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_tong_quan_danh_sach_to_chuc`;
    public static readonly GarnerPPSP_TongQuan_UploadFile: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_tong_quan_upload_file`;
    public static readonly GarnerPPSP_TongQuan_DanhSach_File: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_tong_quan_danh_sach_file`;
    
    // Tab Chính sách
    // Chính sách
    public static readonly GarnerPPSP_ChinhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_chinh_sach`;
    public static readonly GarnerPPSP_ChinhSach_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_chinh_sach_danh_sach`;
    public static readonly GarnerPPSP_ChinhSach_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_chinh_sach_them_moi`;
    public static readonly GarnerPPSP_ChinhSach_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_chinh_sach_CapNhat`;
    public static readonly GarnerPPSP_ChinhSach_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_chinh_sach_kich_hoat_or_huy`;
    public static readonly GarnerPPSP_ChinhSach_BatTatShowApp: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_chinh_sach_bat_tat_show_app`;
    public static readonly GarnerPPSP_ChinhSach_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_chinh_sach_xoa`;
    
    public static readonly GarnerPPSP_KyHan_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_ky_han_them_moi`;
    public static readonly GarnerPPSP_KyHan_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_ky_han_cap_nhat`;
    public static readonly GarnerPPSP_KyHan_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_ky_han_kich_hoat_or_huy`;
    public static readonly GarnerPPSP_KyHan_BatTatShowApp: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_ky_han_bat_tat_show_app`;
    public static readonly GarnerPPSP_KyHan_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_ky_han_xoa`;

    public static readonly GarnerPPSP_HopDongMau_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_mau_them_moi`;
    public static readonly GarnerPPSP_HopDongMau_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_mau_cap_nhat`;
    public static readonly GarnerPPSP_HopDongMau_KichHoatOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_mau_kich_hoat_or_huy`;
    public static readonly GarnerPPSP_HopDongMau_BatTatShowApp: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_mau_bat_tat_show_app`;
    public static readonly GarnerPPSP_HopDongMau_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_mau_xoa`;

    // Tab File chính sách
    public static readonly GarnerPPSP_FileChinhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_file_chinh_sach`;
    public static readonly GarnerPPSP_FileChinhSach_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_file_chinh_sach_danh_sach`;
    public static readonly GarnerPPSP_FileChinhSach_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_file_chinh_sach_them_moi`;
    public static readonly GarnerPPSP_FileChinhSach_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_file_chinh_sach_cap_nhat`;
    public static readonly GarnerPPSP_FileChinhSach_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_file_chinh_sach_xoa`;
    
     //tab bảng giá
     public static readonly Garner_TTCT_BangGia: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_thong_tin_chi_tiet_bg`;
     public static readonly Garner_TTCT_BangGia_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_thong_tin_chi_tiet_bg_danh_sach`;
     public static readonly Garner_TTCT_BangGia_ImportExcelBG: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_thong_tin_chi_tiet_bg_import_excel_bg`;
     public static readonly Garner_TTCT_BangGia_DownloadFileMau: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_thong_tin_chi_tiet_bg_download_file_mau`;
     public static readonly Garner_TTCT_BangGia_XoaBangGia: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_thong_tin_chi_tiet_bg_xoa_bang_gia`;
    
    // Tab Mẫu hợp đồng
    public static readonly GarnerPPSP_MauHopDong: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_mau_hop_dong`;
    public static readonly GarnerPPSP_MauHopDong_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_mau_hop_dong_danh_sach`;
    public static readonly GarnerPPSP_MauHopDong_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_hop_dong_them_moi`;
    public static readonly GarnerPPSP_MauHopDong_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_hop_dong_cap_nhat`;
    public static readonly GarnerPPSP_MauHopDong_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_hop_dong_xoa`;
    // Tab Hợp đồng phân phối
    public static readonly GarnerPPSP_HopDongPhanPhoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_hop_dong_phan_phoi`;
    public static readonly GarnerPPSP_HopDongPhanPhoi_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_hop_dong_phan_phoi_danh_sach`;
    public static readonly GarnerPPSP_HopDongPhanPhoi_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_phan_phoi_them_moi`;
    public static readonly GarnerPPSP_HopDongPhanPhoi_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_phan_phoi_cap_nhat`;
    public static readonly GarnerPPSP_HopDongPhanPhoi_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_hop_dong_phan_phoi_xoa`;
            // Tab Mẫu giao nhận HĐ
    public static readonly GarnerPPSP_MauGiaoNhanHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Tab}ppdt_mau_giao_nhan_hd`;
    public static readonly GarnerPPSP_MauGiaoNhanHD_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}ppdt_mau_giao_nhan_hd_danh_sach`;
    public static readonly GarnerPPSP_MauGiaoNhanHD_ThemMoi: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_giao_nhan_hd_them_moi`;
    public static readonly GarnerPPSP_MauGiaoNhanHD_CapNhat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_giao_nhan_hd_cap_nhat`;
    public static readonly GarnerPPSP_MauGiaoNhanHD_KichHoat: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_giao_nhan_hd_kich_hoat`;
    public static readonly GarnerPPSP_MauGiaoNhanHD_Xoa: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}ppdt_mau_giao_nhan_hd_xoa`;

    // Menu Quản lý phê duyệt
    // qlpd = Quản lý phê duyệt 
    public static readonly GarnerMenuQLPD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}qlpd`;
    // Module Phê duyệt sản phẩm đầu tư
    // PDSPDT = Phê duyệt sản phẩm đầu tư
    public static readonly GarnerMenuPDSPTL: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}pdspdt`;  
    public static readonly GarnerPDSPTL_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}pdspdt_danh_sach`;  
    public static readonly GarnerPDSPTL_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}pdspdt_phe_duyet`;  

    // Module Phê duyệt phân phối đầu tư
    // PDPPDT = Phê duyệt phân phối đầu tư
    static readonly GarnerMenuPDPPSP: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}pdppdt`;  
    static readonly GarnerPDPPSP_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}pdppdt_danh_sach`;  
    static readonly GarnerPDPPSP_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}pdppdt_phe_duyet`;  

    // Module Phê duyệt yêu cầu tái tục
    // PDYCTT = Phê duyệt yêu cầu tái tục
    static readonly GarnerMenuPDYCTT: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}pdyctt`;  
    static readonly GarnerPDYCTT_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}pdyctt_danh_sach`;  
    static readonly GarnerPDYCTT_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}pdyctt_phe_duyet`;  

    // Module Phê duyệt yêu cầu rút vốn
    // PDYCTT = Phê duyệt yêu cầu rút vốn
    static readonly GarnerMenuPDYCRV: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}pdycrv`;  
    static readonly GarnerPDYCRV_DanhSach: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Table}pdycrv_danh_sach`;  
    static readonly GarnerPDYCRV_PheDuyetOrHuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}pdycrv_phe_duyet`;
    static readonly GarnerPDYCRV_ChiTietHD: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}pdycrv_chi_tiet_hd`;  

    // Menu báo cáo
    public static readonly Garner_Menu_BaoCao: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}bao_cao`;

    public static readonly Garner_BaoCao_QuanTri: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}bao_cao_quan_tri`;
    public static readonly Garner_BaoCao_QuanTri_TCTDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_quan_tri_tct_dau_tu`;
    public static readonly Garner_BaoCao_QuanTri_THCKDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_quan_tri_thck_dau_tu`;
    public static readonly Garner_BaoCao_QuanTri_THSanPhamTichLuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_quan_tri_thsp_tich_luy`;
    public static readonly Garner_BaoCao_QuanTri_QuanTriTH: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_quan_tri_quan_tri_th`;
    public static readonly Garner_BaoCao_QuanTri_DauTuBanHo: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_quan_tri_dau_tu_ban_ho`;

    public static readonly Garner_BaoCao_VanHanh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}bao_cao_van_hanh`;
    public static readonly Garner_BaoCao_VanHanh_TCTDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_van_hanh_tct_dau_tu`;
    public static readonly Garner_BaoCao_VanHanh_THCKDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_van_hanh_thck_dau_tu`;
    public static readonly Garner_BaoCao_VanHanh_THSanPhamTichLuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_van_hanh_thsp_tich_luy`;
    public static readonly Garner_BaoCao_VanHanh_QuanTriTH: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_van_hanh_quan_tri_th`;
    public static readonly Garner_BaoCao_VanHanh_DauTuBanHo: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_van_hanh_quan_dau_tu_ban_ho`;

   
    public static readonly Garner_BaoCao_KinhDoanh: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}bao_cao_kinh_doanh`;
    public static readonly Garner_BaoCao_KinhDoanh_TCTDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_kinh_doanh_tct_dau_tu`;
    public static readonly Garner_BaoCao_KinhDoanh_THCKDauTu: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_kinh_doanh_thck_dau_tu`;
    public static readonly Garner_BaoCao_KinhDoanh_THSanPhamTichLuy: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_kinh_doanh_thsp_tich_luy`;
    public static readonly Garner_BaoCao_KinhDoanh_QuanTriTH: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_kinh_doanh_quan_tri_th`;
    public static readonly Garner_BaoCao_KinhDoanh_DauTuBanHo: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.ButtonAction}bao_cao_kinh_doanh_quan_dau_tu_ban_ho`;

    // Menu truy vấn
    public static readonly Garner_Menu_TruyVan: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Menu}truy_van`;

    public static readonly Garner_TruyVan_ThuTien_NganHang: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}truy_van_thu_tien_ngan_hang`;
    public static readonly Garner_TruyVan_ChiTien_NganHang: string = `${PermissionGarnerConst.GarnerModule}${PermissionGarnerConst.Page}truy_van_chi_tien_ngan_hang`;
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
    public static TYPE_TRADING_PROVIDERS = ['T', 'RT'];  // TRADING_PROVIDER HOẶC TRADINGROOT

    public static getUserTypeInfo(code, property) {
        let type = this.list.find(t => t.code == code);
        if (type) return type[property];
        return '';
    }

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
        },
    ];

    public static countTypeList = [
        {
            name: 'Tính từ ngày phát hành',
            code: 1,
            severity: 'help',
        },
        {
            name: 'Tính từ ngày thanh toán',
            code: 2,
            severity: 'warning',
        },
        
    ]

    public static getCountTypeName(code) {
        for (let item of this.countTypeList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusName(code, isClose) {
        if (isClose) {
            return 'Đóng';
        }
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.name;
        return '';
    }

    public static getStatusSeverity(code, isClose) {
        if (isClose) {
            return 'secondary';
        }
        let status = this.statusConst.find(status => status.code == code);
        if (status) return status.severity;
        return '';
    }
    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static fields = [
        {
            code: '1',
            name: 'Thêm mới'
        },
        {
            code: '2',
            name: 'Cập nhật'
        },
        {
            code: '3',
            name: 'Xóa'
        },
    ]

    public static getNameField(code){
        let field = this.fields.find(f => f.code == code);
        return field.name ?? "";
    }
    
}

export class InvestorAccountConst {
    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }
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

export class OrderPaymentConst {
    public static transactionTypes = [
        {
            name: 'Thu',
            code: 1,
            severity: 'success',
        },
        {
            name: 'Chi',
            code: 2,
            severity: 'danger',
        },
    ];

    //
    public static THU = 1;
    public static CHI = 2;

    public static getTransactionTypeInfo(code: number, atribution = 'name') {
        const transactionType = this.transactionTypes.find(t => t.code == code);
        return transactionType ? transactionType[atribution] : '-';
    }

    public static paymentTypes = [
        {
            name: 'Tiền mặt',
            code: 1,
        },
        {
            name: 'Chuyển khoản',
            code: 2,
        },
        {
            name: 'Tặng tích lũy',
            code: 3
        }
    ];

    public static PAYMENT_TYPE_CASH = 1;
    public static PAYMENT_TYPE_TRANSFER = 2;

    public static getPaymentTypeInfo(code: number, atribution = 'name') {
        const paymentType = this.paymentTypes.find(p => p.code == code);
        return paymentType ? paymentType[atribution] : '-';
    }

    public static PAYMENT_APPROVE = 2;
    public static PAYMENT_CANCEL = 3;

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

export class SearchConst {
    public static DEBOUNCE_TIME = 1200;
}

export class ProductPolicyConst {

    public static investorTypes = [
        {
            name: 'Chuyên nghiệp',
            code: 'P',
        },
        {
            name: 'Tất cả',
            code: 'A'
        },
    ];

    public static listClassify = [
        {
            name: 'PRO',
            code: 1,
        },
        {
            name: 'PRO A',
            code: 2,
        },
        {
            name: 'PNOTE',
            code: 3,
        }
    ];

    public static getCustomerType(code) {
        for (let item of this.investorTypes) {
            if (item.code == code) return item.name;
        }
        return '-';
    }

    public static statusList = [
        {
            name: 'Kích hoạt',
            code: 'A',
        },
        {
            name: 'Khóa',
            code: 'D',
        }
    ];

    public static getStatusName(code) {
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    // KIỂU CHÍNH SÁCH
    public static types = [
        {
            name: 'Fix ngày bán cố định',
            code: 1
        },
        {
            name: 'Ngày bán thay đổi',
            code: 2
        },
    ]

    /**
     * LẤY TÊN KIỂU CHÍNH SÁCH
     * @param code 
     * @returns TÊN KIỂU CHÍNH SÁCH
     */
    public static getTypeName(code) {
        var result = this.types.find(o => o.code === code);
        return result ? result?.name : '';
    }

    public static unitDates = [
        {
            name: 'Ngày',
            code: 'D'
        },
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Tuần',
            code: 'W'
        },
        {
            name: 'Năm',
            code: 'Y',
        }
    ];

    public static getUnitName(code) {
        for (let item of this.unitDates) {
            if (item.code == code) return item.name;
        }
        return '';
    }
    public static ACTION_CREATE = 2;
    public static ACTION_UPDATE = 1;
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
        // {
        //     name: 'Đóng',
        //     code: 4,
        //     severity: 'secondary',
        // },
    ];
    //
    public static dataType = [
        {
            name: 'Sản phẩm đầu tư',
            code: 1,
        },
        {
            name: 'Phân phối đầu tư',
            code: 2,
        },
        {
            name: 'Yêu cầu tái tục',
            code: 3,
        },
       
        {
            name: 'Bán theo kỳ hạn',
            code: 5,
        },
        
    ];
    
    public static STATUS_PRODUCT = 1;
    public static STATUS_DISTRIBUTION = 2;
    public static STATUS_REINSTATEMENT = 3;
    public static STATUS_WITHDRAWAL = 11;
    public static STATUS_DISTRI_CONTRACT_FILE = 6;

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

export class PolicyDetailTemplateConst {
    public static interestPeriodType = [
        {
            name: 'Ngày',
            code: 'D'
        },
        // {
        //     name: 'Tuần',
        //     code: 'W'
        // },
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

    public static typePolicyContractTemplate = [
        {
            name: 'Dành cho nhà đầu tư cá nhân',
            code: 'I'
        },
        {
            name: 'Dành cho nhà đầu tư doanh nghiệp',
            code: 'B'
        }
    ];

    public static displayTypePolicyContractTemplate = [
        {
            name: 'Hiện thị trước khi hợp đồng được duyệt',
            code: 'B'
        },
        {
            name: 'Hiện thị sau khi hợp đồng được duyệt',
            code: 'A'
        }
    ];

    public static contractTypePolicyContractTemplate = [
        {
            name: 'Hồ sơ đặt lệnh',
            code: 1
        },
        {
            name: 'Hồ sơ rút tiền',
            code: 2
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

    public static getNameTypePolicyContractTemplate(code) {
        let type = this.typePolicyContractTemplate.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameDisplayTypePolicyContractTemplate(code) {
        let type = this.displayTypePolicyContractTemplate.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameContractTypePolicyContractTemplate(code) {
        let type = this.contractTypePolicyContractTemplate.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

}

export class OrderConst {
    //
    public static JUST_VIEW = 'justview'
    public static VIEW_XU_LY_RUT_TIEN = 'xlrt';
    public static VIEW_GIAO_NHAN_HOP_DONG = 'gnhd';
    public static VIEW_LICH_SU_TICH_LUY = 'lstl';
    public static VIEW_CHI_TRA_LOI_TUC = 'ctlt';
    //
    public static fieldFilters = [
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
        
        // {
        //     name: 'Người đặt lệnh',
        //     code: 4,
        //     field: 'orderer',
        //     placeholder: 'Nhập người đặt lệnh...',
        // },
    ];
    
    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }
    //
    public static statusOrder = [
        {
            name: "Khởi tạo",
            code: 1,
            severity: 'help',
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ thanh toán",
            code: 2,
            severity: 'warning',
            backLink: '/trading-contract/order',
        },
        {
            name: "Chờ ký hợp đồng",
            code: 3,
            severity: "help",
            backLink: '/trading-contract/order',
        },
        {
            name: "Đã xóa",
            code: 9,
            severity: "secondary",
        },
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
            code: 4,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
        },

    ];

    public static statusActive = [
        {
            name: "Đang đầu tư",
            code: 5,
            severity: "help",
            backLink: '/trading-contract/contract-processing',
        },
        {
            name: "Phong tỏa",
            code: 6,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
        },

    ];

    public static statusActiveSettlement = [
        {
            name: "Đang đầu tư",
            code: 5,
            severity: "help",
            backLink: '/trading-contract/contract-processing',
        },
        {
            name: "Tất toán",
            code: 8,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
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
    
    public static SOURCE_ONLINE = 1;
    public static SOURCE_OFFLINE = 2;

    public static getInfoSource(code, field) {
        const source = this.sources.find(type => type.code == code);
        return source ? source[field] : null;
    }

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

    public static getNameInterestType(code) {
        let type = this.interestTypes.find(type => type.code == code);
        return type ? type.name : '';
    }

    public static KHOI_TAO = 1;
    public static CHO_THANH_TOAN = 2;
    public static CHO_KY_HOP_DONG = 3;
    public static CHO_DUYET_HOP_DONG = 4;
    public static DANG_DAU_TU = 5;
    public static PHONG_TOA = 6;
    public static GIAI_TOA = 7;
    public static TAT_TOAN = 8;
    public static XOA = 9;

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
            name: "Đang đầu tư",
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
            name: "Giải tỏa",
            code: this.GIAI_TOA,
            severity: 'success',
            backLink: '/trading-contract/contract-blockage',
        },

        {
            name: "Tất toán",
            code: this.TAT_TOAN,
            severity: 'secondary',
            backLink: '/trading-contract/contract-active',
        },

        {
            name: "Đã xóa",
            code: this.XOA,
            severity: 'secondary',
            backLink: '/trading-contract/contract-active',
        },
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

        // LOẠI SỔ LỆNH
        public static ORDER_TYPES = [
            {
                name: 'Chi tiết sổ lệnh',
                code: 1,
            },
            {
                name: 'Chi tiết sổ lệnh',
                code: 2,
            },            {
                name: 'Chi tiết sổ lệnh',
                code: 3,
            },            {
                name: 'Chi tiết sổ lệnh',
                code: 4,
            },            {
                name: 'Chi tiết hợp đồng',
                code: 5,
            },
            {
                name: 'Chi tiết sổ lệnh',
                code: 6,
            },
            {
                name: 'Chi tiết sổ lệnh',
                code: 7,
            },
            {
                name: 'Chi tiết sổ lệnh',
                code: 8,
            },
            {
                name: 'Chi tiết sổ lệnh',
                code: 9,
            },
        ];

        public static getNameOrderStatus(code) {
            let type = this.ORDER_TYPES.find(type => type.code == code);
            if (type) return type.name;
            return '';
        }
}

export class InterestPaymentConst {
    public static statusInterest = [
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
export class StatusPaymentBankConst {

    public static INTEREST_CONTRACT = 'INTEREST_CONTRACT';
    public static RENEWAL_CONTRACT = 'RENEWAL_CONTRACT';
    public static MANAGER_WITHDRAW = 'MANAGER_WITHDRAW';

    public static PENDING = 1;
    public static APPROVE_ONLINE = 2;
    public static CANCEL = 3;
    public static APPROVE_OFFLINE = 4;
    public static PENDING_ONLINE = 5;
    public static END = [this.APPROVE_ONLINE, this.APPROVE_OFFLINE, this.CANCEL];
    public static SUCCESS = [this.APPROVE_ONLINE, this.APPROVE_OFFLINE];
    public static PENDINGS = [this.APPROVE_ONLINE, this.PENDING];

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
        {
            name: 'Chờ phản hồi',
            code: this.PENDING_ONLINE,
            severity: 'help',
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

export class MediaConst {

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
            name: 'Địa chỉ liên hệ',
            code: 'CONTACT_ADDRESS_ID',
        },
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

//  TẠO CÁC HẰNG SỐ MỚI CHO GARNER TỪ ĐÂY. CÁI NÀO BÊN TRÊN CÓ RỒI BÊ XUỐNG, DỮ LIỆU THỪA BỎ ĐI

export class PolicyTempConst {
    //
    public static garnerType = [
        {
            name: 'Không chọn kỳ hạn',
            code: 1,
        },
        // {
        //     name: 'Chọn kỳ hạn',
        //     code: 2,
        // },
        
    ];

    public static GARNERTYPE_KC_KY_HAN = 1;
    public static GARNERTYPE_C_KY_HAN = 2;

    public static withdrawFeeType = [
        {
            name: 'Theo năm',
            code: 2,
        },
        {
            name: 'Số tiền',
            code: 1,
        },
    ];

    public static investorType = [
        {
            name: 'Tất cả',
            code: 'A',
        },
        {
            name: 'Chuyên nghiệp',
            code: 'P',
        },
    ];

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
    //
    public static interestType = [
        {
            name: 'Cuối kỳ',
            code: 2,
        },
        {
            name: 'Định kỳ',
            code: 1,
        },
        {
            name: 'Ngày cố định',
            code: 3,
        },
        {
            name: 'Ngày đầu tiên của tháng',
            code: 4,
        },
        {
            name: 'Ngày cuối cùng của tháng',
            code: 5,
        },
    ];
    //
    public static INTEREST_TYPE_DINH_KY = 1;
    public static INTEREST_TYPE_CUOI_KY = 2;
    public static INTEREST_TYPE_NGAY_CO_DINH = 3;
    public static INTEREST_TYPE_NGAY_DAU_THANG = 4;
    public static INTEREST_TYPE_NGAY_CUOI_THANG = 5;

    public static orderOfWithdrawal = [
        {
            name: 'Mới nhất - Cũ nhất',
            code: 1,
        },
        {
            name: 'Cũ nhất - Mới nhất',
            code: 2,
        },
        {
            name: 'HĐ có giá trị gần nhất giá trị rút',
            code: 3,
        },
    ];
    //
    public static classify = [
       
        {
            name: 'Hợp tác',
            code: 1,
        },
        {
            name: 'Mua bán',
            code: 2,
        },
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

    public static sortOrder = [
        {
            name: 'Kỳ hạn ngắn - Kỳ hạn dài',
            code: 1,
        },
        {
            name: 'Kỳ hạn dài - Kỳ hạn ngắn',
            code: 2,
        }
    ];
    

    // Danh sách ngày chi trả cố định
   public static getListRepeatFixedDate(): RepeatFixedDate[] {
        let listRepeatFixedDate = [];
        for (let i = 1; i < 29; i++) {
            listRepeatFixedDate.push({
              name: `Ngày ${i} hàng tháng`,
              code: i,
            });
        }
        return listRepeatFixedDate;
    }

    public static REPEAT_FIXED_DATE_DEFAULT = 1;

    public static TYPE_CHI_TRA_CO_DINH_THEO_NGAY = 4;
    //
    public static CLASSIFY_FLEX = 1;
    public static CLASSIFY_FLASH = 2;
    public static CLASSIFY_FIX = 3;

    //
    public static POLICY_DETAIL_TAB = 0;
    public static CONTRACT_TAB = 1;

    public static getsortOrderName(code) {
        let type = this.sortOrder.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameClassify(code) {
        let type = this.classify.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
    //
    public static getInterestType(code, atribution = 'name') {
        let type = this.interestType.find(type => type.code == code);
        return type ? type[atribution] : null;
    }

    public static getInvestorType(code, atribution = 'name') {
        let type = this.investorType.find(type => type.code == code);
        return type ? type[atribution] : null;
    }
}

export class ProductConst {

    public static SHARE = 1;        // CỔ PHẦN
    // public static GARNER = 1;    // CỔ PHIẾU 
    public static BOND = 3;         // TRÁI PHIẾU
    public static INVEST = 4;       // BĐS

    public static types = [
        {
            name:'Cổ phần',
            code: this.SHARE,
        },
        {
            name:'Cổ phiếu',
            code: this.BOND
        },
        {
            name:'Bất động sản',
            code: this.INVEST
        }
    ];

    public static getTypeName(code, atribution = 'name') {
        let type = this.types.find(t => t.code == code);
        return type ? type[atribution] : null;
    }

    //
    public static actionList = [
        {
            name: "Thêm mới",
            code: 1,
            severity: 'help',
        },
        {
            name: "Cập nhật",
            code: 2,
            severity: 'success'
        },
        {
            name: "Xóa",
            code: 3,
            severity: 'cancel'
        },
    ];

    public static getActionInfo(code, atribution= 'name') {
        let action = this.actionList.find(a => a.code == code);
        return action ? action[atribution] : null;
    }

    // KIỂU TRẢ CỔ TỨC
    public static DINH_KY = 1;
    public static CUOI_KY = 2;

    public static cpsInterestRateTypes = [
        {
            name: "Định kỳ",
            code: 1,
            severity: 'help',
        },
        {
            name: "Cuối kỳ",
            code: 2,
            severity: 'success'
        },
    ];

    public static getCpsInterestRateType(code, atribution = 'name') {
        let type = this.cpsInterestRateTypes.find(t => t.code == code);
        return type ? type[atribution] : null;
    }

    // HÌNH THỨC TRẢ CỔ TỨC
    public static countTypes = [
        {
            name: "Tính từ ngày phát hành",
            code: 1,
            severity: 'help',
        },
        {
            name: "Tính từ ngày thanh toán",
            code: 2,
            severity: 'success'
        },
    ];

    public static getCountType(code, atribution = 'name') {
        let type = this.countTypes.find(t => t.code == code);
        return type ? type[atribution] : null;
    }

    public static  investTypes = [
        {
            name: "Nhà riêng",
            code: 1,
        },
        {
            name: "Căn hộ chung cư",
            code: 2,
        },
        {
            name: "Nhà phố, biệt thự dự án",
            code: 3,
        },
        {
            name: "Đất nền dự án",
            code: 4,
        },
        {
            name: "Biệt thự nghỉ dưỡng",
            code: 5,
        },
        {
            name: "Condotel",
            code: 6,
        },
        {
            name: "Shophouse",
            code: 7,
        },
        {
            name: "Officetel",
            code: 8,
        },
    ];

    public static getInvestTypeNames(data = []) {
        let typeNames: string = '';
        for(let code of data) {
            let type = this.investTypes.find(t => t.code == code);
            if(type) typeNames += type.name + ', ';
        }

        typeNames = typeNames.substring(0, typeNames.length - 2); // Xóa dấu "," thừa ở cuối câu

        return typeNames;
    }


}

export class YesNoConst {
    public static list = [
        {
            name: "Có",
            code: "Y",
            severity: 'help',
        },
        {
            name: "Không",
            code: "N",
            severity: 'success',
        },
    ]

    public static getName(code, atribution ='name') {
        let item = this.list.find(yn => yn.code == code);
        return item ? item[atribution] : null;
    }

    public static YES = 'Y';
    public static NO = 'N';
}

export class ActiveDeactiveConst {

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';

    public static list = [
        {
            name: 'Kích hoạt',
            code: this.ACTIVE,
            severity: 'success',

        },
        {
            name: 'Khóa',
            code: this.DEACTIVE,
            severity: 'secondary',
                
        }
    ];

    public static getInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class UnitDateConst {
    public static list = [
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Ngày',
            code: 'D',
        },
        {
            name: 'Năm',
            code: 'Y',
        },
        
    ];

    public static getNameUnitDate(code, atribution ='name') {
        let unit = this.list.find(item => item.code == code);
        return unit ? unit[atribution] : null;
    }
}

export class TradingProviderConst {

    public static THEM_MOI = "Thêm mới phân phối";
    public static CAP_NHAT = "Cập nhật phân phối";
    public static XOA = "Xóa phân phối";
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

export class StatusApprove {

    public static KHOI_TAO = 1;
    public static TRINH_DUYET = 2;
    public static HOAT_DONG = 3;
    public static HUY_DUYET = 4;
    public static DONG = 5;

    public static list = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.KHOI_TAO,
        },
        {
            name: 'Trình duyệt',
            code: this.TRINH_DUYET,
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

    public static getStatusInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class OverviewConst {

    public static KHOI_TAO = 1;
    public static TRINH_DUYET = 2;
    public static HOAT_DONG = 3;
    public static HUY_DUYET = 4;
    public static DONG = 5;

    public static list = [
        {
            name: 'Thông tin sản phẩm',
            severity: 'help',
            code: 1,
        },
        {
            name: 'Chính sách sản phẩm',
            code: 2,
            severity: 'warning',
        },
        {
            name: 'Hồ sơ pháp lý',
            code: 3,
            severity: 'success',
        },
       
    ];

    public static getNameType(code) {
        let type = this.list.find(t => t.code == code);
        return type ? type.name : null;
    }

    public static getStatusInfo(code, atribution = 'name') {
        let status = this.list.find(s => s.code == code);
        return status ? status[atribution] : null;
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

export class CalendarConst {
    public static HEADER = [
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': 0,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': 1,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': 2,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': 3,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': 4,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': 5,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': 6,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
    ];
}

export class WithdrawalConst {

    public static REQUEST = 1;
    public static APPROVE = 2;
    public static CANCEL = 3;

    public static List = [
        {
            name: 'Yêu cầu',
            code: this.REQUEST,
            severity: 'pending',
        },
        {
            name: 'Phê duyệt',
            code: this.APPROVE,
            severity: 'pending',
        },
        {
            name: 'Hủy',
            code: this.REQUEST,
            severity: 'danger',
        }
    ];

    public static getInfo(code, atribution) {
        const status = this.List.find(s => s.code == code);
        return status ? status[atribution] : null;
    }
}

export class MessageErrorConst {

    public static message = {
        Error: "ACTIVE",
        Validate: "Vui lòng nhập đủ thông tin cho các trường có dấu (*)",
        DataNotFound: "Không tìm thấy dữ liệu!"
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
            name: 'Ngày đặt lệnh',
            code: 'BUY_DATE',
           
        },
        {
            name: 'Ngày thanh toán đủ',
            code: 'PAYMENT_FULL_DATE',
            
        },
        {
            name: 'Tên sản phẩm',
            code: 'PRODUCT_NAME',
            
        },
        {
            name: 'Mã sản phẩm',
            code: 'PRODUCT_CODE',
            
        },
        {
            name: 'Tên chính sách',
            code: 'POLICY_NAME',
            
        },
        {
            name: 'Mã chính sách',
            code: 'POLICY_CODE',
            
        },
        {
            name: 'Tên viết tắt nhà đầu tư',
            code: 'SHORT_NAME',
            
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

    //     Ký tự : FIX_TEXT
    // - Ngày thanh toán đủ: PAYMENT_FULL_DATE
    // - Tên viết tắt chính sách: PRODUCT_TYPE
    // - Tên sản phẩm: PRODUCT_NAME
    // - Mã sản phẩm: PRODUCT_CODE
    // - Tên chính sách : POLICY_NAME
    // - Mã chính sách : POLICY_CODE
    // - Tên viết tắt nhà đầu tư ( lấy các chữ cái đầu): SHORT_NAME

    public static DataSeeder = {
        'FIX_TEXT': '/',
        'PAYMENT_FULL_DATE': '02122022',
        'PRODUCT_TYPE': 'PLUS',
        'PRODUCT_NAME': 'TICH_LUY',
        'PRODUCT_CODE': 'TL003',
        'POLICY_NAME': 'DAUTUDAIHAN',
        'POLICY_CODE': 'DAUTU',
        'SHORT_NAME': 'NTT',
    }


}

export class CollectMoneyBankConst {

public static statusRequestConst = [
        {
            name: 'Chờ',
            code: '0',
            severity: 'warning',
        },
        {
            name: 'Thành công',
            code: '1',
            severity: 'success',
        },
        {
            name: 'Thất bại',
            code: '2',
            severity: 'danger',
        },
    ];

    public static getStatusRequest(code, atribution = 'name') {
        const status = this.statusRequestConst.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

public static paymentType = [
        {
            name: 'Chi trả lợi tức',
            code: 3,
            severity: 'warning',
        },
        {
            name: 'Rút tiền',
            code: 4,
            severity: 'success',
        },
    ];
    public static getTypes(code, atribution = 'name') {
        const status = this.paymentType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

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

export class GeneralDescriptionConst {
    public static FILE = 'FILE';
    public static LINK = 'LINK';
    public static types = [
        {
            name: 'File',
            code: this.FILE
        },
        {
            name: 'Đường dẫn',
            code: this.LINK
        }
    ];

    public static mediaStatus = {
        ACTIVE: 'Đã đăng',
        PENDING: 'Trình duyệt',
        DELETED: 'Đã xoá',
        DRAFT: 'Bản nháp'
    }
    public static statusSeverity = {
        ACTIVE: 'success',
        PENDING: 'warning',
        DELETED: 'danger',
        DRAFT: 'secondary'
    }

    public static positionList = [
        {
            name: "Banner",
            code: 'banner'
        },
        {
            name: "Hình ảnh nổi bật",
            code: 'hinh_anh_noi_bat'
        },
        {
            name: "Slide ảnh",
            code: 'slider'
        },
        
    ];

    public static getPositionName(code) {
        for (let item of this.positionList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

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
            severity: 'dark'
        },
        {
            name: "Bản nháp",
            code: 'DRAFT',
            severity: 'secondary'
        }
    ];

    public static getStatusNews(code) {
        return this.mediaStatus[code];
    }

    public static getStatusSeverity(code) {
        return this.statusSeverity[code]
    }

    public static NHAP = 'DRAFT';
    public static TRINH_DUYET = 'PENDING';
    public static ACTIVE = 'ACTIVE';
    public static XOA = 'DELETED'
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

export class SampleContractConst {

    public static contractType = [
        {
            name: 'Hồ sơ đặt lệnh',
            code: 1,
           
        },
        {
            name: 'Hồ sơ rút tiền',
            code: 2,
            
        },
    ];
    public static getContractType(code, atribution = 'name') {
        let status = this.contractType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

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
            name: 'Tất cả',
            code: '',
           
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
            severity: 'help',
        },
        {
            name: 'Bị trùng',
            code: 3,
            severity: 'warning',
        },
        
    ];

    public static valueList = [
        {
            name: 'Ký tự',
            code: 'FIX_TEXT',
           
        },
        {
            name: 'Ngày thanh toán đủ',
            code: 'PaymentFullDate',
            
        },
        {
            name: 'Tên chính sách sản phẩm',
            code: 'PolicyName',
            
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

export class OrderActiveGroupConst {
    //
    public static fieldFilters = [
        {
            name: 'Số điện thoại',
            code: 1,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
        
        {
            name: 'Số giấy tờ',
            code: 2,
            field: 'idNo',
            placeholder: 'Nhập số giấy tờ...',
        },
        {
            name: 'Mã số thuế',
            code: 3,
            field: 'taxCode',
            placeholder: 'Nhập mã số thuế...',
        },

    ];
    
    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }
}

export class WithdrawConst {

    public static fieldSearch = [
        {
            name: 'Mã hợp đồng',
            code: 1,
            field: 'contractCode',
            placeholder: 'Nhập mã hợp đồng...',
        },
        
        {
            name: 'Số điện thoại',
            code: 2,
            field: 'phone',
            placeholder: 'Nhập số điện thoại...',
        },
    ];

    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldSearch.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }

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

export class ProductFileConst {
    public static TAI_SAN_DAM_BAO = 1;
    public static HO_SO_PHAP_lY = 2;
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

export class ExportReportConst {
    public static BC_TCT_DAU_TU = 1;
    public static BC_TH_KHOAN_DAU_TU = 2;
    public static BC_SP_TICH_LUY = 3;
    public static BC_QUAN_TRI_TH = 4;
    public static BC_DAU_TU_BAN_HO = 5;

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

export class DashboardConst {
    // 1: Đặt lệnh mới; 2: Rút tiền; 3: Yêu cầu nhận hợp đồng
    public static DAT_LENH_MOI = 1;
    public static RUT_TIEN = 2;
    public static YEU_CAU_NHAN_HOP_DONG = 3;
    public static listAction = [
        {
            name: 'Đặt lệnh mới',
            code: this.DAT_LENH_MOI,
        },
        {
            name: 'Rút tiền',
            code: this.RUT_TIEN
        },
        {
            name: 'Yêu cầu nhận hợp đồng',
            code: this.YEU_CAU_NHAN_HOP_DONG
        }
    ];

    public static getActionInfo(code) {
        let type = this.listAction.find(t => t.code == code);
        return type?.name ?? '';
    }
}

export const TRADING_CONTRACT_ROUTER = 'trading-contract';

export class QueryMoneyBankCost{
    public static GARNER = 1;
    public static INVEST = 2;
    public static COMPANY_SHARE = 3;
    public static REAL_ESTATE = 4;
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