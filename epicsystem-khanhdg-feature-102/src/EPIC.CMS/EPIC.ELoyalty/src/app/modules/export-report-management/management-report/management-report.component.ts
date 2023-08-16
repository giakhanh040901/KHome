import { Component, Inject, OnInit } from '@angular/core';
import { ExportReportConst, PermissionLoyaltyConst } from '@shared/AppConsts';
import { formatCalendarItem } from '@shared/function-common';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ExportReportService } from '@shared/services/export-report.service';
import { HelpersService } from '@shared/services/helpers.service';
import { PermissionsService } from '@shared/services/permissions.service';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
	selector: 'app-management-report',
	templateUrl: './management-report.component.html',
	styleUrls: ['./management-report.component.scss'],
})
export class ManagementReportComponent implements OnInit {
	constructor(
		private _exportReportService: ExportReportService,
        private breadcrumbService: BreadcrumbService,
        private _permissionService: PermissionsService,
        private _helpersService: HelpersService,
        @Inject(API_BASE_URL) baseUrl?: string,
	) {
		this.breadcrumbService.setItems([
            { label: "Trang chủ", routerLink: ["/home"] },
            { label: "Báo cáo quản trị" },
        ]);
	}

	startDate: any;
    endDate: any;
    type: string;

	isDownload: boolean = false;
    permissions: any[] = [];

    reports: any[] = [];
    ExportReportConst = ExportReportConst;

	ngOnInit(): void {
		this._permissionService.getPermissions$.subscribe(response => {
            this.permissions = response;
            this.reports = [];

			if(this.isGranted([PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri_DSYeuCauDoiVoucher])) {
                this.reports.push({
                    name: "BÁO CÁO DANH SÁCH YÊU CẦU ĐỔI VOUCHER",
                    type: ExportReportConst.BC_DS_YC_DOI_VOUCHER,
                })
            }

            if(this.isGranted([PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri_XuatNhapTonVoucher])) {
                this.reports.push({
                    name: "BÁO CÁO XUẤT NHẬP TỒN VOUCHER",
                    type: ExportReportConst.BC_XUAT_NHAT_TON_VOUCHER,
                })
            }
		})
	}

	isGranted(permissionNames = []): boolean {
        for (let permissionName of permissionNames) {
            if (this.permissions?.includes(permissionName)) return true;
        }
        return false;
    }

	onAccept() {
        this.isDownload = true;
		let startDate = formatCalendarItem(this.startDate);
		let endDate = formatCalendarItem(this.endDate);
        this._exportReportService.exportExcel(startDate, endDate, this.type).subscribe((res) => {
            res?.code && this._helpersService.handleResponseInterceptor(res);
            this.isDownload = false;
        });
        
    }

    validForm(): boolean {
        let validRequired;
        validRequired = (this.startDate || this.endDate) && this.type;             
        return !!validRequired;
    }
}
