import { Component, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CompanyShareInfoConst, CompanyShareDetailConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DistributionContractServiceProxy, CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-distribution-contract-interest-period',
  templateUrl: './distribution-contract-interest-period.component.html',
  styleUrls: ['./distribution-contract-interest-period.component.scss']
})
export class DistributionContractInterestPeriodComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _companyShareInfoService: CompanyShareInfoServiceProxy,
    private _distributionContractService: DistributionContractServiceProxy,

  ) {
    super(injector, messageService);
    this.companyShareId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  @Input() distributionContractId: number;
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

    this._distributionContractService.getCoupon(this.distributionContractId).subscribe((res) => {
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
