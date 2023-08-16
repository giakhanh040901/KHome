import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SearchConst, ApproveConst, WithdrawalConst, StatusPaymentBankConst, OrderPaymentConst, WithdrawConst, OrderConst } from "@shared/AppConsts";
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
import { ListRequestBankComponent } from "./list-request-bank/list-request-bank.component";
import { TradingProviderSelectedService } from "@shared/services/trading-provider.service";
import { Subscription } from "rxjs";

@Component({
	selector: 'app-manager-withdraw',
	templateUrl: './manager-withdraw.component.html',
	styleUrls: ['./manager-withdraw.component.scss']
})

export class ManagerWithdrawComponent extends CrudComponentBase {

	// CONST
	StatusPaymentBankConst = StatusPaymentBankConst;
	ApproveConst = ApproveConst;
	WithdrawConst = WithdrawConst;
	OrderConst = OrderConst;
	//
	ref: DynamicDialogRef;

	modalDialogBanks: boolean;
	//
	rows: any[] = [];
	col: any;
	
	cols = [];
	colFixs: any[];
	colDynamics: any[];
	//
	_selectedColumns: any[];
	listAction: any[] = [];
	
	//
	page = new Page();
	offset = 0;

	//
	actions: any[] = []; // list button actions
	actionsDisplay: any[] = [];
	//

	dataFilter = {
		fieldFilter: null
	}

	fieldFilters = {
		fieldSearch: null,
		status: null,
		withdrawalDate: null,
		approveDate: null,
		tradingProviderIds: []
	}

	selectedRequests = [];
	columnApproveInfos = [];
	distributionChecked = [];

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private _withdrawalService: WithdrawalService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private router: Router,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Phê duyệt yêu cầu tái tục" },
		]);
		//
		if (!this.fieldFilters.status) this.fieldFilters.status = ApproveConst.STATUS_REQUEST;
	}

	tradingProviderSub: Subscription;

	ngOnInit(): void {
		//
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.fieldFilters.tradingProviderIds = change;
			this.setPage();
		})
		//
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		this.subject.isSetPage.subscribe(() => {
			this.setPage(this.getPageCurrentInfo());
		});
		//
		this.colFixs = [
			{ field: 'requestWithdrawalDate', isSort: true, header: 'Ngày y/c rút', width: '11rem', isPin: true },
			{ field: 'contractCode', isSort: true, header: 'Chính sách', width: '16rem', isPin: true },
			{ field: 'customer', header: 'Khách hàng', width: '16rem', isPin: true },
			{ field: 'amountMoneyDisplay', isSort: true, fieldSort: 'amountMoney',header: 'Số tiền rút', width: '11rem', cutText: 'b-cut-text-12', isPin: true },
			// { field: 'policyName', header: 'Chính sách', width: '12rem' },
			{ field: 'requestDate', isSort: true, header: 'Ngày tạo', width: '11rem', isPin: true,  },
			{ field: 'validate', header: 'Kiểm tra', width: '15rem', isResize: true },
		];

		this.colDynamics = [
			{ field: 'approveByDisplay', header: 'Người duyệt / Hủy', width: '12rem', isResize: true,},
			{ field: 'approveDateDisplay', isSort: true, header: 'Ngày duyệt / Hủy', width: '14rem'},
		];
		// 
		this.changeStatus();
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	showData(rows) {
		for (let row of rows) {
			row.contractCode = row?.policy?.name;
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

	changeFieldFilter() {
		if (this.keyword?.trim()) {
		  this.setPage();
		}
	}
	
	detailOrder(id){
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(id));  
		window.open('/trading-contract/order/justview/' + (cryptEncodeId) +'/'+ (OrderConst.VIEW_XU_LY_RUT_TIEN), "_blank");
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		//
		this.isLoading = true;
		this._withdrawalService.getAll(this.page, this.fieldFilters, this.sortData).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				if (this.rows?.length) {
					this.showData(this.rows);
				}
			}
		},
			(err) => {
				this.isLoading = false;
			}
		);
	}

	createList() {
		let distributionId = this.selectedRequests[0]?.distributionId;
		if(distributionId) {
			const ref = this.dialogService.open(
				ListRequestBankComponent,
				{
					header: "Gửi yêu cầu chi tiền",
					width: '850px',
					contentStyle: { "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
					style: { "overflow": "auto" },
					data: {
						requests: [...this.selectedRequests],
						interestType: StatusPaymentBankConst.MANAGER_WITHDRAW,
					}
				});
	
			ref.onClose.subscribe((statusResponse) => {
				if(statusResponse) {
					this.setPage();
				}
			});
		} else {
			this.messageError('Chưa có yêu cầu rút vốn nào được chọn!');
		}
	}

	cancelApprove() {
		this.approve(
			StatusPaymentBankConst.CANCEL,
			'Xác nhận hủy duyệt các yêu cầu đã chọn?',
			'Hủy yêu cầu thành công!'
		);
	}

	approveOffline() {
		this.approve(
			StatusPaymentBankConst.APPROVE_OFFLINE,
			'Xác nhận duyệt chi thủ công?',
			'Duyệt chi thành công!'
		);
	}

	approve(statusApprove, messageConfirm, messageNotify) {
		let body = {
			withdrawalIds: this.selectedRequests.map(s => s.id),
			status: statusApprove,
		}
		//
		this.confirmationService.confirm({
			message: messageConfirm,
			icon: 'pi pi-exclamation-triangle',
			header: 'Thông báo',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this.isLoading = true;
				this._withdrawalService.approve(body, StatusPaymentBankConst.MANAGER_WITHDRAW).subscribe((res) => {
					this.isLoading = false;
					if(this.handleResponseInterceptor(res, messageNotify)) {
						this.selectedRequests = [];
						this.setPage();
					}
				}, (err) => {
					this.isLoading = false;
				});			
			}
		});
	}

	changeStatus() {
		this.selectedRequests = [];
		this.distributionChecked = [];
		//
		if(this.fieldFilters.status == StatusPaymentBankConst.PENDING) {
			this.cols = [...this.colFixs];
		} else {
			this.cols = [...this.colFixs,...this.colDynamics];
			
			let index = this.cols.findIndex(c => c.field == 'validate');
			delete this.cols[index];
			this.cols = this.cols.filter(c => c != null);
		}
		
		this.setPage();
	}

	onRowSelect(row) {
		let checkOnlyGroupDistribution = this.selectedRequests.find(s => (s.distributionId == row.distributionId && s.id != row.id));		
		if(!checkOnlyGroupDistribution && this.selectedRequests.length) {
			this.distributionChecked = [];
			setTimeout(() => {
				this.selectedRequests = [];
				this.selectedRequests.push(row);
				this.toggleSelectAll();
			}, 50);
		} else {
			this.toggleSelectAll();
		}
	}

	toggleSelectAll() {
		let selectGroup = this.rows.filter(row => row.distributionId == this.selectedRequests[0]?.distributionId);
		if(selectGroup?.length != this.selectedRequests?.length) {
			this.distributionChecked = [];
		} else {
			this.distributionChecked = [this.selectedRequests[0]?.distributionId];
		}
	}
	
	onGroupSelect(distributionId, event) {
		if(event.checked?.length) {
			this.distributionChecked = [distributionId];
			this.selectedRequests = this.rows.filter(row => row.distributionId == distributionId);
		} else {
			this.selectedRequests = [];
		}
	}

}
