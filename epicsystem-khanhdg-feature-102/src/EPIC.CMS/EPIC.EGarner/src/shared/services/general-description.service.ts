import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

@Injectable()
export class GeneralDescriptionService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllGeneralDescription(page: Page, status?: string): Observable<any> {
        let url_ = `/api/media/garner?`;
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        if (page.keyword) url_ += this.convertParamUrl("keyword", page.keyword);
        if (status) url_ += this.convertParamUrl('status', status);
        return this.requestGet(url_);
    }

    getGeneralDescriptionById(id?: number): Observable<any> {
        // let url_ = `` + id;
        return this.requestGet("/api/media/garner");
    }

    updateDescription(body): Observable<any> {
        return this.requestPost(body, `/api/media/garner`);
    }

    updateImage(body): Observable<any> {
        return this.requestPost(body, `/api/media/garner/images`);
    }

    updateFeature(body): Observable<any> {
        return this.requestPost(body, `/api/media/garner/features`);
    }


}
