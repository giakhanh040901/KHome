import { AppConsts, InterestPaymentConst, OrderConst, SortConst, YesNoConst } from '@shared/AppConsts';
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

import { API_BASE_URL, ServiceProxyBase } from "../service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from '../service-proxies/service-proxies';
import * as moment from 'moment';
import { TableBody } from 'primeng/table';
import { OrderInterestFilter } from '@shared/interface/filter.model';

@Injectable()
export class InterestPaymentService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/interest-payment/find/" + id;
        return this.requestGet(url_);
    }
   
    getAllContractInterest(page: Page, dataFilter: OrderInterestFilter, apiUrl: string): Observable<any> {
        let url_ = apiUrl;
        for( let [key, value] of Object.entries(dataFilter)) {
            if((value || value === 0) && !dataFilter.paramIgnore.includes(key)) {
                if(key == 'status') {
                    // LỌC THEO LOẠI ĐÃ CHI TRẢ (TẤT CẢ / TỰ ĐỘNG / THỦ CÔNG)
                    if([InterestPaymentConst.STATUS_DONE_ONLINE, InterestPaymentConst.STATUS_DONE_OFFLINE].includes(+value)) {
                        url_ += this.convertParamUrl(key, InterestPaymentConst.STATUS_DONE);
                        url_ += this.convertParamUrl('interestPaymentStatus', InterestPaymentConst.typeInterests[`${value}`]);
                    } else {
                        url_ += this.convertParamUrl(key, `${value}`);
                    }
                } 
                else {
                    if(key === 'keyword') key = dataFilter.searchField || key;
                    if(value instanceof Date) value = moment(value).format("YYYY-MM-DD");
                    url_ += this.convertParamUrl(key, value);
                }
            }
        }

        if(dataFilter?.tradingProviderIds && dataFilter?.tradingProviderIds?.length) {
            dataFilter.tradingProviderIds.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        }

        if(dataFilter?.sortFields && dataFilter?.sortFields?.length){
            dataFilter.sortFields.forEach(item => {
                if (item.field == 'policyDetailName'){
                    url_ += this.convertParamUrl('Sort', `periodType-${SortConst.getValueSort(item.order)}`);
                    url_ += this.convertParamUrl('Sort', `periodQuantity-${SortConst.getValueSort(item.order)}`);
                } else {
                    url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
                }
            })
        }
        url_ += this.convertParamUrl('pageSize', page.getPageSize());
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    addListInterest(body): Observable<any> {
        let url_ = "/api/invest/interest-payment/add";
        return this.requestPost(body, url_);
    }
}