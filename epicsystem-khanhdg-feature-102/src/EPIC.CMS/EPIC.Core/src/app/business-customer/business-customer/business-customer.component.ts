import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessCustomerApproveConst, BusinessCustomerConst, DistributionContractConst, FormNotificationConst, SearchConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { BusinessCustomerServiceProxy } from '@shared/service-proxies/business-customer-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { BankServiceProxy } from '@shared/service-proxies/bank-service';
import { NationalityConst } from '@shared/nationality-list';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { Subject } from 'rxjs';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
  selector: 'app-business-customer',
  templateUrl: './business-customer.component.html',
  styleUrls: ['./business-customer.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class BusinessCustomerComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private _dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private routeActive: ActivatedRoute,
    private _businessCustomerService: BusinessCustomerServiceProxy,
    private breadcrumbService: BreadcrumbService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Khách hàng doanh nghiệp', routerLink: ['/customer/business-customer/business-customer'] },
    ]);
  }

  ref: DynamicDialogRef;
  //
  rows: any[] = [];
  row: any;
  col: any;

  cols: any[];
  
  _selectedColumns: any[];
  listAction: any[] = [];

  BusinessCustomerApproveConst = BusinessCustomerApproveConst;
  BusinessCustomerConst = BusinessCustomerConst;
  DistributionContractConst = DistributionContractConst;
  YesNoConst = YesNoConst;
  NationalityConst = NationalityConst;

  isCheckSearch: any[] = [
    {
      name: 'Tất cả',
      code: ''
    },
    ...BusinessCustomerConst.isCheckConst
  ];

  dataFilter = {
    fieldFilter: null,
    isCheck: null
  }

  businessCustomer: any = {
    "businessCustomerId": 0,
    "code": null,
    "name": null,
    "shortName": null,
    "address": null,
    "phone": null,
    "mobile": null,
    "email": null,
    "taxCode": null,
    "licenseDate": null,
    "licenseIssuer": null,
    "capital": null,
    "tradingAddress": null,
    "nation": null,
    "decisionNo": null,
    "decisionDate": null,
    "numberModified": null,
    "dateModified": null,
    "repName": null,
    "repPosition": null,
    "bankName": null,
    "bankBranchName": null,
    "bankAccNo": null,
    "backAccName": null,
    "status": null,
  }

  genListAction(data = []) {
    this.listAction = data.map(businessCustomerItem => {
      const actions = [];

      if (this.isGranted([this.PermissionCoreConst.CoreKHDN_ThongTinKhachHang])) {
        actions.push({
          data: businessCustomerItem,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if (businessCustomerItem.status == this.BusinessCustomerConst.status.HOAT_DONG 
        && (businessCustomerItem.isCheck == false || businessCustomerItem.isCheck == "N") 
        && this.isGranted([this.PermissionCoreConst.CoreKHDN_XacMinh])) {
        actions.push({
          data: businessCustomerItem,
          label: 'Phê duyệt (Epic)',
          icon: 'pi pi-check',
          command: ($event) => {
            this.check($event.item.data);
          }
        });
      }
      return actions;
    });
  }

  fieldErrors = {};
  fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];
  submitted: boolean;
  expandedRows = {};
  statuses: any[];

  //
  page = new Page();
  offset = 0;

  sortData: any[] = []
  actions: any[] = [];  // list button actions
  actionsDisplay: any[] = [];

  ngOnInit(): void {
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
    this.subject.isSetPage.subscribe(() => {
			this.setPage();
		  })
    this.cols = [
      { field: 'taxCode', header: 'Mã số thuế', width: '12rem', isPin: true, isSort: true },
      { field: 'shortName', header: 'Tên viết tắt', width: '20rem', isPin: true, isResize: true, isSort: true },
      { field: 'name', header: 'Tên doanh nghiệp', width: '35rem', isResize: true, isSort: true },
      { field: 'email', header: 'Thư điện tử', width: '18rem', cutText: 'b-cut-text-18', isSort: true },
      { field: 'isCheck', header: 'Kiểm tra', width: '10rem', class: 'justify-content-center' },
    ];

    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    });
    this._selectedColumns = this.getLocalStorage('busiCusCore') ?? this.cols;
  }
  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('busiCusCore', JSON.stringify(data));
  }

  setColumn(col, _selectedColumns) {
    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
    );
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns)
      }
    });
  }

  showData(rows) {
    for (let row of rows) {
    };
  }

  check(businessCustomer) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn phê duyệt doanh nghiệp này không?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
			this._businessCustomerService.check({ id: businessCustomer?.businessCustomerId }).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Phê duyệt doanh nghiệp thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

  clickDropdown(row) {
    this.businessCustomer = { ...row };
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
  }

  detail(businessCustomer) {
    this.router.navigate(['/customer/business-customer/business-customer/detail/' + this.cryptEncode(businessCustomer?.businessCustomerId)]);
  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.businessCustomer)) {
      this.fieldErrors[key] = false;
    }
  }

  changeStatus() {
    this.setPage({ page: this.offset });
  }

  changeFieldFilter() {
    if (this.keyword?.trim()) {
      this.setPage();
    }
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    //
    this.isLoading = true;
    this._businessCustomerService.getAll(this.page, this.dataFilter,this.sortData).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        if (res.data?.items) {
          this.rows = res.data.items.map(row => {
            row.isCheck = (row.isCheck == this.YesNoConst.YES);
            return row;
          });
        }
        //
        if (res.data?.items?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows);
        }
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
    });
  }
}
