import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ETypeFormatDate, PrizeDrawManagement } from '@shared/AppConsts';
import { IconConfirm } from '@shared/consts/base.const';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IActionTable, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { Page } from '@shared/model/page';
import { HelpersService } from '@shared/services/helpers.service';
import { PrizeDrawService } from '@shared/services/prize-draw.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
	selector: 'app-prize-draw-management',
	templateUrl: './prize-draw-management.component.html',
	styleUrls: ['./prize-draw-management.component.scss'],
})
export class PrizeDrawManagementComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private router: Router,
		private _prizeDrawService: PrizeDrawService,
		private _helpersService: HelpersService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Chương trình trúng thưởng'},
		  ]);
	}
	
	PrizeDrawManagement = PrizeDrawManagement;
	rows: any[] = [];
	cols: any[];
	_selectedColumns: any[];
	
	submitted: boolean;
	page = new Page();
	listAction: IActionTable[][] = [];

	selectedColumns: IHeaderColumn[] = [];
	headerColumns: IHeaderColumn[] = [];
	filter = {
		keyword: '',
		status: undefined,
		startDate: '',
		endDate: '',
	};

	ngOnInit(): void {
		this.setPage();
		this.cols = [
			{ field: 'code', header: 'Mã chương trình', width: '10rem', isPin: true},
			{ field: 'name', header: 'Tên chương trình', width: '16rem', isPin: true},
			{ field: 'numberLuckyScenario', header: 'Kịch bản', width: '12rem'},
			{ field: 'giftQuantity', header: 'Số lượng quà', width: '10rem', cutText: 'b-cut-text-8' },
			{ field: 'giftQuantityRemain', header: 'Số lượng còn lại', width: '12rem'},
			{ field: 'startDate', header: 'Bắt đầu', width: '11rem'},
			{ field: 'endDate', header: 'Kết thúc', width: '13rem'},
            { field: 'columnResize', header: '', type: 'hidden' },
		];
		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		this._selectedColumns = this.getLocalStorage(PrizeDrawManagement.keyStorage) ?? this.cols;
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
				this.setLocalStorage(this._selectedColumns, PrizeDrawManagement.keyStorage)
			}
		});
	}

	create(){
		this.router.navigate(['/prize-draw-management/prize-draw/create/program-infomation'], { queryParams : { isCreateNew: true }});
	}

	showData(rows = []){
		for(let row of rows){
			row.startDate = row?.startDate ? formatDate( row.startDate, ETypeFormatDate.DATE) : "";
			row.endDate = row?.endDate ? formatDate( row.endDate, ETypeFormatDate.DATE) : "";
		}
	}

	genListAction() {
		this.listAction = this.rows.map((data, index) => {
			const actions: IActionTable[] = [];
			if(this.isGranted([this.PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet])){
				actions.push({
					data: data,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
					  this.detail($event.item.data);
					},
				});
			}

			if(this.isGranted([this.PermissionLoyaltyConst.LoyaltyCT_TrungThuong_CaiDatThamGia])){
				actions.push({
					data: data,
					label: 'Cài đặt tham gia',
					icon: 'pi pi-pencil',
					command: ($event) => {
					this.joinSetting($event.item.data);
					},
				});
			}

			if(data.status == PrizeDrawManagement.KICH_HOAT && this.isGranted([this.PermissionLoyaltyConst.LoyaltyCT_TrungThuong_DoiTrangThai])){
				actions.push({
					data: data,
					label: 'Huỷ kích hoạt',
					icon: 'pi pi-times',
					command: ($event) => {
					  this.cancel($event.item.data);
					},
				});
			}

			if(data.status == PrizeDrawManagement.KICH_HOAT && this.isGranted([this.PermissionLoyaltyConst.LoyaltyCT_TrungThuong_Xoa])){
				actions.push({
					data: data,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
					this.delete($event.item.data);
					},
				});
			}

			return actions;
		})
	}

	detail(item){
		this.router.navigate(['prize-draw-management/detail/' + this.cryptEncode(item.id)]);
	}

	joinSetting(item){
		this.router.navigate(['prize-draw-management/join-setting/' + this.cryptEncode(item.id)]);
	}

	cancel(data) {
		const ref = this._helpersService.dialogConfirmRef(
			"Xác nhận Hủy chương trình trúng thưởng?",
			IconConfirm.QUESTION,
		)
		ref.onClose.subscribe((accept: boolean) => {
			if(accept) {
				this._prizeDrawService.cancel(data.id).subscribe(
					(response) => {
					  if (this.handleResponseInterceptor(response, 'Hủy thành công!')) {
						this.setPage({ page: this.offset });
					  }
					}
				);
			}
		})
	}

	delete(data){
		const ref = this._helpersService.dialogConfirmRef(
			"Xác nhận Xoá chương trình trúng thưởng?",
			IconConfirm.DELETE,
		)
		ref.onClose.subscribe((accept: boolean) => {
			if(accept) {
				this._prizeDrawService.delete(data.id).subscribe(
					(response) => {
					  if (this.handleResponseInterceptor(response, 'Xoá thành công!')) {
						this.setPage({ page: this.offset });
					  }
					}
				);
			}
		})	
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._prizeDrawService.getAll(this.page, this.filter).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				if (this.rows?.length) {
					this.genListAction();
					this.showData(this.rows);
				}
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});

	}

}
