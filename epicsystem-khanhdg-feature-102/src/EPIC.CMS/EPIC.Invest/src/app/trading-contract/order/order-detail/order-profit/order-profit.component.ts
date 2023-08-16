import { Component, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductBondInfoConst, ProductBondDetailConst, StatusCoupon, StatusActualCashFlow, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { OrderService } from '@shared/services/order.service';
import { MessageService } from 'primeng/api';

@Component({
	selector: 'app-order-profit',
	templateUrl: './order-profit.component.html',
	styleUrls: ['./order-profit.component.scss']
})
export class OrderProfitComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private _orderService: OrderService,

	) {
		super(injector, messageService);
		this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	orderId: number;
	@Input() orderDetail: any = {};
	@Input() contentHeight: number;

	ProductBondInfoConst = ProductBondInfoConst;
	StatusActualCashFlow = StatusActualCashFlow;

	expectedCashFlows = [];
	actualCashFlows = [];

	StatusCoupon = StatusCoupon;

	columnExpecteds: IColumn[] = [];
	columnActuals: IColumn[] = [];

	ngOnInit() {
		this.columnExpecteds = [
			{ field: 'stt', header: 'STT', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'kyNhan', header: 'Kỳ nhận', width: 12, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'initTotalValue', header: 'Số tiền ĐT', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right text-right' },
			{ field: 'profitRate', header: 'Tỷ lệ lợi tức', width: 9, class: 'justify-content-end text-right'},
			{ field: 'payDate', header: 'Ngày trả', width: 8, type: TableConst.columnTypes.DATE},
			{ field: 'profit', header: 'Lợi tức', width: 10, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'tax', header: 'Thuế TN', width: 10, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'actuallyProfit', header: 'Tiền thực nhận', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'numberOfDays', header: 'Số ngày', width: 8, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: '', header: '', width: 0, isResize: true },
			{ field: 'status', header: 'Trạng thái', width: 10, type: TableConst.columnTypes.CONVERT_DISPLAY, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-left b-border-frozen-right' },
		];
		//
		this.columnActuals = [
			{ field: 'stt', header: 'STT', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'description', header: 'Nội dung', width: 12, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'payDate', header: 'Ngày trả', width: 9, type: TableConst.columnTypes.DATE },
			{ field: 'surplus', header: 'Số dư trước khi rút', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'withdrawalMoney', header: 'Tiền rút', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'numberOfDays', header: 'Số ngày', width: 6, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'profit', header: 'Lợi tức rút', width: 9, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'tax', header: 'Thuế rút', width: 9, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'deductibleProfit', header: 'Lợi tức khấu trừ', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: 'actuallyAmount', header: 'Tiền thực nhận', width: 12, type: TableConst.columnTypes.CURRENCY, class: 'justify-content-end text-right' },
			{ field: '', header: '', width: 0, isResize: true },
			{ field: 'status', header: 'Trạng thái', width: 10, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
		];
		this.getData();
	}

	getData() {
		this.isLoading = true;
		this._orderService.getCoupon(this.orderId).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.expectedCashFlows = res?.data?.expectedCashFlow?.profitInfo ?? [];
				this.setDataExpected(this.expectedCashFlows);
				this.actualCashFlows = res?.data?.actualCashFlow ?? [];
				this.setDataActual(this.actualCashFlows);
			}
		}, () => {
			this.isLoading = false;
		});
	}

	setDataExpected(rows) {
		this.expectedCashFlows = rows.map((row, index) => {
			row.stt = index + 1;
			row.kyNhan = (index !== (rows.length - 1)) ? 'Lợi tức kỳ ' + (index+1) : 'Lợi tức cuối kỳ';
			row.initTotalValue = this.orderDetail?.initTotalValue;
			row.profitRate = this.utils.transformPercent(row.profitRate*100) + '%';
			row.statusDisplay = StatusCoupon.getName(row.status);
			return row;
		}); 
	}

	setDataActual(rows) {
		this.actualCashFlows = rows.map((row, index) => {
			row.stt = index + 1;
			row.statusElement = StatusActualCashFlow.getInfo(row.status);
			return row;
		}); 
	}

}
