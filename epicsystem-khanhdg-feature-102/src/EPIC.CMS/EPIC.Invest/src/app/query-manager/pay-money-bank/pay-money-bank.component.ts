import { Component, Injector } from '@angular/core';
import { CollectMoneyBankConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { QueryMoneyBankService } from '@shared/services/query-bank.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ViewPayMoneyComponent } from './view-pay-money/view-pay-money.component';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { QueryPaymentBankFilter } from '@shared/interface/filter.model';

@Component({
	selector: 'app-pay-money-bank',
	templateUrl: './pay-money-bank.component.html',
	styleUrls: ['./pay-money-bank.component.scss']
})
export class PayMoneyBankComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private _queryService: QueryMoneyBankService
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Chi tiền ngân hàng" },
		]);
	}

	// ACTION BUTTON
	listAction: any[] = [];

	// Data Init
	YesNoConst = YesNoConst;
	CollectMoneyBankConst = CollectMoneyBankConst;

	dataFilter: QueryPaymentBankFilter = new QueryPaymentBankFilter();
	page = new Page();

	rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();
	
	ngOnInit(): void {
		this.columns = [
			{ field: 'id', isSort: true, header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left'},
			{ field: 'requestId', isSort: true, header: 'ID Lô', width: 8, isPin: false },
			{ field: 'dataType', isSort: true, header: 'Loại chi', width: 12, isPin: false },
			{ field: 'contractCode', isSort: true, header: 'Mã hợp đồng', width: 11, isPin: false, isResize: true },
			{ field: 'createdDate', isSort: true, header: 'Gửi yêu cầu', width: 12, isPin: false},
			{ field: 'responseDate', isSort: true, header: 'Phản hồi', width: 12, isPin: false},
			{ field: 'amountMoney', isSort: true, header: 'Số tiền', width: 12, isPin: false, type: TableConst.columnTypes.CURRENCY },
			{ field: 'ownerAccountNo', isSort: true, header: 'Tài khoản nhận', width: 14, isPin: false},
			{ field: 'note', isSort: true, header: 'Nội dung chuyển', width: 20, isPin: false },
			{ field: 'exception', isSort: true, header: 'Nội dung lỗi', width: 25, isPin: false },
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true,type: TableConst.columnTypes.STATUS, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];
		//
		this.setPage();
	}

	showData(rows) {
		for (let row of rows) {
			row.requestId = `epic_${row?.requestId}`
			row.dataType = CollectMoneyBankConst.getTypes(row?.dataType);
			row.createdDate = this.formatDateTimeSecond(row.createdDate);
			row.responseDate = this.formatDateTimeSecond(row.responseDate);
			row.contractCode = row?.investOrder?.genContractCode || row?.investOrder?.contractCode;
			row.statusElement = CollectMoneyBankConst.getStatusInfo(row?.status);
		}
	}

	onSort(event) {
		this.dataFilter.sortFields = event;
		this.setPage();
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._queryService.getAllQueryBank(this.page, 'request-pay', this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res)) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res.data.items;
				if (this.rows?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows);
				}
			}
			console.log({ resDistribution: res });
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(distribution => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT])) {
				actions.push({
					data: distribution,
					label: 'Truy vấn ngân hàng',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data);
					}
				});
			}
			//
		
			//
			return actions;
		});
	}

	view(item) {
		const ref = this.dialogService.open(
			ViewPayMoneyComponent,
			{
				header: 'Truy vấn ngân hàng',
				width: '1000px',
				contentStyle: { "padding-bottom": "10px", },
				data: {
					item: item,
				}
			}
		);
		
		ref.onClose.subscribe((distribution) => {
			console.log('dataCallBack', distribution);
		});
	}
}