import { AppConsts, ApproveConst, SortConst, StatusPaymentBankConst } from '@shared/AppConsts';
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

@Injectable()
export class WithdrawalService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    
    getAll(page: Page, fieldFilters?: any, sortData?: any[]): Observable<any> {
        let url_ = "/api/garner/withdrawal/find-all?";
        //
        if(page.keyword) url_ += this.convertParamUrl(fieldFilters.fieldSearch, page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(fieldFilters?.status) url_ += this.convertParamUrl('status', fieldFilters?.status);
        if(fieldFilters?.status == StatusPaymentBankConst.PENDING) {
            url_ += this.convertParamUrl('status', StatusPaymentBankConst.PENDING_ONLINE);
        }
        //
        if(fieldFilters.withdrawalDate) url_ += this.convertParamUrl('withdrawalDate', moment(fieldFilters.withdrawalDate).format('YYYY-MM-DD'));
        if(fieldFilters.approveDate) url_ += this.convertParamUrl('approveDate', moment(fieldFilters.approveDate).format('YYYY-MM-DD'));
        if(fieldFilters.tradingProviderIds?.length > 0) {
            fieldFilters?.tradingProviderIds?.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        } 
        if(sortData){
            console.log('sortData ', sortData);
            sortData.forEach(item => {
                if (item.field != "distributionId"){
                    url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
                }
            })
        }
        return this.requestGet(url_);
    }

    request(body: string) {
        let url_ = "/api/garner/withdrawal/request";
        return this.requestPost(body, url_);
    }

    approveInterestContract(body, status?: any) {
        let url_ = "/api/garner/interest-payment/approve";
        return this.requestPut(body, url_);
    }

    approve(body, interestType?: string) {
       
        let urls = {
            [StatusPaymentBankConst.INTEREST_CONTRACT]: '/api/garner/interest-payment/approve', //  CHI TRẢ LỢI TỨC, ĐÁO HẠN (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.MANAGER_WITHDRAW]: '/api/garner/withdrawal/approve',  // CHI TRẢ RÚT VỐN (CHI TỰ ĐỘNG)
        };

        let url_ = urls[interestType];
        return this.requestPut(body, url_);
    }

    getPolicy(cifCode: string): Observable<any> {
        let url_ = "/api/garner/order/policy/find-by-cifcode?cifCode=" + cifCode;
        return this.requestGet(url_);
    }

    requestBank(body, requestType?: string) {
        let urls = {
            [StatusPaymentBankConst.INTEREST_CONTRACT]: '/api/garner/interest-payment/prepare-approve', //  REQUEST BANK CHI TRẢ LỢI TỨC, ĐÁO HẠN (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.MANAGER_WITHDRAW]: '/api/garner/withdrawal/prepare-approve',  // REQUEST BANK CHI TRẢ RÚT VỐN (CHI TỰ ĐỘNG)
        };

        let url_ = urls[requestType];
        return this.requestPost(body, url_);
    }

    getDistributionBank(distributionId, type) {
        let url_ = "/api/garner/distribution/find-bank-distribution?";
        //
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl("type", type);
        //
        return this.requestGet(url_);
    }

    getOtp(requestId: number) {
        let url_ = "api/payment/msb/request-pay/get-otp/" + requestId;
        return this.requestGet(url_);
    }

    sendOtp() {
        let url_ = "/api/garner/withdrawal/bank/get-otp";
        //
        return this.requestGet(url_);
    }

}
