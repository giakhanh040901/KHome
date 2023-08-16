import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AtributionConfirmConst, BlockageLiberationConst, OrderConst, TableConst } from '@shared/AppConsts';
import { forkJoin, Subscription } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { OrderService } from '@shared/services/order.service';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { OrderHistoryFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-investment-history',
  templateUrl: './investment-history.component.html',
  styleUrls: ['./investment-history.component.scss']
})
export class InvestmentHistoryComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderService,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Lịch sử đầu tư' },
		]);
	}

	BlockageLiberationConst = BlockageLiberationConst;
	OrderConst = OrderConst;

	rows: any[] = [];
	columns: IColumn[];

	// data Filter
	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];

	submitted: boolean;
	listAction: IAction[][] = [];
	page = new Page();

	order: any = {};
	// Menu otions thao tác
	isInit: boolean = true;
	tradingProviderSub: Subscription;

	dataTableEmit: DataTableEmit = new DataTableEmit();
	dataFilter: OrderHistoryFilter = new OrderHistoryFilter();

	ngOnInit() {
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})
		
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: "b-border-frozen-left", isSort: true },
			{ field: 'contractCode', header: 'Mã hợp đồng', width: 12, isPin: true, isResize: true, isSort: true },
			{ field: 'invCode', header: 'Mã dự án', width: 14, cutText:'b-cut-text-14', isPin: true, isSort: true },
			{ field: 'customerName', header: 'Khách hàng', width: 18, cutText:'b-cut-text-15', isPin: true, isSort: true },
			{ field: 'investDate', header: 'Ngày đầu tư', width: 11, isSort: true, type: TableConst.columnTypes.DATETIME,},
			{ field: 'initTotalValue', header: 'Tiền đầu tư', width: 11, isSort: true, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'policyName', header: 'Chính sách', width: 12, isSort: true },
			{ field: 'policyDetailName', header: 'Kỳ hạn', width: 10, class: 'justify-content-end text-right', isSort: true },
			{ field: 'profit', header: 'Tỷ lệ lợi tức', width: 10, type: TableConst.columnTypes.CURRENCY, unit: '%/năm', isSort: true },
			{ field: 'dueDate', header: 'Ngày đáo hạn', width: 11, isSort: true, type: TableConst.columnTypes.DATE,},
			{ field: 'source', header: 'Loại', width: 6, type: TableConst.columnTypes.STATUS },
			{ field: 'orderSource', header: 'Nguồn', width: 10, type: TableConst.columnTypes.STATUS},
			{ field: 'status', header: 'Trạng thái', width: 10, type: TableConst.columnTypes.STATUS, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right'},
			{ field: '', header: '', width: 4, displaySettingColumn: true, type: TableConst.columnTypes.ACTION_DROPDOWN, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-end'},
		];
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		let apiRequests = [this._orderService.getAll(this.page, 'investmentHistory', this.dataFilter)];
		if(this.isInit) {
			apiRequests[1] = this._distributionService.getDistributionsOrder();
			this.isInit = false;
		}
		//
		this.isLoading = true;
		forkJoin(apiRequests).subscribe(res => {
			this.isLoading = false;
			this.handleData(res[0], res[1]);
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	handleData(resOrder, resDistribution) {
		if(resOrder && this.handleResponseInterceptor(resOrder)) {
			this.page.totalItems = resOrder.data.totalItems;
			if(this.page.pageSize === this.page.pageSizeAll) {
				// LOAD MORE DATA
				if(this.page.pageNumberLoadMore === 1) this.rows = [];
				this.rows = [...this.rows, ...resOrder?.data?.items];
			} else {
				this.rows = resOrder?.data?.items;
			}
			if (this.rows?.length) {
				this.genListAction(this.rows);
				this.showData(this.rows);
			}
		}

		if(resDistribution && this.handleResponseInterceptor(resDistribution)) {
			this.distributions = resDistribution?.data;
			this.distributions = this.distributions.map(item => {
				return { 
					...item,
					invName: [item.project.invName,item.project.invCode].join(' - '),
				}
			});
		}
	}

	showData(rows) {
		for (let row of rows) {
			row.policyDetailName = row?.policyDetail?.name,
			//
			row.sourceElement = OrderConst.getInfoSource(row.source);
			row.orderSourceElement = OrderConst.getInfoOrderSource(row.orderSource);
			row.statusElement = OrderConst.getHistoryStatus(row.investHistoryStatus);	
		};
		console.log('showData', rows);
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = []
			if(this.isGranted([this.PermissionInvestConst.InvestHDPP_LSDT_ThongTinDauTu])) {
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

	// Form Gửi thông báo
	resentNotify(order) {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn gửi thông báo?',
            ...AtributionConfirmConst,
            accept: () => {
                this._orderService.resentNotify(order?.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.setPage();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!', '');
                });
            }
        });
    }
	
	changeDistribution(id) {
		this.dataFilter.policyId = null;
		this.dataFilter.policyDetailId = null;
		this.policies = [];
		//
		const bondSecondary = this.distributions.find(item => item.id == id);
		this.policies = bondSecondary?.policies ?? [];
		if (this.policies?.length) {
			this.policies = [...this.policies];
		}
		//
		this.setPage();
	}

	changePolicy(policy) {
		this.dataFilter.policyDetailId = null;
		this.policyDetails = [];
		//
		const bondPolicy = this.policies.find(item => item.id == policy);
		this.policyDetails = bondPolicy?.policyDetail ?? [];
		//
		if (this.policyDetails?.length) {
			this.policyDetails = [...this.policyDetails];
		}
		this.setPage();
	}

	detail(order) {
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id)); 
		window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
	}
	
}

