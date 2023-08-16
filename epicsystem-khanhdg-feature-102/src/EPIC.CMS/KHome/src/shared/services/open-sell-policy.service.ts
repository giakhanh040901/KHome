import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { ActiveDeactiveConst } from "@shared/AppConsts";

@Injectable()
export class OpenSellPolicyService extends ServiceProxyBase {
    private endPoint = `/api/real-estate/selling-policy`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, openSellId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/find-all-policy?`
        url_ += this.convertParamUrl('openSellId', openSellId);
        // url_ += this.convertParamUrl('status', ActiveDeactiveConst.ACTIVE);
        //
        if (filter.selected) url_ += this.convertParamUrl('selected', filter.selected);
        if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        if (filter.status) url_ += this.convertParamUrl("status", filter.status);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    findById(id: number): Observable<any> {
        let url_ = `/api/real-estate/selling-policy-temp/find-by-id/${id}`   
        return this.requestGet(url_);
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

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+ id + `?status=${status}`);
    }

    addPolicy(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/add-policy`);
    }

    updateStatus(body): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+body.id);
    }

}