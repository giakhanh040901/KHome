import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductBondInfoConst, ProductBondDetailConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-product-bond-info-coupon',
  templateUrl: './product-bond-info-coupon.component.html',
  styleUrls: ['./product-bond-info-coupon.component.scss']
})
export class ProductBondInfoCouponComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _productBondInfoService: ProductBondInfoServiceProxy,

  ) {
    super(injector, messageService);
    this.productBondId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  productBondId: number;
  couponInfo: any = {};

  ProductBondInfoConst = ProductBondInfoConst;

  rows: any[] = [];

  productBondInfoConst = ProductBondInfoConst;
  page = new Page();
  offset = 0;

  ngOnInit() {
    this.setPage();
  }

  changeKeyword() {
    if (this.keyword === '') {
      this.setPage({ page: this.offset });
    }
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._productBondInfoService.getCoupon(this.productBondId).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res?.data?.totalItems;
        this.rows = res?.data?.couponRecurents;
        this.couponInfo = res?.data;
        console.log({ coupon: res });
      }
    }, () => {
      this.isLoading = false;
    });
  }

}
