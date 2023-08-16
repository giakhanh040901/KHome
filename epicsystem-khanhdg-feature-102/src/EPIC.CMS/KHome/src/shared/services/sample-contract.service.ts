import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

@Injectable()
export class SampleContractService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any>{
        return this.requestPost(body, "/api/real-estate/contract-template-temp/add");
    }


    getAll(page: Page, fieldFilters: any): Observable<any> {
        let url_ = '/api/real-estate/contract-template-temp/find-all?';
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(fieldFilters) {
            for(const [key, value] of Object.entries(fieldFilters)) {
                if(key == 'searchField') {
                    if(page.keyword) url_ += this.convertParamUrl(fieldFilters.searchField, page.keyword);
                } 
                else {
                    if(value) url_ += this.convertParamUrl(key, `${value}`);
                    
                }
            }
        }
        return this.requestGet(url_);
    }

    getAllNoPermisson(page: Page, contractSource: any): Observable<any> {
        let url_ = '/api/invest/contract-template-temp/get-all?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('contractSource', contractSource?? '');
        return this.requestGet(url_);
    }

    update(body): Observable<any>{
        return this.requestPut(body, "/api/real-estate/contract-template-temp/update");
    }

    delete(id: any): Observable<void> {
        let url_ = `/api/real-estate/contract-template-temp/delete/` + id;
        return this.requestDelete(url_);
    }

    updateStatus(body): Observable<any>{
        let url_ = `/api/real-estate/contract-template-temp/change-status/${body.id}?`;  
        return this.requestPut(null,url_);
    }

}