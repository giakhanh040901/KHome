import { Component, Injector, Input } from '@angular/core';
import { AppConsts, PermissionCoreConst, PermissionRealStateConst, UserTypes } from '@shared/AppConsts';
import { MessageService } from 'primeng/api';
import { AppMainComponent } from '../main/app.main.component';
import { AppComponentBase } from '@shared/app-component-base';
import { CookieService } from 'ngx-cookie-service';
@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent extends AppComponentBase {
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
        console.log('userLogin____', this.userLogin); 
    }

    @Input() permissionsMenu: any[] = [];

    userLogin: any = {};
    // _tokenService: TokenService;
    UserTypes = UserTypes;
    
    ngOnInit() {
        if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
                this.labelCheck = 'Cấu trúc mã hợp đồng giao dịch';
                this.routerLinkCheck = '/setting/contract-code-structure';
        } else if(UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) {
                this.labelCheck = 'Cấu trúc mã hợp đồng cọc';
                this.routerLinkCheck = '/setting/contract-code-structure';
        }
        //
        this.model = [
            
            { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: this.isPermission(PermissionRealStateConst.RealStatePageDashboard) },
            {
                label: 'Cài đặt', icon: 'pi pi-fw pi-cog', routerLink: ['/setting'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSetting),
                items: [
                    { label: 'Chủ đầu tư', icon: '', routerLink:['/setting/owner'], isShow: ( this.isPermission(PermissionRealStateConst.RealStateMenuChuDT) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type) )},
                    { label: 'Đại lý', icon: '', routerLink: ['/setting/trading-provider'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuDaiLy) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)},
                    { label: 'Chính sách bán hàng', icon: '', routerLink: ['/setting/distribution-policy-temp'], isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuCSPhanPhoi) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type))},
                    {   
                        label: 'Cấu trúc mã HĐ giao dịch', icon: '', 
                        routerLink: [this.routerLinkCheck], 
                        isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuCTMaHDGiaoDich) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) 
                    },
                    {   
                        label: 'Cấu trúc mã HĐ cọc', icon: '', 
                        routerLink: [this.routerLinkCheck], 
                        isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuCTMaHDCoc) && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type))
                    },
                    {   
                        label: 'Mẫu hợp đồng CĐT', icon: '', 
                        routerLink: ['/setting/sample-contract'], 
                        isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuMauHDCDT) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) 
                    },
                    { 
                        label: 'Mẫu hợp đồng đại lý', icon: '', 
                        routerLink: ['/setting/sample-contract'], 
                        isShow:( this.isPermission(PermissionRealStateConst.RealStateMenuMauHDDL) && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type))
                    },
                    
                    { label: 'Chính sách ưu đãi đại lý', icon: '', routerLink: ['/setting/selling-policy-temp'], isShow:( this.isPermission(PermissionRealStateConst.RealStateMenuCSBanHang) && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type) )},
                    { label: 'Thông báo hệ thống', icon: '', routerLink: ['/setting/system-notification-template'], isShow: (this.isPermission(PermissionRealStateConst.RealStateThongBaoHeThong) && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) },
                    // {label: 'Cấu hình ngày nghỉ lễ', icon: '', routerLink: ['/setting/calendar'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuCauHinhNNL)},
                    // {label: 'Tổng thầu', icon: '', routerLink: ['/setting/general-contractor'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuTongThau)},
                    // {label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow:( this.isPermission(PermissionRealStateConst.RealStateMenuHinhAnh) && UserTypes.TYPE_TRADING.includes(this.userLogin.user_type) )},
                    // {label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow:( this.isPermission(PermissionRealStateConst.RealStateMenuHinhAnh))},
                    // {label: 'Thông báo hệ thống', icon: '', routerLink: ['/setting/system-notification-template'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuThongBaoHeThong)},
                ]
            },
            {
                label: 'Khách hàng', icon: 'pi pi-users', routerLink: ['/customer'], isShow: this.isPermission(PermissionCoreConst.CoreMenuKhachHang),
                items: [
                    { label: 'Thêm KH Cá nhân', icon: '', routerLink: ['/customer/add-investor'], isShow: this.isPermission(PermissionCoreConst.CoreMenuDuyetKHCN) },
                    { label: 'Khách hàng cá nhân', icon: '', routerLink: ['/customer/investor'], isShow: this.isPermission(PermissionCoreConst.CoreMenuKHCN) },
                    { label: 'Thêm KH doanh nghiệp', icon: '', routerLink: ['/customer/business-customer/add-business-customer'], isShow: this.isPermission(PermissionCoreConst.CoreMenuDuyetKHDN) },
                    { label: 'Khách hàng doanh nghiệp', icon: '', routerLink: ['/customer/business-customer/business-customer'], isShow: this.isPermission(PermissionCoreConst.CoreMenuKHDN) },
                ]
            },
            {
                label: 'Quản lý dự án', icon: 'pi pi-map', routerLink: ['/project-manager'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuProjectManager),
                items: [
					{ label: 'Tổng quan dự án', icon: '', routerLink: ['/project-manager/project-overview'], isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuProjectOverview) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) },
					{ label: 'Bảng hàng dự án', icon: '', routerLink: ['/project-manager/project-list'], isShow: ( this.isPermission(PermissionRealStateConst.RealStateMenuProjectList) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) },
                    { label: 'Phân phối sản phẩm', icon: '', routerLink: ['/project-manager/product-distribution'], isShow: ( this.isPermission(PermissionRealStateConst.RealStateMenuPhanPhoi) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) },
                    { label: 'Mở bán', icon: '', routerLink: ['/project-manager/open-sell'], isShow: (this.isPermission(PermissionRealStateConst.RealStateMenuMoBan) && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) },
                ]
            }, 
            {
                label: 'Quản lý giao dịch cọc', icon: 'pi pi-book', routerLink: ['/trading-contract'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuQLGiaoDichCoc),
                items: [
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/trading-contract/order'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSoLenh) },    
                    { label: 'Xử lý đặt cọc', icon: '', routerLink: ['/trading-contract/contract-processing'], isShow: this.isPermission(PermissionRealStateConst.RealStateGDC_XLDC) },    
                    { label: 'Hợp đồng đặt cọc', icon: '', routerLink: ['/trading-contract/contract-active'], isShow: this.isPermission(PermissionRealStateConst.RealStateGDC_HDDC) },    
                ]
            },
            {
                label: 'Quản lý giao dịch mua', icon: 'pi pi-book', routerLink: ['/partner-contract'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuQLGiaoDichCoc) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type),
                items: [
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/partner-contract/order'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSoLenh) },    
                    { label: 'Hợp đồng mua', icon: '', routerLink: ['/partner-contract/contract-processing'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSoLenh) },    
                    { label: 'Hợp đồng thanh lý', icon: '', routerLink: ['/partner-contract/contract-active'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSoLenh) },    
                    { label: 'Chuyển nhượng hợp đồng', icon: '', routerLink: ['/partner-contract/contract-transfer'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuSoLenh) },    
                ]
            },
            {
                label: 'Quản lý phê duyệt', icon: 'pi pi-check', routerLink: ['/approve-manager'], isShow: this.isPermission(PermissionRealStateConst.RealStateMenuQLPD),
                items: [
                    { label: 'Phê duyệt dự án', icon: '', routerLink: ['/approve-manager/approve/1'], isShow: true && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)},
                    { label: 'Phê duyệt phân phối', icon: '', routerLink: ['/approve-manager/approve/2'], isShow: true && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)},
                    { label: 'Phê duyệt mở bán', icon: '', routerLink: ['/approve-manager/approve/3'], isShow: true && UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)},
                ]
            },
			
            {
                label: 'Báo cáo thống kê', icon: 'pi pi-file', routerLink: ['/export-report'], isShow: this.isPermission(PermissionRealStateConst.RealState_Menu_BaoCao),
                items: [
                    { label: 'Báo cáo quản trị', icon: '', routerLink: ['/export-report/management-report'], isShow: this.isPermission(PermissionRealStateConst.RealState_BaoCao_QuanTri)},
                    { label: 'Báo cáo vận hành', icon: '', routerLink: ['/export-report/operational-report'], isShow: this.isPermission(PermissionRealStateConst.RealState_BaoCao_VanHanh)},
                    { label: 'Báo cáo kinh doanh', icon: '', routerLink: ['/export-report/business-report'], isShow: this.isPermission(PermissionRealStateConst.RealState_BaoCao_KinhDoanh)}
                ]
            },
            {
                label: 'Truy vấn', icon: 'pi pi-sync', routerLink: ['/query-manager'], isShow: true,
                items: [
					{ label: 'Thu tiền ngân hàng', icon: '', routerLink: ['/query-manager/collect-money-bank'], isShow: true },
                    { label: 'Chi tiền ngân hàng', icon: '', routerLink: ['/query-manager/pay-money-bank'], isShow: true },
                ]
            }, 
        ];
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        // return true;
        return this.permissionsMenu.includes(keyName);
    }
}
