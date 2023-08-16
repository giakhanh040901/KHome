import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DistributionConst, InterestPaymentConst, OrderConst, StatusPaymentBankConst, TableConst } from '@shared/AppConsts';
import { Subscription } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { ListRequestBankComponent } from '../manager-withdraw/list-request-bank/list-request-bank.component';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { InterestPaymentService } from '@shared/services/interest-payment.service';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { OrderInterestFilter } from '@shared/interface/filter.model';

@Component({
	selector: 'app-interest-contract',
	templateUrl: './interest-contract.component.html',
	styleUrls: ['./interest-contract.component.scss']
})
export class InterestContractComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _interestPaymentService: InterestPaymentService,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private _withdrawalService: WithdrawalService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Chi trả lợi tức' },
		]);
	}

	OrderConst = OrderConst;
	InterestPaymentConst = InterestPaymentConst;
	StatusPaymentBankConst = StatusPaymentBankConst; 
	DistributionConst = DistributionConst;
	//
	rows: any[] = [];
	columns: IColumn[];
	listAction: any[] = [];
	page = new Page()

	dataFilter: OrderInterestFilter = new OrderInterestFilter();
	dataTableEmit: DataTableEmit = new DataTableEmit();
	tradingProviderSub: Subscription;

	createdList: boolean;
	loadingStep: number = InterestPaymentConst.STEP_START;

	order: any = {};
	// Menu otions thao tác

	issetFutureDate: boolean = false;
	isPartner: boolean;

	statusPaymentSuccess: number[] = [
		InterestPaymentConst.STATUS_DONE, 
		InterestPaymentConst.STATUS_DONE_OFFLINE, 
		InterestPaymentConst.STATUS_DONE_ONLINE
	];

	linkForStepPayment = {
		[InterestPaymentConst.STATUS_DUEDATE]: {
			'api' : '/api/invest/order/lap-danh-sach-chi-tra?',
			'apiExport' : '/api/invest/export-excel-report/list-interest-payment-due?',
			'loadingStep': 0,
		},
		[InterestPaymentConst.STATUS_CREATED_LIST]: {
			'api': '/api/invest/interest-payment/find?',
			'apiExport' : '/api/invest/export-excel-report/list-interest-payment-paid?',
			'loadingStep': 50
		},
		[InterestPaymentConst.STATUS_DONE]: {
			'api': '/api/invest/interest-payment/find?',
			'apiExport' : '/api/invest/export-excel-report/list-interest-payment-paid?',
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_ONLINE]: {
			'api': '/api/invest/interest-payment/find?',
			'apiExport' : '/api/invest/export-excel-report/list-interest-payment-paid?',
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_OFFLINE]: {
			'api': '/api/invest/interest-payment/find?',
			'apiExport' : '/api/invest/export-excel-report/list-interest-payment-paid?',
			'loadingStep': 100,
		},
	}

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this.getFilterFromSessionStorage();
		this.tradingProviderSub = this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change: number[]= []) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		});
		this.columns = this.getColumns();
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	setPage(event?: Page, isUpdateColumn: boolean = false) {

		console.log('dataFilter', this.dataFilter);
		
      	this.createdList = false;
		this.isLoading = true;
		
		if(!event) {
			this.dataTableEmit.selectedItems = [];
			this.page.pageNumber = 0;
		}

		this.setSessionStorage(this.dataFilter, this.keyStorage);
		this._interestPaymentService.getAllContractInterest(this.page, this.dataFilter, this.linkForStepPayment[this.dataFilter.status].api).subscribe((res) => {
			if(isUpdateColumn) this.columns = this.getColumns();
			this.isLoading = false;
			this.issetFutureDate = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				const resOrder = res?.data?.items;
				if(this.page.pageSize === this.page.pageSizeAll) {
					// LOAD MORE DATA
					if(this.page.pageNumberLoadMore === 1) this.rows = [];
					this.rows = [...this.rows, ...resOrder];
				} else {
					this.rows = resOrder;
				}
				let startIndex = this.page.pageNumber*this.page.pageSize;
				this.rows = resOrder.map((item, index) => {
					const rowIndex = this.page.pageSize === this.page.pageSizeAll ? index : ++startIndex;
					item.index = rowIndex;
					item.isFutureDate = new Date(new Date(item.payDate).setHours(0,0,0,0)).getTime() > new Date(new Date().setHours(0,0,0,0)).getTime();
					if(item.isFutureDate) {
						this.issetFutureDate = true;
					}
					return item;
				});
				console.log('rows', this.rows);
					//
				if (res.data?.items?.length) {
					this.genListAction(this.rows);
					this.setData(this.rows);
				} 
			}
		});
	}

	setData(rows) {
		for (let row of rows) {
			row.invCode = row?.invCode,
			row.policyDetailName = row?.policyDetailName,
			row.amountMoney = row?.amountMoney || row?.actuallyProfit,
			row.bankAccount = row?.investorBank?.bankAccount;
			row.bankName = row?.investorBank?.bankName;
			row.contractCode = row?.genContractCode || row?.contractCode;
			//
			row.methodInterestElement = DistributionConst.getMethodInterestInfo(row.methodInterest);
			row.statusBankElement = StatusPaymentBankConst.getResponseInfo(row.statusBank);
		};
	}

	getColumns() {
		this.columns = [];
		let columns: IColumn[] = [];
		let columnContents = [
			{ field: '', header: '', width: 3, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, type: TableConst.columnTypes.CHECKBOX_ACTION },
			{ field: 'orderId', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'contractCode', header: 'Mã hợp đồng', width: 10.5, isPin: true, isResize: true, isSort: true },
			{ field: 'name', header: 'Khách hàng', width: 18, isPin: true, isSort: true },
			{ field: 'invCode', header: 'Mã dự án', width: 14, isResize: true, isSort: true },
			{ field: 'policyDetailName', header: 'Kỳ hạn', width: 7, class: 'justify-content-end text-right', isSort: true },
			{ field: 'totalValue', header: 'Tiền đầu tư', width: 10, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'tax', header: 'Thuế', width: 8, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'amountMoney', header: 'Lợi tức', width: 9, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'payDate', header: 'Ngày chi trả', width: 10, type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'bankAccount', header: 'TK thụ hưởng', width: 12},
			{ field: 'bankName', header: 'Ngân hàng thụ hưởng', width: 14},
		];

		let columnFooterInit = [
			{ field: 'methodInterest', header: 'Loại chi', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];

		let columnFooterChange = [
			{ field: 'methodInterest', header: 'Loại chi', width: 8.5, type: TableConst.columnTypes.STATUS },
			{ field: 'statusBank', header: 'Kết quả chi', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];

		if(this.statusPaymentSuccess.includes(this.dataFilter.status)) {
			columnContents[0].type = TableConst.columnTypes.CHECKBOX_SHOW;
			columns =  [...columnContents,...columnFooterChange];
		} else {
			columns = [...columnContents,...columnFooterInit];
		}

		return columns;
	}
	
	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
			if(this.isGranted([this.PermissionInvestConst.InvestHDPP_CTLT_ThongTinDauTu])) {
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				});
			}
			return actions;
		});
	}

	changeStatusInterest(status) {
		this.loadingStep = this.linkForStepPayment[status].loadingStep;
		this.setPage(null, true);
	}

	createList() {
		const body = { interestPayments: [] };
		body.interestPayments = [...this.dataTableEmit.selectedItems];
		this.isLoading = true;
		this._interestPaymentService.addListInterest(body).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.dataFilter.status = InterestPaymentConst.STATUS_CREATED_LIST;
				this.loadingStep = this.linkForStepPayment[this.dataFilter.status].loadingStep;
				this.setPage();
			}
		});
	}
	
	approveOnline() {
		let distributionId = this.dataTableEmit.selectedItems[0]?.distributionId;
		if(distributionId) {
			this.dialogService.open(
				ListRequestBankComponent,
				{
					header: "Gửi yêu cầu chi tiền",
					width: '850px',
					data: {
						requests: [...this.dataTableEmit.selectedItems],
						interestType: StatusPaymentBankConst.INTEREST_CONTRACT,
						distributionId: distributionId,
					}
				}
			).onClose.subscribe((statusResponse) => {
				if(statusResponse) {
					this.dataTableEmit.selectedItems = [];
					this.setPage();
				}
			});
		} else {
			this.messageError('Chưa có yêu cầu rút vốn nào được chọn!');
		}
	}

	// DUYỆT CHI THỦ CÔNG
	approveOffline() {
		let body = {
			"interestPaymentIds": [],
			"status": StatusPaymentBankConst.APPROVE_OFFLINE,
		}

    	body.interestPaymentIds = [];
		body.interestPaymentIds = this.dataTableEmit.selectedItems.map(s => s.id);

		this.isLoading = true;
		this.submitted = true;
		this._withdrawalService.approve(body, StatusPaymentBankConst.INTEREST_CONTRACT).subscribe((res) => {
			this.isLoading = false;
			this.submitted = false;
			if(this.handleResponseInterceptor(res, 'Duyệt chi thành công!')) {
				this.dataFilter.status = InterestPaymentConst.STATUS_DONE;
				this.loadingStep = InterestPaymentConst.STEP_END;
				this.setPage();
			} 
		}, (err) => {
			console.log('err__', err);
			this.isLoading = false;
			this.submitted = true;
		});
	}

	detail(order) {
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.orderId)); 
		window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
	}

	exportExcel() {
		this.isLoading = true;
		this._withdrawalService.exportInterestContract(this.page, this.dataFilter, this.linkForStepPayment[this.dataFilter.status].apiExport).subscribe((res) => {
			this.isLoading = false;
			this.handleResponseInterceptor(res);
		});
	}

	public get keyStorage() {
		return 'interestContractFilterParams';
	}

	private getFilterFromSessionStorage() {
		const sessionStorageFilter = this.getSessionStorage(this.keyStorage);
		if (sessionStorageFilter) {
			this.dataFilter = {
				...sessionStorageFilter, 
				ngayChiTra: sessionStorageFilter.ngayChiTra && new Date(sessionStorageFilter.ngayChiTra),
				keyword: '',
			};
			//
			this.loadingStep = this.linkForStepPayment[this.dataFilter.status].loadingStep;
		}
	}

	public refreshFilter(event: any) {
		if (event) {
			this.removeSessionStorage(this.keyStorage);
			this.dataFilter = new OrderInterestFilter();
			this.page = new Page();
			this.setPage();
		}
	}

	onRowSelect() {
		// Kiểm tra các phần tử được chọn phải cùng distributionId, Nếu khác distribution thì sẽ lấy theo phần tử đc chọn sau cùng
		if(this.dataFilter.methodInterest === DistributionConst.CO_CHI_TIEN && this.dataFilter.status === InterestPaymentConst.STATUS_CREATED_LIST) {
			let selectedLength = this.dataTableEmit.selectedItems.length;
			if(selectedLength > 1) {
				let rowLast = this.dataTableEmit.selectedItems[selectedLength - 1];
				let groupDistributionForItemLast = this.dataTableEmit.selectedItems.filter(s => s.distributionId === rowLast.distributionId);
				let isExitsDistributionOther = selectedLength === groupDistributionForItemLast.length;  
				if(!isExitsDistributionOther) {
					setTimeout(() => this.dataTableEmit.selectedItems = [...groupDistributionForItemLast]);
				}
			}
		}
	}

}
