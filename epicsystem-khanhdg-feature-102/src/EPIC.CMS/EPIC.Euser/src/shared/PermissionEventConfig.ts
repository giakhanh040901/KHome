import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionEventConfig = {};
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
    public static readonly Menu_YeuCauveCung: string = `${PermissionEventConst.EventModule}${PermissionEventConst.Menu}YeuCauVeCung`;
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
    // CÀI ĐẶT
    PermissionEventConfig[PermissionEventConst.Menu_CaiDat] = { type: PermissionTypes.Menu, name: 'Cài đặt', parentKey: null, webKey: WebKeys.Event, icon: 'pi pi-cog' };
    
    // CẤU TRÚC MÃ HỢP ĐỒNG
    PermissionEventConfig[PermissionEventConst.Menu_CauTrucMaHD] = { type: PermissionTypes.Menu, name: 'Cấu trúc mã hợp đồng', parentKey: PermissionEventConst.Menu_CaiDat, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.CauTrucMaHD_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_CauTrucMaHD, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.CauTrucMaHD_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.Menu_CauTrucMaHD, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.CauTrucMaHD_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.Menu_CauTrucMaHD, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.CauTrucMaHD_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.Menu_CauTrucMaHD, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.CauTrucMaHD_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionEventConst.Menu_CauTrucMaHD, webKey: WebKeys.Event };

    // THÔNG BÁO HỆ THỐNG
    PermissionEventConfig[PermissionEventConst.Menu_ThongBaoHeThong] = { type: PermissionTypes.Menu, name: 'Thông báo hệ thống', parentKey: PermissionEventConst.Menu_CaiDat, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.ThongBaoHeThong_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_ThongBaoHeThong, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.ThongBaoHeThong_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.Menu_ThongBaoHeThong, webKey: WebKeys.Event };

    // QUẢN LÝ SỰ KIỆN
    PermissionEventConfig[PermissionEventConst.Menu_QuanLySuKien] = { type: PermissionTypes.Menu, name: 'Quản lý sự kiện', parentKey: null, webKey: WebKeys.Event };
    
    // TỔNG QUAN SỰ KIỆN
    PermissionEventConfig[PermissionEventConst.Menu_TongQuanSuKien] = { type: PermissionTypes.Menu, name: 'Tổng quan sự kiện', parentKey: PermissionEventConst.Menu_QuanLySuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_MoBanVe] = { type: PermissionTypes.ButtonAction, name: 'Mở bán vé', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_BatTatShowApp] = { type: PermissionTypes.ButtonAction, name: 'Bật tắt Show App', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_TamDung_HuySuKien] = { type: PermissionTypes.ButtonAction, name: 'Tạm dừng/ Hủy sự kiện', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    
    // TỔNG QUAN SỰ KIỆN - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết sự kiện', parentKey: PermissionEventConst.Menu_TongQuanSuKien, webKey: WebKeys.Event };
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin sự kiện', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung, webKey: WebKeys.Event };
    
    // CHI TIẾT - QUẢN LÝ
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_QuanLy] = { type: PermissionTypes.Tab, name: 'Quản lý', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_QuanLy_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_QuanLy, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_QuanLy_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_QuanLy, webKey: WebKeys.Event };
    
    // CHI TIẾT - THÔNG TIN MÔ TẢ
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa] = { type: PermissionTypes.Tab, name: 'Thông tin mô tả', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa_ChiTiet] = { type: PermissionTypes.Table, name: 'Chi tiết', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa, webKey: WebKeys.Event };
    
    // CHI TIẾT - THỜI GIAN VÀ LOẠI VÉ
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe] = { type: PermissionTypes.Tab, name: 'Thời gian và loại vé', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe, webKey: WebKeys.Event };
    
    // CHI TIẾT - HÌNH ẢNH SỰ KIỆN
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien] = { type: PermissionTypes.Tab, name: 'Hình ảnh sự kiện', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien_DSHinhAnh] = { type: PermissionTypes.Table, name: 'Danh sách hình ảnh', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien, webKey: WebKeys.Event };
    
    // CHI TIẾT - MẪU GIAO NHẬN VÉ
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe] = { type: PermissionTypes.Tab, name: 'Mẫu giao nhận vé', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_XemThuMau] = { type: PermissionTypes.ButtonAction, name: 'Xem thử mẫu', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_TaiMauVe] = { type: PermissionTypes.ButtonAction, name: 'Tải mẫu vé', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe, webKey: WebKeys.Event };
    
    // CHI TIẾT - MẪU VÉ
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe] = { type: PermissionTypes.Tab, name: 'Mẫu vé', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_XemThuMau] = { type: PermissionTypes.ButtonAction, name: 'Xem thử mẫu', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_TaiMauVe] = { type: PermissionTypes.ButtonAction, name: 'Tải mẫu vé', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };
    PermissionEventConfig[PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái', parentKey: PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe, webKey: WebKeys.Event };

    // QUẢN LÝ BÁN VÉ
    PermissionEventConfig[PermissionEventConst.Menu_QuanLyBanVe] = { type: PermissionTypes.Menu, name: 'Quản lý bán vé', parentKey: null, webKey: WebKeys.Event }; 
    
    // SỔ LỆNH
    PermissionEventConfig[PermissionEventConst.Menu_SoLenh] = { type: PermissionTypes.Menu, name: 'Sổ lệnh', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_SoLenh, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.Menu_SoLenh, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_GiaHanGiulenh] = { type: PermissionTypes.ButtonAction, name: 'Gia hạn giữ lệnh', parentKey: PermissionEventConst.Menu_SoLenh, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa', parentKey: PermissionEventConst.Menu_SoLenh, webKey: WebKeys.Event }; 
    
    // SỔ LỆNH - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet] = { type: PermissionTypes.Tab, name: 'Chi tiết', parentKey: PermissionEventConst.Menu_SoLenh, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.SoLenh_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết', parentKey: PermissionEventConst.SoLenh_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.SoLenh_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich] = { type: PermissionTypes.Tab, name: 'Giao dịch', parentKey: PermissionEventConst.SoLenh_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_GuiThongBao] = { type: PermissionTypes.ButtonAction, name: 'Gửi thông báo', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_GiaoDich_HuyThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Hủy thanh toán', parentKey: PermissionEventConst.SoLenh_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - DANH SÁCH VÉ
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_DanhSachVe] = { type: PermissionTypes.Tab, name: 'Danh sách vé', parentKey: PermissionEventConst.SoLenh_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_DanhSachVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách vé', parentKey: PermissionEventConst.SoLenh_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_DanhSachVe_TaiVe] = { type: PermissionTypes.ButtonAction, name: 'Tải vé', parentKey: PermissionEventConst.SoLenh_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_DanhSachVe_XemVe] = { type: PermissionTypes.ButtonAction, name: 'Xem vé', parentKey: PermissionEventConst.SoLenh_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - LỊCH SỬ
    PermissionEventConfig[PermissionEventConst.SoLenh_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionEventConst.SoLenh_ChiTiet, webKey: WebKeys.Event }; 

    // XỬ LÝ GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.Menu_XuLyGiaoDich] = { type: PermissionTypes.Menu, name: 'Xử lý giao dịch', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_XuLyGiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_PheDuyetMuaVe] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt mua vé', parentKey: PermissionEventConst.Menu_XuLyGiaoDich, webKey: WebKeys.Event }; 
    
    // XỬ LÝ GIAO DỊCH - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet] = { type: PermissionTypes.Tab, name: 'Chi tiết', parentKey: PermissionEventConst.Menu_XuLyGiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich] = { type: PermissionTypes.Tab, name: 'Giao dịch', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich_HuyThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Hủy thanh toán', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - DANH SÁCH VÉ
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe] = { type: PermissionTypes.Tab, name: 'Danh sách vé', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách vé', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe_TaiVe] = { type: PermissionTypes.ButtonAction, name: 'Tải vé', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe_XemVe] = { type: PermissionTypes.ButtonAction, name: 'Xem vé', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - LỊCH SỬ
    PermissionEventConfig[PermissionEventConst.XuLyGiaoDich_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionEventConst.XuLyGiaoDich_ChiTiet, webKey: WebKeys.Event }; 

    
    // VÉ BÁN HỢP LỆ
    PermissionEventConfig[PermissionEventConst.Menu_VeBanHopLe] = { type: PermissionTypes.Menu, name: 'Vé bán hợp lệ', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_YeuCauHoaDon] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu hóa đơn', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_YeuCauVeCung] = { type: PermissionTypes.ButtonAction, name: 'Yêu cầu vé cứng', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ThongBaoVeHopLe] = { type: PermissionTypes.ButtonAction, name: 'Thông báo vé hợp lệ', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_KhoaYeuCau] = { type: PermissionTypes.ButtonAction, name: 'Khóa yêu cầu', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    
    // VÉ BÁN HỢP LỆ - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet] = { type: PermissionTypes.Tab, name: 'Chi tiết', parentKey: PermissionEventConst.Menu_VeBanHopLe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung_DoiMaGioiThieu] = { type: PermissionTypes.ButtonAction, name: 'Đổi mã giới thiệu', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich] = { type: PermissionTypes.Tab, name: 'Giao dịch', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Chi tiết', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_CapNhat] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_PheDuyet] = { type: PermissionTypes.ButtonAction, name: 'Phê duyệt', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich_HuyThanhToan] = { type: PermissionTypes.ButtonAction, name: 'Hủy thanh toán', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - DANH SÁCH VÉ
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe] = { type: PermissionTypes.Tab, name: 'Danh sách vé', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách vé', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe_TaiVe] = { type: PermissionTypes.ButtonAction, name: 'Tải vé', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe_XemVe] = { type: PermissionTypes.ButtonAction, name: 'Xem vé', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - LỊCH SỬ
    PermissionEventConfig[PermissionEventConst.VeBanHopLe_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionEventConst.VeBanHopLe_ChiTiet, webKey: WebKeys.Event }; 
    
    // QUẢN LÝ THAM GIA
    PermissionEventConfig[PermissionEventConst.Menu_QuanLyThamGia] = { type: PermissionTypes.Menu, name: 'Quản lý tham gia', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.QuanLyThamGia_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_QuanLyThamGia, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.QuanLyThamGia_XemVe] = { type: PermissionTypes.ButtonAction, name: 'Xem vé', parentKey: PermissionEventConst.Menu_QuanLyThamGia, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.QuanLyThamGia_TaiVe] = { type: PermissionTypes.ButtonAction, name: 'Tải vé', parentKey: PermissionEventConst.Menu_QuanLyThamGia, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.QuanLyThamGia_XacNhanThamGia] = { type: PermissionTypes.ButtonAction, name: 'Xác nhận tham gia', parentKey: PermissionEventConst.Menu_QuanLyThamGia, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.QuanLyThamGia_MoKhoaVe] = { type: PermissionTypes.ButtonAction, name: 'Mở / Khóa vé', parentKey: PermissionEventConst.Menu_QuanLyThamGia, webKey: WebKeys.Event }; 
    
    // YÊU CẦU VÉ CỨNG
    PermissionEventConfig[PermissionEventConst.Menu_YeuCauveCung] = { type: PermissionTypes.Menu, name: 'Yêu cầu vé cứng', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_XuatMauGiaoVe] = { type: PermissionTypes.ButtonAction, name: 'Xuất mẫu giao vé', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_XuatVeCung] = { type: PermissionTypes.ButtonAction, name: 'Xuất vé cứng', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_DoiTrangThai_DangGiao] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái Đang giao', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_DoiTrangThai_HoanThanh] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái Hoàn thành', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    
    // YÊU CẦU VÉ CỨNG - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết vé', parentKey: PermissionEventConst.Menu_YeuCauveCung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Page, name: 'Thông tin chi tiết', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT -GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich] = { type: PermissionTypes.Tab, name: 'Giao dịch', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết giao dịch', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - DANH SÁCH VÉ
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe] = { type: PermissionTypes.Tab, name: 'Danh sách vé', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe_TaiVe] = { type: PermissionTypes.Tab, name: 'Tải vé', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe_XemVe] = { type: PermissionTypes.Tab, name: 'Xem vé', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT- LỊCH SỬ
    PermissionEventConfig[PermissionEventConst.YeuCauVeCung_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionEventConst.YeuCauVeCung_ChiTiet, webKey: WebKeys.Event }; 
    
    // YÊU CẦU HÓA ĐƠN
    PermissionEventConfig[PermissionEventConst.Menu_YeuCauHoaDon] = { type: PermissionTypes.Menu, name: 'Yêu cầu hóa đơn', parentKey: PermissionEventConst.Menu_QuanLyBanVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_DanhSach] = { type: PermissionTypes.Table, name: 'Danh sách', parentKey: PermissionEventConst.Menu_YeuCauHoaDon, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_DoiTrangThai_DangGiao] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái Đang giao', parentKey: PermissionEventConst.Menu_YeuCauHoaDon, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_DoiTrangThai_HoanThanh] = { type: PermissionTypes.ButtonAction, name: 'Đổi trạng thái Hoàn thành', parentKey: PermissionEventConst.Menu_YeuCauHoaDon, webKey: WebKeys.Event }; 
    
    // YÊU CẦU HÓA ĐƠN - CHI TIẾT
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet] = { type: PermissionTypes.Page, name: 'Chi tiết', parentKey: PermissionEventConst.Menu_YeuCauHoaDon, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - THÔNG TIN CHUNG
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_ThongTinChung] = { type: PermissionTypes.Tab, name: 'Thông tin chung', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_ThongTinChung_ChiTiet] = { type: PermissionTypes.Tab, name: 'Thông tin vé', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_ThongTinChung, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - GIAO DỊCH
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich] = { type: PermissionTypes.Tab, name: 'Giao dịch', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich_ChiTiet] = { type: PermissionTypes.Tab, name: 'Chi tiết giao dịch', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - DANH SÁCH VÉ
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe] = { type: PermissionTypes.Tab, name: 'Danh sách vé', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe_DanhSach] = { type: PermissionTypes.Tab, name: 'Danh sách', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe_TaiVe] = { type: PermissionTypes.Tab, name: 'Tải vé', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe_XemVe] = { type: PermissionTypes.Tab, name: 'Xem vé', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe, webKey: WebKeys.Event }; 
    
    // CHI TIẾT - LỊCH SỬ
    PermissionEventConfig[PermissionEventConst.YeuCauHoaDon_ChiTiet_LichSu] = { type: PermissionTypes.Tab, name: 'Lịch sử', parentKey: PermissionEventConst.YeuCauHoaDon_ChiTiet, webKey: WebKeys.Event }; 

export default PermissionEventConfig;


    



