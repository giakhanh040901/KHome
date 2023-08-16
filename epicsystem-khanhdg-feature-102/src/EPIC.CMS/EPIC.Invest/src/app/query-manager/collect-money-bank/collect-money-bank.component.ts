import { Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { CollectMoneyBankConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { QueryCollectionBankFilter } from '@shared/interface/filter.model';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { CollectFilter } from '@shared/interface/query-bank.model';
import { Page } from '@shared/model/page';
import { QueryMoneyBankService } from '@shared/services/query-bank.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-collect-money-bank',
  templateUrl: './collect-money-bank.component.html',
  styleUrls: ['./collect-money-bank.component.scss']
})
export class CollectMoneyBankComponent extends CrudComponentBase {

  constructor(
	injector: Injector,
	messageService: MessageService,
	private router: Router,
	private breadcrumbService: BreadcrumbService,
	private _queryService: QueryMoneyBankService,
  ) {
    super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Thu tiền ngân hàng" }
		]);
	}

	// ACTION BUTTON
	listAction: any[] = [];
	
	// Data Init
	YesNoConst = YesNoConst;
	CollectMoneyBankConst = CollectMoneyBankConst;
	
	dataFilter: QueryCollectionBankFilter = new QueryCollectionBankFilter();
	
	page = new Page();
	rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();

  	ngOnInit(): void {
		this.columns = [
			{ field: 'id', isSort: true, header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left'},
			{ field: 'vaNumber', isSort: true, header: 'Mã giao dịch', width: 12, isPin: false },
			{ field: 'genContractCode', isSort: true, header: 'Mã hợp đồng', width: 11, isPin: false, isResize: true },
			{ field: 'transferDate', isSort: true, header: 'Gửi yêu cầu', width: 12, isPin: false},
			{ field: 'createdDate', isSort: true, header: 'Phản hồi', width: 12, isPin: false},
			{ field: 'tranAmount', isSort: true, header: 'Số tiền', width: 10, isPin: false, type: TableConst.columnTypes.CURRENCY },
			{ field: 'tranRemark', isSort: true, header: 'Nội dung chuyển', width: 22, isPin: false },
			{ field: 'exception', isSort: true, header: 'Nội dung lỗi', width: 22, isPin: false },
			{ field: 'ip', isSort: true, header: 'IP', width: 10, isPin: false},
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true,type: TableConst.columnTypes.STATUS, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
		];
		//
		this.setPage();
  	}

	setData(rows) {
		for (let row of rows) {
			row.createdDate = this.formatDateTimeSecond(row?.createdDate);
			row.transferDate = this.formatDateTimeSecond(row?.transferDate);		
			row.genContractCode = row?.genContractCode || row?.investOrder?.contractCode;	
			row.statusElement = CollectMoneyBankConst.getStatusInfo(row?.status);
		}
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._queryService.getAllQueryBank(this.page, 'collection-payment', this.dataFilter).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) ) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res.data.items;
				if(this.rows?.length) { 
					this.setData(this.rows);
				}
			}
		}, (err) => {
			this.isLoading = false;
		});
	}

	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(distribution => {
			const actions = [];

			if (this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT])) {
				actions.push({
					data: distribution,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data?.id);
					}
				});
			}
			return actions;
		});
	}
  
	view(distributionId) {
		this.router.navigate(["product-manager/distribution/detail/" + this.cryptEncode(distributionId)]);
	}

}
