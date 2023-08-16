import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { SortConst } from "@shared/AppConsts";

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
        return this.requestPost(body, "/api/garner/blockade-liberation/add");
    }

    liberationContractActive(body, id:number): Observable<any>{
        let url_= '/api/garner/blockade-liberation/update?';
        url_ += this.convertParamUrl("id", id);
        return this.requestPut(body, url_);
    }

    getAll(page: Page, status:number, type:number, tradingProviderIds:[], sortData?: any[]): Observable<any> {
        let url_ = '/api/garner/blockade-liberation/find-all?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('status', status?? '');
        if(tradingProviderIds?.length > 0) {
            tradingProviderIds?.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        } 
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        return this.requestGet(url_);
    }
}