import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { AppConsts } from "@shared/AppConsts";

/**
 * PHÁT HÀNH THỨ CẤP
 */
@Injectable()
export class DistributionService extends ServiceProxyBase {

    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getDistributionsOrder(): Observable<any> {
        let url_ = "/api/invest/distribution/find-distribution-order";
        return this.requestGet(url_);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    updateImage(image): Observable<any> {
        let url = `/api/users/update-avatar`;
        return this.requestPut(image, url);
    }

}