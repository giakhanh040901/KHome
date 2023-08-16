import { IConstant, IDropdown, ISelectButton } from './interface/InterfaceConst.interface';

export class AppConsts {
  static remoteServiceBaseUrl: string;
  static appBaseUrl: string;
  static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
  static redicrectHrefOpenDocs = 'https://docs.google.com/viewerng/viewer?url=';
  static baseUrlHome: string;
  static nodeBaseUrl: string;

  static readonly grantType = {
    password: 'password',
    refreshToken: 'refresh_token',
  };

  static readonly messageError = 'Có lỗi xảy ra. Vui lòng thử lại sau ít phút!';

  static clientId: string;
  static clientSecret: string;

  static keyCrypt = 'idCrypt';

  static localeMappings: any = [];

  static readonly userManagement = {
    defaultAdminUserName: 'admin',
  };

  static readonly authorization = {
    accessToken: 'access_token',
    refreshToken: 'refresh_token',
    encryptedAuthTokenName: 'enc_auth_token',
  };

  static readonly localRefreshAction = {
    setToken: 'setToken',
    doNothing: 'doNothing',
  };
  static ApproveConst: any;

  static readonly folder = 'loyalty';
  static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";
}

export class PermissionLoyaltyConst {
  public static readonly WEB_LOYALTY = 8;
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
    public static readonly LoyaltyWeb: string = `${PermissionLoyaltyConst.LoyaltyModule}${PermissionLoyaltyConst.Web}`;
    
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

export class DataTableConst {
  public static message = {
    emptyMessage: 'Không có dữ liệu',
    totalMessage: 'bản ghi',
    selectedMessage: 'chọn',
  };
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
  public static TYPE_PARTNERS = ['P', 'RP']; // PARTNERROOT HOẶC PARTNER
  public static TYPE_ROOTS = ['RP', 'RT']; // PARTNERROOT HOẶC TRADINGROOT
  public static TYPE_TRADING = ['T', 'RT']; // PARTNERROOT HOẶC TRADINGROOT

  public static getUserTypeInfo(code, property) {
    let type = this.list.find((t) => t.code == code);
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
  ];

  public static RESPONSE_TRUE = 1;
  public static RESPONSE_FALSE = 0;
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
  ];

  public static DELETE_TRUE = 'Y';
  public static DELETE_FALSE = 'N';
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
    },
  ];

  public static getInfo(code, atribution = 'name') {
    let status = this.list.find((s) => s.code == code);
    return status ? status[atribution] : null;
  }
}

