import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class MembershipLevelManagementService extends ServiceProxyBase {
  private baseAPI: string = '/api/loyalty/rank';
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
    if (filter.status) url_ += this.convertParamUrl('status', filter.status);
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

  public changeStatus(status: number, id: number) {
    return this.requestPut(
      {
        id,
        status,
      },
      `${this.baseAPI}/status`
    );
  }
}
