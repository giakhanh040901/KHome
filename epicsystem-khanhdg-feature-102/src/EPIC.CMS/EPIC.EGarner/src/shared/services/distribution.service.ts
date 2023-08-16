import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { AppConsts, DistributionConst } from "@shared/AppConsts";

@Injectable()
export class DistributionService extends ServiceProxyBase {
    private distributionEndPoint = `/api/garner/distribution`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, `${this.distributionEndPoint}/add`);
    }

    update(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update`);
    }

    updateOverview(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update-product-overview`);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/garner/export-contract/file-receive-template-pdf?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    delete(id: number): Observable<void> {
        let url_ = `/api/garner/receive-contract-template/delete?id=` + id;
        return this.requestDelete(url_);
    }

    changeReceiveContractStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/receive-contract-template/change-status?id=" + id);
    }

    request(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/request`);
    }
    
    approve(body): Observable<any> {
        return this.requestPut(body,  `${this.distributionEndPoint}/approve`);
    }

    approveEpic(body): Observable<any> {
        return this.requestPut(body,  `/api/garner/distribution/check`);
    }


    cancel(body): Observable<any> {
        return this.requestPut(body,  `${this.distributionEndPoint}/cancel`);
    }

    getOverviewById(id: number): Observable<any> {
        let url_ = `/api/garner/distribution/find-product-overview?id=` + id;
        return this.requestGet(url_);
    }
    
    getHistory(page: Page, id: number) {
        let url_ = "/api/garner/distribution/get-all-history?";
        url_ += this.convertParamUrl('DistributionId', id);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getById(id: number): Observable<any> { 
        let url_ = `${this.distributionEndPoint}/find-by-id?id=` + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status?: string, isClose?:string): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-all?`;
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl('status', status ?? '');
        if (isClose) url_ += this.convertParamUrl('isClose', isClose);
        return this.requestGet(url_);
    }

    getBankList(params?: any): Observable<any> {
        let url_ = "/api/garner/product-trading-provider/list-bank?";
        if(params?.distributionId) url_ += this.convertParamUrl('distributionId', params.distributionId);
        if(params?.type) url_ += this.convertParamUrl('type', params.type);
        //
        return this.requestGet(url_);
    }

    getDistributionsOrder(): Observable<any> { // dung tam api getall distribution
        let url_ = "/api/garner/distribution/distribution/get-all";
        return this.requestGet(url_);
    }

    getAllNoPaging(): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find?`;
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl('status', DistributionConst.HOAT_DONG);

        return this.requestGet(url_);
    }
    /**
     * ĐLSC DUYỆT/BỎ DUYỆT PHÁT HÀNH THỨ CẤP
     * @param body
     * @param secondaryId
     * @param status
     * @returns
     */
    changeStatus(secondaryId, status): Observable<any> {
        let url = `${this.distributionEndPoint}/trading-provider-approve/${secondaryId}?`;
        url += this.convertParamUrl("status", status);

        return this.requestPut({}, url);
    }

    /**
     * BAT TAT CLOSED
     * @param secondaryId
     * @param isCancel
     * @returns
     */
    toggleIsClosed(distributionId): Observable<any> {
        let url = `${this.distributionEndPoint}/is-close/${distributionId}?`;
        return this.requestPut(null, url);
    }

    /**
     * BAT TAT PHAT HANH THU CAP SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    showApp(distributionId): Observable<any> {
        let url = `${this.distributionEndPoint}/show-app/${distributionId}?`;
        return this.requestPut(null, url);
    }

    openOrClose(distributionId): Observable<any> {
        let url = `${this.distributionEndPoint}/close/${distributionId}?`;
        return this.requestPut(null, url);
    }

    /**
     * BAT TAT POLICY SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    toggleIsShowAppPolicy(policyId): Observable<any> {
        let url = `${this.distributionEndPoint}/policy-is-show-app/${policyId}?`;

        return this.requestPut(null, url);
    }

    /**
     * BAT TAT POLICY DETAIL SHOW APP
     * @param secondaryId
     * @param isShowApp
     * @returns
     */
    toggleIsShowAppPolicyDetail(policyDetailId): Observable<any> {
        let url = `${this.distributionEndPoint}/policy-detail-is-show-app/${policyDetailId}?`;
        return this.requestPut(null, url);
    }

    /**
     * TẠO CHÍNH SÁCH
     * @param body
     * @returns
     */
    addPolicy(body): Observable<any> {
        return this.requestPost(body, `${this.distributionEndPoint}/add-policy`);
    }
    
    // Lấy thông tin chính sách
    getPolicyById(policyId): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-policy-by-id/${policyId}`;
        return this.requestGet(url_);
    }

    createPolicyContractTemp(body): Observable<any> {
        return this.requestPost(body, `/api/garner/contract-template/add`);

    }

    updatePolicyContractTemp(body): Observable<any> {
        return this.requestPut(body, "/api/garner/contract-template/update");
    }
    /**
     * SỬA CHÍNH SÁCH
     * @param body
     * @returns
     */
    updatePolicy(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update-policy`);
    }
    /**
     * XOÁ CHÍNH SÁCH
     * @param body
     * @returns
     */
    deletePolicy(id): Observable<any> {
        return this.requestDelete(`${this.distributionEndPoint}/delete-policy/${id}`);
    }

    deletePolicyDetail(policyDetailId): Observable<any> {
        return this.requestDelete(`${this.distributionEndPoint}/delete-policy-detail/${policyDetailId}`);
    }

    changeStatusPolicy(policyId: number): Observable<any> {
        return this.requestPut(null, `${this.distributionEndPoint}/policy/change-status?policyId=` + policyId);
    }

    changeStatusPolicyDetail(policyDetailId: number): Observable<any> {
        return this.requestPut(null, `${this.distributionEndPoint}/policyDetail/change-status?policyDetailId=` + policyDetailId);
    }

    getAllPolicy(distributionId): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-policy/` + distributionId;
        return this.requestGet(url_);
    }

    getAllPolicyDetail(policyId): Observable<any> {
        let url_ = `${this.distributionEndPoint}/policy-detail/find-by-policy?policyId=` + policyId;
        return this.requestGet(url_);
    }

    /**
     * TẠO CHÍNH SÁCH CON
     * @param body
     * @returns
     */
    addPolicyDetail(body): Observable<any> {
        return this.requestPost(body, `${this.distributionEndPoint}/add-policy-detail`);
    }
    /**
     * SỬA CHÍNH SÁCH CON
     * @param body
     * @returns
     */
    updatePolicyDetail(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update-policy-detail`);
    }
    /**
     * XOÁ CHÍNH SÁCH CON
     * @param body
     * @returns
     */


    check(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/check`);
    }

    getAllPolicyTempNoPermission(): Observable<any> {
        return this.requestGet(`/api/garner/policy-temp/find-all-no-permission`);
    } 

    getAllPolicyTemp(): Observable<any> {
        let url_ = "/api/garner/policy-temp/find-all?";
        url_ += this.convertParamUrl('pageSize', -1);
        return this.requestGet(url_);
    }

    getAllProduct(page: Page): Observable<any> {
        let url_ = "/api/garner/product/list-product-by-trading?";
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("keyword", page.keyword);
        return this.requestGet(url_);
    }

    // File Chính sách

    createFile(body): Observable<any> {
        return this.requestPost(body, "/api/garner/distri-policy-file/add");
    }
    updateFile(body): Observable<any> {
        return this.requestPut(body, "/api/garner/distri-policy-file/update/" + body.id);
    }

    deleteFile(id: number): Observable<void> {
        let url_ = "/api/garner/distribution/policy-file/delete/" + id;
        return this.requestDelete(url_);
    }

    getFile(id: number): Observable<any> {
        let url_ = "/api/garner/distri-policy-file/find/" + id;
        return this.requestGet(url_);
    }

    getAllFile(page: Page, distributionId): Observable<any> {
        let url_ = "/api/garner/distri-policy-file/fileAll/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    // File mẫu hợp đồng
    createContractTemplate(body): Observable<any> {
        return this.requestPost(body, "/api/garner/contract-template/add");
    }

    changeStatusContractTemplate(id: number): Observable<any> {
        return this.requestPut(null, "/api/garner/contract-template/change-status/" + id);
    }

    updateContractTemplate(body): Observable<any> {
        return this.requestPut(body, "/api/garner/contract-template/update");
    }

    deleteContractTemplate(id: number): Observable<void> {
        let url_ = "/api/garner/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    getContractTemplate(id: number): Observable<any> {
        let url_ = "/api/garner/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getAllContractTemplate(page: Page, params: any): Observable<any> {
        let url_ = "/api/garner/contract-template/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("distributionId", params?.distributionId);
        if (params?.classify) {
            url_ += this.convertParamUrl("classify", params?.classify);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    // File hợp đồng phân phối
    getAllFileDistribution(page: Page, distributionId): Observable<any> {
        let url_ = "/api/garner/distribution-file/find?";
        // url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
    createFileDistribution(body): Observable<any> {
        return this.requestPost(body, "/api/garner/distribution-file/add");
    }

    deleteFileDistribution(id: number): Observable<void> {
        let url_ = "/api/garner/distribution-file/delete/" + id;
        return this.requestDelete(url_);
    }

    /**
     * 
     */
     getAllReceiveContractTemplate(page: Page, params: any): Observable<any> {
        let url_ = "/api/garner/receive-contract-template/find-by-distribution?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("distributionId", params?.distributionId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    createReceiveContractTemplate(body): Observable<any> {
        return this.requestPost(body, "/api/garner/receive-contract-template/add");
    }

    updateReceiveContractTemplate(body): Observable<any> {
        return this.requestPut(body, "/api/garner/receive-contract-template/update/");
    }

    deletePrice(ditributionId: number): Observable<any> {
        return this.requestDelete(`/api/garner/distribution/delete-product-price/${ditributionId}`);
    }

    getAllPrice(page: Page, ditributionId: number): Observable<any> {
        let url_ = `/api/garner/distribution/find-product-price?`;
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        url_ += this.convertParamUrl("ditributionId", ditributionId);

        return this.requestGet(url_);
    }

    updatePrice(body): Observable<any> {
        return this.requestPut(body, `/api/garner/distribution/update-product-price`);
    }

    importPriceFromExcel(body, ditributionId : number): Observable<any> {
        return this.requestPostFileUtil(body, `/api/garner/distribution/import-price-from-excel/${ditributionId }`);
    }

}