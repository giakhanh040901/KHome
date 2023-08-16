import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApproveConst, SaleConst, SearchConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { CreateSaleComponent } from './create-sale/create-sale.component';
import { SaleService } from '@shared/service-proxies/sale-service';
import { FormRequestComponent } from 'src/app/form-request-approve-cancel/form-request/form-request.component';

@Component({
  selector: 'app-sale-temporary',
  templateUrl: './sale-temporary.component.html',
  styleUrls: ['./sale-temporary.component.scss']
})
export class SaleTemporaryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private _saleService: SaleService,
    ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
    ]);
  }

  SaleConst = SaleConst;
  //
  rows: any[] = [];
	col: any;

	cols: any[];
	_selectedColumns: any[];
  listAction: any[] = [];

  fieldFilter: string;

  dataFilter = {
    field: null,
    saleType: null,
    isInvestor: null,
    source: null,
    status: null,
  }

  saleTypes: any[] = [];
  sources: any[] = [];
  statuses: any[] = [];
  isInvestors: any[] = []
  sortData: any[] = []

  ngOnInit() {
    //
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage(this.keyword);
      }
    });
  this.subject.isSetPage.subscribe(() => {
			this.setPage();
		  })
    this.cols = [
			{ field: 'employeeCode', header: 'Mã nhân viên', width: '12rem', cutText: 'b-cut-text-9', isPin: true, isSort: true },
			{ field: 'fullname', header: 'Họ tên / Tên doanh nghiệp', width: '25rem', isPin: true, isSort: true },
			{ field: 'saleType', header: 'Loại Sale', width: '12rem', isSort: true },
			{ field: 'department.departmentName', header: 'Phòng ban', width: '15rem', isSort: true},
			{ field: 'source', header: 'Nguồn', width: '8rem' },
      { field: 'columnResize', header: '', type:'hidden' },
		];

    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    });

		// this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('saleTemporary') ?? this.cols; 
  }

  genListAction(data = []) {
    this.listAction = data.map(saleTemp => {
      const actions = [];

      if (this.isGranted([this.PermissionCoreConst.CoreDuyetSale_ThongTinSale])) {
        actions.push({
          data: saleTemp,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      return actions;
    });
  }

  getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('saleTemporary', JSON.stringify(data));
	}

	setColumn(col, _selectedColumns) {
		console.log('cols:', col);

		console.log('_selectedColumns', _selectedColumns);

		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
				console.log('Luu o local', this._selectedColumns);
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
      row.fullname = row?.investor ? row?.investor?.investorIdentification?.fullname : (row?.businessCustomer ? row?.businessCustomer?.name : null);
  		row.saleType = SaleConst.getInfoType(row.saleType, 'name');
      row['department.departmentName'] = row?.department?.departmentName ?? '';
      row.placeOfResidenceOrAddress = row?.investor ? row?.investor?.investorIdentification?.placeOfResidence : (row?.businessCustomer ? row?.businessCustomer?.address : null);
		};
		console.log('showData', rows);
	}

  changeFieldFilter() {
    if(this.keyword) {
      this.setPage();
    }
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._saleService.getAllTemp(this.page, this.dataFilter,this.sortData).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data?.totalItems;
        this.rows = res.data?.items;
        //
        if(this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows);
         }
        
        console.log({ rows: res.data?.items });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  create() {
    this.showDialogCreate();
  }

  showDialogCreate() {
    const ref = this.dialogService.open(
			CreateSaleComponent,
			{
        header: "Thêm Sale",
        width: '900px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px", "padding": '1.5rem' },
        baseZIndex: 10000,
        data: {
            
        },
      }
		);

		ref.onClose.subscribe((sale) => {
			console.log('sale------', sale);
      this.setPage();
		});
  }

  detail(saleTemp) {
    this.router.navigate(['/sale-manager/sale-temporary/detail/' + this.cryptEncode(saleTemp?.id)]);
  }

  request(saleTemp) {
    if(saleTemp?.employeeCode && saleTemp?.departmentId) {
      const ref = this.dialogService.open(
        FormRequestComponent,
        this.getConfigDialogServiceRAC("Trình duyệt", {})
      );
      //
      ref.onClose.subscribe((dataCallBack) => {
        console.log('dataCallBack', dataCallBack);
        if (dataCallBack?.accept) {
          const body = {
            ...dataCallBack.data,
            saleTempId: saleTemp.id,
            actionType: saleTemp.saleId ? ApproveConst.ACTION_UPDATE : ApproveConst.ACTION_ADD,
            summary: saleTemp?.investor?.investorIdentification?.fullname + ' - ' + saleTemp?.investor?.phone,
          }
          this._saleService.request(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
              this.setPage();
            }
          });
        }
      });
    } else {
      this.messageService.add({ severity: 'error', detail: 'Nhân viên sale không đủ thông tin để trình duyệt' });
    }
		
	}

  approve(item) {}

  cancel(item) {}

  confirmDelete() {}

}
