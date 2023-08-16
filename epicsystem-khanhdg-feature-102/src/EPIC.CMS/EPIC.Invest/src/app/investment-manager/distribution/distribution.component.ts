import { Component, Injector } from "@angular/core";
import { AppConsts, AtributionConfirmConst, DistributionConst, ProductBondInterestConst, ProductBondSecondaryConst, ProductPolicyConst, SearchConst, TableConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DialogService } from "primeng/dynamicdialog";
import { Router } from "@angular/router";
import { DistributionService } from '@shared/services/distribution.service';
import { CreateDistributionComponent } from './create-distribution/create-distribution.component';
import { FormRequestComponent } from "src/app/form-general/form-request-approve-cancel/form-request/form-request.component";
import { FormApproveComponent } from "src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component";
import { DataTableEmit, IColumn } from "@shared/interface/p-table.model";
import { debounceTime } from "rxjs/operators";
import { BasicFilter } from "@shared/interface/filter.model";

@Component({
	selector: 'app-distribution',
	templateUrl: './distribution.component.html',
	styleUrls: ['./distribution.component.scss']
})
export class DistributionComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _distributionService: DistributionService,
		private _router: Router,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Phân phối đầu tư" }
		]);
	}

	// ACTION BUTTON
	listAction: any[] = [];

	// CONST DATA
	ProductSecondaryConst = ProductBondSecondaryConst;
	ProductPolicyConst = ProductPolicyConst;
	ProductBondInterestConst = ProductBondInterestConst;
	YesNoConst = YesNoConst;
	DistributionConst = DistributionConst;
	AppConsts = AppConsts;

	//
	dataFilter: BasicFilter = new BasicFilter();

	fieldDates = ["openCellDate", "closeCellDate"];
	page = new Page();

	rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();
	
	ngOnInit() {	
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'project.invCode', header: 'Mã dự án', width: 15, isPin: true, isSort: true },
			{ field: 'invName', header: 'Dự án', width: 25, isPin: true, isResize: true, isSort: true },
			{ field: 'isInvested', header: 'Số tiền đã đầu tư', width: 12, type: TableConst.columnTypes.CURRENCY, isSort: true},
			{ field: 'project.totalInvestmentDisplay', header: 'Tổng mức đầu tư', width: 12, type: TableConst.columnTypes.CURRENCY, isSort: true},
			{ field: 'hanMucToiDa', header: 'Hạn mức còn lại', width: 12, type: TableConst.columnTypes.CURRENCY, isSort: true},
			{ field: 'openCellDate', header: 'Ngày mở bán', width: 10.5, type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'closeCellDate', header: 'Ngày kết thúc', width: 10.5, type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'isShowApp', header: 'Show App', width: 7, type: TableConst.columnTypes.CHECKBOX_SHOW },
			{ field: 'isCheck', header: 'Kiểm tra', width: 6, type: TableConst.columnTypes.CHECKBOX_SHOW },
			{ field: 'status', header: 'Trạng thái', width: 7.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end b-table-actions' },
		];
		//
		this.setPage();
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._distributionService.getAllDistribution(this.page, this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			this.rows = res.data.items || [];
			if(this.rows?.length) { 
				this.genListAction(this.rows);
				this.setData(this.rows);
			}
			this.page.totalItems = res?.data?.totalItems;
		} , (err) => {
			this.isLoading = false;
		});
	}


	setData(rows) {
		for (let row of rows) {
			row['project.invName'] = row?.project?.invName;
			row['project.invCode'] = row?.project?.invCode;
			row['project.totalInvestmentDisplay'] = row?.project?.totalInvestmentDisplay;
			if(row.isClose === YesNoConst.YES) row.status = DistributionConst.CLOSED;
			//
			row.statusElement = DistributionConst.getStatusInfo(row?.status);
		}
	}

	onSort(event) {
		this.dataFilter.sortFields = event;
		this.setPage();
	}

	
	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(distribution => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_ThongTinPPDT])) {
				actions.push({
					data: distribution,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data?.id);
					}
				})
			}

			// if (distribution.status == this.ProductSecondaryConst.STATUS.NHAP && this.isGranted([this.PermissionInvestConst.InvestPPDT_TrinhDuyet])) {
			// 	actions.push({
			// 		data: distribution,
			// 		label: 'Trình duyệt',
			// 		icon: 'pi pi-arrow-up',
			// 		command: ($event) => {
			// 			this.request($event.item.data);
			// 		}
			// 	});
			// }

			// if (distribution.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted([])) {
			// 	actions.push({
			// 		data: distribution,
			// 		label: 'Phê duyệt',
			// 		icon: 'pi pi-check',
			// 		command: ($event) => {
			// 			this.approve($event.item.data);
			// 		}
			// 	});
			// }

			// if (distribution.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted([])) {
			// 	actions.push({
			// 		data: distribution,
			// 		label: 'Hủy duyệt',
			// 		icon: 'pi pi-times',
			// 		command: ($event) => {
			// 			this.cancel($event.item.data);
			// 		}
			// 	});
			// }

			if (distribution.status == this.ProductSecondaryConst.STATUS.HOAT_DONG && distribution.isCheck == YesNoConst.NO && this.isGranted([this.PermissionInvestConst.InvestPPDT_EpicXacMinh])) {
				actions.push({
					data: distribution,
					label: 'Phê duyệt (Epic)',
					icon: 'pi pi-check',
					command: ($event) => {
						this.check($event.item.data);
					}
				});
			}

			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_DongTam])) { 
				actions.push({
					data: distribution,
					label: distribution.isClose == YesNoConst.NO ? 'Đóng tạm' : 'Mở',
					icon: distribution.isClose == YesNoConst.NO ? 'pi pi-lock' : 'pi pi-lock-open',
					command: ($event) => {
						this.toggleClosed($event.item.data?.id);
					}
				});
			}

			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_BatTatShowApp])) {
				actions.push({
					data: distribution,
					label: distribution.isShowApp === YesNoConst.NO ? 'Bật show app' : 'Tắt show app',
					icon: distribution.isShowApp === YesNoConst.NO ? 'pi pi-eye' : 'pi pi-eye-slash',
					command: ($event) => {
						this.toggleIsShowApp($event.item.data?.id);
					}
				});
			}

			return actions;
		});
	}

	// Thêm phân phối đầu tư
	create() {
		const ref = this.dialogService.open(
			CreateDistributionComponent,
			{
				header: 'Thêm phân phối đầu tư',
				width: '1000px',
			}
		);
		//
		ref.onClose.subscribe((distribution) => {
			if (distribution?.id) {
				this.messageService.add({ severity: 'success', summary: '', detail: "Thêm mới thành công", life: 1500 });
				this.view(distribution?.id);
			}
		});
	}

	view(distributionId) {
		this._router.navigate(["invest-manager/distribution/detail/" + this.cryptEncode(distributionId)]);
	}

	request(distribution) {
		const params = {
			id: distribution.id,
			summary: 'Phân phối đầu tư bán theo kỳ hạn',
		}
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage();
					}
				});
			}
		});
	}

	// Hủy Duyệt
	cancel(distribution) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", { id: distribution?.id })
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	// DUYỆT / BỎ DUYỆT
	approve(distribution) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: distribution?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	// Api Epic kiểm tra
	check(distribution) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt phân phối đầu tư phiếu này không?',
			...AtributionConfirmConst,
			accept: () => {
				this._distributionService.check({ id: distribution?.id }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			},
			reject: () => {
			},
		});
	}

	// KHOA TAM
	toggleClosed(distributionId) {
		this._distributionService.toggleIsClosed(distributionId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật trạng thái đóng thành công")) {
					this.setPage(this.page);
				}
			}, (err) => {
				console.log('err--', err);
			}
		);
	}

	// BAT TAT IS SHOW APP
	toggleIsShowApp(distributionId) {
		this._distributionService.toggleIsShowApp(distributionId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật show app thành công")) {
					this.setPage(this.page);
				}
			}, (err) => {
				console.log('err---', err);
			}
		);
	}
}
