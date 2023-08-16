import { Component, OnInit } from '@angular/core';
import {
  CreateSaleTicketOrderEvent,
  CustomerSearchModel,
  IntroduceSearchModel,
} from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MenuItem } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'create-sale-ticket-order',
  templateUrl: './create-sale-ticket-order.component.html',
  styleUrls: ['./create-sale-ticket-order.component.scss'],
})
export class CreateSaleTicketOrderComponent implements OnInit {
  constructor(
    private routerService: RouterService,
    private saleTicketOrderService: SaleTicketOrderService,
    private breadcrumbService: BreadcrumbService
  ) {
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Sổ lệnh', routerLink: ['/sale-ticket-management/sale-ticket-order'] },
      { label: 'Đặt lệnh' },
    ]);
  }

  public activeIndex: number = 0;
  public routerLinkCreate: string = '/sale-ticket-management/sale-ticket-order/create';
  public steps: MenuItem[] = [
    {
      label: 'Thông tin khách hàng',
      routerLink: this.routerLinkCreate + '/customer',
    },
    {
      label: 'Thông tin sự kiện',
      routerLink: this.routerLinkCreate + '/event',
    },
    {
      label: 'Xác nhận thông tin',
      routerLink: this.routerLinkCreate + '/confirm',
    },
  ];

  ngOnInit() {
    this.saleTicketOrderService.selectedOrder = {
      customer: new CustomerSearchModel(),
      introduce: new IntroduceSearchModel(),
      event: new CreateSaleTicketOrderEvent(),
    };
    this.saleTicketOrderService.dtoCustomer = new CustomerSearchModel();
    this.saleTicketOrderService.dtoIntroduce = new IntroduceSearchModel();
    this.saleTicketOrderService.dtoEvent = new CreateSaleTicketOrderEvent();

    this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order/create/customer']);
  }
}
