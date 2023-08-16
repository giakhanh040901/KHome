import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompanyShareInfoConst, CompanyShareDetailConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-company-share-info-coupon',
  templateUrl: './company-share-info-coupon.component.html',
  styleUrls: ['./company-share-info-coupon.component.scss']
})
export class CompanyShareInfoCouponComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _companyShareInfoService: CompanyShareInfoServiceProxy,

  ) {
    super(injector, messageService);
    this.companyShareId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  companyShareId: number;
  couponInfo: any = {};

  CompanyShareInfoConst = CompanyShareInfoConst;

  rows: any[] = [];

  companyShareInfoConst = CompanyShareInfoConst;
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

    this._companyShareInfoService.getCoupon(this.companyShareId).subscribe((res) => {
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
