import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { AppConsts, CompanyShareInfoConst, CompanySharePrimaryConst, CompanyShareSecondaryConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageCompanySharePolicyTemplate } from "@shared/model/pageCompanySharePolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";

/**
 * PHÁT HÀNH THỨ CẤP
 */
@Injectable()
export class CompanyShareSecondaryServiceProxy extends ServiceProxyBase {
    private secondaryEndPoint = `/api/company-shares/secondary`;
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    /**
     * TẠO PHÁT HÀNH THỨ CẤP
     * @param body
     * @returns
     */
     create(body): Observable<any> {
        return this.requestPost(body, `${this.secondaryEndPoint}/add`);
    }

    /**
     * CẬP NHẬT PHÁT HÀNH THỨ CẤP
     * @param body
     * @param id
     * @returns
     */
    update(body): Observable<any> {
        return this.requestPut(body, `${this.secondaryEndPoint}/update`);
    }

    updateOverview(body): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-secondary/update-overview");
    }

    delete(id: number): Observable<void> {
        let url_ = `${this.secondaryEndPoint}/delete/` + id;
        return this.requestDelete(url_);
    }

    request(body): Observable<any> {
        return this.requestPost(body, "/api/bond/secondary/request");
    }
    
    approve(body): Observable<any> {
        return this.requestPut(body, "/api/bond/secondary/approve");
    }

    cancel(body): Observable<any> {
        return this.requestPut(body, "/api/bond/secondary/cancel");
    }

    getOverviewById(id: number): Observable<any> {
        let url_ = "/api/bond/product-bond-secondary/over-view-secondary-by-id?bondSecondaryid=" + id;
        return this.requestGet(url_);
    }

    getById(id: number): Observable<any> {
        let url_ = `${this.secondaryEndPoint}/` + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status: string, isClose?: string): Observable<any> {
        let url_ = `${this.secondaryEndPoint}/find?`;
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl('status', status ?? '');
        url_ += this.convertParamUrl('isClose', isClose ?? '');

        return this.requestGet(url_);
    }
    
    getAllNoPaging(): Observable<any> {
        let url_ = `${this.secondaryEndPoint}/find?`;
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl('status', CompanyShareSecondaryConst.STATUS.HOAT_DONG);

        return this.requestGet(url_);
    }

    getAllSecondary(): Observable<any> {
        let url_ = "/api/company-shares/secondary/find-secondary-order";
        return this.requestGet(url_);
    }

    getAllCompanySharePrimary(): Observable<any> {
        let url_ = "/api/company-shares/secondary/find";
        return this.requestGet(url_);
    }

    /**
     * TRÌNH DUYỆT PHÁT HÀNH THỨ CẤP    
     * @param body
     * @param id
     * @returns
     */
    submit(secondaryId): Observable<any> {
        return this.requestPut({}, `${this.secondaryEndPoint}/trading-provider-submit/${secondaryId}`);
    }

    /**
     * ĐLSC DUYỆT/BỎ DUYỆT PHÁT HÀNH THỨ CẤP
     * @param body
     * @param secondaryId
     * @param status
     * @returns
     */
    changeStatus(secondaryId, status): Observable<any> {
        let url = `${this.secondaryEndPoint}/trading-provider-approve/${secondaryId}?`;
        url += this.convertParamUrl("status", status);

        return this.requestPut({}, url);
    }

    /**
     * BAT TAT CLOSED
     * @param secondaryId
     * @param isCancel
     * @returns
     */
    toggleIsClosed(secondaryId, isClose): Observable<any> {
        let url = `${this.secondaryEndPoint}/is-close/${secondaryId}?`;
        url += this.convertParamUrl("isClose", isClose);

        return this.requestPut(null, url);
    }

    /**
     * BAT TAT PHAT HANH THU CAP SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    toggleIsShowApp(secondaryId, isShowApp): Observable<any> {
        let url = `${this.secondaryEndPoint}/is-show-app/${secondaryId}?`;
        url += this.convertParamUrl("isShowApp", isShowApp);

        return this.requestPut(null, url);
    }

    /**
     * BAT TAT POLICY SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    toggleIsShowAppPolicy(policyId, isShowApp): Observable<any> {
        let url = `${this.secondaryEndPoint}/policy-is-show-app/${policyId}?`;
        url += this.convertParamUrl("isShowApp", isShowApp);
        return this.requestPut(null, url);
    }
    //
    changeStatusPolicy(policyId): Observable<any> {
        let url = `${this.secondaryEndPoint}/change-policy-status?`;
        url += this.convertParamUrl("id", policyId);
        return this.requestPut(null, url);
    }

    /**
     * BAT TAT POLICY DETAIL SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    toggleIsShowAppPolicyDetail(policyDetailId, isShowApp): Observable<any> {
        let url = `${this.secondaryEndPoint}/policy-detail-is-show-app/${policyDetailId}?`;
        url += this.convertParamUrl("isShowApp", isShowApp);
        return this.requestPut(null, url);
    }
    //
    changeStatusPolicyDetail(policyDetailId): Observable<any> {
        let url = `${this.secondaryEndPoint}/change-policy-detail-status?`;
        url += this.convertParamUrl("id", policyDetailId);
        return this.requestPut(null, url);
    }

    /**
     * TẠO CHÍNH SÁCH
     * @param body
     * @returns
     */
    addPolicy(body): Observable<any> {
        return this.requestPost(body, `${this.secondaryEndPoint}/add-policy`);
    }
    /**
     * SỬA CHÍNH SÁCH
     * @param body
     * @returns
     */
    updatePolicy(body, id): Observable<any> {
        return this.requestPut(body, `${this.secondaryEndPoint}/update-policy/${id}`);
    }
    /**
     * XOÁ CHÍNH SÁCH
     * @param body
     * @returns
     */
    deletePolicy(id): Observable<any> {
        return this.requestDelete(`${this.secondaryEndPoint}/delete-policy/${id}`);
    }
    /**
     * TẠO CHÍNH SÁCH CON
     * @param body
     * @returns
     */
    addPolicyDetail(body): Observable<any> {
        return this.requestPost(body, `${this.secondaryEndPoint}/add-policy-detail`);
    }
    /**
     * SỬA CHÍNH SÁCH CON
     * @param body
     * @returns
     */
    updatePolicyDetail(body, id): Observable<any> {
        return this.requestPut(body, `${this.secondaryEndPoint}/update-policy-detail/${id}`);
    }
    /**
     * XOÁ CHÍNH SÁCH CON
     * @param body
     * @returns
     */
    deletePolicyDetail(id): Observable<any> {
        return this.requestDelete(`${this.secondaryEndPoint}/delete-policy-detail/${id}`);
    }

    importPriceFromExcel(body, bondSecondaryId: number): Observable<any> {
        return this.requestPostFileUtil(body, `${this.secondaryEndPoint}/import-price-from-excel/${bondSecondaryId}`);
    }

    getAllSecondPrice(page: Page, bondSecondaryId: number): Observable<any> {
        let url_ = `${this.secondaryEndPoint}/find-second-price?`;
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl("bondSecondaryId", bondSecondaryId);

        return this.requestGet(url_);
    }

    deleteSecondPrice(bondSecondaryId: number): Observable<any> {
        return this.requestDelete(`${this.secondaryEndPoint}/delete-second-price/${bondSecondaryId}`);
    }

    check(body): Observable<any> {
        return this.requestPut(body, "/api/bond/secondary/check");
    }
    getAllPolicyDetail(id): Observable<any>{ 
        let url_ = "/api/bond/secondary/find-all-policy-detail/"+id;
        return this.requestGet(url_);
    }
    updateSecondPrice(body): Observable<any> {
        return this.requestPut(body, `${this.secondaryEndPoint}/update-second-price`);
    }

}

