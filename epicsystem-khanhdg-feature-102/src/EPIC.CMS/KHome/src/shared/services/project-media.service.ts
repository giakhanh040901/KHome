import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProjectMediaService extends ServiceProxyBase {
    private endPoint = `/api/real-estate/project-media`;
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    find(projectId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/find/${projectId}?`
        if (filter.position) url_ += this.convertParamUrl('position', filter.position);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        return this.requestGet(url_);
    }

    findGroup(projectId: number, filter?: any): Observable<any> {
        let url_ = `/api/real-estate/project-media-detail/find/${projectId}?`
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        return this.requestGet(url_);
    }

    add(body): Observable<any> {
        return this.requestPost(body, '/api/real-estate/project-media-detail/add-list-detail');
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    createGroup(body): Observable<any> {
        return this.requestPost(body, '/api/real-estate/project-media-detail/add');
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update`);
    } 
    
    updateDetail(body): Observable<any> {
        return this.requestPut(body, `/api/real-estate/project-media-detail/update`);
    } 

    delete(id: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete/`+ id);
    }

    deleteDetail(id: number): Observable<any> {
        return this.requestDelete(`/api/real-estate/project-media-detail/delete/`+ id);
    }

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+ id + `?status=${status}`);
    }

    changeStatusDetail(id: number, status: string): Observable<any> {
        return this.requestPut(null, `/api/real-estate/project-media-detail/change-status/`+ id + `?status=${status}`);
    }

    setPositionItem(body) {
        return this.requestPut(body, `/api/real-estate/project-media/sort-order`);
    }

    setPositionItemDetail(body) {
        return this.requestPut(body, `/api/real-estate/project-media-detail/sort-order`);
    }

}