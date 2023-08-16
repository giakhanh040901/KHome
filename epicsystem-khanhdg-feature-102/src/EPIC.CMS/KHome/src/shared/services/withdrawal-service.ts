import { AppConsts, ApproveConst, InterestPaymentConst, StatusPaymentBankConst } from '@shared/AppConsts';
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
    
    getAll(page: Page, fieldFilters?: any): Observable<any> {
        let url_ = "/api/invest/withdrawal/get-all?";
        //
        url_ += this.convertParamUrl('status', (fieldFilters?.status || ApproveConst.STATUS_REQUEST));
        url_ += this.convertParamUrl(`${fieldFilters.searchField}`, page.keyword);
        if(fieldFilters.requestDate) url_ += this.convertParamUrl('requestDate', moment(fieldFilters.requestDate).format('YYYY-MM-DD'));
        if(fieldFilters.approveDate) url_ += this.convertParamUrl('approveDate', moment(fieldFilters.approveDate).format('YYYY-MM-DD'));
        //
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }

    request(body: string, componentType?: any) {
        let url_ = "/api/invest/withdrawal/request";
         
        return this.requestPost(body, url_);
    }

    approve(body, interestType: string) {
        let urls = {
            [StatusPaymentBankConst.INTEREST_CONTRACT]: '/api/invest/interest-payment/approve-payment', //  CHI TRẢ LỢI TỨC, ĐÁO HẠN (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.RENEWAL_CONTRACT]: '/api/invest/interest-payment/approve-payment-renewal',  // TÁI TỤC HỢP ĐỒNG (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.MANAGER_WITHDRAW]: '/api/invest/withdrawal/approve-withdrawal',  // CHI TRẢ RÚT VỐN (CHI TỰ ĐỘNG)
        };

        let url_ = urls[interestType];
        return this.requestPut(body, url_);
    }

    exportInterestCreateList(page: Page, fieldFilters: any, apiExportExcel: string) {
        let url_ = apiExportExcel;

        for( const [key, value] of Object.entries(fieldFilters)) {
            if(value || value == 0) {
                if(key == 'searchField') {
                    // TÌM KIẾM KEYWORD THEO LOẠI TÌM KIẾM
                    if(page.keyword) url_ += this.convertParamUrl(fieldFilters.searchField, page.keyword);
                } else if (key == 'ngayChiTra') {
                    // TÌM KIẾM THEO NGÀY CHI TRẢ
                    url_ += this.convertParamUrl("ngayChiTra", moment(fieldFilters.ngayChiTra).format("YYYY-MM-DD"));
                } else if(key == 'status') {
                    // LỌC THEO LOẠI ĐÃ CHI TRẢ (TẤT CẢ / TỰ ĐỘNG / THỦ CÔNG)
                    if([InterestPaymentConst.STATUS_DONE_ONLINE, InterestPaymentConst.STATUS_DONE_OFFLINE].includes(+value)) {
                        url_ += this.convertParamUrl(key, InterestPaymentConst.STATUS_DONE);
                        url_ += this.convertParamUrl('interestPaymentStatus', InterestPaymentConst.typeInterests[`${value}`]);
                    } else {
                        url_ += this.convertParamUrl(key, `${value}`);
                    }
                } else {
                    url_ += this.convertParamUrl(key, `${value}`);
                }
            }
        }
        return this.requestDownloadFile(url_);
    }
    
    exportInterestDone(body, interestType: string) {
        let urls = {
            [StatusPaymentBankConst.INTEREST_CONTRACT]: '/api/invest/interest-payment/approve-payment', //  CHI TRẢ LỢI TỨC, ĐÁO HẠN (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.RENEWAL_CONTRACT]: '/api/invest/interest-payment/approve-payment-renewal',  // TÁI TỤC HỢP ĐỒNG (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.MANAGER_WITHDRAW]: '/api/invest/withdrawal/approve-withdrawal',  // CHI TRẢ RÚT VỐN (CHI TỰ ĐỘNG)
        };

        let url_ = urls[interestType];
        return this.requestPut(body, url_);
    }

    requestBank(body, requestType: string) {
        let urls = {
            [StatusPaymentBankConst.INTEREST_CONTRACT]: '/api/invest/interest-payment/prepare-approve', //  REQUEST BANK CHI TRẢ LỢI TỨC, ĐÁO HẠN (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.RENEWAL_CONTRACT]: '/api/invest/interest-payment/prepare-approve',  // REQUEST BANK TÁI TỤC HỢP ĐỒNG (CHI TỰ ĐỘNG)
            [StatusPaymentBankConst.MANAGER_WITHDRAW]: '/api/invest/withdrawal/prepare-approve',  // REQUEST BANK CHI TRẢ RÚT VỐN (CHI TỰ ĐỘNG)
        };

        let url_ = urls[requestType];
        return this.requestPut(body, url_);
    }

    getDistributionBank(distributionId, type) { 
        let url_ = "/api/invest/distribution/find-bank-distribution?";
        //
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl("type", type);
        //
        return this.requestGet(url_);
    }

    getOtp(requestId: number, tradingBankAccId: number) {
        let url_ = `/api/payment/msb/request-pay/get-otp/${requestId}?tradingBankAccId=${tradingBankAccId}`;
        return this.requestPost(null, url_);
    }
}