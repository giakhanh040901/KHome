import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

/**
 * Phong toả, giải toả
 */
@Injectable()
export class BlockageLiberationService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    
    blockadeContractActive(body): Observable<any>{
        return this.requestPost(body, "/api/invest/blockade-liberation/add");
    }
    liberationContractActive(body, id:number): Observable<any>{
        let url_= '/api/invest/blockade-liberation/update?';
        url_ += this.convertParamUrl("id", id);
        return this.requestPut(body, url_);
    }

    getAll(page: Page, status:number, type:number): Observable<any> {
        let url_ = '/api/invest/blockade-liberation/find?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('status', status?? '');
        return this.requestGet(url_);
    }
}