import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable()
export class SettingContractCodeService extends ServiceProxyBase {
  private readonly baseAPI = '/api/event/config-contract-code';

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public findAll(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.status || filter?.status === 0) url_ += this.convertParamUrl('status', filter.status);
    return this.requestGet(url_);
  }

  public createOrEdit(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}/update`);
    }
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public getById(id: number) {
    let url_ = `${this.baseAPI}/${id}?`;
    return this.requestGet(url_);
  }

  public changeStatus(id: number) {
    return this.requestPut(
      {
        configContractCodeId: id,
      },
      `${this.baseAPI}/update-status/${id}`
    );
  }

  public delete(id: number) {
    return this.requestDelete(
      `${this.baseAPI}/delete/${id}`
    );
  }
}
