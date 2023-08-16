import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { QueryMoneyBankCost, SortConst } from "@shared/AppConsts";
import * as moment from "moment";
import { QueryCollectionBankFilter, QueryPaymentBankFilter } from "@shared/interface/filter.model";

@Injectable()
export class QueryMoneyBankService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    QueryMoneyBankCost = QueryMoneyBankCost;

    getAllQueryBank(page: Page, type: string, dataFilter: QueryPaymentBankFilter | QueryCollectionBankFilter): Observable<any> {
        let url_ = `/api/payment/msb/${type}/find-all?`;
        url_ += this.convertParamUrl("ProductType", QueryMoneyBankCost.INVEST)
        
        for(let [key,value] of Object.entries(dataFilter)) {
            if(value && key != 'sortFields') {
                if(value instanceof Date) value = moment(value).format("YYYY-MM-DD");
                url_ += this.convertParamUrl(key, value);
            }
        }
        
        if(dataFilter.sortFields) {
            dataFilter.sortFields.forEach(s => {
                url_ += this.convertParamUrl('Sort', `${s.field}-${SortConst.getValueSort(s.order)}`);
            })
        }

        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    getView(item?: any): Observable<any> {
        let url_ = `/api/payment/msb/request-pay/inquiry-batch/${item?.requestId}`;
        return this.requestGet(url_);
    }
}
