import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProductMediaService extends ServiceProxyBase {
    private endPointMedia = `/api/real-estate/product-item-media`;
    private endPointMediaDetail = `/api/real-estate/product-item-media-detail`;
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    find(productId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPointMedia}/find/${productId}?`
        if (filter.position) url_ += this.convertParamUrl('position', filter.position);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        return this.requestGet(url_);
    }

    findGroup(productId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPointMediaDetail}/find/${productId}?`
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        return this.requestGet(url_);
    }

    add(body): Observable<any> {
        return this.requestPost(body, `${this.endPointMediaDetail}/add-list-detail`);
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.endPointMedia}/add`);
    }

    createGroup(body): Observable<any> {
        return this.requestPost(body, `${this.endPointMediaDetail}/add`);
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.endPointMedia}/update`);
    } 
    
    updateDetail(body): Observable<any> {
        return this.requestPut(body, `${this.endPointMediaDetail}/update`);
    } 

    delete(id: number): Observable<any> {
        return this.requestDelete(`${this.endPointMedia}/delete/`+ id);
    }

    deleteDetail(id: number): Observable<any> {
        return this.requestDelete(`${this.endPointMediaDetail}/delete/`+ id);
    }

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPointMedia}/change-status/`+ id + `?status=${status}`);
    }

    changeStatusDetail(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPointMediaDetail}/change-status/`+ id + `?status=${status}`);
    }

    setPositionItem(body) {
        return this.requestPut(body, `/api/real-estate/product-item-media/sort-order`);
    }

    setPositionItemDetail(body) {
        return this.requestPut(body, `/api/real-estate/product-item-media-detail/sort-order`);
    }
}