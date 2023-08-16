import { Component, Injector, Input, OnInit } from '@angular/core';
import { EPositionFrozenCell, EPositionTextCell, ETypeDataTable, ETypeFormatDate, HistoryConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IHeaderColumn, IValueFormatter } from '@shared/interface/InterfaceConst.interface';
import { PrizeDrawService } from '@shared/services/prize-draw.service';
import { MessageService } from 'primeng/api';

@Component({
	selector: 'app-history-update-prize-draw',
	templateUrl: './history-update-prize-draw.component.html',
	styleUrls: ['./history-update-prize-draw.component.scss'],
})
export class HistoryUpdatePrizeDrawComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _prizeDrawService: PrizeDrawService,
	) {
		super(injector, messageService);
	}

	HistoryConst = HistoryConst;
	ETypeFormatDate = ETypeFormatDate;
	
	@Input() prizeDrawId: number = null;
	
	headerColumns: IHeaderColumn[] = [];
	rows: any[] = [];


	ngOnInit(): void {
		this.setpage();
		this.headerColumns = [
			{
				field: 'id',
				header: '#ID',
				width: '5rem',
				isPin: true,
				type: ETypeDataTable.INDEX,
				posTextCell: EPositionTextCell.CENTER,
				isFrozen: true,
				posFrozen: EPositionFrozenCell.LEFT,
			},
			{
				field: 'summary',
				header: 'Thông tin thay đổi',
				width: '15rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'oldValue',
				header: 'Dữ liệu cũ',
				width: '15rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'newValue',
				header: 'Dữ liệu mới',
				width: '15rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'actionDisplay',
				header: 'Loại hình',
				width: '8rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'createdDate',
				header: 'Thời gian chỉnh sửa',
				width: '12rem',
				type: ETypeDataTable.TEXT,
				valueFormatter: (param: IValueFormatter) => (param.data ? formatDate(param.data, ETypeFormatDate.DATE) : ''),
			},
			{
				field: 'createdBy',
				header: 'Người chỉnh sửa',
				width: '15rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
		];



	}

	setData(rows = []){
		for(let row of rows){
			row.actionDisplay = HistoryConst.getActionName(row?.action);
		}
	}

	setpage(){
		if(this.prizeDrawId){
			this.isLoading = true;
			this._prizeDrawService.getById(this.prizeDrawId).subscribe((res) => {
				this.isLoading = false;
				if(this.handleResponseInterceptor(res) && res?.data) {
					this.rows = res?.data?.historyUpdates;
					this.setData(this.rows);
				} 
			}, () => {
				this.isLoading = false;
			});
		}
	}
}
