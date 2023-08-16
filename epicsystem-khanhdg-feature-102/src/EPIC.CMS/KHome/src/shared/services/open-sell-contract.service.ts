import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

@Injectable()
export class OpenSellContractService extends ServiceProxyBase {
    private endPoint = `/api/real-estate/open-sell-contract-template`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, openSellId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('openSellId', openSellId);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.contractType) url_ += this.convertParamUrl('contractType', filter.contractType);
        if (filter.keyword) url_ += this.convertParamUrl("contractTemplateTempName", filter.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllConfig(page: Page, type?): Observable<any> {
        let url_ = "/api/real-estate/config-contract-code/get-all-config?";
        if(type) url_ += this.convertParamUrl("type", type);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('status', 'A');
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllSellingPolicy(openSellId: any, status?: string): Observable<any> {
        let url_ = "/api/real-estate/distribution-policy/get-all-policy?";
        url_ += this.convertParamUrl('openSellId', openSellId);
        if(status) url_ += this.convertParamUrl('status', status);
        return this.requestGet(url_);
    }

    getAllSampleContract(page: Page, contractType?: any, type?: any): Observable<any> {
        let url_ = "/api/real-estate/contract-template-temp/get-all-contract?";
        if(contractType) url_ += this.convertParamUrl("contractType", contractType);
        if(type) url_ += this.convertParamUrl("type", type);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('status', 'A');
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    updateStatus(body): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+body.id);
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update`);
    } 




    delete(id: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete/`+ id);
    }
}