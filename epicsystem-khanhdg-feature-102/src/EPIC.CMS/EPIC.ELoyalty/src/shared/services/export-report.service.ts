import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

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
    
    exportExcel(startDate: string, endDate: string, type: string): Observable<any> {
        let url_ = `/api/loyalty/export-excel-report/${type}?`;   
        if(startDate) url_ += this.convertParamUrl("startDate", startDate);
        if(endDate) url_ += this.convertParamUrl("endDate", endDate);
        return this.requestDownloadFile(url_, true);
    }
}