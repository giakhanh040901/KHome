import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AtributionConfirmConst, BlockageLiberationConst, OrderConst, PolicyTemplateConst, SearchConst, TableConst } from '@shared/AppConsts';
import { forkJoin, Subscription } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { OrderService } from '@shared/services/order.service';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { OrderRenewalFilter } from '@shared/interface/filter.model';

@Component({
  selector: 'app-contract-renewal',
  templateUrl: './contract-renewal.component.html',
  styleUrls: ['./contract-renewal.component.scss']
})
export class ContractRenewalComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderService,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private confirmationService: ConfirmationService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Hợp đồng tái tục' },
		]);
	}

	OrderConst = OrderConst;
	PolicyTemplateConst = PolicyTemplateConst;
	BlockageLiberationConst = BlockageLiberationConst;

	rows: any[] = [];
	columns: IColumn[];

	// data Filter
	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];

	dataFilter: OrderRenewalFilter = new OrderRenewalFilter();

	submitted: boolean;
	listAction: any[] = [];
	page = new Page();

	order: any = {};
	// Menu otions thao tác

	isInit: boolean = true;
	isPartner: boolean;
	tradingProviderSub: Subscription;
	dataTableEmit: DataTableEmit = new DataTableEmit();

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		});

		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true},
			{ field: 'genContractCode', header: 'Mã HĐ mới', width: 11, isPin: true, isResize: true, isSort: true },
			{ field: 'contractCodeOriginal', header: 'Mã HĐ gốc', width: 11, isPin: true, isSort: true },
			{ field: 'customerName', header: 'Khách hàng', width: 18, isPin: true, isSort: true },
			{ field: 'project.invCode', header: 'Mã dự án', width: 15, isSort: true },
			{ field: 'requestDate', header: 'Ngày yêu cầu', width: 11, isSort: true, type: TableConst.columnTypes.DATETIME, },
			{ field: 'policy.name', header: 'Chính sách', width: 12, isSort: true },
			{ field: 'policyDetailName', header: 'Kỳ hạn', width: 8, class: 'justify-content-end text-right', isSort: true },
			{ field: 'source', header: 'Loại hình', width: 9, type: TableConst.columnTypes.STATUS, class: 'justify-content-center text-center'},
			{ field: 'renewalMoney', header: 'Tiền tái tục', width: 10, type: TableConst.columnTypes.CURRENCY, isSort: true},
			{ field: 'settlementMethodDisplay', header: 'Loại tái tục', width: 8, class: 'justify-content-center text-center'},
			{ field: 'orderSource', header: 'Nguồn đặt', width: 8, type: TableConst.columnTypes.STATUS },
			{ field: 'approveRenewalBy', header: 'Người duyệt', width: 11 },
			{ field: 'approveRenewalDate', header: 'Ngày duyệt', width: 12, isSort: true, type: TableConst.columnTypes.DATETIME, },
			{ field: 'status', header: 'Trạng thái', width: 8.1, type: TableConst.columnTypes.STATUS, isFrozen:true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: true, type: TableConst.columnTypes.ACTION_DROPDOWN, isFrozen:true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-end' },
		];
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	setData(rows) {
		for (let row of rows) {
			row.customerName = row?.businessCustomer?.name || row?.investor?.investorIdentification?.fullname;
			row['project.invCode'] = row?.project?.invCode;
			row['policy.name'] = row?.policy?.name;
      		row.invName =row?.project?.invName || row?.project?.shareName;
			row.policyDetailName = row?.policyDetail?.name;
			row.settlementMethodDisplay = OrderConst.getReinstatementType(row.settlementMethod, 'shortName');
			//
			row.sourceElement = OrderConst.getInfoSource(row.source);
			row.orderSourceElement = OrderConst.getInfoOrderSource(row.orderSource);
			row.statusElement = OrderConst.getStatusRenewal(row.status);	
		}
	}
	
	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
			//
			if(this.isGranted([this.PermissionInvestConst.InvestHDPP_HDTT_ThongTinDauTu])) {
				//
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item?.data?.orderId);
					}
				});	
				//
				if(orderItem?.orderNewId) {
					actions.push({
						data: orderItem,
						label: 'Thông tin HĐ mới',
						icon: 'pi pi-info-circle',
						command: ($event) => {
							this.detail($event.item?.data?.orderNewId);
						}
					});
				}
			}
			if(!this.isPartner && (true || this.isGranted([this.PermissionInvestConst.InvestHDPP_HDTT_HuyYeuCau])) && orderItem.status == OrderConst.RENEWAL_PENDING) {
				//
				actions.push({
					data: orderItem,
					label: 'Hủy yêu cầu',
					icon: 'pi pi-times-circle',
					command: ($event) => {
						this.cancelRenewal($event.item?.data?.orderId);
					}
				});	
			}
			//
			return actions;
		});
	}

	onSort(event) {
		this.sortData = event;
		this.setPage();
	}

	setPage(event?: any) {
		if(!event) this.page.pageNumber = 0;
		let requestApis = [this._orderService.getAll(this.page, 'orderContractRenewal', this.dataFilter, this.sortData)];
		if(this.isInit) {
			requestApis[1] = this._distributionService.getDistributionsOrder();
			this.isInit = false;
		}
		//
		this.isLoading = true;
		forkJoin(requestApis).subscribe(res => {
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
			//
			if (this.rows?.length) {
				this.genListAction(this.rows);
				this.setData(this.rows);
			}
		}

		if(resDistribution && this.handleResponseInterceptor(resDistribution)) {
			this.distributions = resDistribution?.data;
			this.distributions = this.distributions.map(item => {
				return {
					...item,
					invName: [item.project.invName, item.project.invCode].join(' - ')
				}
			});
		}
	}

	changeDistribution(id) {
		this.policies = [];
		this.dataFilter.policyDetailId = null;
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
		if (this.policyDetails?.length) {
			this.policyDetails = [...this.policyDetails];
		}
		//
		this.setPage();
	}

	detail(orderId) {
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(orderId)); 
		window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
	}

	cancelRenewal(orderId) {
		this.confirmationService.confirm({
			message: 'Bạn chắc chắn Hủy yêu cầu tái tục này?',
			...AtributionConfirmConst,
			accept: () => {
				this._orderService.cancelRenewal(orderId).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy thành công!")) {
						this.setPage();
					}
				});
			},
		});
	  }
}

