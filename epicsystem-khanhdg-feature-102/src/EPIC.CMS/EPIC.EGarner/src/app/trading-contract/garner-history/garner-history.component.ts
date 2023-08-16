import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BlockageLiberationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DistributionService } from '@shared/services/distribution.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-garner-history',
  templateUrl: './garner-history.component.html',
  styleUrls: ['./garner-history.component.scss']
})
export class GarnerHistoryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
		messageService: MessageService,
		private _orderService: OrderServiceProxy,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private dialogService: DialogService,
		private router: Router,
  ) { 
    super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Giao nhận hợp đồng' },
		]);
  }

  OrderConst = OrderConst;

	rows: any[] = [];

	cols: any[];
	_selectedColumns: any[];

	// data Filter
	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];

  sources: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.sources,
	];
	//
	orderSources: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.orderers,
	];

  dataFilter = {
		id: null,
		policyId: null,
		policyDetailId: null,
		fieldFilter: null,
		source: null,
    	orderSource: null,
		settlementDate: null,
		tradingProviderIds: []
	}

  BlockageLiberationConst = BlockageLiberationConst;
	status: any;
	
	submitted: boolean;
	listAction: any[] = [];

	page = new Page();
	offset = 0;

	order: any = {};

  	// Menu otions thao tác

	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.statusActiveSettlement
	];

  ngOnInit(): void {
	this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
		this.dataFilter.tradingProviderIds = change;
		this.setPage();
	})

    this.init();
		// this.setPage({ page: this.offset });

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

    this.cols = [
			// { field: 'productCode', header: 'Mã dự án', width: '14rem', cutText:'b-cut-text-14', isPin: true },
			{ field: 'contractCode', isSort: true, header: 'Mã HĐ', width: '12rem', cutText:'b-cut-text-12', isPin: true },
			{ field: 'customerName', isSort: true, header: 'Khách hàng', width: '18rem', cutText:'b-cut-text-18', isPin: true },
			{ field: 'investDate', isSort: true, header: 'Ngày tích lũy', width: '12rem' },
			{ field: 'settlementDate', isSort: true, header: 'Ngày tất toán', width: '12rem' },
			{ field: 'policy.name', isSort: true, header: 'Chính sách', width: '12rem', },
			{ field: 'totalValue', isSort: true, header: 'Số tiền tích lũy', width: '13rem'},
			// { field: 'policyDetailName', header: 'Thời hạn', width: '8rem', class: 'text-right justify-content-end'},
			// { field: 'profitDisplay', header: 'Lợi tức', width: '8.5rem', class: 'text-right justify-content-end'},
			{ field: 'source', header: 'Loại', width: '5rem' },
			{ field: 'orderer', header: 'Nguồn', width: '8rem' },
			{ field: 'columnResize', header: '', type:'hidden' },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});

		this._selectedColumns = this.getLocalStorage('garnerHistoryGan') ?? this.cols;

  	}

	setColumn(col, _selectedColumns) {
		console.log('cols:', col);

		console.log('_selectedColumns', _selectedColumns);

		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'deliveryContractGan')
				console.log('Luu o local', this._selectedColumns);
			}
		});
	}

  init() {
		this.isLoading = true;
		forkJoin([this._orderService.getAllGarnerHistory(this.page, this.status, this.dataFilter), this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resSecondary]) => {
		  this.isLoading = false;
		  if (this.handleResponseInterceptor(resOrder, '')) {
			this.page.totalItems = resOrder?.data?.totalItems;
			this.rows = resOrder?.data?.items;
			this.distributions = resSecondary?.data;
			//
			if(this.rows?.length) {
			  this.genListAction(this.rows);
			  this.showData(this.rows)
			 }
			console.log({ rowsOrder: resOrder.data.items, totalItems: resOrder.data.totalItems, resSecondary: resSecondary.data.items });
		  }
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

  showData(rows) {
		for (let row of rows) {
			row.customerName = row?.businessCustomer?.name || (row?.investor?.investorIdentification?.fullname || row?.investor?.name);
			row.productCode = row?.product?.code;
			row.activeDate = this.formatDateTime(row?.activeDate);
			row.totalValue = this.utils.transformMoney(row?.totalValue || row?.initTotalValue);
			row['policy.name'] = row?.policy?.name;
			row.policyDetailName = row?.policyDetail?.name;
			row.profitDisplay = row?.policyDetail?.profit ? this.utils.transformPercent(row?.policyDetail?.profit) + "%/năm" : "";
			row.investDate = this.formatDateTime(row?.investDate);
			row.settlementDate = this.formatDateTime(row?.settlementDate);
		};
		console.log('showData', rows);
	}

  genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			console.log("orderItem?.isWithdrawalRequest",orderItem?.isWithdrawalRequest);
			
			const actions = []
		
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				});	
		

			return actions;
		});
	}

  changeStatus() {
		this.setPage({ Page: this.offset })
	}

  changeDistribution(id) {
		this.policies = [];
		const distribution = this.distributions.find(item => item.id == id);
		this.policies = distribution?.policies ?? [];
		if (this.policies?.length) {
			this.policies = [...this.policies];
		}
		this.setPage();
		console.log(" this.policies", this.policies);
	}


	changePolicy(policyId) {
		this.setPage();
	}

  
	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		//
		this.isLoading = true;
		
		this._orderService.getAllGarnerHistory(this.page, this.status, this.dataFilter, this.sortData).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				//
				if (this.rows?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows);
				}
				console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

  detail(order) {
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id));  
		// this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
		window.open('/trading-contract/order/justview/' + (cryptEncodeId) + '/'+(OrderConst.VIEW_LICH_SU_TICH_LUY), "_blank");

	}
}
