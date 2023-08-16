import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { BlockageLiberationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject, Subscription } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { DistributionService } from '@shared/services/distribution.service';
import { debounceTime } from 'rxjs/operators';
import { ReinstatementRequestComponent } from './reinstatement-request/reinstatement-request.component';
import { WithdrawalRequestComponent } from './withdrawal-request/withdrawal-request.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';

@Component({
	selector: 'app-contract-active',
	templateUrl: './contract-active.component.html',
	styleUrls: ['./contract-active.component.scss']
})
export class ContractActiveComponent extends CrudComponentBase {

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
		//
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
		tradingProviderIds: []
	}

	blockageLiberation: any = {
		"id": 0,
		"type": null,
		"blockadeDescription": "",
		"blockadeDate": null,
		"orderId": 0,
		"blockader": null,
		"blockadeTime": null,
		"liberationDescription": null,
		"liberationDate": null,
		"liberator": null,
		"liberationTime": null,
		"status": null,
		"contractCode": null,
		"totalValue": null
	};

	BlockageLiberationConst = BlockageLiberationConst;
	status: any;
	
	submitted: boolean;
	listAction: any[] = [];

	page = new Page();
	offset = 0;

	order: any = {};
	blockageDialog: boolean;
	// Menu otions thao tác

	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.statusActiveSettlement
	];
	tradingProviderSub: Subscription;

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})

		this.init();
		this.blockageLiberation.type = 2;

		this.setPage({ page: this.offset });
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

		this.cols = [
			// { field: 'productCode', header: 'Mã dự án', width: '14rem', cutText:'b-cut-text-14', isPin: true },
			{ field: 'contractCode', isSort: true, header: 'Mã HĐ', width: '10rem', isPin: true, cutText: 'b-cut-text-10' },
			{ field: 'customerName', isSort: true, header: 'Khách hàng', width: '18rem',isPin: true },
			{ field: 'investDate', isSort: true, header: 'Ngày tích lũy', width: '12rem' },
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
		
		// this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('contractActiveGan') ?? this.cols;
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
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
		};
		console.log('showData', rows);
	}

	hideDialog() {
		this.blockageDialog = false;
	}

	saveBlockade() {
		this.confirmationService.confirm({
		  message: 'Bạn có chắc chắn phong toả hợp đồng này?',
		  header: 'Phong toả hợp đồng',
		  acceptLabel: "Đồng ý",
		  rejectLabel: "Hủy",
		  icon: 'pi pi-exclamation-triangle',
		  accept: () => {
			this._orderService.blockadeContractActive(this.blockageLiberation).subscribe((response) => {
				if (this.handleResponseInterceptor(response, "Phong toả thành công")) {
					this.setPage();
					this.blockageDialog = false;
					this.blockageLiberation.blockadeDescription = null;
				}
			});
		  },
		  reject: () => {
	
		  },
		});
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
				this.setLocalStorage(this._selectedColumns, 'contractActiveGan')
				console.log('Luu o local', this._selectedColumns);
			}
		});
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			console.log("orderItem?.isWithdrawalRequest",orderItem?.isWithdrawalRequest);
			
			const actions = []
			if(this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTC])) {
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				});	
			}
			//
			if (!this.isPartner && orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao])) {
				actions.push({
					data: orderItem,
					label: 'Gửi thông báo',
					icon: 'pi pi-envelope',
					command: ($event) => {
						this.resentNotify($event.item.data);
					}
				})
			}

			//
			// if (orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauTaiTuc]) && !orderItem?.isRenewalsRequest) {
			// 	actions.push({
			// 		data: orderItem,
			// 		label: 'Yêu cầu tái tục',
			// 		icon: 'pi pi-sort-amount-up',
			// 		command: ($event) => {
			// 			this.reinstatementRequest($event.item.data);
			// 		}
			// 	})
			// }
			//
			// if (orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauRutVon]) && !orderItem?.isWithdrawalRequest ) {
			if (!this.isPartner && orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_HopDong_YeuCauRutVon]) ) {
				actions.push({
					data: orderItem,
					label: 'Yêu cầu rút vốn',
					icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.withdrawalRequest($event.item.data);
					}
				})
			}
			
			// if (this.isGranted([this.PermissionGarnerConst.GarnerHDPP_HopDong_PhongToaHopDong])) {
			// 	actions.push({
			// 		data: orderItem,
			// 		label: 'Phong tỏa HĐ',
			// 		icon: 'pi pi-ban',
			// 		command: ($event) => {
			// 			this.blockade($event.item.data);
			// 		}
			// 	})
			// }

			return actions;
		});
	}

	blockade(contractActive) {
		console.log("contractActive",contractActive);
		
		this.blockageLiberation.orderId = contractActive?.id;
		this.blockageLiberation.totalValue = contractActive?.totalValue;
		this.blockageLiberation.blockadeDate = new Date();
		this.blockageDialog = true;
	  }
	

	init() {
		this.isLoading = true;
		forkJoin([this._orderService.getAll(this.page, 'orderContract', this.status,this.dataFilter), this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resSecondary]) => {
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
		
		this._orderService.getAll(this.page, 'orderContract', this.status, this.dataFilter, this.sortData).subscribe((res) => {
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

	changeStatus() {
		this.setPage({ Page: this.offset })
	}

	// Form Gửi thông báo
	resentNotify(order) {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn gửi thông báo?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderService.resentNotify(order?.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.setPage();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!');
                });
            },
            reject: () => {

            },
        });
    }

	// Form yêu cầu tái tục
	reinstatementRequest(order) {
		const ref = this.dialogService.open(
			ReinstatementRequestComponent,
			{
				header: "Yêu cầu tái tục",
				width: '450px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
				style: { "overflow": "auto" },
				data: {
				orderId: order?.id,
			}
		});

		ref.onClose.subscribe((res) => {
			if(res.status) {
				this.messageSuccess('Yêu cầu thành công');
				this.setPage();
			}
		});
	}

	// Form yêu cầu rút vốn
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
			if(res.status) {
				this.messageSuccess('Yêu cầu thành công');
				this.setPage();
			}
		});
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

	detail(order) {
		this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
	}

}
