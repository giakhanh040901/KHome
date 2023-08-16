import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import * as moment from 'moment';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';


@Injectable()
export class ProductCollateralService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAll(page: Page, body: any): Observable<any> {
        let url_ = `/api/garner/product/find-all-product-file/${body.productId}?`;
        url_ += this.convertParamUrl('documentType', body.documentType);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    addProductFile(body): Observable<any> {
        let url_ = `/api/garner/product/add-product-file/${body.productId}`;
        return this.requestPost(body, url_);
    }
    
    updateProductFile(body): Observable<any> {
        return this.requestPut(body, '/api/garner/product/update-product-file');
    }

    
    delete(id: number): Observable<void> {
        let url_ = "/api/garner/product/delete-product-file/" + id;
        return this.requestDelete(url_);
    }
}
