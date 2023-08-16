import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { OrderConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { DistributionService } from '@shared/services/distribution.service';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';

@Component({
	selector: 'app-contract-processing',
	templateUrl: './contract-processing.component.html',
	styleUrls: ['./contract-processing.component.scss']
})
export class ContractProcessingComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderServiceProxy,
		private _distributionService: DistributionService,
		private dialogService: DialogService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		// private _distributionservice: ProductdistributionserviceProxy,
		private router: Router,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Xử lý hợp đồng' },
		]);
	}

	OrderConst = OrderConst;

	rows: any[] = [];
	row: any;
	col: any;

	cols: any[];
	_selectedColumns: any[];

	// data Filter
	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];
	dataFilter = {
		distributionId: null,
		policyId: null,
		policyDetailId: null,
		customerName: '',
		contractCode: '',
		tradingDate: null,
		source: null,
        orderSource: null,
		tradingProviderIds: []
	}
	status: any;
	tradingDate: null;
	customerName = '';
	contractCode = '';

	submitted: boolean;
	listAction: any[] = [];

	page = new Page();
	offset = 0;

	order: any = {};
	// Menu otions thao tác
	// Menu otions thao tác
	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.statusProcessing
	];
	//
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

	ngOnInit() {
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		});
		this.init();
		this.setPage({ page: this.offset });
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
			{ field: 'contractCode', isSort: true, header: 'Mã HĐ', width: '10rem' , isPin: true, cutText: 'b-cut-text-10' },
			{ field: 'customerName', isSort: true, header: 'Khách hàng', width: '18rem', cutText:'b-cut-text-18', isPin: true  },
			{ field: 'buyDate', isSort: true, header: 'Ngày đặt lệnh', width: '11rem'},
			{ field: 'policy.name', isSort: true, header: 'Chính sách', width: '12rem' },
			// { field: 'profitDisplay', header: 'Lợi tức', width: '10rem', class: 'text-right justify-content-end' },
			{ field: 'totalValue', isSort: true, header: 'Số tiền', width: '10rem', cutText: 'b-cut-text-10' },
			// { field: 'bondPolicyDetailName', header: 'Thời hạn', width: '8rem', class: 'text-right justify-content-end' },
			{ field: 'source', header: 'Loại', width: '5rem' },
			{ field: 'orderer', header: 'Nguồn', width: '8rem' },
			{ field: 'columnResize', header: '', type:'hidden' },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});

		// this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('contractProcessingGan') ?? this.cols;
	}

	changeStatus() {
		this.setPage({ Page: this.offset })
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
				this.setLocalStorage(this._selectedColumns, 'contractProcessingGan')
				console.log('Luu o local', this._selectedColumns);
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.customerName = row?.businessCustomer?.name || (row?.investor?.investorIdentification?.fullname || row?.investor?.name),
			row.contractCode = row?.contractCode,
			row.buyDate = this.formatDateTime(row?.buyDate),
			row['policy.name'] = row?.policy?.name,
			row.bondPolicyDetailName = row?.policyDetail?.name,
			row.totalValue = this.utils.transformMoney(row?.totalValue),
			row.profitDisplay = row?.policyDetail?.profit ? this.utils.transformPercent(row?.policyDetail?.profit) + "%/năm" : ""
		};
		console.log('showData', rows);
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [
				{
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				}
			];
			return actions;
		});
	}

	detail(order) {
		this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id) + '']);
	}

	init() {
		this.isLoading = true;
		forkJoin([this._orderService.getAll(this.page, 'orderContractProcessing', this.status, this.dataFilter), this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resSecondary]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resOrder, '')) {
				this.page.totalItems = resOrder?.data?.totalItems;
				this.rows = resOrder?.data?.items;
				this.distributions = resSecondary?.data;
				//
				if (this.rows?.length) {
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

	changeFieldFilter() {
		if (this.keyword?.trim()) {
		  this.setPage();
		}
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		//
		this.isLoading = true;
		console.log("this.status", this.status);
	
		this._orderService.getAll(this.page, 'orderContractProcessing', this.status, this.dataFilter, this.sortData).subscribe((resOrder) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resOrder, '')) {
				this.page.totalItems = resOrder?.data?.totalItems;
				this.rows = resOrder?.data?.items;
				// 
				if (this.rows?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows)
				}
				console.log({ rowsOrder: resOrder.data.items, totalItems: resOrder.data.totalItems });
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

	changeDistribution(id) {
		this.policies = [];
		const distribution = this.distributions.find(item => item.id == id);
		this.policies = distribution?.policies ?? [];
		if (this.policies?.length) {
			this.policies = [ ...this.policies];
		}
		this.setPage();
		console.log(" this.policies", this.policies);
	}

	changePolicy(policyId) {
		this.setPage();
	}
}
