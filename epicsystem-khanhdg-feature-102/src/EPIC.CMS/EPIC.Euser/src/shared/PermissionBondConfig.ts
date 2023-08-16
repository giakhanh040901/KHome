import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionBondConfig = {};
export class PermissionBondConst {
    
    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly BondModule: string = "bond.";
    //
    public static readonly BondPageDashboard: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}dashboard`;
    
    // Cài đặt
    public static readonly BondMenuCaiDat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}cai_dat`;
    
    // CHNNL: cấu hình ngày nghỉ lễ
    public static readonly BondMenuCaiDat_CHNNL: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_chnnl`;
    public static readonly BondCaiDat_CHNNL_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_chnnl_danh_sach`;
    public static readonly BondCaiDat_CHNNL_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_chnnl_cap_nhat`;
    
    // Cài đặt -> Tổ chức phát hành
    // TCPH: Tổ chức phát hành
    public static readonly BondMenuCaiDat_TCPH: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_tcph`;
    public static readonly BondCaiDat_TCPH_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_tcph_danh_sach`;
    public static readonly BondCaiDat_TCPH_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_tcph_them_moi`;
    
    // TCPH - tab thông tin chi tiết
    public static readonly Bond_TCPH_ThongTinChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}tcph_thong_tin_chi_tiet`;
    public static readonly Bond_TCPH_ThongTinChung: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}tcph_thong_tin_chung`;
    public static readonly Bond_TCPH_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}tcph_chi_tiet`;
    public static readonly Bond_TCPH_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}tcph_cap_nhat`;

    public static readonly Bond_TCPH_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}tcph_xoa`;

    // Cài đặt -> Đại lý sơ cấp
    // DLSC: Đại lý sơ cấp
    public static readonly BondMenuCaiDat_DLSC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_dlsc`;
    public static readonly BondCaiDat_DLSC_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_dlsc_danh_sach`;
    public static readonly BondCaiDat_DLSC_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_dlsc_them_moi`;
    
    // DLSC - tab thông tin chi tiết
    // TTC: Thông tin chung
    public static readonly Bond_DLSC_ThongTinChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}dlsc_thong_tin_chi_tiet`;
    public static readonly Bond_DLSC_ThongTinChung: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}dlsc_thong_tin_chung`;
    public static readonly Bond_DLSC_TTC_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}dlsc_ttc_chi_tiet`;

    // TKDN: Tài khoản đăng nhập
    public static readonly Bond_DLSC_TaiKhoanDangNhap: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}dlsc_tkdn`;
    public static readonly Bond_DLSC_TKDN_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}dlsc_tkdn_danh_sach`;
    public static readonly Bond_DLSC_TKDN_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}dlsc_tkdn_them_moi`;

    
    // Cài đặt -> Đại lý lưu ký
    // DLLK: Đại lý lưu ký
    public static readonly BondMenuCaiDat_DLLK: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_dllk`;
    public static readonly BondCaiDat_DLLK_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_dllk_danh_sach`;
    public static readonly BondCaiDat_DLLK_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_dllk_them_moi`;

    // TCPH - tab thông tin chi tiết đại lý lưu ký
    public static readonly Bond_DLLK_ThongTinChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}dllk_thong_tin_chi_tiet`;
    public static readonly Bond_DLLK_ThongTinChung: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}dllk_thong_tin_chung`;
    public static readonly Bond_DLLK_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}dllk_chi_tiet`;
    public static readonly Bond_DLLK_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}dllk_xoa`;

    // Cài đặt -> Chính sách mẫu
    // CSM: Chính sách mẫu
    public static readonly BondMenuCaiDat_CSM: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_csm`;
    public static readonly BondCaiDat_CSM_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_csm_danh_sach`;
    public static readonly BondCaiDat_CSM_ThemChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_them_chinh_sach`;
    public static readonly BondCaiDat_CSM_CapNhatChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_cap_nhat_chinh_sach`;
    public static readonly BondCaiDat_CSM_XoaChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_xoa_chinh_sach`;
    public static readonly BondCaiDat_CSM_KichHoatOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_kich_hoat_or_huy`;
    public static readonly BondCaiDat_CSM_ThemKyHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_them_ky_han`;
    public static readonly BondCaiDat_CSM_CapNhatKyHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_cap_nhat_ky_han`;
    public static readonly BondCaiDat_CSM_XoaKyHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_xoa_ky_han`;
    public static readonly BondCaiDat_CSM_KyHan_KichHoatOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_csm_ky_han_kich_hoat_or_huy`;

    // Cài đặt -> Mẫu thông báo
    // MTB: Mẫu thông báo
    public static readonly BondMenuCaiDat_MTB: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_mtb`;
    public static readonly BondCaiDatMTB_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}caidat_mtb_cap_nhat`;

    // Cài đặt -> Hình ảnh
    public static readonly BondMenuCaiDat_HinhAnh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}caidat_hinhanh`;

    
    public static readonly BondCaiDat_HinhAnh_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}caidat_hinhanh_danh_sach`;
    public static readonly BondCaiDat_HinhAnh_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_hinhanh_them_moi`;
    public static readonly BondCaiDat_HinhAnh_Sua: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_hinhanh_sua`;
    public static readonly BondCaiDat_HinhAnh_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_hinhanh_xoa`;
    public static readonly BondCaiDat_HinhAnh_DuyetDang: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}caidat_hinhanh_duyet_dang`;

    // Quản lý trái phiếu
    // QLTP: Quản lý trái phiếu
    public static readonly BondMenuQLTP: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qltp`;

    // QLTP -> Lô trái phiếu
    // LTP: Lô trái phiếu
    // TTCT: Thông tin chi tiết
    public static readonly BondMenuQLTP_LTP: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qltp_ltp`;
    public static readonly BondMenuQLTP_LTP_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_ltp_danh_sach`;
    public static readonly BondMenuQLTP_LTP_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_them_moi`;
    public static readonly BondMenuQLTP_LTP_TTCT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}qltp_ltp_thong_tin_chi_tiet`;
    public static readonly BondMenuQLTP_LTP_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_dong_xoa`;
    public static readonly BondMenuQLTP_LTP_TrinhDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_dong_trinh_duyet`;
    public static readonly BondMenuQLTP_LTP_PheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_phe_duyet`;
    public static readonly BondMenuQLTP_LTP_EpicXacMinh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_epic_phe_duyet`;
    public static readonly BondMenuQLTP_LTP_DongTraiPhieu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_ltp_dong_trai_phieu`;
    
    // Thông tin chi tiết lô trái phiếu
    // LTP: Lô trái phiếu
    // TTC: Thông tin chung
    // TSDB: Tài sản đảm bảo, HSPL: Hồ sơ pháp lý, TTTT: Thông tin trái tức
    public static readonly Bond_LTP_TTC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}ltp_thong_tin_chung`;
    public static readonly Bond_LTP_TTC_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}ltp_ttc_chi_tiet`;
    public static readonly Bond_LTP_TTC_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}ltp_ttc_cap_nhat`;
    
    // public static readonly Bond_LTP_MoTa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}ltp_mo_ta`;
    public static readonly Bond_LTP_TSDB: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}ltp_tsdb`;
    public static readonly Bond_LTP_TSDB_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}ltp_ttdb_danh_sach`;
    public static readonly Bond_LTP_TSDB_Them: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_ttdb_them`;
    public static readonly Bond_LTP_TSDB_Sua: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_ttdb_sua`;
    public static readonly Bond_LTP_TSDB_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_ttdb_xoa`;
    public static readonly Bond_LTP_TSDB_TaiXuong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_ttdb_tai_xuong`;
    
    public static readonly Bond_LTP_HSPL: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}ltp_hspl`;
    public static readonly Bond_LTP_HSPL_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}ltp_hsbl_danh_sach`;
    public static readonly Bond_LTP_HSPL_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_hspl_them_moi`;
    public static readonly Bond_LTP_HSPL_XemHoSo: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_hspl_xem_ho_so`;
    public static readonly Bond_LTP_HSPL_Download: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_hspl_download`;
    public static readonly Bond_LTP_HSPL_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}ltp_hspl_xoa`;
   
    public static readonly Bond_LTP_TTTT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}ltp_tttt`;
    public static readonly Bond_LTP_TTTT_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}ltp_tttt_danh_sach`;


    // QLTP -> Phát hành sơ cấp
    // PHSC: Phát hành sơ cấp
    public static readonly BondMenuQLTP_PHSC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qltp_phsc`;
    public static readonly BondMenuQLTP_PHSC_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_phsc_danh_sach`;
    public static readonly BondMenuQLTP_PHSC_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_phsc_them_moi`;
    public static readonly BondMenuQLTP_PHSC_TrinhDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_phsc_trinh_duyet`;
    public static readonly BondMenuQLTP_PHSC_PheDuyetOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_phsc_phe_duyet_or_huy`;
    public static readonly BondMenuQLTP_PHSC_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_phsc_xoa`;

    public static readonly BondMenuQLTP_PHSC_TTCT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}qltp_phsc_thong_tin_chi_tiet`;
    public static readonly Bond_PHSC_TTC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}phsc_thong_tin_chung`;
    public static readonly Bond_PHSC_TTC_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}phsc_ttc_chi_tiet`;
    public static readonly Bond_PHSC_TTC_ChinhSua: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}phsc_ttc_chinh_sua`;

    // public static readonly Bond_PHSC_CSL: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}phsc_chinh_sach_lai`;


    // QLTP -> Hợp đồng phân phối
    // HDPP: Hợp đồng phân phối
    public static readonly BondMenuQLTP_HDPP: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qltp_hdpp`;
    public static readonly BondMenuQLTP_HDPP_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_hdpp_danh_sach`;
    public static readonly BondMenuQLTP_HDPP_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_hdpp_them_moi`;
    public static readonly BondMenuQLTP_HDPP_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_hdpp_xoa`;

    public static readonly BondMenuQLTP_HDPP_TTCT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}qltp_hdpp_thong_tin_chi_tiet`;
    public static readonly Bond_HDPP_TTC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_thong_tin_chung`;
    public static readonly Bond_HDPP_TTC_ChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}hdpp_ttc_chi_tiet`;

    // Tab thông tin thanh toán = tttt
    public static readonly Bond_HDPP_TTThanhToan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_thong_tin_thanh_toan`;
    public static readonly Bond_HDPP_TTTT_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_tttt_danh_sach`;
    public static readonly Bond_HDPP_TTTT_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_tttt_them_moi`;
    public static readonly Bond_HDPP_TTTT_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_tttt_cap_nhat`;
    public static readonly Bond_HDPP_TTTT_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_tttt_xoa`;
    public static readonly Bond_HDPP_TTTT_ChiTietThanhToan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}hdpp_tttt_chi_tiet_thanh_toan`;
    public static readonly Bond_HDPP_TTTT_PheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_tttt_phe_duyet`;
    public static readonly Bond_HDPP_TTTT_HuyPheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_tttt_huy_phe_duyet`;

    public static readonly Bond_HDPP_DMHSKHK: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_danh_muc_ho_so_hang_ky`;
    public static readonly Bond_HDPP_DMHSKHK_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_danh_muc_ho_so_hang_ky_danh_sach`;
    public static readonly Bond_HDPP_DMHSKHK_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_them_moi`;
    public static readonly Bond_HDPP_DMHSKHK_PheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_phe_duyet`;
    public static readonly Bond_HDPP_DMHSKHK_HuyPheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_huy_phe_duyet`;
    public static readonly Bond_HDPP_DMHSKHK_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_danh_muc_ho_so_hang_ky_xoa`;

    public static readonly Bond_HDPP_TTTraiTuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_thong_tin_trai_tuc`;
    public static readonly Bond_HDPP_TTTraiTuc_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_thong_tin_trai_tuc_danh_sach`;


    // QLTP -> Bán theo kỳ hạn
    // BTKH: Bán theo kỳ hạn
    public static readonly BondMenuQLTP_BTKH: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qltp_btkh`;
    public static readonly BondMenuQLTP_BTKH_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_danh_sach`;
    public static readonly BondMenuQLTP_BTKH_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_them_moi`;
    public static readonly BondMenuQLTP_BTKH_TrinhDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_trinh_duyet`;
    public static readonly BondMenuQLTP_BTKH_DongTam: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_dong_tam`;
    public static readonly BondMenuQLTP_BTKH_BatTatShowApp: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_bat_tat_show_app`;
    public static readonly BondMenuQLTP_BTKH_EpicXacMinh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_epic_phe_duyet`;
    public static readonly BondMenuQLTP_BTKH_ThongTinChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}qltp_btkh_thong_tin_chi_tiet`;

    public static readonly Bond_BTKH_TTCT_ThongTinChung: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_ttc`;
    public static readonly Bond_BTKH_TTCT_ThongTinChung_XemChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}qltp_btkh_thong_tin_chi_tiet_ttc_xem_chi_tiet`;
    public static readonly Bond_BTKH_TTCT_ThongTinChung_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ttc_cap_nhat`;
    
    //Tab Tổng quan
    public static readonly Bond_BTKH_TongQuan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}btkh_tong_quan`;
    public static readonly Bond_BTKH_TongQuan_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}btkh_tong_quan_cap_nhat`;
    public static readonly Bond_BTKH_TongQuan_ChonAnh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}btkh_tong_quan_chon_anh`;
    public static readonly Bond_BTKH_TongQuan_ThemToChuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}btkh_tong_quan_them_to_chuc`;
    public static readonly Bond_BTKH_TongQuan_UploadFile: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}btkh_tong_quan_upload_file`;
    public static readonly Bond_BTKH_TongQuan_DanhSach_ToChuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}btkh_tong_quan_danh_sach_to_chuc`;
    public static readonly Bond_BTKH_TongQuan_DanhSach_File: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}btkh_tong_quan_danh_sach_file`;
    //Tab Chính sách        
    public static readonly Bond_BTKH_TTCT_ChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_cs`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_thong_tin_chi_tiet_cs_ds`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_themcs`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_cap_nhat`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_xoa`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_BatTatShowApp: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_bat_tat_show_app`;
    public static readonly Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_cs_huy_kich_hoat_or_huy`;
    public static readonly Bond_BTKH_TTCT_KyHan_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_them_moi`;
    public static readonly Bond_BTKH_TTCT_KyHan_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_cap_nhat`;
    public static readonly Bond_BTKH_TTCT_KyHan_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_Xoa`;
    public static readonly Bond_BTKH_TTCT_KyHan_BatTatShowApp: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_bat_tat_show_app`;
    public static readonly Bond_BTKH_TTCT_KyHan_KichHoatOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_ky_han_kich_hoat_or_huy`;

    public static readonly Bond_BTKH_TTCT_BangGia: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_bg`;
    public static readonly Bond_BTKH_TTCT_BangGia_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_thong_tin_chi_tiet_bg_danh_sach`;
    public static readonly Bond_BTKH_TTCT_BangGia_ImportExcelBG: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_import_excel_bg`;
    public static readonly Bond_BTKH_TTCT_BangGia_DownloadFileMau: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_download_file_mau`;
    public static readonly Bond_BTKH_TTCT_BangGia_XoaBangGia: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_bg_xoa_bang_gia`;

    public static readonly Bond_BTKH_TTCT_FileChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_filecs`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_thong_tin_chi_tiet_filecs_danhsach`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_UploadFileChinhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_uploadfilecs`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_Sua: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_sua`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_xoa`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_Download: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_download`;
    public static readonly Bond_BTKH_TTCT_FileChinhSach_XemFile: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_filecs_xem_file`;


    public static readonly Bond_BTKH_TTCT_MauHopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong`;
    public static readonly Bond_BTKH_TTCT_MauHopDong_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_ds`;
    public static readonly Bond_BTKH_TTCT_MauHopDong_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_them_moi`;
    public static readonly Bond_BTKH_TTCT_MauHopDong_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_cap_nhat`;
    public static readonly Bond_BTKH_TTCT_MauHopDong_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_xoa`;
    public static readonly Bond_BTKH_TTCT_MauHopDong_KichHoatOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_hop_dong_kich_hoat_or_huy`;

    // Mẫu giao nhận HĐ
    public static readonly Bond_BTKH_TTCT_MauGiaoNhanHD: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd`;
    public static readonly Bond_BTKH_TTCT_MauGiaoNhanHD_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_ds`;
    public static readonly Bond_BTKH_TTCT_MauGiaoNhanHD_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_giao_nhan_hd_them_moi`;
    public static readonly Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}qltp_btkh_thong_tin_chi_tiet_mau_giao_nhan_hd_cap_nhat`;


    public static readonly BondMenuHDPP: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp`;

    // Hợp đồng phân phối -> Sổ lệnh
    public static readonly BondHDPP_SoLenh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_solenh`;
    public static readonly BondHDPP_SoLenh_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_solenh_ds`;
    public static readonly BondHDPP_SoLenh_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_them_moi`;
    public static readonly BondHDPP_SoLenh_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_xoa`;

    // TT Chi tiết
    public static readonly BondHDPP_SoLenh_TTCT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}hdpp_solenh_ttct`;
    
    //Tab TT Chung  
    public static readonly BondHDPP_SoLenh_TTCT_TTC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_solenh_ttct_ttc`;
    public static readonly BondHDPP_SoLenh_TTCT_TTC_XemChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Form}hdpp_xlhd_ttct_ttc_xem_chi_tiet`;
    public static readonly BondHDPP_SoLenh_TTCT_TTC_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttc_cap_nhat`;
    
    // 
    public static readonly BondHDPP_SoLenh_TTCT_TTC_DoiKyHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ky_han`;
    public static readonly BondHDPP_SoLenh_TTCT_TTC_DoiMaGT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_ma_gt`;
    public static readonly BondHDPP_SoLenh_TTCT_TTC_DoiTKNganHang: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_tk_ngan_hang`;
    public static readonly BondHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_xlhd_ttct_ttc_doi_so_tien_dau_tu`;

    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_solenh_ttct_ttthanhtoan`;
    
    //TT Thanh Toan
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_solenh_ttct_ttthanhtoan_ds`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_themtt`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_chi_tiettt`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_cap_nhat`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_phe_duyet`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_huy_phe_duyet`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_gui_thong_bao`;
    public static readonly BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_ttthanhtoan_xoa`;

    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_solenh_ttct_hskh_dangky`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_solenh_ttct_hskh_dangky_ds`;
    
    //HSM: Hồ sơ mẫu, HSCKDT: Hồ sơ chữ ký điện tử
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsm`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_hsckdt`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_tai_len_ho_so`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_xem_hs_tai_len`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_chuyen_online`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_nhat_hs`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_ky_dien_tu`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_huy_ky_dien_tu`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_gui_thong_bao`;
    public static readonly BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_solenh_ttct_hskh_dangky_cap_duyet_hs_or_huy`;

    public static readonly BondHDPP_SoLenh_TTCT_LoiTuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_solenh_ttct_loituc`;
    public static readonly BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_solenh_ttct_loituc_ds`;
    
    //
    public static readonly BondHDPP_SoLenh_TTCT_TraiTuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_solenh_ttct_traituc`;
    public static readonly BondHDPP_SoLenh_TTCT_TraiTuc_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_solenh_ttct_traituc_ds`;
    
    // HDPP -> Xử lý hợp đồng
    // XLHD: Xử lý hợp đồng
    public static readonly BondHDPP_XLHD: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_xlhd`;
    public static readonly BondHDPP_XLHD_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_xlhd_ds`;

    // HDPP -> Hợp đồng
    public static readonly BondHDPP_HopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_hopdong`;
    public static readonly BondHDPP_HopDong_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_hopdong_ds`;
    public static readonly BondHDPP_HopDong_YeuCauTaiTuc: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hopdong_yc_tai_tuc`;
    
    // public static readonly BondHDPP_HopDong_YeuCauRutVon: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hopdong_yc_rut_von`;
    public static readonly BondHDPP_HopDong_PhongToaHopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hopdong_phong_toa_hd`;

    // HDPP -> Giao nhận hợp đồng
    public static readonly BondHDPP_GiaoNhanHopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_giaonhanhopdong`;
    public static readonly BondHDPP_GiaoNhanHopDong_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_giaonhanhopdong_ds`;
    public static readonly BondHDPP_GiaoNhanHopDong_DoiTrangThai: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_giaonhanhopdong_doitrangthai`;
    public static readonly BondHDPP_GiaoNhanHopDong_XuatHopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_giaonhanhopdong_xuathopdong`;

    public static readonly BondHDPP_GiaoNhanHopDong_ThongTinChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}hdpp_giaonhanhopdong_ttct`;
    
    // Thông tin chung
    public static readonly BondHDPP_GiaoNhanHopDong_TTC: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Tab}hdpp_giaonhanhopdong_ttc`;

    // HDPP -> Phong tỏa giải tóa
    // PTGT: Phong tỏa giải tỏa
        
    public static readonly BondHDPP_PTGT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_ptgt`;
    public static readonly BondHDPP_PTGT_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_ptgt_ds`;
    public static readonly BondHDPP_PTGT_GiaiToaHopDong: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_ptgt_phong_toa_hd`;
    public static readonly BondHDPP_PTGT_XemChiTiet: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_ptgt_xem_chi_tiet`;


    // HDDH: HỢP ĐỒNG ĐÁO HẠN
        
    public static readonly BondHDPP_HDDH: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_hddh`;
    public static readonly BondHDPP_HDDH_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_hddh_ds`;
    public static readonly BondHDPP_HDDH_ThongTinDauTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hddh_thong_tin_dau_tu`;
    public static readonly BondHDPP_HDDH_LapDSChiTra: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hddh_lap_ds_chi_tra`;
    public static readonly BondHDPP_HDDH_DuyetKhongChi: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_hddh_duyet_khong_chi`;

    //=========================================================

    // Quản lý phê duyệt
    // QLPD: Quản lý phê duyệt

    public static readonly BondMenuQLPD: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}qlpd`;

    // Quản lý phê duyệt -> Phê duyệt lô trái phiếu
    // PDLTP: Phê duyệt lô trái phiếu
    public static readonly BondQLPD_PDLTP: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_pdltp`;
    public static readonly BondQLPD_PDLTP_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_pdltp_ds`;
    public static readonly BondQLPD_PDLTP_PheDuyetOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_pdltp_phe_duyet_or_huy`;

    // Quản lý phê duyệt -> Phê duyệt bán theo kỳ hạn
    // PDLTP: Phê duyệt bán theo kỳ hạn
    public static readonly BondQLPD_PDBTKH: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_pdbtkh`;
    public static readonly BondQLPD_PDBTKH_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_pdbtkh_ds`;
    public static readonly BondQLPD_PDBTKH_PheDuyetOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_pdbtkh_phe_duyet_or_huy`;

    // Quản lý phê duyệt -> Phê duyệt yêu cầu tái tục
    // PDYCTT: Phê duyệt yêu cầu tái tục
    public static readonly BondQLPD_PDYCTT: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}hdpp_pdyctt`;
    public static readonly BondQLPD_PDYCTT_DanhSach: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Table}hdpp_pdyctt_ds`;
    public static readonly BondQLPD_PDYCTT_PheDuyetOrHuy: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}hdpp_pdyctt_phe_duyet_or_huy`;

    // Menu báo cáo
    public static readonly Bond_Menu_BaoCao: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Menu}bao_cao`;

    public static readonly Bond_BaoCao_QuanTri: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}bao_cao_quan_tri`;
    public static readonly Bond_BaoCao_QuanTri_THCGTraiPhieu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_quan_tri_thcg_trai_phieu`;
    public static readonly Bond_BaoCao_QuanTri_THCGDauTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_quan_tri_thcg_dau_tu`;
    public static readonly Bond_BaoCao_QuanTri_TGLDenHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_quan_tri_tgl_den_han`;

    public static readonly Bond_BaoCao_VanHanh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}bao_cao_van_hanh`;
    public static readonly Bond_BaoCao_VanHanh_THCGTraiPhieu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_van_hanh_thcg_trai_phieu`;
    public static readonly Bond_BaoCao_VanHanh_THCGDauTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_van_hanh_thcg_dau_tu`;
    public static readonly Bond_BaoCao_VanHanh_TGLDenHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_van_hanh_tgl_den_han`;

    public static readonly Bond_BaoCao_KinhDoanh: string = `${PermissionBondConst.BondModule}${PermissionBondConst.Page}bao_cao_kinh_doanh`;
    public static readonly Bond_BaoCao_KinhDoanh_THCGTraiPhieu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_kinh_doanh_thcg_trai_phieu`;
    public static readonly Bond_BaoCao_KinhDoanh_THCGDauTu: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_kinh_doanh_thcg_dau_tu`;
    public static readonly Bond_BaoCao_KinhDoanh_TGLDenHan: string = `${PermissionBondConst.BondModule}${PermissionBondConst.ButtonAction}bao_cao_kinh_doanh_tgl_den_han`;
}

    // Dashboard
    PermissionBondConfig[PermissionBondConst.BondPageDashboard] = { type: PermissionTypes.Menu, name: 'Dashboard tổng quan', parentKey: null, icon: 'pi pi-fw pi-home' };

    // Menu cài đặt
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat] = { type: PermissionTypes.Menu, name: 'Cài đặt', parentKey: null, icon: 'pi pi-fw pi-cog' };
    // CHNNL: cấu hình ngày nghỉ lễ
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_CHNNL] = { type: PermissionTypes.Menu, name: 'Cấu hình ngày nghỉ lễ', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CHNNL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_CHNNL };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CHNNL_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.BondMenuCaiDat_CHNNL };

    // Cài đặt -> Tổ chức phát hành
    // TCPH: Tổ chức phát hành
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_TCPH] = { type: PermissionTypes.Menu, name: 'Tổ chức phát hành', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_TCPH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_TCPH };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_TCPH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuCaiDat_TCPH };
    PermissionBondConfig[PermissionBondConst.Bond_TCPH_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuCaiDat_TCPH, webKey: WebKeys.Bond };

    // TCPH - tab thông tin chi tiết
    PermissionBondConfig[PermissionBondConst.Bond_TCPH_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuCaiDat_TCPH, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_TCPH_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.Bond_TCPH_ThongTinChiTiet, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_TCPH_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_TCPH_ThongTinChung, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_TCPH_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_TCPH_ThongTinChung, webKey: WebKeys.Bond };

    
    // Cài đặt -> Đại lý sơ cấp
    // DLSC: Đại lý sơ cấp
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_DLSC] = { type: PermissionTypes.Menu, name: 'Đại lý sơ cấp', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_DLSC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_DLSC };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_DLSC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuCaiDat_DLSC };

    // Tab thông tin đại lý sơ cấp
    // Thông tin chung
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuCaiDat_DLSC, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.Bond_DLSC_ThongTinChiTiet, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_TTC_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_DLSC_ThongTinChung, webKey: WebKeys.Bond };
    // Tài khoản đăng nhập
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_TaiKhoanDangNhap] = { type: PermissionTypes.Page, name: 'Tài khoản đăng nhập', parentKey: PermissionBondConst.BondMenuCaiDat_DLSC, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_TKDN_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_DLSC_TaiKhoanDangNhap, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLSC_TKDN_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_DLSC_TaiKhoanDangNhap };

    // Cài đặt -> Đại lý lưu ký
    // DLLK: Đại lý lưu ký
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_DLLK] = { type: PermissionTypes.Menu, name: 'Đại lý lưu ký', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_DLLK_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_DLLK };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_DLLK_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuCaiDat_DLLK };
    PermissionBondConfig[PermissionBondConst.Bond_DLLK_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuCaiDat_DLLK, webKey: WebKeys.Bond };

    // Tab thông tin đại lý lưu ký
    // Thông tin chung
    PermissionBondConfig[PermissionBondConst.Bond_DLLK_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuCaiDat_DLLK, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLLK_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.Bond_DLLK_ThongTinChiTiet, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_DLLK_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_DLLK_ThongTinChung, webKey: WebKeys.Bond };

    // Cài đặt -> Chính sách mẫu
    // CSM: Chính sách mẫu
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_CSM] = { type: PermissionTypes.Menu, name: 'Chính sách mẫu', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_ThemChinhSach] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_CapNhatChinhSach] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_XoaChinhSach] = { type: PermissionTypes.ButtonAction, name: 'Xóa chính sách', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Chính sách)', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_ThemKyHan] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_CapNhatKyHan] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_XoaKyHan] = { type: PermissionTypes.ButtonAction, name: 'Xóa kỳ hạn', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_CSM_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Kỳ hạn)', parentKey: PermissionBondConst.BondMenuCaiDat_CSM };

    // Cài đặt -> Mẫu thông báo
    // MTB: Mẫu thông báo
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_MTB] = { type: PermissionTypes.Menu, name: 'Mẫu thông báo', parentKey: PermissionBondConst.BondMenuCaiDat };
    PermissionBondConfig[PermissionBondConst.BondCaiDatMTB_CapNhat] = { type: PermissionTypes.Form, name: 'Cài đặt mẫu thông báo', parentKey: PermissionBondConst.BondMenuCaiDat_MTB };

    // Cài đặt -> Hình ảnh
    PermissionBondConfig[PermissionBondConst.BondMenuCaiDat_HinhAnh] = { type: PermissionTypes.Menu, name: 'Hình ảnh', parentKey: PermissionBondConst.BondMenuCaiDat };

    PermissionBondConfig[PermissionBondConst.BondCaiDat_HinhAnh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuCaiDat_HinhAnh };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_HinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuCaiDat_HinhAnh };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_HinhAnh_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionBondConst.BondMenuCaiDat_HinhAnh };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_HinhAnh_DuyetDang] = { type: PermissionTypes.ButtonAction, name: 'Duyệt đăng', parentKey: PermissionBondConst.BondMenuCaiDat_HinhAnh };
    PermissionBondConfig[PermissionBondConst.BondCaiDat_HinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuCaiDat_HinhAnh };

    // Quản lý trái phiếu

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP] = { type: PermissionTypes.Menu, name: 'Quản lý trái phiếu', parentKey: null , icon: 'pi pi-map'};

    // QLTP -> Lô trái phiếu
    // LTP: Lô trái phiếu
    // TTCT: Thông tin chi tiết
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP] = { type: PermissionTypes.Menu, name: 'Lô trái phiếu', parentKey: PermissionBondConst.BondMenuQLTP };

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_DongTraiPhieu] = { type: PermissionTypes.ButtonAction, name: 'Đóng / Mở trái phiếu', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuQLTP_LTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_LTP_TTCT] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuQLTP_LTP };

    // Thông tin chi tiết
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondMenuQLTP_LTP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TTC_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_LTP_TTC, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TTC_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_LTP_TTC, webKey: WebKeys.Bond };

    // PermissionBondConfig[PermissionBondConst.Bond_LTP_MoTa] = { type: PermissionTypes.Tab, name: 'Mô tả', parentKey: PermissionBondConst.Bond_LTP_TTC, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB] = { type: PermissionTypes.Tab, name: 'Tài sản đảm bảo', parentKey: PermissionBondConst.BondMenuQLTP_LTP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_LTP_TSDB, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB_Them] = { type: PermissionTypes.ButtonAction, name: 'Thêm', parentKey: PermissionBondConst.Bond_LTP_TSDB, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionBondConst.Bond_LTP_TSDB, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB_TaiXuong] = { type: PermissionTypes.ButtonAction, name: 'Tải xuống', parentKey: PermissionBondConst.Bond_LTP_TSDB, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TSDB_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_LTP_TSDB, webKey: WebKeys.Bond };


    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL] = { type: PermissionTypes.Tab, name: 'Hồ sơ pháp lý', parentKey: PermissionBondConst.BondMenuQLTP_LTP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_LTP_HSPL, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_LTP_HSPL, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL_XemHoSo] = { type: PermissionTypes.ButtonAction, name: 'Xem hồ sơ', parentKey: PermissionBondConst.Bond_LTP_HSPL, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL_Download] = { type: PermissionTypes.ButtonAction, name: 'Tải hồ sơ', parentKey: PermissionBondConst.Bond_LTP_HSPL, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_HSPL_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_LTP_HSPL, webKey: WebKeys.Bond };

    PermissionBondConfig[PermissionBondConst.Bond_LTP_TTTT] = { type: PermissionTypes.Tab, name: 'Thông tin trái tức', parentKey: PermissionBondConst.BondMenuQLTP_LTP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_LTP_TTTT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_LTP_TTTT, webKey: WebKeys.Bond };

    // QLTP -> Phát hành sơ cấp
    // PHSC: Phát hành sơ cấp
    // TTCT: Thông tin chi tiết

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC] = { type: PermissionTypes.Menu, name: 'Phát hành sơ cấp', parentKey: PermissionBondConst.BondMenuQLTP };

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt / Hủy', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };

    // Thông tin chi tiết
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_PHSC_TTCT] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuQLTP_PHSC };
    PermissionBondConfig[PermissionBondConst.Bond_PHSC_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondMenuQLTP_PHSC_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_PHSC_TTC_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_PHSC_TTC, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_PHSC_TTC_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionBondConst.Bond_PHSC_TTC, webKey: WebKeys.Bond };


    //  PermissionBondConfig[PermissionBondConst.Bond_PHSC_CSL] = { type: PermissionTypes.Tab, name: 'Chính sách lãi', parentKey: PermissionBondConst.BondMenuQLTP_PHSC_TTCT, webKey: WebKeys.Bond };

    // QLTP -> Hợp đồng phân phối
    // HDPP: Hợp đồng phân phối
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_HDPP] = { type: PermissionTypes.Menu, name: 'Hợp đồng phân phối', parentKey: PermissionBondConst.BondMenuQLTP };

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_HDPP_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuQLTP_HDPP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_HDPP_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuQLTP_HDPP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_HDPP_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.BondMenuQLTP_HDPP };

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_HDPP_TTCT] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuQLTP_HDPP }; 
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondMenuQLTP_HDPP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTC_ChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_HDPP_TTC, webKey: WebKeys.Bond };

    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTThanhToan] = { type: PermissionTypes.Tab, name: 'Thông tin thanh toán', parentKey: PermissionBondConst.BondMenuQLTP_HDPP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_ChiTietThanhToan] = { type: PermissionTypes.Form, name: 'Chi tiết thanh toán', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_HuyPheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Hủy phê duyệt', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTT_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_HDPP_TTThanhToan, webKey: WebKeys.Bond };


    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK] = { type: PermissionTypes.Tab, name: 'Danh mục hồ sơ khách hàng ký', parentKey: PermissionBondConst.BondMenuQLTP_HDPP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_HDPP_DMHSKHK, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_HDPP_DMHSKHK, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionBondConst.Bond_HDPP_DMHSKHK, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK_HuyPheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Hủy phê duyệt', parentKey: PermissionBondConst.Bond_HDPP_DMHSKHK, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_DMHSKHK_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_HDPP_DMHSKHK, webKey: WebKeys.Bond };

    
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTraiTuc] = { type: PermissionTypes.Tab, name: 'Thông tin trái tức', parentKey: PermissionBondConst.BondMenuQLTP_HDPP_TTCT, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_HDPP_TTTraiTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_HDPP_TTTraiTuc, webKey: WebKeys.Bond };

    // Bán theo kỳ hạn

    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH] = { type: PermissionTypes.Menu, name: 'Bán theo kỳ hạn', parentKey: PermissionBondConst.BondMenuQLTP };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_TrinhDuyet] = { type: PermissionTypes.ButtonAction, name: 'Trình duyệt', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_DongTam] = { type: PermissionTypes.ButtonAction, name: 'Đóng tạm', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật show app', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_EpicXacMinh] = { type: PermissionTypes.ButtonAction, name: 'Epic xác minh', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };
    PermissionBondConfig[PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondMenuQLTP_BTKH };

    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung_XemChiTiet] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung };

    // Tổng quan 
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan] = { type: PermissionTypes.Tab, name: 'Tổng quan', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_ChonAnh] = { type: PermissionTypes.ButtonAction, name: 'Chọn ảnh', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_ThemToChuc] = { type: PermissionTypes.ButtonAction, name: 'Thêm tổ chức', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_DanhSach_ToChuc] = { type: PermissionTypes.Table, name: 'Danh sách tổ chức', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_UploadFile] = { type: PermissionTypes.ButtonAction, name: 'Upload file', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TongQuan_DanhSach_File] = { type: PermissionTypes.Table, name: 'Danh sách file', parentKey: PermissionBondConst.Bond_BTKH_TongQuan, webKey: WebKeys.Invest };

    //Chính sách
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach] = { type: PermissionTypes.Menu, name: 'Chính sách', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm chính sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật chính sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật tắt show App (Chính sách)', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Chính sách)', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_ChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa chính sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    //
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_KyHan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm kỳ hạn', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_KyHan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật kỳ hạn', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_KyHan_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật tắt show App (Kỳ hạn)', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_KyHan_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy (Kỳ hạn)', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_KyHan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa kỳ hạn', parentKey: PermissionBondConst.Bond_BTKH_TTCT_ChinhSach };


    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_BangGia] = { type: PermissionTypes.Menu, name: 'Bảng giá', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_BangGia_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_BangGia };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_BangGia_ImportExcelBG] = { type: PermissionTypes.ButtonAction, name: 'Import file excel bảng giá', parentKey: PermissionBondConst.Bond_BTKH_TTCT_BangGia };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_BangGia_DownloadFileMau] = { type: PermissionTypes.ButtonAction, name: 'Download file mẫu', parentKey: PermissionBondConst.Bond_BTKH_TTCT_BangGia };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_BangGia_XoaBangGia] = { type: PermissionTypes.ButtonAction, name: 'Xóa bảng giá', parentKey: PermissionBondConst.Bond_BTKH_TTCT_BangGia };

    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach] = { type: PermissionTypes.Menu, name: 'File chính sách', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_UploadFileChinhSach] = { type: PermissionTypes.ButtonAction, name: 'Upload file chính sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_Sua] = { type: PermissionTypes.ButtonAction, name: 'Sửa', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_Download] = { type: PermissionTypes.ButtonAction, name: 'Tải file', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_XemFile] = { type: PermissionTypes.ButtonAction, name: 'Xem file', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_BTKH_TTCT_FileChinhSach };


    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong] = { type: PermissionTypes.Menu, name: 'Mẫu hợp đồng', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauHopDong };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauHopDong };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauHopDong };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy kích hoạt', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauHopDong };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauHopDong_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauHopDong };
    
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD] = { type: PermissionTypes.Menu, name: 'Mẫu giao nhận HĐ', parentKey: PermissionBondConst.BondMenuQLTP_BTKH_ThongTinChiTiet };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD };
    PermissionBondConfig[PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD };


    // Menu Hợp đồng phân phối

    PermissionBondConfig[PermissionBondConst.BondMenuHDPP] = { type: PermissionTypes.Menu, name: 'Hợp đồng phân phối', parentKey: null, icon: 'pi pi-book'};

    // Hợp đồng phân phối -> Sổ lệnh
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh] = { type: PermissionTypes.Menu, name: 'Sổ lệnh', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_DanhSach] = { type: PermissionTypes.Menu, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_SoLenh };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_ThemMoi] = { type: PermissionTypes.Menu, name: 'Thêm mới', parentKey: PermissionBondConst.BondHDPP_SoLenh };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_Xoa] = { type: PermissionTypes.Menu, name: 'Xóa', parentKey: PermissionBondConst.BondHDPP_SoLenh };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT] = { type: PermissionTypes.Menu, name: 'Chi tiết sổ lệnh', parentKey: PermissionBondConst.BondMenuHDPP };

    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_XemChiTiet] = { type: PermissionTypes.Tab, name: 'Xem chi tiết', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_CapNhat] = { type: PermissionTypes.Tab, name: 'Cập nhật sổ lệnh', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };
    
    //
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_DoiKyHan] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật Kỳ hạn', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_DoiMaGT] = { type: PermissionTypes.Tab, name: 'Cập nhật Mã giới thiệu', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_DoiTKNganHang] = { type: PermissionTypes.Tab, name: 'Cập nhật TK ngân hàng', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC_DoiSoTienDauTu] = { type: PermissionTypes.Tab, name: 'Cập nhật Số tiền đầu tư', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTC };

    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan] = { type: PermissionTypes.Tab, name: 'Thông tin thanh toán', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_ThemMoi] = { type: PermissionTypes.Tab, name: 'Thêm mới', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan] = { type: PermissionTypes.Tab, name: 'Chi tiết thanh toán', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat] = { type: PermissionTypes.Tab, name: 'Sửa', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet] = { type: PermissionTypes.Tab, name: 'Phê duyệt', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet] = { type: PermissionTypes.Tab, name: 'Hủy phê duyệt', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao] = { type: PermissionTypes.Tab, name: 'Gửi thông báo', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa] = { type: PermissionTypes.Tab, name: 'Xóa', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan };

    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy] = { type: PermissionTypes.Tab, name: 'HSKH đăng ký', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM] = { type: PermissionTypes.Tab, name: 'Tải hồ sơ mẫu', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT] = { type: PermissionTypes.Tab, name: 'Tải hồ sơ chữ ký điện tử', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS] = { type: PermissionTypes.Tab, name: 'Tải lên hồ sơ', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_XemHSTaiLen] = { type: PermissionTypes.Tab, name: 'Xem hồ sơ tải lên', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_ChuyenOnline] = { type: PermissionTypes.Tab, name: 'Chuyển online', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS] = { type: PermissionTypes.Tab, name: 'Cập nhật hồ sơ', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu] = { type: PermissionTypes.Tab, name: 'Ký điện tử', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_HuyKyDienTu] = { type: PermissionTypes.Tab, name: 'Hủy ký điện tử', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao] = { type: PermissionTypes.Tab, name: 'Gửi thông báo', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy_DuyetHoSoOrHuy] = { type: PermissionTypes.Tab, name: 'Duyệt / Hủy hồ sơ', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_HSKHDangKy };

    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_LoiTuc] = { type: PermissionTypes.Tab, name: 'Lợi tức', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_LoiTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_LoiTuc };

    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TraiTuc] = { type: PermissionTypes.Tab, name: 'Trái tức', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_SoLenh_TTCT_TraiTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_SoLenh_TTCT_TraiTuc };

    // Hợp đồng phân phối -> Xử lý hợp đồng
    PermissionBondConfig[PermissionBondConst.BondHDPP_XLHD] = { type: PermissionTypes.Menu, name: 'Xử lý hợp đồng', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_XLHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_XLHD };

    // Hợp đồng phân phối -> Hợp đồng

    PermissionBondConfig[PermissionBondConst.BondHDPP_HopDong] = { type: PermissionTypes.Menu, name: 'Hợp đồng', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_HopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HopDong_YeuCauTaiTuc] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu tái tục', parentKey: PermissionBondConst.BondHDPP_HopDong };
    
    // PermissionBondConfig[PermissionBondConst.BondHDPP_HopDong_YeuCauRutVon] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu rút vốn', parentKey: PermissionBondConst.BondHDPP_HopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HopDong_PhongToaHopDong] = { type: PermissionTypes.ButtonAction, name: 'Phong tỏa hợp đồng', parentKey: PermissionBondConst.BondHDPP_HopDong };

    // Hợp đồng phân phối -> Giao nhận hợp đồng

    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong] = { type: PermissionTypes.Menu, name: 'Giao nhận hợp đồng', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_GiaoNhanHopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionBondConst.BondHDPP_GiaoNhanHopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong_XuatHopDong] = { type: PermissionTypes.ButtonAction, name: 'Xuất hợp đồng', parentKey: PermissionBondConst.BondHDPP_GiaoNhanHopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong_ThongTinChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionBondConst.BondHDPP_GiaoNhanHopDong };
    PermissionBondConfig[PermissionBondConst.BondHDPP_GiaoNhanHopDong_TTC] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionBondConst.BondHDPP_GiaoNhanHopDong_ThongTinChiTiet };

    // Hợp đồng phân phối -> Phong tỏa, giải tỏa
    PermissionBondConfig[PermissionBondConst.BondHDPP_PTGT] = { type: PermissionTypes.Menu, name: 'Phong tỏa giải tỏa', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_PTGT_DanhSach] = { type: PermissionTypes.Menu, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_PTGT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_PTGT_GiaiToaHopDong] = { type: PermissionTypes.Menu, name: 'Giải tỏa hợp đồng', parentKey: PermissionBondConst.BondHDPP_PTGT };
    PermissionBondConfig[PermissionBondConst.BondHDPP_PTGT_XemChiTiet] = { type: PermissionTypes.Menu, name: 'Xem chi tiết', parentKey: PermissionBondConst.BondHDPP_PTGT };

    // Hợp đồng đáo hạn
    PermissionBondConfig[PermissionBondConst.BondHDPP_HDDH] = { type: PermissionTypes.Menu, name: 'Hợp đồng đáo hạn', parentKey: PermissionBondConst.BondMenuHDPP };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HDDH_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionBondConst.BondHDPP_HDDH };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HDDH_ThongTinDauTu] = { type: PermissionTypes.ButtonAction, name: 'Thông tin đầu tư', parentKey: PermissionBondConst.BondHDPP_HDDH };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HDDH_LapDSChiTra] = { type: PermissionTypes.ButtonAction, name: 'Lập danh sách chi trả', parentKey: PermissionBondConst.BondHDPP_HDDH };
    PermissionBondConfig[PermissionBondConst.BondHDPP_HDDH_DuyetKhongChi] = { type: PermissionTypes.ButtonAction, name: 'Duyệt không chi', parentKey: PermissionBondConst.BondHDPP_HDDH };

    //==========================

    //Quản lý phê duyệt
    PermissionBondConfig[PermissionBondConst.BondMenuQLPD] = { type: PermissionTypes.Menu, name: 'Quản lý phê duyệt', parentKey: null, icon: 'pi pi-check-circle' };

    // QLPD -> Phê duyệt lô trái phiếu
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDLTP] = { type: PermissionTypes.Menu, name: 'Phê duyệt lô trái phiếu', parentKey: PermissionBondConst.BondMenuQLPD };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDLTP_DanhSach] = { type: PermissionTypes.Menu, name: 'Danh sách', parentKey: PermissionBondConst.BondQLPD_PDLTP };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDLTP_PheDuyetOrHuy] = { type: PermissionTypes.Menu, name: 'Phê duyệt / Hủy', parentKey: PermissionBondConst.BondQLPD_PDLTP };

    // QLPD -> Phê duyệt bán theo kỳ hạn
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDBTKH] = { type: PermissionTypes.Menu, name: 'Phê duyệt bán theo kỳ hạn', parentKey: PermissionBondConst.BondMenuQLPD };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDBTKH_DanhSach] = { type: PermissionTypes.Menu, name: 'Danh sách', parentKey: PermissionBondConst.BondQLPD_PDBTKH };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDBTKH_PheDuyetOrHuy] = { type: PermissionTypes.Menu, name: 'Phê duyệt / Hủy', parentKey: PermissionBondConst.BondQLPD_PDBTKH };

    // QLPD -> Phê duyệt yêu cầu tái tục
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDYCTT] = { type: PermissionTypes.Menu, name: 'Phê duyệt yêu cầu tái tục', parentKey: PermissionBondConst.BondMenuQLPD };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDYCTT_DanhSach] = { type: PermissionTypes.Menu, name: 'Danh sách', parentKey: PermissionBondConst.BondQLPD_PDYCTT };
    PermissionBondConfig[PermissionBondConst.BondQLPD_PDYCTT_PheDuyetOrHuy] = { type: PermissionTypes.Menu, name: 'Phê duyệt / Hủy', parentKey: PermissionBondConst.BondQLPD_PDYCTT };

    // Báo cáo
    PermissionBondConfig[PermissionBondConst.Bond_Menu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.Bond, icon: 'pi pi-file-export' };

    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionBondConst.Bond_Menu_BaoCao };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_QuanTri_THCGTraiPhieu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói trái phiếu', parentKey: PermissionBondConst.Bond_BaoCao_QuanTri, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_QuanTri_THCGDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói đầu tư', parentKey: PermissionBondConst.Bond_BaoCao_QuanTri, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_QuanTri_TGLDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Tính gốc lãi đến hạn', parentKey: PermissionBondConst.Bond_BaoCao_QuanTri, webKey: WebKeys.Bond };

    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_VanHanh] = { type: PermissionTypes.Page, name: 'Báo cáo vận hành', parentKey: PermissionBondConst.Bond_Menu_BaoCao };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_VanHanh_THCGTraiPhieu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói trái phiếu', parentKey: PermissionBondConst.Bond_BaoCao_VanHanh, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_VanHanh_THCGDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói đầu tư', parentKey: PermissionBondConst.Bond_BaoCao_VanHanh, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_VanHanh_TGLDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Tính gốc lãi đến hạn', parentKey: PermissionBondConst.Bond_BaoCao_VanHanh, webKey: WebKeys.Bond };

    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_KinhDoanh] = { type: PermissionTypes.Page, name: 'Báo cáo kinh doanh', parentKey: PermissionBondConst.Bond_Menu_BaoCao };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_KinhDoanh_THCGTraiPhieu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói trái phiếu', parentKey: PermissionBondConst.Bond_BaoCao_KinhDoanh, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_KinhDoanh_THCGDauTu] = { type: PermissionTypes.ButtonAction, name: 'B.C Tổng hợp các gói đầu tư', parentKey: PermissionBondConst.Bond_BaoCao_KinhDoanh, webKey: WebKeys.Bond };
    PermissionBondConfig[PermissionBondConst.Bond_BaoCao_KinhDoanh_TGLDenHan] = { type: PermissionTypes.ButtonAction, name: 'B.C Tính gốc lãi đến hạn', parentKey: PermissionBondConst.Bond_BaoCao_KinhDoanh, webKey: WebKeys.Bond };
export default PermissionBondConfig;
