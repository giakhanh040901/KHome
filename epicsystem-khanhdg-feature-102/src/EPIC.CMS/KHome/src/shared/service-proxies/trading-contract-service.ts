import { AppConsts, InterestPaymentConst, OrderConst, SortConst, YesNoConst } from '@shared/AppConsts';
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
    filter,
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
import { NavigationEnd, Router } from '@angular/router';

@Injectable()
export class OrderServiceProxy extends ServiceProxyBase {
    previousUrl: string = '';
    currentUrl: string = '';
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        public router: Router,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
        this.router.events
            .pipe(
                filter((event) => event instanceof NavigationEnd)
            ).subscribe((event: NavigationEnd) => {
                this.previousUrl = this.currentUrl;
                this.currentUrl = event.url;
            });
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

    postEkyc(body): Observable<any> {
        
        return this.requestPostFileUtil(body, "/api/core/manager-investor/ekyc");
    }

    // tìm saler theo mã giới thiệu
    getByTradingProvider(referralCode:string): Observable<any> {
        let url_ = `/api/core/manager-sale/find-by-trading-provider?`;
        if(referralCode){
            url_ += this.convertParamUrl("referralCode", referralCode);
        }
        return this.requestGet(url_);
    }

    // lấy ra danh sách mở bán
    getOpenSell(): Observable<any> {
        let url_ = "/api/real-estate/open-sell/find-all?";
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("status", 3);
        return this.requestGet(url_);
    }

    // lấy ra sản phẩm theo openSellId
    getProduct(orderInfo: any): Observable<any> {
        let url_ = "/api/real-estate/open-sell/detail?";
        url_ += this.convertParamUrl("keyword", orderInfo.keyword);
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("openSellId", orderInfo.openSellId);
        return this.requestGet(url_);
    }

    // Api tìm kiếm căn hộ hợp lệ để đặt lệnh cho đại lý
    findProduct(info: any):Observable<any> {
        let url_ = "/api/real-estate/open-sell/get-all-for-order?";
        url_ += this.convertParamUrl("projectId", info.projectId);
        url_ += this.convertParamUrl("keyword", info.keyword);
        return this.requestGet(url_);
    }
    
    // thêm mới sổ lệnh giao dịch cọc
    create(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/order/add");
    }

    addCoOwner(body): Observable<any> {
        return this.requestPost(body, "/api/real-estate/order/add-co-owner");
    }

    deleteCoOwner(id: any,orderId: any ): Observable<void> {
        let url_ = `/api/real-estate/order/delete-co-owner/${id}?`;
        url_ += this.convertParamUrl("orderId", orderId);
        return this.requestDelete(url_);
    }





