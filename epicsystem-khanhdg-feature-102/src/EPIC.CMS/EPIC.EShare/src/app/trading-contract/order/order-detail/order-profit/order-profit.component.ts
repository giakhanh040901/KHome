import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompanyShareInfoConst, CompanyShareDetailConst, StatusCoupon } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-order-profit',
  templateUrl: './order-profit.component.html',
  styleUrls: ['./order-profit.component.scss']
})
export class OrderProfitComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _orderService: OrderServiceProxy,

  ) {
    super(injector, messageService);
    this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }
  
  orderId: number;
  couponInfo: any = {};

  CompanyShareInfoConst = CompanyShareInfoConst;

  rows: any[] = [];

  companyShareInfoConst = CompanyShareInfoConst;
  StatusCoupon = StatusCoupon;

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.isLoading = true;
    this._orderService.getCoupon(this.orderId).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.rows = res?.data?.profitInfo ?? [];
        this.couponInfo = res?.data?.couponInfo ?? [];
        console.log({ coupon: res });
      }
    }, () => {
      this.isLoading = false;
    });
  }

}
