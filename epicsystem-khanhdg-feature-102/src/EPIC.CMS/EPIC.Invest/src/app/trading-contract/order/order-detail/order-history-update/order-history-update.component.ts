
import { Component, ElementRef, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrderConst, ProductBondInfoConst, StatusCoupon, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { OrderService } from '@shared/services/order.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-order-history-update',
  templateUrl: './order-history-update.component.html',
  styleUrls: ['./order-history-update.component.scss']
})
export class OrderHistoryUpdateComponent extends CrudComponentBase {

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
	couponInfo: any = {};
	page = new Page();

	tableHeight: number;
	ProductBondInfoConst = ProductBondInfoConst;

	rows: any[] = [];

	productBondInfoConst = ProductBondInfoConst;
	StatusCoupon = StatusCoupon;

	columns: IColumn[] = [];

	@Input() contentHeight: number = 0;

	ngOnInit() {
		this.columns = [
			{ field: 'stt', header: 'STT', width: 4, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT },
			{ field: 'fieldNameDisplay', header: 'Trường thay đổi', width: 14, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'summary', header: 'Mô tả', width: 16, isResize: true },
			{ field: 'oldValue', header: 'Dữ liệu cũ', width: 16, isResize: true, class: 'justify-content-end text-right text-right' },
			{ field: 'newValue', header: 'Dữ liệu mới', width: 16, isResize: true, class: 'justify-content-end text-right'},
			{ field: 'createdDate', header: 'Thời gian', width: 11 },
			{ field: 'createdBy', header: 'Tác nhân', width: 15 },
			// { field: 'actionInfo', header: 'Thông tin sửa', width: 12 },
		];
		//
		this.setPage();
	}
	
	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._orderService.getHistory(this.page,this.orderId).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items;
				this.setData(this.rows);
			}
		}, () => {
			this.isLoading = false;
		});
	}

	setValueData(row, value){
		if (row.fieldName == StatusCoupon.SO_TIEN_DAU_TU){
			return this.utils.transformMoney(+value);
		}

		if (row.fieldName == StatusCoupon.NGUON){
			return OrderConst.getInfoSource(value, 'name');
		}

		return value
	}

	setData(rows) {
		this.rows = rows.map((row,index) => {
			row.stt = index + 1;
			row.fieldNameDisplay = StatusCoupon.getFieldName(row.fieldName);
			row.oldValue = this.setValueData(row, row.oldValue);
			row.newValue = this.setValueData(row, row.newValue);
			row.createdDate = this.formatDateTime(row.createdDate);
			// row.actionInfo = `${this.formatDateTime(row.createdDate)} <br> ${row.createdBy}`
			return row;
		})
	}

}

