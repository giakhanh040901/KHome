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
        return this.requestPost(body, "/api/garner/policy-temp/add");
    }

    createPolicyDetail(body): Observable<any> {
        return this.requestPost(body, "/api/garner/policy-detail-temp/add");
    }

    createPolicyContractTemp(body): Observable<any> {
        return this.requestPost(body, "/api/garner/contract-template-temp/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/garner/policy-temp/update?id=" + id);
    }

    updatePolicyDetail(body): Observable<any> {
        return this.requestPut(body, "/api/garner/policy-detail-temp/update");
    }

    updatePolicyContractTemp(body): Observable<any> {
        return this.requestPut(body, "/api/garner/contract-template-temp/update");
    }

    changeStatusPolicy(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/policy-temp/change-status/" + id);
    }
    
    changeStatusPolicyDetail(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/policy-temp/change-status-policy-detail-temp?id=" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/garner/policy-temp/delete/" + id;
        return this.requestDelete(url_);
    }

    deletePolicyDetail(id: number): Observable<void> {
        let url_ = "/api/garner/policy-detail-temp/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/garner/policy-temp/find-by-id?id=" + id;
        return this.requestGet(url_);
    }

    getPolicyDetails(policyId: number): Observable<any> {
        let url_ = "/api/garner/policy-detail-temp/find-by-policy-temp?policyTempId=" + policyId;
        return this.requestGet(url_);
    }
    
    getContracts(policyId: number): Observable<any> {
        let url_ = "/api/garner/contract-template-temp/find-by-policy-temp?policyTempId=" + policyId;
        return this.requestGet(url_);
    }

    getAll(page: Page, status:any): Observable<any> {
        let url_ = "/api/garner/policy-temp/find-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("status", status ?? '');
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}
