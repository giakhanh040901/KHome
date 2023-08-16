import { Component, Injector, OnInit } from '@angular/core';
import { CompanyShareInfoConst, CompanyShareDetailConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService, ConfirmationService, MenuItem } from 'primeng/api';
import { forkJoin, Subscription } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { OrderService } from '@shared/service-proxies/shared-data-service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public orderService: OrderService,
    private _companyShareInfoService: CompanyShareInfoServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink:['/home'] },
      { label: 'Sổ lệnh',  routerLink: ['trading-contract/order']},
      { label: 'Đặt lệnh'},
    ]);
  }

  modalDialog: boolean;

  rows: any[] = [];

  submitted: boolean;

  page = new Page();
  offset = 0;
  // Menu otions thao tác
  actions: any[] = [];
  actionsDisplay: any[] = [];

  //
  items: MenuItem[];
  subscription: Subscription;

  ngOnInit() {
    // this.setPage({ page: this.offset });

    this.items = [
      {
        label: 'Thông tin khách hàng',
        routerLink: '/trading-contract/order/create/filter-customer'
      },
      {
        label: 'Thông tin cổ phần',
        routerLink: '/trading-contract/order/create/filter-company-share'
      },
      {
        label: 'Xác nhận thông tin',
        routerLink: '/trading-contract/order/create/view'
      },
    ];
    this.subscription = this.orderService.orderComplete$.subscribe((personalInformation) =>{
      // this.messageService.add({severity:'success', summary:'Order submitted', detail: 'Dear, ' + personalInformation.firstname + ' ' + personalInformation.lastname + ' your order completed.'});
  });
  }

  clickDropdown(row) {
    // this.companyShareInfo = { ...row };
    // this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
  }

  detail() {
    // this.router.navigate(['/company-share-manager/company-share-info/detail/' + this.companyShareInfo.companyShareId]);
  }

  create() {
    this.router.navigate['/trading-contract/order/filter-customer'];
  }



  edit() {
    this.modalDialog = true;
  }

  changeKeyword() {
    if (this.keyword === '') {
      this.setPage({ page: this.offset });
    }
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._companyShareInfoService.getAll(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  hideDialog() {
    this.modalDialog = false;
    this.submitted = false;
  }

  save() {
    this.submitted = true;
  }

}
