import { Component, Injector, OnInit } from "@angular/core";
import { CollectMoneyBankConst, DistributionConst, ProductConst, ProductPolicyConst, SearchConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DynamicDialogRef, DialogService } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { Router } from "@angular/router";
import { OJBECT_DISTRIBUTION_CONST } from '@shared/base-object';
import { DistributionService } from '@shared/services/distribution.service';
import { FormSetDisplayColumnComponent } from "src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { FormRequestComponent } from "src/app/form-general/form-request-approve-cancel/form-request/form-request.component";
import { FormApproveComponent } from "src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component";
import { CollectMoneyBankService } from "@shared/services/collect-money-bank.service";
import { QueryMoneyBankService } from "@shared/services/query-bank.service";
import * as moment from "moment";

const { POLICY, POLICY_DETAIL, BASE } = OJBECT_DISTRIBUTION_CONST;

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
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		//
		private _collectMoneyBankService: CollectMoneyBankService,
		private _queryService: QueryMoneyBankService,

	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Thu tiền ngân hàng" }
		]);
	}

	rows: any[] = [];
	row: any;
	col: any;
	_selectedColumns: any[];

	// ACTION BUTTON
	listAction: any[] = [];

	// Data Init
	YesNoConst = YesNoConst;
	CollectMoneyBankConst = CollectMoneyBankConst;

	///////
	status: any;
	queryDate: Date;

	statusSearch = [...CollectMoneyBankConst.statusConst];

	cols: any[];

	fieldDates = ["openCellDate", "closeCellDate"];
	isClose = '';
	
	page = new Page();
	offset = 0;

	fieldCheckBoxs = [];

	ngOnInit() {
		//
		this.setPage({ page: this.offset });
		//
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		//
		this.fieldCheckBoxs = ['isCheck', 'isShowApp', 'isClose'];
		this.cols = [
			{ field: 'vaNumber', header: 'Mã giao dịch', width: '12rem', isPin: false, isResize: false, class: '' },
			{ field: 'genContractCode', header: 'Mã hợp đồng', width: '11rem', isPin: false, isResize: true, class: 'b-cut-text-11' },
			{ field: 'transferDateDisplay', header: 'Gửi yêu cầu', width: '12rem', isPin: false, isResize: false, class: ''},
			{ field: 'createdDateDisplay', header: 'Phản hồi', width: '12rem', isPin: false, isResize: false, class: ''},
			{ field: 'tranAmountDisplay', header: 'Số tiền', width: '10rem', isPin: false, isResize: false, class: ''},
			{ field: 'tranRemark', header: 'Nội dung chuyển', width: '30rem', isPin: false, isResize: false, class: '' },
			{ field: 'exception', header: 'Nội dung lỗi', width: '22rem', isPin: false, isResize: false, class: '' },
			{ field: 'ip', header: 'IP', width: '10rem', isPin: false, isResize: false, class: ''},
			{ field: 'columnResize', header: '', type:'hidden' },
		];
		//
		this.cols = this.cols.map((item,index) => {
			item.position = index + 1;
			return item;
		});
		// this._selectedColumns = this.cols;
		this._selectedColumns = [...(this.getLocalStorage('collectMoneyBankGan') ?? this.cols)];
		console.log('_selectedColumns', this._selectedColumns);
		
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
				this.setLocalStorage(this._selectedColumns, 'collectMoneyBankGan');
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.createdDateDisplay = this.formatDateTimeSecond(row?.createdDate);
			row.transferDateDisplay = this.formatDateTimeSecond(row?.transferDate);		
			row.tranAmountDisplay = this.formatCurrency(+row?.tranAmount);
			row.genContractCode = row?.genContractCode || row?.garnerOrder?.contractCode;
			// row.approveDateDisplay = this.formatDateTimeSecond(row.garnerOrders[0].approveDate)
		}
		console.log('showData', rows);
	}

	changeQueryDate(){
		this.setPage();
		
	}
	
	changeStatus() {
		this.setPage();
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		if(this.queryDate){
			var queryDate = moment(this.queryDate).format('YYYY-MM-DD')
		}
		//
		this._queryService.getAllCollectMoneyBank(this.page, this.status, queryDate).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) ) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res.data.items;
				if(this.rows?.length) { 
					// this.genListAction(this.rows);
					this.showData(this.rows);
				}
			}
			console.log({ resDistribution: res });
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			}
		);
	}

	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(distribution => {
			const actions = [];

			if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ThongTinPPSP])) {
				actions.push({
					data: distribution,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data?.id);
					}
				});
			}
			//
		
			//
			return actions;
		});
	}
  
	view(distributionId) {
		this.router.navigate(["product-manager/distribution/detail/" + this.cryptEncode(distributionId)]);
	}

}

