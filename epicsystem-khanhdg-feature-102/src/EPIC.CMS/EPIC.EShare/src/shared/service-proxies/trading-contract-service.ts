import { AppConsts, OrderConst } from '@shared/AppConsts';
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

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from './service-proxies';
import * as moment from 'moment';
import { TableBody } from 'primeng/table';

@Injectable()
export class OrderServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    downloadFile(orderId, contractTempId): Observable<any> {
        let url_ = "/api/company-share/export-contract/invest-company-share?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        return this.requestDownloadFile(url_);
    }

    createOrderContractFile(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/order/order-contract-file/add");
    }

    updateOrderContractFile(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/order/order-contract-file/update");
    }

    updateOrderContract(id: number): Observable<any> {
        let url_ = "/api/company-share/order/update-contract-file?";
        url_ += this.convertParamUrl("orderId", id);
        return this.requestPut(null, url_);
    }

    signOrderContract(id: number): Observable<any> {
        let url_ = "/api/company-share/order/sign-contract-file?";
        url_ += this.convertParamUrl("orderId", id);
        return this.requestPut(null, url_);
    }

    updateSecondaryContractFileScan(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/order/scan-contract-file/update");
    }

    createSecondaryContractFileScan(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/order/scan-contract-file/add");
    }

    downloadFileScanContract(orderId, contractTempId, orderContractFileId): Observable<any> {
        let url_ = "/api/company-share/export-contract/file-scan?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        url_ += this.convertParamUrl('secondaryContractFileId', orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileTempContract(orderId, contractTempId, orderContractFileId): Observable<any> {
        let url_ = "/api/company-share/export-contract/file-temp?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        url_ += this.convertParamUrl('secondaryContractFileId', orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileSignatureContract(orderId, contractTempId, orderContractFileId): Observable<any> {
        let url_ = "/api/company-share/export-contract/file-signature?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        url_ += this.convertParamUrl('secondaryContractFileId', orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/order/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/order/update?orderId=" + body.orderId);
    }

    getDueDate(policyDetailId, paymentFullDate): Observable<any> {
        let url_ = "/api/company-share/order/check-investment-day?";
        url_ += this.convertParamUrl("policyDetailId", policyDetailId);
        url_ += this.convertParamUrl("paymentFullDate", paymentFullDate);
        return this.requestGet(url_);
    }


    updateField(body, fieldInfo: any): Observable<any> {
        let url_ = fieldInfo.apiPath;
        url_ += this.convertParamUrl("orderId", body.orderId);
        url_ += this.convertParamUrl(fieldInfo.name, body[fieldInfo.name]);
        return this.requestPut(body, url_);
    }

    delete(id): Observable<void> {
        let url_ = "/api/company-share/order/order/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-shares/order/find/" + id;
        return this.requestGet(url_);
    }

    getPriceDate(companyShareSecondaryId, date = new Date()): Observable<any> {
        let url_ = "/api/company-share/order/price/find?";
        url_ += this.convertParamUrl("companyShareSecondaryId", companyShareSecondaryId);
        url_ += this.convertParamUrl('priceDate', moment(date).format("YYYY/MM/DD"));
        return this.requestGet(url_);
    }

    getInfoBusinessCustomer(page: Page, keyword: string) {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getInfoInvestorCustomer(keyword: string) {
        // let url_ = "/api/core/manager-investor/find-all-list?";
        let url_ = "/api/core/manager-investor/filter?requireKeyword=true&";
        if (keyword) {
            url_ += this.convertParamUrl("keyword", keyword);
        }
        // url_ += this.convertParamUrl("keyword", keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        //
        return this.requestGet(url_);
    }

    getAll(page: Page, type?: string, status?: string, dataFilter?: any): Observable<any> {
        if (!type) type = 'order';

        let urlList = {
            order: '/api/company-shares/order/find?',
            orderContractProcessing: '/api/company-shares/order/find-contract-processing?',
            orderContract: '/api/company-share/order/find-active?',
            orderContractBlockage: '/api/company-shares/order/find-cancel?',
        }

        let url_ = urlList[type];
        if (dataFilter) {
            if (dataFilter?.secondaryId) url_ += this.convertParamUrl('secondaryId', dataFilter?.secondaryId);
            if (dataFilter?.policyId) url_ += this.convertParamUrl('companySharePolicy', dataFilter?.policyId);
            if (dataFilter?.policyDetailId) url_ += this.convertParamUrl('policyDetailId', dataFilter?.policyDetailId);
            if (dataFilter?.source) url_ += this.convertParamUrl('source', dataFilter?.source);
            if (dataFilter?.orderSource) url_ += this.convertParamUrl('orderer', dataFilter?.orderSource);
            if (page.keyword) url_ += this.convertParamUrl(dataFilter?.fieldFilter, page.keyword);
        }
        
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('status', status ?? '');

        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllDeliveryContract(page: Page, deliveryStatus?: number, source?: number): Observable<any> {
        let url_ = '/api/company-shares/order/find-delivery-status?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('deliveryStatus', deliveryStatus ?? '');
        url_ += this.convertParamUrl('source', source ?? '');
        return this.requestGet(url_);
    }

    activeOnline(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/order/update/source?orderId=" + id);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/order/update/approve?orderId=" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/order/update/cancel?orderId=" + id);
    }

    getCoupon(orderId) {
        return this.requestGet("/api/company-share/order/interest/" + orderId);
    }
    
    resentNotify(ordeId: number): Observable<any> {
        return this.requestPost(null, "/api/company-share/order/resend-notification/" + ordeId);
    }

    // Yêu cầu tái tục
    reinstatementRequest(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/renewals-request/request");
    }


}

@Injectable()
export class OrderPaymentServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/order/payment/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/order/payment/update?orderPaymentId=" + body.orderPaymentId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/order/payment/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/order/payment/find" + id;
        return this.requestGet(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/order/payment/approve/" + id + "?status=2");
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/order/payment/approve/" + id + "?status=3");
    }

    resentNotify(ordePaymentId: number): Observable<any> {
        return this.requestPost(null, "/api/company-share/order/payment/resend-notification/" + ordePaymentId);
    }

    getAll(page: Page, orderId): Observable<any> {
        let url_ = "/api/company-share/order/payment/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('orderId', orderId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class InterestPaymentServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/company-share-payment/find/" + id;
        return this.requestGet(url_);
    }

    payInterest(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/interest-payment/change-established-to-paid-status");
    }

    addListInterest(body): Observable<any> {
        let url_ = "/api/company-share/interest-payment/add";
        return this.requestPost(body, url_);
    }

    getAllContractInterest(page: Page, dataFilter: any, apiUrl): Observable<any> {
        let url_ = apiUrl;
        url_ += this.convertParamUrl(dataFilter.fieldFilter, page.keyword);
        if(dataFilter.ngayChiTra) {
            url_ += this.convertParamUrl("ngayChiTra", moment(dataFilter.ngayChiTra).format("YYYY-MM-DD"));
        }

        if(dataFilter.typeInterest) {
            url_ += this.convertParamUrl("status", dataFilter.typeInterest);
        }

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    // getAllContractInterest(page: Page, orderId): Observable<any> {
    //     let url_ = "/api/company-share/interest-payment/find?";
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl('orderId', orderId);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    //     return this.requestGet(url_);
    // }

    // getAllContractInterestDone(page: Page, orderId): Observable<any> {
    //     let url_ = "/api/company-share/interest-payment/find?";
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl('orderId', orderId);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    //     return this.requestGet(url_);
    // }
}


