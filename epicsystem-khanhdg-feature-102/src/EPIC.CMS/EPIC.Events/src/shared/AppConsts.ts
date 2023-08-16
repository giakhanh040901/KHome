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

  static readonly folder = 'events';
  static defaultAvatar = "assets/layout/images/topbar/anonymous-avatar.jpg";

  static readonly permissionInWeb = 9;
}

export class PermissionEventConst {
    
  private static readonly Web: string = "web_";
  private static readonly Menu: string = "menu_";
  private static readonly Tab: string = "tab_";
  private static readonly Page: string = "page_";
  private static readonly Table: string = "table_";
  private static readonly Form: string = "form_";
  private static readonly ButtonTable: string = "btn_table_";
  private static readonly ButtonForm: string = "btn_form_";
  private static readonly ButtonAction: string = "btn_action_";

  public static readonly EventModule: string = "event.";
  
  // CÀI ĐẶT
  public static readonly Menu_CaiDat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}CaiDat`;
  // CẤU TRÚC MÃ HỢP ĐỒNG
  public static readonly Menu_CauTrucMaHD: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}CauTrucMaHD`;
  public static readonly CauTrucMaHD_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}CauTrucMaHD_DanhSach`;
  public static readonly CauTrucMaHD_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}CauTrucMaHD_ThemMoi`;
  public static readonly CauTrucMaHD_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}CauTrucMaHD_ChiTiet`;
  public static readonly CauTrucMaHD_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}CauTrucMaHD_CapNhat`;
  public static readonly CauTrucMaHD_DoiTrangThai: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}CauTrucMaHD_DoiTrangThai`;
  
  // THÔNG BÁO HỆ THỐNG
  public static readonly Menu_ThongBaoHeThong: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}ThongBaoHeThong`;
  public static readonly ThongBaoHeThong_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}ThongBaoHeThong_DanhSach`;
  public static readonly ThongBaoHeThong_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}ThongBaoHeThong_CapNhat`;

  // QUẢN LÝ SỰ KIỆN
  public static readonly Menu_QuanLySuKien: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}QuanLySuKien`;
  
  // TỔNG QUAN SỰ KIỆN
  public static readonly Menu_TongQuanSuKien: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}TongQuanSuKien`;
  public static readonly TongQuanSuKien_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_DanhSach`;
  public static readonly TongQuanSuKien_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ThemMoi`;
  public static readonly TongQuanSuKien_MoBanVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_MoBanVe`;
  public static readonly TongQuanSuKien_BatTatShowApp: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_BatTatShowApp`;
  public static readonly TongQuanSuKien_TamDung_HuySuKien: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_TamDungOrHuySuKien`;
  public static readonly TongQuanSuKien_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet`;
  
  // TỔNG QUAN SỰ KIỆN - THÔNG TIN CHUNG
  public static readonly TongQuanSuKien_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_ThongTinChung`;
  public static readonly TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet`;
  public static readonly TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat`;
  
  // TỔNG QUAN SỰ KIỆN - QUẢN LÝ
  public static readonly TongQuanSuKien_ChiTiet_QuanLy: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_QuanLy`;
  public static readonly TongQuanSuKien_ChiTiet_QuanLy_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_ChiTiet_QuanLy_DanhSach`;
  public static readonly TongQuanSuKien_ChiTiet_QuanLy_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_QuanLy_CapNhat`;
  
  // TỔNG QUAN SỰ KIỆN - THÔNG TIN MÔ TẢ
  public static readonly TongQuanSuKien_ChiTiet_ThongTinMoTa: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_ThongTinMoTa`;
  public static readonly TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet`;
  public static readonly TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat`;
  
  // TỔNG QUAN SỰ KIỆN - THỜI GIAN VÀ LOẠI VÉ
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe`;
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach`;
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi`;
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet`;
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat`;
  public static readonly TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai`;
  
  // TỔNG QUAN SỰ KIỆN - HÌNH ẢNH SỰ KIỆN
  public static readonly TongQuanSuKien_ChiTiet_HinhAnhSuKien: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_HinhAnhSuKien`;
  public static readonly TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh`;
  public static readonly TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat`;
  
  // TỔNG QUAN SỰ KIỆN - MẪU GIAO NHẬN VÉ
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_MauGiaoNhanVe`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe`;
  public static readonly TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai`;
  
  // TỔNG QUAN SỰ KIỆN - MẪU VÉ
  public static readonly TongQuanSuKien_ChiTiet_MauVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}TongQuanSuKien_ChiTiet_MauVe`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}TongQuanSuKien_ChiTiet_MauVe_DanhSach`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_ThemMoi`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_ChiTiet`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_CapNhat`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_XemThuMau: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_XemThuMau`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_TaiMauVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_TaiMauVe`;
  public static readonly TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai`;

  // QUẢN LÝ BÁN VÉ
  public static readonly Menu_QuanLyBanVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}QuanLyBanVe`;
  
  // SỔ LỆNH
  public static readonly Menu_SoLenh: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}SoLenh`;
  public static readonly SoLenh_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}SoLenh_DanhSach`;
  public static readonly SoLenh_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ThemMoi`;
  public static readonly SoLenh_GiaHanGiulenh: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_GiaHanGiuLenh`;
  public static readonly SoLenh_Xoa: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_Xoa`;
  
  // CHI TIẾT SỔ LỆNH
  public static readonly SoLenh_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}SoLenh_ChiTiet`;
  
  // SỔ LỆNH - THÔNG TIN CHUNG
  public static readonly SoLenh_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}SoLenh_ChiTiet_ThongTinChung`;
  public static readonly SoLenh_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}SoLenh_ChiTiet_ThongTinChung_ChiTiet`;
  public static readonly SoLenh_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_ThongTinChung_CapNhat`;

  // SỔ LỆNH - GIAO DỊCH
  public static readonly SoLenh_ChiTiet_GiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}SoLenh_ChiTiet_GiaoDich`;
  public static readonly SoLenh_ChiTiet_GiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}SoLenh_ChiTiet_GiaoDich_DanhSach`;
  public static readonly SoLenh_ChiTiet_GiaoDich_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_ThemMoi`;
  public static readonly SoLenh_ChiTiet_GiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_ChiTiet`;
  public static readonly SoLenh_ChiTiet_GiaoDich_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_CapNhat`;
  public static readonly SoLenh_ChiTiet_GiaoDich_GuiThongBao: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_GuiThongBao`;
  public static readonly SoLenh_ChiTiet_GiaoDich_PheDuyet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_PheDuyet`;
  public static readonly SoLenh_ChiTiet_GiaoDich_HuyThanhToan: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_GiaoDich_HuyThanhToan`;

  // SỔ LỆNH - DANH SÁCH VÉ
  public static readonly SoLenh_ChiTiet_DanhSachVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}SoLenh_ChiTiet_DanhSachVe`;
  public static readonly SoLenh_ChiTiet_DanhSachVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}SoLenh_ChiTiet_DanhSachVe_DanhSach`;
  public static readonly SoLenh_ChiTiet_DanhSachVe_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_DanhSachVe_TaiVe`;
  public static readonly SoLenh_ChiTiet_DanhSachVe_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}SoLenh_ChiTiet_DanhSachVe_XemVe`;
  
  // SỔ LỆNH - LỊCH SỬ
  public static readonly SoLenh_ChiTiet_LichSu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}SoLenh_ChiTiet_LichSu`;

  // XỬ LÝ GIAO DỊCH
  public static readonly Menu_XuLyGiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}XuLyGiaoDich`;
  public static readonly XuLyGiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}XuLyGiaoDich_DanhSach`;
  public static readonly XuLyGiaoDich_PheDuyetMuaVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_PheDuyetMuave`;
  
  // CHI TIẾT XỬ LÝ GIAO DỊCH
  public static readonly XuLyGiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}XuLyGiaoDich_ChiTiet`;
  
  // XỬ LÝ GIAO DỊCH - THÔNG TIN CHUNG
  public static readonly XuLyGiaoDich_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}XuLyGiaoDich_ChiTiet_ThongTinChung`;
  public static readonly XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet`;
  public static readonly XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat`;

  // XỬ LÝ GIAO DỊCH - GIAO DỊCH
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}XuLyGiaoDich_ChiTiet_GiaoDich`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet`;
  public static readonly XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan`;

  // XỬ LÝ GIAO DỊCH - DANH SÁCH VÉ
  public static readonly XuLyGiaoDich_ChiTiet_DanhSachVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}XuLyGiaoDich_ChiTiet_DanhSachVe`;
  public static readonly XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach`;
  public static readonly XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe`;
  public static readonly XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe`;
  
  // XỬ LÝ GIAO DỊCH - LỊCH SỬ
  public static readonly XuLyGiaoDich_ChiTiet_LichSu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}XuLyGiaoDich_ChiTiet_LichSu`;
  
  // VÉ BÁN HỢP LỆ
  public static readonly Menu_VeBanHopLe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}VeBanHopLe`;
  public static readonly VeBanHopLe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}VeBanHopLe_DanhSach`;
  public static readonly VeBanHopLe_YeuCauHoaDon: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_YeuCauHoaDon`;
  public static readonly VeBanHopLe_YeuCauVeCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_YeuCauVeCung`;
  public static readonly VeBanHopLe_ThongBaoVeHopLe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ThongBaoVeHopLe`;
  public static readonly VeBanHopLe_KhoaYeuCau: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_KhoaYeuCau`;
  
  // CHI TIẾT VÉ BÁN HỢP LỆ
  public static readonly VeBanHopLe_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}VeBanHopLe_ChiTiet`;
  
  // VÉ BÁN HỢP LỆ - THÔNG TIN CHUNG
  public static readonly VeBanHopLe_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}VeBanHopLe_ChiTiet_ThongTinChung`;
  public static readonly VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet`;
  public static readonly VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu`;

  // VÉ BÁN HỢP LỆ - GIAO DỊCH
  public static readonly VeBanHopLe_ChiTiet_GiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}VeBanHopLe_ChiTiet_GiaoDich`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}VeBanHopLe_ChiTiet_GiaoDich_DanhSach`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_GiaoDich_ThemMoi`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_GiaoDich_ChiTiet`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_GiaoDich_CapNhat`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_PheDuyet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_GiaoDich_PheDuyet`;
  public static readonly VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan`;

  // VÉ BÁN HỢP LỆ - DANH SÁCH VÉ
  public static readonly VeBanHopLe_ChiTiet_DanhSachVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}VeBanHopLe_ChiTiet_DanhSachVe`;
  public static readonly VeBanHopLe_ChiTiet_DanhSachVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}VeBanHopLe_ChiTiet_DanhSachVe_DanhSach`;
  public static readonly VeBanHopLe_ChiTiet_DanhSachVe_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_DanhSachVe_TaiVe`;
  public static readonly VeBanHopLe_ChiTiet_DanhSachVe_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}VeBanHopLe_ChiTiet_DanhSachVe_XemVe`;
  
  // VÉ BÁN HỢP LỆ - LỊCH SỬ
  public static readonly VeBanHopLe_ChiTiet_LichSu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}VeBanHopLe_ChiTiet_LichSu`;

  // QUẢN LÝ THAM GIA
  public static readonly Menu_QuanLyThamGia: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}QuanLyThamGia`;
  public static readonly QuanLyThamGia_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}QuanLyThamGia_DanhSach`;
  public static readonly QuanLyThamGia_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}QuanLyThamGia_XemVe`;
  public static readonly QuanLyThamGia_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}QuanLyThamGia_TaiVe`;
  public static readonly QuanLyThamGia_XacNhanThamGia: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}QuanLyThamGia_XacNhanThamGia`;
  public static readonly QuanLyThamGia_MoKhoaVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}QuanLyThamGia_MoKhoaVe`;

  // YÊU CẦU VÉ CỨNG
  public static readonly Menu_YeuCauVeCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}YeuCauVeCung`;
  public static readonly YeuCauVeCung_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauVeCung_DanhSach`;
  public static readonly YeuCauVeCung_XuatMauGiaoVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_XuatMauGiaoVe`;
  public static readonly YeuCauVeCung_XuatVeCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_XuatVeCung`;
  public static readonly YeuCauVeCung_DoiTrangThai_DangGiao: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_DoiTrangThai_DangGiao`;
  public static readonly YeuCauVeCung_DoiTrangThai_HoanThanh: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_DoiTrangThai_HoanThanh`;

  public static readonly YeuCauVeCung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}YeuCauVeCung_ChiTiet`;
  // YÊU CẦU VÉ CỨNG - CHI TIẾT - THÔNG TIN CHUNG
  public static readonly YeuCauVeCung_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauVeCung_ChiTiet_ThongTinChung`;
  public static readonly YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet`;
  // public static readonly YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauHoaDon: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauHoaDon`;
  // public static readonly YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauVeCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_ThongTinChung_YeuCauVeCung`;
  // public static readonly YeuCauVeCung_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_ThongTinChung_CapNhat`;

  // YÊU CẦU VÉ CỨNG - CHI TIẾT - GIAO DỊCH
  public static readonly YeuCauVeCung_ChiTiet_GiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauVeCung_ChiTiet_GiaoDich`;
  public static readonly YeuCauVeCung_ChiTiet_GiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauVeCung_ChiTiet_GiaoDich_DanhSach`;
  // public static readonly YeuCauVeCung_ChiTiet_GiaoDich_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_GiaoDich_ThemMoi`;
  public static readonly YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet`;
  // public static readonly YeuCauVeCung_ChiTiet_GiaoDich_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_GiaoDich_CapNhat`;
  // public static readonly YeuCauVeCung_ChiTiet_GiaoDich_PheDuyet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_GiaoDich_PheDuyet`;
  // public static readonly YeuCauVeCung_ChiTiet_GiaoDich_HuyThanhToan: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_GiaoDich_HuyThanhToan`;

  // YÊU CẦU VÉ CỨNG - CHI TIẾT - DANH SÁCH VÉ
  public static readonly YeuCauVeCung_ChiTiet_DanhSachVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauVeCung_ChiTiet_DanhSachVe`;
  public static readonly YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach`;
  public static readonly YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe`;
  public static readonly YeuCauVeCung_ChiTiet_DanhSachVe_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauVeCung_ChiTiet_DanhSachVe_XemVe`;
  
  // YÊU CẦU VÉ CỨNG - CHI TIẾT - LỊCH SỬ
  public static readonly YeuCauVeCung_ChiTiet_LichSu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauVeCung_ChiTiet_LichSu`;
  
  // YÊU CẦU HÓA ĐƠN
  public static readonly Menu_YeuCauHoaDon: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}YeuCauHoaDon`;
  public static readonly YeuCauHoaDon_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauHoaDon_DanhSach`;
  public static readonly YeuCauHoaDon_DoiTrangThai_DangGiao: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_DoiTrangThai_DangGiao`;
  public static readonly YeuCauHoaDon_DoiTrangThai_HoanThanh: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_DoiTrangThai_HoanThanh`;

  public static readonly YeuCauHoaDon_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}YeuCauHoaDon_ChiTiet`;
  // YÊU CẦU HÓA ĐƠN - CHI TIẾT - THÔNG TIN CHUNG
  public static readonly YeuCauHoaDon_ChiTiet_ThongTinChung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauHoaDon_ChiTiet_ThongTinChung`;
  public static readonly YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Page}YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet`;
  // public static readonly YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauHoaDon: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauHoaDon`;
  // public static readonly YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauVeCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_ThongTinChung_YeuCauVeCung`;
  // public static readonly YeuCauHoaDon_ChiTiet_ThongTinChung_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_ThongTinChung_CapNhat`;

  // YÊU CẦU HÓA ĐƠN - CHI TIẾT - GIAO DỊCH
  public static readonly YeuCauHoaDon_ChiTiet_GiaoDich: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauHoaDon_ChiTiet_GiaoDich`;
  public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach`;
  // public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_ThemMoi: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_GiaoDich_ThemMoi`;
  public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet`;
  // public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_CapNhat: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_GiaoDich_CapNhat`;
  // public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_PheDuyet: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_GiaoDich_PheDuyet`;
  // public static readonly YeuCauHoaDon_ChiTiet_GiaoDich_HuyThanhToan: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_GiaoDich_HuyThanhToan`;

  // YÊU CẦU HÓA ĐƠN - CHI TIẾT - DANH SÁCH VÉ
  public static readonly YeuCauHoaDon_ChiTiet_DanhSachVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Tab}YeuCauHoaDon_ChiTiet_DanhSachVe`;
  public static readonly YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach`;
  public static readonly YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe`;
  public static readonly YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe: string = `${PermissionEventConst.EventModule}${PermissionEventConst.ButtonAction}YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe`;
  
  // YÊU CẦU HÓA ĐƠN - CHI TIẾT - LỊCH SỬ
  public static readonly YeuCauHoaDon_ChiTiet_LichSu: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Table}YeuCauHoaDon_ChiTiet_LichSu`;

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
  INFO: 'info',
};

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
  SELECT_CHECK_BOX = 7,
  COUNT_DOWN_TIME = 8,
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

