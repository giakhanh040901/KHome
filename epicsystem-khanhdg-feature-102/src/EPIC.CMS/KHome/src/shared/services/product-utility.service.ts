import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProductUtilityService extends ServiceProxyBase { 
    private endPoint = `/api/real-estate/product-item`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, productId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/utility/find-all?`
        url_ += this.convertParamUrl('productItemId', productId);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.selected) url_ += this.convertParamUrl('selected', filter.selected);
        if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        return this.requestGet(url_);
    }

    getAll(productId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/utility/get-all?`
        url_ += this.convertParamUrl('productItemId', productId);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.selected) url_ += this.convertParamUrl('selected', filter.selected);
        if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        return this.requestGet(url_);
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update`);
    } 

    delete(id: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete/`+ id);
    }

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+ id + `?status=${status}`);
    }

    updateUtility(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/utility/update`);
    }

    updateStatus(body): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status-utility/`+body.id);
    }

}