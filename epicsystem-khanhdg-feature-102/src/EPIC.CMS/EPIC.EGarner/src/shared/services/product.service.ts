import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

@Injectable()
export class ProductService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, '/api/garner/product/add');
    }

    update(body): Observable<any> {
        return this.requestPut(body, '/api/garner/product/update');
    }

    delete(id: number): Observable<void> {
        let url_ = '/' + id;
        return this.requestDelete(url_);
    }

    request(body): Observable<any> {
        return this.requestPut(body, "/api/garner/product/request");
    }
    
    approve(body): Observable<any> {
        return this.requestPut(body,  '/api/garner/product/approve');
    }

    cancel(body): Observable<any> {
        return this.requestPut(body,  '/api/garner/product/cancel');
    }

    rootCheck(body): Observable<any> {
        return this.requestPut(body,  '/api/garner/product/check');
    }

    changeStatus(productId): Observable<any> {
        return this.requestPut(null, '/api/garner/product/change-status/'+productId);
    }

    getById(id: number): Observable<any> {
        let url_ = '/api/garner/product/find-by-id?id=' + id;
        return this.requestGet(url_);
    }

    getAll(page: Page,fieldFilters): Observable<any> {
        let url_ = '/api/garner/product/find-all?';
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
       
        // BỘ LỌC
        url_ += this.convertParamUrl("code", page.keyword);
        if(fieldFilters?.status) url_ += this.convertParamUrl('status', fieldFilters?.status);
        if(fieldFilters?.issuerId) url_ += this.convertParamUrl('issuerId', fieldFilters?.issuerId);
        if(fieldFilters?.productType) url_ += this.convertParamUrl('productType', fieldFilters?.productType);
        
        return this.requestGet(url_);
    }

    getAllIssuer(): Observable<any> {
        return this.requestGet('/api/garner/product/distinct-issuer?pageSize=-1');
    }

    // Tìm kiếm doanh nghiệp
    getBusinessCustomer(taxCode): Observable<any> {
        let url_ = '/api/core/business-customer/findTaxCode/' + taxCode;
        return this.requestGet(url_);
    }
  
    getAllTradingProvider(productId:any): Observable<any> {
        // let url_ = `/api/garner/product/find-by-id?id=`+productId;
        let url_ = "/api/garner/product-trading-provider/find-all-by-product?productId=" + productId;
       
        return this.requestGet(url_);
    }

    getHistoryProduct(productId:any): Observable<any> {
        let url_ = "/api/garner/product/find-history-update-by-id?id=" + productId;
        return this.requestGet(url_);
    }

    getTradingProvider(): Observable<any> {
        let url_ = "/api/core/partner/find-trading-provider";
        return this.requestGet(url_);
    }


    getFindTradingProvider(taxCode): Observable<any> {
        let url_ = "/api/core/business-customer/findTaxCode/" + taxCode;
        return this.requestGet(url_);
    }

    createTradingProvider(body): Observable<any> {
        return this.requestPost(body, "/api/garner/product-trading-provider/add");
    }

    updateTradingProvider(body): Observable<any> {
        return this.requestPut(body, "/api/garner/product-trading-provider/update");
    }
}