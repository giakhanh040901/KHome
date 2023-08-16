import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import * as moment from 'moment';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';


@Injectable()
export class InterestPaymentService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/garner/interest-payment/find/" + id;
        return this.requestGet(url_);
    }

    payInterest(body): Observable<any> {
        return this.requestPut(body, "/api/garner/interest-payment/change-established-to-paid-status");
    }

    getAll(page: Page, dataFilter: any, apiUrl): Observable<any> {
        let url_ = apiUrl;
        url_ += this.convertParamUrl(dataFilter.fieldFilter, page.keyword);
        if(dataFilter.ngayChiTra) {
            url_ += this.convertParamUrl("ngayChiTra", moment(dataFilter.ngayChiTra).format("YYYY-MM-DD"));
        }

        if(dataFilter.typeInterest) {
            url_ += this.convertParamUrl("status", dataFilter.typeInterest);
        }

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    addListInterest(body): Observable<any> {
        let url_ = "/api/garner/interest-payment/add";
        return this.requestPost(body, url_);
    }
}
