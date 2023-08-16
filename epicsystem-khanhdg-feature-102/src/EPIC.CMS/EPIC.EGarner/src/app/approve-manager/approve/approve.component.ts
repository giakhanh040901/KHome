import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchConst, ApproveConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ApproveServiceProxy } from "@shared/service-proxies/approve-service";
import { DistributionService } from "@shared/services/distribution.service";
import { ProductService } from "@shared/services/product.service";
import * as moment from "moment";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { Subject } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { FormApproveComponent } from "src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component";
import { FormCancelComponent } from "src/app/form-general/form-request-approve-cancel/form-cancel/form-cancel.component";
import { FormSetDisplayColumnComponent } from "src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
	selector: "app-approve",
	templateUrl: "./approve.component.html",
	styleUrls: ["./approve.component.scss"],
	providers: [DialogService, ConfirmationService, MessageService],
})
export class ApproveComponent extends CrudComponentBase implements OnDestroy {
	constructor(
		injector: Injector,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private router: Router,
		private _productService: ProductService,
		private routeActive: ActivatedRoute,
		private approveService: ApproveServiceProxy,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private _distributionService: DistributionService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Quản lý duyệt", routerLink: ["/approve-manager/approve/1"] },
		]);
	}

	ref: DynamicDialogRef;

	modalDialog: boolean;
	deleteItemDialog: boolean = false;
	deleteItemsDialog: boolean = false;

	rows: any[] = [];
	cols: any[];
	_selectedColumns: any[];
	listAction: any[] = [];

	ApproveConst = ApproveConst;

	
	approveId: number;
	approve: any = {};

	dataType: number;
	routeSubcribe: any = null;

	fieldErrors = {};
	fieldDates = ["licenseDate", "decisionDate", "dateModified"];

	submitted: boolean;
	expandedRows = {};
	
	page = new Page();
	offset = 0;

	actions: any[] = [];
	actionsDisplay: any[] = [];

	fieldFilterDates = ['requestDate', 'approveDate'];
	fieldFilters = {
		keyword: null,
		status: null,
		actionType: null,
		requestDate: null,
		approveDate: null,
	}

	ngOnInit(): void {
		this.isLoading = true;
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.fieldFilters.requestDate = '';
				this.fieldFilters.approveDate = '';
				this.setPage({ page: this.offset }, params.dataType);
			}
			this.dataType = params.dataType;
		});

		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});

		this.cols = [
			{ field: 'summary', header: 'Nội dung duyệt', width: '40rem' },
			{ field: 'confirmDate', header: 'Ngày duyệt/ hủy', width: '10.5rem' },
			{ field: 'requestNote', header: 'Ghi chú trình duyệt', width: '18rem' },
			{ field: 'approveNote', header: 'Ghi chú duyệt', width: '18rem' },
			{ field: 'cancelNote', header: 'Ghi chú hủy duyệt', width: '18rem' },
			{ field: 'isCheck', header: 'EPIC duyệt', width: '10rem', class: 'justify-content-center' },
			{ field: 'columnResize', header: '', type:'hidden' },
		];

		if ( this.dataType == 1){
			this.cols.unshift({field: 'issuer', header: 'Tổ chức phát hành', width: '20rem'});
		}
		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		//
		this._selectedColumns = this.getLocalStorage('approveGan') ?? this.cols;
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {

			const actions = [
				{
					data: item,
					label: "Thông tin chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail();
					},
				}
			];

			if (item?.status == 1 && 
				((this.isGranted([this.PermissionGarnerConst.GarnerPDSPTL_PheDuyetOrHuy]) && this.dataType == ApproveConst.STATUS_PRODUCT) 
				|| (this.isGranted([this.PermissionGarnerConst.GarnerPDPPSP_PheDuyetOrHuy]) && this.dataType == ApproveConst.STATUS_DISTRIBUTION))) {
				actions.push({
					data: item,
					label: "Xử lý yêu cầu",
					icon: "pi pi-check",
					command: ($event) => {
						this.approveSharing($event.item.data);
					},
				});
			}
			return actions;
		});
	}

	approveSharing(investor) {
		if (investor.dataType == ApproveConst.STATUS_PRODUCT || investor.dataType == ApproveConst.STATUS_DISTRIBUTION) {
			const ref = this.dialogService.open(
				FormApproveComponent,
				{
					header: "Xử lý yêu cầu",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						id: investor.referId
					},
				}
			);
			console.log("abc", investor);

			ref.onClose.subscribe((dataCallBack) => {
				if (dataCallBack?.accept) {
					const body1 = {
						approveNote: dataCallBack?.data?.approveNote,
						id: investor.referId,
						cancelNote: dataCallBack?.data?.approveNote,
					}
					if (investor.dataType == ApproveConst.STATUS_PRODUCT && dataCallBack?.checkApprove == true) {

						this._productService.approve(body1).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
								this.setPage(this.getPageCurrentInfo(), this.dataType);
							}
						});
					} else if (investor.dataType == ApproveConst.STATUS_PRODUCT && dataCallBack?.checkApprove == false) {
						this._productService.cancel(body1).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
								this.setPage(this.getPageCurrentInfo(), this.dataType);
							}
						});
					} else if (investor.dataType == ApproveConst.STATUS_DISTRIBUTION && dataCallBack?.checkApprove == true) {

						this._distributionService.approve(body1).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
								this.setPage(this.getPageCurrentInfo(), this.dataType);
							}
						});
					} else if (investor.dataType == ApproveConst.STATUS_DISTRIBUTION && dataCallBack?.checkApprove == false) {
						this._distributionService.cancel(body1).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
								this.setPage(this.getPageCurrentInfo(), this.dataType);
							}
						});
					}
				}
			});
		}
	}

	changeRequestDate() {
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage(this.getPageCurrentInfo(), params.dataType);
			}
		});
	}

	changeActionType() {
		this.isLoading = true;
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage(this.getPageCurrentInfo(), params.dataType);
			}
		});
	}

	changeStatus() {
		this.isLoading = true;

		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage({ page: this.offset }, params.dataType,);
			}
		});
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'approveGan');
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.issuer = row?.cpsIssuer?.name,
			row.requestDate = row?.requestDate,
			row.summary = row?.summary,
			row.requestNote = row.requestNote ;
			row.confirmDate = (row.approveDate || row.cancelDate) ? this.formatDateTime(row?.approveDate || row.cancelDate) : null;
		};
	}

	ngOnDestroy(): void {
		this.routeSubcribe.unsubscribe();
	}

	clickDropdown(row) {
		this.approve = { ...row };
		this.actionsDisplay = this.actions.filter((action) => action.statusActive.includes(row.status) && action.permission);
		console.log({ approve: row });
	}

	detail() {
		switch (this.approve.dataType) {
			case this.ApproveConst.STATUS_PRODUCT:
				this.router.navigate(["/product-manager/detail/" + this.cryptEncode(this.approve.referId)]);
				break;
			case this.ApproveConst.STATUS_DISTRIBUTION:
				this.router.navigate(["/product-manager/distribution/detail/" + this.cryptEncode(this.approve.referId)]);
				break;
			default:
				break;
		}
	}

	setFieldError() {
		for (const [key, value] of Object.entries(this.approve)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}

	confirm() {
		this.approve = true;
	}

	getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

	setPage(pageInfo?: any, dataType?: any) {
		this.setFieldError();
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

		this.fieldFilters.keyword = this.keyword;
		this.isLoading = true;
		if (dataType) this.dataType = dataType;

		let fieldFilters = this.formatCalendar(this.fieldFilterDates, {...this.fieldFilters});

		this.approveService.getAll(this.page, this.dataType, fieldFilters).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				if (this.rows?.length) {
					this.showData(this.rows);
					this.genListAction(this.rows);
				}
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
			}
		},
			(err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			}
		);
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}

}
