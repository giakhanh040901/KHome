import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppConsts, PermissionGarnerConst, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { AppMainComponent } from '../main/app.main.component';
import jwt_decode from "jwt-decode";
import { TokenService } from '@shared/services/token.service';
import { AppComponentBase } from '@shared/app-component-base';
import { CookieService } from 'ngx-cookie-service';
@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent extends CrudComponentBase implements OnInit {
    protected _cookieService: CookieService;
    model: any[];
    checkPartner: any;
    AppConsts = AppConsts;
    labelCheck: string;
    routerLinkCheck: string;
    constructor(
        public appMain: AppMainComponent,
        injector: Injector,
        messageService: MessageService,
        cookieService: CookieService,
    ) {
        super(injector, messageService);
        this._cookieService = cookieService;
        this.userLogin = this.getUser();
    }

    @Input() permissionsMenu: any[] = [];

    userLogin: any = {};
    // _tokenService: TokenService;
    UserTypes = UserTypes;
    
    ngOnInit() {
        // 
        if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
            this.labelCheck = 'Cấu hình ngày nghỉ lễ';
            this.routerLinkCheck = '/setting/calendar-partner';
        } else if(UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) {
            this.labelCheck = 'Cấu hình ngày nghỉ lễ';
            this.routerLinkCheck = '/setting/calendar';
        }
        //            
        this.model = [
            { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: this.isPermission(PermissionGarnerConst.GarnerPageDashboard) },
            {
                label: 'Cài đặt', icon: 'pi pi-fw pi-cog', routerLink: ['/setting'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuSetting),
                items: [
                    {label: this.labelCheck, icon: '', routerLink: [this.routerLinkCheck], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuCauHinhNNL)},
                    {label: 'Chính sách mẫu', icon: '', routerLink: ['/setting/policy-template'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuChinhSachMau)},
                    {label: 'Đại lý', icon: '', routerLink: ['/setting/trading-provider'], isShow: (this.isPermission(PermissionGarnerConst.GarnerMenuDaiLy))},
                    //chua co api hinh anh garner
                    // {label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow:( this.isPermission(PermissionGarnerConst.GarnerMenuHinhAnh) && UserTypes.TYPE_TRADING.includes(this.userLogin.user_type) )},
                    {label: 'Thông báo hệ thống', icon: '', routerLink: ['/setting/system-notification-template'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuThongBaoHeThong)},
                    // {label: 'Mô tả chung', icon: '', routerLink: ['/setting/general-description'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuMoTaChung)},
                    {label: 'Mô tả chung', icon: '', routerLink: ['/setting/general-description/detail'], isShow: this.isPermission(PermissionGarnerConst.GarnerMTC_ThongTinMTC)},
                    {label: 'Cấu trúc mã hợp đồng', icon: '', routerLink: ['/setting/contract-code-structure'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuCauHinhMaHD)},
                    {label: 'Mẫu hợp đồng', icon: '', routerLink: ['/setting/sample-contract'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuMauHD)},
                ]
            },
            {
                label: 'Quản lý sản phẩm', icon: 'pi pi-map', routerLink: ['/product-manager'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuQLDT),
                items: [
					{ label: 'Sản phẩm tích lũy', icon: '', routerLink: ['/product-manager/list'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuSPTL)},
                    { label: 'Phân phối sản phẩm', icon: '', routerLink: ['/product-manager/distribution/list'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuPPSP)},
                ]
            }, 
            {
                label: 'Quản lý phê duyệt', icon: 'pi pi-check', routerLink: ['/approve-manager'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuQLPD),
                items: [
                    { label: 'Phê duyệt sản phẩm tích lũy', icon: '', routerLink: ['/approve-manager/approve/1'], isShow: (this.isPermission(PermissionGarnerConst.GarnerMenuPDSPTL)) },
                    { label: 'Phê duyệt phân phối sản phẩm', icon: '', routerLink: ['/approve-manager/approve/2'], isShow: (this.isPermission(PermissionGarnerConst.GarnerMenuPDPPSP)) },
                    // { label: 'Phê duyệt yêu cầu tái tục', icon: '', routerLink: ['/approve-manager/request-reinstatement'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuPDYCTT) },
                ]
            },
			{
                label: 'Hợp đồng phân phối', icon: 'pi pi-book', routerLink: ['/trading-contract'], isShow: this.isPermission(PermissionGarnerConst.GarnerMenuHDPP),
                items: [
                    // { label: 'Đặt lệnh', icon: '', routerLink: ['/'] },
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/trading-contract/order'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_SoLenh) },
                    { label: 'Xử lý hợp đồng', icon: '', routerLink: ['/trading-contract/contract-processing'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_XLHD) },
                    { label: 'Hợp đồng', icon: '', routerLink: ['/trading-contract/contract-active'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_HopDong) },
                    { label: 'Hợp đồng theo chính sách', icon: '', routerLink: ['/trading-contract/contract-active-group'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_HopDong) },
                    { label: 'Giao nhận hợp đồng', icon: '', routerLink: ['/trading-contract/delivery-contract'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong) },
                    { label: 'Phong tỏa, giải tỏa', icon: '', routerLink: ['/trading-contract/contract-blockage'], isShow: this.isPermission(PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa) },
                    { label: 'Chi trả lợi tức', icon: '', routerLink: ['/trading-contract/interest-contract'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_CTLC) },
                    { label: 'Xử lý rút tiền', icon: '', routerLink: ['/trading-contract/manager-withdraw'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_XLRutTien) },
                    { label: 'Lịch sử tích lũy', icon: '', routerLink: ['/trading-contract/garner-history'], isShow: this.isPermission(PermissionGarnerConst.GarnerHDPP_LSTL) },
                ]
            },
            {
                label: 'Truy vấn', icon: 'pi pi-sync', routerLink: ['/query'], isShow: this.isPermission(PermissionGarnerConst.Garner_Menu_TruyVan),
                items: [
					{ label: 'Thu tiền ngân hàng', icon: '', routerLink: ['/query/collect-money-bank'], isShow: this.isPermission(PermissionGarnerConst.Garner_TruyVan_ThuTien_NganHang) },
                    { label: 'Chi tiền ngân hàng', icon: '', routerLink: ['/query/pay-money-bank'], isShow: this.isPermission(PermissionGarnerConst.Garner_TruyVan_ChiTien_NganHang) },
               
                ]
            }, 
            {
                label: 'Báo cáo thống kê', icon: 'pi pi-save', routerLink: ['/export-report'], isShow: this.isPermission(PermissionGarnerConst.Garner_Menu_BaoCao),
                items: [
                    { label: 'Báo cáo quản trị', icon: '', routerLink: ['/export-report/management-report'], isShow: this.isPermission(PermissionGarnerConst.Garner_BaoCao_QuanTri)},
                    { label: 'Báo cáo vận hành', icon: '', routerLink: ['/export-report/operational-report'], isShow: this.isPermission(PermissionGarnerConst.Garner_BaoCao_VanHanh)},
                    { label: 'Báo cáo kinh doanh', icon: '', routerLink: ['/export-report/business-report'], isShow: this.isPermission(PermissionGarnerConst.Garner_BaoCao_KinhDoanh)}
                ]
            },
        ];
    }

    // getUser() {
    //     const token = this._tokenService.getToken();
    //     console.log('token__________________', token);
    //     if(token) {
    //         alert('vao dau');
    //         const userInfo = jwt_decode(token);
    //         return userInfo;
    //     }
    //     return {};
    // }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        // return true;
        return this.permissionsMenu.includes(keyName);
    }
}
