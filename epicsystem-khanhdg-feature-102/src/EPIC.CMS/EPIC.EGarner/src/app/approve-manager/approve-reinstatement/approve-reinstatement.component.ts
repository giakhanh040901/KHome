import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchConst, ApproveConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ApproveServiceProxy } from "@shared/service-proxies/approve-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormApproveRequestComponent } from "src/app/form-general/form-approve-request/form-approve-request.component";

@Component({
  selector: 'app-approve-reinstatement',
  templateUrl: './approve-reinstatement.component.html',
  styleUrls: ['./approve-reinstatement.component.scss']
})
export class ApproveReinstatementComponent extends CrudComponentBase {

  constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private routeActive: ActivatedRoute,
		private approveService: ApproveServiceProxy,
		private breadcrumbService: BreadcrumbService,
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
			{ field: 'summary', header: 'Nội dung duyệt', width: '30rem', isPin: true },
			{ field: 'confirmDate', header: 'Ngày duyệt/ hủy', width: '12rem' },
			{ field: 'requestNote', header: 'Ghi chú', width: '30rem', cutText: 'b-cut-text-30', isResize: true },
			// { field: 'approveNote', header: 'Ghi chú duyệt', width: '20rem', cutText: 'b-cut-text-20' },
			// { field: 'cancelNote', header: 'Ghi chú hủy duyệt', width: '20rem', cutText: 'b-cut-text-20' },
			// { field: 'columnResize', header: '', type:'hidden' },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});;

		this._selectedColumns = this.getLocalStorage('approveGan') ?? this.cols;
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {

			const actions = []

			if (item?.status == ApproveConst.STATUS_REQUEST && this.isGranted([this.PermissionGarnerConst.GarnerPDYCTT_PheDuyetOrHuy])) {
				actions.push({
					data: item,
					label: "Phê duyệt",
					icon: "pi pi-check",
					command: ($event) => {
						this.approveOrCancel($event.item.data);
					},
				});
			}

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

	showData(rows) {
		for (let row of rows) {
			row.requestDate = row?.requestDate,
			row.summary = row?.summary,
			row.requestNote = row.requestNote ;
			row.confirmDate = (row.approveDate || row.cancelDate) ? this.formatDateTime(row?.approveDate || row.cancelDate) : null;
		};
	}

	approveOrCancel(item) {
		const ref = this.dialogService.open(
			FormApproveRequestComponent,
			{
				header: 'Phê duyệt yêu cầu tái tục',
				width: '500px',
				contentStyle: { "height": "auto", "overflow": "hidden", "padding-bottom": "50px" },
        		style: { "overflow": "hidden"},
				data: {
					dataType: ApproveConst.STATUS_REINSTATEMENT,
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
    //
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
    //
		this.isLoading = true;
		this.approveService.getAllRequestReinstatement(this.page).subscribe((res) => {
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
