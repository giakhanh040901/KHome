import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { AppConsts, ProductBondInfoConst, ProductBondPrimaryConst, ProductBondSecondaryConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";
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
        return this.requestPut(null, '/api/bond/order/delivery-status-delivered/'+ id);
    }

    deliveryStatusReceived(id: number): Observable<any> {
        return this.requestPut(null, '/api/bond/order/delivery-status-received/'+ id);
    }

    deliveryStatusDone(id: number): Observable<any> {
        return this.requestPut(null, '/api/bond/order/delivery-status-done/'+ id);
    }

    exportContractReceive(orderId: any,bondSecondaryId: any, tradingProviderId: any, source: any): Observable<any> {
        let url_ = `/api/bond/export-contract/export-contract-receive?`;
        if(orderId){
            url_ += this.convertParamUrl("orderId", orderId);
        }
        if(bondSecondaryId){
            url_ += this.convertParamUrl("bondSecondaryId", bondSecondaryId);
        }
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(source){
            url_ += this.convertParamUrl("source", source);
        }
        return this.requestDownloadFile(url_);
    }
}