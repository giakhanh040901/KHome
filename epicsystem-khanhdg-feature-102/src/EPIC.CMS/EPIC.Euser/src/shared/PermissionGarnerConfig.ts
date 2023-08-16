import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionGarnerConfig = {};
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
    //
    PermissionGarnerConfig[PermissionGarnerConst.GarnerPageDashboard] = { type: PermissionTypes.Menu, name: 'Dashboard tổng quan', parentKey: null, icon: "pi pi-fw pi-home" };
    //
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuSetting] = { type: PermissionTypes.Menu, name: 'Cài đặt', parentKey: null, icon: "pi pi-fw pi-cog" };

    // Quản lý chủ đầu tư
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuChuDT] = { type: PermissionTypes.Menu, name: 'Chủ đầu tư', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };

    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuChuDT, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuChuDT, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerMenuChuDT, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_ThongTinChuDauTu] = { type: PermissionTypes.Page, name: 'Thông tin chủ đầu tư', parentKey: PermissionGarnerConst.GarnerMenuChuDT, webKey: WebKeys.Garner };

    // // Thông tin chi tiết chủ đầu tư
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerChuDT_ThongTinChuDauTu, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerChuDT_ThongTinChung, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerChuDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerChuDT_ThongTinChung, webKey: WebKeys.Garner };
    
    // Cấu hình ngày nghỉ lễ
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuCauHinhNNL] = { type: PermissionTypes.Menu, name: 'Cấu hình ngày nghỉ lễ', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhNNL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuCauHinhNNL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhNNL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerMenuCauHinhNNL, webKey: WebKeys.Garner };
   
    // Chính sách mẫu = CMS
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuChinhSachMau] = { type: PermissionTypes.Menu, name: 'Chính sách mẫu', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (Chính sách)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_KyHan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_KyHan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Kỳ hạn)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_KyHan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa (Kỳ hạn)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };

    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_HopDongMau_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm hợp đồng mẫu', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_HopDongMau_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hợp đồng mẫu', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_HopDongMau_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Hợp đồng mẫu)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerCSM_HopDongMau_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa (Hợp đồng mẫu)', parentKey: PermissionGarnerConst.GarnerMenuChinhSachMau, webKey: WebKeys.Garner };

 // Cấu hình mã hợp đồng
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuCauHinhMaHD] = { type: PermissionTypes.Menu, name: 'Cấu hình mã hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhMaHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuCauHinhMaHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhMaHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuCauHinhMaHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhMaHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionGarnerConst.GarnerMenuCauHinhMaHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerCauHinhMaHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerMenuCauHinhMaHD, webKey: WebKeys.Garner };

 // mẫu hợp đồng
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuMauHD] = { type: PermissionTypes.Menu, name: 'Mẫu hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_TaiFileDoanhNghiep] = { type: PermissionTypes.ButtonAction, name: 'Tải file d/nghiệp', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_TaiFileCaNhan] = { type: PermissionTypes.ButtonAction, name: 'Tải file cá nhân', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };
 PermissionGarnerConfig[PermissionGarnerConst.GarnerMauHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerMenuMauHD, webKey: WebKeys.Garner };

    // Module Mô tả chung
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuMoTaChung] = { type: PermissionTypes.Menu, name: 'Mô tả chung', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuMoTaChung, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuMoTaChung, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThongTinMTC] = { type: PermissionTypes.Page, name: 'Thông tin ', parentKey: PermissionGarnerConst.GarnerMenuMoTaChung, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThongTinMTC_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa mô tả chung', parentKey: PermissionGarnerConst.GarnerMTC_ThongTinMTC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThongTinMTC_NoiBat] = { type: PermissionTypes.ButtonAction, name: 'Đặt nổi bật', parentKey: PermissionGarnerConst.GarnerMTC_ThongTinMTC, webKey: WebKeys.Garner };
    
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThongTinMTC_Xem] = { type: PermissionTypes.ButtonAction, name: 'Xem chi tiết', parentKey: PermissionGarnerConst.GarnerMTC_ThongTinMTC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMTC_ThongTinMTC_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionGarnerConst.GarnerMTC_ThongTinMTC, webKey: WebKeys.Garner };

    // Tổng thầu
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuTongThau] = { type: PermissionTypes.Menu, name: 'Tổng thầu', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuTongThau, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuTongThau, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerMenuTongThau, webKey: WebKeys.Garner };
        
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_ThongTinTongThau] = { type: PermissionTypes.Page, name: 'Thông tin tổng thầu', parentKey: PermissionGarnerConst.GarnerMenuTongThau, webKey: WebKeys.Garner };

    // //Tab Thông tin chung
    //     PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerTongThau_ThongTinTongThau, webKey: WebKeys.Garner };
    //     PermissionGarnerConfig[PermissionGarnerConst.GarnerTongThau_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerTongThau_ThongTinChung, webKey: WebKeys.Garner };
    
    // Đại lý
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuDaiLy] = { type: PermissionTypes.Menu, name: 'Đại lý', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/ Đóng', parentKey: PermissionGarnerConst.GarnerMenuDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionGarnerConst.GarnerMenuDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_ThongTinDaiLy] = { type: PermissionTypes.Page, name: 'Thông tin đại lý', parentKey: PermissionGarnerConst.GarnerMenuDaiLy, webKey: WebKeys.Garner };
    //Tab Thông tin chung
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerDaiLy_ThongTinDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerDaiLy_ThongTinChung, webKey: WebKeys.Garner };
    //Tab Tài khoản đăng nhập 
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_TKDN] = { type: PermissionTypes.Tab, name: 'Tài khoản đăng nhập', parentKey: PermissionGarnerConst.GarnerDaiLy_ThongTinDaiLy, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_TKDN_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerDaiLy_TKDN, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerDaiLy_TKDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerDaiLy_TKDN, webKey: WebKeys.Garner };
    
    // Hình ảnh
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuHinhAnh] = { type: PermissionTypes.Menu, name: 'Hình ảnh', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_DuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng / Huỷ duyệt đăng', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi Tiết', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerMenuHinhAnh, webKey: WebKeys.Garner };
    
    // Thông báo hệ thống
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Thông báo hệ thống', parentKey: PermissionGarnerConst.GarnerMenuSetting, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuThongBaoHeThong_CaiDat] = { type: PermissionTypes.ButtonAction, name: 'Cài đặt', parentKey: PermissionGarnerConst.GarnerMenuThongBaoHeThong, webKey: WebKeys.Garner };


