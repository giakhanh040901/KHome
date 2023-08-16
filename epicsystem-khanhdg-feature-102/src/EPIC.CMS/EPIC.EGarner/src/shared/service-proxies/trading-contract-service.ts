import { AppConsts, OrderConst,OrderPaymentConst, SortConst } from '@shared/AppConsts';
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
import { MessageService, TreeNode } from 'primeng/api';
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
        let url_ = "/api/bond/export-contract/invest-bond?";
        url_ += this.convertParamUrl("orderId", orderId);
        url_ += this.convertParamUrl('contractTemplateId', contractTempId);
        return this.requestDownloadFile(url_);
    }

    downloadOrderTemple(): Observable<any>{
        return this.requestDownloadFile(`/api/garner/order/import-template`);
    }

    importFromExcel(body): Observable<any> {
        return this.requestPostFileUtil(body, `/api/garner/order/import`);
    }

    createOrderContractFile(body): Observable<any> {
        return this.requestPost(body, "/api/garner/order/order-contract-file/add");
    }

    updateOrderContractFile(orderContractFile): Observable<any> {
        let url_ = `/api/garner/order/update-file-scan?`;
        url_ += this.convertParamUrl("Id", orderContractFile.orderContractFileId);
        url_ += this.convertParamUrl('FileScanUrl', orderContractFile.FileScanUrl);
        if(orderContractFile.FileSignatureUrl) {
            url_ += this.convertParamUrl('FileSignatureUrl', orderContractFile.FileSignatureUrl);
        }
        if(orderContractFile.FileTempUrl) {

            url_ += this.convertParamUrl('FileTempUrl', orderContractFile.FileTempUrl);
        }

        return this.requestPut(null, url_);
    }

    updateOrderContract(id: number): Observable<any> {
        let url_ = "/api/garner/order/update-contract-file/"+id;
        return this.requestPut(null, url_);
    }

    signOrderContract(id: number): Observable<any> {
        let url_ = "/api/garner/order/sign-contract-file/"+id;
        return this.requestPut(null, url_);
    }

    getAllDeliveryContract(page: Page, deliveryStatus?:number, source?:number, tradingProviderIds?:any, sortData?: any[]): Observable<any> {
        let url_ = '/api/garner/order/active/find-all-delivery?';
        url_ += this.convertParamUrl("contractCode", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl("source", source ?? '');
        url_ += this.convertParamUrl('deliveryStatus', deliveryStatus ?? '');
        if (tradingProviderIds.length > 0){
            tradingProviderIds?.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        }
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        return this.requestGet(url_);
    }

    updateSecondaryContractFileScan(body): Observable<any> {
        return this.requestPut(body, "/api/garner/order/scan-contract-file/update");
    }

    updateInfoContactCustomer(body): Observable<any> {
        let url_ = "/api/garner/order/update/info-customer" ;
        return this.requestPut(body, url_);
    }

    downloadFileScanContract(orderContractFileId): Observable<any> {
        let url_ = "/api/garner/export-contract/file-scan?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileTempContract(orderContractFileId): Observable<any> {
        let url_ = "/api/garner/export-contract/file-temp?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileTempPdfContract(orderContractFileId): Observable<any> {
        let url_ = "/api/garner/export-contract/file-temp-pdf?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    downloadFileSignatureContract(orderContractFileId): Observable<any> {
        let url_ = "/api/garner/export-contract/file-signature?";
        url_ += this.convertParamUrl("orderContractFileId", orderContractFileId);
        return this.requestDownloadFile(url_);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/garner/order/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/garner/order/update");
    }

    updateField(body, fieldInfo: any): Observable<any> {
        let url_ = `${fieldInfo.apiPath}/`+ body.id+ `?`;
        if(fieldInfo.name != 'bankAccId') {
            url_ += this.convertParamUrl(fieldInfo.name, body[fieldInfo.name]);
        } else if(fieldInfo.name == 'bankAccId') {
            if(body.investorBankAccId) url_ += this.convertParamUrl('investorBankAccId', body?.investorBankAccId);
            if(body.businessCustomerBankAccId) url_ += this.convertParamUrl('businessCustomerBankAccId', body?.businessCustomerBankAccId);
        }
        return this.requestPut(null, url_);
    }

    delete(ids): Observable<void> {
        let url_ = "/api/garner/order/deleted";
        return this.requestPut(ids, url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/garner/order/find-by-id/" + id;
        return this.requestGet(url_);
    }

    getPolicyForCifcode(cifCode) {
        let url_ = "/api/garner/order/policy/find-by-cifcode/" + encodeURI(cifCode);
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

    getAll(page: Page, type?: string, status?: any, dataFilter?: any, sortData?: any[]): Observable<any> {
        if(!type) type = 'order';
        let urlList = {
            order: '/api/garner/order/find-all?',
            orderContractProcessing: '/api/garner/order/processing/find-all?',
            orderContract: '/api/garner/order/active/find-all?',
            orderContractBlockage: '/api/garner/order-lockdown/find-all?',
        }
        let url_ = urlList[type];

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(page.keyword) url_ += this.convertParamUrl(dataFilter.fieldFilter, page.keyword);
        if(status) url_ += this.convertParamUrl('status', status ?? '');
        //
        if(dataFilter) {
            if(dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
            if(dataFilter?.policyId) url_ += this.convertParamUrl('policyId', dataFilter?.policyId);
            if(dataFilter?.source) url_ += this.convertParamUrl('source', dataFilter?.source);
            if (dataFilter?.orderSource) url_ += this.convertParamUrl('orderer', dataFilter?.orderSource);
            if (dataFilter?.orderer) url_ += this.convertParamUrl('orderer', dataFilter?.orderer);
            if(dataFilter.tradingProviderIds?.length > 0) {
                dataFilter?.tradingProviderIds?.forEach(id => {
                    url_ += this.convertParamUrl('tradingProviderIds', id);
                });
            } 
        }

        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        return this.requestGet(url_);
    }

    getProduct(): Observable<any> {
        let url_ = "/api/garner/distribution/distribution/get-all";
        return this.requestGet(url_);
    }

    getAllBankByCifcode(cifcode: any): Observable<any> {
        return this.requestGet("/api/garner/order/banks/find-by-cifcode/" + cifcode);
    }

    getAllGarnerHistory(page: Page, status?: any, dataFilter?: any, sortData?: any[]): Observable<any> {
        let url_ = "/api/garner/order/settlement/find-all?"
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(page.keyword) url_ += this.convertParamUrl(dataFilter.fieldFilter, page.keyword);
        if(status) url_ += this.convertParamUrl('status', status ?? '');
        //
        if(dataFilter) {
            if(dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
            if(dataFilter?.policyId) url_ += this.convertParamUrl('policyId', dataFilter?.policyId);
            if(dataFilter?.source) url_ += this.convertParamUrl('source', dataFilter?.source);
            if (dataFilter?.orderSource) url_ += this.convertParamUrl('orderer', dataFilter?.orderSource);
            if (dataFilter?.orderer) url_ += this.convertParamUrl('orderer', dataFilter?.orderer);
            if (dataFilter?.settlementDate) url_ += this.convertParamUrl('settlementDate', moment(dataFilter?.settlementDate).format("YYYY-MM-DD"));
            if(dataFilter.tradingProviderIds?.length > 0) {
                dataFilter?.tradingProviderIds?.forEach(id => {
                    url_ += this.convertParamUrl('tradingProviderIds', id);
                });
            } 
        }

        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        return this.requestGet(url_);
    }

    activeOnline(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order/update/source/" + id);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order/approve/" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order/update/cancel/" + id);
    }

    resentNotify(ordeId: number): Observable<any> {
        return this.requestPost(null, "/api/garner/order/resend-notify/" + ordeId);
    }

    getCoupon(orderId) {
        return this.requestGet("/api/garner/order/interest/" + orderId);
    }

    getHistory(page: Page, id: number) {
        let url_ = "/api/garner/order/find-all-history?";
        url_ += this.convertParamUrl("RealTableId", id);
        url_ += this.convertParamUrl("UploadTable", 4);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getCashFlow(page: Page, id: number) {
        let url_ = "/api/garner/order/profit-future/" + id;
        // url_ += this.convertParamUrl('pageSize', page.pageSize);
        // url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    blockadeContractActive(body): Observable<any>{
        return this.requestPost(body, "/api/garner/blockade-liberation/add");
    }

    reinstatementRequest(body): Observable<any> {
        return this.requestPost(body, "/api/garner/renewals-request/request");
    }

    withdrawalRequest(body): Observable<any> {
        return this.requestPost(body, "/api/garner/withdrawal/add");
    }

    takeHardContract(orderId: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order/process-contract/" + orderId);
    }


    // ORDER GROUP THEO KHÁCH HÀNG VÀ CHÍNH SÁCH

    getAllOrderGroup() {
        return this.http.get<any>('assets/customers-medium.json')
            .toPromise()
            .then(data => { return data; });
    }

    getAllGroupPolicy(page,dataFilter?: any, sortData?: any[]) {
        let url_ = '/api/garner/order/policy/find-all?';
        //
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        if(page.keyword) url_ += this.convertParamUrl(dataFilter.fieldFilter, page.keyword);
        if(dataFilter?.productType) url_ += this.convertParamUrl('productType', dataFilter?.productType);
        if(dataFilter?.distributionId) url_ += this.convertParamUrl('distributionId', dataFilter?.distributionId);
        if(dataFilter?.policyId) url_ += this.convertParamUrl('policyId', dataFilter?.policyId);
        if(dataFilter?.status) url_ += this.convertParamUrl('status', dataFilter?.status);
        if(dataFilter.tradingProviderIds?.length > 0) {
            dataFilter?.tradingProviderIds?.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        } 
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        return this.requestGet(url_);
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
        return this.requestPost(body, "/api/garner/order-payment/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/garner/order-payment/update");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/garner/order-payment/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/garner/order/payment/find" + id;
        return this.requestGet(url_);
    }
    
    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order-payment/approve/" + id + "?status="+OrderPaymentConst.PAYMENT_APPROVE);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/order-payment/approve/" + id + "?status="+OrderPaymentConst.PAYMENT_CLOSE);
    }

    resentNotify(ordePaymentId: number): Observable<any> {
        return this.requestPost(null, "/api/garner/order-payment/resend-notify/" + ordePaymentId);
    }

    getAll(page: Page, orderId): Observable<any> {
        let url_ = "/api/garner/order-payment/find-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('orderId', orderId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class ContractTemplateServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/garner/contract-template/add");
    }

    changeStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/contract-template/change-status?id=" + id);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/garner/contract-template/update/");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/garner/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/garner/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getByOrder(page: Page, orderId): Observable<any> {
        let url_ = "/api/garner/contract-template/find-by-order?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (orderId) {
            url_ += this.convertParamUrl("orderId", orderId);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAll(page: Page, params: any): Observable<any> {
        let url_ = "/api/garner/contract-template/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("distributionId", params?.distributionId);
        url_ += this.convertParamUrl("type", params?.type);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllContractTypeIssuer(page: Page): Observable<any> {
        let url_ = "/api/garner/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    downloadContractTemplateWord(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/garner/export-contract/file-template-word?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/garner/export-contract/file-template-pdf?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
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
        let url_ = "/api/garner/interest-payment/find/" + id;
        return this.requestGet(url_);
    }

    payInterest(body): Observable<any> {
        return this.requestPut(body, "/api/garner/interest-payment/change-established-to-paid-status");
    }

    getAllContractInterest(page: Page, dataFilter: any, apiUrl, sortData?: any[]): Observable<any> {
        console.log("dataFilter",dataFilter);
        
        let url_ = apiUrl;
        if(page.keyword) {
            url_ += this.convertParamUrl(dataFilter.searchField, page.keyword);
        }
        if(dataFilter.ngayChiTra) {
            url_ += this.convertParamUrl("payDate", moment(dataFilter.ngayChiTra).format("YYYY-MM-DD"));
        }
        if(dataFilter.isExactDate) {
            url_ += this.convertParamUrl("isExactDate", dataFilter.isExactDate);
        }
        // if(dataFilter?.status) {
            url_ += this.convertParamUrl("status", dataFilter.status);
        // }

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if(dataFilter.tradingProviderIds?.length > 0) {
            dataFilter?.tradingProviderIds?.forEach(id => {
                url_ += this.convertParamUrl('tradingProviderIds', id);
            });
        } 
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        return this.requestGet(url_);
    }

    addListInterest(body): Observable<any> {
        let url_ = "/api/garner/interest-payment/add";
        return this.requestPost(body, url_);
    }
}


