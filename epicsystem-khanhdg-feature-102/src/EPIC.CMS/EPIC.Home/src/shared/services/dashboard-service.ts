import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class DashboardService extends ServiceProxyBase {
    // private secondaryEndPoint = `/api/bond/secondary`;
    constructor(
        messageService: MessageService,
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    /**
     * TẠO PHÁT HÀNH THỨ CẤP
     * @param body
     * @returns
     */
    getInfoDashboardInvest(dataFilter?: any): Observable<any> {
        let url_ = "/api/invest/dashboard?";
        if (dataFilter) {
            if (dataFilter?.firstDate) url_ += this.convertParamUrl('firstDate', dataFilter?.firstDate);
            if (dataFilter?.endDate) url_ += this.convertParamUrl('endDate', dataFilter?.endDate);
            if (dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
        }
        return this.requestGet(url_);
    }

    getInfoDashboardBond(dataFilter?: any): Observable<any> {
        let url_ = "/api/bond/dashboard?";
        if (dataFilter) {
            if (dataFilter?.firstDate) url_ += this.convertParamUrl('firstDate', dataFilter?.firstDate);
            if (dataFilter?.endDate) url_ += this.convertParamUrl('endDate', dataFilter?.endDate);
            if (dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
        }
        return this.requestGet(url_);
    }

}