export class UnitDateConst {
  public static list = [
    {
      name: 'Tháng',
      code: 'M',
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

  public static getNameUnitDate(code, atribution = 'name') {
    let unit = this.list.find((item) => item.code == code);
    return unit ? unit[atribution] : null;
  }
}

export const sloganWebConst = [
  // "Việc gì khó, đã có EMIR lo!",
  'Đừng bao giờ sợ thất bại. Bạn chỉ cần đúng có một lần trong đời thôi - CEO của Starbucks!',
  'Luôn bắt đầu với mong đợi những điều tốt đẹp sẽ xảy ra – Tom Hopkins!',
  'Nơi nào không có cạnh tranh, nơi đó không có thị trường!',
  'Kẻ chiến thắng không bao giờ bỏ cuộc; kẻ bỏ cuộc không bao giờ chiến thắng!',
  // "Nhất cận thị, nhì cận giang, muốn giàu sang thì…cận sếp!",
  'Công việc quan trọng nhất luôn ở phía trước, không bao giờ ở phía sau bạn!',
  'Chúng ta có thể gặp nhiều thất bại, nhưng chúng ta không được bị đánh bại!',
  'Điều duy nhất vượt qua được vận may là sự chăm chỉ. – Harry Golden!',
  'Muốn đi nhanh thì đi một mình. Muốn đi xa thì đi cùng nhau!',
  'Đôi lúc bạn đối mặt với khó khăn không phải vì bạn làm điều gì đó sai mà bởi vì bạn đang đi đúng hướng!',
  'Điều quan trọng không phải vị trí đứng mà hướng đi. Mỗi khi có ý định từ bỏ, hãy nghĩ đến lý do mà bạn bắt đầu!',
  'Cách tốt nhất để dự đoán tương lai là hãy tạo ra nó!',
  'Kỷ luật là cầu nối giữa mục tiêu và thành tựu!',
  'Di chuyển nhanh và phá vỡ các quy tắc. Nếu vẫn chưa phá vỡ cái gì, chứng tỏ bạn di chuyển chưa đủ nhanh!',
  'Đằng nào thì bạn cũng phải nghĩ, vì vậy sao không nghĩ lớn luôn? - Donald Trump!',
  'Những khách hàng không hài lòng sẽ là bài học tuyệt vời cho bạn - Bill Gates!',
  'Nếu mọi người thích bạn, họ sẽ lắng nghe bạn, nhưng nếu họ tin tưởng bạn, họ sẽ làm kinh doanh với bạn!',
  'Hoàn cảnh thuận lợi luôn chứa đựng những yếu tố nguy hiểm. Hoàn cảnh khó khăn luôn giúp ta vững vàng hơn!',
];

export class SearchConst {
  public static DEBOUNCE_TIME = 1200;
}

export class FormNotificationConst {
  public static IMAGE_APPROVE = 'IMAGE_APPROVE';
  public static IMAGE_CLOSE = 'IMAGE_CLOSE';
}

export const OBJECT_CONFIRMATION_DIALOG = {
  DEFAULT_IMAGE: {
    IMAGE_APPROVE: 'assets/layout/images/icon-dialog/icon-approve-modalDialog.svg',
    IMAGE_CLOSE: 'assets/layout/images/icon-dialog/icon-close-modalDialog.svg',
  },
};

export const SEVERITY = {
  PRIMARY: 'primary',
  SECONDARY: 'secondary',
  WARNING: 'warning',
  SUCCESS: 'success',
  DANGER: 'danger',
  INFO: 'info'
}

/* #region customer-management */
export class IndividualCustomer {
  public static keyStorage = 'individualCustomer';
  public static keyStorageDetailOffer = 'individualCustomerDetailOffer';
  public static listGender: IDropdown[] = [
    {
      value: 'M',
      label: 'Nam',
    },
    {
      value: 'F',
      label: 'Nữ',
    },
  ];
  public static listVoucherLevel: IDropdown[] = [
    {
      value: 1,
      label: 'Chưa cấp',
    },
    {
      value: 2,
      label: 'Đã cấp',
    },
  ];
  public static listAccountType: IDropdown[] = [
    {
      value: 1,
      label: 'Đã xác thực',
    },
    {
      value: 2,
      label: 'Chưa xác thực',
    },
  ];
  public static listClass: IDropdown[] = [
    {
      value: 1,
      label: 'Vàng',
    },
    {
      value: 2,
      label: 'Bạc',
    },
    {
      value: 3,
      label: 'Đồng',
    },
    {
      value: 4,
      label: 'Chưa xếp hạng',
    },
  ];
  public static listCardType: IDropdown[] = [];
  public static listVoucherType: IDropdown[] = [
    {
      value: 'C',
      label: 'Thẻ cứng',
    },
    {
      value: 'DT',
      label: 'Thẻ điện tử',
    },
  ];
  public static listStatus: IDropdown[] = [
    {
      value: 1,
      label: 'Kích hoạt',
    },
    {
      value: 2,
      label: 'Hủy kích hoạt',
    },
    {
      value: 3,
      label: 'Hết hạn',
    },
    {
      value: 4,
      label: 'Đã xóa',
    },
  ];
  public static listSex: IConstant[] = [
    {
      id: 'M',
      value: 'Nam',
    },
    {
      id: 'F',
      value: 'Nữ',
    },
  ];
  public static getStatus(code: string, atribution = 'label') {
    if (atribution === 'label') return !!code ? 'Đã xác thực' : 'Chưa xác thực';
    if (atribution === 'severity') return !!code ? 'success' : 'danger';
    return '';
  }
  public static KHOI_TAO = 1;
  public static TIEP_NHAN_YC = 2;
  public static DANG_GIAO = 3;
  public static HOAN_THANH = 4;
  public static HUY_YEU_CAU = 5;
  public static HET_HAN_VOUCHER = 6;
  public static listStatusVoucher: IDropdown[] = [
    {
      value: this.KHOI_TAO,
      label: 'Khởi tạo',
      severity: SEVERITY.PRIMARY,
    },
    {
      value: this.TIEP_NHAN_YC,
      label: 'Tiếp nhận Y/C',
      severity: SEVERITY.SECONDARY,
    },
    {
      value: this.DANG_GIAO,
      label: 'Đang giao',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.HOAN_THANH,
      label: 'Hoàn thành',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_YEU_CAU,
      label: 'Hủy yêu cầu',
      severity: SEVERITY.DANGER,
    },
    {
      value: this.HET_HAN_VOUCHER,
      label: 'Hết hạn',
      severity: SEVERITY.INFO,
    },
  ];

  public static getStatusVoucher(code: string, atribution = 'label') {
    let status = this.listStatusVoucher.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }
  public static listSource: IConstant[] = [
    {
      id: 1,
      value: 'Online',
    },
    {
      id: 2,
      value: 'Offline',
    },
  ]
}
/* #endregion */

/* #region voucher-management */
export class VoucherManagement {
  public static keyStorage = 'voucherManagement';
  public static KHOI_TAO = 1;
  public static KICH_HOAT = 2;
  public static HUY_KICH_HOAT = 3;
  public static DA_XOA = 4;
  public static HET_HAN = 5;

  public static listStatus: IDropdown[] = [
    // {
    //   value: this.KHOI_TAO,
    //   label: 'Khởi tạo',
    //   severity: SEVERITY.PRIMARY,
    // },
    {
      value: this.KICH_HOAT,
      label: 'Kích hoạt',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_KICH_HOAT,
      label: 'Hủy kích hoạt',
      severity: SEVERITY.SECONDARY,
    },
    {
      value: this.DA_XOA,
      label: 'Đã xóa',
      severity: SEVERITY.DANGER,
    },
    {
      value: this.HET_HAN,
      label: 'Hết hạn',
      severity: SEVERITY.INFO,
    },
  ];

  public static getStatus(code: string, atribution = 'label') {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listKindOfVoucher: IDropdown[] = [
    {
      value: 'C',
      label: 'Thẻ cứng',
    },
    {
      value: 'DT',
      label: 'Thẻ điện tử',
    },
  ];

  public static listTypeOfVoucher: IDropdown[] = [
    {
      value: 'TD',
      label: 'Tiêu dùng',
    },
    {
      value: 'MS',
      label: 'Mua sắm',
    },
    {
      value: 'AT',
      label: 'Ẩm thực',
    },
    {
      value: 'DV',
      label: 'Dịch vụ',
    },
  ];

  public static listShowApp: IDropdown[] = [
    {
      value: 'Y',
      label: 'Có',
    },
    {
      value: 'N',
      label: 'Không',
    },
  ];

  public static VND = 'VND';
  public static PERCENT = 'P';
  public static listValueUnit: IDropdown[] = [
    {
      value: this.VND,
      label: 'VND',
    },
    {
      value: this.PERCENT,
      label: '%',
    },
  ];

  public static CUSTOMER = {
    INDIVIDUAL: 0, // cá nhân
    BUSINESS: 1, // doanh nghiệp
  };
}
/* #endregion */

/* #region prize-draw-management */
export class PrizeDrawManagement{
  public static keyStorage = 'prizeDrawManagementLoyalty';
  public static KICH_HOAT = 1;
  public static TAM_DUNG = 2;
  public static HET_HAN = 3;

  public static listStatus: IDropdown[] = [
    {
      value: this.KICH_HOAT,
      label: 'Kích hoạt',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.TAM_DUNG,
      label: 'Tạm dừng',
      severity: SEVERITY.DANGER,
    },
    {
      value: this.HET_HAN,
      label: 'Hết hạn',
      severity: SEVERITY.SECONDARY,
    },
  ];

  public static EXPIRED_STATUS = {
    label: 'Hết hạn',
    severity: SEVERITY.SECONDARY,
  }

  public static getInfoStatus(row, field) {
    if (row.isExpired){
      return this.EXPIRED_STATUS[field] ?? null;
    }
    const status = this.listStatus.find(type => type.value == row.status);
    return status ? status[field] : null;
  }

  public static listTime: IDropdown[] = [
    {
      label: 'Giờ',
      value: 'H'
    },
    {
      label: 'Ngày',
      value: 'D'
    },
    {
      label: 'Tuần',
      value: 'W'
    },
    {
      label: 'Tháng',
      value: 'M'
    },
    {
      label: 'Năm',
      value: 'Y'
    },
  ];


  public static TIMELINE = 1;
  public static CUSTOMER_JOIN = 2;

  public static joinTimeSettings = [
    {
      label: 'Tính theo mốc thời gian',
      value: this.TIMELINE
    },
    {
      label: 'Tính theo thời điểm khi khách hàng tham gia',
      valuel: this.CUSTOMER_JOIN
    }
  ];

  public static DEFAULT_VOUCHER = {
    label: 'Chúng bạn may mắn lần sau',
    value: -1
  }

  public static VONG_QUAY_MAY_MAN = 1;

  public static luckyScenarioTypes = [
    {
      label: "Vòng quay may mắn",
      value: this.VONG_QUAY_MAY_MAN
    }
  ]; 

  public static getNameLuckyScenarioType(value){
    const status = this.luckyScenarioTypes.find(type => type.value == value);
    return status ? status.label : null;
  }

  public static listJoinStatus: IDropdown[] = [
    {
      value: false,
      label: 'Chưa tham gia',
      severity: SEVERITY.WARNING,
    },
    {
      value: true,
      label: 'Đã tham gia',
      severity: SEVERITY.SUCCESS,
    },
  ];

  public static getStatus(code: string, atribution = 'label') {
    let status = this.listJoinStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static customerTypes = [
    {
      value: 1,
      label: 'Sale'
    },
    {
      value: 2,
      label: 'Khách hàng đã đầu tư'
    },
    {
      value: 3,
      label: 'Khách hàng chưa đầu tư'
    },
  ]
}
/* #endregion */



export const DEFAULT_MEDIA_RST = {
  DEFAULT_IMAGE: {
    IMAGE_ADD: 'assets/layout/images/add-image-bg.png',
  },
};

export enum EConfigDataModal {
  VIEW = 'view',
  CREATE = 'create',
  EDIT = 'edit',
}
export const DEFAULT_WIDTH = 100;
export const DEFAULT_HEIGHT = 100;
export const MARKDOWN_OPTIONS = {
  MARKDOWN: 'MARKDOWN',
  HTML: 'HTML',
};
export const HTML_MARKDOWN_OPTIONS: ISelectButton[] = [
  {
    label: 'MARKDOWN',
    value: MARKDOWN_OPTIONS.MARKDOWN,
  },
  {
    label: 'HTML',
    value: MARKDOWN_OPTIONS.HTML,
  },
];
export const TYPE_UPLOAD = {
  EXCEL: '.xlsx,.xls',
};
export enum ETypeDataTable {
  INDEX = 1,
  TEXT = 2,
  ACTION_BUTTON = 3,
  STATUS = 4,
  ACTION = 5,
  CHECK_BOX = 6,
  CHECKBOX_ACTION = 7,
}
export enum EPositionTextCell {
  CENTER = 'center',
  LEFT = 'start',
  RIGHT = 'end',
}
export enum EPositionFrozenCell {
  LEFT = 'left',
  RIGHT = 'right',
}

export class MediaNewsConst {

  public static positionList = [
      {
          name: "Banner popup",
          code: 'banner_popup'
      },
      {
          name: "Popup sinh nhật",
          code: 'popup_birthday'
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
          name: "Vì sao sử dụng epic",
          code: 'vi_sao_su_dung_epic'

      },
      {
          name: "Video",
          code: 'videos'

      }
  ];

  public static getPositionName(code) {
      for (let item of this.positionList) {
          if (item.code == code) return item.name;
      }
      return '';
  }

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
          severity: 'dark'
      },
      {
          name: "Bản nháp",
          code: 'DRAFT',
          severity: 'secondary'
      }
  ];
  public static categoryList = [
      {
          name: "Tài chính cá nhân",
          code: 'FINANCE'
      },
      {
          name: "Cẩm nang",
          code: 'CAM_NANG'
      },
      {
          name: "Xu hướng",
          code: 'TRENDING'
      },
      {
          name: "Đầu tư",
          code: 'INVESTMENT'
      },
      {
          name: "Bảo mật",
          code: 'BAO_MAT'
      },
      {
          name: "Quy trình xử lý",
          code: 'QUY_TRINH_XU_LY'
      }
  ];

}

export class MediaConst {

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

  public static newsTypes = {
      PURE_NEWS: 'Tin tức', //Tin tức thuần
      SHARING: 'Chia sẻ từ KOL', //Chia sẻ từ KOL
      PROMOTION: 'Ưu đãi' //Ưu đãi
  }
  //[ 'FINANCE', 'TRENDING', 'INVESTMENT' ]
  public static knowledgeCategories = {
      FINANCE: 'Tài chính cá nhân', //Tin tức thuần
      TRENDING: 'Xu hướng', //Chia sẻ từ KOL
      INVESTMENT: 'Đầu tư', //Ưu đãi,
      CAM_NANG: 'Cẩm nang',
      BAO_MAT: 'Bảo mật',
      QUY_TRINH_XU_LY: 'Quy trình xử lý'
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

  public static getKnowledgeBaseCategory(code) {
      return this.knowledgeCategories[code]
  }

  public static ACTIVE = 'ACTIVE';
  public static TRINH_DUYET = 'PENDING';
  public static NHAP = 'DRAFT';

}
export const RESIZE_TEXTAREA_TYPE = {
  HORIZONTAL: 'horizontal',
  VERTICAL: 'vertical',
};
export const MIN_HEIGHT_RESIZE_TEXTAREA = 50;

/* #region point-management */
export class AccumulatePointManegement {
  public static keyStorage = 'accumulatePointManegement';
  public static TICH_DIEM = 1;
  public static TIEU_DIEM = 2;
  public static KHOI_TAO = 6;
  public static HOAN_THANH = 4;
  public static HUY_YEU_CAU = 5;
  public static listType: IDropdown[] = [
    {
      value: this.TICH_DIEM,
      label: 'Tích điểm',
    },
    {
      value: this.TIEU_DIEM,
      label: 'Tiêu điểm',
    },
  ];
  public static listStatus: IDropdown[] = [
    {
      value: this.KHOI_TAO,
      label: 'Khởi tạo',
      severity: SEVERITY.PRIMARY,
    },
    {
      value: this.HOAN_THANH,
      label: 'Hoàn thành',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_YEU_CAU,
      label: 'Hủy yêu cầu',
      severity: SEVERITY.DANGER,
    },
  ];
  public static getStatus(code: string, atribution = 'label') {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }
}

export class MembershipLevelManagement {
  public static keyStorage = 'membershipLevelManagement';
  public static KHOI_TAO = 1;
  public static KICH_HOAT = 2;
  public static HUY_KICH_HOAT = 3;
  public static listStatus: IDropdown[] = [
    {
      value: this.KHOI_TAO,
      label: 'Khởi tạo',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.KICH_HOAT,
      label: 'Kích hoạt',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_KICH_HOAT,
      label: 'Hủy kích hoạt',
      severity: SEVERITY.DANGER,
    },
  ];
  public static getStatus(code: string, atribution = 'label') {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }
}
/* #endregion */

/* #region voucher-management */
export class ChangeVoucherRequest {
  public static keyStorage = 'changeVoucherRequest';
  public static KHOI_TAO = 1;
  public static TIEP_NHAN_YC = 2;
  public static DANG_GIAO = 3; 
  public static HOAN_THANH = 4;
  public static HUY_YEU_CAU = 5;

  public static listStatus: IDropdown[] = [
    {
      value: this.KHOI_TAO,
      label: 'Khởi tạo',
      severity: SEVERITY.PRIMARY,
    },
    {
      value: this.TIEP_NHAN_YC,
      label: 'Tiếp nhận Y/C',
      severity: SEVERITY.SECONDARY,
    },
    {
      value: this.DANG_GIAO,
      label: 'Đang giao',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.HOAN_THANH,
      label: 'Hoàn thành',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_YEU_CAU,
      label: 'Hủy yêu cầu',
      severity: SEVERITY.DANGER,
    },
  ];
  public static getStatus(code: string, atribution = 'label') {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  };
  public static listSource: IConstant[] = [
    {
      id: 1,
      value: 'Online',
    },
    {
      id: 2,
      value: 'Offline',
    },
  ];
  public static DOI_VOUCHER = 1;
  public static listRequestType: IDropdown[] = [
    {
      value: this.DOI_VOUCHER,
      label: "Đổi voucher",
    },
    {
      value: 2,
      label: "Đổi điểm",
    },
  ];
  public static TANG_KHACH_HANG = 2;
  public static listApplyType: IDropdown[] = [
    {
      value: 1,
      label: "Khách hàng đổi điểm",
    },
    {
      value: this.TANG_KHACH_HANG,
      label: "Tặng khách hàng",
    },
  ];
}
/* #endregion */

export class NotificationTemplateConst{

  public static navigationTypes = [
      { 
        value: 'IN_APP',
        name: 'In App',
        buttonNavigation: 'Giao dịch ngay',
      },
      {
        value: 'LIEN_KET_KHAC',
        name: 'Liên kết khác',
        buttonNavigation: 'Khám phá ngay'
      }
    ];

  public static getButtonNavigation(code, opt) {
      if(code == NotificationTemplateConst.LIEN_KET_KHAC) {
          const status = this.navigationTypes.find(item => item.value == code);
          return status ? status.buttonNavigation : '';
      } else {
          const status = this.levelOneOptions.find(item => item.value == opt);
          return status ? status.buttonNavigation : '';
      }
  }

  public static status = [
      {
          name: 'Kích hoạt',
          code: 'ACTIVE',
          severity: 'success'
      },
      {
          name: 'Huỷ kích hoạt',
          code: 'INACTIVE',
          severity: 'secondary'
      },
  ];

  public static HUY_KICH_HOAT = 'INACTIVE';
  public static KICH_HOAT = 'ACTIVE';

  public static IN_APP = 'IN_APP';
  public static LIEN_KET_KHAC = 'LIEN_KET_KHAC';

  public static voucherOptions = [
      {
          value: 'Màn đổi điểm Voucher',
          secondLevel: 'Màn đổi điểm Voucher',
      },
      {
          value: 'Sử dụng Voucher',
          secondLevel: 'Sử dụng Voucher',
       
      },
  ];

  public static DAU_TU_TAI_CHINH = 'DAU_TU_TAI_CHINH';
  public static DAU_TU_TICH_LUY = 'DAU_TU_TICH_LUY';
  public static GIAO_DICH_BDS = 'GIAO_DICH_BDS';
  public static VOUCHER = 'VOUCHER';

  public static levelOneOptions = [
      { 
      value: this.DAU_TU_TAI_CHINH,
      name: 'Đầu tư tài chính',
      buttonNavigation: 'Giao dịch ngay',
      },
      {
      value: this.DAU_TU_TICH_LUY,
      name: 'Đầu tư tích lũy',
      buttonNavigation: 'Giao dịch ngay',
      },
      {
      value: this.GIAO_DICH_BDS,
      name: 'Giao dịch bất động sản',
      buttonNavigation: 'Giao dịch ngay',
      },
      {
      value: 'DANG_KY_TU_VAN_VIEN',
      name: 'Đăng ký Tư vấn viên',
      buttonNavigation: 'Đăng ký Tư vấn viên',
      secondLevel: 'Màn đăng ký CTV',
      },
      {
      value: 'CHIA_SE_MA_QR',
      name: 'Chia sẻ mã QR',
      buttonNavigation: 'Khám phá ngay',
      secondLevel: 'Màn QR của tôi',
      },
      {
      value: 'VOUCHER',
      name: 'Voucher',
      buttonNavigation: 'Khám phá ngay',
      secondLevel: 'Màn đổi điểm Voucher, Sử dụng Voucher',
      },
      {
      value: 'EVENT',
      name: 'Event',
      buttonNavigation: 'Khám phá ngay', 
      secondLevel: 'Đăng ký tham gia Envent',
      },
  ];
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

export class NotifyManagerConst { 

  public static typeSelecteds = [
      {
          code: 1,
          name: 'Từng trang',
      },
      {
          code: 2,
          name: 'Tất cả',
      },
  ]

  public static TYPE_SELECTED_PAGE = 1;
  public static TYPE_SELECTED_FULL = 2;

  //
  public static fieldSearchs = [
      {
          name: 'Số điện thoại',
          code: 1,
          field: 'phone',
          placeholder: 'Nhập số điện thoại...',
      },
      {
          name: 'Họ tên',
          code: 2,
          field: 'fullname',
          placeholder: 'Nhập họ tên...',
      },
      
      {
          name: 'Email',
          code: 3,
          field: 'email',
          placeholder: 'Nhập email...',
      },
     
  ];
  
  public static getFieldSearchInfo(field, attribute: string) {
      const fieldSearch = this.fieldSearchs.find(fields => fields.field == field);
      return fieldSearch ? fieldSearch[attribute] : null;
  }

  public static fieldSearchsNodeJS = [
      {
          name: 'Số điện thoại',
          code: 1,
          field: 'phoneNumber',
          placeholder: 'Nhập số điện thoại...',
      },
      {
          name: 'Họ tên',
          code: 2,
          field: 'fullName',
          placeholder: 'Nhập họ tên...',
      },
      
      {
          name: 'Email',
          code: 3,
          field: 'email',
          placeholder: 'Nhập email...',
      },
     
  ];

  public static getFieldSearchInfoNodeJS(field, attribute: string) {
      const fieldSearch = this.fieldSearchsNodeJS.find(fields => fields.field == field);
      return fieldSearch ? fieldSearch[attribute] : null;
  }


  //
  public static customerTypes = [
      {
          name: 'Khách hàng',
          code: 1,
      },
      {
          name: 'Tài khoản chưa xác minh',
          code: 2,
      },
      {
          name: 'Sale',
          code: 3,
      }
  ]

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

  public static isProConst = [
      {
          name: 'Chuyên nghiệp',
          code: 'Y'

      },
      {
          name: "Không chuyên",
          code: 'N'
      }
  ]

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

}

export const COMPARE_TYPE = {
  EQUAL: 1,
  GREATER: 2,
  LESS: 3,
  GREATER_EQUAL: 4,
  LESS_EQUAL: 5,
}

export enum ETypeFormatDate {
  DATE = 'DD/MM/YYYY',
  DATE_TIME = 'DD/MM/YYYY HH:mm',
  DATE_TIME_SECOND = 'DD/MM/YYYY HH:mm:ss',
  DATE_YMD = 'YYYY/MM/DD',
  DATE_TIME_VIEW = 'YYYY-MM-DDTHH:mm',
}

export enum YES_NO {
  YES = 'Y',
  NO = 'N',
}

export enum ETypeSortTable {
  ASCENDING = 1,
  DESCENDING = -1,
  ASC = 'asc',
  DESC = 'desc',
}

export class ExportReportConst {
  public static BC_DS_YC_DOI_VOUCHER = "report-conversion-point";
  public static BC_XUAT_NHAT_TON_VOUCHER = "report-voucher";
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

export class GenderCost{
  public static MALE = 'M';
  public static FEMALE = 'F';

  public static genders = [
      {
          label: 'Nam',
          value: this.MALE
      },
      {
          label: 'Nữ',
          value: this.FEMALE
      },
  ];

  public static getGenderName(code){
      const gender = this.genders.find(s => s.value == code);
      return gender ? gender.label : null;
  }
}

export class MessageErrorConst {

  public static message = {
      Error: "ACTIVE",
      Validate: "Vui lòng nhập đủ thông tin cho các trường có dấu (*)",
      DataNotFound: "Không tìm thấy dữ liệu!"
  }
  
}