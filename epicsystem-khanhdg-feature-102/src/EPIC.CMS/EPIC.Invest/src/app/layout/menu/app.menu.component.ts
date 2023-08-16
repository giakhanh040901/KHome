import { Component, Injector, Input, OnInit } from '@angular/core';
import { PermissionInvestConst, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { AppMainComponent } from '../main/app.main.component';
import jwt_decode from "jwt-decode";
import { TokenService } from '@shared/services/token.service';
import { AppComponentBase } from '@shared/app-component-base';
@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent extends AppComponentBase {

    model: any[];

    constructor(
        public appMain: AppMainComponent,
        injector: Injector,
        messageService: MessageService,
    ) {
        super(injector, messageService);
        this.userLogin = this.getUser();
    }

    @Input() permissionsMenu: any[] = [];

    userLogin: any = {};
    // _tokenService: TokenService;
    UserTypes = UserTypes;
    
    ngOnInit() {
        this.model = [
            
            { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: this.isPermission(PermissionInvestConst.InvestPageDashboard) },
            {
                label: 'Cài đặt', icon: 'pi pi-fw pi-cog', routerLink: ['/setting'], isShow: this.isPermission(PermissionInvestConst.InvestMenuSetting),
                items: [
                    {label: 'Chủ đầu tư', icon: '', routerLink:['/setting/owner'], isShow: this.isPermission(PermissionInvestConst.InvestMenuChuDT)},
                    {label: 'Cấu hình ngày nghỉ lễ', icon: '', routerLink: ['/setting/calendar'], isShow: this.isPermission(PermissionInvestConst.InvestMenuCauHinhNNL)},
                    {label: 'Chính sách mẫu', icon: '', routerLink: ['/setting/policy-template'], isShow: this.isPermission(PermissionInvestConst.InvestMenuChinhSachMau)},
                    {label: 'Tổng thầu', icon: '', routerLink: ['/setting/general-contractor'], isShow: this.isPermission(PermissionInvestConst.InvestMenuTongThau)},
                    {label: 'Đại lý', icon: '', routerLink: ['/setting/trading-provider'], isShow: this.isPermission(PermissionInvestConst.InvestMenuDaiLy)},
                    // {label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow:( this.isPermission(PermissionInvestConst.InvestMenuHinhAnh) && UserTypes.TYPE_TRADING.includes(this.userLogin.user_type) )},
                    {label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow:( this.isPermission(PermissionInvestConst.InvestMenuHinhAnh))},
                    {label: 'Thông báo hệ thống', icon: '', routerLink: ['/setting/system-notification-template'], isShow: this.isPermission(PermissionInvestConst.InvestMenuThongBaoHeThong)},
                    {label: 'Cấu hình mã hợp đồng', icon: '', routerLink: ['/setting/contract-code-structure'], isShow: this.isPermission(PermissionInvestConst.InvestMenuCauHinhMaHD)},
                    {label: 'Mẫu hợp đồng', icon: '', routerLink: ['/setting/sample-contract'], isShow: this.isPermission(PermissionInvestConst.InvestMenuMauHD)},
                ]
            },
            {
                label: 'Quản lý đầu tư', icon: 'pi pi-map', routerLink: ['/invest-manager'], isShow: this.isPermission(PermissionInvestConst.InvestMenuQLDT),
                items: [
					{ label: 'Sản phẩm đầu tư', icon: '', routerLink: ['/invest-manager/project'], isShow: this.isPermission(PermissionInvestConst.InvestMenuSPDT) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type) },
                    { label: 'Phân phối đầu tư', icon: '', routerLink: ['/invest-manager/distribution'], isShow: this.isPermission(PermissionInvestConst.InvestMenuPPDT)},
                ]
            }, 
            {
                label: 'Quản lý phê duyệt', icon: 'pi pi-check', routerLink: ['/approve-manager'], isShow: this.isPermission(PermissionInvestConst.InvestMenuQLPD),
                items: [
                    { label: 'Phê duyệt sản phẩm đầu tư', icon: '', routerLink: ['/approve-manager/approve/1'], isShow: this.isPermission(PermissionInvestConst.InvestMenuPDSPDT) && UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)},
                    { label: 'Phê duyệt phân phối đầu tư', icon: '', routerLink: ['/approve-manager/approve/2'], isShow: this.isPermission(PermissionInvestConst.InvestMenuPDPPDT)},
                    // { label: 'Phê duyệt yêu cầu tái tục', icon: '', routerLink: ['/approve-manager/request-reinstatement'], isShow: this.isPermission(PermissionInvestConst.InvestMenuPDYCTT) },
                    // { label: 'Phê duyệt yêu cầu rút vốn', icon: '', routerLink: ['/approve-manager/request-withdrawal'], isShow: this.isPermission(PermissionInvestConst.InvestMenuPDYCRV) },
                ]
            },
			{
                label: 'Hợp đồng phân phối', icon: 'pi pi-book', routerLink: ['/trading-contract'], isShow: this.isPermission(PermissionInvestConst.InvestMenuHDPP),
                items: [
                    // { label: 'Đặt lệnh', icon: '', routerLink: ['/'] },
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/trading-contract/order'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_SoLenh) },
                    { label: 'Xử lý hợp đồng', icon: '', routerLink: ['/trading-contract/contract-processing'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_XLHD) },
                    { label: 'Hợp đồng', icon: '', routerLink: ['/trading-contract/contract-active'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_HopDong) },
                    { label: 'Giao nhận hợp đồng', icon: '', routerLink: ['/trading-contract/delivery-contract'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_GiaoNhanHopDong) },
                    { label: 'Phong tỏa, giải tỏa', icon: '', routerLink: ['/trading-contract/contract-blockage'], isShow: this.isPermission(PermissionInvestConst.InvestHopDong_PhongToaGiaiToa) },
                    
                    { label: 'Chi trả lợi tức', icon: '', routerLink: ['/trading-contract/interest-contract'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_CTLT) },
                    { label: 'Xử lý rút tiền', icon: '', routerLink: ['/trading-contract/manager-withdraw'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_XLRT) },
                    { label: 'Hợp đồng đáo hạn', icon: '', routerLink: ['/trading-contract/expire-contract'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_HDDH) },
                    
                    // { label: 'Hợp đồng tất toán', icon: '', routerLink: ['/trading-contract/settlement-contract'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_TTHD) },
                    { label: 'Lịch sử đầu tư', icon: '', routerLink: ['/trading-contract/investment-history'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_LSDT) },
                    { label: 'Hợp đồng tái tục', icon: '', routerLink: ['/trading-contract/renewal-contract'], isShow: this.isPermission(PermissionInvestConst.InvestHDPP_HDTT) },
                ]
            },
            {
                label: 'Báo cáo thống kê', icon: 'pi pi-file', routerLink: ['/export-report'], isShow: this.isPermission(PermissionInvestConst.Invest_Menu_BaoCao),
                items: [
                    { label: 'Báo cáo quản trị', icon: '', routerLink: ['/export-report/management-report'], isShow: this.isPermission(PermissionInvestConst.Invest_BaoCao_QuanTri)},
                    { label: 'Báo cáo vận hành', icon: '', routerLink: ['/export-report/operational-report'], isShow: this.isPermission(PermissionInvestConst.Invest_BaoCao_VanHanh)},
                    { label: 'Báo cáo kinh doanh', icon: '', routerLink: ['/export-report/business-report'], isShow: this.isPermission(PermissionInvestConst.Invest_BaoCao_KinhDoanh)}
                ]
            },
            {
                label: 'Truy vấn', icon: 'pi pi-sync', routerLink: ['/query-manager'], isShow: this.isPermission(PermissionInvestConst.Invest_Menu_TruyVan),
                items: [
					{ label: 'Thu tiền ngân hàng', icon: '', routerLink: ['/query-manager/collect-money-bank'], isShow: this.isPermission(PermissionInvestConst.Invest_TruyVan_ThuTien_NganHang) },
                    { label: 'Chi tiền ngân hàng', icon: '', routerLink: ['/query-manager/pay-money-bank'], isShow: this.isPermission(PermissionInvestConst.Invest_TruyVan_ChiTien_NganHang) },
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
