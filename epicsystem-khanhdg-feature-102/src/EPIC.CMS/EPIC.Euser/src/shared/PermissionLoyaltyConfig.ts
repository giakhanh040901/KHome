import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionLoyaltyConfig = {};
export class PermissionLoyaltyConst {
    
    private static readonly Web: string = "web_";
    private static readonly Menu: string = "menu_";
    private static readonly Tab: string = "tab_";
    private static readonly Page: string = "page_";
    private static readonly Table: string = "table_";
    private static readonly Form: string = "form_";
    private static readonly ButtonTable: string = "btn_table_";
    private static readonly ButtonForm: string = "btn_form_";
    private static readonly ButtonAction: string = "btn_action_";

    public static readonly LoyaltyModule: string = "loyalty.";
    
    // Khách hàng
    public static readonly LoyaltyMenu_QLKhachHang: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_khach_hang`;
    // Khách hàng cá nhân
    public static readonly LoyaltyMenu_KhachHangCaNhan: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}khach_hang_ca_nhan`;
    public static readonly Loyalty_KhachHangCaNhan_XuatDuLieu: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}khach_hang_ca_nhan_xuat_du_lieu`;
    public static readonly Loyalty_KhachHangCaNhan_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}khach_hang_ca_nhan_danh_sach`;
    public static readonly Loyalty_KhachHangCaNhan_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}khach_hang_ca_nhan_cap_nhat`;
    
    public static readonly Loyalty_KhachHangCaNhan_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Page}khach_hang_ca_nhan_chi_tiet`;
    public static readonly Loyalty_KhachHangCaNhan_ThongTinChung: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}khach_hang_ca_nhan_thong_tin_chung`;
    public static readonly Loyalty_KhachHangCaNhan_ThongTinChung_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Form}khach_hang_ca_nhan_thong_tin_chung_chi_tiet`;
   
    public static readonly Loyalty_KhachHangCaNhan_UuDai: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}khach_hang_ca_nhan_uu_dai`;
    public static readonly Loyalty_KhachHangCaNhan_UuDai_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}khach_hang_ca_nhan_uu_dai_danh_sach`;
    public static readonly Loyalty_KhachHangCaNhan_UuDai_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}khach_hang_ca_nhan_uu_dai_them_moi`;

    public static readonly Loyalty_KhachHangCaNhan_SuKienThamGia: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}khach_hang_ca_nhan_su_kien_tham_gia`;

    public static readonly Loyalty_KhachHangCaNhan_LichSuThamGia: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}khach_hang_ca_nhan_lich_su_tham_gia`;
    public static readonly Loyalty_KhachHangCaNhan_LichSuThamGia_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}khach_hang_ca_nhan_lich_su_tham_gia_danh_sach`;

    // Ưu đãi
    public static readonly LoyaltyMenu_QLUuDai: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_uu_dai`;
    // Danh sách ưu đãi
    public static readonly LoyaltyMenu_DanhSachUuDai: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}danh_sach_uu_dai`;
    public static readonly Loyalty_DanhSachUuDai_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}danh_sach_uu_dai_danh_sach`;
    public static readonly Loyalty_DanhSachUuDai_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_them_moi`;
    public static readonly Loyalty_DanhSachUuDai_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_chi_tiet`;
    public static readonly Loyalty_DanhSachUuDai_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_chinh_sua`;
    public static readonly Loyalty_DanhSachUuDai_DanhDauOrTatNoiBat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_danh_dau_or_tat_noi_bat`;
    public static readonly Loyalty_DanhSachUuDai_BatOrTatShowApp: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_bat_or_tat_show_app`;
    public static readonly Loyalty_DanhSachUuDai_KichHoatOrHuy: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_kich_hoat_or_huy`;
    public static readonly Loyalty_DanhSachUuDai_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}danh_sach_uu_dai_xoa`;
    // Lịch sử cấp phát
    public static readonly LoyaltyMenu_LichSuCapPhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}lich_su_cap_phat`;
    public static readonly Loyalty_LichSuCapPhat_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}lich_su_cap_phat_danh_sach`;

    // Điểm
    public static readonly LoyaltyMenu_QLDiem: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_diem`;
    // Tích điểm
    public static readonly LoyaltyMenu_TichDiem: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_tich_diem`;
    public static readonly LoyaltyMenu_TichDiem_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_them_moi`;
    public static readonly LoyaltyMenu_TichDiem_UploadDSDiem: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_upload_ds_diem`;
    public static readonly LoyaltyMenu_TichDiem_DownloadMau: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_download_mau`;
    public static readonly LoyaltyMenu_TichDiem_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}tich_diem_danh_sach`;
    public static readonly LoyaltyMenu_TichDiem_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_chi_tiet`;
    public static readonly LoyaltyMenu_TichDiem_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_chinh_sua`;
    public static readonly LoyaltyMenu_TichDiem_Huy: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tich_diem_huy`;
    // Hạng thành viên
    public static readonly LoyaltyMenu_HangThanhVien: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_hang_thanh_vien`;
    public static readonly LoyaltyMenu_HangThanhVien_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hang_thanh_vien_them_moi`;
    public static readonly LoyaltyMenu_HangThanhVien_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}hang_thanh_vien_danh_sach`;
    public static readonly LoyaltyMenu_HangThanhVien_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hang_thanh_vien_chi_tiet`;
    public static readonly LoyaltyMenu_HangThanhVien_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hang_thanh_vien_chinh_sua`;
    public static readonly LoyaltyMenu_HangThanhVien_KichHoatOrHuy: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hang_thanh_vien_kich_hoat_or_huy`;

    // Yêu cầu
    public static readonly LoyaltyMenu_QLYeuCau: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}quan_ly_yeu_cau`;
    // Yêu cầu đổi voucher
    public static readonly LoyaltyMenu_YeuCauDoiVoucher: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}yeu_cau_doi_voucher`;
    public static readonly Loyalty_YeuCauDoiVoucher_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}yeu_cau_doi_voucher_danh_sach`;
    public static readonly Loyalty_YeuCauDoiVoucher_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_them_moi`;
    public static readonly Loyalty_YeuCauDoiVoucher_ChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_chi_tiet`;
    public static readonly Loyalty_YeuCauDoiVoucher_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_chinh_sua`;
    public static readonly Loyalty_YeuCauDoiVoucher_BanGiao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_ban_giao`;
    public static readonly Loyalty_YeuCauDoiVoucher_HoanThanh: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_hoan_thanh`;
    public static readonly Loyalty_YeuCauDoiVoucher_HuyYeuCau: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}yeu_cau_doi_voucher_huy_yeu_cau`;

    // Truyền thông
    public static readonly LoyaltyMenu_TruyenThong: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}truyen_thong`;

    // Truyền thông - hình ảnh
    public static readonly LoyaltyMenu_HinhAnh: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}hinh_anh`;
    public static readonly LoyaltyHinhAnh_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}hinh_anh_danh_sach`;
    public static readonly LoyaltyHinhAnh_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hinh_anh_them_moi`;
    public static readonly LoyaltyHinhAnh_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hinh_anh_cap_nhat`;
    public static readonly LoyaltyHinhAnh_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hinh_anh_xoa`;
    public static readonly LoyaltyHinhAnh_PheDuyetOrHuyDang: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}hinh_anh_phe_duyet_or_huy_dang`;

    // Truyền thông - Tin tức
    public static readonly LoyaltyMenu_TinTuc: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}tin_tuc`;
    public static readonly LoyaltyTinTuc_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}tin_tuc_danh_sach`;
    public static readonly LoyaltyTinTuc_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tin_tuc_them_moi`;
    public static readonly LoyaltyTinTuc_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tin_tuc_cap_nhat`;
    public static readonly LoyaltyTinTuc_PheDuyetOrHuyDang: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tin_tuc_phe_duyet_or_huy_dang`;
    public static readonly LoyaltyTinTuc_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}tin_tuc_xoa`;
  

    // Thông báo 
    public static readonly LoyaltyMenu_ThongBao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}thong_bao`;
    
    // Thông báo - thông báo hệ thống
    public static readonly LoyaltyMenu_ThongBaoHeThong: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}thong_bao_he_thong`;
    public static readonly LoyaltyThongBaoHeThong_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}thong_bao_he_thong_cap_nhat`;

    // Thông báo - Mẫu thông báo
    public static readonly LoyaltyMenu_MauThongBao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}mau_thong_bao`;
    public static readonly LoyaltyMauThongBao_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}mau_thong_bao_danh_sach`;
    public static readonly LoyaltyMauThongBao_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}mau_thong_bao_them_moi`;
    public static readonly LoyaltyMauThongBao_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}mau_thong_bao_cap_nhat`;
    public static readonly LoyaltyMauThongBao_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}mau_thong_bao_xoa`;
    public static readonly LoyaltyMauThongBao_KichHoatOrHuy: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}mau_thong_bao_kich_hoat_hoac_huy`;

    // Thông báo - Quản lý thông báo = QLTB
    public static readonly LoyaltyMenu_QLTB: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}qltb`;
    public static readonly LoyaltyQLTB_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}qltb_danh_sach`;
    public static readonly LoyaltyQLTB_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_them_moi`;
    public static readonly LoyaltyQLTB_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_xoa`;
    public static readonly LoyaltyQLTB_KichHoatOrHuy: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_kich_hoat_hoac_huy`;

    //
    public static readonly LoyaltyQLTB_PageChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Page}qltb_page_chi_tiet`;
    
    //
    public static readonly LoyaltyQLTB_ThongTinChung: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}qltb_page_chi_tiet_thong_tin_chung`;
    public static readonly LoyaltyQLTB_PageChiTiet_ThongTin: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Form}qltb_page_chi_tiet_thong_tin`;
    public static readonly LoyaltyQLTB_PageChiTiet_CapNhat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_page_chi_tiet_cap_nhat`;
    
    //
    public static readonly LoyaltyQLTB_GuiThongBao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}qltb_page_chi_tiet_gui_thong_bao`;
    public static readonly LoyaltyQLTB_PageChiTiet_GuiThongBao_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Form}qltb_page_chi_tiet_gui_thong_bao_danh_sach`;
    public static readonly LoyaltyQLTB_PageChiTiet_GuiThongBao_CaiDat: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_page_chi_tiet_gui_thong_bao_cai_dat`;
    public static readonly LoyaltyQLTB_PageChiTiet_GuiThongBao_GuiThongBao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_page_chi_tiet_gui_thong_bao_gui_thong_bao`;
    public static readonly LoyaltyQLTB_PageChiTiet_GuiThongBao_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}qltb_page_chi_tiet_gui_thong_bao_xoa`;

    // Báo cáo thống kê
    public static readonly LoyaltyMenu_BaoCao: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}bao_cao`;
    public static readonly Loyalty_BaoCao_QuanTri: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Page}bao_cao_quan_tri`;
    public static readonly Loyalty_BaoCao_QuanTri_DSYeuCauDoiVoucher: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}bao_cao_quan_tri_ds_yeu_cau_doi_voucher`;
    public static readonly Loyalty_BaoCao_QuanTri_XuatNhapTonVoucher: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}bao_cao_quan_tri_xuat_nhap_ton_voucher`;

    // Chương trình trúng thưởng
    public static readonly LoyaltyMenu_CT_TrungThuong: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Menu}ct_trung_thuong`;
    public static readonly LoyaltyCT_TrungThuong_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}ct_trung_thuong_danh_sach`;
    public static readonly LoyaltyCT_TrungThuong_ThemMoi: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_them_moi`;
    public static readonly LoyaltyCT_TrungThuong_CaiDatThamGia: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_cai_dat_tham_gia`;
    public static readonly LoyaltyCT_TrungThuong_DoiTrangThai: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_doi_trang_thai`;
    public static readonly LoyaltyCT_TrungThuong_Xoa: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_xoa`;
    public static readonly LoyaltyCT_TrungThuong_PageChiTiet: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Page}ct_trung_thuong_page_chi_tiet`;
    public static readonly LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}ct_trung_thuong_page_chi_tiet_thong_tin_chuong_trinh`;
    public static readonly LoyaltyCT_TrungThuong_ThongTinChuongTrinh_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_thong_tin_chuong_trinh_chinh_sua`;
    public static readonly LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}ct_trung_thuong_page_chi_tiet_cau_hinh_chuong_trinh`;
    public static readonly LoyaltyCT_TrungThuong_CauHinhChuongTrinh_ChinhSua: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.ButtonAction}ct_trung_thuong_cau_hinh_chuong_trinh_chinh_sua`;
    public static readonly LoyaltyCT_TrungThuong_PageChiTiet_LichSu: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Tab}ct_trung_thuong_page_chi_tiet_lich_su`;
    public static readonly LoyaltyCT_TrungThuong_LichSu_DanhSach: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Table}ct_trung_thuong_lich_su_danh_sach`;

}
    // Khách hàng ----- Start
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_QLKhachHang] = { type: PermissionTypes.Menu, name: 'Khách hàng', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-users' };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan] = { type: PermissionTypes.Menu, name: 'Khách hàng cá nhân', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLKhachHang, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_XuatDuLieu] = { type: PermissionTypes.ButtonAction, name: 'Xuất dữ liệu', parentKey: PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan, webKey: WebKeys.Loyalty};
   
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ThongTinChung_ChiTiet] = { type: PermissionTypes.Tab, name: 'Chi tiết', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ThongTinChung, webKey: WebKeys.Loyalty};

    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai] = { type: PermissionTypes.Tab, name: 'Danh sách ưu đãi', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_UuDai, webKey: WebKeys.Loyalty};

    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_SuKienThamGia] = { type: PermissionTypes.Tab, name: 'Sự kiện tham gia', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet, webKey: WebKeys.Loyalty};

    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_LichSuThamGia] = { type: PermissionTypes.Tab, name: 'Lịch sử tham gia', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_LichSuThamGia_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_LichSuThamGia, webKey: WebKeys.Loyalty};

    // Ưu đãi
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_QLUuDai] = { type: PermissionTypes.Menu, name: 'Ưu đãi', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-ticket' };
    // Danh sách ưu đãi
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai] = { type: PermissionTypes.Menu, name: 'Danh sách ưu dãi', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_DanhDauOrTatNoiBat] = { type: PermissionTypes.ButtonAction, name: 'Đánh dấu/Tắt nổi bật', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_BatOrTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật/Tắt showapp', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/Huỷ', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_DanhSachUuDai_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai, webKey: WebKeys.Loyalty};
    // Lịch sử cấp phát
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_LichSuCapPhat] = { type: PermissionTypes.Menu, name: 'Lịch sử cấp phát', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLUuDai, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_LichSuCapPhat_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_LichSuCapPhat, webKey: WebKeys.Loyalty};
    // Điểm
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_QLDiem] = { type: PermissionTypes.Menu, name: 'Điểm', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-wallet' };
    // Tích điểm
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem] = { type: PermissionTypes.Menu, name: 'Tích điểm', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_UploadDSDiem] = { type: PermissionTypes.ButtonAction, name: 'Upload danh sách điểm', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_DownloadMau] = { type: PermissionTypes.ButtonAction, name: 'Download mẫu', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TichDiem_Huy] = { type: PermissionTypes.ButtonAction, name: 'Huỷ', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TichDiem, webKey: WebKeys.Loyalty};
    // Hạng thành viên
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien] = { type: PermissionTypes.Menu, name: 'Hạng thành viên', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLDiem, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt/Huỷ', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien, webKey: WebKeys.Loyalty};

    // Yêu cầu
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_QLYeuCau] = { type: PermissionTypes.Menu, name: 'Yêu cầu', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-list' };
    // Yêu cầu đổi voucher
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher] = { type: PermissionTypes.Menu, name: 'Yêu cầu đổi điểm', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLYeuCau, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_ThemMoi] = { type: PermissionTypes.Table, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_BanGiao] = { type: PermissionTypes.ButtonAction, name: 'Bàn giao', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_HoanThanh] = { type: PermissionTypes.ButtonAction, name: 'Hoàn thành', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_HuyYeuCau] = { type: PermissionTypes.ButtonAction, name: 'Hủy yêu cầu', parentKey: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher, webKey: WebKeys.Loyalty};


    // Truyền thông
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TruyenThong] = { type: PermissionTypes.Menu, name: 'Truyền thông', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-send' };

    // Truyền thông - Tin Tức
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_TinTuc] = { type: PermissionTypes.Menu, name: 'Tin tức', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TruyenThong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyTinTuc_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TinTuc, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyTinTuc_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TinTuc, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyTinTuc_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TinTuc, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyTinTuc_PheDuyetOrHuyDang] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt đăng', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TinTuc, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyTinTuc_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TinTuc, webKey: WebKeys.Loyalty };


    // Truyền thông - Hình ảnh
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_HinhAnh] = { type: PermissionTypes.Menu, name: 'Hình ảnh', parentKey: PermissionLoyaltyConst.LoyaltyMenu_TruyenThong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyHinhAnh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyHinhAnh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyHinhAnh_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyHinhAnh_PheDuyetOrHuyDang] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt/Huỷ đăng', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyHinhAnh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh, webKey: WebKeys.Loyalty };

    // Menu Thông báo 
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_ThongBao] = { type: PermissionTypes.Menu, name: 'Thông báo', parentKey: null, webKey: WebKeys.Loyalty , icon: 'pi pi-comment'};

    // Thông báo - thông báo hệ thống
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_ThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Cấu hình thông báo', parentKey: PermissionLoyaltyConst.LoyaltyMenu_ThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyThongBaoHeThong_CapNhat] = { type: PermissionTypes.Menu, name: 'Cập nhật cài đặt', parentKey: PermissionLoyaltyConst.LoyaltyMenu_ThongBaoHeThong, webKey: WebKeys.Loyalty };

    // Thông báo - Mẫu thông báo
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_MauThongBao] = { type: PermissionTypes.Menu, name: 'Mẫu thông báo', parentKey: PermissionLoyaltyConst.LoyaltyMenu_ThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMauThongBao_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMauThongBao_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMauThongBao_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMauThongBao_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy kích hoạt', parentKey: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao, webKey: WebKeys.Loyalty };
    // PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMauThongBao_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao, webKey: WebKeys.Loyalty };


    // Thông báo - Quản lý thông báo
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_QLTB] = { type: PermissionTypes.Menu, name: 'Quản lý thông báo', parentKey: PermissionLoyaltyConst.LoyaltyMenu_ThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLTB, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLTB, webKey: WebKeys.Loyalty };
    // PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_KichHoatOrHuy] = { type: PermissionTypes.ButtonAction, name: 'Kích hoạt / Hủy kích hoạt', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLTB, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLTB, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_QLTB, webKey: WebKeys.Loyalty };
    
    // Chi tiết thông báo
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_ThongTin] = { type: PermissionTypes.Form, name: 'Xem chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_ThongTinChung, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_ThongTinChung, webKey: WebKeys.Loyalty };

    // Tab Gửi thông báo
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_GuiThongBao] = { type: PermissionTypes.Menu, name: 'Gửi thông báo', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_GuiThongBao_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách thông báo', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_GuiThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_GuiThongBao_CaiDat] = { type: PermissionTypes.ButtonAction, name: 'Cài đặt danh sách thông báo', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_GuiThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_GuiThongBao_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_GuiThongBao, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet_GuiThongBao_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá khách hàng', parentKey: PermissionLoyaltyConst.LoyaltyQLTB_GuiThongBao, webKey: WebKeys.Loyalty };

    // Báo cáo
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_BaoCao] = { type: PermissionTypes.Menu, name: 'Báo cáo', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-file-export' };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri] = { type: PermissionTypes.Page, name: 'Báo cáo quản trị', parentKey: PermissionLoyaltyConst.LoyaltyMenu_BaoCao };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri_DSYeuCauDoiVoucher] = { type: PermissionTypes.ButtonAction, name: 'B.C Danh sách yêu cầu đổi voucher', parentKey: PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri_XuatNhapTonVoucher] = { type: PermissionTypes.ButtonAction, name: 'B.C Xuất nhập tồn voucher', parentKey: PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri, webKey: WebKeys.Loyalty };

    // Chương trình trúng thưởng
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong] = { type: PermissionTypes.Menu, name: 'Chương trình trúng thưởng', parentKey: null, webKey: WebKeys.Loyalty, icon: 'pi pi-star' };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_CaiDatThamGia] = { type: PermissionTypes.ButtonAction, name: 'Cài đặt tham gia', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xoá', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh] = { type: PermissionTypes.Tab, name: 'Thông tin chương trình', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_ThongTinChuongTrinh_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_ThongTinChuongTrinh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh] = { type: PermissionTypes.Tab, name: 'Cấu hình chương trình', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_CauHinhChuongTrinh_ChinhSua] = { type: PermissionTypes.ButtonAction, name: 'Chỉnh sửa', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_CauHinhChuongTrinh, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet, webKey: WebKeys.Loyalty };
    PermissionLoyaltyConfig[PermissionLoyaltyConst.LoyaltyCT_TrungThuong_LichSu_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet_LichSu, webKey: WebKeys.Loyalty };

export default PermissionLoyaltyConfig;