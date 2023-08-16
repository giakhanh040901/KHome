import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { OrderConst, SearchConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { forkJoin, Subject, Subscription } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { debounceTime } from 'rxjs/operators'
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';

@Component({
	selector: 'app-order',
	templateUrl: './order.component.html',
	styleUrls: ['./order.component.scss'],
})
export class OrderComponent extends CrudComponentBase {

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
			{ label: 'Sổ lệnh' },
		]);
		//
	}

	modalDialog: boolean;

	OrderConst = OrderConst;
	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.statusOrder,
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


	distributions: any[] = [];
	tradingDate: null;
	status: any;
	source: any;

	customerName = '';
	contractCode = '';

	rows: any[] = [];
	row: any;
	col: any;

	cols: any[];
	defaultCols: any[];
	_selectedColumns: any[];

	submitted: boolean;
	listAction: any[] = [];

	page = new Page();
	offset = 0;

	order: any = {};

	dataFilter = {
		distributionId: null,
		policyId: null,
		policyDetailId: null,
		tradingDate: null,
		fieldFilter: null,
		source: null,
        orderSource: null,
		orderer: null,
		tradingProviderIds: []
	}

	policies: any[] = [];
	policyDetails: any[] = [];

	selectedItems = [];
	tradingProviderSub: Subscription;
	fileExcel: any;

	ngOnInit() { 
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})

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
			{ field: 'contractCode', isSort: true, header: 'Mã HĐ', width: '10rem', isPin: true, cutText: 'b-cut-text-10' },
			{ field: 'customerName', isSort: true, header: 'Tên khách hàng', width: '18rem'},
			{ field: 'buyDate', isSort: true, header: 'Ngày đặt lệnh', width: '11rem'},
			{ field: 'policy.name', isSort: true, header: 'Chính sách', width: '12rem'},
			{ field: 'totalValue', isSort: true, header: 'Số tiền', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'source', header: 'Loại', width: '5rem' },
			{ field: 'orderer', header: 'Nguồn', width: '8rem' },
			{ field: 'columnResize', header: '', type:'hidden' },
		];
		// this._selectedColumns = this.cols;
		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		this._selectedColumns = this.getLocalStorage('orderGan') ?? this.cols;
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

	@ViewChild('fubauto') fubauto: any;
	myUploader(event) {
		this.fileExcel = event?.files[0];
		if (this.fileExcel) {
		  this.isLoading = true;
		  this._orderService.importFromExcel({file: this.fileExcel}).subscribe(
			(response) => {
			  this.fubauto.clear();
			  if (this.handleResponseInterceptor(response, 'Upload thành công!')) {
				this.setPage(this.getPageCurrentInfo());
			  }
			},
			(err) => {
			  this.messageError("Có sự cố khi upload!");
			}
		  ).add(() => {
			this.isLoading = false;
		  });
		}
	}

	importTemplate() {
		this.isLoading = true;
		this._orderService.downloadOrderTemple().subscribe((res) => {
			this.isLoading = false;
			this.handleResponseInterceptor(res);
		}, err => {
			this.isLoading = false;
		});
	}
	

	changeStatus() {
		this.setPage({ Page: this.offset })
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'orderGan')
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.customerName = row?.businessCustomer?.name || (row?.investor?.investorIdentification?.fullname || row?.investor?.name),
			row.contractCode = row?.contractCode,
			row.buyDate = this.formatDateTime(row?.buyDate),
			row['policy.name'] = row?.policy?.name,
			row.policyDetailName = row?.policyDetail?.name,
			row.totalValue = this.utils.transformMoney(row?.totalValue),
			row.profitDisplay = row?.policyDetail?.profit ? this.utils.transformPercent(row?.policyDetail?.profit) + "%/năm" : "";
			// row.quantity = this.utils.transformMoney(row?.quantity)
		};
		console.log('row', rows);
	}

	genListAction(data = []) {
		this.listAction = data?.map(orderItem => {
			const actions = [];

			if (this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT])) {
				actions.push({
					data: orderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}

			if (!this.isPartner && (orderItem.status == this.OrderConst.KHOI_TAO || orderItem.status == this.OrderConst.CHO_THANH_TOAN ) && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_Xoa])) {
			  actions.push({
			    data: orderItem,
			    label: 'Xoá',
			    icon: 'pi pi-trash',
			    command: ($event) => {
			      this.delete($event.item.data);
			      console.log("event...",$event.item.data);
			    }
			  });
			}

			return actions;
		});
	}

	delete(order) {
		this.confirmationService.confirm({
			message: 'Bạn chắc chắn xoá số lệnh này?',
			header: 'Cảnh báo!',
			icon: 'pi pi-exclamation-triangle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this._orderService.delete([order?.id]).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Xóa thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			},
			reject: () => {

			},
		});
	}

	deleteOrders() {
		let orderIds = this.selectedItems.map(item => item.id);
		this.confirmationService.confirm({
			message: 'Bạn chắc chắn xoá những lệnh này?',
			header: 'Cảnh báo!',
			icon: 'pi pi-exclamation-triangle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this._orderService.delete(orderIds).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Xóa thành công")) {
						this.selectedItems = [];
						this.setPage(this.getPageCurrentInfo());
					}
				});
			},
			reject: () => {
			},
		});
	}

	getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

	detail(order) {
		this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.id)]);
	}

	create() {
		this.router.navigate(['/trading-contract/order/create/filter-customer'], { queryParams : { isCreateNew: true }});
	}

	init() {
		this.isLoading = true;
		forkJoin([this._orderService.getAll(this.page, 'order', this.status, this.dataFilter), this._distributionService.getDistributionsOrder()]).subscribe(([res, resBondSecondary]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				console.log('res?.data?.totalItems: ', res?.data);
				
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items;
				this.distributions = resBondSecondary?.data;
				if (this.rows?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows)
				}
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
			this.policies = [...this.policies];
		}
		this.setPage();
		console.log(" this.policies", this.policies);
	}

	changePolicy(policyId) {
		this.setPage();
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
		this.isLoading = true;
		//
		this._orderService.getAll(this.page, 'order', this.status, this.dataFilter, this.sortData).subscribe((res) => {
			
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
}
