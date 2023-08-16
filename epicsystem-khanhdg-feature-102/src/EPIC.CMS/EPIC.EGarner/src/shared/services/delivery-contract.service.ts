import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";

/**
 * Giao nhận hợp đồng
 */
@Injectable()
export class DeliveryContractService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    
    deliveryStatusDelivered(id: number): Observable<any> {
        return this.requestPut(null, '/api/garner/order/delivery-status-delivery/'+ id);
    }

    deliveryStatusReceived(id: number): Observable<any> {
        return this.requestPut(null, '/api/garner/order/delivery-status-received/'+ id);
    }

    deliveryStatusDone(id: number): Observable<any> {
        return this.requestPut(null, '/api/garner/order/delivery-status-done/'+ id);
    }
    
    exportContractReceive(orderId: any): Observable<any> {
        let url_ = `/api/garner/export-contract/file-receive?`;
        if(orderId){
            url_ += this.convertParamUrl("orderId", orderId);
        }
        return this.requestDownloadFile(url_);
    }
}