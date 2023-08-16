import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

@Injectable()
export class PolicyFileService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    
    create(body): Observable<any>{
        return this.requestPost(body, "/api/garner/distribution/policy-file/add");
    }

    getAll(page: Page, distributionId?: any): Observable<any> {
        let url_ = "/api/garner/distribution/get-all-policy-file?";
        url_ += this.convertParamUrl("DistributionId", distributionId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        return this.requestGet(url_);
    }

    update(body): Observable<any>{
        return this.requestPut(body, "/api/garner/distribution/policy-file/update");
    }

    updateStatus(body): Observable<any>{

        let url_ = `/api/garner/config-contract-code/update-status/${body.id}?`;
        url_ += this.convertParamUrl("customerType", body.customerType);    
        return this.requestPut(null,url_);
    }

    delete(id: any): Observable<void> {
        let url_ = `/api/garner/distribution/policy-file/delete/` + id;
        return this.requestDelete(url_);
    }
}