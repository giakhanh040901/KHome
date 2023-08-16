import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

@Injectable()
export class PolicyTemplateService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/selling-policy-temp/add");
    }

    createPartnerTemp(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/distribution-policy-temp/add");
    }

    createPolicyDetail(body): Observable<any> {
        return this.requestPost(body, "/api/invest/policy-temp/add-policy-detail-temp");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/selling-policy-temp/update");
    }

    updatePartnerTemp(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/distribution-policy-temp/update");
    }

    updatePolicyDetail(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/invest/policy-temp/update-policy-detail-temp?id=" + id);
    }

    changeStatusPolicy(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/selling-policy-temp/change-status/" + id);
    }

    changeStatusPartnerTemp(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/distribution-policy-temp/change-status/" + id);
    }
    
    changeStatusPolicyDetail(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/policy-temp/change-status-policy-detail-temp?id=" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/real-estate/distribution-policy-temp/delete/" + id;
        return this.requestDelete(url_);
    }

    deleteSelling(id: number): Observable<void> {
        let url_ = "/api/real-estate/selling-policy-temp/delete/" + id;
        return this.requestDelete(url_);
    }

    deletePolicyDetail(id: number): Observable<void> {
        let url_ = "/api/invest/policy-temp/delete-policy-detail-temp?id=" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/policy-temp/find-by-id?id=" + id;
        return this.requestGet(url_);
    }

    getAllPartnerTemp(page: Page, fieldFilter: any) {
        let url_ = "/api/real-estate/distribution-policy-temp/find-all?";
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if(fieldFilter) {
            for(const [key, value] of Object.entries(fieldFilter)) {
                if(key == 'searchField') {
                    if(page.keyword) url_ += this.convertParamUrl(fieldFilter.searchField, page.keyword);
                } 
                else {
                    if(value) url_ += this.convertParamUrl(key, `${value}`);
                    
                }
            }
        }

        return this.requestGet(url_);
    }

    getAll(page: Page, fieldFilter:any): Observable<any> {
        let url_ = "/api/real-estate/selling-policy-temp/find-all?";
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(fieldFilter) {
            for(const [key, value] of Object.entries(fieldFilter)) {
                if(key == 'searchField') {
                    if(page.keyword) url_ += this.convertParamUrl(fieldFilter.searchField, page.keyword);
                } 
                else {
                    if(value) url_ += this.convertParamUrl(key, `${value}`);
                    
                }
            }
        }

        return this.requestGet(url_);
    }
}
