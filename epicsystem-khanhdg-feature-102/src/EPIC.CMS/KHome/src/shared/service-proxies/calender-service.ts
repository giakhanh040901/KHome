import { AppConsts } from '@shared/AppConsts';
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

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from './service-proxies';
@Injectable()
export class CalendarServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/invest/calendar/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/calendar/update");
    }

    get(workingDate): Observable<any> {
        let url_ = "/api/invest/calendar/" + workingDate;
        return this.requestGet(url_);
    }

    getAll(workingYear): Observable<any> {
        let url_ = "/api/invest/calendar/find/" + workingYear;
        return this.requestGet(url_);
    }
}