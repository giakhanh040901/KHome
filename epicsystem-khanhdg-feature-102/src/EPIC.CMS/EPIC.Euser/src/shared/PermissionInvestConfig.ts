import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionInvestConfig = {};
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
    //
    PermissionInvestConfig[PermissionInvestConst.InvestPageDashboard] = { type: PermissionTypes.Menu, name: 'Dashboard tổng quan', parentKey: null, icon: "pi pi-fw pi-home" };
    //
    PermissionInvestConfig[PermissionInvestConst.InvestMenuSetting] = { type: PermissionTypes.Menu, name: 'Cài đặt', parentKey: null, icon: "pi pi-fw pi-cog" };

    // Quản lý chủ đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuChuDT] = { type: PermissionTypes.Menu, name: 'Chủ đầu tư', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuChuDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuChuDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuChuDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_ThongTinChuDauTu] = { type: PermissionTypes.Page, name: 'Thông tin chủ đầu tư', parentKey: PermissionInvestConst.InvestMenuChuDT, webKey: WebKeys.Invest };

    // Thông tin chi tiết chủ đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestChuDT_ThongTinChuDauTu, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestChuDT_ThongTinChung, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestChuDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestChuDT_ThongTinChung, webKey: WebKeys.Invest };
    
    // Cấu hình ngày nghỉ lễ
    PermissionInvestConfig[PermissionInvestConst.InvestMenuCauHinhNNL] = { type: PermissionTypes.Menu, name: 'Cấu hình ngày nghỉ lễ', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhNNL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuCauHinhNNL, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhNNL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestMenuCauHinhNNL, webKey: WebKeys.Invest };
   
    // Chính sách mẫu = CMS
    PermissionInvestConfig[PermissionInvestConst.InvestMenuChinhSachMau] = { type: PermissionTypes.Menu, name: 'Chính sách mẫu', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (Chính sách)', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_KyHan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_KyHan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Kỳ hạn)', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCSM_KyHan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa (Kỳ hạn)', parentKey: PermissionInvestConst.InvestMenuChinhSachMau, webKey: WebKeys.Invest };

    // Cấu hình mã hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestMenuCauHinhMaHD] = { type: PermissionTypes.Menu, name: 'Cấu hình mã hợp đồng', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhMaHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuCauHinhMaHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhMaHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuCauHinhMaHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhMaHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionInvestConst.InvestMenuCauHinhMaHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestCauHinhMaHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuCauHinhMaHD, webKey: WebKeys.Invest };

    // mẫu hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestMenuMauHD] = { type: PermissionTypes.Menu, name: 'Mẫu hợp đồng', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_TaiFileDoanhNghiep] = { type: PermissionTypes.ButtonAction, name: 'Tải file d/nghiệp', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_TaiFileCaNhan] = { type: PermissionTypes.ButtonAction, name: 'Tải file cá nhân', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật ', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMauHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuMauHD, webKey: WebKeys.Invest };

    // Tổng thầu
    PermissionInvestConfig[PermissionInvestConst.InvestMenuTongThau] = { type: PermissionTypes.Menu, name: 'Tổng thầu', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuTongThau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuTongThau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuTongThau, webKey: WebKeys.Invest };
        
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_ThongTinTongThau] = { type: PermissionTypes.Page, name: 'Thông tin tổng thầu', parentKey: PermissionInvestConst.InvestMenuTongThau, webKey: WebKeys.Invest };

    //Tab Thông tin chung
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestTongThau_ThongTinTongThau, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestTongThau_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestTongThau_ThongTinChung, webKey: WebKeys.Invest };
    
    // Đại lý
    PermissionInvestConfig[PermissionInvestConst.InvestMenuDaiLy] = { type: PermissionTypes.Menu, name: 'Đại lý', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/ Đóng', parentKey: PermissionInvestConst.InvestMenuDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionInvestConst.InvestMenuDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_ThongTinDaiLy] = { type: PermissionTypes.Page, name: 'Thông tin đại lý', parentKey: PermissionInvestConst.InvestMenuDaiLy, webKey: WebKeys.Invest };
    
    //Tab Thông tin chung
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestDaiLy_ThongTinDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestDaiLy_ThongTinChung, webKey: WebKeys.Invest };
    
    //Tab Tài khoản đăng nhập 
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_TKDN] = { type: PermissionTypes.Tab, name: 'Tài khoản đăng nhập', parentKey: PermissionInvestConst.InvestDaiLy_ThongTinDaiLy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_TKDN_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestDaiLy_TKDN, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestDaiLy_TKDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestDaiLy_TKDN, webKey: WebKeys.Invest };
    
    // Hình ảnh
    PermissionInvestConfig[PermissionInvestConst.InvestMenuHinhAnh] = { type: PermissionTypes.Menu, name: 'Hình ảnh', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_DuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng / Huỷ duyệt đăng', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi Tiết', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuHinhAnh, webKey: WebKeys.Invest };
    
    // Thông báo hệ thống
    PermissionInvestConfig[PermissionInvestConst.InvestMenuThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Thông báo hệ thống', parentKey: PermissionInvestConst.InvestMenuSetting, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestMenuThongBaoHeThong_CaiDat] = { type: PermissionTypes.ButtonAction, name: 'Cài đặt', parentKey: PermissionInvestConst.InvestMenuThongBaoHeThong, webKey: WebKeys.Invest };


    // Hợp đồng phân phối
    PermissionInvestConfig[PermissionInvestConst.InvestMenuHDPP] = { type: PermissionTypes.Menu, name: 'Hợp đồng phân phối', parentKey: null, webKey: WebKeys.Invest, icon: "pi pi-book" };

    //Sổ lệnh
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh] = { type: PermissionTypes.Menu, name: 'Sổ lệnh', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_SoLenh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestHDPP_SoLenh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionInvestConst.InvestHDPP_SoLenh, webKey: WebKeys.Invest };
    //
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT] = { type: PermissionTypes.Page, name: 'Thông tin sổ lệnh', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    //Tab Thông tin chung
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC_XemChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Invest };
    //
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC_DoiMaGT] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật mã giới thiệu', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC_DoiTTKhachHang] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật thông tin KH', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật Số tiền đầu tư', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC, webKey: WebKeys.Invest };
    
    //Tab Thông tin thanh toán
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan] = { type: PermissionTypes.Tab, name: 'Thông tin thanh toán', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết thanh toán', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Huỷ phê duyệt', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan, webKey: WebKeys.Invest };

    //Tab HSKH đăng ký
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy] = { type: PermissionTypes.Tab, name: 'HSKH đăng ký', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    //
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM] = { type: PermissionTypes.ButtonAction, name: 'Tải hồ sơ mẫu', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT] = { type: PermissionTypes.ButtonAction, name: 'Tải hồ sơ chữ ký điện tử', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS] = { type: PermissionTypes.ButtonAction, name: 'Upload hồ sơ', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen] = { type: PermissionTypes.ButtonAction, name: 'Xem hồ sơ tải lên', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline] = { type: PermissionTypes.ButtonAction, name: 'Chuyển Online', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật hồ sơ', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu] = { type: PermissionTypes.ButtonAction, name: 'Ký điện tử', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu] = { type: PermissionTypes.ButtonAction, name: 'Hủy ký điện tử', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Duyệt hồ sơ / Hủy hồ sơ', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy, webKey: WebKeys.Invest };
    
    //Tab Lợi nhuận
    PermissionInvestConfig[PermissionInvestConst.InvestSoLenh_LoiNhuan] = { type: PermissionTypes.Tab, name: 'Lợi nhuận', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSoLenh_LoiNhuan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestSoLenh_LoiNhuan, webKey: WebKeys.Invest };
    
    //Tab Lịch sử hd
    PermissionInvestConfig[PermissionInvestConst.InvestSoLenh_LichSuHD] = { type: PermissionTypes.Tab, name: 'Lịch sử hợp đồng', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSoLenh_LichSuHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestSoLenh_LichSuHD, webKey: WebKeys.Invest };

    //Tab Trái tức
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TraiTuc] = { type: PermissionTypes.Tab, name: 'Trái tức', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TraiTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TraiTuc, webKey: WebKeys.Invest };
        
    // Xử lý hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLHD] = { type: PermissionTypes.Menu, name: 'Xử lý hợp đồng', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_XLHD, webKey: WebKeys.Invest };
       
    // Hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HopDong] = { type: PermissionTypes.Menu, name: 'Hợp đồng', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_HopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HopDong_YeuCauTaiTuc] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu tái tục', parentKey: PermissionInvestConst.InvestHDPP_HopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HopDong_YeuCauRutVon] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu rút vốn', parentKey: PermissionInvestConst.InvestHDPP_HopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HopDong_PhongToaHopDong] = { type: PermissionTypes.ButtonAction, name: 'Phong tỏa hợp đồng', parentKey: PermissionInvestConst.InvestHDPP_HopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung] = { type: PermissionTypes.ButtonAction, name: 'Nhận hợp đồng bản cứng', parentKey: PermissionInvestConst.InvestHDPP_HopDong, webKey: WebKeys.Invest };


    // Giao nhận hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong] = { type: PermissionTypes.Menu, name: 'Giao nhận hợp đồng', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_GiaoNhanHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionInvestConst.InvestHDPP_GiaoNhanHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_XuatHopDong] = { type: PermissionTypes.ButtonAction, name: 'Xuất hợp đồng', parentKey: PermissionInvestConst.InvestHDPP_GiaoNhanHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionInvestConst.InvestHDPP_GiaoNhanHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_ThongTinChiTiet, webKey: WebKeys.Invest };
    
    // Phong toả, giải toả
    PermissionInvestConfig[PermissionInvestConst.InvestHopDong_PhongToaGiaiToa] = { type: PermissionTypes.Menu, name: 'Phong toả giải toả', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHopDong_PhongToaGiaiToa, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_GiaiToaHD] = { type: PermissionTypes.ButtonAction, name: 'Giải toả hợp đồng', parentKey: PermissionInvestConst.InvestHopDong_PhongToaGiaiToa, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa] = { type: PermissionTypes.Page, name: 'Thông tin phong toả giải toả', parentKey: PermissionInvestConst.InvestHopDong_PhongToaGiaiToa, webKey: WebKeys.Invest };

    // XỬ LÝ RÚT TIỀN
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT] = { type: PermissionTypes.Menu, name: 'Xử lý rút tiền', parentKey: PermissionInvestConst.InvestMenuHDPP };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_XLRT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionInvestConst.InvestHDPP_XLRT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT_DuyetChiTuDong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi tự động', parentKey: PermissionInvestConst.InvestHDPP_XLRT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT_DuyetChiThuCong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi thủ công', parentKey: PermissionInvestConst.InvestHDPP_XLRT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_XLRT_HuyYeuCau] = { type: PermissionTypes.ButtonAction, name: 'Hủy yêu cầu', parentKey: PermissionInvestConst.InvestHDPP_XLRT };
    
    // HỢP ĐỒNG ĐÁO HẠN
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH] = { type: PermissionTypes.Menu, name: 'Hợp đồng đáo hạn', parentKey: PermissionInvestConst.InvestMenuHDPP };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_LapDSChiTra] = { type: PermissionTypes.ButtonAction, name: 'Lập danh sách chi trả', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_DuyetChiTuDong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi tự động', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_DuyetChiThuCong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi thủ công', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_DuyetTaiTuc] = { type: PermissionTypes.ButtonAction, name: 'Duyệt tái tục', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDDH_ExportExcel] = { type: PermissionTypes.ButtonAction, name: 'Xuất Excel', parentKey: PermissionInvestConst.InvestHDPP_HDDH };
    
    // CHI TRẢ LỢI TỨC
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT] = { type: PermissionTypes.Menu, name: 'Chi trả lợi tức', parentKey: PermissionInvestConst.InvestMenuHDPP };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách chi trả', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_LapDSChiTra] = { type: PermissionTypes.ButtonAction, name: 'Lập danh sách chi trả', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_DuyetChiTuDong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi tự động', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_DuyetChiThuCong] = { type: PermissionTypes.ButtonAction, name: 'Duyệt chi thủ công', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_CTLT_ExportExcel] = { type: PermissionTypes.ButtonAction, name: 'Xuất Excel', parentKey: PermissionInvestConst.InvestHDPP_CTLT };
    
    // Tất toán hợp đồng
    // PermissionInvestConfig[PermissionInvestConst.InvestHDPP_TTHD] = { type: PermissionTypes.Menu, name: 'Tất toán hợp đồng', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    // PermissionInvestConfig[PermissionInvestConst.InvestHDPP_TTHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_TTHD, webKey: WebKeys.Invest };
    
    // LỊCH SỬ ĐẦU TƯ
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_LSDT] = { type: PermissionTypes.Menu, name: 'Lịch sử đầu tư', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_LSDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_LSDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_LSDT_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionInvestConst.InvestHDPP_LSDT, webKey: WebKeys.Invest };
    
    // HỢP ĐỒNG TÁI TỤC
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDTT] = { type: PermissionTypes.Menu, name: 'Hợp đồng tái tục', parentKey: PermissionInvestConst.InvestMenuHDPP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDTT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestHDPP_HDTT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDTT_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionInvestConst.InvestHDPP_HDTT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestHDPP_HDTT_HuyYeuCau] = { type: PermissionTypes.ButtonAction, name: 'Hủy yêu cầu', parentKey: PermissionInvestConst.InvestHDPP_HDTT, webKey: WebKeys.Invest };
           
    // Quản lý đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuQLDT] = { type: PermissionTypes.Menu, name: 'Quản lý đầu tư', parentKey: null, webKey: WebKeys.Invest, icon: "pi pi-map" };
    //Sản phẩm đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuSPDT] = { type: PermissionTypes.Menu, name: 'Sản phẩm đầu tư', parentKey: PermissionInvestConst.InvestMenuQLDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_Dong] = { type: PermissionTypes.ButtonAction, name: 'Đóng sản phẩm', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };
    

    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_ThongTinSPDT] = { type: PermissionTypes.Page, name: 'Thông tin sản phẩm đầu tư', parentKey: PermissionInvestConst.InvestMenuSPDT, webKey: WebKeys.Invest };

    //Tab Thông tin chung
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestSPDT_ThongTinSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestSPDT_ThongTinChung, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestSPDT_ThongTinChung, webKey: WebKeys.Invest };
    
    //Tab hình ảnh đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HADT] = { type: PermissionTypes.Tab, name: 'Hình ảnh đầu tư', parentKey: PermissionInvestConst.InvestSPDT_ThongTinSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HADT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestSPDT_HADT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HADT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestSPDT_HADT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HADT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestSPDT_HADT, webKey: WebKeys.Invest };

    //Tab Hồ sơ pháp lý
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL] = { type: PermissionTypes.Tab, name: 'Hồ sơ pháp lý', parentKey: PermissionInvestConst.InvestSPDT_ThongTinSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestSPDT_HSPL, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestSPDT_HSPL, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL_Preview] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionInvestConst.InvestSPDT_HSPL, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL_DownloadFile] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionInvestConst.InvestSPDT_HSPL, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_HSPL_DeleteFile] = { type: PermissionTypes.ButtonAction, name: 'Xoá file', parentKey: PermissionInvestConst.InvestSPDT_HSPL, webKey: WebKeys.Invest };

    //Tab Tin tức sản phẩm
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP] = { type: PermissionTypes.Tab, name: 'Tin tức sản phẩm', parentKey: PermissionInvestConst.InvestSPDT_ThongTinSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestSPDT_TTSP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestSPDT_TTSP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestSPDT_TTSP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng / Bỏ duyệt đăng', parentKey: PermissionInvestConst.InvestSPDT_TTSP, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestSPDT_TTSP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestSPDT_TTSP, webKey: WebKeys.Invest };
        
    //Phân phối đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuPPDT] = { type: PermissionTypes.Menu, name: 'Phân phối đầu tư', parentKey: PermissionInvestConst.InvestMenuQLDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_DongTam] = { type: PermissionTypes.ButtonAction, name: 'Đóng tạm / Mở', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật / Tắt show app', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ThongTinPPDT] = { type: PermissionTypes.Page, name: 'Thông tin phân phối đầu tư', parentKey: PermissionInvestConst.InvestMenuPPDT, webKey: WebKeys.Invest };

    //Tab Thông tin chung
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChiTiet] = { type: PermissionTypes.Form, name: 'Chi tiết', parentKey: PermissionInvestConst.InvestPPDT_ThongTinChung, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_ThongTinChung, webKey: WebKeys.Invest };
    
    //Tab Tổng quan
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan] = { type: PermissionTypes.Tab, name: 'Tổng quan', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_ChonAnh] = { type: PermissionTypes.ButtonAction, name: 'Chọn ảnh', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_ThemToChuc] = { type: PermissionTypes.ButtonAction, name: 'Thêm tổ chức', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_DanhSach_ToChuc] = { type: PermissionTypes.Table, name: 'Danh sách tổ chức', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_UploadFile] = { type: PermissionTypes.ButtonAction, name: 'Upload file', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_TongQuan_DanhSach_File] = { type: PermissionTypes.Table, name: 'Danh sách file', parentKey: PermissionInvestConst.InvestPPDT_TongQuan, webKey: WebKeys.Invest };

    //Tab Chính sách
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach] = { type: PermissionTypes.Tab, name: 'Chính sách', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Chính sách)', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật ? Tắt show App (Chính sách)', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_ChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá chính sách', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_KyHan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_KyHan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Huỷ (Kỳ hạn)', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_KyHan_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật / Tắt show App (Kỳ hạn)', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_KyHan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá (kỳ hạn)', parentKey: PermissionInvestConst.InvestPPDT_ChinhSach, webKey: WebKeys.Invest };

    // Tab File chính sách
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_FileChinhSach] = { type: PermissionTypes.Tab, name: 'File chính sách', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_FileChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestPPDT_FileChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_FileChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestPPDT_FileChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_FileChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_FileChinhSach, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_FileChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestPPDT_FileChinhSach, webKey: WebKeys.Invest };
    
    // Tab Mẫu hợp đồng
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauHopDong] = { type: PermissionTypes.Tab, name: 'Mẫu hợp đồng', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestPPDT_MauHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauHopDong_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestPPDT_MauHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauHopDong_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_MauHopDong, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauHopDong_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestPPDT_MauHopDong, webKey: WebKeys.Invest };

    // Tab Hợp đồng phân phối
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_HopDongPhanPhoi] = { type: PermissionTypes.Tab, name: 'Hợp đồng phân phối', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestPPDT_HopDongPhanPhoi, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestPPDT_HopDongPhanPhoi, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_HopDongPhanPhoi, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestPPDT_HopDongPhanPhoi, webKey: WebKeys.Invest };

    // Tab Mẫu giao nhận HĐ
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD] = { type: PermissionTypes.Tab, name: 'Mẫu giao nhận hợp đồng', parentKey: PermissionInvestConst.InvestPPDT_ThongTinPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestPPDT_MauGiaoNhanHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionInvestConst.InvestPPDT_MauGiaoNhanHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionInvestConst.InvestPPDT_MauGiaoNhanHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_KichHoat] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt', parentKey: PermissionInvestConst.InvestPPDT_MauGiaoNhanHD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionInvestConst.InvestPPDT_MauGiaoNhanHD, webKey: WebKeys.Invest };

    // Quản lý phê duyệt
    PermissionInvestConfig[PermissionInvestConst.InvestMenuQLPD] = { type: PermissionTypes.Menu, name: 'Quản lý phê duyệt', parentKey: null, webKey: WebKeys.Invest, icon: "pi pi-check-circle" };
    //Phê duyệt sản phẩm đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuPDSPDT] = { type: PermissionTypes.Menu, name: 'Phê duyệt sản phẩm đầu tư', parentKey: PermissionInvestConst.InvestMenuQLPD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDSPDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuPDSPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDSPDT_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionInvestConst.InvestMenuPDSPDT, webKey: WebKeys.Invest };
    //Phê duyệt phân phối đầu tư
    PermissionInvestConfig[PermissionInvestConst.InvestMenuPDPPDT] = { type: PermissionTypes.Menu, name: 'Phê duyệt phân phối đầu tư', parentKey: PermissionInvestConst.InvestMenuQLPD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDPPDT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuPDPPDT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDPPDT_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionInvestConst.InvestMenuPDPPDT, webKey: WebKeys.Invest };
    //Phê duyệt yêu cầu tái tục
    PermissionInvestConfig[PermissionInvestConst.InvestMenuPDYCTT] = { type: PermissionTypes.Menu, name: 'Phê duyệt yêu cầu tái tục', parentKey: PermissionInvestConst.InvestMenuQLPD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDYCTT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuPDYCTT, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDYCTT_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionInvestConst.InvestMenuPDYCTT, webKey: WebKeys.Invest };
    //Phê duyệt yêu cầu rút vốn
    PermissionInvestConfig[PermissionInvestConst.InvestMenuPDYCRV] = { type: PermissionTypes.Menu, name: 'Phê duyệt yêu cầu rút vốn', parentKey: PermissionInvestConst.InvestMenuQLPD, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDYCRV_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionInvestConst.InvestMenuPDYCRV, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDYCRV_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionInvestConst.InvestMenuPDYCRV, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.InvestPDYCRV_ChiTietHD] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết hợp đồng', parentKey: PermissionInvestConst.InvestMenuPDYCRV, webKey: WebKeys.Invest };

    // Báo cáo
    PermissionInvestConfig[PermissionInvestConst.Invest_Menu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.Invest, icon: 'pi pi-file-export' };

    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionInvestConst.Invest_Menu_BaoCao };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_THCMaBDS] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các mã BDS', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_DCDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Dự chi đến hạn', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_ThucChi] = { type: PermissionTypes.ButtonAction, name: 'B.C Thực chi', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_THCKDTVanHanhHVF] = { type: PermissionTypes.ButtonAction, name: 'B.C tổng hợp các khoản đầu tư VH HVF', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_SKTKNhaDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C sao kê tài khoản nhà đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_QuanTri_THCKDTBanHo] = { type: PermissionTypes.ButtonAction, name: 'B.C tổng hợp các khoản đầu tư bán hộ', parentKey: PermissionInvestConst.Invest_BaoCao_QuanTri, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh] = { type: PermissionTypes.Page, name: 'Báo cáo vận hành', parentKey: PermissionInvestConst.Invest_Menu_BaoCao };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_VanHanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh_THCMaBDS] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các mã BDS', parentKey: PermissionInvestConst.Invest_BaoCao_VanHanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh_DCDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Dự chi đến hạn', parentKey: PermissionInvestConst.Invest_BaoCao_VanHanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh_ThucChi] = { type: PermissionTypes.ButtonAction, name: 'B.C Thực chi', parentKey: PermissionInvestConst.Invest_BaoCao_VanHanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_VanHanh_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_VanHanh, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_KinhDoanh] = { type: PermissionTypes.Page, name: 'Báo cáo kinh doanh', parentKey: PermissionInvestConst.Invest_Menu_BaoCao };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_KinhDoanh_THCKDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các khoản đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_KinhDoanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_KinhDoanh_DCDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Dự chi đến hạn', parentKey: PermissionInvestConst.Invest_BaoCao_KinhDoanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_KinhDoanh_ThucChi] = { type: PermissionTypes.ButtonAction, name: 'B.C Thực chi', parentKey: PermissionInvestConst.Invest_BaoCao_KinhDoanh, webKey: WebKeys.Invest };
    PermissionInvestConfig[PermissionInvestConst.Invest_BaoCao_KinhDoanh_TCTDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng chi trả đầu tư', parentKey: PermissionInvestConst.Invest_BaoCao_KinhDoanh, webKey: WebKeys.Invest };

    PermissionInvestConfig[PermissionInvestConst.Invest_Menu_TruyVan] = { type: PermissionTypes.Menu, name: 'Truy vấn', parentKey: null, webKey: WebKeys.Garner, icon: 'pi pi-sync' };

    PermissionInvestConfig[PermissionInvestConst.Invest_TruyVan_ThuTien_NganHang] = { type: PermissionTypes.Page, name: 'Thu tiền ngân hàng', parentKey: PermissionInvestConst.Invest_Menu_TruyVan };
    PermissionInvestConfig[PermissionInvestConst.Invest_TruyVan_ChiTien_NganHang] = { type: PermissionTypes.Page, name: 'Chi tiền ngân hàng', parentKey: PermissionInvestConst.Invest_Menu_TruyVan };
    
export default PermissionInvestConfig;