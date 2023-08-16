import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductBondInfoConst, ProductBondDetailConst, StatusCoupon } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-order-coupon',
  templateUrl: './order-coupon.component.html',
  styleUrls: ['./order-coupon.component.scss']
})
export class OrderCouponComponent extends CrudComponentBase {

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

  ProductBondInfoConst = ProductBondInfoConst;

  rows: any[] = [];

  productBondInfoConst = ProductBondInfoConst;
  StatusCoupon = StatusCoupon;

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.isLoading = true;
    this._orderService.getCoupon(this.orderId).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.rows = res?.data?.couponInfo ?? [];
        console.log({ coupon: res });
      }
    }, () => {
      this.isLoading = false;
    });
  }
}