export const RESIZE_TEXTAREA_TYPE = {
  HORIZONTAL: 'horizontal',
  VERTICAL: 'vertical',
};

export const MIN_HEIGHT_RESIZE_TEXTAREA = 50;
export const COMPARE_TYPE = {
  EQUAL: 1,
  GREATER: 2,
  LESS: 3,
  GREATER_EQUAL: 4,
  LESS_EQUAL: 5,
};

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
  ASC = 'asc',
  DESC = 'desc',
}

export enum ETypeStatus {
  SEVERITY = 'severity',
  LABEL = 'label',
}

/* #region setting-contract-code */
export class SettingContractCode {
  public static keyStorage = 'settingContractCode';
  public static DANG_SU_DUNG = 'A';
  public static HUY_KICH_HOAT = 'D';

  public static listStatus: IDropdown[] = [
    {
      value: this.DANG_SU_DUNG,
      label: 'Kích hoạt',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_KICH_HOAT,
      label: 'Hủy kích hoạt',
      severity: SEVERITY.WARNING,
    },
    {
      value: 'D',
      label: 'Đã xóa',
      severity: SEVERITY.WARNING,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static KY_TU = 'FIX_TEXT';
  public static listValueContractCode: IDropdown[] = [
    {
      value: this.KY_TU,
      label: 'Ký tự',
    },
    {
      value: 'ORDER_ID',
      label: 'Số thứ tự hợp đồng',
    },
    {
      value: 'ORDER_ID_PREFIX_0',
      label: 'Số thứ tự đầy đủ',
    },
    {
      value: 'INVEST_DATE',
      label: 'Ngày đặt lệnh',
    },
    {
      value: 'EVENT_CODE',
      label: 'Mã sự kiện',
    },
  ];
}
/* #endregion */

/* #region event-overview */
export class EventOverview {
  public static keyStorage = 'eventOverview';
  public static KHOI_TAO = 1;
  public static DANG_MO_BAN = 2;
  public static TAM_DUNG = 3;
  public static HUY_SU_KIEN = 4;
  public static KET_THUC = 5;

  public static listStatus: IDropdown[] = [
    {
      value: this.KHOI_TAO,
      label: 'Khởi tạo',
      severity: SEVERITY.PRIMARY,
    },
    {
      value: this.DANG_MO_BAN,
      label: 'Đang mở bán',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.TAM_DUNG,
      label: 'Tạm dừng',
      severity: SEVERITY.SECONDARY,
    },
    {
      value: this.HUY_SU_KIEN,
      label: 'Hủy sự kiện',
      severity: SEVERITY.DANGER,
    },
    {
      value: this.KET_THUC,
      label: 'Kết thúc',
      severity: SEVERITY.INFO,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listTypeEvent: IDropdown[] = [
    {
      value: 1,
      label: 'Nhạc sống',
    },
    {
      value: 2,
      label: 'Sân khấu',
    },
    {
      value: 3,
      label: 'Văn hóa nghệ thuật',
    },
    {
      value: 4,
      label: 'Ngoài trời',
    },
    {
      value: 5,
      label: 'NightLife',
    },
    {
      value: 6,
      label: 'OnlineEvent',
    },
    {
      value: 7,
      label: 'Hội thảo',
    },
    {
      value: 8,
      label: 'Khoa học',
    },
    {
      value: 9,
      label: 'Triển lãm',
    },
    {
      value: 10,
      label: 'Hội họp',
    },
    {
      value: 11,
      label: 'Thể thao',
    },
    {
      value: 12,
      label: 'Cộng đồng',
    },
    {
      value: 13,
      label: 'Vui chơi giải trí',
    },
  ];

  public static listReason: IDropdown[] = [
    {
      value: 1,
      label: 'Thời tiết xấu',
    },
    {
      value: 2,
      label: 'Thay đổi thời gian',
    },
    {
      value: 3,
      label: 'Vướng mắc tổ chức',
    },
    {
      value: 4,
      label: 'Khác',
    },
  ];

  public static listEventViewer: IDropdown[] = [
    {
      value: 1,
      label: 'Tất cả',
    },
    {
      value: 4,
      label: 'Khách hàng chưa giao dịch',
    },
    {
      value: 2,
      label: 'Khách hàng đã giao dịch',
    },
    {
      value: 3,
      label: 'Tư vấn viên',
    },
  ];

  public static KICH_HOAT = 1;
  public static HUY_KICH_HOAT = 2;
  public static HET_VE = 3;
  public static listStatusInfor: IDropdown[] = [
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
    {
      value: this.HET_VE,
      label: 'Hết vé',
      severity: SEVERITY.INFO,
    },
  ];

  public static getStatusInfor(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatusInfor.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static CO = true;
  public static listYesNo: IDropdown[] = [
    {
      value: this.CO,
      label: 'Có',
    },
    {
      value: false,
      label: 'Không',
    },
  ];

  public static listStatusTicket: IDropdown[] = [
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

  public static getStatusTicket(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatusTicket.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static keyEventDetailMedia = {
    AVATAR: 'AvatarEvent',
    BANNER: 'BannerEvent',
    SLIDE: 'SlideEvent',
  };
}
/* #endregion */

/* #region sale-ticket-management */
/* #region sale-ticket-order */
export class SaleTicketOrder {
  public static keyStorage = 'saleTicketOrder';
  public static HET_THOI_GIAN = 6;

  public static orderTicketStatus = {
    CHUA_THAM_GIA: 1,
    DA_THAM_GIA: 2,
  }

  public static CHO_XU_LY = 3;
  public static HOP_LE = 4;
  public static listStatus: IDropdown[] = [
    {
      value: 1,
      label: 'Khởi tạo',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: 2,
      label: 'Chờ thanh toán',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.HET_THOI_GIAN,
      label: 'Hết thời gian',
      severity: SEVERITY.SECONDARY,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listOnineOffline: IDropdown[] = [
    {
      value: 1,
      label: 'Online',
    },
    {
      value: 2,
      label: 'Offline',
    },
  ];

  public static listSource: IDropdown[] = [
    {
      value: 1,
      label: 'Quản trị viên',
    },
    {
      value: 2,
      label: 'Khách hàng',
    },
    {
      value: 3,
      label: 'Tư vấn viên',
    },
  ];

  public static CUSTOMER = 0;
  public static BUSINESS = 1;

  public static listReason: IDropdown[] = [
    {
      value: 1,
      label: 'Khách xin gia hạn',
    },
    {
      value: 2,
      label: 'Khách ngoại giao',
    },
    {
      value: 3,
      label: 'Lỗi ngân hàng',
    },
    {
      value: 4,
      label: 'Sự cố hệ thống',
    },
    {
      value: 5,
      label: 'Khác',
    },
  ];

  public static THU = 1;
  public static listRevenueExpenditure: IDropdown[] = [
    {
      value: this.THU,
      label: 'Thu',
    },
    {
      value: 2,
      label: 'Chi',
    },
  ];

  public static THANH_TOAN_MUA_VE = 1;
  public static listTradingTypeTransaction: IDropdown[] = [
    {
      value: this.THANH_TOAN_MUA_VE,
      label: 'Thanh toán mua vé',
    },
  ];

  public static CHUYEN_KHOAN = 2;
  public static listTransactionType: IDropdown[] = [
    {
      value: this.CHUYEN_KHOAN,
      label: 'Chuyển khoản',
    },
    {
      value: 1,
      label: 'Tiền mặt',
    },
  ];

  public static TRINH_DUYET = 1;
  public static DA_THANH_TOAN = 2;
  public static HUY_THANH_TOAN = 3;
  public static listStatusTransaction: IDropdown[] = [
    {
      value: this.TRINH_DUYET,
      label: 'Trình duyệt',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.DA_THANH_TOAN,
      label: 'Đã thanh toán',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.HUY_THANH_TOAN,
      label: 'Hủy thanh toán',
      severity: SEVERITY.DANGER,
    },
  ];

  public static getStatusTransaction(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatusTransaction.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static keyStorageTransaction = 'saleTicketOrderTransaction';
  public static listActionHistory: IConstant[] = [
    {
      id: 1,
      value: 'Thêm mới',
    },
    {
      id: 2,
      value: 'Cập nhật',
    },
    {
      id: 3,
      value: 'Xóa',
    },
    {
      id: 4,
      value: 'Duyệt',
    },
  ];

  public static UpdateTable = 2;
}
/* #endregion */

/* #region transaction-processing */
export class TransactionProcessing {
  public static keyStorage = 'transactionProcessing';
  public static listStatus: IDropdown[] = [
    {
      value: 3,
      label: 'Chờ xử lý',
      severity: SEVERITY.WARNING,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listOnineOffline: IDropdown[] = [
    {
      value: 1,
      label: 'Online',
    },
    {
      value: 2,
      label: 'Offline',
    },
  ];

  public static listSource: IDropdown[] = [
    {
      value: 1,
      label: 'Quản trị viên',
    },
    {
      value: 2,
      label: 'Khách hàng',
    },
    {
      value: 3,
      label: 'Tư vấn viên',
    },
  ];
}
/* #endregion */

/* #region valid-sale-ticket */
export class ValidSaleTicket {
  public static keyStorage = 'validSaleTicket';
  public static HOP_LE = false;
  public static TAM_KHOA = true;

  public static listStatus: IDropdown[] = [
    {
      value: this.HOP_LE,
      label: 'Hợp lệ',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.TAM_KHOA,
      label: 'Tạm khóa',
      severity: SEVERITY.DANGER,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listSource: IDropdown[] = [
    {
      value: 1,
      label: 'Quản trị viên',
    },
    {
      value: 2,
      label: 'Khách hàng',
    },
    {
      value: 3,
      label: 'Tư vấn viên',
    },
  ];
}
/* #endregion */

/* #region participation-management */
export class ParticipationManagement {
  public static keyStorage = 'participationManagement';
  public static CHUA_THAM_GIA = 1;
  public static DA_THAM_GIA = 2;
  public static TAM_KHOA = 3;

  public static listStatus: IDropdown[] = [
    {
      value: this.DA_THAM_GIA,
      label: 'Đã tham gia',
      severity: SEVERITY.SUCCESS,
    },
    {
      value: this.CHUA_THAM_GIA,
      label: 'Chưa tham gia',
      severity: SEVERITY.WARNING,
    },
    {
      value: this.TAM_KHOA,
      label: 'Tạm khóa',
      severity: SEVERITY.DANGER,
    },
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }

  public static listCheckinType: IDropdown[] = [
    {
      value: 1,
      label: 'Tự động',
    },
    {
      value: 2,
      label: 'Thủ công',
    },
  ];

  public static KHAC = 3;
  public static listReason: IDropdown[] = [
    {
      value: 1,
      label: 'Hạn chế tham gia',
    },
    {
      value: 2,
      label: 'Xử lý công nợ',
    },
    {
      value: this.KHAC,
      label: 'Khác',
    },
  ];
}
/* #endregion */

/* #region ticket-request-list */
export class TicketRequestList {
  public static keyStorage = 'ticketRequestList';
  public static CHO_XU_LY = 1;
  public static DANG_GIAO = 2;
  public static HOAN_THANH = 3;

  public static listStatus: IDropdown[] = [
    {
      value: this.CHO_XU_LY,
      label: 'Chờ xử lý',
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
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }
}
/* #endregion */

/* #region bill-request-list */
export class BillRequestList {
  public static keyStorage = 'billRequestList';
  public static CHO_XU_LY = 1;
  public static DANG_GIAO = 2;
  public static HOAN_THANH = 3;

  public static listStatus: IDropdown[] = [
    {
      value: this.CHO_XU_LY,
      label: 'Chờ xử lý',
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
  ];

  public static getStatus(code: string, atribution = ETypeStatus.LABEL) {
    let status = this.listStatus.find((status) => status.value === code);
    return status ? status[atribution] : '';
  }
}
/* #endregion */
/* #endregion sale-ticket-management */

export const AtributionConfirmConst = {
  header: 'Thông báo!',
  icon: 'pi pi-exclamation-triangle',
  acceptLabel: 'Đồng ý',
  rejectLabel: 'Hủy bỏ',
}

export class ConfigureSystemConst {

  public static key = [
      {
          name: 'Xóa lệnh Invest',
          code: 'DeletedOrderInvest',
      },
      {
          name: 'Xóa lệnh Garner',
          code: 'DeletedOrderGarner',
      },
      {
          name: 'Xóa lệnh RealEstate',
          code: 'DeletedOrderRealEstate',
      },
      
  ];

  public static keyNotifyDates = [

    {
      name: 'Thông báo sự kiện sắp diễn ra',
      code: 'EventTimeSendNotiEventUpComingForCustomer',
      keyNotify: "EVENT_SU_KIEN_SAP_DIEN_RA"
    },
    {
      name: 'Thông báo sự kiện sắp diễn ra',
      code: 'EventDaySendNotiEventUpcomingForCustomer',
      keyNotify: "EVENT_SU_KIEN_SAP_DIEN_RA",
    },
  ];

  public static keyDates = this.keyNotifyDates.map(config => config.code); 

  public static getName(code) {
      const status = this.key.find(item => item.code == code);
      return status ? status.name : '';
  }
}
