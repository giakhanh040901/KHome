import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { BasicFilter } from "@shared/interface/filter.model";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProjectShareService extends ServiceProxyBase {
    private endPoint = `/api/invest/project-information-share`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, projectId: number, dataFilter?: BasicFilter): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`;
        url_ += this.convertParamUrl('projectId', projectId);
        //
        if(dataFilter.keyword) url_ += this.convertParamUrl("keyword", dataFilter.keyword);
        if (dataFilter.status) url_ += this.convertParamUrl('status', dataFilter.status);

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        return this.requestGet(url_);
    }

    findById(id: number): Observable<any> {
        return this.requestGet(`${this.endPoint}/find-by-id/${id}`);
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

    changeStatus(id: number): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+ id);
    }
 }