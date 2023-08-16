import { AppConsts } from '@shared/AppConsts';
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

import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';


@Injectable()
export class ProjectStructureService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/project-structure/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/project-structure/update");
    }
   
    delete(id: number): Observable<void> {
        let url_ = "/api/real-estate/project-structure/delete/" + id;
        return this.requestDelete(url_);
    }

    getDepartmentChild(parentId?: number): Observable<any> {
        let url_ = "/api/real-estate/project-structure/find-by-project-id/"+parentId;
        if(parentId) {
            // url_ += this.convertParamUrl("parentId", parentId);
        }
        return this.requestGet(url_);
    }

    getNodeLasts(projectId) {
        let url_ = "/api/real-estate/project-structure/find-all-child/" + projectId;
        return this.requestGet(url_);
    }
    getNodeByLevel(level, projectId){
        let url_ = `/api/real-estate/project-structure/find-all/level/${level}?`;
        url_ += this.convertParamUrl("projectId", projectId);

        return this.requestGet(url_);
    }
}