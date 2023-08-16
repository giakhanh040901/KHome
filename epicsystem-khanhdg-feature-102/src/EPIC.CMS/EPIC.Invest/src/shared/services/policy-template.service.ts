import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { SortConst } from "@shared/AppConsts";
import { PolicyTemplateFilter } from "@shared/interface/filter.model";

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
        return this.requestPost(body, "/api/invest/policy-temp/add");
    }

    createPolicyDetail(body): Observable<any> {
        return this.requestPost(body, "/api/invest/policy-temp/add-policy-detail-temp");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/invest/policy-temp/update?id=" + id);
    }

    updatePolicyDetail(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/invest/policy-temp/update-policy-detail-temp?id=" + id);
    }

    changeStatusPolicy(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/policy-temp/change-status?id=" + id);
    }
    
    changeStatusPolicyDetail(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/policy-temp/change-status-policy-detail-temp?id=" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/invest/policy-temp/delete?id=" + id;
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

    getAll(page: Page, dataFilter: PolicyTemplateFilter): Observable<any> {
        let url_ = "/api/invest/policy-temp/find?";

        for(const [key,value] of Object.entries(dataFilter)) {
            if(value && key != "sortFields") {
                url_ += this.convertParamUrl(key, value);
            }
        }
        
        if(dataFilter.sortFields){
            dataFilter.sortFields.forEach(s => {
                url_ += this.convertParamUrl('Sort', `${s.field}-${SortConst.getValueSort(s.order)}`);
            })
        }

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}