    //chua dung den
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
        return this.requestPut(body, "/api/real-estate/order/upload-file-scan");
    }

    updateOrderContract(id: number): Observable<any> {
        let url_ = `/api/real-estate/order/update-contract-file/${id}`;
        return this.requestPut(null, url_);
    }

    signOrderContract(id: number): Observable<any> {
        let url_ = "/api/invest/order/sign-contract-file?";
        url_ += this.convertParamUrl("orderId", id);
        return this.requestPut(null, url_);
    }

    getAllDeliveryContract(page: Page, dataFilter?: any): Observable<any> {
        let url_ = '/api/invest/order/find-delivery-status?';
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        if(dataFilter) {
            if(dataFilter?.source) url_ += this.convertParamUrl('source', dataFilter?.source);
            if(dataFilter?.deliveryStatus) url_ += this.convertParamUrl('deliveryStatus', dataFilter?.deliveryStatus);
            if(dataFilter?.dateFilter) {
                if(dataFilter?.dateFilter && dataFilter?.fieldFilter) url_ += this.convertParamUrl(`${dataFilter?.fieldFilter}`, dataFilter?.dateFilter);
                if(dataFilter?.dateFilter && !dataFilter?.fieldFilter) url_ += this.convertParamUrl("date", dataFilter?.dateFilter);
            }
           
        }
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

    downloadFileTempContract(id): Observable<any> {
        let url_ = `/api/real-estate/export-contract/file-temp/${id}`;
        return this.requestDownloadFile(url_);
    }

    downloadFileTempPdfContract(id): Observable<any> {
        let url_ = `/api/real-estate/export-contract/file-temp-pdf/${id}`;
        return this.requestDownloadFile(url_);
    }

    downloadFileSignatureContract(orderContractFileId): Observable<any> {
        let url_ = `/api/real-estate/export-contract/file-signature/${orderContractFileId}`;
        return this.requestDownloadFile(url_);
    }

   

    update(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/order/update");
    }

    updateField(data, fieldInfo: any): Observable<any> {
        let url_ = fieldInfo.apiPath ;
        let body = {
            id: data.id,
            paymentType: data.paymentType
        }
        // url_ += this.convertParamUrl("id", body.id);
        // url_ += this.convertParamUrl(fieldInfo.name, body[fieldInfo.name]);
        return this.requestPut(body, url_);
    }
    //
    updateInfoContactCustomer(body): Observable<any> {
        let url_ = "/api/real-estate/order/update/co-owner" ;
        return this.requestPut(body, url_);
    }

    delete(id): Observable<void> {
        let url_ = "/api/real-estate/order/deleted/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/real-estate/order/find-by-id/" + id;
        return this.requestGet(url_);
    }

    getInfoBusinessCustomer(page: Page, keyword:string) {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", keyword);
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    

    getAll(page: Page, type?: string, status?:number, fieldFilters?: any, sortData?: any[]): Observable<any> {
        if(!type) type = 'order';
        let urlList = {
            order: '/api/real-estate/order/find-all?',
            orderContractProcessing: '/api/real-estate/order/processing/find-all?',
            orderContract: '/api/real-estate/order/active/find-all?',
            orderContractBlockage: '/api/invest/order/find-cancel?',
            orderContractRenewal: '/api/invest/order/renewals-request?',
            investmentHistory: '/api/invest/order/invest-history?',
        }
        //
        let url_ = urlList[type];
        
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        
        if(fieldFilters) {
            for(const [key, value] of Object.entries(fieldFilters)) {
                if(key == 'searchField') {
                    if(page.keyword) url_ += this.convertParamUrl(fieldFilters.searchField, page.keyword);
                } 
                else if(key == 'tradingProviderIds') {} 
                else {
                    // if (key === 'depositDate'){
                    //     continue;
                    // } else {
                        if(value) url_ += this.convertParamUrl(key, `${value}`);
                    // }
                }
            }
        }
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('status', (!!status || status === 0) ? status : "");

        return this.requestGet(url_);
    }

    activeOnline(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/order/change-source/" + id);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/order/approve/" + id);
    }

    extendedTime(order: any): Observable<any> {
        return this.requestPut(order, `/api/real-estate/order/extended-keep-time`);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/order/cancel/" + id);
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
        let url_ = "/api/real-estate/history/order/find-all?";
        url_ += this.convertParamUrl("RealTableId", id);
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
        return this.requestPost(body, "/api/real-estate/order-payment/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/real-estate/order-payment/update");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/real-estate/order-payment/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/order/payment/find" + id;
        return this.requestGet(url_);
    }
    
    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/order-payment/approve/" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/real-estate/order-payment/cancel/" + id);
    }

    resentNotify(ordePaymentId: number): Observable<any> {
        return this.requestPost(null, "/api/invest/order/payment/resend-notification/" + ordePaymentId);
    }

    getAll(page: Page, orderId): Observable<any> {
        let url_ = "/api/real-estate/order-payment/find-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageFullSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        url_ += this.convertParamUrl('orderId', orderId);
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
        return this.requestPost(body, "/api/invest/contract-template/add");
    }

    changeStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/contract-template/change-status?id=" + id);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/invest/contract-template/update/");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/invest/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/invest/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getByOrder(page: Page, orderId): Observable<any> {
        let url_ = `/api/real-estate/order-contract-file/get-all-contract-file/${orderId}`;
        return this.requestGet(url_);
    }

    getAll(params: any): Observable<any> {
        let url_ = `/api/invest/contract-template/find-all/${params.distributionId}`;
        return this.requestGet(url_);
    }

    getAllContractTypeIssuer(page: Page): Observable<any> {
        let url_ = "/api/invest/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    downloadContractTemplateWord(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/invest/export-contract/file-template-word?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/invest/export-contract/file-template-pdf?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }


    /**
     * 
     * @param distributionId 
     * @returns 
     */
    getAllContractForm(page: Page, distributionId : number, fieldFilters: any): Observable<any> {
        let url_ = `/api/invest/contract-template/find-all?`;
        //
        url_ += this.convertParamUrl("distributionId", distributionId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        //
        for(const [key, value] of Object.entries(fieldFilters)) {
           if(fieldFilters[key]) url_ += this.convertParamUrl(`${key}`, fieldFilters[key]);
        }
        //
        return this.requestGet(url_);
    }
    
    createContractForm(body): Observable<any>{
        return this.requestPost(body, "/api/invest/contract-template/add");
    }

    deleteContractForm(id: any): Observable<void> {
        let url_ = `/api/invest/contract-template/delete/` + id;
        return this.requestDelete(url_);
    }

    updateContractForm(body): Observable<any>{
        return this.requestPut(body, "/api/invest/contract-template/update");
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
        let url_ = "/api/invest/interest-payment/find/" + id;
        return this.requestGet(url_);
    }
   
    getAllContractInterest(page: Page, fieldFilters: any, apiUrl): Observable<any> {
        let url_ = apiUrl;
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

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    addListInterest(body): Observable<any> {
        let url_ = "/api/invest/interest-payment/add";
        return this.requestPost(body, url_);
    }
}

@Injectable()
export class SettlementServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllSettlementContract(page: Page, status?: any, dataFilter?: any): Observable<any> {
        let url_ = "/api/invest/order/find-settlement?";
        url_ += this.convertParamUrl(dataFilter?.fieldFilter, page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('status', status ?? '');
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
