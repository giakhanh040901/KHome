import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";
import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import * as moment from "moment";

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
            if (dataFilter?.firstDate) url_ += this.convertParamUrl('firstDate', dataFilter?.firstDate);
            if (dataFilter?.endDate) url_ += this.convertParamUrl('endDate', dataFilter?.endDate);
            if (dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
        }
        return this.requestGet(url_);
    }

    getDuChiDashBoard(month: number, year: number): Observable<any> {
        let url_ = "/api/invest/dashboard/du-chi?";
        url_ += this.convertParamUrl('month', month);
        url_ += this.convertParamUrl('year', year);
        return this.requestGet(url_);
    }

    getData(filter: any) {
        let url = "/api/real-estate/dashboard/find-all?";
        if (filter) {         
            filter.dates && (
                url += this.convertParamUrl('firstDate', moment(filter.dates[0]).format('YYYY-MM-DD'))
                , url += this.convertParamUrl('endDate', moment(filter.dates[1]).format('YYYY-MM-DD'))
            );
            filter.project && (url += this.convertParamUrl('projectId', filter.project));
        }
        return this.requestGet(url);
    }

    getListProject() {
        let url = "/api/real-estate/dashboard/list-project";
        return this.requestGet(url);
    }
}