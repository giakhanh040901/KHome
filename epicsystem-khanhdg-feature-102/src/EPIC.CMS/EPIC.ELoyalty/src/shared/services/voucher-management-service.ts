import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { VoucherManagement } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class VoucherManagementService extends ServiceProxyBase {
  private readonly baseAPI = '/api/loyalty/voucher';

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public getAllVoucher(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.status || filter?.status === 0) url_ += this.convertParamUrl('status', filter.status);
    if (filter?.kind) url_ += this.convertParamUrl('voucherType', filter.kind);
    if (filter?.type) url_ += this.convertParamUrl('useType', filter.type);
    if (filter?.isShowApp) url_ += this.convertParamUrl('isShowApp', filter.isShowApp);
    if (filter?.expireDate) url_ += this.convertParamUrl('expiredDate', filter.expireDate);
    return this.requestGet(url_);
  }

  public getAllActiveVoucher(){
    return this.requestGet(`${this.baseAPI}/find?pageSize=999&status=${VoucherManagement.KICH_HOAT}`);
  }

  public createOrEditVoucher(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}`);
    }
    return this.requestPost(body, `${this.baseAPI}`);
  }

  public getVoucherById(voucherId: number) {
    let url_ = `${this.baseAPI}/${voucherId}?`;
    return this.requestGet(url_);
  }

  public getVoucherCustomerById(id: number) {
    const url_ = `/api/loyalty/investor-point/conversion-history-get-by-id/${id}?`;
    return this.requestGet(url_);
  }

  public changeStatusVoucher(status: number, voucherId: number) {
    return this.requestPut(
      {
        id: voucherId,
        status,
      },
      `${this.baseAPI}/status`
    );
  }

  public deleteVoucher(voucherId: number) {
    return this.requestDelete(`${this.baseAPI}/${voucherId}`);
  }

  public highlightVoucher(voucherId: number, isHighlight: string) {
    return this.requestPut(
      {
        id: voucherId,
        isHot: isHighlight,
      },
      `${this.baseAPI}/is-hot`
    );
  }

  public showAppVoucher(voucherId: number, isShowApp: string) {
    return this.requestPut(
      {
        id: voucherId,
        isShowApp,
      },
      `${this.baseAPI}/is-show-app`
    );
  }

  public voucherHistory(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/conversion-history?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.kind) url_ += this.convertParamUrl('voucherType', filter.kind);
    if (filter?.type) url_ += this.convertParamUrl('useType', filter.type);
    if (filter?.applyDate) url_ += this.convertParamUrl('conversionPointFinishedDate', filter.applyDate);
    return this.requestGet(url_);
  }
}
