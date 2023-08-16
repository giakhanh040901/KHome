import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApproveConst, FormNotificationConst, SaleConst, SearchConst, TypeFormatDateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { SaleService } from '@shared/service-proxies/sale-service';
import { FormRequestComponent } from 'src/app/form-request-approve-cancel/form-request/form-request.component';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import { SaleDirectionalComponent } from './sale-directional/sale-directional.component';

@Component({
  selector: 'app-sale-register',
  templateUrl: './sale-register.component.html',
  styleUrls: ['./sale-register.component.scss']
})
export class SaleRegisterComponent extends CrudComponentBase {

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
  SaleTypes = SaleConst.types;
  FormNotificationConst = FormNotificationConst;
  //
  rows: any[] = [];
	col: any;

	cols: any[];
	_selectedColumns: any[];
  listAction: any[] = [];

  sortData: any[] = []
  dataFilter = {
    field: null,
    status: null,
  }

  saleTypes: any[] = [];
  sources: any[] = [];
  statuses: any[] = [];

  directionalBody: any = {
		"saleRegisterIds": [],
		"isCancel": null,
		"tradingProviders": [],
		"saleType": 0,
	};

  ngOnInit() {
    //----
    this.statuses = [
      {
        'code': '',
        'name': 'Tất cả',
      },
      ...SaleConst.statusRegister
    ];
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
			{ field: 'investor.investorIdentification.fullname', header: 'Họ tên', width: '25rem', isPin: true },
			{ field: 'department', header: 'Phòng ban', width: '25rem', isPin: true },
			{ field: 'investor.investorIdentification.dateOfBirth', header: 'Ngày sinh', width: '8rem'},
      { field: 'columnResize', header: '', type:'hidden' },
		];

    this.cols = this.cols.map((item,index) => {
      item.position = index + 1;
      return item;
    })
		this._selectedColumns = this.getLocalStorage('saleApp') ?? this.cols; 
  }

  genListAction(data = []) {
		this.listAction = data.map((item) => {

			const actions = [];

			if (item?.status == 1 && this.isGranted([this.PermissionCoreConst.CoreSaleApp_DieuHuong])) {
				actions.push({
					data: item,
					label: "Điều hướng",
					icon: "pi pi-forward",
					command: ($event) => {
						this.directional($event.item.data);
					},
				})
			}

      if (item?.status == 1 && this.isGranted([this.PermissionCoreConst.CoreSaleApp_DieuHuong])) {
				actions.push({
					data: item,
					label: "Hủy điều hướng",
					icon: "pi pi-backward",
					command: ($event) => {
						this.directionalCancel($event.item.data);
					},
				})
			}

			

			return actions;
		});
	}

  // Điều hướng
  directional(sale){
    const ref = this.dialogService.open(
			SaleDirectionalComponent,
			{
				header: "Chọn điều hướng",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					saleManagerId : sale?.saleManagerId,
					
				},
			}
		);

    ref.onClose.subscribe((dataCallBack) => {
    	console.log("dataCallBack",dataCallBack);
    	if (dataCallBack?.accept) {
    		this.directionalBody.saleRegisterIds = [sale.id];
        this.directionalBody.isCancel = false;
        this.directionalBody.saleType = dataCallBack?.data?.saleType;
        this.directionalBody.tradingProviders = dataCallBack?.data?.tradingProviders;
        this._saleService.directional(this.directionalBody).subscribe((response) => {
          if (
            this.handleResponseInterceptor(
              response,
              "Điều hướng thành công"
            )
          ) {
            this.setPage();
          }
        });
    	}
    });
    
  }

  // Hủy điều hướng
  directionalCancel(sale){    
    const ref = this.dialogService.open(
			FormNotificationComponent,
			{
				header: "Hủy điều hướng",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title : "Bạn có chắc chắn hủy điều hướng Sale này không?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);

    ref.onClose.subscribe((dataCallBack) => {
    	console.log("dataCallBack",dataCallBack);
    	if (dataCallBack?.accept) {
    		this.directionalBody.saleRegisterIds = [sale.id];
        this.directionalBody.isCancel = true;
        this.directionalBody.saleType = 2;
        this.directionalBody.tradingProviders = [1];
        this._saleService.directional(this.directionalBody).subscribe((response) => {
          if (
            this.handleResponseInterceptor(
              response,
              "Hủy điều hướng thành công"
            )
          ) {
            this.setPage();
          }
        });
    	}
    });
  }

  getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('saleApp', JSON.stringify(data));
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
      row['investor.investorIdentification.fullname'] = row?.investor?.investorIdentification?.fullname,
      row.departmentName = row?.departmentName ?? "Sale chưa thuộc phòng ban",
      row['investor.investorIdentification.dateOfBirth'] = this.formatDate(row?.investor?.investorIdentification?.dateOfBirth,TypeFormatDateConst.DMY)
		};

	}

  changeFieldFilter() {
    if(this.keyword) {
      this.setPage();
    }
  }

  setPage(pageInfo?: any) {
    // this.setFieldError();
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._saleService.getAllRegister(this.page, this.dataFilter,this.sortData).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data?.totalItems;
        this.rows = res.data?.items;
        //
        if(this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows);
         }
         //
        console.log({ rows: res.data?.items });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  detail(sale) {
    this.router.navigate(['/sale-manager/sale-active/detail/' + this.cryptEncode(sale?.saleId)]);
  }
}
