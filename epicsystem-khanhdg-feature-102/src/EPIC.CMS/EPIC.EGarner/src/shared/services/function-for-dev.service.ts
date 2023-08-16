import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

@Injectable()
export class FunctionForDevService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any>{
        return this.requestPost(body, "/api/invest/contract-template-temp/add");
    }


    getAll(page: Page, fieldFilters: any): Observable<any> {
        let url_ = '/api/invest/contract-template-temp/find-all?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(fieldFilters?.status) url_ += this.convertParamUrl('status', fieldFilters.status);
        if(fieldFilters?.contractSource) url_ += this.convertParamUrl('contractSource', fieldFilters.contractSource);
        if(fieldFilters?.contractType) url_ += this.convertParamUrl('contractType', fieldFilters.contractType);
        return this.requestGet(url_);
    }

    getAllNoPermisson(tradingBankAccId : any): Observable<any> {
        let url_ = `/api/core/trading-provider/msb-prefix-account/find-by-trading-bank-id/${tradingBankAccId}`;
     
        return this.requestGet(url_);
    }

    receive(body): Observable<any>{
        return this.requestPost(body, "/api/payment/msb/receive-notification");
    }

}