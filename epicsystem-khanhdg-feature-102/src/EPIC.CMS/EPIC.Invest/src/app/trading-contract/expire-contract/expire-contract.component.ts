import { DistributionConst, PolicyTemplateConst, StatusPaymentBankConst, TableConst, YesNoConst } from './../../../shared/AppConsts';
import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { InterestPaymentConst, OrderConst } from '@shared/AppConsts';
import { forkJoin, Subject, Subscription } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { DistributionService } from '@shared/services/distribution.service';
import { ListRequestBankComponent } from '../manager-withdraw/list-request-bank/list-request-bank.component';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { InterestPaymentService } from '@shared/services/interest-payment.service';
import { ProjectServiceProxy } from '@shared/services/project-manager-service';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { OrderExpireFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-expire-contract',
  templateUrl: './expire-contract.component.html',
  styleUrls: ['./expire-contract.component.scss']
})
export class ExpireContractComponent extends CrudComponentBase {

  constructor (
		injector: Injector,
		messageService: MessageService,
		private _interestPaymentService: InterestPaymentService,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private _withdrawalService: WithdrawalService,
		private _projectService: ProjectServiceProxy,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Hợp đồng đáo hạn' },
		]);
	}

	OrderConst = OrderConst;
	InterestPaymentConst = InterestPaymentConst;
	YesNoConst = YesNoConst;
	StatusPaymentBankConst = StatusPaymentBankConst;
	PolicyTemplateConst = PolicyTemplateConst;
	DistributionConst = DistributionConst;

	rows: any[] = [];
	columns: IColumn[] = [];

	distributions: any[] = [];
	projects = [];

	createdList: boolean;
	loadingStep: number = 0;

	submitted: boolean;
	listAction: IAction[][] = [];

	page = new Page();
	order: any = {};

	issetFutureDate: boolean = false;

	apiGeneral = {
		'api': '/api/invest/interest-payment/find?',
		'apiExport': '/api/invest/export-excel-report/list-interest-payment-paid?',
	}

	linkForStepPayment = {
		[InterestPaymentConst.STATUS_DUEDATE]: {
			'api' : '/api/invest/order/lap-danh-sach-chi-tra-cuoi-ky?',
			'apiExport': '/api/invest/export-excel-report/list-last-interest-payment-due?',
			'loadingStep': 0,
		},
		[InterestPaymentConst.STATUS_CREATED_LIST]: {
			...this.apiGeneral,
			'loadingStep': 50
		},
		[InterestPaymentConst.STATUS_DONE]: {
			...this.apiGeneral,
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_ONLINE]: {
			...this.apiGeneral,
			'loadingStep': 100,
		},
		[InterestPaymentConst.STATUS_DONE_OFFLINE]: {
			...this.apiGeneral,
			'loadingStep': 100,
		},
	}

	public get keyStorage() {
		return 'expireContractCacheFilter';
	}

	dataTableEmit: DataTableEmit = new DataTableEmit();
	dataFilter: OrderExpireFilter = new OrderExpireFilter();
	tradingProviderSub: Subscription;
	isInit: boolean = true;

	ngOnInit() {
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})
		this.subject.isSetPage.subscribe(() => {
			this.setPage(this.getPageCurrentInfo());
		});
		this.getFilterFromSessionStorage();
		this.columns = this.getColumns();
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	getColumns(changeColumn: boolean = false) {
		this.columns = [];
		let columns: IColumn[] = [];
		let columnContents = [
			{ field: '', header: '', width: 3, isPin: true, displaySettingColumn: false, type: TableConst.columnTypes.CHECKBOX_ACTION, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'orderId', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'contractCode', header: 'Mã hợp đồng', width: 10.5, isPin: true, isResize: true, isSort: true },
			{ field: 'name', header: 'Khách hàng', width: 18, isPin: true, isSort: true },
			{ field: 'invCode', header: 'Mã dự án', width: 14, isSort: true },
			{ field: 'policyDetailName', header: 'Kỳ hạn', width: 8, class: 'justify-content-end text-right', isSort: true },
			{ field: 'totalValue', header: 'Tiền đầu tư', width: 10, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'tax', header: 'Thuế', width: 8, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'amountMoney', header: 'Lợi tức', width: 8, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'payDate', header: 'Ngày chi trả', width: 10, type: TableConst.columnTypes.DATE, isSort: true },
			{ field: 'settlementMethod', header: 'Loại hình', width: 11, type: TableConst.columnTypes.CONVERT_DISPLAY, },
			{ field: 'totalExpire', header: 'Nhận cuối kỳ', width: 10, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'calculateType', header: 'Cách tính', width: 7 },
			{ field: 'bankAccount', header: 'TK thụ hưởng', width: 12 },
			{ field: 'bankName', header: 'Ngân hàng thụ hưởng', width: 14 },
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

		if(changeColumn) {
			columnContents[0].type = TableConst.columnTypes.CHECKBOX_SHOW;
			columns =  [...columnContents,...columnFooterChange];
		} else {
			columns = [...columnContents,...columnFooterInit];
		}

		return columns;
	}

	changeStatusInterest(status) {
		this.loadingStep = this.linkForStepPayment[status].loadingStep;
		this.setPage(null, true);
	}

	updateColumnAtribution() {
		this.columns = this.getColumns(this.loadingStep === InterestPaymentConst.STEP_END);
	}

	showData(rows) {
		for (let row of rows) {
			row.invCode = row?.invCode,
			row.policyDetailName = row?.policyDetailName,
			row.totalValue = row?.totalValue || row?.totalValueInvestment;
			// TÍNH LỢI NHUẬN
			row.amountMoney = this.dataFilter.status < InterestPaymentConst.STATUS_DONE ? +row?.amountMoney : +row?.profit;
			// TÍNH SỐ TIỀN TẤT TOÁN
			row.totalExpire = this.dataFilter.status < InterestPaymentConst.STATUS_DONE 
							? (row?.renewalsRequest?.settlementMethod == InterestPaymentConst.EXPIRE_RENEWAL_ORIGINAL  
								? +row?.amountMoney // Tái tục gốc tiền tất toán == lợi nhuận || Tái tục gốc + LN tiền tất toán = 0 || Không tái tục tiền tất toán = Tiền gốc + LN
								: (row?.renewalsRequest?.settlementMethod == InterestPaymentConst.EXPIRE_RENEWAL_PROFIT ? 0 : (+row.totalValue + +row?.amountMoney))
								) 
							:  +row?.amountMoney;
			// LOẠI HÌNH
			row.settlementMethodDisplay = InterestPaymentConst.getTypeRenewal(row?.renewalsRequest?.settlementMethod);
			row.calculateType = PolicyTemplateConst.getCalculateType(row?.policyCalculateType);
			row.bankAccount = row?.investorBank?.bankAccount;
			row.bankName = row?.investorBank?.bankName;
			row.contractCode = row?.genContractCode || row?.contractCode;
			// Tag StatusElement
			row.methodInterestElement = DistributionConst.getMethodInterestInfo(row.methodInterest);
			row.statusBankElement = StatusPaymentBankConst.getResponseInfo(row.statusBank);
		};
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
			//
			if(this.isGranted([this.PermissionInvestConst.InvestHDPP_HDDH_ThongTinDauTu])) {
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
	 
	setPage(event?: any, isUpdateColumn: boolean = false) {
		if(!event) this.page.pageNumber = 0;
		this.dataTableEmit.selectedItems = [];
      	this.createdList = false;
		let apiRequests = [this._interestPaymentService.getAllContractInterest(this.page, this.dataFilter, this.linkForStepPayment[this.dataFilter.status].api)];
		if(this.isInit) {
			apiRequests[1] = this._distributionService.getDistributionsOrder();
			apiRequests[2] = this._projectService.getAllNoPaging();
			this.isInit = false;
		}
		//
		this.setSessionStorage(this.dataFilter, this.keyStorage);
		this.isLoading = true;
		forkJoin(apiRequests).subscribe(res => {
			if(isUpdateColumn) {
				this.updateColumnAtribution();
			}
			this.isLoading = false;
			this.handleData(res[0], res[1], res[2]);
		});
	}

	handleData(resOrder, resDistribution, resProject) {
		if(resOrder && this.handleResponseInterceptor(resOrder)) {
			this.page.totalItems = resOrder.data?.totalItems;
			const rows = resOrder?.data?.items;
			if(this.page.pageSize === this.page.pageSizeAll) {
				// LOAD MORE DATA
				if(this.page.pageNumberLoadMore === 1) this.rows = [];
				this.rows = [...this.rows, ...rows];
			} else {
				this.rows = rows;
			}
			let startIndex = this.page.pageNumber*this.page.pageSize;
			this.rows = rows.map((item, index) => {
				const rowIndex = this.page.pageSize === this.page.pageSizeAll ? index : ++startIndex;
				return { ...item, index: rowIndex }
			});
			if (resOrder.data?.items?.length) {
				this.genListAction(this.rows);
				this.showData(this.rows);
			} 
		}

		if(resDistribution && this.handleResponseInterceptor(resDistribution)) {
			this.distributions = resDistribution?.data;
		}

		if(resProject && this.handleResponseInterceptor(resProject)) {
			this.distributions = resProject?.data;
		}

	}

	// DUYỆT TÁI TỤC ONLINE
	approveRenewalOnline() {
		this.approveOnline(StatusPaymentBankConst.RENEWAL_CONTRACT)
	}

	// DUYỆT ĐÁO HẠN ONLINE
	approveExpireOnline() {
		this.approveOnline(StatusPaymentBankConst.INTEREST_CONTRACT)
	}
	
	// DUYỆT CHI TỰ ĐỘNG
	approveOnline(interestType) {	
		this.dialogService.open(
			ListRequestBankComponent,
			{
				header: "Gửi yêu cầu chi tiền",
				width: '850px',
				data: {
					requests: [...this.dataTableEmit.selectedItems],
					interestType: interestType,
					typeContract: 'DaoHanHopDong',
					distributionId: this.dataTableEmit.selectedItems[0]?.distributionId,
				}
			}).onClose.subscribe((statusResponse) => {
				if(statusResponse) {
					this.setPage();
				}
			}
		);
	}

	// DUYỆT TÁI TỤC OFFLINE
	approveRenewalOffline() {
		this.approveOffline(StatusPaymentBankConst.RENEWAL_CONTRACT)
	}

	// DUYỆT ĐÁO HẠN OFFLINE
	approveExpireOffline() {
		this.approveOffline(StatusPaymentBankConst.INTEREST_CONTRACT)
	}

	// DUYỆT CHI THỦ CÔNG
	approveOffline(interestType) {
		let body = {
			"interestPaymentIds": [],
			"status": StatusPaymentBankConst.APPROVE_OFFLINE,
		}

    	body.interestPaymentIds = [];
		body.interestPaymentIds = this.dataTableEmit.selectedItems.map(s => s.id);

		this.isLoading = true;
		this.submitted = true;
		this._withdrawalService.approve(body, interestType).subscribe((res) => {
			this.isLoading = false;
			this.submitted = false;
			if(this.handleResponseInterceptor(res, 'Duyệt thành công!')) {
				this.dataTableEmit.selectedItems = [];
				this.dataFilter.status = InterestPaymentConst.STATUS_DONE;
				this.loadingStep = 100;
				this.setPage();
			} 
		}, (err) => {
			console.log('err__', err);
			this.isLoading = false;
			this.submitted = true;
		});
	}

	createList() {
		const body = { interestPayments: [] };
		body.interestPayments = [...this.dataTableEmit.selectedItems];
		console.log('body____', body);
		this.isLoading = true;
		this._interestPaymentService.addListInterest(body).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.loadingStep = 50;
				this.dataFilter.status = InterestPaymentConst.STATUS_CREATED_LIST;
				this.setPage();
			}
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

	private getFilterFromSessionStorage() {
		const sessionStorage = this.getSessionStorage(this.keyStorage);
		if (sessionStorage) {
			this.dataFilter = {
				...sessionStorage, 
				ngayChiTra: sessionStorage.ngayChiTra ? new Date(sessionStorage.ngayChiTra) : null,
				keyword: '',
			};
		}
	}

	public refreshFilter(event: any) {
		if (event) {
			this.removeSessionStorage(this.keyStorage);
			this.loadingStep = this.linkForStepPayment[this.dataFilter.status].loadingStep;
			this.dataFilter = new OrderExpireFilter();
			this.page = new Page();
			this.setPage();
		}
	}

	onRowSelect() {
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