@Injectable()
export class CompanyShareSecondaryFileServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/policy-file/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/policy-file/update/" + body.policyFileId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/policy-file/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/policy-file/find/" + id;
        return this.requestGet(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/distribution-contract/file/approve/" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/distribution-contract/file/cancel/" + id);
    }

    getAll(page: Page, companyShareSecondaryId): Observable<any> {
        let url_ = "/api/company-share/policy-file/fileAll/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('companyShareSecondaryId', companyShareSecondaryId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}
@Injectable()
export class CompanyShareDetailServiceProxy extends ServiceProxyBase {
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/company-share-detail/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/issuer/update/" + body.companyShareDetailId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/company-share-detail/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/company-share-detail/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, market?: string): Observable<any> {
        let url_ = "/api/company-share/company-share-detail/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (market) {
            url_ += this.convertParamUrl("market", market);
        }
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllCompanyShareInfo(page: Page): Observable<any> {
        let url_ = "/api/company-share/company-share-info/find?";
        url_ += this.convertParamUrl("pageSize", -1);

        return this.requestGet(url_);
    }
}

@Injectable()
export class CompanyShareInfoServiceProxy extends ServiceProxyBase {
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/cps-info/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/cps-info/update/" + body.id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-shares/cps-info/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-shares/cps-info/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status: string): Observable<any> {
        let url_ = "/api/company-shares/cps-info/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl("status", status ?? '');
        return this.requestGet(url_);
    }
 
    getAllCompanyShareInfo(page: Page, status: string, isCheck:string, issueDate:any, dueDate:any): Observable<any> {
        let url_ = "/api/company-shares/cps-info/find?";
        url_ += this.convertParamUrl("issueDate", issueDate??'');
        url_ += this.convertParamUrl("dueDate", dueDate?? '');
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl("status", status ?? '');
        url_ += this.convertParamUrl("isCheck", isCheck ??'');
       
        return this.requestGet(url_);
    }

    getAllIssuer(page: Page): Observable<any> {
        let url_ = "/api/bond/issuer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllDepositProvider(page: Page): Observable<any> {
        let url_ = "/api/bond/deposit-provider/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllBondType(): Observable<any> {
        let url_ = "/api/bond/product-bond-type/find?";
        url_ += this.convertParamUrl("pageSize", -1);
        return this.requestGet(url_);
    }

    request(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/cps-info/request");
    }

    approve(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/cps-info/approve");
    }

    cancel(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/cps-info/cancel");
    }

    closeOrOpen(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-shares/cps-info/close-open?id=" + id);
    }

    check(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/cps-info/check");
    }

    getCoupon(id: number): Observable<any> {
        return this.requestGet("/api/company-shares/cps-info/coupon/" + id);
    }
}

