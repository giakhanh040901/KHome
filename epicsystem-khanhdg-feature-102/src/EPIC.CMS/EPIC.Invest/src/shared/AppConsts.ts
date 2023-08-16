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

    static readonly folder = 'invest';

    static readonly imageDefault = 'assets/layout/images/no-image.png';
    static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";
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

    public static getStatus(code, atribution = null) {
        const status = this.deliveryStatus.find(p => p.code == code);
        return atribution && status ? status[atribution] : status;
    }

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

export class PermissionInvestConst {

    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly InvestModule: string = "invest.";
    
    //
    public static readonly InvestPageDashboard: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}dashboard`;
    
    // dt = DT = Đầu tư
    public static readonly InvestMenuSetting: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}setting`;
    
    // Menu Cài đặt
    // Module chủ đầu tư
    public static readonly InvestMenuChuDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}chu_dau_tu`;
    public static readonly InvestChuDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}chu_dt_danh_sach`;
    public static readonly InvestChuDT_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}chu_dt_them_moi`;
    public static readonly InvestChuDT_ThongTinChuDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}chu_dt_thong_tin_chu_dau_tu`;
    public static readonly InvestChuDT_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}chu_dt_xoa`;
            
    // Tab thông tin chung
    public static readonly InvestChuDT_ThongTinChung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}chu_dt_thong_tin_chung`;
    public static readonly InvestChuDT_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}chu_dt_chi_tiet`;
    public static readonly InvestChuDT_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}chu_dt_cap_nhat`;

    // nnl = NNL = Ngày nghỉ lễ
    // Module cấu hình ngày nghỉ lễ
    public static readonly InvestMenuCauHinhNNL: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}cau_hinh_nnl`;
    public static readonly InvestCauHinhNNL_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}cau_hinh_nnl_danh_sach`;
    public static readonly InvestCauHinhNNL_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}cau_hinh_nnl_cap_nhat`;

    // Module Chính sách mẫu = CSM
    public static readonly InvestMenuChinhSachMau: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}csm`;
    public static readonly InvestCSM_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}csm_danh_sach`;
    public static readonly InvestCSM_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_them_moi`;
    public static readonly InvestCSM_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_cap_nhat`;
    public static readonly InvestCSM_KichHoatOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_kich_hoat_or_huy`;
    public static readonly InvestCSM_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_xoa`;
    
    //
    public static readonly InvestCSM_KyHan_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_ky_han_them_moi`;
    public static readonly InvestCSM_KyHan_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_ky_han_cap_nhat`;
    public static readonly InvestCSM_KyHan_KichHoatOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_ky_han_kich_hoat_or_huy`;
    public static readonly InvestCSM_KyHan_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}csm_ky_han_xoa`;

    // Module cấu hình mã hợp đồng
    public static readonly InvestMenuCauHinhMaHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}cau_hinh_ma_hd`;
    public static readonly InvestCauHinhMaHD_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}cau_hinh_ma_hd_danh_sach`;
    public static readonly InvestCauHinhMaHD_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}cau_hinh_ma_hd_them_moi`;
    public static readonly InvestCauHinhMaHD_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}cau_hinh_ma_hd_cap_nhat`;
    public static readonly InvestCauHinhMaHD_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}cau_hinh_ma_hd_xoa`;

    // Module mẫu hợp đồng
    public static readonly InvestMenuMauHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}mau_hd`;
    public static readonly InvestMauHD_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}mau_hd_danh_sach`;
    public static readonly InvestMauHD_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}mau_hd_them_moi`;
    public static readonly InvestMauHD_TaiFileDoanhNghiep: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}mau_hd_tai_file_doanh_nghiep`;
    public static readonly InvestMauHD_TaiFileCaNhan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}mau_hd_tai_file_ca_nhan`
    public static readonly InvestMauHD_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}mau_hd_cap_nhat`
    public static readonly InvestMauHD_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}mau_hd_xoa`;

    // Module Tổng thầu
    public static readonly InvestMenuTongThau: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}tong_thau`;
    public static readonly InvestTongThau_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}tong_thau_danh_sach`;
    public static readonly InvestTongThau_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}tong_thau_them_moi`;
    public static readonly InvestTongThau_ThongTinTongThau: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}tong_thau_thong_tin_tong_thau`;
    public static readonly InvestTongThau_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}tong_thau_xoa`;
    
    //Tab Thông tin chung
    public static readonly InvestTongThau_ThongTinChung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}tong_thau_thong_tin_chung`;
    public static readonly InvestTongThau_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}tong_thau_chi_tiet`;

    // Module Đại lý
    public static readonly InvestMenuDaiLy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}dai_ly`;
    public static readonly InvestDaiLy_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}dai_ly_danh_sach`;
    public static readonly InvestDaiLy_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}dai_ly_them_moi`;
    public static readonly InvestDaiLy_KichHoatOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}dai_ly_kich_hoat_or_huy`;
    public static readonly InvestDaiLy_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}dai_ly_xoa`;
    public static readonly InvestDaiLy_ThongTinDaiLy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}dai_ly_thong_tin_dai_ly`;
    
    //Tab Thông tin chung
    public static readonly InvestDaiLy_ThongTinChung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}dai_ly_thong_tin_chung`;
    public static readonly InvestDaiLy_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}dai_ly_chi_tiet`;
    //     public static readonly InvestDaiLy_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}dai_ly_cap_nhat`;
    
    //Tab TKDN = Tài khoản đăng nhập
    public static readonly InvestDaiLy_TKDN: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}dai_ly_tkdn`;
    public static readonly InvestDaiLy_TKDN_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}dai_ly_tkdn_danh_sach`;
    public static readonly InvestDaiLy_TKDN_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}dai_ly_tkdn_them_moi`;
    
    //Module Hình ảnh
    public static readonly InvestMenuHinhAnh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hinh_anh`;
    public static readonly InvestHinhAnh_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hinh_anh_danh_sach`;
    public static readonly InvestHinhAnh_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hinh_anh_them_moi`;
    public static readonly InvestHinhAnh_Sua: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hinh_anh_sua`;
    public static readonly InvestHinhAnh_DuyetDang: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hinh_anh_duyet_dang`;
    public static readonly InvestHinhAnh_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hinh_anh_chi_tiet`;
    public static readonly InvestHinhAnh_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hinh_anh_xoa`;
        
    //Module Thông báo hệ thống
    public static readonly InvestMenuThongBaoHeThong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}thong_bao_he_thong`;
    public static readonly InvestMenuThongBaoHeThong_CaiDat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}thong_bao_he_thong_cai_dat`;

    
    // HDPP = Hợp đồng phân phối
    // Menu Hợp đồng phân phối
    public static readonly InvestMenuHDPP: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp`;
    
    // Module Sổ lệnh
    public static readonly InvestHDPP_SoLenh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_solenh`;
    public static readonly InvestHDPP_SoLenh_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}hdpp_solenh_ds`;
    public static readonly InvestHDPP_SoLenh_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_them_moi`;
    public static readonly InvestHDPP_SoLenh_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_xoa`;

    // TT Chi tiết = TTCT
    public static readonly InvestHDPP_SoLenh_TTCT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}hdpp_solenh_ttct`;
    
    //Tab TT Chung = TTC
    public static readonly InvestHDPP_SoLenh_TTCT_TTC: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_ttc`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTC_XemChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}hdpp_solenh_ttct_ttc_xem_chi_tiet`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTC_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttc_cap_nhat`;
    
    // 
    public static readonly InvestHDPP_SoLenh_TTCT_TTC_DoiMaGT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ma_gt`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_tt_khach_hang`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu`;

    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_ttthanhtoan`;
    
    //TT Thanh Toan
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_ttthanhtoan_ds`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_themtt`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_cap_nhat`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_chi_tiettt`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_phe_duyet`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao`;
    public static readonly InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_xoa`;

    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_hskh_dangky`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_solenh_ttct_hskh_dangky_ds`;
    
    //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsm`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsckdt`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_chuyen_online`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_ky_dien_tu`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_gui_thong_bao`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy`;

    public static readonly InvestSoLenh_LoiNhuan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_loi_nhuan`;
    public static readonly InvestSoLenh_LoiNhuan_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_solenh_ttct_loi_nhuan_ds`;
    
    public static readonly InvestSoLenh_LichSuHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_lich_su_hd`;
    public static readonly InvestSoLenh_LichSuHD_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_solenh_ttct_lich_su_hd_ds`;
    
    //
    public static readonly InvestHDPP_SoLenh_TTCT_TraiTuc: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_solenh_ttct_traituc`;
    public static readonly InvestHDPP_SoLenh_TTCT_TraiTuc_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_solenh_ttct_traituc_ds`;
    
    // HDPP -> Xử lý hợp đồng
    // XLHD: Xử lý hợp đồng
    public static readonly InvestHDPP_XLHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_xlhd`;
    public static readonly InvestHDPP_XLHD_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_xlhd_ds`;

    // HDPP -> Hợp đồng
    public static readonly InvestHDPP_HopDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_hopdong`;
    public static readonly InvestHDPP_HopDong_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_hopdong_ds`;
    public static readonly InvestHDPP_HopDong_YeuCauTaiTuc: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hopdong_yc_tai_tuc`;
    public static readonly InvestHDPP_HopDong_YeuCauRutVon: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hopdong_yc_rut_von`;
    public static readonly InvestHDPP_HopDong_PhongToaHopDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hopdong_phong_toa_hd`;
    public static readonly InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_nhan_hd_cung`;

    // HDPP -> Giao nhận hợp đồng
    public static readonly InvestHDPP_GiaoNhanHopDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_giaonhanhopdong`;
    public static readonly InvestHDPP_GiaoNhanHopDong_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_giaonhanhopdong_ds`;
    public static readonly InvestHDPP_GiaoNhanHopDong_DoiTrangThai: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_giaonhanhopdong_doitrangthai`;
    public static readonly InvestHDPP_GiaoNhanHopDong_XuatHopDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_giaonhanhopdong_xuat_hd`;

    public static readonly InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}hdpp_giaonhanhopdong_ttct`;
    
    // Thông tin chung
    public static readonly InvestHDPP_GiaoNhanHopDong_TTC: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}hdpp_giaonhanhopdong_ttc`;

    // Module phong toả, giải toả
    // HD = Hợp đồng
    public static readonly InvestHopDong_PhongToaGiaiToa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hop_dong_phong_toa_giai_toa`;
    public static readonly InvestHopDong_PhongToaGiaiToa_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hop_dong_phong_toa_giai_toa_danh_sach`; 
    public static readonly InvestHopDong_PhongToaGiaiToa_GiaiToaHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hop_dong_phong_toa_giai_toa_giai_toa_HD`; 
    public static readonly InvestHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}hop_dong_phong_toa_giai_toa_thong_tin`; 

    // XỬ LÝ RÚT TIỀN
    public static readonly InvestHDPP_XLRT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_xlrt`;
    public static readonly InvestHDPP_XLRT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_xlrt_danh_sach`;
    public static readonly InvestHDPP_XLRT_ThongTinDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlrt_thong_tin_dau_tu`;
    public static readonly InvestHDPP_XLRT_DuyetChiTuDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlrt_duyet_chi_tu_dong`;
    public static readonly InvestHDPP_XLRT_DuyetChiThuCong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlrt_duyet_chi_thu_cong`;
    public static readonly InvestHDPP_XLRT_HuyYeuCau: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_xlrt_huy_yeu_cau`;

    // HDDH: HỢP ĐỒNG ĐÁO HẠN
    public static readonly InvestHDPP_HDDH: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_hddh`;
    public static readonly InvestHDPP_HDDH_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_hddh_ds`;
    public static readonly InvestHDPP_HDDH_LapDSChiTra: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_lap_ds_chi_tra`;
    public static readonly InvestHDPP_HDDH_ThongTinDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_thong_tin_dau_tu`;
    public static readonly InvestHDPP_HDDH_DuyetChiThuCong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_duyet_chi_thu_cong`;
    public static readonly InvestHDPP_HDDH_DuyetChiTuDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_duyet_chi_tu_dong`;
    public static readonly InvestHDPP_HDDH_DuyetTaiTuc: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_duyet_tai_tuc`;
    public static readonly InvestHDPP_HDDH_ExportExcel: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hddh_export_excel`;

    // CHI TRẢ LỢI TỨC
    public static readonly InvestHDPP_CTLT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_ctlt`;
    public static readonly InvestHDPP_CTLT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_ctlt_danh_sach`;
    public static readonly InvestHDPP_CTLT_LapDSChiTra: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_ctlt_lap_ds_chi_tra`;
    public static readonly InvestHDPP_CTLT_ThongTinDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_ctlt_thong_tin_dau_tu`;
    public static readonly InvestHDPP_CTLT_DuyetChiTuDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_ctlt_duyet_chi_tu_dong`;
    public static readonly InvestHDPP_CTLT_DuyetChiThuCong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_ctlt_duyet_chi_thu_cong`;
    public static readonly InvestHDPP_CTLT_ExportExcel: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_ctlt_export_excel`;

    // LỊCH SỬ ĐẦU TƯ
    public static readonly InvestHDPP_LSDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_lsdt`;
    public static readonly InvestHDPP_LSDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_lsdt_danh_sach`;
    public static readonly InvestHDPP_LSDT_ThongTinDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_lsdt_thong_tin_dau_tu`;

    // HDPP: HỢP ĐỒNG TÁI TỤC
    public static readonly InvestHDPP_HDTT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}hdpp_hdtt`;
    public static readonly InvestHDPP_HDTT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}hdpp_hdtt_ds`;
    public static readonly InvestHDPP_HDTT_ThongTinDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hdtt_thong_tin_dau_tu`;
    public static readonly InvestHDPP_HDTT_HuyYeuCau: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}hdpp_hdtt_huy_yeu_cau`;

    //==================================================
          
    // Menu Quản lý đầu tư
    // qldt = Quản lý đầu tư
    public static readonly InvestMenuQLDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}qldt`;
        
    // Module Sản phẩm đầu tư
    // SPDT = Sản phẩm đầu tư
    public static readonly InvestMenuSPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}spdt`;  
    public static readonly InvestSPDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}spdt_danh_sach`; 
    public static readonly InvestSPDT_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_them_moi`;  
    public static readonly InvestSPDT_TrinhDuyet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_trinh_duyet`;  
    public static readonly InvestSPDT_Dong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_dong_san_pham`;  
    public static readonly InvestSPDT_EpicXacMinh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_epic_xac_minh`;  
    public static readonly InvestSPDT_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_xoa`;  

    public static readonly InvestSPDT_ThongTinSPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}spdt_thong_tin_spdt`;

    // Tab Thông tin chung
    public static readonly InvestSPDT_ThongTinChung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}spdt_thong_tin_chung`;
    public static readonly InvestSPDT_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}spdt_chi_tiet`;
    public static readonly InvestSPDT_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_cap_nhat`;
    
    // hình ảnh đầu tư
    public static readonly InvestSPDT_HADT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}spdt_hadt`;
    public static readonly InvestSPDT_HADT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}spdt_hadt_danh_sach`;
    public static readonly InvestSPDT_HADT_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hadt_them_moi`;
    public static readonly InvestSPDT_HADT_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hadt_xoa`;

    // Tab Hồ sơ pháp lý
    // HSPL = Hồ sơ pháp lý
    public static readonly InvestSPDT_HSPL: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}spdt_hspl`;
    public static readonly InvestSPDT_HSPL_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}spdt_hspl_danh_sach`;
    public static readonly InvestSPDT_HSPL_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hspl_them_moi`;
    public static readonly InvestSPDT_HSPL_Preview: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hspl_xem_file`;
    public static readonly InvestSPDT_HSPL_DownloadFile: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hspl_download_file`;
    public static readonly InvestSPDT_HSPL_DeleteFile: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_hspl_xoa_file`;
    
    // Tab Tin tức sản phẩm
    // TTSP = Tin tức sản phẩm
    public static readonly InvestSPDT_TTSP: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}spdt_ttsp`;
    public static readonly InvestSPDT_TTSP_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}spdt_ttsp_danh_sach`;
    public static readonly InvestSPDT_TTSP_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_ttsp_them_moi`;
    public static readonly InvestSPDT_TTSP_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_ttsp_CapNhat`;
    public static readonly InvestSPDT_TTSP_PheDuyetOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_ttsp_phe_duyet_or_huy`;
    public static readonly InvestSPDT_TTSP_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}spdt_ttsp_xoa`;
        
    // Module Phân phối đầu tư
    // PPDT = phân phối đầu tư
    public static readonly InvestMenuPPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}ppdt`;  
    public static readonly InvestPPDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_danh_sach`; 
    public static readonly InvestPPDT_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_them_moi`;  
    public static readonly InvestPPDT_DongTam: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_xoa`;  
    public static readonly InvestPPDT_TrinhDuyet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_trinh_duyet`;  
    public static readonly InvestPPDT_BatTatShowApp: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_tat_show_app`;
    public static readonly InvestPPDT_EpicXacMinh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_epic_xac_minh`;
    public static readonly InvestPPDT_ThongTinPPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}ppdt_thong_tin_ppdt`;
        
    // Tab Thông tin chung
    public static readonly InvestPPDT_ThongTinChung: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_thong_tin_chung`;
    public static readonly InvestPPDT_ChiTiet: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Form}ppdt_chi_tiet`;
    public static readonly InvestPPDT_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_cap_nhat`;
        
    //Tab Tổng quan
    public static readonly InvestPPDT_TongQuan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_tong_quan`;
    public static readonly InvestPPDT_TongQuan_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_tong_quan_cap_nhat`;
    public static readonly InvestPPDT_TongQuan_ChonAnh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_tong_quan_chon_anh`;
    public static readonly InvestPPDT_TongQuan_ThemToChuc: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_tong_quan_them_to_chuc`;
    public static readonly InvestPPDT_TongQuan_DanhSach_ToChuc: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_tong_quan_danh_sach_to_chuc`;
    public static readonly InvestPPDT_TongQuan_UploadFile: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_tong_quan_upload_file`;
    public static readonly InvestPPDT_TongQuan_DanhSach_File: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_tong_quan_danh_sach_file`;
    
        
    // Tab Chính sách
    // Chính sách
    public static readonly InvestPPDT_ChinhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_chinh_sach`;
    public static readonly InvestPPDT_ChinhSach_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_chinh_sach_danh_sach`;
    public static readonly InvestPPDT_ChinhSach_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_chinh_sach_them_moi`;
    public static readonly InvestPPDT_ChinhSach_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_chinh_sach_CapNhat`;
    public static readonly InvestPPDT_ChinhSach_KichHoatOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_chinh_sach_kich_hoat_or_huy`;
    public static readonly InvestPPDT_ChinhSach_BatTatShowApp: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_chinh_sach_bat_tat_show_app`;
    public static readonly InvestPPDT_ChinhSach_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_chinh_sach_xoa`;
    
    public static readonly InvestPPDT_KyHan_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_ky_han_them_moi`;
    public static readonly InvestPPDT_KyHan_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_ky_han_cap_nhat`;
    public static readonly InvestPPDT_KyHan_KichHoatOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_ky_han_kich_hoat_or_huy`;
    public static readonly InvestPPDT_KyHan_BatTatShowApp: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_ky_han_bat_tat_show_app`;
    public static readonly InvestPPDT_KyHan_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_ky_han_xoa`;

    // Tab File chính sách
    public static readonly InvestPPDT_FileChinhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_file_chinh_sach`;
    public static readonly InvestPPDT_FileChinhSach_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_file_chinh_sach_danh_sach`;
    public static readonly InvestPPDT_FileChinhSach_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_file_chinh_sach_them_moi`;
    public static readonly InvestPPDT_FileChinhSach_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_file_chinh_sach_cap_nhat`;
    public static readonly InvestPPDT_FileChinhSach_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_file_chinh_sach_xoa`;
        
    // Tab Mẫu hợp đồng
    public static readonly InvestPPDT_MauHopDong: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_mau_hop_dong`;
    public static readonly InvestPPDT_MauHopDong_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_mau_hop_dong_danh_sach`;
    public static readonly InvestPPDT_MauHopDong_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_hop_dong_them_moi`;
    public static readonly InvestPPDT_MauHopDong_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_hop_dong_cap_nhat`;
    public static readonly InvestPPDT_MauHopDong_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_hop_dong_xoa`;
        
    // Tab Hợp đồng phân phối
    public static readonly InvestPPDT_HopDongPhanPhoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_hop_dong_phan_phoi`;
    public static readonly InvestPPDT_HopDongPhanPhoi_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_hop_dong_phan_phoi_danh_sach`;
    public static readonly InvestPPDT_HopDongPhanPhoi_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_hop_dong_phan_phoi_them_moi`;
    public static readonly InvestPPDT_HopDongPhanPhoi_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_hop_dong_phan_phoi_cap_nhat`;
    public static readonly InvestPPDT_HopDongPhanPhoi_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_hop_dong_phan_phoi_xoa`;
        
    // Tab Mẫu giao nhận HĐ
    public static readonly InvestPPDT_MauGiaoNhanHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Tab}ppdt_mau_giao_nhan_hd`;
    public static readonly InvestPPDT_MauGiaoNhanHD_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}ppdt_mau_giao_nhan_hd_danh_sach`;
    public static readonly InvestPPDT_MauGiaoNhanHD_ThemMoi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_giao_nhan_hd_them_moi`;
    public static readonly InvestPPDT_MauGiaoNhanHD_CapNhat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_giao_nhan_hd_cap_nhat`;
    public static readonly InvestPPDT_MauGiaoNhanHD_KichHoat: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_giao_nhan_hd_kich_hoat`;
    public static readonly InvestPPDT_MauGiaoNhanHD_Xoa: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}ppdt_mau_giao_nhan_hd_xoa`;
    
    // Menu Quản lý phê duyệt
    // qlpd = Quản lý phê duyệt 
    public static readonly InvestMenuQLPD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}qlpd`;
    
    // Module Phê duyệt sản phẩm đầu tư
    // PDSPDT = Phê duyệt sản phẩm đầu tư
    public static readonly InvestMenuPDSPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}pdspdt`;  
    public static readonly InvestPDSPDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}pdspdt_danh_sach`;  
    public static readonly InvestPDSPDT_PheDuyetOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}pdspdt_phe_duyet`;  

    // Module Phê duyệt phân phối đầu tư
    // PDPPDT = Phê duyệt phân phối đầu tư
    static readonly InvestMenuPDPPDT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}pdppdt`;  
    static readonly InvestPDPPDT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}pdppdt_danh_sach`;  
    static readonly InvestPDPPDT_PheDuyetOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}pdppdt_phe_duyet`;  

    // Module Phê duyệt yêu cầu tái tục
    // PDYCTT = Phê duyệt yêu cầu tái tục
    static readonly InvestMenuPDYCTT: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}pdyctt`;  
    static readonly InvestPDYCTT_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}pdyctt_danh_sach`;  
    static readonly InvestPDYCTT_PheDuyetOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}pdyctt_phe_duyet`;  

    // Module Phê duyệt yêu cầu rút vốn
    // PDYCTT = Phê duyệt yêu cầu rút vốn
    static readonly InvestMenuPDYCRV: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}pdycrv`;  
    static readonly InvestPDYCRV_DanhSach: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Table}pdycrv_danh_sach`;  
    static readonly InvestPDYCRV_PheDuyetOrHuy: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}pdycrv_phe_duyet`;
    static readonly InvestPDYCRV_ChiTietHD: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}pdycrv_chi_tiet_hd`;  

    // Menu báo cáo
    public static readonly Invest_Menu_BaoCao: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}bao_cao`;

    public static readonly Invest_BaoCao_QuanTri: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}bao_cao_quan_tri`;
    public static readonly Invest_BaoCao_QuanTri_THCKDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_thck_dau_tu`;
    public static readonly Invest_BaoCao_QuanTri_THCMaBDS: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_thc_ma_bds`;
    public static readonly Invest_BaoCao_QuanTri_DCDenHan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_dc_den_han`;
    public static readonly Invest_BaoCao_QuanTri_ThucChi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_thuc_chi`;
    public static readonly Invest_BaoCao_QuanTri_TCTDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_tct_dau_tu`;
    public static readonly Invest_BaoCao_QuanTri_THCKDTVanHanhHVF: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_thckdt_van_hanh_hvf`;
    public static readonly Invest_BaoCao_QuanTri_SKTKNhaDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_sktk_nha_dau_tu`;
    public static readonly Invest_BaoCao_QuanTri_THCKDTBanHo: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_quan_tri_thckdt_ban_ho`;

    public static readonly Invest_BaoCao_VanHanh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}bao_cao_van_hanh`;
    public static readonly Invest_BaoCao_VanHanh_THCKDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_van_hanh_thck_dau_tu`;
    public static readonly Invest_BaoCao_VanHanh_THCMaBDS: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_van_hanh_thc_ma_bds`;
    public static readonly Invest_BaoCao_VanHanh_DCDenHan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_van_hanh_dc_den_han`;
    public static readonly Invest_BaoCao_VanHanh_ThucChi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_van_hanh_thuc_chi`;
    public static readonly Invest_BaoCao_VanHanh_TCTDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_van_hanh_tct_dau_tu`;

    public static readonly Invest_BaoCao_KinhDoanh: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}bao_cao_kinh_doanh`;
    public static readonly Invest_BaoCao_KinhDoanh_THCKDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_kinh_doanh_thck_dau_tu`;
    // public static readonly Invest_BaoCao_KinhDoanh_THCMaBDS: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_kinh_doanh_thc_ma_bds`;
    public static readonly Invest_BaoCao_KinhDoanh_DCDenHan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_kinh_doanh_dc_den_han`;
    public static readonly Invest_BaoCao_KinhDoanh_ThucChi: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_kinh_doanh_thuc_chi`;
    public static readonly Invest_BaoCao_KinhDoanh_TCTDauTu: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.ButtonAction}bao_cao_kinh_doanh_tct_dau_tu`;

    // Menu truy vấn
    public static readonly Invest_Menu_TruyVan: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Menu}truy_van`;

    public static readonly Invest_TruyVan_ThuTien_NganHang: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}truy_van_thu_tien_ngan_hang`;
    public static readonly Invest_TruyVan_ChiTien_NganHang: string = `${PermissionInvestConst.InvestModule}${PermissionInvestConst.Page}truy_van_chi_tien_ngan_hang`;
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
    public static TYPE_TRADING_PROVIDERS = ['T', 'RT'];  // PARTNERROOT HOẶC TRADINGROOT
    
    

    public static getUserTypeInfo(code, property) {
        let type = this.list.find(t => t.code == code);
        if (type) return type[property];
        return '';
    }

}

export class ProductBondPrimaryConst {

    public static priceTypes = [
        {
            name: 'Nguyên giá',
            code: 1,
        },
        {
            name: 'Nguyên giá + lãi phát sinh',
            code: 2,
        },
    ];
    // public static statusConst = [
    //     {
    //         name: 'Khởi tạo',
    //         code: 'T'
    //     },
    //     {
    //         name: 'Trình duyệt',
    //         code: 'P'
    //     },
    //     {
    //         name: 'Duyệt',
    //         code: 'A'
    //     },
    //     {
    //         name: 'Hủy duyệt',
    //         code: 'C'
    //     }
    // ];
    public static statusList = [
        {
            name: 'Kích hoạt',
            code: 'A'
        },
        {
            name: 'Chưa kích hoạt',
            code: 'D'
        }
    ]

    // public static NHAP = 'T';
    // public static TRINH_DUYET = 'P';
    // public static DUYET = 'A';
    // public static DONG = 'C';


    public static getNamePriceType(code: number) {
        let priceType = this.priceTypes.find(priceType => priceType.code == code);
        if (priceType) return priceType.name;
        return '';
    }

    public static paymentTypes = [
        {
            name: 'Từ ngày phát hành',
            code: 1,
        }
    ];
    
    public static getNamePaymentTypes(code) {
        let paymentType = this.paymentTypes.find(paymentType => paymentType.code == code);
        if (paymentType) return paymentType.name;
        return '';
    }

    public static status = [
        {
            name: 'Khởi tạo',
            code: 'T',
            severity: 'help'

        },
        {
            name: 'Trình duyệt',
            code: 'P',
            severity: 'warning'
        },
        {
            name: 'Hoạt động',
            code: 'A',
            severity: 'success'
        },
        {
            name: 'Hủy duyệt',
            code: 'C',
            severity: 'secondary'
        }
    ];

    public static getNameStatus(code) {
        for (let item of this.status) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getSeverityStatus(code) {
        for (let item of this.status) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    public static KHOI_TAO = 'T';
    public static TRINH_DUYET = 'P';
    public static HOAT_DONG = 'A';
    public static HUY_DUYET = 'C';

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
    public static list = [
        {
            name: 'Kích hoạt',
            code: 'A',
            severity: 'success'
        },
        {
            name: 'Khóa',
            code: 'D',
            severity: 'secondary'
        }
    ];

    public static ACTIVE = 'A';
    public static DEACTIVE = 'D';
   
    public static getInfo(code, atribution=null) {
        let status = this.list.find(type => type.code == code);
        return status && atribution ? status[atribution] : status;
    }
   
}

export class ProductBondDetailConst {

    public static getUnitDates(code) {
        for (let item of this.unitDates) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getAllowOnlineTradings(code) {
        for (let item of this.allowOnlineTradings) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getMarkets(code) {
        for (let item of this.markets) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static markets = [
        {
            name: 'Phân phối',
            code: '1'
        },
        {
            name: 'Ghi sổ',
            code: '2'
        }
    ];

    public static unitDates = [
        {
            name: 'Năm',
            code: 'Y',
        },
        {
            name: 'Tháng',
            code: 'M'
        }

    ];

    public static allowOnlineTradings = [
        {
            name: 'Cho phép',
            code: 'Y'
        },
        {
            name: 'Không cho phép',
            code: 'N'
        }
    ]
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

    public static getInfo(code, atribution=null) {
        let status = this.list.find(s => s.code == code);
        return status && atribution ? status[atribution] : status;
    }
}

export class DistributionConst {
    public static KHOI_TAO = 1;
    public static CHO_DUYET = 2;
    public static HOAT_DONG = 3;
    public static CANCEL = 4
    public static CLOSED = 5;

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
            isClose: 'N',
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
    ];

    public static getStatusInfo(code, atribution = null) {
        const status = this.statusConst.find(p => p.code == code);
        return (atribution && status) ? status[atribution] : status;
    }

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

    public static CO_CHI_TIEN = 1;
    public static KHONG_CHI_TIEN = 2;

    public static methodInterests = [
        {
            name: 'Có chi tiền',
            code: this.CO_CHI_TIEN,
            severity: 'success',
        },
        {
            name: 'Không chi tiền',
            code: this.KHONG_CHI_TIEN,
            severity: 'secondary',
        }
    ];

    public static getMethodInterestInfo(code, atribution = null) {
        let type = this.methodInterests.find(type => type.code == code) || this.methodInterests[0];
        return atribution && type ? type[atribution] : type;
    }
}

export class DistributionContractTemplateConst {

    public static contractTypeList = [
        {
            name: 'Hợp đồng đặt lệnh',
            code: 1,
           
        },
        {
            name: 'Hợp đồng rút tiền',
            code: 2,
            
        },
        {
            name: 'Hợp đồng tái tục',
            code: 3,
            
        },
        // {
        //     name: 'Hợp đồng rút tiền App',
        //     code: 4,
        // },
        {
            name: 'Hợp đồng tái tục gốc + lợi nhận',
            code: 5,
        },
    ];

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
    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
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

    public static INVESTOR = 'I';
    public static BUSINESSCUSTOMER = 'B';

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

    public static tranClassifies = [
        {
            name: 'Thanh toán hợp đồng',
            code: 1,
        },
        {
            name: 'Chi trả lợi tức',
            code: 2,
        },
        {
            name: 'Rút vốn',
            code: 3,
        },
        {
            name: 'Tái tục hợp đồng',
            code: 4,
        },
    ];

    public static LOAI_GD_THANH_TOAN_HOP_DONG = 1;
    public static LOAI_GD_CHI_TRA_LOI_TUC = 2;
    public static LOAI_GD_RUT_VON = 3;
    public static LOAI_GD_TAI_TUC_HOP_DONG = 4;
    //
    public static BLOCK_ACTION_THANH_TOAN = [
        this.LOAI_GD_CHI_TRA_LOI_TUC, 
        this.LOAI_GD_RUT_VON, 
        this.LOAI_GD_TAI_TUC_HOP_DONG
    ];

    public static getTranClassify(code: number) {
        const tranClassify = this.tranClassifies.find(p => p.code == code);
        return tranClassify ? tranClassify.name : '-';
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

    public static getPaymentStatusInfo(code: number, atribution = null) {
        const status = this.paymentStatus.find(p => p.code == code);
        return status && atribution ? status[atribution] : status;
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

    public static getStatusInfo(code, atribution = null) {
        const status = this.statusConst.find(p => p.code == code);
        return (atribution && status) ? status[atribution] : status;
    }

    public static getNameStatus(code) {
        let type = this.statusConst.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

}

export class SearchConst {
    public static DEBOUNCE_TIME = 1200;
}

export class ProductBondInterestConst {
    public static interestPeriodTypes = [
        {
            name: 'Năm',
            code: 'Y',
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
            name: 'Ngày',
            code: 'D'
        },
        {
            name: 'Quý',
            code: 'Q',
        }
    ];

    public static periodTypes = [
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
            code: 'Y',
        }

    ];

    public static INTEREST_TYPES = {
        DINH_KY: 1,
        CUOI_KY: 2,
    };

    // KIỂU TRẢ LÃI
    public static interestTypes = [
        {
            name: 'Định kỳ',
            code: this.INTEREST_TYPES.DINH_KY,
        },
        {
            name: 'Cuối kỳ',
            code: this.INTEREST_TYPES.CUOI_KY,
        },
    ]

    public static isDinhKy(interest) {
        return interest == this.INTEREST_TYPES.DINH_KY;
    }

    public static getPeriodUnits(code) {
        for (let item of this.interestPeriodTypes) {
            if (item.code == code) return item.name;
        }
        return '';
    }
    /**
     * LẤY TÊN KIỂU TRẢ LÃI
     * @param code 
     * @returns TÊN KIỂU TRẢ LÃI
     */
    public static getInterestTypeName(code) {
        const found = this.interestTypes.find(o => o.code === code);
        return found ? found.name : '';
    }
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
    
    public static PROJECT = 1;
    public static DISTRIBUTION = 2;
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

    public static getStatusInfo(code, atribution=null) {
        let status = this.status.find(status => status.code == code);
        return status && atribution ? status[atribution] : status;
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

    public static getActionTypeName(code, atribution = null) {
        let actionType = this.actionType.find(actionType => actionType.code == code);
        return (atribution && actionType) ? actionType[atribution] : actionType;
    }
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
}

export class ProductBondSecondaryConst {

    public static getAllowOnlineTradings(code) {
        for (let item of this.allowOnlineTradings) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static allowOnlineTradings = [
        {
            name: 'Cho phép',
            code: 'Y'
        },
        {
            name: 'Không cho phép',
            code: 'N'
        }
    ];

    public static STATUS = {
        NHAP: 1,
        TRINH_DUYET: 2,
        HOAT_DONG: 3,
        CLOSED: 4,
    }

    public static statusList = [
        {
            name: 'Khởi tạo',
            severity: 'help',
            code: this.STATUS.NHAP,
        },
        {
            name: 'Trình duyệt',
            code: this.STATUS.TRINH_DUYET,
            severity: 'warning',
        },
        {
            name: 'Hoạt động',
            code: this.STATUS.HOAT_DONG,
            severity: 'success',
        },
        {
            name: 'Đóng',
            code: this.STATUS.CLOSED,
            severity: 'secondary',
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
}
export class PolicyTemplateConst {
    //
    public static NO = 'N';
    public static YES = 'Y';

    public static KY_HAN_THAP_HON_GAN_NHAT = 1;
    public static CO_DINH = 2;
    public static calculateWithdrawTypes = [
        {
            name: 'Kỳ hạn thấp hơn gần nhất',
            code: this.KY_HAN_THAP_HON_GAN_NHAT,
        },
        {
            name: 'Giá trị cố định',
            code: this.CO_DINH,
        },
    ];
    
    //
    public static TYPE_CO_DINH = 1;
    public static TYPE_LINH_HOAT = 2;
    public static TYPE_GIOI_HAN = 3;
    public static TYPE_CHI_TRA_CO_DINH_THEO_NGAY = 4;

    public static types = [
        {
            name: 'Cố định',
            code: this.TYPE_CO_DINH,
        },
        {
            name: 'Linh hoạt',
            code: this.TYPE_LINH_HOAT,
        },
        // {
        //     name: 'Giới hạn',
        //     code: 3,
        // },
        // {
        //     name: 'Chi trả cố định theo ngày',
        //     code: 4,
        // }

    ];

    public static RENEWAL_TYPE_NEW = 1;
    public static RENEWAL_TYPE_HOLD = 2;

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

    public static getCalculateType(code, atribution = 'name') {
        const calculateType = this.calculateType.find(c => c.code == code);
        return calculateType ? calculateType[atribution] : null;
    }

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
            name: "Khóa",
            code: "D",
            severity: 'secondary'
        }
    ];
    //
    public static KICH_HOAT = 'A';
    public static KHOA = 'D';

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

    public static getStatusInfo(code, atribution = null) {
        const status = this.statuses.find(p => p.code == code);
        return (atribution && status) ? status[atribution] : status;
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

    public static INTEREST_RATE_TYPE_PERIODIC = 1;
    public static INTEREST_RATE_TYPE_PERIOD_END = 2;
    public static NGAY_CO_DINH = 3;
    public static NGAY_DAU_THANG = 4;
    public static NGAY_CUOI_THANG = 5;

    public static interestType = [
        {
            name: 'Định kỳ',
            code: this.INTEREST_RATE_TYPE_PERIODIC,
        },
        {
            name: 'Cuối kỳ',
            code: this.INTEREST_RATE_TYPE_PERIOD_END,
        },
        {
            name: 'Ngày cố định hàng tháng',
            code: this.NGAY_CO_DINH,
        },
        {
            name: 'Ngày đầu tháng',
            code: this.NGAY_DAU_THANG,
        },
        {
            name: 'Ngày cuối tháng',
            code: this.NGAY_CUOI_THANG,
        }
    ];

    public static FIX_PAYMENT_DATE_DEFAULT = 1;

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

        // Danh sách ngày chi trả cố định
   public static getListFixedPaymentDate() {
    let listFixedPaymentDate = [];
    for (let i = 1; i < 29; i++) {
        listFixedPaymentDate.push({
          name: `Ngày ${i} hàng tháng`,
          code: i,
        });
    }
    return listFixedPaymentDate;
}

}
export class ProductBondPolicyDetailTemplateConst {
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
    public static GIAI_TOA = 7;
    public static TAT_TOAN = 8;
    public static DA_XOA = 9;

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
        
        {
            name: 'CMND/CCCD',
            code: 4,
            field: 'idNo',
            placeholder: 'Nhập số CMND hoặc CCCD ...'
        },
        // {
        //     name: 'Cấu trúc mã HĐ',
        //     code: 5,
        //     field: 'contractCodeGen',
        //     placeholder: 'Nhập cấu trúc mã HĐ ...'
        // }
    ];

    
    
    public static getInfoFieldFilter(field, attribute: string = 'name') {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
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
        // {
        //     name: "Đã xóa",
        //     code: this.DA_XOA,
        //     severity: 'secondary',
        // }
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
            backLink: '/trading-contract/contract-active',
        },
        {
            name: "Phong tỏa",
            code: 6,
            severity: 'danger',
            backLink: '/trading-contract/contract-active',
        },
        {
            name: "Tất toán",
            code: 8,
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

    public static SOURCE_ONLINE = 1;
    public static SOURCE_OFFLINE = 2;

    public static sources = [
        {
            name: 'Online',
            code: this.SOURCE_ONLINE,
            severity: 'success'

        },
        {
            name: 'Offline',
            code: this.SOURCE_OFFLINE,
            severity: 'secondary'

        },
        // {
        //     name: 'Sale đặt lệnh',
        //     code: 3,
        // }
    ];

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

    public static getHistoryStatus(code, atribution = null) {
        const status = this.investHistoryStatus.find(type => type.code == code);
        return atribution && status ? status[atribution] : status;
    }

    public static getStatusRenewal(code, atribution=null) {
        const status = this.statusRenewal.find(type => type.code == code);
        return status && atribution ? status[atribution] : status;
    }

    public static getInfoSource(code, atribution = null) {
        const source = this.sources.find(type => type.code == code);
        return (source && atribution) ? source[atribution] : source;
    }

    public static getInfoOrderSource(code, atribution = null) {
        const orderSource = this.orderSources.find(type => type.code == code);
        return (orderSource && atribution) ? orderSource[atribution] : orderSource;
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
            backLink: '/trading-contract/settlement-contract',

        },
        {
            name: "Đã xóa",
            code: this.DA_XOA,
            severity: 'secondary',
        }
    ];

    public static getStatusInfo(code, atribution = null) {
        const status = this.status.find(p => p.code == code);
        return (atribution && status) ? status[atribution] : status;
    }

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

}


export class InterestPaymentConst {
    public static apiForStepList = {
        due: {
            api: '/api/invest/order/lap-danh-sach-chi-tra?',
            apiExport: '/api/invest/export-excel-report/list-interest-payment-due?',
        },
        paid: {
            api: '/api/invest/interest-payment/find?',
            apiExport: '/api/invest/export-excel-report/list-interest-payment-due?',
        }
    }

    public static STATUS_DUEDATE = 0;
    public static STATUS_CREATED_LIST = 1;
    public static STATUS_DONE = 2;
    public static STATUS_DONE_ONLINE = 3;
    public static STATUS_DONE_OFFLINE = 4;

    public static STEP_START = 0;
    public static STEP_CENTER = 50;
    public static STEP_END = 100;

    public static statusInterests = [
        {
            name: 'Đến hạn chi trả',
            code: this.STATUS_DUEDATE,
            api: this.apiForStepList.due.api,
            apiExport: this.apiForStepList.due.apiExport,
            loadingStep: this.STEP_START,
        },
        {
            name: 'Đã lập chưa chi trả',
            code: this.STATUS_CREATED_LIST,
            api: this.apiForStepList.due.api,
            apiExport: this.apiForStepList.due.apiExport,
            loadingStep: this.STEP_CENTER,
        },
        {
            name: 'Đã chi trả',
            code: this.STATUS_DONE,
            api: this.apiForStepList.due.api,
            apiExport: this.apiForStepList.due.apiExport,
            loadingStep: this.STEP_END,
        },
        {
            name: 'Đã chi trả (Tự động)',
            code: this.STATUS_DONE_ONLINE,
            api: this.apiForStepList.due.api,
            apiExport: this.apiForStepList.due.apiExport,
            loadingStep: this.STEP_END,
        },
        {
            name: 'Đã chi trả (Thủ công)',
            code: this.STATUS_DONE_OFFLINE,
            api: this.apiForStepList.due.api,
            apiExport: this.apiForStepList.due.apiExport,
            loadingStep: this.STEP_END,
        },
    ];

    public static getstatusInterestsInfo(code, atribution = null) {
        const status = this.statusInterests.find(status => status.code == code);
        return atribution && status ? status[atribution] : status;
    }
    

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

    public static getTypeRenewal(code, atribution = 'name') {
        const type = this.statusExpires.find(s => s.code == code);
        return type ? type[atribution] : 'Nhận gốc + lợi tức';
    }

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

    public static getStatusInfo(code) {
        return {name: this.mediaStatus[code], severity: this.statusSeverity[code]};
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
    ];

    public static TAI_KHOAN_NGAN_HANG = 'BUSINESS_CUSTOMER_BANK_ACC_ID';
    public static SO_TIEN_DAU_TU = 'TOTAL_VALUE';
    public static KY_HAN = 'POLICY_DETAIL_ID';
    public static MA_GIOI_THIEU = 'SALE_REFERRAL_CODE';
    public static DIA_CHI = 'CONTACT_ADDRESS_ID';
    public static GIAY_TO = 'INVESTOR_IDEN_ID';
    public static NGUON = 'SOURCE';
    
    public static fieldName = [
        {
            name: 'Tài khoản ngân hàng',
            code: this.TAI_KHOAN_NGAN_HANG,
        },
        {
            name: 'Số tiền đầu tư',
            code: this.SO_TIEN_DAU_TU,
        },
        {
            name: 'Kỳ hạn',
            code: this.KY_HAN,
        },
        {
            name: 'Mã giới thiệu',
            code: this.MA_GIOI_THIEU,
        },
        {
            name: 'Địa chỉ',
            code: this.DIA_CHI,
        },
        {
            name: 'Giấy tờ',
            code: this.GIAY_TO,
        },
        {
            name: 'Nguồn',
            code: this.NGUON
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

    public static KICH_HOAT = 1;
    public static HUY_KICH_HOAT = 2;

    public static status = [
        {
            name: 'Kích hoạt',
            code: this.KICH_HOAT,
            severity: 'success',
        },
        {
            name: 'Hủy kích hoạt',
            code: this.HUY_KICH_HOAT,
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

    public static LOAI_KHAC = 1;
    public static LOAI_CAM_CO_KHOAN_VAY = 2;
    public static LOAI_UNG_VON = 3;

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

    public static getStatusInfo(code, atribution = null) {
        const status = this.status.find(p => p.code == code);
        return atribution && status ? status[atribution] : status;
    }
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

export class ExportReportConst {
    public static BC_THCK_DAU_TU = 1;
    public static BC_THCK_CAC_BDS = 2;
    public static BC_DC_DEN_HAN = 3;
    public static BC_THUC_CHI = 4;
    public static BC_TTCT_DAU_TU = 5;
    public static BC_THCKDT_VAN_HANH_HVF = 6;
    public static BC_SKTK_NHA_DAU_TU = 7;
    public static BC_THCKDT_BAN_HO = 8;
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

    public static statuses = [
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

    public static getStatusInfo(code, atribution = null) {
        let status = this.statuses.find(s => s.code == code);
        return (atribution && status) ? status[atribution] : status;
    }

    public static getCustomerInfo(code, atribution = 'name') {
        let status = this.customerType.find(s => s.code == code);
        return status ? status[atribution] : null;
    }

}

export class SampleContractConst {

    public static contractType = [
        {
            name: 'Hợp đồng đặt lệnh',
            code: 1,
           
        },
        {
            name: 'Hợp đồng rút tiền',
            code: 2,
            
        },
        {
            name: 'Hợp đồng tái tục',
            code: 3,
            
        },
        // {
        //     name: 'Hợp đồng rút tiền App',
        //     code: 4,
        // },
        {
            name: 'Hợp đồng tái tục gốc + lợi nhận',
            code: 5,
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
            name: 'Online',
            code: 1,
           
        },
        {
            name: 'Offline',
            code: 2,
            
        },
    ];

    public static getContractSource(code, atribution=null) {
        let status = this.contractSource.find(s => s.code == code);
        return status && atribution ? status[atribution] : status;
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

    public static getInfo(code, atribution: string = '') {
        let status = this.list.find(s => s.code == code);
        return atribution && status ? status[atribution] : status; 
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

    public static getResponseInfo(code, atribution = null) {
        let response = this.responses.find(s => s.code == code);
        return atribution && response ? response[atribution] : response; 
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

    public static getInfo(code, atribution= null) {
        let status = this.list.find(s => s.code == code);
        return status && atribution ? status[atribution] : status;
    }
}

export class CollectMoneyBankConst {

    public static paymentType = [
        {
            name: 'Chi trả lợi tức',
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

    public static getStatusInfo(code, atribution = null) {
        const status = this.statusConst.find(p => p.code == code);
        return (atribution && status) ? status[atribution] : status;
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

export class DashboardConst {
    // 1: Đặt lệnh mới; 2: Rút tiền; 3: Yêu cầu nhận hợp đồng
    public static DAT_LENH_MOI = 1;
    public static TAI_TUC = 2;
    public static RUT_TIEN = 3;
    public static NHAN_HOP_DONG = 4;

    public static listAction = [
        {
            name: 'Đặt lệnh mới',
            code: this.DAT_LENH_MOI,
        },
        {
            name: 'Tạo yêu cầu tái tục',
            code: this.TAI_TUC
        },
        {
            name: 'Tạo yêu cầu rút tiền',
            code: this.RUT_TIEN
        },
        {
            name: 'Tạo yêu cầu nhận hợp đồng',
            code: this.NHAN_HOP_DONG
        }
    ];

    public static getActionInfo(code) {
        let type = this.listAction.find(t => t.code == code);
        return type?.name ?? '';
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

export const TRADING_CONTRACT_ROUTER = 'trading-contract';

export class QueryMoneyBankCost{
    public static GARNER = 1;
    public static INVEST = 2;
    public static COMPANY_SHARE = 3;
    public static REAL_ESTATE = 4;
}

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

    public static getInfo(code, atribution=null) {
        let status = this.listStatus.find(type => type.code == code);
        return status && atribution ? status[atribution] : status;
    }
}

export class TableConst {
    public static columnTypes = {
        TEXT: 'TEXT',
        STATUS: 'STATUS',
        /*STATUS: gen ra 1 property mới với quy tắc row['field+Element']. 
        Ví dụ: field columnTables là status giá trị số nhưng hiển thị ra là thẻ tag hiển thị kiểu trạng thái
        row.statusElement = Object gồm các property cần có trong thẻ tag
        */
        CHECKBOX_SHOW: 'CHECKBOX_SHOW',
        CHECKBOX_ACTION: 'CHECKBOX_ACTION',
        ACTION_ICON: 'ACTION_ICON',
        ACTION_DROPDOWN: 'ACTION_DROPDOWN',
        ACTION_BUTTONS: 'ACTION_BUTTONS',
        ACTION_BUTTON: 'ACTION_BUTTON',
        CURRENCY: 'CURRENCY',   // FORMAT CURRENCY FOR PIPE
        CONVERT_DISPLAY: 'CONVERT_DISPLAY', 
        /*CONVERT_DISPLAY: gen ra 1 property mới với quy tắc row['field+Display']. 
        Ví dụ: field columnTables là type giá trị số nhưng hiển thị ra với giá trị text convert từ giá trị số
        row.typeDisplay = Giá trị convert
        */
        DATE: 'DATE',
        DATETIME: 'DATETIME',
        IMAGE: "IMAGE",
        REORDER: "REORDER", // KÉO THẢ DÒNG
    };
    //

    public static alignFrozenColumn = {
        LEFT: 'left',
        RIGHT: 'right',
    }

    public static message = {
        emptyMessage: 'Không có dữ liệu',
        totalMessage: 'bản ghi',
        selectedMessage: 'chọn'
    }
}

export const AtributionConfirmConst = {
    header: 'Thông báo!',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Đồng ý',
    rejectLabel: 'Hủy bỏ',
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