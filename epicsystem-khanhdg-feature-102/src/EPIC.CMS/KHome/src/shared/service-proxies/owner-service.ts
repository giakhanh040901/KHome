import { AppConsts } from '@shared/AppConsts';
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional, Injector } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from './service-proxies';

@Injectable()
export class OwnerServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/owner/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/owner/update");
    }
    
    delete(id: number): Observable<void> {
        let url_ = "/api/real-estate/owner/delete/" + id;
        return this.requestDelete(url_);
    }

    getOwner(id: number): Observable<any> {
        let url_ = "/api/real-estate/owner/find-by-id/" + id;
        return this.requestGet(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }

    getByTrading(){
        return this.requestGet('/api/real-estate/owner/get-all-by-trading');
    }

    getAllOwner(page: Page, status:any): Observable<any> {
        let url_ = "/api/real-estate/owner/find-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl("status", status ?? '');
        return this.requestGet(url_);
    }

    changeStatus(id: any, status: any): Observable<any> {
        return this.requestPut(null, "/api/real-estate/owner/change-status/" + id+ "?status="+status);
    }
}
