import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { IndividualCustomer, MembershipLevelManagement } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class IndividualCustomerService extends ServiceProxyBase {
  public individualCustomerId: number;
  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public getAllIndividualCustomer(page: Page, filter: any, sort?: string): Observable<any> {
    let url_ = '/api/loyalty/voucher/investors?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ = this.getParamApiGetAll(page, filter, url_);
    !!sort && (url_ += this.convertParamUrl('Sort', sort));
    return this.requestGet(url_);
  }

  public getCustomerDetail() {
    const url_ = '/api/core/manager-investor';
    return this.requestGet(
      `${url_}/${this.individualCustomerId}?isTemp=false&isNeedDefaultIdentification=true&isNeedReferralInvestor=true&isNeedApproveStatus=true&isNeedDefaultBank=true&isNeedListIdentification=true&isNeedListBank=true&isNeedDefaultAddress=true&isNeedLoyalty=true`
    );
  }

  public getListVoucherByCustomerId(page: Page, filter: any): Observable<any> {
    let url_ = '/api/loyalty/investor-point/conversion-history?';
    url_ += this.convertParamUrl('investorId', this.individualCustomerId);
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', filter.keyword);
    url_ += this.convertParamUrl('voucherType', filter.voucherType ? filter.voucherType : '');
    if(filter.status) {
      if(filter.status !== IndividualCustomer.HET_HAN_VOUCHER) {
        url_ += this.convertParamUrl('status', filter.status);
      } else {
        url_ += this.convertParamUrl('isExpired', true);
      }
    }
    
    return this.requestGet(url_);
  }

  public getHistoryOfInvestor(page: Page, filter: any): Observable<any> {
    let url_ = `/api/loyalty/accumulate-point/investor/${this.individualCustomerId}/find?`;
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter.type) url_ += this.convertParamUrl('pointType', filter.type);
    if (filter.reason) url_ += this.convertParamUrl('reason', filter.reason);
    return this.requestGet(url_);
  }

  public getListClass() {
    let url_ = '/api/loyalty/rank/find?';
    url_ += this.convertParamUrl('pageNumber', 1);
    url_ += this.convertParamUrl('pageSize', 100);
    url_ += this.convertParamUrl('status', MembershipLevelManagement.KICH_HOAT);
    return this.requestGet(url_);
  }

  public exportExcel(page: Page, filter: any) {
    let url_ = '/api/loyalty/voucher/investors/export-excel?';
    url_ += this.convertParamUrl('pageSize', -1);
    url_ = this.getParamApiGetAll(page, filter, url_);
    return this.requestDownloadFile(url_);
  }

  private getParamApiGetAll(page: Page, filter: any, url_: string) {
    const isAddedVoucher = filter.voucherLevel === 2 ? true : filter.voucherLevel === 1 ? false : '';
    const isCheckedInvestor = filter.account === 1 ? true : filter.account === 2 ? false : '';
    url_ += this.convertParamUrl('keyword', page.keyword);
    url_ += this.convertParamUrl('sex', filter.gender || '');
    url_ += this.convertParamUrl('rank', filter.class || '');
    url_ += this.convertParamUrl('isAddedVoucher', isAddedVoucher);
    url_ += this.convertParamUrl('isCheckedInvestor', isCheckedInvestor);
    return url_;
  }
}
