import { PermissionTypes } from "./AppConsts";
import { WebKeys } from "./AppConsts";

const PermissionUserConfig = {};
export class PermissionUserConst {
    
    public static readonly Web: string = "web_";
    public static readonly Menu: string = "menu_";
    public static readonly Tab: string = "tab_";
    public static readonly Page: string = "page_";
    public static readonly Table: string = "table_";
    public static readonly Form: string = "form_";
    public static readonly ButtonTable: string = "btn_table_";
    public static readonly ButtonForm: string = "btn_form_";
    public static readonly ButtonAction: string = "btn_action_";

    public static readonly UserModule: string = "user.";
    //
    
    // PHÂN QUYỀN WEBSITE
    public static readonly UserPhanQuyen_Websites: string = `${PermissionUserConst.UserModule}${PermissionUserConst.Menu}phan_quyen_websites`;
    public static readonly UserPhanQuyen_Website_CauHinhVaiTro: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}phan_quyen_website_cau_hinh_vaitro`;
    public static readonly UserPhanQuyen_Website_ThemVaiTro: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}phan_quyen_website_them_vaitro`;
    public static readonly UserPhanQuyen_Website_CapNhatVaiTro: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}phan_quyen_website_cap_nhat_vaitro`;

    // QUẢN LÝ TÀI KHOẢN
    public static readonly UserQL_TaiKhoan: string = `${PermissionUserConst.UserModule}${PermissionUserConst.Menu}ql_taikhoan`;
    public static readonly UserQL_TaiKhoan_ThemMoi: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_them_moi`;
    public static readonly UserQL_TaiKhoan_CapNhatVaiTro: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_cap_nhat_vai_tro`;
    public static readonly UserQL_TaiKhoan_SetMatKhau: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_set_matkhau`;
    public static readonly UserQL_TaiKhoan_ChiTiet: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_chitiet`;
    public static readonly UserQL_TaiKhoan_Xoa: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_xoa`;

    public static readonly UserQL_TaiKhoan_CauHinhQuyen: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_cau_hinh_quyen`;
    public static readonly UserQL_TaiKhoan_PhanQuyenWebsite: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_phan_quyen_website`;
    public static readonly UserQL_TaiKhoan_PhanQuyenChiTietWebsite: string = `${PermissionUserConst.UserModule}${PermissionUserConst.ButtonAction}ql_taikhoan_phan_quyen_chi_tiet_website`;

}

// PHÂN QUYỀN WEBSITE
PermissionUserConfig[PermissionUserConst.UserPhanQuyen_Websites] = { type: PermissionTypes.Menu, name: 'Phân quyền website', parentKey: null, icon: 'pi pi-fw pi-cog', webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserPhanQuyen_Website_CauHinhVaiTro] = { type: PermissionTypes.ButtonAction, name: 'Cấu hình vai trò', parentKey: PermissionUserConst.UserPhanQuyen_Websites, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserPhanQuyen_Website_ThemVaiTro] = { type: PermissionTypes.ButtonAction, name: 'Thêm vai trò', parentKey: PermissionUserConst.UserPhanQuyen_Websites, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserPhanQuyen_Website_CapNhatVaiTro] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật vai trò', parentKey: PermissionUserConst.UserPhanQuyen_Websites, webKey: WebKeys.User };

// QUẢN LÝ TÀI KHOẢN
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan] = { type: PermissionTypes.Menu, name: 'Quản lý tài khoản', parentKey: null, icon: 'pi pi-fw pi-cog' };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_ThemMoi] = { type: PermissionTypes.ButtonAction, name: 'Thêm mới tài khoản', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_CapNhatVaiTro] = { type: PermissionTypes.ButtonAction, name: 'Cập nhật vai trò', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_SetMatKhau] = { type: PermissionTypes.ButtonAction, name: 'Set mật khẩu', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_ChiTiet] = { type: PermissionTypes.ButtonAction, name: 'Thông tin tài khoản', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_Xoa] = { type: PermissionTypes.ButtonAction, name: 'Xóa tài khoản', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };

// CẤU HÌNH QUYỀN CHO TÀI KHOẢN
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_CauHinhQuyen] = { type: PermissionTypes.ButtonAction, name: 'Cấu hình quyền', parentKey: PermissionUserConst.UserQL_TaiKhoan, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_PhanQuyenWebsite] = { type: PermissionTypes.ButtonAction, name: 'Phân quyền truy cập website', parentKey: PermissionUserConst.UserQL_TaiKhoan_CauHinhQuyen, webKey: WebKeys.User };
PermissionUserConfig[PermissionUserConst.UserQL_TaiKhoan_PhanQuyenChiTietWebsite] = { type: PermissionTypes.ButtonAction, name: 'Phân quyền chi tiết website', parentKey: PermissionUserConst.UserQL_TaiKhoan_CauHinhQuyen, webKey: WebKeys.User };

export default PermissionUserConfig;
