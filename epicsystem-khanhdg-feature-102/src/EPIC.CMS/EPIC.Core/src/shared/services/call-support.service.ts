import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { AppConsts } from "@shared/AppConsts";

@Injectable()
export class CallSupportService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string,

    ) {
        super(messageService, _cookieService, http, baseUrl);
        this.baseUrl = AppConsts.nodeBaseUrl;
    }

    getAll(page: Page, filters?: any): Observable<any> {
        let url_ = "/api/media/call?";
        url_ += this.convertParamUrl("limit", page.pageSize);
        url_ += this.convertParamUrl("page", page.getPageNumber());
        return this.requestGet(url_);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/media/call");
    }

}