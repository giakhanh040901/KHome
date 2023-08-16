import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { Observable } from 'rxjs';
import { SpinnerService } from './spinner.service';

@Injectable()
export class BillRequestListService extends ServiceProxyBase {
  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    private spinnerService: SpinnerService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public findAll(page: Page, filter: any): Observable<any> {
    let url_ = '/api/event/order/find-all-delivery-invoice?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.eventType && filter?.eventType.length)
      url_ += this.convertParamTypeArrayUrl('eventType', filter.eventType);
    if (filter?.status && filter?.status.length)
      url_ += this.convertParamTypeArrayUrl('deliveryInvoiceStatus', filter.status);
    if (filter?.date) url_ += this.convertParamUrl('processingInvoiceDate', filter.date);
    return this.requestGet(url_);
  }

  public changeStatus(id: number, status: number) {
    return this.requestPut(
      {
        orderId: id,
        deliveryStatus: status,
      },
      `/api/event/order/change-delivery-invoice-status?orderId=${id}&deliveryStatus=${status}`
    );
  }
}
