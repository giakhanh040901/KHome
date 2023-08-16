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
export class NotificationExtendService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getDistributionInvest(): Observable<any> {
        let url_ = "/api/invest/distribution/get-all?";   
        return this.requestGet(url_);
    }

    getDistributionGarner(): Observable<any> {
        let url_ = "/api/garner/distribution/find-list-ban-ho";   
        return this.requestGet(url_);
    }

    getOpenSellRst(): Observable<any> {
        let url_ = "/api/real-estate/open-sell/find-list-ban-ho";   
        return this.requestGet(url_);
    }

    getEvent(): Observable<any> {
        let url_ = "/api/event/manager-event/find-event-sell-behalf";   
        return this.requestGet(url_);
    }
}