// Hợp đồng phân phối
PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuHDPP] = { type: PermissionTypes.Menu, name: 'Hợp đồng phân phối', parentKey: null, webKey: WebKeys.Garner, icon: "pi pi-book" };
    //Sổ lệnh
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh] = { type: PermissionTypes.Menu, name: 'Sổ lệnh', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh, webKey: WebKeys.Garner };
    //
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT] = { type: PermissionTypes.Page, name: 'Thông tin sổ lệnh', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
        //Tab Thông tin chung
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_XemChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        //
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_DoiKyHan] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_DoiMaGT] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật mã giới thiệu', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật thông tin khách hàng', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật Số tiền đầu tư', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Garner };
        
        //Tab Thông tin thanh toán
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan] = { type: PermissionTypes.Tab, name: 'Thông tin thanh toán', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết thanh toán', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Huỷ phê duyệt', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Garner };

        //Tab HSKH đăng ký
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy] = { type: PermissionTypes.Tab, name: 'HSKH đăng ký', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        //
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM] = { type: PermissionTypes.ButtonAction, name: 'Tải hồ sơ mẫu', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT] = { type: PermissionTypes.ButtonAction, name: 'Tải hồ sơ chữ ký điện tử', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS] = { type: PermissionTypes.ButtonAction, name: 'Upload hồ sơ', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen] = { type: PermissionTypes.ButtonAction, name: 'Xem hồ sơ tải lên', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline] = { type: PermissionTypes.ButtonAction, name: 'Chuyển Online', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hồ sơ', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu] = { type: PermissionTypes.ButtonAction, name: 'Ký điện tử', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu] = { type: PermissionTypes.ButtonAction, name: 'Hủy ký điện tử', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Duyệt hồ sơ / Hủy hồ sơ', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Garner };
        
        //Tab Lợi nhuận
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSoLenh_LoiNhuan] = { type: PermissionTypes.Tab, name: 'Lợi nhuận', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSoLenh_LoiNhuan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSoLenh_LoiNhuan, webKey: WebKeys.Garner };
       
        //Tab Lịch sử hd
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSoLenh_LichSuHD] = { type: PermissionTypes.Tab, name: 'Lịch sử hợp đồng', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSoLenh_LichSuHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSoLenh_LichSuHD, webKey: WebKeys.Garner };

        //Tab Trái tức
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TraiTuc] = { type: PermissionTypes.Tab, name: 'Trái tức', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TraiTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TraiTuc, webKey: WebKeys.Garner };
        
    // Xử lý hợp đồng
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLHD] = { type: PermissionTypes.Menu, name: 'Xử lý hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_XLHD, webKey: WebKeys.Garner };
       
    // Hợp đồng
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HopDong] = { type: PermissionTypes.Menu, name: 'Hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_HopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauTaiTuc] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu tái tục', parentKey: PermissionGarnerConst.GarnerHDPP_HopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauRutVon] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu rút vốn', parentKey: PermissionGarnerConst.GarnerHDPP_HopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HopDong_PhongToaHopDong] = { type: PermissionTypes.ButtonAction, name: 'Phong tỏa hợp đồng', parentKey: PermissionGarnerConst.GarnerHDPP_HopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung] = { type: PermissionTypes.ButtonAction, name: 'Nhận hợp đồng bản cứng', parentKey: PermissionGarnerConst.GarnerHDPP_HopDong, webKey: WebKeys.Garner };

    // HDDH: XỬ LÝ RÚT TIỀN
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien] = { type: PermissionTypes.Menu, name: 'Xử lý rút tiền', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_XLRutTien, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionGarnerConst.GarnerHDPP_XLRutTien, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien_ChiTienTD] = { type: PermissionTypes.ButtonAction, name: 'Chi tiền tự động', parentKey: PermissionGarnerConst.GarnerHDPP_XLRutTien, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien_ChiTienTC] = { type: PermissionTypes.ButtonAction, name: 'Chi tiền thủ công', parentKey: PermissionGarnerConst.GarnerHDPP_XLRutTien, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_XLRutTien_HuyYeuCau] = { type: PermissionTypes.ButtonAction, name: 'Hủy yêu cầu', parentKey: PermissionGarnerConst.GarnerHDPP_XLRutTien, webKey: WebKeys.Garner };

    // Chi tra loi tuc
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC] = { type: PermissionTypes.Menu, name: 'Chi trả lợi tức', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_CTLC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC_ThongTinChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionGarnerConst.GarnerHDPP_CTLC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC_DuyetChiTD] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi tự động', parentKey: PermissionGarnerConst.GarnerHDPP_CTLC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC_DuyetChiTC] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi tiền thủ công', parentKey: PermissionGarnerConst.GarnerHDPP_CTLC, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_CTLC_LapDSChiTra] = { type: PermissionTypes.ButtonAction, name: 'Lập DS chi trả', parentKey: PermissionGarnerConst.GarnerHDPP_CTLC, webKey: WebKeys.Garner };
    // LỊCH SỬ TÍCH LŨY
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_LSTL] = { type: PermissionTypes.Menu, name: 'Lịch sử tích lũy', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_LSTL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_LSTL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_LSTL_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionGarnerConst.GarnerHDPP_LSTL, webKey: WebKeys.Garner };


    // Giao nhận hợp đồng
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong] = { type: PermissionTypes.Menu, name: 'Giao nhận hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_XuatHopDong] = { type: PermissionTypes.ButtonAction, name: 'Xuất hợp đồng', parentKey: PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet, webKey: WebKeys.Garner };
    
    
    // Phong toả, giải toả
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa] = { type: PermissionTypes.Menu, name: 'Phong toả giải toả', parentKey: PermissionGarnerConst.GarnerMenuHDPP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_GiaiToaHD] = { type: PermissionTypes.ButtonAction, name: 'Giải toả hợp đồng', parentKey: PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa] = { type: PermissionTypes.Page, name: 'Thông tin phong toả giải toả', parentKey: PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa, webKey: WebKeys.Garner };

    // Hợp đồng đáo hạn
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HDDH] = { type: PermissionTypes.Menu, name: 'Hợp đồng đáo hạn', parentKey: PermissionGarnerConst.GarnerMenuHDPP };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HDDH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerHDPP_HDDH };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HDDH_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionGarnerConst.GarnerHDPP_HDDH };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HDDH_LapDSChiTra] = { type: PermissionTypes.ButtonAction, name: 'Lập danh sách chi trả', parentKey: PermissionGarnerConst.GarnerHDPP_HDDH };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerHDPP_HDDH_DuyetKhongChi] = { type: PermissionTypes.ButtonAction, name: 'Duyệt không chi', parentKey: PermissionGarnerConst.GarnerHDPP_HDDH };
    
        
    
