import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { SortConst } from "@shared/AppConsts";
import { SampleContractFilter } from "@shared/interface/filter.model";

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
        return this.requestPost(body, "/api/invest/contract-template-temp/add");
    }


    getAll(page: Page, dataFilter: SampleContractFilter): Observable<any> {
        let url_ = '/api/invest/contract-template-temp/find-all?';

        for(const [key, value] of Object.entries(dataFilter)) {
            if(value && key != 'sortFields') {
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

    getAllNoPermisson(contractSource: any): Observable<any> {
        let page: Page = new Page();
        let url_ = '/api/invest/contract-template-temp/get-all?';
        url_ += this.convertParamUrl('contractSource', contractSource ?? '');

        url_ += this.convertParamUrl('pageSize', page.pageSizeAll);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    update(body): Observable<any>{
        return this.requestPut(body, "/api/invest/contract-template-temp/update");
    }

    delete(id: any): Observable<void> {
        let url_ = `/api/invest/contract-template-temp/delete/` + id;
        return this.requestDelete(url_);
    }

    updateStatus(body): Observable<any>{
        let url_ = `/api/invest/contract-template-temp/change-status/${body.id}?`;  
        return this.requestPut(null,url_);
    }

    testFillContract(params): Observable<any> {
        let url_ = `/api/invest/export-contract/file-template-temp-pdf?`;
        for(const[key, value] of Object.entries(params)) {
            if(value) url_ += this.convertParamUrl(key, params[key]);
        }
        return this.requestDownloadFile(url_);
    }

}