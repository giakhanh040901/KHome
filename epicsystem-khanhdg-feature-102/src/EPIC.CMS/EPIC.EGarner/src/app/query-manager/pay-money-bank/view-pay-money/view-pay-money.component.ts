import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { CollectMoneyBankConst, ContractFormConst, PolicyDetailTemplateConst, YesNoConst } from '@shared/AppConsts';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PolicyTemplateService } from '@shared/services/policy-template.service';
import { ContractFormService } from '@shared/services/contract-form.service';
import { Page } from '@shared/model/page';
import { QueryMoneyBankService } from '@shared/services/query-bank.service';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';

@Component({
	selector: 'app-view-pay-money',
	templateUrl: './view-pay-money.component.html',
	styleUrls: ['./view-pay-money.component.scss']
})
export class ViewPayMoneyComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _contractFormService: ContractFormService,
		private dialogService: DialogService,
		public configDialog: DynamicDialogConfig,
		public ref: DynamicDialogRef,
		// public confirmationService: ConfirmationService,
		private _queryService: QueryMoneyBankService,
	) {
		super(injector, messageService);
	}
	item: any;
	rows: any = [];
	_selectedColumns: any[];
	cols: any[];
	CollectMoneyBankConst = CollectMoneyBankConst;

	ngOnInit(): void {

		if (this.configDialog?.data?.item) {
			this.item = this.configDialog?.data?.item;
			this.get();
		}
		this.cols = [
			// { field: 'napasTransId', header: 'Napas GD', width: '9rem', isPin: false, isResize: true, class: '' },
			{ field: 'amountMoneyDisplay', header: 'Số tiền', width: '8rem', isPin: false, isResize: false, class: '' },
			{ field: 'receiveAccount', header: 'Tài khoản nhận', width: '12rem', isPin: false, isResize: false, class: '' },
			{ field: 'receiveName', header: 'Người nhận', width: '12rem', isPin: false, isResize: false, class: '' },
			{ field: 'rrn', header: 'rrn', width: '12rem', isPin: false, isResize: false, class: '' },
			{ field: 'senderAccount', header: 'Tài khoản người gửi', width: '15rem', isPin: false, isResize: false, class: '' },
			{ field: 'columnResize', header: '', type: 'hidden' },
		];
		//
		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		// this._selectedColumns = this.cols;
		this._selectedColumns = [...(this.getLocalStorage('payMoneyBankDetailGan') ?? this.cols)];
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'payMoneyBankDetailGan');
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.contractCode = row?.investOrder?.contractCode;
			row.dataTypeDisplay = CollectMoneyBankConst.getTypes(row?.dataType);
			row.amountMoneyDisplay = this.formatCurrency(row?.amount);
			row.approveDateDisplay = this.formatDateTimeSecond(row.investOrder?.approveDate);
			row.responseDate = this.formatDateTimeSecond(row.responseDate);
			row.createdDate = this.formatDateTimeSecond(row.createdDate);
		}
	}

	get() {
		this._queryService.getView(this.item).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res)) {
				this.rows = res?.data
				this.showData(this.rows)
			}
			console.log({ resDistribution: res });
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		}
		);
	}


	close() {
		this.ref.close();
	}
}




