import { Attribute } from '@angular/core';
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
}


export class PermissionCompanyShareConst {
    
    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly CompanyShareModule: string = "bond.";
    public static readonly CompanyShareWeb: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Web}`;
    //
    public static readonly CompanySharePageDashboard: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}dashboard`;
    // Cài đặt
    public static readonly CompanyShareMenuCaiDat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}cai_dat`;
    // CHNNL: cấu hình ngày nghỉ lễ
    public static readonly CompanyShareMenuCaiDat_CHNNL: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_chnnl`;
    public static readonly CompanyShareCaiDat_CHNNL_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_chnnl_danh_sach`;
    public static readonly CompanyShareCaiDat_CHNNL_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_chnnl_cap_nhat`;
    // Cài đặt -> Tổ chức phát hành
    // TCPH: Tổ chức phát hành
    public static readonly CompanyShareMenuCaiDat_TCPH: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_tcph`;
    public static readonly CompanyShareCaiDat_TCPH_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_tcph_danh_sach`;
    public static readonly CompanyShareCaiDat_TCPH_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_tcph_them_moi`;
    
    // TCPH - tab thông tin chi tiết
    public static readonly CompanyShare_TCPH_ThongTinChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}tcph_thong_tin_chi_tiet`;
    public static readonly CompanyShare_TCPH_ThongTinChung: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}tcph_thong_tin_chung`;
    public static readonly CompanyShare_TCPH_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}tcph_chi_tiet`;
    public static readonly CompanyShare_TCPH_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}tcph_cap_nhat`;

    public static readonly CompanyShare_TCPH_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}tcph_xoa`;

    // Cài đặt -> Đại lý sơ cấp
    // DLSC: Đại lý sơ cấp
    public static readonly CompanyShareMenuCaiDat_DLSC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_dlsc`;
    public static readonly CompanyShareCaiDat_DLSC_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_dlsc_danh_sach`;
    public static readonly CompanyShareCaiDat_DLSC_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_dlsc_them_moi`;
    // DLSC - tab thông tin chi tiết
    // TTC: Thông tin chung
    public static readonly CompanyShare_DLSC_ThongTinChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}dlsc_thong_tin_chi_tiet`;
    public static readonly CompanyShare_DLSC_ThongTinChung: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}dlsc_thong_tin_chung`;
    public static readonly CompanyShare_DLSC_TTC_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}dlsc_ttc_chi_tiet`;

    // TKDN: Tài khoản đăng nhập
    public static readonly CompanyShare_DLSC_TaiKhoanDangNhap: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}dlsc_tkdn`;
    public static readonly CompanyShare_DLSC_TKDN_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}dlsc_tkdn_danh_sach`;
    public static readonly CompanyShare_DLSC_TKDN_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}dlsc_tkdn_them_moi`;

    // Cài đặt -> Đại lý lưu ký
    // DLLK: Đại lý lưu ký
    public static readonly CompanyShareMenuCaiDat_DLLK: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_dllk`;
    public static readonly CompanyShareCaiDat_DLLK_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_dllk_danh_sach`;
    public static readonly CompanyShareCaiDat_DLLK_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_dllk_them_moi`;

    // TCPH - tab thông tin chi tiết đại lý lưu ký
    public static readonly CompanyShare_DLLK_ThongTinChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}dllk_thong_tin_chi_tiet`;
    public static readonly CompanyShare_DLLK_ThongTinChung: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}dllk_thong_tin_chung`;
    public static readonly CompanyShare_DLLK_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}dllk_chi_tiet`;
    public static readonly CompanyShare_DLLK_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}dllk_xoa`;

    // Cài đặt -> Chính sách mẫu
    // CSM: Chính sách mẫu
    public static readonly CompanyShareMenuCaiDat_CSM: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_csm`;
    public static readonly CompanyShareCaiDat_CSM_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_csm_danh_sach`;
    public static readonly CompanyShareCaiDat_CSM_ThemChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_them_chinh_sach`;
    public static readonly CompanyShareCaiDat_CSM_ThemKyHan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_them_ky_han`;
    public static readonly CompanyShareCaiDat_CSM_CapNhatChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_cap_nhat_chinh_sach`;
    public static readonly CompanyShareCaiDat_CSM_XoaChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_xoa_chinh_sach`;
    public static readonly CompanyShareCaiDat_CSM_KichHoatOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_kich_hoat_or_huy`;
    public static readonly CompanyShareCaiDat_CSM_CapNhatKyHan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_cap_nhat_ky_han`;
    public static readonly CompanyShareCaiDat_CSM_XoaKyHan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_xoa_ky_han`;
    public static readonly CompanyShareCaiDat_CSM_KyHan_KichHoatOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_csm_ky_han_kich_hoat_or_huy`;

    // Cài đặt -> Mẫu thông báo
    // MTB: Mẫu thông báo
    public static readonly CompanyShareMenuCaiDat_MTB: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_mtb`;
    public static readonly CompanyShareCaiDatMTB_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}caidat_mtb_cap_nhat`;

    // Cài đặt -> Hình ảnh
    public static readonly CompanyShareMenuCaiDat_HinhAnh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}caidat_hinhanh`;

    
    public static readonly CompanyShareCaiDat_HinhAnh_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}caidat_hinhanh_danh_sach`;
    public static readonly CompanyShareCaiDat_HinhAnh_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_hinhanh_them_moi`;
    public static readonly CompanyShareCaiDat_HinhAnh_Sua: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_hinhanh_sua`;
    public static readonly CompanyShareCaiDat_HinhAnh_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_hinhanh_xoa`;
    public static readonly CompanyShareCaiDat_HinhAnh_DuyetDang: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}caidat_hinhanh_duyet_dang`;

    // Quản lý cổ phần
    // QLTP: Quản lý cổ phần
    public static readonly CompanyShareMenuQLTP: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qltp`;

    // QLTP -> Lô cổ phần
    // LTP: Lô cổ phần
    // TTCT: Thông tin chi tiết
    public static readonly CompanyShareMenuQLTP_LTP: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qltp_ltp`;
    public static readonly CompanyShareMenuQLTP_LTP_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_ltp_danh_sach`;
    public static readonly CompanyShareMenuQLTP_LTP_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_them_moi`;
    public static readonly CompanyShareMenuQLTP_LTP_TTCT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}qltp_ltp_thong_tin_chi_tiet`;
    public static readonly CompanyShareMenuQLTP_LTP_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_dong_xoa`;
    public static readonly CompanyShareMenuQLTP_LTP_TrinhDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_dong_trinh_duyet`;
    public static readonly CompanyShareMenuQLTP_LTP_PheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_phe_duyet`;
    public static readonly CompanyShareMenuQLTP_LTP_EpicXacMinh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_epic_phe_duyet`;
    public static readonly CompanyShareMenuQLTP_LTP_DongTraiPhieu: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_ltp_dong_trai_phieu`;
    
    // Thông tin chi tiết lô cổ phần
    // LTP: Lô cổ phần
    // TTC: Thông tin chung
    // TSDB: Tài sản đảm bảo, HSPL: Hồ sơ pháp lý, TTTT: Thông tin trái tức
    public static readonly CompanyShare_LTP_TTC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}ltp_thong_tin_chung`;
    public static readonly CompanyShare_LTP_TTC_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}ltp_ttc_chi_tiet`;
    public static readonly CompanyShare_LTP_TTC_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}ltp_ttc_cap_nhat`;
    // public static readonly CompanyShare_LTP_MoTa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}ltp_mo_ta`;
    public static readonly CompanyShare_LTP_TSDB: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}ltp_tsdb`;
    public static readonly CompanyShare_LTP_TSDB_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}ltp_ttdb_danh_sach`;
    public static readonly CompanyShare_LTP_TSDB_Them: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_ttdb_them`;
    public static readonly CompanyShare_LTP_TSDB_Sua: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_ttdb_sua`;
    public static readonly CompanyShare_LTP_TSDB_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_ttdb_xoa`;
    public static readonly CompanyShare_LTP_TSDB_TaiXuong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_ttdb_tai_xuong`;
    
    public static readonly CompanyShare_LTP_HSPL: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}ltp_hspl`;
    public static readonly CompanyShare_LTP_HSPL_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}ltp_hsbl_danh_sach`;
    public static readonly CompanyShare_LTP_HSPL_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_hspl_them_moi`;
    public static readonly CompanyShare_LTP_HSPL_XemHoSo: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_hspl_xem_ho_so`;
    public static readonly CompanyShare_LTP_HSPL_Download: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_hspl_download`;
    public static readonly CompanyShare_LTP_HSPL_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}ltp_hspl_xoa`;
   
    public static readonly CompanyShare_LTP_TTTT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}ltp_tttt`;
    public static readonly CompanyShare_LTP_TTTT_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}ltp_tttt_danh_sach`;


    // QLTP -> Phát hành sơ cấp
    // PHSC: Phát hành sơ cấp
    public static readonly CompanyShareMenuQLTP_PHSC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qltp_phsc`;
    public static readonly CompanyShareMenuQLTP_PHSC_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_phsc_danh_sach`;
    public static readonly CompanyShareMenuQLTP_PHSC_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_phsc_them_moi`;
    public static readonly CompanyShareMenuQLTP_PHSC_TrinhDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_phsc_trinh_duyet`;
    public static readonly CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_phsc_phe_duyet_or_huy`;
    public static readonly CompanyShareMenuQLTP_PHSC_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_phsc_xoa`;

    public static readonly CompanyShareMenuQLTP_PHSC_TTCT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}qltp_phsc_thong_tin_chi_tiet`;
    public static readonly CompanyShare_PHSC_TTC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}phsc_thong_tin_chung`;
    public static readonly CompanyShare_PHSC_TTC_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}phsc_ttc_chi_tiet`;
    public static readonly CompanyShare_PHSC_TTC_ChinhSua: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}phsc_ttc_chinh_sua`;

    // public static readonly CompanyShare_PHSC_CSL: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}phsc_chinh_sach_lai`;


    // QLTP -> Hợp đồng phân phối
    // HDPP: Hợp đồng phân phối
    public static readonly CompanyShareMenuQLTP_HDPP: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qltp_hdpp`;
    public static readonly CompanyShareMenuQLTP_HDPP_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_hdpp_danh_sach`;
    public static readonly CompanyShareMenuQLTP_HDPP_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_hdpp_them_moi`;
    public static readonly CompanyShareMenuQLTP_HDPP_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_hdpp_xoa`;

    public static readonly CompanyShareMenuQLTP_HDPP_TTCT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}qltp_hdpp_thong_tin_chi_tiet`;
    public static readonly CompanyShare_HDPP_TTC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_thong_tin_chung`;
    public static readonly CompanyShare_HDPP_TTC_ChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}hdpp_ttc_chi_tiet`;

    // Tab thông tin thanh toán = tttt
    public static readonly CompanyShare_HDPP_TTThanhToan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_thong_tin_thanh_toan`;
    public static readonly CompanyShare_HDPP_TTTT_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_tttt_danh_sach`;
    public static readonly CompanyShare_HDPP_TTTT_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_tttt_them_moi`;
    public static readonly CompanyShare_HDPP_TTTT_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_tttt_cap_nhat`;
    public static readonly CompanyShare_HDPP_TTTT_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_tttt_xoa`;
    public static readonly CompanyShare_HDPP_TTTT_ChiTietThanhToan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}hdpp_tttt_chi_tiet_thanh_toan`;
    public static readonly CompanyShare_HDPP_TTTT_PheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_tttt_phe_duyet`;
    public static readonly CompanyShare_HDPP_TTTT_HuyPheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_tttt_huy_phe_duyet`;

    public static readonly CompanyShare_HDPP_DMHSKHK: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_danh_muc_ho_so_hang_ky`;
    public static readonly CompanyShare_HDPP_DMHSKHK_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_danh_muc_ho_so_hang_ky_danh_sach`;
    public static readonly CompanyShare_HDPP_DMHSKHK_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_them_moi`;
    public static readonly CompanyShare_HDPP_DMHSKHK_PheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_phe_duyet`;
    public static readonly CompanyShare_HDPP_DMHSKHK_HuyPheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_huy_phe_duyet`;
    public static readonly CompanyShare_HDPP_DMHSKHK_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_xoa`;

    public static readonly CompanyShare_HDPP_TTTraiTuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_thong_tin_trai_tuc`;
    public static readonly CompanyShare_HDPP_TTTraiTuc_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_thong_tin_trai_tuc_danh_sach`;


    // QLTP -> Bán theo kỳ hạn
    // BTKH: Bán theo kỳ hạn
    public static readonly CompanyShareMenuQLTP_BTKH: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qltp_btkh`;
    public static readonly CompanyShareMenuQLTP_BTKH_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_danh_sach`;
    public static readonly CompanyShareMenuQLTP_BTKH_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_them_moi`;
    public static readonly CompanyShareMenuQLTP_BTKH_TrinhDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_trinh_duyet`;
    public static readonly CompanyShareMenuQLTP_BTKH_DongTam: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_dong_tam`;
    public static readonly CompanyShareMenuQLTP_BTKH_BatTatShowApp: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_bat_tat_show_app`;
    public static readonly CompanyShareMenuQLTP_BTKH_EpicXacMinh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_epic_phe_duyet`;
    public static readonly CompanyShareMenuQLTP_BTKH_ThongTinChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}qltp_btkh_thong_tin_chi_tiet`;

    public static readonly CompanyShare_BTKH_TTCT_ThongTinChung: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_ttc`;
    public static readonly CompanyShare_BTKH_TTCT_ThongTinChung_XemChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}qltp_btkh_thong_tin_chi_tiet_ttc_xem_chi_tiet`;
    public static readonly CompanyShare_BTKH_TTCT_ThongTinChung_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ttc_cap_nhat`;

    //Tab Tổng quan
    public static readonly CompanyShare_BTKH_TongQuan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}btkh_tong_quan`;
    public static readonly CompanyShare_BTKH_TongQuan_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}btkh_tong_quan_cap_nhat`;
    public static readonly CompanyShare_BTKH_TongQuan_ChonAnh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}btkh_tong_quan_chon_anh`;
    public static readonly CompanyShare_BTKH_TongQuan_ThemToChuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}btkh_tong_quan_them_to_chuc`;
    public static readonly CompanyShare_BTKH_TongQuan_UploadFile: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}btkh_tong_quan_upload_file`;
    public static readonly CompanyShare_BTKH_TongQuan_DanhSach_ToChuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}btkh_tong_quan_danh_sach_to_chuc`;
    public static readonly CompanyShare_BTKH_TongQuan_DanhSach_File: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}btkh_tong_quan_danh_sach_file`;

    public static readonly CompanyShare_BTKH_TTCT_ChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_cs`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_thong_tin_chi_tiet_cs_ds`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_themcs`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_cap_nhat`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_xoa`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_BatTatShowApp: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_bat_tat_show_app`;
    public static readonly CompanyShare_BTKH_TTCT_ChinhSach_KichHoatOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_huy_kich_hoat_or_huy`;
    public static readonly CompanyShare_BTKH_TTCT_KyHan_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_them_moi`;
    public static readonly CompanyShare_BTKH_TTCT_KyHan_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_cap_nhat`;
    public static readonly CompanyShare_BTKH_TTCT_KyHan_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_Xoa`;
    public static readonly CompanyShare_BTKH_TTCT_KyHan_BatTatShowApp: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_bat_tat_show_app`;
    public static readonly CompanyShare_BTKH_TTCT_KyHan_KichHoatOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_kich_hoat_or_huy`;

    public static readonly CompanyShare_BTKH_TTCT_BangGia: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_bg`;
    public static readonly CompanyShare_BTKH_TTCT_BangGia_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_thong_tin_chi_tiet_bg_danh_sach`;
    public static readonly CompanyShare_BTKH_TTCT_BangGia_ImportExcelBG: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_import_excel_bg`;
    public static readonly CompanyShare_BTKH_TTCT_BangGia_DownloadFileMau: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_download_file_mau`;
    public static readonly CompanyShare_BTKH_TTCT_BangGia_XoaBangGia: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_xoa_bang_gia`;

    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_filecs`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_thong_tin_chi_tiet_filecs_danhsach`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_UploadFileChinhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_uploadfilecs`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_Sua: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_sua`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_xoa`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_Download: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_download`;
    public static readonly CompanyShare_BTKH_TTCT_FileChinhSach_XemFile: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_xem_file`;


    public static readonly CompanyShare_BTKH_TTCT_MauHopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong`;
    public static readonly CompanyShare_BTKH_TTCT_MauHopDong_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_ds`;
    public static readonly CompanyShare_BTKH_TTCT_MauHopDong_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_them_moi`;
    public static readonly CompanyShare_BTKH_TTCT_MauHopDong_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_cap_nhat`;
    public static readonly CompanyShare_BTKH_TTCT_MauHopDong_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_xoa`;
    public static readonly CompanyShare_BTKH_TTCT_MauHopDong_KichHoatOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_kich_hoat_or_huy`;

    // Mẫu giao nhận HĐ
    public static readonly CompanyShare_BTKH_TTCT_MauGiaoNhanHD: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd`;
    public static readonly CompanyShare_BTKH_TTCT_MauGiaoNhanHD_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_ds`;
    public static readonly CompanyShare_BTKH_TTCT_MauGiaoNhanHD_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_giao_nhan_hd_them_moi`;
    public static readonly CompanyShare_BTKH_TTCT_MauGiaoNhanHD_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_cap_nhat`;



    public static readonly CompanyShareMenuHDPP: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp`;

    // Hợp đồng phân phối -> Sổ lệnh
    public static readonly CompanyShareHDPP_SoLenh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_solenh`;
    public static readonly CompanyShareHDPP_SoLenh_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_solenh_ds`;
    public static readonly CompanyShareHDPP_SoLenh_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_them_moi`;
    public static readonly CompanyShareHDPP_SoLenh_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_xoa`;

    // TT Chi tiết
    public static readonly CompanyShareHDPP_SoLenh_TTCT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}hdpp_solenh_ttct`;
    //Tab TT Chung  
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_solenh_ttct_ttc`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_XemChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Form}hdpp_xlhd_ttct_ttc_xem_chi_tiet`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttc_cap_nhat`;
    // 
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_DoiKyHan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ky_han`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_DoiMaGT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ma_gt`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_DoiTKNganHang: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_tk_ngan_hang`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu`;

    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_solenh_ttct_ttthanhtoan`;
    //TT Thanh Toan
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_solenh_ttct_ttthanhtoan_ds`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_themtt`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_chi_tiettt`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_CapNhat: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_cap_nhat`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_phe_duyet`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TTThanhToan_Xoa: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_xoa`;

    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_solenh_ttct_hskh_dangky`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_solenh_ttct_hskh_dangky_ds`;
    //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsm`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsckdt`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_chuyen_online`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_ky_dien_tu`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_gui_thong_bao`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy`;

    public static readonly CompanyShareHDPP_SoLenh_TTCT_LoiTuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_solenh_ttct_loituc`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_LoiTuc_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_solenh_ttct_loituc_ds`;
    //
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TraiTuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_solenh_ttct_traituc`;
    public static readonly CompanyShareHDPP_SoLenh_TTCT_TraiTuc_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_solenh_ttct_traituc_ds`;
    
    // HDPP -> Xử lý hợp đồng
    // XLHD: Xử lý hợp đồng
    public static readonly CompanyShareHDPP_XLHD: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_xlhd`;
    public static readonly CompanyShareHDPP_XLHD_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_xlhd_ds`;

    // HDPP -> Hợp đồng
    public static readonly CompanyShareHDPP_HopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_hopdong`;
    public static readonly CompanyShareHDPP_HopDong_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_hopdong_ds`;
    public static readonly CompanyShareHDPP_HopDong_YeuCauTaiTuc: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hopdong_yc_tai_tuc`;
    // public static readonly CompanyShareHDPP_HopDong_YeuCauRutVon: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hopdong_yc_rut_von`;
    public static readonly CompanyShareHDPP_HopDong_PhongToaHopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hopdong_phong_toa_hd`;

    // HDPP -> Giao nhận hợp đồng
    public static readonly CompanyShareHDPP_GiaoNhanHopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_giaonhanhopdong`;
    public static readonly CompanyShareHDPP_GiaoNhanHopDong_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_giaonhanhopdong_ds`;
    public static readonly CompanyShareHDPP_GiaoNhanHopDong_DoiTrangThai: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_giaonhanhopdong_doitrangthai`;
    public static readonly CompanyShareHDPP_GiaoNhanHopDong_XuatHopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_giaonhanhopdong_xuathopdong`;
    
    public static readonly CompanyShareHDPP_GiaoNhanHopDong_ThongTinChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}hdpp_giaonhanhopdong_ttct`;
    // Thông tin chung
    public static readonly CompanyShareHDPP_GiaoNhanHopDong_TTC: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Tab}hdpp_giaonhanhopdong_ttc`;

    // HDPP -> Phong tỏa giải tóa
    // PTGT: Phong tỏa giải tỏa
        
    public static readonly CompanyShareHDPP_PTGT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_ptgt`;
    public static readonly CompanyShareHDPP_PTGT_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_ptgt_ds`;
    public static readonly CompanyShareHDPP_PTGT_GiaiToaHopDong: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_ptgt_phong_toa_hd`;
    public static readonly CompanyShareHDPP_PTGT_XemChiTiet: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_ptgt_xem_chi_tiet`;

    // HDDH: HỢP ĐỒNG ĐÁO HẠN
    public static readonly CompanyShareHDPP_HDDH: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_hddh`;
    public static readonly CompanyShareHDPP_HDDH_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_hddh_ds`;
    public static readonly CompanyShareHDPP_HDDH_ThongTinDauTu: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hddh_thong_tin_dau_tu`;
    public static readonly CompanyShareHDPP_HDDH_LapDSChiTra: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hddh_lap_ds_chi_tra`;
    public static readonly CompanyShareHDPP_HDDH_DuyetKhongChi: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_hddh_duyet_khong_chi`;

    // Quản lý phê duyệt
    // QLPD: Quản lý phê duyệt

    public static readonly CompanyShareMenuQLPD: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}qlpd`;

    // Quản lý phê duyệt -> Phê duyệt lô cổ phần
    // PDLTP: Phê duyệt lô cổ phần
    public static readonly CompanyShareQLPD_PDLTP: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_pdltp`;
    public static readonly CompanyShareQLPD_PDLTP_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_pdltp_ds`;
    public static readonly CompanyShareQLPD_PDLTP_PheDuyetOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_pdltp_phe_duyet_or_huy`;


    // Quản lý phê duyệt -> Phê duyệt bán theo kỳ hạn
    // PDLTP: Phê duyệt bán theo kỳ hạn
    public static readonly CompanyShareQLPD_PDBTKH: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_pdbtkh`;
    public static readonly CompanyShareQLPD_PDBTKH_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_pdbtkh_ds`;
    public static readonly CompanyShareQLPD_PDBTKH_PheDuyetOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_pdbtkh_phe_duyet_or_huy`;

    // Quản lý phê duyệt -> Phê duyệt yêu cầu tái tục
    // PDYCTT: Phê duyệt yêu cầu tái tục
    public static readonly CompanyShareQLPD_PDYCTT: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}hdpp_pdyctt`;
    public static readonly CompanyShareQLPD_PDYCTT_DanhSach: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Table}hdpp_pdyctt_ds`;
    public static readonly CompanyShareQLPD_PDYCTT_PheDuyetOrHuy: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.ButtonAction}hdpp_pdyctt_phe_duyet_or_huy`;

    // Menu báo cáo
    public static readonly CompanyShare_Menu_BaoCao: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Menu}bao_cao`;
    public static readonly CompanyShare_BaoCao_QuanTri: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}bao_cao_quan_tri`;
    public static readonly CompanyShare_BaoCao_VanHanh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}bao_cao_van_hanh`;
    public static readonly CompanyShare_BaoCao_KinhDoanh: string = `${PermissionCompanyShareConst.CompanyShareModule}${PermissionCompanyShareConst.Page}bao_cao_kinh_doanh`;
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
    public static TYPE_TRADING = ['T', 'RT'];  // PARTNERROOT HOẶC TRADINGROOT
    
    

    public static getUserTypeInfo(code, property) {
        let type = this.list.find(t => t.code == code);
        if (type) return type[property];
        return '';
    }

}

export class CompanySharePrimaryConst {

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

export class CompanyShareDetailConst {

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

export class StatusCompanyShareInfoFileConst {
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


    public static status = {
        ACTIVE: "A",
        DEACTIVE: "D"
    }

    public static classify = [
        { name: "PRO", code: 1 },
        { name: "PRO A", code: 2 },
        { name: "PNOTE", code: 3 }
    ]

    public static contractType = [
        { name: "Hợp đồng đầu tư cổ phần", code: 1 },
        { name: "Biên nhận hồ sơ", code: 2 },
        { name: "Giấy xác nhận giao dịch cổ phần", code: 3 },
        { name: "Phiếu đề nghị thực hiện giao dịch", code: 4 },
        { name: "Hợp đồng đặt mua cổ phần", code: 5 },
        { name: "Hợp đồng đặt bán cổ phần", code: 6 },
        { name: "Biên nhận hợp đồng", code: 7 },
        { name: "Giấy xác nhận số dư cổ phần", code: 8 },
        { name: "Bảng minh họa thu nhập từ cổ phần và kết quả đầu tư", code: 9 }
    ]

    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }
}

export class InvestorAccountConst {
    public static statusName = {
        A: { name: "Kích hoạt", color: "success" },
        D: { name: "Chưa kích hoạt", color: "warning" }
    }
}

export class CompanyShareInfoConst {

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

    public static companySharePeriodUnits = [
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

export class IsSignPdfConst {
    public static YES = 'Y';
    public static NO = 'N';
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

    public static getNameTransactionType(code: number) {
        const transactionType = this.transactionTypes.find(t => t.code == code);
        return transactionType ? transactionType.name : '-';
    }

    public static paymentTypes = [
        {
            name: 'Chuyển khoản',
            code: 1,
        },
        {
            name: 'Tiền mặt',
            code: 2,
        },
    ];

    public static PAYMENT_TYPE_CASH = 1;
    public static PAYMENT_TYPE_TRANSFER = 2;

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

    public static paymentTypes = [
        {
            name: 'Tiền mặt',
            code: 1,
        },
        {
            name: 'Chuyển khoản',
            code: 2,
        },
    ];

    public static PAYMENT_TYPE_CASH = 1;
    public static PAYMENT_TYPE_TRANSFER = 2;

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

    public static STATUS_YES = 'Y';
    public static STATUS_NO = 'N';
}

export class SearchConst {
    public static DEBOUNCE_TIME = 800;
}

export class CompanyShareInterestConst {
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
            severity: 'success',
        },
        {
            name: 'Khóa',
            code: 'D',
            severity: 'secondary',
        }
    ];

    public static getStatusName(code, property) {
        for (let item of this.statusList) {
            if (item.code == code) return item[property];
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
            name: 'Đóng',
            code: 4,
            severity: 'secondary',
        },
    ];

    public static TRINH_DUYET = 1;
    public static DA_DUYET = 2;
    public static HUY = 3;
    
    public static dataType = [
        {
            name: 'Người dùng',
            code: 1,
        },
        {
            name: 'Nhà đầu tư',
            code: 2,
        },
        {
            name: 'Khách hàng doanh nghiệp',
            code: 3,
        },
        {
            name: 'Lô cổ phần',
            code: 4,
        },
        {
            name: 'Bán theo kỳ hạn',
            code: 5,
        },
        {
            name: 'Tệp tin hợp đồng',
            code: 6,
        },
        {
            name: 'Phát hành sơ cấp',
            code: 7,
        },
    ];

    public static STATUS_USER = 1;
    public static STATUS_INVESTOR = 2;
    public static STATUS_BUSINESS_CUSTOMER = 3;
    public static STATUS_PRO_COMPANY_SHARE_INFO = 4;
    public static STATUS_PRO_COMPANY_SHARE_SECONDARY = 5;
    public static STATUS_DISTRI_CONTRACT_FILE = 6;
    public static STATUS_PRO_COMPANY_SHARE_PRIMARY = 7;
    public static STATUS_REINSTATEMENT = 11;

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
        },
        {
            name: 'Sửa',
            code: 2,
        },
        {
            name: 'Xoá',
            code: 3,
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

export class CompanyShareSecondaryConst {

    public static getAllowOnlineTradings(code) {
        for (let item of this.allowOnlineTradings) {
            if (item.code == code) return item.name;
        }
        return '';
    }
    public static displayType = [
        { name: "Trước khi duyệt hợp đồng", code: "B" },
        { name: "Sau khi duyệt hợp đồng", code: "A" },
    ]
    public static types = [
        { name: "Cá nhân", code: "I" },
        { name: "Doanh nghiệp", code: "B" },
    ]  

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
        CANCEL: 4,
        CLOSED: 5,
    }

    public static KICH_HOAT = 'A';
    public static KHOA = 'D';

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
            name: 'Hủy duyệt',
            code: this.STATUS.CANCEL,
            severity: 'danger',
        },
        {
            name: 'Đóng',
            code: this.STATUS.CLOSED,
            severity: 'secondary'
        }
    ];

    public static getStatusName(code, isClose) {
        if (isClose == 'Y') return 'Đóng';
        for (let item of this.statusList) {
            if (item.code == code) return item.name;
        }
        return '';
    }

    public static getStatusSeverity(code, isClose) {
        if (isClose == 'Y') return 'secondary';
        for (let item of this.statusList) {
            if (item.code == code) return item.severity;
        }
        return '';
    }

    public static getType(code) {
        for (let item of this.types) {
            if (item.code == code) return item.name;
        }
        return '';
    }

}
export class CompanySharePolicyTemplateConst {
    public static type = [
        {
            name: 'Ngày bán cố định',
            code: 1,
        },
        {
            name: 'Ngày bán thay đổi',
            code: 2,
        }
    ];
    public static classify = [
        {
            name: 'PRO',
            code: 1,
        },
        {
            name: 'PRO A',
            code: 2,
        },
        {
            name: 'PNote',
            code: 3,
        },
    ];

    public static investorType = [
        {
            name: 'Chuyên nghiệp',
            code: 'P',
        },
        {
            name: 'Tất cả',
            code: 'A',
        }
    ];

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
    public static KICH_HOAT = 'A';
    public static KHONG_KICH_HOAT = 'D';

    public static getSeverityStatus(code) {
        const status = this.status.find(p => p.code == code);
        return status ? status.severity : '-';
    }
    public static getNameStatus(code) {
        let type = this.status.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameType(code) {
        let type = this.type.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameClassify(code) {
        let type = this.classify.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }

    public static getNameInvestorType(code) {
        let type = this.investorType.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
    public static getNameIsTransfer(code) {
        let type = this.isTransfer.find(type => type.code == code);
        if (type) return type.name;
        return '';
    }
}

export class CompanySharePolicyDetailTemplateConst {
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
    public static KHONG_KICH_HOAT = 'D';

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
export class OrderConst {

    public static fieldFilters = [
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
            name: 'Mã hợp đồng',
            code: 3,
            field: 'contractCode',
            placeholder: 'Nhập mã hợp đồng...',
        }
    ];

    public static getInfoFieldFilter(field, attribute: string) {
        const fieldFilter = this.fieldFilters.find(fieldFilter => fieldFilter.field == field);
        return fieldFilter ? fieldFilter[attribute] : null;
    }
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
            severity: 'success',
        },
        {
            name: 'Offline',
            code: 2,
            severity: 'secondary',
        },
        // {
        //     name: 'Sale đặt lệnh',
        //     code: 3,
        //     severity: 'secondary',
        // }
    ];

    public static SOURCE_ONLINE = 1;
    public static SOURCE_OFFLINE = 2;

    public static orderSources = [
        {
            name: 'Quản trị viên',
            code: 1,
        },
        {
            name: 'Khách hàng',
            code: 2,
        },
        {
            name: 'Tư vấn viên',
            code: 3,
        }
    ];

    public static getInfoSource(code, infoName) {
        let source = this.sources.find(type => type.code == code);
        return source ? source[infoName] : '';
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

    public static status = [
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
            name: "Chờ duyệt hợp đồng",
            code: 4,
            severity: 'danger',
            backLink: '/trading-contract/contract-processing',
        },
        {
            name: "Đang đầu tư",
            code: 5,
            severity: 'success',
            backLink: '/trading-contract/contract-active',
        },
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
    ];

    public static statusProcessing = [
        {
            name: "Chờ ký hợp đồng",
            code: 3,
            severity: "help",
            backLink: '/trading-contract/contract-processing',
        },
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
            severity: 'success',
            backLink: '/trading-contract/contract-active',
        },
        {
            name: "Phong toả",
            code: 6,
            severity: 'success',
            backLink: '/trading-contract/contract-active',
        },
    ];

    public static statusBlock = [
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

    public static REINSTATEMENT_TYPES = [
        {
            name: 'Tái tục gốc',
            code: 2,
        },
        {
            name: 'Tái tục gốc + lợi nhuận',
            code: 3,
        }
    ];

    public static ORIGINAL_REINSTATEMENT = 2;
    public static ALL_REINSTATEMENT = 3;
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
    ];

    public static STATUS_DUEDATE = 0;
    public static STATUS_CREATED_LIST = 1;
    public static STATUS_DONE = 2;
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

    public static getName(code) {
        const status = this.list.find(item => item.code == code);
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

export class FormNotificationConst {
    public static IMAGE_APPROVE = "IMAGE_APPROVE";
    public static IMAGE_CLOSE = "IMAGE_CLOSE";
}