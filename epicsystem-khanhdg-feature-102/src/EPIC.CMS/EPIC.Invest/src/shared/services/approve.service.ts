import { AppConsts, ApproveConst } from '@shared/AppConsts';
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional, Injector } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import * as moment from 'moment';
import { ApproveFilter } from '@shared/interface/filter.model';

@Injectable()
export class ApproveService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllTradingProvider(status?): Observable<any> {
        let url_ = "/api/core/trading-provider/find?";
        if (status) url_ += this.convertParamUrl("status", status);
        url_ += this.convertParamUrl("pageSize", -1);

        return this.requestGet(url_);
    }
    
    getAll(page: Page, dataFilter: ApproveFilter): Observable<any> {
        let url_ = "/api/invest/approve/all?";
        for (let [key, value] of Object.entries(dataFilter)) {
            if(value && key !='sortFields') {
                if(value instanceof Date) value = moment(value).format("YYYY-MM-DD");
                url_ += this.convertParamUrl(key, value);
            }
        }   

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    // Quản lý phê duyệt yêu cầu tái tục

    getAllRequestReinstatement(page: Page): Observable<any> {
        let url_ = "/api/invest/approve/all?";
        url_ += this.convertParamUrl("dataType", ApproveConst.STATUS_REINSTATEMENT);

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }

    getAllRequestWithdrawal(page: Page): Observable<any> {
        let url_ = "/api/invest/withdrawal/get-all?";

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }

    reinstatementApprove(body, action: string) {
        let url_ = "/api/invest/renewals-request/" + action;
        return this.requestPut(body, url_);
    }

    withdrawalApprove(body, action: string) {
        let url_ = "/api/invest/withdrawal/" + action + '/' + body.id;
        return this.requestPut(body, url_);
    }
}