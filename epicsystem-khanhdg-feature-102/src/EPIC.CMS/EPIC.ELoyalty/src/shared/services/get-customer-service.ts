import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class GetCustomerService extends ServiceProxyBase {
  private readonly baseAPI = '/api/core';
  public currentUser: string = '';
  public currentTime = new Date();

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public getCustomer(keyword: string, isBussiness: boolean = false): Observable<any> {
    let url_ = !isBussiness
      ? `${this.baseAPI}/manager-investor/filter?requireKeyword=true&`
      : `${this.baseAPI}/business-customer/find?`;
    if (keyword) {
      url_ += this.convertParamUrl('keyword', keyword);
    }
    url_ += this.convertParamUrl('pageSize', -1);
    return this.requestGet(url_);
  }

  public getCustomerDetail(id: number): Observable<any> {
    const url_ = `${this.baseAPI}/manager-investor`;
    return this.requestGet(
      `${url_}/${id}?isTemp=false&isNeedDefaultIdentification=true&isNeedReferralInvestor=true&isNeedApproveStatus=true&isNeedDefaultBank=true&isNeedListIdentification=true&isNeedListBank=true&isNeedDefaultAddress=true`
    );
  }

  public getCustomerForVoucher(investorId?: number, phone?: string) {
    let url_ = `/api/loyalty/investor-point/find?`;
    if (investorId) url_ += this.convertParamUrl('investorId', investorId);
    if (phone) url_ += this.convertParamUrl('phone', phone);
    return this.requestGet(url_);
  }
}
