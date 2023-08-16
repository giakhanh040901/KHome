import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { AppConsts, ProductBondInfoConst, ProductBondPrimaryConst, ProductBondSecondaryConst, SortConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { OrderBlockageFilter } from "@shared/interface/filter.model";

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

    getAll(page: Page, dataFilter: OrderBlockageFilter): Observable<any> {
        let url_ = '/api/invest/blockade-liberation/find?';
        
        if(dataFilter.keyword) url_ += this.convertParamUrl("keyword", dataFilter.keyword);
        if(dataFilter.status) url_ += this.convertParamUrl('status', dataFilter.status);
        //
        if(dataFilter?.sortFields?.length){
            dataFilter.sortFields.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        if(dataFilter.tradingProviderIds.length > 0) {
            dataFilter.tradingProviderIds.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        } 

        url_ += this.convertParamUrl('pageSize', page.getPageSize());
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}