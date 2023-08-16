import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';

import { MessageService, MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderShareService } from '@shared/service-proxies/shared-data-service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public orderService: OrderShareService,
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

    this.items = [
      {
        label: 'Thông tin khách hàng',
        routerLink: '/trading-contract/order/create/filter-customer'
      },
      {
        label: 'Thông tin dự án đầu tư',
        routerLink: '/trading-contract/order/create/filter-project'
      },
      {
        label: 'Xác nhận thông tin',
        routerLink: '/trading-contract/order/create/view'
      },
    ];
    this.subscription = this.orderService.orderComplete$.subscribe((personalInformation) =>{
  });
  }

  create() {
    this.router.navigate['/trading-contract/order/filter-customer'];
  }

  edit() {
    this.modalDialog = true;
  }

  hideDialog() {
    this.modalDialog = false;
    this.submitted = false;
  }

  save() {
    this.submitted = true;
  }

}
