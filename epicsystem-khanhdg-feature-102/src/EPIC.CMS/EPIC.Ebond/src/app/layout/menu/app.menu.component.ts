import jwt_decode from "jwt-decode";
import { AppMainComponent } from '../main/app.main.component';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase } from "@angular/common/http";
import { ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppConsts, PermissionBondConst, UserTypes } from '@shared/AppConsts';
import { CookieService } from 'ngx-cookie-service';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { StatusResponseConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CalendarServiceProxy } from '@shared/service-proxies/setting-service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from 'primeng/tristatecheckbox';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { PermissionsService } from "@shared/services/permissions.service";

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {
    protected _cookieService: CookieService;
    model: any[];
    labelCheck: string;
    routerLinkCheck: string;
    checkPartner: any;
    checkBl: boolean;

    AppConsts = AppConsts;
    UserTypes = UserTypes;

    @Input() permissions:any[] = [];

    constructor(
        public appMain: AppMainComponent,
        cookieService: CookieService) {
        this._cookieService = cookieService;
      }
    // constructor(public appMain: AppMainComponent,
    //         cookieService: CookieService,
    //             ) {this._cookieService = cookieService;	}

    ngOnInit() {
        this.checkPartner = jwt_decode(this._cookieService.get(AppConsts.authorization.accessToken));
        console.log("this.checkPartner",this.checkPartner);
        Object.keys(this.checkPartner).forEach((key) => {
            console.log("this.key",key);
            if (key == `trading_provider_id`) {
            
                this.labelCheck = 'Cấu hình ngày nghỉ lễ';
                this.routerLinkCheck = '/setting/calendar';
            } else if (key == `partner_id`) {
            
                this.labelCheck = 'Cấu hình ngày nghỉ lễ';
                this.routerLinkCheck = '/setting/calendar-partner';
            }
        })
        console.log("this.checkPartner == true",this.checkBl );

        console.log("!UserTypes.TYPE_TRADING.includes(this.checkPartner)", !UserTypes.TYPE_TRADING.includes(this.checkPartner.user_type));
        
        this.model = [
            // { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: this.isPermission(PermissionBondConst.BondPageDashboard) },
            { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: true },
            {
                label: 'Cài đặt', icon: 'pi pi-fw pi-cog', routerLink: ['/setting'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat),
                items: [
                    {label: this.labelCheck, icon: '', routerLink: [this.routerLinkCheck], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_CHNNL)},
                    {label: 'Tổ chức phát hành', icon: '', routerLink: ['/setting/issuer'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_TCPH)},
                    {label: 'Đại lý sơ cấp', icon: '', routerLink: ['/setting/trading-provider'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_DLSC)},
                    {label: 'Đại lý lưu ký', icon: '', routerLink: ['/setting/deposit-provider'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_DLLK)},
                    {label: 'Chính sách mẫu', icon: '', routerLink: ['/setting/product-bond-policy-template'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_CSM)},
                    {label: 'Mẫu thông báo', icon: '', routerLink: ['/setting/system-notification'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_MTB)},
                    { label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow: this.isPermission(PermissionBondConst.BondMenuCaiDat_HinhAnh) && UserTypes.TYPE_TRADING.includes(this.checkPartner.user_type)},
                ]
            },
            {
                label: 'Quản lý trái phiếu', icon: 'pi pi-map', routerLink: ['/bond-manager'], isShow: this.isPermission(PermissionBondConst.BondMenuQLTP),
                items: [
                    {label: 'Lô trái phiếu', icon: '', routerLink: ['/bond-manager/product-bond-info'], isShow: this.isPermission(PermissionBondConst.BondMenuQLTP_LTP)},
                    {label: 'Phát hành sơ cấp', icon: '', routerLink: ['/bond-manager/product-bond-primary'], isShow: this.isPermission(PermissionBondConst.BondMenuQLTP_PHSC)},
                    {label: 'Hợp đồng phân phối', icon: '', routerLink: ['/bond-manager/distribution-contract'], isShow: this.isPermission(PermissionBondConst.BondMenuQLTP_HDPP)},
					{ label: 'Bán theo kỳ hạn', icon: '', routerLink: ['/bond-manager/product-bond-secondary'], isShow: this.isPermission(PermissionBondConst.BondMenuQLTP_BTKH) },
                ]
            },
			{
                label: 'Hợp đồng phân phối', icon: 'pi pi-book', routerLink: ['/trading-contract'], isShow: this.isPermission(PermissionBondConst.BondMenuHDPP),
                items: [
                    // { label: 'Đặt lệnh', icon: '', routerLink: ['/'] },
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/trading-contract/order'], isShow: this.isPermission(PermissionBondConst.BondHDPP_SoLenh) },
                    { label: 'Xử lý hợp đồng', icon: '', routerLink: ['/trading-contract/contract-processing'], isShow: this.isPermission(PermissionBondConst.BondHDPP_XLHD) },
                    { label: 'Hợp đồng', icon: '', routerLink: ['/trading-contract/contract-active'], isShow: this.isPermission(PermissionBondConst.BondHDPP_HopDong) },
                    { label: 'Giao nhận hợp đồng', icon: '', routerLink: ['/trading-contract/delivery-contract'], isShow: this.isPermission(PermissionBondConst.BondHDPP_GiaoNhanHopDong) },
                    { label: 'Phong tỏa, giải tỏa', icon: '', routerLink: ['/trading-contract/contract-blockage'], isShow: this.isPermission(PermissionBondConst.BondHDPP_PTGT) },
                    { label: 'Hợp đồng đáo hạn', icon: '', routerLink: ['/trading-contract/interest-contract'], isShow: this.isPermission(PermissionBondConst.BondHDPP_HDDH) },
                ]
            },
            {
                label: 'Quản lý phê duyệt', icon: 'pi pi-check', routerLink: ['/approve-manager'], isShow: this.isPermission(PermissionBondConst.BondMenuQLPD),
                items: [
                    { label: 'Phê duyệt lô trái phiếu', icon: '', routerLink: ['/approve-manager/approve/4'], isShow: this.isPermission(PermissionBondConst.BondQLPD_PDLTP) },
                    { label: 'Phê duyệt bán theo kỳ hạn', icon: '', routerLink: ['/approve-manager/approve/5'], isShow: this.isPermission(PermissionBondConst.BondQLPD_PDBTKH) },
                    { label: 'Phê duyệt yêu cầu tái tục', icon: '', routerLink: ['/approve-manager/request-reinstatement'], isShow: this.isPermission(PermissionBondConst.BondQLPD_PDYCTT) },
                ]
            },
            {
                label: 'Báo cáo thông kê', icon: 'pi pi-save', routerLink: ['/export-report'], isShow: this.isPermission(PermissionBondConst.Bond_Menu_BaoCao),
                items: [
                    { label: 'Báo cáo quản trị', icon: '', routerLink: ['/export-report/management-report'], isShow: this.isPermission(PermissionBondConst.Bond_BaoCao_QuanTri)},
                    { label: 'Báo cáo vận hành', icon: '', routerLink: ['/export-report/operational-report'], isShow: this.isPermission(PermissionBondConst.Bond_BaoCao_VanHanh)},
                    { label: 'Báo cáo kinh doanh', icon: '', routerLink: ['/export-report/business-report'], isShow: this.isPermission(PermissionBondConst.Bond_BaoCao_KinhDoanh)}
                ]
            },
        ];
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        // return true;
        return this.permissions.includes(keyName);
    }
}
