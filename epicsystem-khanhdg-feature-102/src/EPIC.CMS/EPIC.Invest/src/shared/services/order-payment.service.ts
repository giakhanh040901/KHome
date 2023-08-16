import { AppConsts, InterestPaymentConst, OrderConst, YesNoConst } from '@shared/AppConsts';
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

import { API_BASE_URL, ServiceProxyBase } from "../service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from '../service-proxies/service-proxies';
import * as moment from 'moment';
import { TableBody } from 'primeng/table';


@Injectable()
export class OrderPaymentService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
		_cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/invest/order/payment/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/order/payment/update?id=" + body.id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/invest/order/payment/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/order/payment/find" + id;
        return this.requestGet(url_);
    }
    
    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/payment/approve/" + id + "?status=2");
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/payment/approve/" + id + "?status=3");
    }

    resentNotify(ordePaymentId: number): Observable<any> {
        return this.requestPost(null, "/api/invest/order/payment/resend-notification/" + ordePaymentId);
    }

    getAll(page: Page, orderId): Observable<any> {
        let url_ = "/api/invest/order/payment/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSizeAll);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('orderId', orderId);
        return this.requestGet(url_);
    }
}
