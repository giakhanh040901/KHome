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
export class ContractFormService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any>{
        return this.requestPost(body, "/api/invest/distribution/config-contract-code/add");
    }

    delete(id: any): Observable<void> {
        let url_ = `/api/invest/distribution/config-contract-code/delete/` + id;
        return this.requestDelete(url_);
    }

    getAll(page: Page, fieldFilters): Observable<any> {
        let url_ = "/api/invest/distribution/config-contract-code/get-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        if(fieldFilters?.status) url_ += this.convertParamUrl('status', fieldFilters.status);
        //
        return this.requestGet(url_);
    }

    getAllNoPermisson(page: Page): Observable<any> {
        let url_ = "/api/invest/distribution/config-contract-code/get-all-status-active?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    update(body): Observable<any>{
        return this.requestPut(body, "/api/invest/distribution/config-contract-code/update");
    }

    /**
     * 
     * @param mẫu hợp đồng 
     * @returns 
     */

    updateStatus(body): Observable<any>{

        let url_ = `/api/invest/distribution/config-contract-code/update-status/${body.id}?`;
        url_ += this.convertParamUrl("customerType", body.customerType);    
        return this.requestPut(null,url_);
    }

    /**
     * 
     * @param hợp đồng mẫu
     * @returns 
     */

    getAllContractForm(distributionId : any): Observable<any> {
        let url_ = `/api/invest/contract-template/find-all/${distributionId}`;
        return this.requestGet(url_);
    }
    
    createContractForm(body): Observable<any>{
        return this.requestPost(body, "/api/invest/contract-template/add");
    }

    deleteContractForm(id: any): Observable<void> {
        let url_ = `/api/invest/contract-template/delete/` + id;
        return this.requestDelete(url_);
    }

    updateContractForm(body): Observable<any>{
        return this.requestPut(body, "/api/invest/contract-template/update");
    }
}