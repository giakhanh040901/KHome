import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { ApproveConst, StatusPaymentBankConst, OrderConst, DistributionConst, AtributionConfirmConst, TableConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { WithdrawalService } from "@shared/services/withdrawal-service";
import { ListRequestBankComponent } from "./list-request-bank/list-request-bank.component";
import { TradingProviderSelectedService } from "@shared/services/trading-provider-selected.service";
import { Subscription } from "rxjs";
import { DataTableEmit, IAction, IColumn } from "@shared/interface/p-table.model";
import { OrderWithdrawFilter } from "@shared/interface/filter.model";

@Component({
	selector: 'app-manager-withdraw',
	templateUrl: './manager-withdraw.component.html',
	styleUrls: ['./manager-withdraw.component.scss']
})

export class ManagerWithdrawComponent extends CrudComponentBase {

	// CONST
	StatusPaymentBankConst = StatusPaymentBankConst;
	ApproveConst = ApproveConst;
	OrderConst = OrderConst;
	DistributionConst = DistributionConst;
	//
	rows: any[] = [];
	columns: IColumn[] = [];
	listAction: IAction[][] = [];
	page = new Page();

	dataFilter: OrderWithdrawFilter = new OrderWithdrawFilter();
	dataTableEmit: DataTableEmit = new DataTableEmit();

	isPartner: boolean;
	tradingProviderSub: Subscription;

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private confirmationService: ConfirmationService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private _withdrawalService: WithdrawalService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: 'Hợp đồng phân phối' },
			{ label: 'Xử lý rút tiền' },
		]);
	}

	ngOnInit(): void {
		this.isPartner = this.getIsPartner();
		this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.dataFilter.tradingProviderIds = change;
			this.setPage();
		})
		//
		this.columns = [
			{ field: '', header: '', width: 3, isPin: true, displaySettingColumn: false, type: TableConst.columnTypes.CHECKBOX_ACTION, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
			{ field: 'order.contractCode', header: 'Mã hợp đồng', width: 10, isPin: true, isResize: true, isSort: true },
			{ field: 'project.invCode', header: 'Sản phẩm', width: 18, isPin: true, isSort: true },
			{ field: 'order.investDate', header: 'Ngày đầu tư', width: 10, isSort: true, type: TableConst.columnTypes.DATE, },
			{ field: 'withdrawalIndex', header: 'Lần rút vốn', width: 10, isSort: true },
			{ field: 'withdrawalDate', header: 'Ngày yêu cầu', width: 11, isSort: true, type: TableConst.columnTypes.DATE, },
			{ field: 'amountMoney', header: 'Số tiền rút', width: 9, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'order.totalValue', header: 'Tiền đầu tư', width: 10, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'order.initTotalValue', header: 'Đầu tư BĐ', width: 10, title: 'Số tiền đầu tư ban đầu', type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'profit', header: 'Lợi tức rút', width: 8.5, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'deductibleProfit', header: 'Lợi tức KT', width: 8.5, title: 'Lợi tức khấu trừ', type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'tax', header: 'Thuế TNCN', width: 10, title: 'Thuế thu nhập cá nhân', type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'actuallyAmount', header: 'Thực nhận', width: 10, type: TableConst.columnTypes.CURRENCY, isSort: true },
			{ field: 'createdBy', header: 'Người yêu cầu', width: 12, isSort: true },
			{ field: 'approveDate', header: 'Ngày duyệt', width: 11, isSort: true, type: TableConst.columnTypes.DATETIME, },
			{ field: 'approveBy', header: 'Người duyệt', width: 15, isSort: true },
			{ field: 'statusBank', isShow: false, displaySettingColumn: false, header: 'Kết quả chi', width: 9, type: TableConst.columnTypes.STATUS, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
			{ field: 'status', header: 'Trạng thái', width: 8, type: TableConst.columnTypes.STATUS,isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end'},
		];
	}

	ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}
	
	showData(rows) {
		for (let row of rows) {
			row['order.initTotalValue'] = row?.order?.initTotalValue;
			row['project.invCode'] = row?.project?.invName,
			row.projectCode = row?.project?.invCode,
			row.requestDate = row?.requestDate,
			row['order.contractCode'] = row?.order?.genContractCode || row?.order?.contractCode;
			row['order.investDate'] = row?.order?.investDate;
			row['order.totalValue'] = row?.order?.totalValue;
			//
			row.statusElement = StatusPaymentBankConst.getInfo(row?.status);
			row.statusBankElement = StatusPaymentBankConst.getResponseInfo(row?.statusBank);
		};
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestHDPP_XLRT_ThongTinDauTu])) {
				actions.push({
					data: item,
					label: "Thông tin chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail($event.item.data);
					},
				});
			}
			return actions;
		});
	}

	detail(item) {
		let cryptEncodeId = encodeURIComponent(this.cryptEncode(item?.orderId));  
		window.open('/trading-contract/order/detail-view/' + (cryptEncodeId), "_blank");
	}

	onSort(event) {
		this.sortData = event;
		this.setPage();
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.dataTableEmit.selectedItems = [];
		this.isLoading = true;
		this._withdrawalService.getAll(this.page, this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				const resOrder = res?.data?.items;
				if(this.page.pageSize === this.page.pageSizeAll) {
					// LOAD MORE DATA
					if(this.page.pageNumberLoadMore === 1) this.rows = [];
					this.rows = [...this.rows, ...resOrder];
				} else {
					this.rows = resOrder;
				}
				let startIndex = this.page.pageNumber*this.page.pageSize;
				this.rows = resOrder.map((item, index) => {
					const rowIndex = this.page.pageSize === this.page.pageSizeAll ? index : ++startIndex;
					return {...item, index: rowIndex }
				});
				if (this.rows?.length) {
					this.showData(this.rows);
					this.genListAction(this.rows);
				}
			}
		},(err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			}
		);
	}

	changeStatus(status) {
		// CHECKBOX CHỈ TICK ĐC KHI Ở DANH SÁCH LỌC TRẠNG TRÁI CHỜ XỬ LÝ
		this.columns[0].type = StatusPaymentBankConst.PENDING === status ? TableConst.columnTypes.CHECKBOX_ACTION : TableConst.columnTypes.CHECKBOX_SHOW;
		// CỘT KẾT QUẢ CHI CHỈ HIỆN KHI DANH SÁCH LỌC THEO TRẠNG THÁI CHI TỰ ĐỘNG
		let indexStatusBank = this.columns.findIndex(c => c.field == 'statusBank');
		this.columns[indexStatusBank].isShow = (StatusPaymentBankConst.APPROVE_ONLINE === status);
		this.columns[indexStatusBank].isShow = (StatusPaymentBankConst.APPROVE_ONLINE === status);
		//REFRESH COLUMNS
		let columnsCache = [...this.columns];
		this.columns = [];
		setTimeout(() => this.columns = columnsCache);
		
		this.setPage();
	}

	createList() {
		this.dialogService.open(
			ListRequestBankComponent,
			{
				header: "Gửi yêu cầu chi tiền",
				width: '850px',
				data: {
					requests: [...this.dataTableEmit.selectedItems],
					interestType: StatusPaymentBankConst.MANAGER_WITHDRAW,
					typeContract: 'ChiTraHopDong',
					distributionId: this.dataTableEmit.selectedItems[0].order.distributionId,
				}
			}).onClose.subscribe((statusResponse) => {
				if(statusResponse) {
					this.setPage();
				}
			}
		);
	}

	cancelApprove() {
		this.approve(
			StatusPaymentBankConst.CANCEL,
			'Xác nhận hủy duyệt các yêu cầu đã chọn?',
			'Hủy yêu cầu thành công!'
		);
	}

	approveOffline() {
		this.approve(
			StatusPaymentBankConst.APPROVE_OFFLINE,
			'Xác nhận duyệt chi thủ công?',
			'Duyệt chi thành công!'
		);
	}

	approve(statusApprove, messageConfirm, messageNotify) {
		let body = {
			withdrawalIds: this.dataTableEmit.selectedItems.map(s => s.id),
			status: statusApprove,
		}
		//
		this.confirmationService.confirm({
			message: messageConfirm,
			...AtributionConfirmConst,
			accept: () => {
				this.isLoading = true;
				this._withdrawalService.approve(body, StatusPaymentBankConst.MANAGER_WITHDRAW).subscribe((res) => {
					this.isLoading = false;
					if(this.handleResponseInterceptor(res, messageNotify)) {
						this.setPage();
					}
				}, (err) => {
					console.log('err__', err);
					this.isLoading = false;
				});			
			}
		});
	}

	onRowSelect() {
		// Kiểm tra các phần tử được chọn phải cùng distributionId, Nếu khác distribution thì sẽ lấy theo phần tử đc chọn sau cùng
		if(this.dataFilter.methodInterest === DistributionConst.CO_CHI_TIEN) {
			let selectedLength = this.dataTableEmit.selectedItems.length;
			if(selectedLength > 1) {
				let rowLast = this.dataTableEmit.selectedItems[selectedLength - 1];
				let groupDistributionForItemLast = this.dataTableEmit.selectedItems.filter(s => s.order?.distributionId === rowLast.order?.distributionId);
				let isExitsDistributionOther = selectedLength === groupDistributionForItemLast.length;  
				if(!isExitsDistributionOther) {
					setTimeout(() => this.dataTableEmit.selectedItems = [...groupDistributionForItemLast]);
				}
			}
		}
		
	}
}
