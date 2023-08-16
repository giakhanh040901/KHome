import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AtributionConfirmConst, BlockageLiberationConst, DistributionConst, OrderConst, TableConst } from '@shared/AppConsts';
import { forkJoin, Observable, Subscription } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { DistributionService } from '@shared/services/distribution.service';
import { ReinstatementRequestComponent } from './reinstatement-request/reinstatement-request.component';
import { WithdrawalRequestComponent } from './withdrawal-request/withdrawal-request.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider-selected.service';
import { OrderService } from '@shared/services/order.service';
import { TradingProviderService } from '@shared/services/trading-provider.service';
import { IAction, IColumn, DataTableEmit } from '@shared/interface/p-table.model';
import { OrderFilter } from '@shared/interface/filter.model';

@Component({
	selector: 'app-contract-active',
	templateUrl: './contract-active.component.html',
	styleUrls: ['./contract-active.component.scss']
})
export class ContractActiveComponent extends CrudComponentBase  {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _orderService: OrderService,
		private _distributionService: DistributionService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private _tradingProviderService: TradingProviderService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private dialogService: DialogService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Hợp đồng' },
		]);
	}

	modalDialog: boolean;

	BlockageLiberationConst = BlockageLiberationConst;
	OrderConst = OrderConst;
	DistributionConst = DistributionConst;

	rows: any[] = [];
	columns: IColumn[] = [];
	listAction: IAction[][] = [];
	page = new Page();

	distributions: any[] = [];
	policies: any[] = [];
	policyDetails: any[] = [];
	//
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

	order: any = {};
	blockageDialog: boolean;
	tradingProviders: any[] = [];
	// Menu otions thao tác
	isPartner: boolean;

	dataFilter: OrderFilter = new OrderFilter();
	dataTableEmit: DataTableEmit = new DataTableEmit();

	tradingProviderSub: Subscription;
	isInit: boolean = true;

	ngOnInit() {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change: number[] = []) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		});

		this.blockageLiberation.type = BlockageLiberationConst.LOAI_CAM_CO_KHOAN_VAY;

		// Config atribution columnTable 
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: 'left', class: 'b-border-frozen-left', isSort: true },
			{ field: 'project.invCode', header: 'Mã dự án', width: 14, isPin: true, isResize: true, isSort: true },
			{ field: 'contractCode', header: 'Mã hợp đồng', width: 10.5, isPin: true, isSort: true },
			{ field: 'totalValue', header: 'Số tiền đầu tư', width: 10.5, isPin: true, type: TableConst.columnTypes.CURRENCY, isResize: true, isSort: true },
			{ field: 'customerName', header: 'Khách hàng', width: 18, isPin: true, isSort: true },
			{ field: 'investDate', header: 'Ngày đầu tư', width: 10, isSort: true, type: TableConst.columnTypes.DATE },
			{ field: 'policy.name', header: 'Chính sách', width: 12, isSort: true },
			{ field: 'policyDetailName', header: 'Thời hạn', width: 8, class: 'text-right justify-content-end', isSort: true},
			{ field: 'policyDetail.profit', header: 'Tỉ lệ lợi tức', width: 9, type: TableConst.columnTypes.CURRENCY, unit: '%/năm', isSort: true},
			{ field: 'source', header: 'Loại', width: 5, type: TableConst.columnTypes.STATUS },
			{ field: 'orderSource', header: 'Nguồn', width: 8, type: TableConst.columnTypes.STATUS },
			{ field: 'methodInterest', header: 'Loại chi', width: 8.5, type: TableConst.columnTypes.STATUS },
			{ field: 'status', header: 'Trạng thái', width: 7, isFrozen: true, alignFrozen: 'right', type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, isFrozen: true, alignFrozen: 'right', displaySettingColumn: false, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		let apis: Observable<any>[] = [this._orderService.getAll(this.page, 'orderContract', this.dataFilter, this.sortData)];
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
			// row status
			row.sourceElement = OrderConst.getInfoSource(row?.source);
			row.orderSourceElement = OrderConst.getInfoOrderSource(row?.orderSource);
			row.methodInterestElement = DistributionConst.getMethodInterestInfo(row?.methodInterest);
			row.statusElement = OrderConst.getStatusInfo(row?.status);
		};
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}
	
	hideDialog() {
		this.blockageDialog = false;
	}
	
	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions: IAction[] = [];
			if(this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTC])) {
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event?.item?.data || $event);
					}
				});	
			}
			//
			if (!this.isPartner && orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao])) {
				actions.push({
					data: orderItem,
					label: 'Gửi thông báo',
					icon: 'pi pi-envelope',
					command: ($event) => {
						this.resentNotify($event?.item?.data || $event);
					}
				})
			}

			//
			if (!this.isPartner && orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionInvestConst.InvestHDPP_HopDong_YeuCauTaiTuc])) {
				actions.push({
					data: orderItem,
					label: 'Yêu cầu tái tục',
					icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.reinstatementRequest($event?.item?.data || $event);
					}
				})
			}

			// Hợp đồng đang đầu tư và loại sản phẩm phân phối khác KHONG_CHI_TIEN thì mới đc yêu cầu rút vốn
			if (!this.isPartner && orderItem?.status == OrderConst.DANG_DAU_TU && orderItem?.methodInterest != DistributionConst.KHONG_CHI_TIEN && this.isGranted([this.PermissionInvestConst.InvestHDPP_HopDong_YeuCauRutVon]) ) {
				actions.push({
					data: orderItem,
					label: 'Yêu cầu rút vốn',
					icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.withdrawalRequest($event?.item?.data || $event);
					}
				})
			}

			if (!this.isPartner && (orderItem.status == OrderConst.DANG_DAU_TU) && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_NhanHDCung])) {
				actions.push({
					data: orderItem,
					label: 'Nhận HĐ bản cứng',
					icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.takeHardContract($event?.item?.data || $event);
					}
				})
			}
			//
			if (!this.isPartner && this.isGranted([this.PermissionInvestConst.InvestHDPP_HopDong_PhongToaHopDong])) {
				actions.push({
					data: orderItem,
					label: 'Phong tỏa HĐ',
					icon: 'pi pi-ban',
					command: ($event) => {
						this.blockade($event?.item?.data || $event);
					}
				})
			}
			return actions;
		});
	}
	
	detail(order) {
		// this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id)); 
		window.open('/trading-contract/order/detail/' + (cryptEncodeId), "_blank");
	}	

	// Form yêu cầu tái tục
	reinstatementRequest(order) {
		this.dialogService.open(
			ReinstatementRequestComponent,
			{
				header: "Yêu cầu tái tục",
				width: '450px',
				data: {
				orderId: order?.id,
			}
		}).onClose.subscribe((res) => {
			if(res.status) {
				this.messageSuccess('Yêu cầu thành công', '');
				this.setPage();
			}
		});
	}

	// Form yêu cầu rút vốn
	withdrawalRequest(order) {
		this.dialogService.open(
			WithdrawalRequestComponent,
			{
				header: "Yêu cầu rút vốn",
				width: '450px',
				data: {
					orderId: order?.id,
				}
			}).onClose.subscribe((res) => {
				if(res.status) {
					this.messageSuccess('Yêu cầu thành công', '');
					this.setPage();
				}
			}
		);
	}

	blockade(contractActive) {
		this.blockageLiberation.orderId = contractActive?.id;
		this.blockageLiberation.totalValue = contractActive?.totalValue;
		this.blockageLiberation.blockadeDate = new Date();
		this.blockageDialog = true;
	}
	
	saveBlockade() {
		this._orderService.blockadeContractActive(this.blockageLiberation).subscribe((response) => {
			if (this.handleResponseInterceptor(response, "Phong toả thành công")) {
				this.setPage();
				this.blockageDialog = false;
				this.blockageLiberation.blockadeDescription = null;
			}
		});
	}

	takeHardContract(order) {
		this.confirmDialog(
			'Xác nhận gửi yêu cầu nhận hợp đồng bản cứng?',
			this._orderService.takeHardContract(order.id)
		);
    }

	// Form Gửi thông báo
	resentNotify(order) {
		this.confirmDialog(
			'Xác nhận gửi thông báo?',
			this._orderService.resentNotify(order?.id)
		);
    }
	//
	confirmDialog(message: string, apiCall: Observable<number>) {
        this.confirmationService.confirm({
            message: message,
            ...AtributionConfirmConst,
            accept: () => {
                apiCall.subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.setPage();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!', '');
                });
            },
        });
    }

	changeDistribution(id) {
		this.dataFilter.policy = [];
		this.dataFilter.policyDetailId = null;
		this.policies = [];
		const bondSecondary = this.distributions.find(item => item.id == id);
		this.policies = bondSecondary?.policies ?? [];
		if (this.policies?.length) {
			this.policies = [...this.policies];
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
