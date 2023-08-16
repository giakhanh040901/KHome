import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class PartnerBankService extends ServiceProxyBase {
    private endPoint = `/api/core/partner-bank-account`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, partnerId: number): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('partnerId', partnerId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    findById(id: number, partnerId: number): Observable<any> {
        return this.requestGet(`${this.endPoint}/find-by-id?id=${id}&partnerId=${partnerId}`);
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update`);
    } 

    delete(id: number, partnerId: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete?id=${id}&partnerId=${partnerId}`);
    }

    changeStatus(id: number, partnerId: number): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status?id=${id}&partnerId=${partnerId}`);
    }

    setDefault(id: number, partnerId: number): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/set-default?id=${id}&partnerId=${partnerId}`);
    }
}