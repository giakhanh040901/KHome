import { Component, Inject, Injector } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { ExportExcelConst, MessageErrorConst, PermissionRealStateConst } from "@shared/AppConsts";
import { IReport } from "@shared/interface/report.model";
import { UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { ExportReportService } from "@shared/services/export-report.service";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
	selector: "app-operational-report",
	templateUrl: "../management-report/management-report.component.html",
	styleUrls: ["../management-report/management-report.component.scss"],
})
export class OperationalReportComponent extends AppComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _exportReportService: ExportReportService,
		private breadcrumbService: BreadcrumbService,
		private _userService: UserServiceProxy,
		@Inject(API_BASE_URL) baseUrl?: string
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Báo cáo vận hành" },
		]);
	}
	
	startDate: any;
	endDate: any;
	type: any = null;
	
	isLoadingPage: boolean = false;
	isDownload: boolean = false;
	permission: any;

	reports: IReport[] = [];
	PermissionRealStateConst = PermissionRealStateConst;

	ngOnInit(): void {
		this.isLoadingPage = true;
		this._userService.getAllPermission().subscribe(
			(res) => {
				this.isLoadingPage = false;
				this.permission = res.data;
				this.reports = [];

				if (
					this.isGranted([
						PermissionRealStateConst.RealState_BaoCao_VanHanh_TQBangHangDuAn,
					])
				) {
					this.reports.push({
						name: "BÁO CÁO TỔNG QUAN BẢNG HÀNG DỰ ÁN",
						type: ExportExcelConst.TONG_QUAN_BANG_HANG_DU_AN,
					});
				}

				if (
					this.isGranted([
						PermissionRealStateConst.RealState_BaoCao_VanHanh_TH_TienVeDA,
					])
				) {
					this.reports.push({
						name: "BÁO CÁO TỔNG HỢP TIỀN VỀ DỰ ÁN",
						type: ExportExcelConst.TONG_HOP_TIEN_VE_DU_AN,
					});
				}

				if (
					this.isGranted([
						PermissionRealStateConst.RealState_BaoCao_VanHanh_TH_CacKhoanGD,
					])
				) {
					this.reports.push({
						name: "BÁO CÁO TỔNG HỢP CÁC KHOẢN GIAO DỊCH",
						type: ExportExcelConst.TONG_HOP_CAC_KHOAN_GIAO_DICH,
					});
				}
			},
			() => {}
		);
	}

    isGranted(permissionNames = []): boolean {
        for (let permissionName of permissionNames) {
            if (this.permission?.includes(permissionName)) return true;
        }
        return false;
    }

	onAccept() {
		if (this.validForm()) {
			this.isDownload = true;
			this._exportReportService.exportExcel(this.startDate, this.endDate, this.type).subscribe((res) => {
					res?.code && this.handleResponseInterceptor(res);
					this.isDownload = false;
				});
		} else {
			this.messageError(MessageErrorConst.message.Validate);
		}
	}

	validForm(): boolean {
		let validRequired;
		validRequired = this.startDate && this.endDate && this.type;

		return validRequired;
	}
}
