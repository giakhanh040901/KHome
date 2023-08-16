import { SortConst } from '@shared/AppConsts';
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
import * as moment from 'moment';
import { OrderDeliveryFilter, OrderFilter } from '@shared/interface/filter.model';

@Injectable()
export class OrderService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    downloadFile(orderId, contractTempId): Observable<any> {
        let url_ = "/api/bond/export-contract/invest-bond?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        return this.requestDownloadFile(url_);
    }

    createOrderContractFile(body): Observable<any> {
        return this.requestPost(body, "/api/invest/order/order-contract-file/add");
    }

    updateOrderContractFile(body): Observable<any> {
        return this.requestPut(body, "/api/invest/order/order-contract-file/update");
    }

    updateOrderContract(id: number): Observable<any> {
        let url_ = "/api/invest/order/update-contract-file?";
        url_ += this.convertParamUrl("orderId", id);
        return this.requestPut(null, url_);
    }

    signOrderContract(id: number): Observable<any> {
        let url_ = "/api/invest/order/sign-contract-file?";
        url_ += this.convertParamUrl("orderId", id);
        return this.requestPut(null, url_);
    }

    getAllDeliveryContract(page: Page, dataFilter?: OrderDeliveryFilter): Observable<any> {
        let url_ = '/api/invest/order/find-delivery-status?';
        for(let [key, value] of Object.entries(dataFilter)) {
            if(value && !dataFilter?.paramIgnore.includes(key)) {
                if(key === 'date') {
                    key = dataFilter.typeDate;
                    value = moment(value).format("YYYY-MM-DD")
                }
                url_ += this.convertParamUrl(key, value);
            }
        }
        //
        if(dataFilter.tradingProviderIds?.length) {
            dataFilter.tradingProviderIds.forEach(tradingProviderId => {
                url_ += this.convertParamUrl('tradingProviderIds', tradingProviderId);
            });
        } 
        //
        if(dataFilter?.sortFields?.length){
            dataFilter.sortFields.forEach(item => {
                if (item.field == 'policyDetailName'){
                    url_ += this.convertParamUrl('Sort', `policyDetail.periodType-${SortConst.getValueSort(item.order)}`);
                    url_ += this.convertParamUrl('Sort', `policyDetail.periodQuantity-${SortConst.getValueSort(item.order)}`);
                } else{
                    url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
                }
            })
        }

        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.getPageSize());

        return this.requestGet(url_);
    }

    updateSecondaryContractFileScan(body): Observable<any> {
        return this.requestPut(body, "/api/invest/order/scan-contract-file/update");
    }

    downloadFileScanContract(orderContractFileId): Observable<any> {
        let url_ = "/api/invest/export-contract/file-scan?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileTempContract(orderContractFileId): Observable<any> {
        let url_ = "/api/invest/export-contract/file-temp?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileTempPdfContract(orderContractFileId): Observable<any> {
        let url_ = "/api/invest/export-contract/file-temp-pdf?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileSignatureContract(orderContractFileId): Observable<any> {
        let url_ = "/api/invest/export-contract/file-signature?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/invest/order/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/order/update?orderId=" + body.id);
    }

    updateField(body, fieldInfo: any): Observable<any> {
        let url_ = fieldInfo.apiPath ;
        url_ += this.convertParamUrl("id", body.id);
        if(body[fieldInfo.name] !== null) url_ += this.convertParamUrl(fieldInfo.name, body[fieldInfo.name]);
        return this.requestPut(body, url_);
    }
    //
    updateInfoContactCustomer(body): Observable<any> {
        let url_ = "/api/invest/order/update/info-customer" ;
        return this.requestPut(body, url_);
    }

    delete(ids): Observable<void> {
        let url_ = "/api/invest/order/delete";
        return this.requestPut(ids, url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/order/find/" + id;
        return this.requestGet(url_);
    }

    getInfoBusinessCustomer(page: Page, keyword:string) {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getInfoInvestorCustomer(keyword:string) {
        // let url_ = "/api/core/manager-investor/find-all-list?";
        let url_ = "/api/core/manager-investor/filter?requireKeyword=true&";
        if(keyword) {
			url_ += this.convertParamUrl("keyword", keyword);
		}
        url_ += this.convertParamUrl('pageSize', -1);
        //
        return this.requestGet(url_);
    }

    getAll(page: Page, type?: string, dataFilter?: OrderFilter, sortData?: any[]): Observable<any> {
        if(!type) type = 'order';
        let urlList = {
            order: '/api/invest/order/find?',
            orderContractProcessing: '/api/invest/order/find-contract-processing?',
            orderContract: '/api/invest/order/find-active?',
            orderContractBlockage: '/api/invest/order/find-cancel?',
            orderContractRenewal: '/api/invest/order/renewals-request?',
            investmentHistory: '/api/invest/order/invest-history?',
        }
        //
        let url_ = urlList[type];
       
        for(let [key, value] of Object.entries(dataFilter)) {
            if(value && !dataFilter?.paramIgnore.includes(key)) {
                if(key === 'keyword') key = dataFilter.searchField || key;
                if(value instanceof Date) value = moment(value).format("YYYY-MM-DD");
                url_ += this.convertParamUrl(key, value);
            }
        }

        if(dataFilter?.policy?.length) {
            url_ += this.convertParamUrl('policy', dataFilter.policy.join(','));
        }

        if(dataFilter?.tradingProviderIds?.length) {
            dataFilter.tradingProviderIds.forEach(tradingProviderId => {
                url_ += this.convertParamUrl('tradingProviderIds', tradingProviderId);
            });
        } 

        if(dataFilter?.sortFields?.length){
            dataFilter.sortFields.forEach(item => {
                if (item.field == 'policyDetailName'){
                    url_ += this.convertParamUrl('Sort', `policyDetail.periodType-${SortConst.getValueSort(item.order)}`);
                    url_ += this.convertParamUrl('Sort', `policyDetail.periodQuantity-${SortConst.getValueSort(item.order)}`);
                }
                else {
                    url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
                }
            })
        }

        url_ += this.convertParamUrl('pageSize', page.getPageSize());
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    activeOnline(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/update/source?id=" + id);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/update/approve?id=" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/update/cancel?id=" + id);
    }

    cancelSign(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/order-contract-file/update-is-sign?orderId=" + id);
    }

    resentNotify(ordeId: number): Observable<any> {
        return this.requestPost(null, "/api/invest/order/resend-notification/" + ordeId);
    }

    takeHardContract(ordeId: number): Observable<any> {
        return this.requestPut(null, "/api/invest/order/process-contract?orderId=" + ordeId);
    }

    getCoupon(orderId) {
        return this.requestGet("/api/invest/order/interest/" + orderId);
    }

    getHistory(page: Page, id: number) {
        let url_ = "/api/invest/order/get-order-history-update?";
        url_ += this.convertParamUrl("orderId", id);
        // url_ += this.convertParamUrl("keyword", keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    blockadeContractActive(body): Observable<any>{
        return this.requestPost(body, "/api/invest/blockade-liberation/add");
    }

    reinstatementRequest(body): Observable<any> {
        return this.requestPost(body, "/api/invest/renewals-request/request");
    }

    withdrawalRequest(body): Observable<any> {
        return this.requestPost(body, "/api/invest/withdrawal/add");
    }

    cancelRenewal(ordeId): Observable<any> {
        return this.requestPut(null, "/api/invest/order/cancel-renewal-request?orderId=" + ordeId);
    }

    public viewFilePDF(url: string) {
        return this.viewFilePDFFromAPI(url);
    }
}