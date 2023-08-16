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

import { API_BASE_URL, ServiceProxyBase } from "../service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from '../service-proxies/service-proxies';

@Injectable()
export class GeneralContractorService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/invest/general-contractor/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/general-contractor/update" + body.id);
    }

    
    delete(id: number): Observable<void> {
        let url_ = "/api/invest/general-contractor/delete/" + id;
        return this.requestDelete(url_);
    }

    getContractor(id: number): Observable<any> {
        let url_ = "/api/invest/general-contractor/" + id;
        return this.requestGet(url_);
    }
    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/find/" + id;
        return this.requestGet(url_);
    }

    getByTaxCode(taxCode): Observable<any> {
        let url_ = "/api/core/business-customer/findTaxCode/" + (taxCode || null);
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllContractor(page: Page, status?: any): Observable<any> {
        let url_ = "/api/invest/general-contractor/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if(status) url_ += this.convertParamUrl("status", status);
        return this.requestGet(url_);
    }
}