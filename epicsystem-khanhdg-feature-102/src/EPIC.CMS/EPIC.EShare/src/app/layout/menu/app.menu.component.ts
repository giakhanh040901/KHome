import jwt_decode from "jwt-decode";
import { AppMainComponent } from '../main/app.main.component';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase } from "@angular/common/http";
import { ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppConsts, PermissionCompanyShareConst, UserTypes } from '@shared/AppConsts';
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
            // { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: this.isPermission(PermissionCompanyShareConst.CompanySharePageDashboard) },
            { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'], isShow: true },
            {
                label: 'Cài đặt', icon: 'pi pi-fw pi-cog', routerLink: ['/setting'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat),
                items: [
                    {label: this.labelCheck, icon: '', routerLink: [this.routerLinkCheck], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_CHNNL)},
                    {label: 'Tổ chức phát hành', icon: '', routerLink: ['/setting/issuer'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_TCPH)},
                    {label: 'Đại lý sơ cấp', icon: '', routerLink: ['/setting/trading-provider'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_DLSC)},
                    // {label: 'Đại lý lưu ký', icon: '', routerLink: ['/setting/deposit-provider'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_DLLK)},
                    {label: 'Chính sách mẫu', icon: '', routerLink: ['/setting/company-share-policy-template'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_CSM)},
                    {label: 'Mẫu thông báo', icon: '', routerLink: ['/setting/system-notification'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_MTB)},
                    { label: 'Hình ảnh', icon: '', routerLink: ['/setting/media'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuCaiDat_HinhAnh) && UserTypes.TYPE_TRADING.includes(this.checkPartner.user_type)},
                ]
            },
            {
                label: 'Quản lý cổ phần', icon: 'pi pi-map', routerLink: ['/company-share-manager'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLTP),
                items: [
                    {label: 'Lô cổ phần', icon: '', routerLink: ['/company-share-manager/company-share-info'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP)},
                    {label: 'Phát hành sơ cấp', icon: '', routerLink: ['/company-share-manager/company-share-primary'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC)},
                    {label: 'Hợp đồng phân phối', icon: '', routerLink: ['/company-share-manager/distribution-contract'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLTP_HDPP)},
					{ label: 'Bán theo kỳ hạn', icon: '', routerLink: ['/company-share-manager/company-share-secondary'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH) },
                ]
            },
			{
                label: 'Hợp đồng phân phối', icon: 'pi pi-book', routerLink: ['/trading-contract'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuHDPP),
                items: [
                    // { label: 'Đặt lệnh', icon: '', routerLink: ['/'] },
                    { label: 'Sổ lệnh', icon: '', routerLink: ['/trading-contract/order'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_SoLenh) },
                    { label: 'Xử lý hợp đồng', icon: '', routerLink: ['/trading-contract/contract-processing'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_XLHD) },
                    { label: 'Hợp đồng', icon: '', routerLink: ['/trading-contract/contract-active'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_HopDong) },
                    { label: 'Giao nhận hợp đồng', icon: '', routerLink: ['/trading-contract/delivery-contract'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_GiaoNhanHopDong) },
                    { label: 'Phong tỏa, giải tỏa', icon: '', routerLink: ['/trading-contract/contract-blockage'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_PTGT) },
                    { label: 'Hợp đồng đáo hạn', icon: '', routerLink: ['/trading-contract/interest-contract'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareHDPP_HDDH) },
                ]
            },
            {
                label: 'Quản lý phê duyệt', icon: 'pi pi-check', routerLink: ['/approve-manager'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareMenuQLPD),
                items: [
                    { label: 'Phê duyệt lô cổ phần', icon: '', routerLink: ['/approve-manager/approve/4'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareQLPD_PDLTP) },
                    { label: 'Phê duyệt bán theo kỳ hạn', icon: '', routerLink: ['/approve-manager/approve/5'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareQLPD_PDBTKH) },
                    { label: 'Phê duyệt yêu cầu tái tục', icon: '', routerLink: ['/approve-manager/request-reinstatement'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShareQLPD_PDYCTT) },
                ]
            },
            {
                label: 'Báo cáo thông kê', icon: 'pi pi-save', routerLink: ['/export-report'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShare_Menu_BaoCao),
                items: [
                    { label: 'Báo cáo quản trị', icon: '', routerLink: ['/export-report/management-report'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShare_BaoCao_QuanTri)},
                    { label: 'Báo cáo vận hành', icon: '', routerLink: ['/export-report/operational-report'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShare_BaoCao_VanHanh)},
                    { label: 'Báo cáo kinh doanh', icon: '', routerLink: ['/export-report/business-report'], isShow: this.isPermission(PermissionCompanyShareConst.CompanyShare_BaoCao_KinhDoanh)}
                ]
            },
        ];
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        return true;
        return this.permissions.includes(keyName);
    }
}
