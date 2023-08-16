import { FormApproveRequestComponent } from './../form-approve-request/form-approve-request.component';
import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchConst, ApproveConst, OrderConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column.component';
import { ApproveService } from '@shared/services/approve.service';

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
		private confirmationService: ConfirmationService,
		private routeActive: ActivatedRoute,
		private _router: Router,
		private approveService: ApproveService,
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
	OrderConst = OrderConst;

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
		this.cols = [
			{ field: 'contractCode', header: 'Số hợp đồng', width: '12rem', isPin: true },
			{ field: 'amountMoneyDisplay', header: 'Số tiền rút', width: '15rem', cutText: 'b-cut-text-15', isPin: true },
			{ field: 'totalValue', header: 'Số tiền đầu tư', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'columnResize', header: '', type:'hidden' },
		];
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {
			const actions = []
			if (item?.status == ApproveConst.STATUS_REQUEST && this.isGranted([this.PermissionInvestConst.InvestPDYCRV_PheDuyetOrHuy])) {
				actions.push({
					data: item,
					label: "Xử lý yêu cầu",
					icon: "pi pi-check",
					command: ($event) => {
						this.approveOrCancel($event.item.data);
					},
				});
			}
			// 
			if(this.isGranted([this.PermissionInvestConst.InvestPDYCRV_ChiTietHD])) {
				actions.push({
					data: item,
					label: "Chi tiết HĐ",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail($event.item);
					},
				});
			}

			return actions;
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.requestDate = row?.requestDate,
			row.contractCode = row?.order?.contractCode,
			row.amountMoneyDisplay = row?.amountMoney ? this.utils.transformMoney(row.amountMoney) : null,
			row.confirmDate = row?.approveDate ? this.formatDateTime(row.approveDate) : null;
			row.totalValue = row?.order?.totalValue ? this.formatCurrency(row?.order?.totalValue) : null;
		};
	}

	approveOrCancel(item) {
		const ref = this.dialogService.open(
			FormApproveRequestComponent,
			{
				header: 'Phê duyệt yêu cầu rút vốn',
				width: '500px',
				data: {
					dataType: ApproveConst.STATUS_WITHDRAWAL,
					item: item,
				}
			});

		ref.onClose.subscribe((res) => {
			if (res.status) {
				this.messageSuccess('Thao tác thành công', '');
				this.setPage();
			}
		});
	}

	setPage(event?: Page, dataType?: any) {
		this.isLoading = true;
		this.approveService.getAllRequestWithdrawal(this.page).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				if (this.rows?.length) {
					this.showData(this.rows);
					this.genListAction(this.rows);
				}
			}
		},
			(err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			}
		);
	}

	//
	detail(item) {
		this._router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(item?.data?.id)]);
	}

}
