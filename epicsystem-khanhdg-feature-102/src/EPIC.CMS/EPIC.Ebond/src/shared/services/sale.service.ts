import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { AppConsts, ProductBondInfoConst, ProductBondPrimaryConst, ProductBondSecondaryConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

/**
 * Sale
 */
@Injectable()
export class SaleService extends ServiceProxyBase {
    private distributionEndPoint = `/api/core/manager-sale`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    getById(id:number): Observable<any>{
        let url_ = `${this.distributionEndPoint}/find/` + id;
        return this.requestGet(url_);
    }
    getByTradingProvider(referralCode:string): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-by-trading-provider?`;
        if(referralCode){
            url_ += this.convertParamUrl("referralCode", referralCode);
        }
        return this.requestGet(url_);
    }
  
}