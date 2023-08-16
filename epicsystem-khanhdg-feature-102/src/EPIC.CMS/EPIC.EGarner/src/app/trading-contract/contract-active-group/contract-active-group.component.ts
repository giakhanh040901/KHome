import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService, TreeNode } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { OrderActiveGroupConst, OrderConst, ProductConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { DistributionService } from '@shared/services/distribution.service';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { WithdrawalRequestComponent } from '../contract-active/withdrawal-request/withdrawal-request.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';

@Component({
	selector: 'app-contract-active-group',
	templateUrl: './contract-active-group.component.html',
	styleUrls: ['./contract-active-group.component.scss']
})
export class ContractActiveGroupComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderServiceProxy,
		private breadcrumbService: BreadcrumbService,
		private _distributionService: DistributionService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private dialogService: DialogService,

		// private _bondSecondaryService: ProductBondSecondaryServiceProxy,
		private router: Router,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Hợp đồng' },
		]);
	}

	ProductConst = ProductConst
	OrderActiveGroupConst = OrderActiveGroupConst;
	OrderConst = OrderConst; 
	
	rows: TreeNode[];

	statuses: any[] = [
		{
			name: 'Tất cả',
			code: ''
		},
		...OrderConst.statusActive
	]

	dataFilter = {
		fieldFilter: null,
		productType: null,
		distributionId: null,
		policyId: null,
		status: null,
		tradingProviderIds: []
	}

	distributions: any[] = [];
	policies: any[] = [];
	listAction: any[] = [];
	listActionGroup: any[] = [];

	isInit: boolean = true;

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		//
		this.subject.isSetPage.subscribe(() => {
			this.setPage(this.getPageCurrentInfo());
		});
		this.setPage();
	}

	changeFieldFilter() {
		if (this.keyword?.trim()) {
			this.setPage();
		}
	}

	changeStatus() {
		this.setPage();
	}

	changePolicy(policyId) {
		this.setPage();
	}

	changeDistribution(id) {
		this.policies = [];
		const distribution = this.distributions.find(item => item.id == id);
		this.policies = distribution?.policies ?? [];
		this.setPage();
	}

	genListAction(rows = []) {
		this.listActionGroup = rows.map(row => {
			const actionGroup = [];
			for(let order of row.listOrder) {
				const actions = [];
				actions.push({
					data: order,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				});
				this.listAction[order.id] = [...actions];  
			} 
			//
			if (!this.isPartner && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauRutVon])) {
				actionGroup.push({
					data: row.listOrder[0],
					label: 'Yêu cầu rút vốn',
					icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.withdrawalRequest($event.item.data);
					}
				});
			}
			//
			return actionGroup;
		});
		
	}

	getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		//
		this.isLoading = true;
		let apiRequests = [];
		// Lấy danh sách Order
		apiRequests.push(this._orderService.getAllGroupPolicy(this.page,this.dataFilter, this.sortData));
		// Lấy danh sách sản phẩm phân phối cho bộ lọc chỉ gọi lần load trang đầu tiên
		if(this.isInit) {
			apiRequests.push(this._distributionService.getDistributionsOrder());
		}
		//
		forkJoin(apiRequests).subscribe(res => {
			this.isLoading = false;
			const resOrder: any = res[0];
			if (this.handleResponseInterceptor(resOrder, '')) {
				this.page.totalItems = resOrder.data.totalItems;
				this.rows = resOrder.data.items;
				//
				if (this.rows?.length) {
					this.genListAction(this.rows);
				}
			}
			//
			if(this.isInit) {
				const resDistribution: any = res[1];
				this.distributions = resDistribution?.data;
			}
			//
			this.isInit = false;
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	detail(order) {
		this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
	}

	withdrawalRequest(order) {
		const ref = this.dialogService.open(
			WithdrawalRequestComponent,
			{
				header: "Tạo yêu cầu rút tích lũy",
				width: '500px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
				style: { "overflow": "auto" },
				data: {
				orderDetail: order,
			}
		});

		ref.onClose.subscribe((res) => {
			if(res?.status) {
				this.messageSuccess('Yêu cầu thành công');
				this.setPage(this.getPageCurrentInfo());
			}
		});
	}
}