@Injectable()
export class CompanySharePrimaryServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/product-bond-primary/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-primary/update/" + body.bondPrimaryId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/product-bond-primary/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/product-bond-primary/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status: string): Observable<any> {
        let url_ = "/api/bond/product-bond-primary/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('status', status ?? '');

        return this.requestGet(url_);
    }

    request(body): Observable<any> {
        return this.requestPost(body, "/api/bond/product-bond-primary/request");
    }

    cancel(body): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-primary/cancel");
    }

    check(body): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-primary/check");
    }

    approve(body): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-primary/approve");
    }

    // close(id: number): Observable<any> {
    //     return this.requestPut(null, "/api/bond/product-bond-primary/change-approve-to-close/" + id);
    // }

    getAllList(): Observable<any> {
        let url_ = "/api/bond/product-bond-primary/find?";
        url_ += this.convertParamUrl('pageSize', -1);

        return this.requestGet(url_);
    }

    getFindForTradingProvider(tradingProdviderId): Observable<any> {
        let url_ = "/api/bond/product-bond-primary/find-by-trading/" + tradingProdviderId;
        return this.requestGet(url_);
    }

    getAllCompanyShareInfo(page: Page): Observable<any> {
        let url_ = "/api/bond/product-bond-info/find?";
        url_ += this.convertParamUrl('pageSize', -1);
        url_ += this.convertParamUrl('status', CompanyShareInfoConst.HOAT_DONG);

        return this.requestGet(url_);
    }

    getAllTradingProvider(page: Page): Observable<any> {
        let url_ = "/api/bond/trading-provider/find?";
        url_ += this.convertParamUrl("pageSize", -1);

        return this.requestGet(url_);
    }

    getAllBondType(): Observable<any> {
        let url_ = "/api/bond/product-bond-type/find?";
        url_ += this.convertParamUrl('pageSize', -1);
        return this.requestGet(url_);
    }
}

