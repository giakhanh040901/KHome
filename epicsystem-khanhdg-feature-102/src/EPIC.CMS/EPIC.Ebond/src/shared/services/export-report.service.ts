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
export class ExportReportService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getExportInvest(requestDate: any, approveDate: any): Observable<any> {
        let url_ = "/api/bond/export-excel-report/export-file?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_);
    }

    getExportInvestment(requestDate: any, approveDate: any): Observable<any> {
        let url_ = "/api/bond/export-excel-report/export-bond-investment?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_);
    }

    getExportBondDue(requestDate: any, approveDate: any): Observable<any> {
        let url_ = "/api/bond/export-excel-report/export-bond-due?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_);
    }
  
}