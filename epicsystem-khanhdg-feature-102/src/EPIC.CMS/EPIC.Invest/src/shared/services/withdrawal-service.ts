import { AppConsts, ApproveConst, InterestPaymentConst, SortConst, StatusPaymentBankConst } from '@shared/AppConsts';
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
import { OrderWithdrawFilter } from '@shared/interface/filter.model';

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
    
    getAll(page: Page, dataFilter?: OrderWithdrawFilter): Observable<any> {
        let url_ = "/api/invest/withdrawal/get-all?";
        //
        for(let [key,value] of Object.entries(dataFilter)) {
            if((value || value === 0) && !dataFilter.paramIgnore.includes(key)) {
                if(key === 'keyword') key = dataFilter.searchField || key;
                if(value instanceof Date) value = moment(value).format("YYYY-MM-DD");
                url_ += this.convertParamUrl(key, value);
            }
        }
        //
        if(dataFilter?.sortFields?.length) {
            dataFilter.sortFields.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        if(dataFilter?.tradingProviderIds?.length){
            dataFilter.tradingProviderIds.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        }
        
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

    exportInterestContract(page: Page, dataFilter: any, apiExportExcel: string) {
        let url_ = apiExportExcel;

        for( const [key, value] of Object.entries(dataFilter)) {
            if(value || value == 0) {
                if(key == 'searchField') {
                    // TÌM KIẾM KEYWORD THEO LOẠI TÌM KIẾM
                    if(page.keyword) url_ += this.convertParamUrl(dataFilter.searchField, dataFilter.keyword);
                } else if (key == 'ngayChiTra') {
                    // TÌM KIẾM THEO NGÀY CHI TRẢ
                    url_ += this.convertParamUrl("ngayChiTra", moment(dataFilter.ngayChiTra).format("YYYY-MM-DD"));
                } else if(key == 'status') {
                    // LỌC THEO LOẠI ĐÃ CHI TRẢ (TẤT CẢ / TỰ ĐỘNG / THỦ CÔNG)
                    if([InterestPaymentConst.STATUS_DONE_ONLINE, InterestPaymentConst.STATUS_DONE_OFFLINE].includes(+value)) {
                        url_ += this.convertParamUrl(key, InterestPaymentConst.STATUS_DONE);
                        url_ += this.convertParamUrl('interestPaymentStatus', InterestPaymentConst.typeInterests[`${value}`]);
                    } else {
                        url_ += this.convertParamUrl(key, `${value}`);
                    }
                } else if(key == 'tradingProviderIds') {
                    if(dataFilter.tradingProviderIds?.length > 0) {
                        dataFilter?.tradingProviderIds?.forEach(id => {
                            url_ += this.convertParamUrl('tradingProviderIds', id);
                        });
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