@Injectable()
export class ReceiveContractTemplateServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/receive-contract-template/add");
    }

    changeStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/contract-template/change-status?id=" + id);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/company-share/export-contract/file-receive-template-pdf?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    uploadFileGetUrl(file: File, folder = ''): Observable<any> {
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/receive-contract-template/update/");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-shares/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-shares/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getByOrder(page: Page, orderId): Observable<any> {
        let url_ = "/api/company-shares/contract-template/find-by-order?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (orderId) {
            url_ += this.convertParamUrl("orderId", orderId);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAll(page: Page, params: any): Observable<any> {
        let url_ = "/api/company-shares/receive-contract-template/find-by-secondary?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("companyShareSecondaryId", params?.companyShareSecondaryId);
        if (params?.classifyId) {
            url_ += this.convertParamUrl("classify", params?.classifyId);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllContractTypeIssuer(page: Page): Observable<any> {
        let url_ = "/api/company-shares/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
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
        return this.requestPost(body, "/api/company-shares/contract-template/add");
    }

    changeStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-shares/contract-template/change-status?id=" + id);
    }

    uploadFileGetUrl(file: File, folder = ''): Observable<any> {
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/contract-template/update/");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-shares/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-shares/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getByOrder(page: Page, orderId): Observable<any> {
        let url_ = "/api/company-shares/contract-template/find-by-order?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (orderId) {
            url_ += this.convertParamUrl("orderId", orderId);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAll(page: Page, params: any): Observable<any> {
        let url_ = "/api/company-shares/contract-template/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("companyShareSecondaryId", params?.companyShareSecondaryId);
        if (params?.classifyId) {
            url_ += this.convertParamUrl("classify", params?.classifyId);
        }
        if (params?.type) {
            url_ += this.convertParamUrl("type", params?.type);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllContractTypeIssuer(page: Page): Observable<any> {
        let url_ = "/api/company-shares/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    downloadContractTemplateWord(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/company-shares/export-contract/file-template-word?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/company-shares/export-contract/file-template-pdf?`;
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
export class ContractTypeServiceProxy extends ServiceProxyBase {
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/contract-type/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/company-share/contract-type/update/" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/contract-type/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/contract-type/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/company-share/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }
}
/**
 * CHÍNH SÁCH VÀ BÁN THEO KỲ HẠN
 */
@Injectable()
export class CompanySharePolicyServiceProxy extends ServiceProxyBase {
    constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/secondary/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/bond/secondary/update");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/secondary/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/secondary/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/bond/policy-temp/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllTemporaryNoPaging(): Observable<any> {
        let url_ = "/api/bond/policy-temp/find?";
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("isNoPaging", true);

        return this.requestGet(url_);
    }
}

@Injectable()
export class DistributionContractServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/distribution-contract/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/bond/distribution-contract/update/" + body.distributionContractId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/distribution-contract/delete/" + id;
        return this.requestDelete(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/bond/distribution-contract/duyet/" + id);
    }

    pending(id: number): Observable<any> {
        return this.requestPut(null, "/api/bond/distribution-contract/pending/" + id);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/distribution-contract/" + id;
        return this.requestGet(url_);
    }

    getCoupon(id: number): Observable<any> {
        let url_ = "/api/bond/distribution-contract/coupon/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status: string): Observable<any> {
        let url_ = "/api/bond/distribution-contract/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('status', status ?? '');
        return this.requestGet(url_);
    }
}

@Injectable()
export class DistributionContractPaymentServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/distribution-contract/payment/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/distribution-contract/payment/update/" + body.paymentId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/distribution-contract/payment/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/distribution-contract/payment/find/" + id;
        return this.requestGet(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/distribution-contract/payment/approve/" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/distribution-contract/payment/cancel/" + id);
    }

    getAll(page: Page, distributionContractId): Observable<any> {
        let url_ = "/api/company-share/distribution-contract/payment/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('contractId', distributionContractId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class CompanyShareInfoFileServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/juridical-file/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/bond/juridical-file/update/" + body.juridicalFileId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/juridical-file/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/juridical-file/file/find" + id;
        return this.requestGet(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/bond/distribution-contract/file/approve/" + id);
    }

    cancel(id: number): Observable<any> {
        return this.requestPut(null, "/api/bond/distribution-contract/file/cancel/" + id);
    }

    getAll(page: Page, productBondId): Observable<any> {
        let url_ = "/api/bond/juridical-file/file/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('productBondId', productBondId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class DistributionContractFileServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/distribution-contract/file/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/distribution-contract/file/update/" + body.fileId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/distribution-contract/file/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/distribution-contract/file/find/" + id;
        return this.requestGet(url_);
    }

    approve(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-share/distribution-contract/file/approve/" + id);
    }

    cancel(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/distribution-contract/file/cancel");
    }

    getAll(page: Page, distributionContractId): Observable<any> {
        let url_ = "/api/company-share/distribution-contract/file/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('contractId', distributionContractId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class CompanySharePolicyTemplateServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/policy-temp/add");
    }

    createCompanySharePolicyDetailTemplate(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/policy-temp/add-policy-detail-temp");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/company-shares/policy-temp/update?id=" + id);
    }

    updateCompanySharePolicyDetailTemplate(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/company-shares/policy-temp/update-policy-detail-temp?id=" + id);
    }

    changeStatusCompanySharePolicyTemplate(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-shares/policy-temp/change-status?id=" + id);
    }
    changeStatusCompanySharePolicyDetailTemplate(id: number): Observable<any> {
        return this.requestPut(null, "/api/company-shares/policy-temp/change-status-policy-detail-temp?id=" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-shares/policy-temp/delete?id=" + id;
        return this.requestDelete(url_);
    }

    deleteCompanySharePolicyDetailTemplate(id: number): Observable<void> {
        let url_ = "/api/company-shares/policy-temp/delete-policy-detail-temp?id=" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-shares/policy-temp/find-by-id?id=" + id;
        return this.requestGet(url_);
    }

    getAll(page: PageCompanySharePolicyTemplate, status:any): Observable<any> {
        let url_ = "/api/company-shares/policy-temp/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("status", status ?? '');
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    addListPolicy(body, id: number): Observable<any> {
        return this.requestPost(body, "/api/company-shares/secondary/add-list-policy?policytempId=" + id);
    }

    getAllListPolicy(page: Page, status: string, isClose?: string): Observable<any> {
        let url_ = `/api/company-shares/secondary/find?`;
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("isNoPaging", true);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl('status', status ?? '');
        url_ += this.convertParamUrl('isClose', isClose ?? '');

        return this.requestGet(url_);
    }
}

@Injectable()
export class GuaranteeAssetServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/guarantee-asset/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/bond/guarantee-asset/update");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/guarantee-asset/delete/" + id;
        return this.requestDelete(url_);
    }

    deleteFile(id: number): Observable<void> {
        let url_ = "/api/bond/guarantee-asset/file/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/guarantee-asset/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, productBondId): Observable<any> {
        let url_ = "/api/bond/guarantee-asset/findAll?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("productBondId", productBondId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}