import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { CollectMoneyBankConst, TableConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { QueryMoneyBankService } from '@shared/services/query-bank.service';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column.component';
import { IColumn } from '@shared/interface/p-table.model';

@Component({
	selector: 'app-view-pay-money',
	templateUrl: './view-pay-money.component.html',
	styleUrls: ['./view-pay-money.component.scss']
})
export class ViewPayMoneyComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		public configDialog: DynamicDialogConfig,
		public ref: DynamicDialogRef,
		private _queryService: QueryMoneyBankService,
	) {
		super(injector, messageService);
	}
	item: any;
	rows: any = [];
	columns: IColumn[] = [];
	CollectMoneyBankConst = CollectMoneyBankConst;

	ngOnInit(): void {

		if (this.configDialog?.data?.item) {
			this.item = this.configDialog?.data?.item;
			this.get();
		}
		this.columns = [
			{ field: 'transId', header: 'Id GD', width: 9, isPin: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'amount', header: 'Số tiền', width: 10, type: TableConst.columnTypes.CURRENCY, class: '' },
			{ field: 'receiveAccount', header: 'Tài khoản nhận', width: 12, class: '' },
			{ field: 'receiveName', header: 'Người nhận', width: 12, isPin: false, isResize: false, class: '' },
			{ field: 'rrn', header: 'rrn', width: 12, class: '' },
			{ field: 'senderAccount', header: 'Tài khoản người gửi', width: 12, class: '' },
			{ field: 'status', header: 'Trạng thái', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
		];
		
	}
	
	showData(rows) {
		for (let row of rows) {
			row.contractCode = row?.investOrder?.contractCode;
			row.dataTypeDisplay = CollectMoneyBankConst.getTypes(row?.dataType);
			row.approveDateDisplay = this.formatDateTimeSecond(row.investOrder?.approveDate);
			row.responseDate = this.formatDateTimeSecond(row.responseDate);
			row.createdDate = this.formatDateTimeSecond(row.createdDate);
			row.statusElement = CollectMoneyBankConst.getStatusInfo(row.status);
		}
	}

	get() {
		this._queryService.getView(this.item).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res)) {
				this.rows = res?.data;
				this.showData(this.rows)
			}
			console.log({ resDistribution: res });
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}


	close() {
		this.ref.close();
	}
}



