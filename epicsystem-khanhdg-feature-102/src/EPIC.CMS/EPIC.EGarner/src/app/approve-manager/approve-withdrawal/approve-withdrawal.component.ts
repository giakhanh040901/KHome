import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchConst, ApproveConst, WithdrawalConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ApproveServiceProxy } from "@shared/service-proxies/approve-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormApproveRequestComponent } from "src/app/form-general/form-approve-request/form-approve-request.component";
import { WithdrawalService } from "@shared/services/withdrawal-service";

@Component({
  selector: 'app-approve-withdrawal',
  templateUrl: './approve-withdrawal.component.html',
  styleUrls: ['./approve-withdrawal.component.scss']
})
export class ApproveWithdrawalComponent extends CrudComponentBase {

  constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private _withdrawalService: WithdrawalService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Phê duyệt yêu cầu tái tục"},
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

	cols: any[];
	_selectedColumns: any[];
	listAction: any[] = [];

	ApproveConst = ApproveConst;

	statusSearch: any[] = [
		{
			name: "Tất cả",
			code: ''

		},
		...ApproveConst.statusConst];

	approveId: number;
	approve: any = {};
	dataType: number;
	submitted: boolean;
	expandedRows = {};
	//
	page = new Page();
	offset = 0;

	//
	actions: any[] = []; // list button actions
	actionsDisplay: any[] = [];

	ngOnInit(): void {
		this.isLoading = true;

    this.setPage();

		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});

		this.cols = [
			{ field: 'requestWithdrawalDate', header: 'Ngày y/c rút', width: '8rem', isPin: true },
			{ field: 'customer', header: 'Khách hàng', width: '16rem', isPin: true },
			{ field: 'amountMoneyDisplay', header: 'Số tiền rút', width: '10rem', cutText: 'b-cut-text-12', isPin: true },
			{ field: 'policyName', header: 'Chính sách', width: '12rem' },
			{ field: 'approveByDisplay', header: 'Người duyệt / Hủy', width: '12rem', isResize: true },
			{ field: 'approveDateDisplay', header: 'Ngày duyệt / Hủy', width: '11rem' },
			// { field: 'userRequest', header: 'Người y/c', width: '12rem' },
			{ field: 'requestDate', header: 'Ngày tạo', width: '11rem', isPin: true },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		
		this._selectedColumns = this.getLocalStorage('approveGan') ?? this.cols;
	}

	showData(rows) {
		for (let row of rows) {
			row.requestWithdrawalDate = row?.withdrawalDate ? this.formatDate(row?.withdrawalDate) : null,
			row.amountMoneyDisplay = row?.amountMoney ? this.utils.transformMoney(row.amountMoney) : null,
			row.customer = row?.investor?.investorIdentification?.fullname || row?.businessCustomer?.name;
			row.policyName = row?.policy?.code;
			row.approveByDisplay = row?.approveBy || row?.cancelBy;
			row.approveDateDisplay = (row?.approveDate || row?.cancelDate) ? this.formatDateTime(row?.approveDate || row?.cancelDate) : null;
			row.userRequest = row?.approveBy || row?.cancelBy;
			row.requestDate = row?.createdDate ? this.formatDateTime(row?.createdDate) : null,
			row.confirmDate = row?.approveDate ? this.formatDateTime(row.approveDate) : null;
		};
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {

			const actions = []

			if (item?.status == WithdrawalConst.REQUEST && this.isGranted([this.PermissionGarnerConst.GarnerPDYCRV_PheDuyetOrHuy])) {
				actions.push({
					data: item,
					label: "Phê duyệt",
					icon: "pi pi-check",
					command: ($event) => {
						this.approveOrCancel($event.item.data);
					},
				});
			}
			//
			// if (this.isGranted([this.PermissionGarnerConst.GarnerPDYCRV_PheDuyetOrHuy])) {
			// 	actions.push({
			// 		data: item,
			// 		label: "Thông tin chi tiết",
			// 		icon: "pi pi-info-circle",
			// 		command: ($event) => {
			// 			this.approveOrCancel($event.item.data);
			// 		},
			// 	});
			// }

			return actions;
		});

		console.log(this.listAction);
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
				this.setLocalStorage(this._selectedColumns, 'approveGan')
			}
		});
	}
	
	approveOrCancel(item) {
		const ref = this.dialogService.open(
			FormApproveRequestComponent,
			{
				header: 'Phê duyệt yêu cầu rút tiền',
				width: '500px',
				contentStyle: { "height": "auto", "overflow": "hidden", "padding-bottom": "50px" },
        		style: { "overflow": "hidden"},
				data: {
					dataType: ApproveConst.STATUS_WITHDRAWAL,
					item: item,
				}
			});

		ref.onClose.subscribe((res) => {
			if (res.status) {
				this.messageSuccess('Thao tác thành công');
				this.setPage();
			}
		});
	}

	changeKeyword() {
		if (this.keyword === "") {
			this.setPage({ page: this.offset });
		}
	}

	setPage(pageInfo?: any, dataType?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		//
		this.isLoading = true;
		this._withdrawalService.getAll(this.page).subscribe((res) => {
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
}
