import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";

@Injectable()
export class ConfigureSystemService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/core/trading-provider-config/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/core/trading-provider-config/update");
    }

    getAll(page: Page, isAllItem: boolean = false): Observable<any> {
        let url_ = "/api/core/trading-provider-config/get-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', isAllItem ? page.pageSizeAll : page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    createOrUpdate(body) {
        if (!body?.isUpdate) return this.requestPost(body, "/api/core/trading-provider-config/add");
        return this.requestPut(body, "/api/core/trading-provider-config/update");
    }
}