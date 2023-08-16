import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class OrderSellingPolicyService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, orderId: number,filter?: any): Observable<any> {
        let url_ = `/api/real-estate/order-selling-policy/find-all?`
        url_ += this.convertParamUrl('OrderId', orderId);
        if (filter.selected) url_ += this.convertParamUrl('isSelected', filter.selected);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }
 }