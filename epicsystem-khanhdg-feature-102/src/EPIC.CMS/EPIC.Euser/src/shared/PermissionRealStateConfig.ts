import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionRealStateConfig = {};

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

    //
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
    
    // Module Thông báo hệ thống
    public static readonly RealStateThongBaoHeThong: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}thong_bao_he_thong`;

    // Menu Quản lý dự án
    public static readonly RealStateMenuProjectManager: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}project_manager`;

    public static readonly RealStateMenuProjectOverview: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Menu}project_overview`;
    public static readonly RealStateProjectOverview_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}project_overview_danh_sach`;
    public static readonly RealStateProjectOverview_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_them_moi`;
    public static readonly RealStateProjectOverview_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}project_overview_xoa`;

    public static readonly RealStateProjectOverview_ThongTinProjectOverview: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Page}project_overview_thong_tin_project_overview`;
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
   
    // Tab Thông tin chung - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_thong_tin_chung`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_thong_tin_chung_them_moi`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_thong_tin_chung_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_thong_tin_chung_cap_nhat`;

    // Tab Chính sách ưu đãi CĐT - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_chinh_sach`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_chinh_sach_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_chinh_sach_cap_nhat`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_chinh_sach_doi_trang_thai`;

    // Tab Tiện ích - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_tien_ich`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_tien_ich_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_tien_ich_cap_nhat`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_TienIch_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_tien_ich_doi_trang_thai`;

    // Tab Vật liệu - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_vat_lieu`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_vat_lieu_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_vat_lieu_cap_nhat`;

    // Tab Sơ đồ thiết kế - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_so_do_tk`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_so_do_tk_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_so_do_tk_cap_nhat`;

    // Tab Hình ảnh - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_hinh_anh_du_an`;

    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_hinh_anh`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_detail_hinh_anh_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_them_moi`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_cap_nhat`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_doi_trang_thai`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_hinh_anh_xoa`;

    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ChiTiet: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Form}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_chi_tiet`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ThemMoi: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_them_moi`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_CapNhat: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_cap_nhat`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_DoiTrangThai: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_doi_trang_thai`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_Xoa: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.ButtonAction}phan_phoi_dssp_chi_tiet_detail_nhom_hinh_anh_xoa`;

    // Tab Lịch sử - Chi tiết căn hộ phân phối
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_LichSu: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Tab}phan_phoi_dssp_chi_tiet_detail_lich_su`;
    public static readonly RealStatePhanPhoi_DSSP_ChiTiet_LichSu_DanhSach: string = `${PermissionRealStateConst.RealStateModule}${PermissionRealStateConst.Table}phan_phoi_dssp_chi_tiet_detail_lich_su_danh_sach`;
    
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

    PermissionRealStateConfig[PermissionRealStateConst.RealStatePageDashboard] = { type: PermissionTypes.Menu, name: 'Dashboard tổng quan', parentKey: null, icon: "pi pi-fw pi-home" };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSetting] = { type: PermissionTypes.Menu, name: 'Cài đặt', parentKey: null, icon: "pi pi-fw pi-cog" };
    
    // Quản lý chủ đầu tư
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuChuDT] = { type: PermissionTypes.Menu, name: 'Chủ đầu tư', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuChuDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuChuDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuChuDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/Huỷ', parentKey: PermissionRealStateConst.RealStateMenuChuDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_ThongTinChuDauTu] = { type: PermissionTypes.Page, name: 'Thông tin chủ đầu tư', parentKey: PermissionRealStateConst.RealStateMenuChuDT, webKey: WebKeys.RealState };

    // Thông tin chi tiết chủ đầu tư
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateChuDT_ThongTinChuDauTu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateChuDT_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateChuDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateChuDT_ThongTinChung, webKey: WebKeys.RealState };

    // Module Đại lý
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuDaiLy] = { type: PermissionTypes.Menu, name: 'Đại lý', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuDaiLy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuDaiLy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/Huỷ', parentKey: PermissionRealStateConst.RealStateMenuDaiLy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuDaiLy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_ThongTinDaiLy] = { type: PermissionTypes.Page, name: 'Thông tin đại lý', parentKey: PermissionRealStateConst.RealStateMenuDaiLy, webKey: WebKeys.RealState };
    //Tab Thông tin chung
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateDaiLy_ThongTinDaiLy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateDaiLy_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateDaiLy_ThongTinChung, webKey: WebKeys.RealState };

    // Chính sách phân phối 
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuCSPhanPhoi] = { type: PermissionTypes.Menu, name: 'Chính sách phân phối', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSPhanPhoi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuCSPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSPhanPhoi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionRealStateConst.RealStateMenuCSPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSPhanPhoi_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionRealStateConst.RealStateMenuCSPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSPhanPhoi_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionRealStateConst.RealStateMenuCSPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSPhanPhoi_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (Chính sách)', parentKey: PermissionRealStateConst.RealStateMenuCSPhanPhoi, webKey: WebKeys.RealState };

    // Chính sách bán hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuCSBanHang] = { type: PermissionTypes.Menu, name: 'Chính sách bán hàng', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSBanHang_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuCSBanHang, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSBanHang_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionRealStateConst.RealStateMenuCSBanHang, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSBanHang_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionRealStateConst.RealStateMenuCSBanHang, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSBanHang_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionRealStateConst.RealStateMenuCSBanHang, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCSBanHang_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (Chính sách)', parentKey: PermissionRealStateConst.RealStateMenuCSBanHang, webKey: WebKeys.RealState };

    // Cấu trúc hợp đồng giao dịch
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich] = { type: PermissionTypes.Menu, name: 'Cấu trúc mã hợp đồng giao dịch', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDGiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDGiaoDich_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDGiaoDich_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDGiaoDich_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDGiaoDich_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich, webKey: WebKeys.RealState };

    // Cấu trúc hợp đồng coc
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuCTMaHDCoc] = { type: PermissionTypes.Menu, name: 'Cấu trúc hợp đồng cọc', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDCoc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDCoc_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDCoc_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDCoc_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateCTMaHDCoc_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuCTMaHDCoc, webKey: WebKeys.RealState };

    // Mẫu hợp đồng chủ đầu tư
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuMauHDCDT] = { type: PermissionTypes.Menu, name: 'Mẫu HĐ chủ đầu tư', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_TaiFileDoanhNghiep] = { type: PermissionTypes.ButtonAction, name: 'Tải file d/nghiệp', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_TaiFileCaNhan] = { type: PermissionTypes.ButtonAction, name: 'Tải file cá nhân', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDCDT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuMauHDCDT, webKey: WebKeys.RealState };

    // Mẫu hợp đồng đại lý
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuMauHDDL] = { type: PermissionTypes.Menu, name: 'Mẫu HĐ đại lý', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_TaiFileDoanhNghiep] = { type: PermissionTypes.ButtonAction, name: 'Tải file d/nghiệp', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_TaiFileCaNhan] = { type: PermissionTypes.ButtonAction, name: 'Tải file cá nhân', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMauHDDL_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuMauHDDL, webKey: WebKeys.RealState };
    
    // Thông báo hệ thống
    PermissionRealStateConfig[PermissionRealStateConst.RealStateThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Thông báo hệ thống', parentKey: PermissionRealStateConst.RealStateMenuSetting, webKey: WebKeys.RealState };

    // Menu Quản lý dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectManager] = { type: PermissionTypes.Menu, name: 'Quản lý dự án', parentKey: null, webKey: WebKeys.RealState, icon: "pi pi-map" };
    // Tổng quan dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectOverview] = { type: PermissionTypes.Menu, name: 'Tổng quan dự án', parentKey: PermissionRealStateConst.RealStateMenuProjectManager, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectOverview, webKey: WebKeys.RealState };

    // Tab Thông tin chung
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinChung, webKey: WebKeys.RealState };
    
    // Tab Mô tả dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_MoTa] = { type: PermissionTypes.Tab, name: 'Mô tả dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_MoTa_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectOverview_MoTa, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_MoTa_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_MoTa, webKey: WebKeys.RealState };
    
    // Tab Tiện ích
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIch] = { type: PermissionTypes.Tab, name: 'Tiện ích', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchHeThong] = { type: PermissionTypes.Table, name: 'Tiện ích hệ thống', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIch, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchHeThong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchHeThong, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchHeThong_QuanLy] = { type: PermissionTypes.ButtonAction, name: 'Quản lý', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchHeThong, webKey: WebKeys.RealState };
    
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac] = { type: PermissionTypes.Table, name: 'Tiện ích khác', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIch, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_DoiTrangThaiNoiBat] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái nổi bật', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchKhac_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchKhac, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa] = { type: PermissionTypes.Table, name: 'Ảnh minh hoạ', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIch, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_TienIchMinhHoa, webKey: WebKeys.RealState };

    // Tab Cấu trúc dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_CauTruc] = { type: PermissionTypes.Tab, name: 'Cấu trúc dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_CauTruc_DanhSach] = { type: PermissionTypes.Form, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_CauTruc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_CauTruc_Them] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_CauTruc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_CauTruc_Sua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStateProjectOverview_CauTruc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_CauTruc_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionRealStateConst.RealStateProjectOverview_CauTruc, webKey: WebKeys.RealState };
        
    // Tab Hình ảnh
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnhDuAn] = { type: PermissionTypes.Tab, name: 'Hình ảnh dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh] = { type: PermissionTypes.Form, name: 'Hình ảnh', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnhDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnh, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh] = { type: PermissionTypes.Form, name: 'Nhóm hình ảnh', parentKey: PermissionRealStateConst.RealStateProjectOverview_HinhAnhDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_NhomHinhAnh, webKey: WebKeys.RealState };

    // Tab Chính sách dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach_DanhSach] = { type: PermissionTypes.Form, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChinhSach, webKey: WebKeys.RealState };
    
    // Tab Hồ sơ pháp lý
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo] = { type: PermissionTypes.Tab, name: 'Hồ sơ dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_DanhSach] = { type: PermissionTypes.Form, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_XemFile] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_TaiXuong] = { type: PermissionTypes.ButtonAction, name: 'Tải xuống', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_HoSo_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_HoSo, webKey: WebKeys.RealState };
    
    // Tab Bài đăng Facebook
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post] = { type: PermissionTypes.Tab, name: 'Bài đăng facebook', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_Facebook_Post, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_Facebook_Post, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_Facebook_Post, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_Facebook_Post, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_Facebook_Post_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_Facebook_Post, webKey: WebKeys.RealState };
    
    // Tab Chia sẻ dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn] = { type: PermissionTypes.Tab, name: 'Chia sẻ dự án', parentKey: PermissionRealStateConst.RealStateProjectOverview_ThongTinProjectOverview, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectOverview_ChiaSeDuAn, webKey: WebKeys.RealState };

    // Bảng hàng dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectList] = { type: PermissionTypes.Menu, name: 'Bảng hàng dự án', parentKey: PermissionRealStateConst.RealStateMenuProjectManager, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectList_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuProjectList, webKey: WebKeys.RealState };
    
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail] = { type: PermissionTypes.Page, name: 'Chi tiết bảng hàng', parentKey: PermissionRealStateConst.RealStateMenuProjectList, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách chi tiết bảng hàng', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_UploadFile] = { type: PermissionTypes.ButtonAction, name: 'Upload File', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_TaiFileMau] = { type: PermissionTypes.ButtonAction, name: 'Tải File mẫu', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_KhoaCan] = { type: PermissionTypes.ButtonAction, name: 'Khoá căn', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_NhanBan] = { type: PermissionTypes.ButtonAction, name: 'Nhân bản', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết căn hộ', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail, webKey: WebKeys.RealState };

    // Tab Thông tin chung - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ThongTinChung, webKey: WebKeys.RealState };
    
    // Tab Chính sách ưu đãi CĐT - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách ưu đãi CĐT', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChinhSach, webKey: WebKeys.RealState };
    
    // Tab Tiện ích - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch] = { type: PermissionTypes.Tab, name: 'Tiện ích', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_TienIch, webKey: WebKeys.RealState };
    
    // Tab Vật liệu - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_VatLieu] = { type: PermissionTypes.Tab, name: 'Vật liệu', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_VatLieu_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_VatLieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_VatLieu_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_VatLieu, webKey: WebKeys.RealState };
    
    // Tab Sơ đồ thiết kế - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_SoDoTK] = { type: PermissionTypes.Tab, name: 'Sơ đồ thiết kế', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_SoDoTK_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_SoDoTK, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuProjectListDetail_SoDoTK_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_SoDoTK, webKey: WebKeys.RealState };
    
    // Tab Hình ảnh - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnhDuAn] = { type: PermissionTypes.Tab, name: 'Hình ảnh dự án', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh] = { type: PermissionTypes.Form, name: 'Hình ảnh', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnhDuAn, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_HinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnh, webKey: WebKeys.RealState };

    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh] = { type: PermissionTypes.Form, name: 'Nhóm hình ảnh', parentKey: PermissionRealStateConst.RealStateProjectListDetail_HinhAnhDuAn, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateProjectListDetail_NhomHinhAnh, webKey: WebKeys.RealState };

    // Tab lịch sử - Bảng hàng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionRealStateConst.RealStateMenuProjectListDetail_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateProjectListDetail_LichSu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateProjectListDetail_LichSu, webKey: WebKeys.RealState };

    // Menu Phân phối
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi] = { type: PermissionTypes.Menu, name: 'Phân phối sản phẩm', parentKey: PermissionRealStateConst.RealStateMenuProjectManager, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi, webKey: WebKeys.RealState };

    // Tab Thông tin chung - Phân phối
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung phân phối', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMenuPhanPhoi_ThongTinChung, webKey: WebKeys.RealState };

    // Tab Danh sách sản phẩm
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP] = { type: PermissionTypes.Tab, name: 'Danh sách sản phẩm', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ThemMoi] = { type: PermissionTypes.Table, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_Mo_KhoaCan] = { type: PermissionTypes.ButtonAction, name: 'Mở/Khóa căn', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP, webKey: WebKeys.RealState };
    
    // Tab Thông tin chung - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ThemMoi] = { type: PermissionTypes.Form, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ThongTinChung, webKey: WebKeys.RealState };
    
    // // Tab Chính sách ưu đãi CĐT - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách ưu đãi CĐT', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_ChinhSach, webKey: WebKeys.RealState };
    
    // // Tab Tiện ích - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch] = { type: PermissionTypes.Tab, name: 'Tiện ích', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_TienIch, webKey: WebKeys.RealState };
    
    // // Tab Vật liệu - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu] = { type: PermissionTypes.Tab, name: 'Vật liệu', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_VatLieu, webKey: WebKeys.RealState };
    
    // // Tab Sơ đồ thiết kế - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK] = { type: PermissionTypes.Tab, name: 'Sơ đồ thiết kế', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_SoDoTK, webKey: WebKeys.RealState };
    
    // // Tab Hình ảnh - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn] = { type: PermissionTypes.Tab, name: 'Hình ảnh dự án', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };

    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh] = { type: PermissionTypes.Form, name: 'Hình ảnh', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnh, webKey: WebKeys.RealState };

    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh] = { type: PermissionTypes.Form, name: 'Nhóm hình ảnh', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_HinhAnhDuAn, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_NhomHinhAnh, webKey: WebKeys.RealState };

    // // Tab lịch sử - Chi tiết căn hộ phân phối
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet, webKey: WebKeys.RealState };
    // PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_LichSu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStatePhanPhoi_DSSP_ChiTiet_LichSu, webKey: WebKeys.RealState };

    // Tab Chính sách phân phối
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách phân phối', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_ChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChinhSach, webKey: WebKeys.RealState };
    
    // Tab mẫu biểu hợp đồng
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu] = { type: PermissionTypes.Tab, name: 'Mẫu biểu hợp đồng', parentKey: PermissionRealStateConst.RealStatePhanPhoi_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_XuatWord] = { type: PermissionTypes.ButtonAction, name: 'Xuất Word', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_XuatPdf] = { type: PermissionTypes.ButtonAction, name: 'Xuất PDF', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePhanPhoi_MauBieu_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStatePhanPhoi_MauBieu, webKey: WebKeys.RealState };
    
    // Menu Mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuMoBan] = { type: PermissionTypes.Menu, name: 'Mở bán', parentKey: PermissionRealStateConst.RealStateMenuProjectManager, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DoiNoiBat] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái nổi bật', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DoiShowApp] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái ShowApp', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DungBan] = { type: PermissionTypes.ButtonAction, name: 'Dừng bán', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuMoBan, webKey: WebKeys.RealState };
    // Tab Thông tin chung
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung mở bán', parentKey: PermissionRealStateConst.RealStateMoBan_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMoBan_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMoBan_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThongTinChung_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionRealStateConst.RealStateMoBan_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ThongTinChung_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMoBan_ThongTinChung, webKey: WebKeys.RealState };

    // Tab Danh sách sản phẩm - Mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP] = { type: PermissionTypes.Tab, name: 'Danh sách sản phẩm', parentKey: PermissionRealStateConst.RealStateMoBan_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_Them] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_DoiShowApp] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái ShowApp', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_DoiShowPrice] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái hiển thị giá', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP, webKey: WebKeys.RealState };
    // Tab Thông tin chung - Chi tiết căn hộ mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_ThongTinChung, webKey: WebKeys.RealState };
    // Tab Lịch sử - Chi tiết căn hộ mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_LichSu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMoBan_DSSP_ChiTiet_LichSu, webKey: WebKeys.RealState };

    // Tab Chính sách mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách', parentKey: PermissionRealStateConst.RealStateMoBan_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMoBan_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChinhSach_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMoBan_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionRealStateConst.RealStateMoBan_ChinhSach, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_ChinhSach_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMoBan_ChinhSach, webKey: WebKeys.RealState };
    
    // Tab mẫu biểu hợp đồng - Mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_MauBieu] = { type: PermissionTypes.Tab, name: 'Mẫu biểu', parentKey: PermissionRealStateConst.RealStateMoBan_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_MauBieu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMoBan_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_MauBieu_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMoBan_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_MauBieu_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStateMoBan_MauBieu, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_MauBieu_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMoBan_MauBieu, webKey: WebKeys.RealState };
    
    // Tab hồ sơ dự án - Mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo] = { type: PermissionTypes.Tab, name: 'Hồ sơ', parentKey: PermissionRealStateConst.RealStateMoBan_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_XemFile] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_Tai] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMoBan_HoSo_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMoBan_HoSo, webKey: WebKeys.RealState };
    
    // Menu Quản lý giao dịch cọc
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuQLGiaoDichCoc] = { type: PermissionTypes.Menu, name: 'Quản lý giao dịch cọc', parentKey: null, webKey: WebKeys.RealState, icon: "pi pi-map" };
    // Module Sổ lệnh
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh] = { type: PermissionTypes.Menu, name: 'Sổ lệnh', parentKey: PermissionRealStateConst.RealStateMenuQLGiaoDichCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuSoLenh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuSoLenh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_GiaHanGiuCho] = { type: PermissionTypes.ButtonAction, name: 'Gia hạn thời gian giữ chỗ', parentKey: PermissionRealStateConst.RealStateMenuSoLenh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuSoLenh, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin sổ lệnh', parentKey: PermissionRealStateConst.RealStateMenuQLGiaoDichCoc, webKey: WebKeys.RealState };
    // Tab thông tin chung
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung_DoiLoaiHD] = { type: PermissionTypes.ButtonAction, name: 'Đổi loại hợp đồng', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung_DoiHinhThucThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Đổi hình thức thanh toán', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinChung, webKey: WebKeys.RealState };
    // Tab Thanh toán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan] = { type: PermissionTypes.Tab, name: 'Thanh toán', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt/ huỷ', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ThongTinThanhToan, webKey: WebKeys.RealState };
    // Tab Chính sách ưu đãi
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_CSUuDai] = { type: PermissionTypes.Tab, name: 'Chính sách ưu đãi', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_CSUuDai_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_CSUuDai, webKey: WebKeys.RealState };
    
    // Tab HSKH đăng ký
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy] = { type: PermissionTypes.Tab, name: 'HSKH đăng ký', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy_ChuyenOnline] = { type: PermissionTypes.ButtonAction, name: 'Chuyển online', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy_CapNhatHS] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hồ sơ', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy_DuyetHS] = { type: PermissionTypes.ButtonAction, name: 'Duyệt hồ sơ', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy_HuyDuyetHS] = { type: PermissionTypes.ButtonAction, name: 'Hủy duyệt hồ sơ', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_HSKHDangKy, webKey: WebKeys.RealState };
    
    // Tab Lịch sử - Sổ lệnh
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_ChiTiet, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuSoLenh_LichSu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuSoLenh_LichSu, webKey: WebKeys.RealState };

    // Xử lý đặt cọc 
    PermissionRealStateConfig[PermissionRealStateConst.RealStateGDC_XLDC] = { type: PermissionTypes.Menu, name: 'Xử lý đặt cọc', parentKey: PermissionRealStateConst.RealStateMenuQLGiaoDichCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateGDC_XLDC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateGDC_XLDC, webKey: WebKeys.RealState };
        
    // Hợp đồng
    PermissionRealStateConfig[PermissionRealStateConst.RealStateGDC_HDDC] = { type: PermissionTypes.Menu, name: 'Hợp đồng đặt cọc', parentKey: PermissionRealStateConst.RealStateMenuQLGiaoDichCoc, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStateGDC_HDDC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateGDC_HDDC, webKey: WebKeys.RealState };
    // Menu Quản lý phê duyệt
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuQLPD] = { type: PermissionTypes.Menu, name: 'Quản lý phê duyệt', parentKey: null, webKey: WebKeys.RealState, icon: "pi pi-check-circle" };
    // Phê duyệt dự án
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPDDA] = { type: PermissionTypes.Menu, name: 'Phê duyệt dự án', parentKey: PermissionRealStateConst.RealStateMenuQLPD, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDDA_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuPDDA, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDDA_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMenuPDDA, webKey: WebKeys.RealState };
    // Phê duyệt phân phối
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPDPP] = { type: PermissionTypes.Menu, name: 'Phê duyệt phân phối', parentKey: PermissionRealStateConst.RealStateMenuQLPD, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDPP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuPDPP, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDPP_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMenuPDPP, webKey: WebKeys.RealState };
    // Phê duyệt mở bán
    PermissionRealStateConfig[PermissionRealStateConst.RealStateMenuPDMB] = { type: PermissionTypes.Menu, name: 'Phê duyệt mở bán', parentKey: PermissionRealStateConst.RealStateMenuQLPD, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDMB_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionRealStateConst.RealStateMenuPDMB, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealStatePDMB_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionRealStateConst.RealStateMenuPDMB, webKey: WebKeys.RealState };
    
    // Báo cáo
    PermissionRealStateConfig[PermissionRealStateConst.RealState_Menu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.RealState, icon: 'pi pi-file-export' };

    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionRealStateConst.RealState_Menu_BaoCao };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_QuanTri_TQBangHangDuAn] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng quan bảng hàng dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_QuanTri, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_QuanTri_TH_TienVeDA] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp tiền về dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_QuanTri, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_QuanTri_TH_CacKhoanGD] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản giao dịch', parentKey: PermissionRealStateConst.RealState_BaoCao_QuanTri, webKey: WebKeys.RealState };
    
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_VanHanh] = { type: PermissionTypes.Page, name: 'Báo cáo vận hành', parentKey: PermissionRealStateConst.RealState_Menu_BaoCao };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_VanHanh_TQBangHangDuAn] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng quan bảng hàng dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_VanHanh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_VanHanh_TH_TienVeDA] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp tiền về dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_VanHanh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_VanHanh_TH_CacKhoanGD] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản giao dịch', parentKey: PermissionRealStateConst.RealState_BaoCao_VanHanh, webKey: WebKeys.RealState };

    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_KinhDoanh] = { type: PermissionTypes.Page, name: 'Báo cáo kinh doanh', parentKey: PermissionRealStateConst.RealState_Menu_BaoCao };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_KinhDoanh_TQBangHangDuAn] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng quan bảng hàng dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_KinhDoanh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_KinhDoanh_TH_TienVeDA] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp tiền về dự án', parentKey: PermissionRealStateConst.RealState_BaoCao_KinhDoanh, webKey: WebKeys.RealState };
    PermissionRealStateConfig[PermissionRealStateConst.RealState_BaoCao_KinhDoanh_TH_CacKhoanGD] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản giao dịch', parentKey: PermissionRealStateConst.RealState_BaoCao_KinhDoanh, webKey: WebKeys.RealState };
    
    export default PermissionRealStateConfig;

