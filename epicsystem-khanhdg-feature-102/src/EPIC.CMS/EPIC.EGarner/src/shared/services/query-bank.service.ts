import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { QueryMoneyBankCost } from "@shared/AppConsts";

@Injectable()
export class QueryMoneyBankService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    QueryMoneyBankCost = QueryMoneyBankCost;
    
    getAllCollectMoneyBank(page: Page, status?: string, queryDate?: any): Observable<any> {
        let url_ = `/api/payment/msb/collection-payment/find-all?`;
        url_ += this.convertParamUrl("ProductType", QueryMoneyBankCost.GARNER)
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        if (page.keyword) url_ += this.convertParamUrl("keyword", page.keyword);
        if (status) url_ += this.convertParamUrl('status', status);
        if (queryDate) url_ += this.convertParamUrl('CreatedDate', queryDate)
        return this.requestGet(url_);
    }

    getAllPayMoneyBank(page: Page, status?: string, queryDate?: any): Observable<any> {
        let url_ = `/api/payment/msb/request-pay/find-all?`;
        url_ += this.convertParamUrl("ProductType", QueryMoneyBankCost.GARNER)
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        if (page.keyword) url_ += this.convertParamUrl("keyword", page.keyword);
        if (status) url_ += this.convertParamUrl('status', status);
        if (queryDate) url_ += this.convertParamUrl('ApproveDate', queryDate)
        return this.requestGet(url_);
    }

    getView(item?: any): Observable<any> {
        let url_ = `/api/payment/msb/request-pay/inquiry-batch/${item?.requestId}`;
        return this.requestGet(url_);
    }
}
