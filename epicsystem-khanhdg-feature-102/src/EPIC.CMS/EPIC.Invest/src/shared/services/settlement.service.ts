import { AppConsts, InterestPaymentConst, OrderConst, YesNoConst } from '@shared/AppConsts';
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

@Injectable()
export class SettlementService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllSettlementContract(page: Page, dataFilter?: any): Observable<any> {
        let url_ = "/api/invest/order/find-settlement?";
        url_ += this.convertParamUrl(dataFilter?.fieldFilter, page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        if(dataFilter) {
            if(dataFilter?.id) url_ += this.convertParamUrl('distributionId', dataFilter?.id);
            if(dataFilter?.policy) url_ += this.convertParamUrl('policy', dataFilter?.policy);
            if(dataFilter?.policyDetailId) url_ += this.convertParamUrl('policyDetailId', dataFilter?.policyDetailId);
            if(dataFilter?.source) url_ += this.convertParamUrl('source', dataFilter?.source);
            if (dataFilter?.orderSource) url_ += this.convertParamUrl('orderer', dataFilter?.orderSource);
            if (dataFilter?.tradingDate) url_ += this.convertParamUrl('tradingDate', moment(dataFilter?.tradingDate).format('YYYY-MM-DD'));
        }
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}
