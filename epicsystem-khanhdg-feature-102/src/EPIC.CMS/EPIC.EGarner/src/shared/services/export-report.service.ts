import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
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
    
    getTotalInvestmentPayment(requestDate: any, approveDate: any): Observable<any> {
        let url_ = "/api/garner/export-excel-report/export-total-investment-payment?";  
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_, true);
    }

    getTotalInvestment(requestDate: any, approveDate: any): Observable<any> {
        let url_ = "/api/garner/export-excel-report/export-list-total-investment?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_, true);
    }

    getTotalProduct(requestDate: any, approveDate: any): Observable<any>{
        let url_ = "/api/garner/export-excel-report/excel-list-garner-product?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_, true);
    }

    getAdministration(requestDate: any, approveDate: any): Observable<any>{
        let url_ = "/api/garner/export-excel-report/export-garner-list-administration?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_, true);
    }

    getSaleInvestment(requestDate: any, approveDate: any): Observable<any>{
        let url_ = "/api/garner/export-excel-report/excel-list-sale-investment?";   
        url_ += this.convertParamUrl("startDate", requestDate ?? '');
        url_ += this.convertParamUrl("endDate", approveDate ?? '');
        return this.requestDownloadFile(url_, true);
    }
    
}