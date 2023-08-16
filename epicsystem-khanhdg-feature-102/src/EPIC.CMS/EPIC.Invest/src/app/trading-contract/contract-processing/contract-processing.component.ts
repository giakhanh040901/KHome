import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DistributionConst, OrderConst, TableConst } from '@shared/AppConsts';
import { forkJoin, Observable, Subscription } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { OrderService } from '@shared/services/order.service';
import { TradingProviderService } from '@shared/services/trading-provider.service';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { OrderFilter } from '@shared/interface/filter.model';

@Component({
	selector: 'app-contract-processing',
	templateUrl: './contract-processing.component.html',
	styleUrls: ['./contract-processing.component.scss']
})
export class ContractProcessingComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderService,
		private _distributionService: DistributionService,
		private _tradingProviderService: TradingProviderService,
		private breadcrumbService: BreadcrumbService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
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
	columns: IColumn[] = [];
	listAction: IAction[][] = [];
	page = new Page();

	// data Filter
	distributions = [];
	policies = [];
	policyDetails = [];
	
	tradingProviders: any[] = [];

	dataFilter: OrderFilter = new OrderFilter();
	dataTableEmit: DataTableEmit = new DataTableEmit();

	tradingProviderSub: Subscription;
	isInit: boolean = true;

	ngOnInit() {
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change: number[]= []) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		});

		this.columns = [
			{ field: 'id', header: '#ID', width: 5, cutText:'b-cut-text-14', isPin: true,isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'project.invCode', header: 'Mã dự án', width: 14, isPin: true, isResize: true, isSort: true },
			{ field: 'contractCode', header: 'Mã hợp đồng', width: 10.5, isPin: true, isSort: true },
			{ field: 'totalValue', header: 'Số tiền đầu tư', width: 10.5, isPin: true, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'customerName', header: 'Khách hàng', width: 18, isPin: true, isSort: true  },
			{ field: 'buyDate', header: 'Ngày đặt lệnh', width: 11, isSort: true , type: TableConst.columnTypes.DATE},
			{ field: 'policy.name', header: 'Chính sách', width: 9, isSort: true },
			{ field: 'policyDetailName', header: 'Thời hạn', width: 8, class: 'text-right justify-content-end', isSort: true },
			{ field: 'policyDetail.profit', header: 'Tỉ lệ lợi nhuận', type: TableConst.columnTypes.CURRENCY, unit: '%/năm', width: 11, isSort: true },
			{ field: 'source', header: 'Loại', width: 6, type: TableConst.columnTypes.STATUS },
			{ field: 'orderSource', header: 'Nguồn', width: 8, type: TableConst.columnTypes.STATUS },
			{ field: 'methodInterest', header: 'Loại chi', width: 8.5, type: TableConst.columnTypes.STATUS },
			{ field: 'status', header: 'Trạng thái', width: 10, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		let apis: Observable<any>[] = [this._orderService.getAll(this.page, 'orderContractProcessing', this.dataFilter, this.sortData)];
		if(this.isInit) {
			apis[1] = this._distributionService.getDistributionsOrder(); 
			apis[2] = this._tradingProviderService.getAllTradingProviderNoPermision();
			this.isInit = false;
		}
		//
		this.isLoading = true;
		forkJoin(apis).subscribe((response) => {
			this.isLoading = false;
			this.handleData(response[0], response[1], response[2]);
		}, (err) => {
			this.isLoading = false;
		});
	}

	handleData(resOrder, resDistribution, resTradingProvider) {
		if(resOrder && this.handleResponseInterceptor(resOrder)) {
			this.page.totalItems = resOrder?.data?.totalItems;
			if(this.page.pageSize === this.page.pageSizeAll) {
				// LOAD MORE DATA
				if(this.page.pageNumberLoadMore === 1) this.rows = [];
				this.rows = [...this.rows, ...resOrder?.data?.items];
			} else {
				this.rows = resOrder?.data?.items;
			}
			//
			if (this.rows?.length) {
				this.genListAction(this.rows);
				this.setData(this.rows)
			}
		}
		//
		if(resDistribution && this.handleResponseInterceptor(resDistribution)) {
			this.distributions = resDistribution?.data;
			if (this.distributions?.length){
				this.distributions.forEach(element => {
					this.policies =  [...this.policies, ...element?.policies];
					element.invName = element.project.invName + ' - ' + element.project.invCode;
				});
			}
		}
		//
		if (resTradingProvider && this.handleResponseInterceptor(resTradingProvider, '')) {
			this.tradingProviders = resTradingProvider?.data?.items; 
		}
	}


	setData(rows) {
		for (let row of rows) {
			row.customerName = row?.businessCustomer?.name || row?.investor?.investorIdentification?.fullname || row?.investor?.name,
			row['project.invCode'] = row?.project?.invCode,
			row['policy.name'] = row?.policy?.name,
			row.policyDetailName = row?.policyDetail?.name,
			row['policyDetail.profit'] = row?.policyDetail?.profit;
			row.contractCode = row?.genContractCode || row?.contractCode;
			//
			row.sourceElement = OrderConst.getInfoSource(row?.source);
			row.orderSourceElement = OrderConst.getInfoOrderSource(row?.orderSource);
			row.methodInterestElement = DistributionConst.getMethodInterestInfo(row?.methodInterest);
			row.statusElement = OrderConst.getStatusInfo(row?.status);
		};
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions: IAction[] = [
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
		// this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id)); 
		window.open('/trading-contract/order/detail/' + (cryptEncodeId), "_blank");
	}

	changeDistribution(id) {
		this.dataFilter.policy = [];
		this.dataFilter.policyDetailId = null;
		this.policies = [];
		const bondSecondary = this.distributions.find(item => item.id == id);
		this.policies = bondSecondary?.policies ?? [];
		if (this.policies?.length) {
			this.policies = [ ...this.policies];
		}
		this.setPage();
	}

	changePolicy(policies) {
		this.dataFilter.policyDetailId = null;
		this.policyDetails = [];
		//	
		if (policies?.length == 1){
			let policyDetail = this.policies.find(item => item.id == policies)?.policyDetail;
			this.policyDetails = [...this.policyDetails, ...policyDetail];
		} 
		//
		this.setPage();
	}
}
