import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { IssuerConst, DistributionContractConst, SearchConst, ApproveConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ApproveServiceProxy } from "@shared/service-proxies/approve-service";
import { ProductBondInfoServiceProxy, ProductBondSecondaryServiceProxy } from "@shared/service-proxies/bond-manager-service";
import * as moment from "moment";

import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";
import { FormSetDisplayColumnComponent } from "src/app/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { FormBondInfoApproveComponent } from "./form-bond-info-approve/form-bond-info-approve.component";

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
		private routeActive: ActivatedRoute,
		private approveService: ApproveServiceProxy,
		private _productBondSecondaryService: ProductBondSecondaryServiceProxy,
		messageService: MessageService,
		private _productBondInfoService: ProductBondInfoServiceProxy,
		private breadcrumbService: BreadcrumbService
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
	row: any;
	col: any;
	requestDate: any;
	approveDate: any;
	actionType: any;
	listAction: any[] = [];
	cols: any[];
	_selectedColumns: any[];
	checkApprovePage: any;
	ApproveConst = ApproveConst;
	IssuerConst = IssuerConst;
	DistributionContractConst = DistributionContractConst;


	statusSearch: any[] = [
		{
			name: "Tất cả",
			code: ''

		},
		...ApproveConst.statusConst];

	actionTypeSearch = [
		{
			name: "Tất cả",
			code: ''
		},
		...ApproveConst.actionType];

	approveId: number;
	approve: any = {};
	dataType: number;
	routeSubcribe: any = null;

	fieldErrors = {};
	fieldDates = ["licenseDate", "decisionDate", "dateModified"];
	submitted: boolean;
	expandedRows = {};
	//
	page = new Page();
	offset = 0;

	//
	actions: any[] = []; // list button actions
	actionsDisplay: any[] = [];

	ngOnInit(): void {
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			this.checkApprovePage = params.dataType;
			if (params.dataType) {
				this.status = '';
				this.actionType = '';
				this.requestDate = '';
				this.approveDate = '';
				this.setPage({ page: this.offset }, params.dataType);
				console.log("12324356475868ouỳdhdzgdsADsrdfkfuỵdstửqằgsrdr5ye", params.dataType);

			}
		});

		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});


		this.cols = [
			{ field: 'summary', header: 'Nội dung duyệt', width: '25rem', position: 1, cutText: 'b-cut-text-25' },
			{ field: 'approveDate', header: 'Ngày duyệt', width: '10rem', position: 2 },
			{ field: 'cancelDate', header: 'Ngày hủy', width: '10rem', position: 3 },
			{ field: 'requestNote', header: 'Ghi chú yêu cầu', width: '10rem', position: 4, cutText: 'b-cut-text-10' },
			{ field: 'approveNote', header: 'Ghi chú duyệt', width: '10rem', position: 5, cutText: 'b-cut-text-10' },
			{ field: 'cancelNote', header: 'Ghi chú hủy duyệt', width: '15rem', position: 5, cutText: 'b-cut-text-15' },
			{ field: 'isCheck', header: 'EPIC duyệt', width: '10rem', position: 6, class: 'justify-content-center' },
		];

		//   this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('approve') ?? this.cols;
	}

	genListAction(data = []) {
		this.listAction = data.map(dataBond => {
			const actions = [
				{
					data: dataBond,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail();
					}
				},

			];
			console.log('actions',this.checkApprovePage)
			if (dataBond.status == ApproveConst.TRINH_DUYET && ((this.isGranted([this.PermissionBondConst.BondQLPD_PDLTP_PheDuyetOrHuy]) && this.checkApprovePage == 4)
			|| (this.isGranted([this.PermissionBondConst.BondQLPD_PDBTKH_PheDuyetOrHuy]) && this.checkApprovePage == 5))) {
				actions.push({
					data: dataBond,
					label: 'Phê duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.approveBond($event.item.data);
					}
				});
			}

			return actions;
		});
	}
	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('approve', JSON.stringify(data));
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
			}
		});
	}

	approveBond(bondData) {
		console.log("bondDataâ",bondData);
		
		const ref = this.dialogService.open(
			FormBondInfoApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: bondData
			}
		);

		ref.onClose.subscribe((dataCallBack) => {

			if (dataCallBack?.accept) {
				if (this.dataType == 4) {
					if (dataCallBack.checkApprove == true) {
						this._productBondInfoService.approve(dataCallBack.data).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
								this.routeSubcribe = this.routeActive.params.subscribe((params) => {
									if (params.dataType) {
										this.setPage({ page: this.offset }, params.dataType);
									}
								});
							}
						});
					}
					else {
						this._productBondInfoService.cancel(dataCallBack.data).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Huỷ duyệt thành công!")) {
								this.routeSubcribe = this.routeActive.params.subscribe((params) => {
									if (params.dataType) {
										this.setPage({ page: this.offset }, params.dataType);
									}
								});
							}
						});
					}
				}

				if (this.dataType == 5) {
					if (dataCallBack.checkApprove == true) {
						this._productBondSecondaryService.approve(dataCallBack.data).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
								this.routeSubcribe = this.routeActive.params.subscribe((params) => {
									if (params.dataType) {
										this.setPage({ page: this.offset }, params.dataType);
									}
								});
							}
						});
					}
					else {
						this._productBondSecondaryService.cancel(dataCallBack.data).subscribe((response) => {
							if (this.handleResponseInterceptor(response, "Huỷ duyệt thành công!")) {
								this.routeSubcribe = this.routeActive.params.subscribe((params) => {
									if (params.dataType) {
										this.setPage({ page: this.offset }, params.dataType);
									}
								});
							}
						});
					}
				}

			}
		});
	}
	showData(rows) {
		for (let row of rows) {
			row.summary = row?.summary,
				row.approveDate = this.formatDateTime(row?.approveDate);
			row.cancelDate = this.formatDateTime(row?.cancelDate);
			row.requestNote = row.requestNote;
			row.approveNote = row.approveNote;
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
			case this.ApproveConst.STATUS_USER:
				this.router.navigate(["/user/detail/" + this.approve.referIdTemp]);
				break;
			case this.ApproveConst.STATUS_INVESTOR:
				this.router.navigate([`/customer/investor/${this.approve.referIdTemp}/temp/1`]);
				break;
			case this.ApproveConst.STATUS_BUSINESS_CUSTOMER:
				this.router.navigate(["/customer/business-customer/business-customer/detail/" + this.approve.referIdTemp]);
				break;
			case this.ApproveConst.STATUS_PRO_BOND_INFO:
				this.router.navigate(["/bond-manager/product-bond-info/detail/" + this.cryptEncode(this.approve.referId)]);
				break;
			case this.ApproveConst.STATUS_PRO_BOND_SECONDARY:
				this.router.navigate(["/bond-manager/product-bond-secondary/update/" + this.cryptEncode(this.approve.referId)]);
				break;
			case this.ApproveConst.STATUS_PRO_BOND_PRIMARY:
				this.router.navigate(["/bond-manager/product-bond-primary/detail/" + this.cryptEncode(this.approve.referId)]);
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

	changeStatus() {
		this.isLoading = true;
		this.isLoadingPage = true;

		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage({ page: this.offset }, params.dataType,);
			}
		});
	}

	changeRequestDate() {
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage({ page: this.offset }, params.dataType);
			}
		});
	}

	changeActionType() {
		this.isLoading = true;
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				this.setPage({ page: this.offset }, params.dataType);
			}
		});
	}

	changeKeyword() {
		this.routeSubcribe = this.routeActive.params.subscribe((params) => {
			if (params.dataType) {
				console.log({ params });
				this.setPage({ page: this.offset }, params.dataType);
			}
		});
	}

	setPage(pageInfo?: any, dataType?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		if (dataType) this.dataType = dataType;
		if (this.requestDate) {
			var requestDate = moment(this.requestDate).format('YYYY-MM-DD');
		}
		if (this.approveDate) {
			var approveDate = moment(this.approveDate).format('YYYY-MM-DD');
		}
		this.approveService.getAll(this.page, this.dataType, this.status, this.actionType, requestDate, approveDate).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems; ``
				this.rows = res.data?.items;
				if (this.rows?.length) {
					this.showData(this.rows);
					this.genListAction(this.rows);
				}
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
			}
		}, (err) => {
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
