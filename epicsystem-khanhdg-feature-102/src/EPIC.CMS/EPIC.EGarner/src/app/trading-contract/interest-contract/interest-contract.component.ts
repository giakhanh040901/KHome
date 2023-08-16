import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { InterestPaymentServiceProxy, OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { InterestPaymentConst, OrderConst, ProductConst, SearchConst, StatusPaymentBankConst, WithdrawConst, YesNoConst } from '@shared/AppConsts';
import { forkJoin, Subject, Subscription } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { ListRequestBankComponent } from '../manager-withdraw/list-request-bank/list-request-bank.component';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { DistributionService } from '@shared/services/distribution.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';

@Component({
	selector: 'app-interest-contract',
	templateUrl: './interest-contract.component.html',
	styleUrls: ['./interest-contract.component.scss']
})
export class InterestContractComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderServiceProxy,
		private _interestPaymentService: InterestPaymentServiceProxy,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private dialogService: DialogService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private _withdrawalService: WithdrawalService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Hợp đồng' },
		]);
	}

	modalDialog: boolean;

	OrderConst = OrderConst;
	InterestPaymentConst = InterestPaymentConst;
	StatusPaymentBankConst = StatusPaymentBankConst; 
	WithdrawConst = WithdrawConst;
	rows: any[] = [];
	row: any;
	col: any;
	dataFilter = {
		fieldFilter: null
	}
	cols: any[];
	_selectedColumns: any[];

	// data Filter
	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];

	fieldFilters = {
		id: null,
		policy: null,
		policyDetailId: null,
		searchField: null,
		ngayChiTra: new Date(),
		status: InterestPaymentConst.STATUS_DUEDATE, 
		isExactDate: YesNoConst.YES,
		isLastPeriod: YesNoConst.NO,
		tradingProviderIds: []
	}

	selectedContracts = [];
	createdList: boolean;
	loadingStep: number = 0;

	linkForStepPayment = {
		[InterestPaymentConst.STATUS_DUEDATE]: {
			'api' : '/api/garner/interest-payment/find-all-interest-payment?',
			'loadingStep': 0,
		},
		[InterestPaymentConst.STATUS_CREATED_LIST]: {
			'api': '/api/garner/interest-payment/find-all?',
			'loadingStep': 50
		},
		[InterestPaymentConst.STATUS_DONE]: {
			'api': '/api/garner/interest-payment/find-all?',
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_ONLINE]: {
			'api': '/api/garner/interest-payment/find-all?',
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_OFFLINE]: {
			'api': '/api/garner/interest-payment/find-all?',
			'loadingStep': 100,
		},
	}

	status: any;

	submitted: boolean;
	listAction: any[] = [];

	page = new Page();
	offset = 0;

	order: any = {};
	ProductConst = ProductConst;

	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.statusActive
	];

	interestPaymentItem = {
		"orderId": null,
		"periodIndex": null,
		"cifCode": null,
		"amountMoney": null,
		"policyDetailId": null,
		"payDate": null,
		"isLastPeriod": null,
	}
	colFixs: any[];
	colDynamics: any[];
	issetFutureDate: boolean = false;
	distributionChecked = [];
	tradingProviderSub: Subscription;

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.fieldFilters.tradingProviderIds = change;
			this.setPage();
		})

		this.getFilterFromSessionStorage();
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword.trim()) {
				this.setPage({ page: this.offset });
			}
		});

		this.subject.isSetPage.subscribe(() => {
			this.setPage(this.getPageCurrentInfo());
		});

		this.colFixs = [
			{ field: 'name', header: 'Khách hàng', width: '18rem', isPin: true },
			{ field: 'productTypeGet', header: 'Loại tích lũy', width: '12rem', isPin: true },
			{ field: 'policyGet', header: 'Chính sách', width: '12rem', isPin: true },
			{ field: 'allTotalValue', header: 'Tích lũy', width: '10rem', cutText: 'b-cut-text-12', isPin: true },
			{ field: 'amountMoneyDisplay', fieldSort: 'amountMoney', header: 'Lợi tức thực nhận', width: '12rem' },
			{ field: 'tax', header: 'Thuế TNCN', width: '15rem', isResize: true },
		];

		this.colDynamics = [
			// { field: 'approveByDisplay', header: 'Người duyệt / Hủy', width: '12rem', isResize: true,},
			// { field: 'approveDateDisplay', header: 'Ngày duyệt / Hủy', width: '11rem'},
		];
		
		this.changeStatus();
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	toggleSelectAll() {
		let selectGroup = this.rows.filter(row => row.distributionId == this.selectedContracts[0]?.distributionId);
		if(selectGroup?.length != this.selectedContracts?.length) {
			this.distributionChecked = [];
		} else {
			this.distributionChecked = [this.selectedContracts[0]?.distributionId];
		}
	}

	
	detailOrder(id){
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(id));  
		window.open('/trading-contract/order/justview/' + (cryptEncodeId) +'/'+ (OrderConst.VIEW_XU_LY_RUT_TIEN), "_blank");
	}

	onRowSelect(row) {
		let checkOnlyGroupDistribution = this.selectedContracts.find(s => (s.distributionId == row.distributionId && s.id != row.id));
		
		if(!checkOnlyGroupDistribution && this.selectedContracts.length) {
			this.distributionChecked = [];
			setTimeout(() => {
				this.selectedContracts = [];
				this.selectedContracts.push(row);
				this.toggleSelectAll();
			}, 50);
		} else {
			this.toggleSelectAll();
		}
	}

	onGroupSelect(distributionId, event) {
		
		if(event.checked?.length) {
			this.distributionChecked = [distributionId];
			this.selectedContracts = this.rows.filter(row => row.distributionId == distributionId);
		} else {
			this.selectedContracts = [];
		}
	}

	changeStatus() {
		this.selectedContracts = [];
		this.distributionChecked = [];
		
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

	showData(rows) {
		for (let row of rows) {
			row.productTypeGet = ProductConst.getTypeName(row?.product?.productType),
			row.policyGet = row?.policy?.name,
			row.code = row?.product?.code,
			row.name = row?.investor?.name || row?.investor?.investorIdentification?.fullname,
			row.policyDetailName = row?.policyDetailName,
			row.amountMoneyDisplay = this.utils.transformPercent(row?.amountMoney || row?.actuallyProfit),
			row.totalValueDisplay = this.utils.transformPercent(row?.allTotalValue);
			row.taxDisplay = this.utils.transformPercent(row?.tax);
			row.totalValueInvestmentDisplay = row?.totalValueInvestment ? this.utils.transformPercent(row?.totalValueInvestment) : null;
			row.payDateDisplay = this.formatDate(row?.payDate);
			row.bankAccount = row?.investorBank?.bankAccount;
			row.bankName = row?.investorBank?.bankName;
		};

	}


	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
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
				this.setLocalStorage(this._selectedColumns, 'interestContractGarner')
			}
		});
	}

	init() {
		this.isLoading = true;
		forkJoin([this._orderService.getAll(this.page, 'orderContract', this.status, this.fieldFilters), this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resSecondary]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resOrder, '')) {
				this.page.totalItems = resOrder?.data?.totalItems;
				this.rows = resOrder?.data?.items;
				this.distributions = resSecondary?.data;
				this.distributions = [...[{ invName: 'Tất cả', id: '' }], ...this.distributions];
				if (this.rows?.length) {
					this.showData(this.rows)
				}
			}
		});
	}

	changeFieldFilter() {
		if (this.keyword?.trim()) {
			this.setPage();
		}
	}

	setPage(pageInfo?: any) {
		this.selectedContracts = [];
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.fieldFilters.ngayChiTra && (this.fieldFilters.ngayChiTra = new Date(this.fieldFilters.ngayChiTra));
		
      	this.createdList = false;
		this.isLoading = true;

		this.setFilterSessionStorage();
		this._interestPaymentService.getAllContractInterest(this.page, this.fieldFilters, this.linkForStepPayment[this.fieldFilters.status].api, this.sortData).subscribe((res) => {
			this.isLoading = false;
			this.issetFutureDate = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.rows = res.data?.items;
				this.page.totalItems = res.data.totalItems;
				if (res.data?.items?.length) {
					this.rows = res.data.items.map((item, index) => {
						item.index = index + 1;
						item.distributionId = item.policy.distributionId
						item.isFutureDate = new Date(new Date(item.payDate).setHours(0,0,0,0)).getTime() > new Date(new Date().setHours(0,0,0,0)).getTime();
						if(item.isFutureDate) {
							this.issetFutureDate = true;
						}
						return item;
					});
					//
					this.showData(this.rows);
				} 
			}
		});
	}

	changeStatusInterest(status) {
		this.loadingStep = this.linkForStepPayment[status].loadingStep;
		this.setPage();
	}

	createList() {
		const body = [...this.selectedContracts];
		this.isLoading = true;
		this._interestPaymentService.addListInterest(body).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.loadingStep = 50;
				this.fieldFilters.status = InterestPaymentConst.STATUS_CREATED_LIST;
				this.setPage();
			}
		});
	}
	
	approveOnline() {		
		let id = this.selectedContracts[0]?.id;
		if(id) {
			const ref = this.dialogService.open(
				ListRequestBankComponent,
				{
					header: "Gửi yêu cầu chi tiền",
					width: '850px',
					contentStyle: { "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
					style: { "overflow": "auto" },
					data: {
						requests: [...this.selectedContracts],
						interestType: StatusPaymentBankConst.INTEREST_CONTRACT
					}
				});
	
			ref.onClose.subscribe((statusResponse) => {
				if(statusResponse) {
					this.selectedContracts = [];
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
		body.interestPaymentIds = this.selectedContracts.map(s => s.id);
		
		this.isLoading = true;
		this.submitted = true;
		this._withdrawalService.approveInterestContract(body, StatusPaymentBankConst.INTEREST_CONTRACT).subscribe((res) => {
			this.isLoading = false;
			this.submitted = false;
			if(this.handleResponseInterceptor(res, 'Duyệt chi thành công!')) {
				this.selectedContracts = [];
				this.fieldFilters.status = InterestPaymentConst.STATUS_DONE;
				this.loadingStep = 100;
				this.setPage();
			} 
		}, (err) => {
			this.isLoading = false;
			this.submitted = true;
		});
	}

	detail(order) {
		// this.router.navigate(['/trading-contract/interest-contract/detail/' + this.cryptEncode(order?.orderId)]);
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.orderId)); 
		window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
	}

	exportExcel() {
		// this.page.keyword = this.keyword;
		// this.isLoading = true;
		// //
		// this._withdrawalService.exportInterestCreateList(this.page, this.fieldFilters).subscribe((res) => {
		// 	this.isLoading = false;
		// 	if (this.handleResponseInterceptor(res, '')) {
				
		// 	}
		// });
	}

	public get keyStorage() {
		return 'interestContractGarner';
	}

	private getFilterFromSessionStorage() {
		const sessionStorage = this.getLocalStorage(this.keyStorage);
		if (sessionStorage && sessionStorage.length) {
			sessionStorage.forEach((data: any) => {
				this[data.key] = data.value;
			})
			if (this.fieldFilters.status || this.fieldFilters.status === 0) {
				this.loadingStep = this.linkForStepPayment[this.fieldFilters.status].loadingStep;
			}
		}
	}

	private setFilterSessionStorage() {
		let result: any[] = [];
		result = [
			{
				key: 'keyword',
				value: this.keyword,
			},
			{
				key: 'fieldFilters',
				value: this.fieldFilters,
			},
		]
		this.setLocalStorage(result, this.keyStorage);
	}

	public refreshFilter(event: any) {
		if (event) {
			this.removeSessionStorage(this.keyStorage);
			this.keyword = "";
			this.fieldFilters = {
				id: null,
				policy: null,
				policyDetailId: null,
				searchField: null,
				ngayChiTra: new Date(),
				status: InterestPaymentConst.STATUS_DUEDATE, 
				isExactDate: YesNoConst.YES,
				isLastPeriod: YesNoConst.NO,
				tradingProviderIds: this.fieldFilters.tradingProviderIds
			}
			this.loadingStep = this.linkForStepPayment[this.fieldFilters.status].loadingStep;
			this.setPage({ page: this.offset });
		}
	}

}
