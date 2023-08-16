import { Component, Injector, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CollectMoneyBankConst, SearchConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { QueryMoneyBankService } from "@shared/services/query-bank.service";
import * as moment from "moment";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormSetDisplayColumnComponent } from "src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { ViewPayMoneyComponent } from "./view-pay-money/view-pay-money.component";

@Component({
  selector: "app-pay-money-bank",
  templateUrl: "./pay-money-bank.component.html",
  styleUrls: ["./pay-money-bank.component.scss"],
})
export class PayMoneyBankComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
	private _queryService: QueryMoneyBankService
  ) //
  // private _transactionService: TransactionService
  {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Chi tiền ngân hàng" },
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

	cols: any[];

	fieldDates = ["openCellDate", "closeCellDate"];
	isClose = "";


	status: any;
	queryDate: Date;
 
	statusSearch = [
		...CollectMoneyBankConst.statusConst
	];

  page = new Page();
  offset = 0;

  ngOnInit(): void {
	
	this.setPage({ page: this.offset });
	//
	this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
		if (this.keyword === "") {
			this.setPage({ page: this.offset });
		} else {
			this.setPage();
		}
	});

	this.cols = [
		{ field: 'requestIdDisplay', header: 'ID Lô', width: '8rem', isPin: false, isResize: false, class: '' },
		{ field: 'dataTypeDisplay', header: 'Loại chi', width: '8rem', isPin: false, isResize: false, class: '' },
		{ field: 'contractCodeDisplay', header: 'Mã hợp đồng', width: '11rem', isPin: false, isResize: true, class: 'b-cut-text-11' },
		{ field: 'createdDateDisplay', header: 'Gửi yêu cầu', width: '12rem', isPin: false, isResize: false, class: ''},
		{ field: 'responseDateDisplay', header: 'Phản hồi', width: '12rem', isPin: false, isResize: false, class: ''},
		{ field: 'amountMoneyDisplay', header: 'Số tiền', width: '15rem', isPin: false, isResize: false, class: ''},
		{ field: 'bankName', header: 'Ngân hàng nhận', width: '10rem', isPin: false, isResize: false, class: ''},
		{ field: 'ownerAccountNo', header: 'Tài khoản nhận', width: '10rem', isPin: false, isResize: false, class: ''},
		{ field: 'note', header: 'Nội dung chuyển', width: '22rem', isPin: false, isResize: false, class: '' },
		{ field: 'exception', header: 'Nội dung lỗi', width: '30rem', isPin: false, isResize: false, class: ''},
		{ field: 'columnResize', header: '', type:'hidden' },
	];
	//
	this.cols = this.cols.map((item,index) => {
		item.position = index + 1;
		return item;
	});
	// this._selectedColumns = this.cols;
	this._selectedColumns = [...(this.getLocalStorage('payMoneyBankGan') ?? this.cols)];
	console.log('_selectedColumns', this._selectedColumns);
  }

  showData(rows) {
	for (let row of rows) {
			row.requestIdDisplay = `epic_${row?.requestId}`
			row.dataTypeDisplay = CollectMoneyBankConst.getTypes(row?.dataType);
			row.contractCodeDisplay = row?.garnerOrders[0]?.genContractCode || row?.garnerOrders[0]?.contractCode;
			row.amountMoneyDisplay = this.formatCurrency(row?.amountMoney);
			row.withdrawalDateDisplay = this.formatDateTimeSecond(row.withdrawalDate);
			row.createdDateDisplay = this.formatDateTimeSecond(row.createdDate);
			row.responseDateDisplay = this.formatDateTimeSecond(row.responseDate);
		}
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
				this.setLocalStorage(this._selectedColumns, 'payMoneyBankGan');
			}
		});
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
		this._queryService.getAllPayMoneyBank(this.page, this.status, queryDate).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res)) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res.data.items;
				if(this.rows?.length) { 
					this.genListAction(this.rows);
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

			if (true) {
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
		console.log("item__",item);
		const ref = this.dialogService.open(
			ViewPayMoneyComponent,
			{
				header: 'Truy vấn ngân hàng',
				width: '1000px',
				contentStyle: { "max-height": "300px"},
				baseZIndex: 10000,
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
