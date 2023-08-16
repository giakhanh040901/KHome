import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf, throwError } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { UserTypes } from "@shared/AppConsts";

@Injectable()
export class CallCenterConfigService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    update(body): Observable<any> {
      return this.requestPut(body, "/api/core/call-center-config/update");
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/core/call-center-config/find-all?";
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        if(page?.keyword) {
          url_ += this.convertParamUrl("keyword", page.keyword);
        }
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllUserForConfig(): Observable<any> {
        let url_ = "/api/users/permission-data/user-by-manager?pageSize=-1";
        return this.requestGet(url_);
      }
}