// Quản lý đầu tư
PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuQLDT] = { type: PermissionTypes.Menu, name: 'Quản lý đầu tư', parentKey: null, webKey: WebKeys.Garner, icon: "pi pi-map" };
    //Sản phẩm đầu tư
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuSPTL] = { type: PermissionTypes.Menu, name: 'Sản phẩm tích lũy', parentKey: PermissionGarnerConst.GarnerMenuQLDT, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DongOrMo] = { type: PermissionTypes.ButtonAction, name: 'Đóng / Mở', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };
    

    PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_ThongTinSPTL] = { type: PermissionTypes.Page, name: 'Thông tin sản phẩm tích lũy', parentKey: PermissionGarnerConst.GarnerMenuSPTL, webKey: WebKeys.Garner };

        //Tab Thông tin chung
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinSPTL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinChung, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinChung, webKey: WebKeys.Garner };
        
        //Tab đại lý phân phối
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP] = { type: PermissionTypes.Tab, name: 'Đại lý phân phối', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinSPTL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSPTL_DLPP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerSPTL_DLPP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerSPTL_DLPP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng / Bỏ duyệt đăng', parentKey: PermissionGarnerConst.GarnerSPTL_DLPP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPTL_DLPP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerSPTL_DLPP, webKey: WebKeys.Garner };

        //tai san dam bao
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB] = { type: PermissionTypes.Tab, name: 'Tài sản đảm bảo', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinSPTL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_Preview] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_DownloadFile] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TSDB_DeleteFile] = { type: PermissionTypes.ButtonAction, name: 'Xoá file', parentKey: PermissionGarnerConst.GarnerSPDT_TSDB, webKey: WebKeys.Garner };

        //Tab Hồ sơ pháp lý
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL] = { type: PermissionTypes.Tab, name: 'Hồ sơ pháp lý', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinSPTL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_Preview] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_DownloadFile] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_HSPL_DeleteFile] = { type: PermissionTypes.ButtonAction, name: 'Xoá file', parentKey: PermissionGarnerConst.GarnerSPDT_HSPL, webKey: WebKeys.Garner };

        //Tab Tin tức sản phẩm
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP] = { type: PermissionTypes.Tab, name: 'Tin tức sản phẩm', parentKey: PermissionGarnerConst.GarnerSPTL_ThongTinSPTL, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerSPDT_TTSP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerSPDT_TTSP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerSPDT_TTSP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng / Bỏ duyệt đăng', parentKey: PermissionGarnerConst.GarnerSPDT_TTSP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerSPDT_TTSP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerSPDT_TTSP, webKey: WebKeys.Garner };
        
    //Phân phối đầu tư
    PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuPPSP] = { type: PermissionTypes.Menu, name: 'Phân phối sản phẩm', parentKey: PermissionGarnerConst.GarnerMenuQLDT, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_DongOrMo] = { type: PermissionTypes.ButtonAction, name: 'Đóng / Mở', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };
    PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật / Tắt show app', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };
    // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };

    PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ThongTinPPSP] = { type: PermissionTypes.Page, name: 'Thông tin phân phối sản phẩm', parentKey: PermissionGarnerConst.GarnerMenuPPSP, webKey: WebKeys.Garner };

        //Tab Thông tin chung
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinChung, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinChung, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinChung, webKey: WebKeys.Garner };
        
        //Tab Tổng quan
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_TongQuan] = { type: PermissionTypes.Tab, name: 'Tổng quan', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_TongQuan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_TongQuan, webKey: WebKeys.Garner };

        //Tab Chính sách
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật ? Tắt show App (Chính sách)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_ChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá chính sách', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };

        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_KyHan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_KyHan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Kỳ hạn)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_KyHan_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật / Tắt show App (Kỳ hạn)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_KyHan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (kỳ hạn)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };

        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongMau_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm hợp đồng mẫu', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongMau_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hợp đồng mẫu', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongMau_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Hợp đồng mẫu)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongMau_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật / Tắt show App (Hợp đồng mẫu)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongMau_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (Hợp đồng mẫu)', parentKey: PermissionGarnerConst.GarnerPPSP_ChinhSach, webKey: WebKeys.Garner };

        // // Tab File chính sách
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_FileChinhSach] = { type: PermissionTypes.Tab, name: 'File chính sách', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_FileChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerPPSP_FileChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_FileChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerPPSP_FileChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_FileChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_FileChinhSach, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_FileChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerPPSP_FileChinhSach, webKey: WebKeys.Garner };
        
        //tab bang gia
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TTCT_BangGia] = { type: PermissionTypes.Menu, name: 'Bảng giá', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TTCT_BangGia_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.Garner_TTCT_BangGia, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TTCT_BangGia_ImportExcelBG] = { type: PermissionTypes.ButtonAction, name: 'Import file excel bảng giá', parentKey: PermissionGarnerConst.Garner_TTCT_BangGia, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TTCT_BangGia_DownloadFileMau] = { type: PermissionTypes.ButtonAction, name: 'Download file mẫu', parentKey: PermissionGarnerConst.Garner_TTCT_BangGia, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TTCT_BangGia_XoaBangGia] = { type: PermissionTypes.ButtonAction, name: 'Xóa bảng giá', parentKey: PermissionGarnerConst.Garner_TTCT_BangGia, webKey: WebKeys.Garner };
       
        // Tab Mẫu hợp đồng
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauHopDong] = { type: PermissionTypes.Tab, name: 'Mẫu hợp đồng', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerPPSP_MauHopDong, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauHopDong_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerPPSP_MauHopDong, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauHopDong_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_MauHopDong, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauHopDong_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerPPSP_MauHopDong, webKey: WebKeys.Garner };
    
        // // Tab Hợp đồng phân phối
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi] = { type: PermissionTypes.Tab, name: 'Hợp đồng phân phối', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerPPSP_HopDongPhanPhoi, webKey: WebKeys.Garner };

        // // Tab Mẫu giao nhận HĐ
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD] = { type: PermissionTypes.Tab, name: 'Mẫu giao nhận hợp đồng', parentKey: PermissionGarnerConst.GarnerPPSP_ThongTinPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_KichHoat] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt', parentKey: PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD, webKey: WebKeys.Garner };

        // Quản lý phê duyệt
        PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuQLPD] = { type: PermissionTypes.Menu, name: 'Quản lý phê duyệt', parentKey: null, webKey: WebKeys.Garner, icon: "pi pi-check-circle" };
        //Phê duyệt sản phẩm đầu tư
        PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuPDSPTL] = { type: PermissionTypes.Menu, name: 'Phê duyệt sản phẩm đầu tư', parentKey: PermissionGarnerConst.GarnerMenuQLPD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPDSPTL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuPDSPTL, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPDSPTL_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionGarnerConst.GarnerMenuPDSPTL, webKey: WebKeys.Garner };
        //Phê duyệt phân phối đầu tư
        PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuPDPPSP] = { type: PermissionTypes.Menu, name: 'Phê duyệt phân phối sản phẩm', parentKey: PermissionGarnerConst.GarnerMenuQLPD, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPDPPSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuPDPPSP, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.GarnerPDPPSP_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionGarnerConst.GarnerMenuPDPPSP, webKey: WebKeys.Garner };
        // //Phê duyệt yêu cầu tái tục
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuPDYCTT] = { type: PermissionTypes.Menu, name: 'Phê duyệt yêu cầu tái tục', parentKey: PermissionGarnerConst.GarnerMenuQLPD, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPDYCTT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuPDYCTT, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPDYCTT_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionGarnerConst.GarnerMenuPDYCTT, webKey: WebKeys.Garner };
        // //Phê duyệt yêu cầu rút vốn
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerMenuPDYCRV] = { type: PermissionTypes.Menu, name: 'Phê duyệt yêu cầu rút vốn', parentKey: PermissionGarnerConst.GarnerMenuQLPD, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPDYCRV_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionGarnerConst.GarnerMenuPDYCRV, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPDYCRV_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionGarnerConst.GarnerMenuPDYCRV, webKey: WebKeys.Garner };
        // PermissionGarnerConfig[PermissionGarnerConst.GarnerPDYCRV_ChiTietHD] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết hợp đồng', parentKey: PermissionGarnerConst.GarnerMenuPDYCRV, webKey: WebKeys.Garner };

        // Báo cáo
        PermissionGarnerConfig[PermissionGarnerConst.Garner_Menu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.Garner, icon: 'pi pi-file-export' };

        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionGarnerConst.Garner_Menu_BaoCao };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_QuanTri, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_QuanTri, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri_THSanPhamTichLuy] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các sản phẩm tích luỹ', parentKey: PermissionGarnerConst.Garner_BaoCao_QuanTri, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri_QuanTriTH] = { type: PermissionTypes.ButtonAction, name: 'B.C Quản trị tổng hợp', parentKey: PermissionGarnerConst.Garner_BaoCao_QuanTri, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_QuanTri_DauTuBanHo] = { type: PermissionTypes.ButtonAction, name: 'B.C Các khoản đầu tư bán hộ', parentKey: PermissionGarnerConst.Garner_BaoCao_QuanTri, webKey: WebKeys.Garner };

        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh] = { type: PermissionTypes.Page, name: 'Báo cáo vận hành', parentKey: PermissionGarnerConst.Garner_Menu_BaoCao };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_VanHanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_VanHanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh_THSanPhamTichLuy] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các sản phẩm tích luỹ', parentKey: PermissionGarnerConst.Garner_BaoCao_VanHanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh_QuanTriTH] = { type: PermissionTypes.ButtonAction, name: 'B.C Quản trị tổng hợp', parentKey: PermissionGarnerConst.Garner_BaoCao_VanHanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_VanHanh_DauTuBanHo] = { type: PermissionTypes.ButtonAction, name: 'B.C Các khoản đầu tư bán hộ', parentKey: PermissionGarnerConst.Garner_BaoCao_VanHanh, webKey: WebKeys.Garner };

        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh] = { type: PermissionTypes.Page, name: 'Báo cáo kinh doanh', parentKey: PermissionGarnerConst.Garner_Menu_BaoCao };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_KinhDoanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionGarnerConst.Garner_BaoCao_KinhDoanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh_THSanPhamTichLuy] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các sản phẩm tích luỹ', parentKey: PermissionGarnerConst.Garner_BaoCao_KinhDoanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh_QuanTriTH] = { type: PermissionTypes.ButtonAction, name: 'B.C Quản trị tổng hợp', parentKey: PermissionGarnerConst.Garner_BaoCao_KinhDoanh, webKey: WebKeys.Garner };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_BaoCao_KinhDoanh_DauTuBanHo] = { type: PermissionTypes.ButtonAction, name: 'B.C Các khoản đầu tư bán hộ', parentKey: PermissionGarnerConst.Garner_BaoCao_KinhDoanh, webKey: WebKeys.Garner };

        PermissionGarnerConfig[PermissionGarnerConst.Garner_Menu_TruyVan] = { type: PermissionTypes.Menu, name: 'Truy vấn', parentKey: null, webKey: WebKeys.Garner, icon: 'pi pi-sync' };

        PermissionGarnerConfig[PermissionGarnerConst.Garner_TruyVan_ThuTien_NganHang] = { type: PermissionTypes.Page, name: 'Thu tiền ngân hàng', parentKey: PermissionGarnerConst.Garner_Menu_TruyVan };
        PermissionGarnerConfig[PermissionGarnerConst.Garner_TruyVan_ChiTien_NganHang] = { type: PermissionTypes.Page, name: 'Chi tiền ngân hàng', parentKey: PermissionGarnerConst.Garner_Menu_TruyVan };

export default PermissionGarnerConfig;