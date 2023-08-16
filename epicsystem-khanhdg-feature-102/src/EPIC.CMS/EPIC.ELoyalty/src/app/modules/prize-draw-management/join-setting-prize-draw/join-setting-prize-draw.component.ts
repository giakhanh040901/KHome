import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { TabView } from 'primeng/tabview';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AddPersonListComponent } from './add-person-list/add-person-list.component';
import { IActionTable, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { AccumulatePointManegement, EPositionFrozenCell, EPositionTextCell, ETypeDataTable, PrizeDrawManagement } from '@shared/AppConsts';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { LuckyProgramInvestorService } from '@shared/services/lucky-program-investor.service';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
	selector: 'app-join-setting-prize-draw',
	templateUrl: './join-setting-prize-draw.component.html',
	styleUrls: ['./join-setting-prize-draw.component.scss'],
})
export class JoinSettingPrizeDrawComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private activatedRouter: ActivatedRoute,
		private _luckyProgramInvestorService: LuckyProgramInvestorService,
		private dialogService: DialogService,
		private _helpersService: HelpersService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home']},
			{ label: "Danh sách chương trình", routerLink: ['/prize-draw-management']},
			{ label: "Cài đặt tham gia"},
		]);
		this.prizeDrawId = +(this.cryptDecode(this.activatedRouter.snapshot.paramMap.get('id')));
	}
	
	prizeDrawId: number;

	PrizeDrawManagement = PrizeDrawManagement;

	@ViewChild(TabView) tabView: TabView;
	tabViewActive = {
		danhSachThamGia: true,
		// cauHinhChuongTrinh: false,
		// lichSu: false,
	}

	filter = {
		keyword: '',
		isJoin: null
	}

	headerColumns: IHeaderColumn[] = [];
	_selectedColumns: any[];
	selectedColumns: IHeaderColumn[] = [];
	listAction: IActionTable[][] = [];

// 
	rows: any[] = [];

	selectedItems: any[] = [];

	ngOnInit(): void {
		console.log('!!! prizeDrawId ', this.prizeDrawId);
		this.headerColumns = [
			// { 
			// 	field: '', 
			// 	header: '', 
			// 	width: '3rem', 
			// 	displaySettingColumn: false, 
			// 	isPin: true, 
			// 	isFrozen: true, 
			// 	posFrozen: EPositionFrozenCell.LEFT, 
			// 	type: ETypeDataTable.CHECKBOX_ACTION 
			// },
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
				field: 'fullname',
				header: 'Tên khách hàng',
				width: '20rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'phone',
				header: 'SĐT',
				width: '12rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'idNo',
				header: 'Số giấy tờ',
				width: '15rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'rankName',
				header: 'Hạng thành viên',
				width: '12rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'totalPoint',
				header: 'Điểm hiện tại',
				width: '10rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{
				field: 'voucherNum',
				header: 'Số voucher đang có',
				width: '12rem',
				isPin: true,
				type: ETypeDataTable.TEXT,
			},
			{ 
				field: '', 
				header: '', 
				width: '', 
				displaySettingColumn: false, 
				isPin: true,
				type: ETypeDataTable.TEXT 
			},
			{
				field: 'isJoin',
				header: 'Trạng thái',
				width: '8rem',
				type: ETypeDataTable.STATUS,
				funcStyleClassStatus: this.funcStyleClassStatus,
				funcLabelStatus: this.funcLabelStatus,
				posTextCell: EPositionTextCell.CENTER,
				isFrozen: true,
				posFrozen: EPositionFrozenCell.RIGHT,
				class: 'b-border-frozen-right',
			},
			{
				field: '',
				header: '',
				width: '3rem',
				type: ETypeDataTable.ACTION,
				posTextCell: EPositionTextCell.CENTER,
				isFrozen: true,
				posFrozen: EPositionFrozenCell.RIGHT,
			},
		].map((item: IHeaderColumn, index: number) => {
			item.position = index + 1;
			return item;
		});
		this.selectedColumns = this.getLocalStorage('joinSettingPrizeDraw') ?? this.headerColumns;
		this.setPage({ page: this.offset });
	}

	public funcStyleClassStatus = (status: any) => {
		return this.getStatusSeverity(status);
	};
	
	public funcLabelStatus = (status: any) => {
		return this.getStatusName(status);
	};

	public getStatusSeverity(code: any) {
		return PrizeDrawManagement.getStatus(code, 'severity');
	}
	
	public getStatusName(code: any) {
		return PrizeDrawManagement.getStatus(code, 'label');
	}

	changeTab(event) {
        let tabHeader = this.tabView.tabs[event.index].header;
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
    }

	public setColumn(event: any) {
		if (event) {
			const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(this.headerColumns, this.selectedColumns)
		  );
		  ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
			  this.selectedColumns = dataCallBack.data.sort(function (a, b) {
				return a.position - b.position;
			  });
			  this.setLocalStorage(this.selectedColumns, AccumulatePointManegement.keyStorage);
			}
		  });
		}
	  }


	create(){
		const ref = this.dialogService.open(AddPersonListComponent, {
			header: 'Thiết lập danh sách tham gia',
			width: '1000px',
			data: {
				prizeDrawId: this.prizeDrawId
			}
		});
		ref.onClose.subscribe((response: any) => {
			if (response) {
				this.messageSuccess("Đã lưu thành công");
				this.setPage({ page: this.offset });
			}
		});
	}

	genListAction(){
		this.listAction = this.rows.map((data: any, index: number) => {
			const actions: IActionTable[] = [];
			actions.push({
				data: data,
				label: 'Xoá',
				icon: 'pi pi-trash',
				command: ($event) => {
				  this.delete($event.item.data);
				},
			  });
			return actions;
		})
	}

	delete(data){
		const ref = this._helpersService.dialogConfirmRef(
			"Xác nhận Xoá khách hàng?",
			IconConfirm.DELETE,
		)
		ref.onClose.subscribe((accept: boolean) => {
			if(accept) {
				this._luckyProgramInvestorService.delete(data.id).subscribe(
					(response) => {
					  if (this.handleResponseInterceptor(response, 'Xoá thành công!')) {
						this.setPage({ page: this.offset });
					  }
					}
				);
			}
		})	
	}
	setData(rows = []){
		for(let row of rows){
			row.fullname = row.fullname ?? row.username;
		}
	}

	deleteInvestor(){
		console.log('selectedItems ', this.selectedItems);
		
	}

	setPage(pageInfo?: any){
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._luckyProgramInvestorService.getAll(this.prizeDrawId, this.page, this.filter).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res)){
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items.map(item => {
					item.status = item.isJoin;
					return item;
				});

				if (this.rows?.length) {
					this.genListAction();
					this.setData(this.rows);
				}
			}
		}, () =>{
			this.isLoading = false;
		})
	}
}
