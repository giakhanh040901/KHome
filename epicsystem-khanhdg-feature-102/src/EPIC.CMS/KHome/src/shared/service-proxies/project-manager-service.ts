import { AppConsts, ProjectConst } from '@shared/AppConsts';
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional, Injector } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from './service-proxies';

@Injectable()
export class ProjectServiceProxy extends ServiceProxyBase {

    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    get(projectId): Observable<any> {
        let url_ = "/api/invest/project/find/" + projectId;
        return this.requestGet(url_);
    }

    getProjectTradingProvider(projectId): Observable<any> {
        let url_ = "/api/invest/project-trading-provider/find-all?projectId=" + projectId;
        return this.requestGet(url_);
    }

    getAll(page: Page, status: string): Observable<any> {
        let url_ = "/api/invest/project/find?";
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl('status', status ?? '');
        url_ += this.convertParamUrl("keyword", page.keyword);
        return this.requestGet(url_);
    }

    getAllTradingProvider(): Observable<any> {
        let url_ = "/api/core/partner/find-trading-provider";
        return this.requestGet(url_);
    }

    getAllNoPaging(): Observable<any> {
        let url_ = "/api/invest/project/find?";
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl('status', ProjectConst.HOAT_DONG);

        return this.requestGet(url_);
    }


    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/project/update");
    }
    
    create(body): Observable<any> {
        return this.requestPost(body, "/api/invest/project/add");
    }
    delete(projectId): Observable<any> {
        let url_ = "/api/invest/project/delete/" + projectId;
        return this.requestDelete(url_);
    }

    request(body): Observable<any> {
        return this.requestPost(body, "/api/invest/project/request");
    }

    approve(body): Observable<any> {
        return this.requestPut(body, "/api/invest/project/approve");
    }

    // PHÊ DUYỆT HOẶC HỦY SẢN PHẨM
    approveOrCancel(body, approve: boolean): Observable<any> {
        if(approve) {
            return this.requestPut(body,  `/api/real-estate/project/approve`);
        }
        // CANCEL SP
        return this.requestPut(body,  `/api/real-estate/project/cancel`);
    }

    cancel(body): Observable<any> {
        return this.requestPut(body, "/api/invest/project/cancel");
    }

    check(body): Observable<any> {
        return this.requestPut(body, "/api/invest/project/check");
    }

    closeOrOpen(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/project/close-open?id=" + id);
    }

    // PROJECT JURIDICAL FILE 

    createJuridicalFile(body): Observable<any> {
        return this.requestPost(body, "/api/invest/project-juridical-file/add");
    }

    updateJuridicalFile(body): Observable<any> {
        return this.requestPut(body, "/api/invest/project-juridical-file/update/" + body.id);
    }

    deleteJuridicalFile(id: number): Observable<void> {
        let url_ = "/api/invest/project-juridical-file/delete/" + id;
        return this.requestDelete(url_);
    }

    getJuridicalFile(id: number): Observable<any> {
        let url_ = "/api/invest/project-juridical-file/file/find" + id;
        return this.requestGet(url_);
    }

    getAllJuridicalFile(page: Page, projectId): Observable<any> {
        let url_ = "/api/invest/project-juridical-file/file/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('projectId', projectId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    /**
     * PROJECT IMAGE
     */
    getAllProjectImage(projectId): Observable<any> {
        let url_ = "/api/invest/project-image/find/"+projectId;
        return this.requestGet(url_);
    }

    createProjectImage(body): Observable<any> {
        return this.requestPost(body, "/api/invest/project-image/add");
    }

    deleteProjectImage(id: number): Observable<void> {
        let url_ = "/api/invest/project-image/delete/" + id;
        return this.requestDelete(url_);
    }
} 
