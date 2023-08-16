import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class LuckyProgramInvestorService extends ServiceProxyBase {
    private readonly baseAPI = '/api/loyalty/lucky-program/investor';

    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllInvestor(luckyProgramId: number, page: Page, filter: any): Observable<any>{
        let url_ = '/api/loyalty/lucky-program/get-all-investor?'
        url_ += this.convertParamUrl('isSelected', false);
        url_ += this.convertParamUrl('luckyProgramId', luckyProgramId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        
        if(filter.keyword) url_ += this.convertParamUrl('keyword', filter.keyword);
        if(filter.sex) url_ += this.convertParamUrl('sex', filter.sex);
        if(filter.rankId) url_ += this.convertParamUrl('rankId', filter.rankId);
        if(filter.customerType) url_ += this.convertParamUrl('customerType', filter.customerType);
        
        return this.requestGet(url_);
    }

    getAll(luckyProgramId: number, page: Page, filter: any): Observable<any>{
        let url_ = this.baseAPI + '/find-all?'
        url_ += this.convertParamUrl('luckyProgramId', luckyProgramId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        if(filter.keyword) url_ += this.convertParamUrl('keyword', filter.keyword);
        if(filter.isJoin) url_ += this.convertParamUrl('isJoin', filter.isJoin);
        
        return this.requestGet(url_);
    }

    create(body): Observable<any>{
        return this.requestPost(body, `${this.baseAPI}/add`)
    }

    delete(id: number){
        return this.requestPut(null, `${this.baseAPI}/delete/${id}`)
    }
}