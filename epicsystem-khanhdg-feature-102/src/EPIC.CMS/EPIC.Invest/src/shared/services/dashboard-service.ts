import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import * as moment from "moment";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";
import { API_BASE_URL, ServiceProxyBase } from "../service-proxies/service-proxies-base";

@Injectable()
export class DashBoardServiceProxy extends ServiceProxyBase {
    // private secondaryEndPoint = `/api/bond/secondary`;
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    /**
     * TẠO PHÁT HÀNH THỨ CẤP
     * @param body
     * @returns
     */
    getInfoDashBoard(dataFilter?: any): Observable<any> {
        let url_ = "/api/invest/dashboard?";
        if (dataFilter) {
            if (dataFilter?.firstDate) url_ += this.convertParamUrl('firstDate', moment(dataFilter.firstDate).format('YYYY-MM-DD'));
            if (dataFilter?.endDate) url_ += this.convertParamUrl('endDate', moment(dataFilter.endDate).format('YYYY-MM-DD'));
            if (dataFilter?.tradingProviderId) url_ += this.convertParamUrl('tradingProviderId', dataFilter?.tradingProviderId);
            if (dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
        }
        return this.requestGet(url_);
    }

    getProduct(tradingProviderId?:number){
        let url_ = "/api/invest/dashboard/distribution?";
        if (tradingProviderId) url_ += this.convertParamUrl('tradingProviderId', tradingProviderId);
        //
        return this.requestGet(url_);
    }

}