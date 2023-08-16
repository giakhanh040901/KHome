import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

/**
 * Giao nhận hợp đồng
 */
@Injectable()
export class DistributionContractService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    
    getAllNoPermisson(page: Page): Observable<any> {
        let url_ = "/api/garner/distribution/config-contract-code/get-all-status-active?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllContractForm(page: Page, distributionId : any, fieldFilters): Observable<any> {
        let url_ = `/api/garner/contract-template/find-all?`;

        url_ += this.convertParamUrl("distributionId", distributionId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        for(const [key, value] of Object.entries(fieldFilters)) {
           if(fieldFilters[key]) url_ += this.convertParamUrl(`${key}`, fieldFilters[key]);
        }
        return this.requestGet(url_);
    }
    
    createContractForm(body): Observable<any>{
        return this.requestPost(body, "/api/garner/contract-template/add");
    }

    deleteContractForm(id: any): Observable<void> {
        let url_ = `/api/garner/contract-template/delete/` + id;
        return this.requestDelete(url_);
    }

    updateContractForm(body): Observable<any>{
        return this.requestPut(body, "/api/garner/contract-template/update");
    }
}