import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class AccumulatePointManagementService extends ServiceProxyBase {
  private baseAPI: string = '/api/loyalty/accumulate-point';
  public currentUser: string = '';
  public currentTime = new Date();

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this.currentUser = this.appSessionService.user.userName;
  }

  public getAll(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter.type) url_ += this.convertParamUrl('pointType', filter.type);
    return this.requestGet(url_);
  }

  public getDetail(id: number): Observable<any> {
    const url_ = `${this.baseAPI}/${id}`;
    return this.requestGet(url_);
  }

  public createOrEditVoucher(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}`);
    }
    return this.requestPost(body, `${this.baseAPI}`);
  }

  public getAllReasons() {
    return this.requestGet(`${this.baseAPI}/reasons`);
  }

  public cancelRequest(id: number) {
    const url_ = `${this.baseAPI}/status/cancel`;
    return this.requestPut({ id }, url_);
  }

  public importPoint(body: any) {
    return this.requestPostFile(body, `${this.baseAPI}/import-excel`);
  }
}
