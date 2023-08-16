import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProjectFileService extends ServiceProxyBase { 
    private endPoint = `/api/real-estate/project-file`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, projectId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('projectId', projectId);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.keyword) url_ += this.convertParamUrl('name', filter.keyword);
        if (filter.type) url_ += this.convertParamUrl('projectFileType', filter.type);
        // url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
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

    public viewFilePDF(url: string) {
        return this.viewFilePDFFromAPI(url);
    }
}