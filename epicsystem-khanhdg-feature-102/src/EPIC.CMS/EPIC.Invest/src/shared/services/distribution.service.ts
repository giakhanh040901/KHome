import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { AppConsts, DistributionConst, ProductBondInfoConst, ProductBondPrimaryConst, ProductBondSecondaryConst, SortConst, YesNoConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { PageBondPolicyTemplate } from "@shared/model/pageBondPolicyTemplate";
import { TRISTATECHECKBOX_VALUE_ACCESSOR } from "primeng/tristatecheckbox";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { BasicFilter } from "@shared/interface/filter.model";

/**
 * PHÁT HÀNH THỨ CẤP
 */
@Injectable()
export class DistributionService extends ServiceProxyBase {
    private distributionEndPoint = `/api/invest/distribution`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    /**
     * TẠO PHÁT HÀNH THỨ CẤP
     * @param body
     * @returns
     */
    create(body): Observable<any> {
        return this.requestPost(body, `${this.distributionEndPoint}/add`);
    }

    /**
     * CẬP NHẬT PHÁT HÀNH THỨ CẤP
     * @param body
     * @param id
     * @returns
     */
    update(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update`);
    }

    updateOverview(body): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update-overview`);
    }

    downloadContractTemplatePdf(tradingProviderId: any, contractTemplateId: any): Observable<any> {
        let url_ = `/api/invest/export-contract/file-receive-template-pdf?`;
        if(tradingProviderId){
            url_ += this.convertParamUrl("tradingProviderId", tradingProviderId);
        }
        if(contractTemplateId){
            url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        }
        return this.requestDownloadFile(url_);
    }

    downloadContractTemplateFillPdf(contractTemplateId, type): Observable<any> {
        let url_ = `/api/invest/export-contract/file-template-pdf?`;
        if(contractTemplateId) url_ += this.convertParamUrl("contractTemplateId", contractTemplateId);
        if(type) url_ += this.convertParamUrl("type", type);
        //
        return this.requestDownloadFile(url_);
    }

    delete(id: number): Observable<void> {
        let url_ = `${this.distributionEndPoint}/delete/` + id;
        return this.requestDelete(url_);
    }

    deleteRecceiveContractTemp(id: number): Observable<void> {
        let url_ = `/api/invest/receive-contract-template/delete?id=` + id;
        return this.requestPost(null, url_);
    }

    request(body): Observable<any> {
        return this.requestPost(body, `${this.distributionEndPoint}/request`);
    }
    
    approve(body): Observable<any> {
        return this.requestPut(body,  `${this.distributionEndPoint}/approve`);
    }

    cancel(body): Observable<any> {
        return this.requestPut(body,  `${this.distributionEndPoint}/cancel`);
    }

    getOverviewById(id: number): Observable<any> {
        let url_ = `${this.distributionEndPoint}/over-view-distribution-by-id?id=` + id;
        return this.requestGet(url_);
    }

    getById(id: number): Observable<any> {
        let url_ = `${this.distributionEndPoint}/` + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, status?: string, isClose?: string): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find?`;
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        if(status) url_ += this.convertParamUrl('status', status);
        // url_ += this.convertParamUrl('isClose', isClose ?? '');
        return this.requestGet(url_);
    }

    getAllDistribution(page: Page, dataFilter?: BasicFilter): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find?`;
        if(dataFilter?.keyword) {
            url_ += this.convertParamUrl("keyword", dataFilter.keyword);
        }
        
        if(dataFilter.status) {
            if(dataFilter.status != DistributionConst.CLOSED) {
                url_ += this.convertParamUrl('status', dataFilter.status);
                url_ += this.convertParamUrl('isClose', YesNoConst.NO);
            } else {
                url_ += this.convertParamUrl('isClose', YesNoConst.YES);
            }
        } 

        if(dataFilter?.sortFields?.length){
            dataFilter.sortFields.forEach(s => {
                url_ += this.convertParamUrl('Sort', `${s.field}-${SortConst.getValueSort(s.order)}`);
            })
        }
        
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    getBankList(params?: any): Observable<any> {
        let url_ = "/api/invest/distribution/list-bank?";
        //
        if(params?.distributionId) url_ += this.convertParamUrl('distributionId', params.distributionId);
        if(params?.type) url_ += this.convertParamUrl('type', params.type);
        return this.requestGet(url_);
    }


    getDistributionsOrder(): Observable<any> {
        let url_ = "/api/invest/distribution/find-distribution-order";
        return this.requestGet(url_);
    }

    getAllNoPaging(): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find?`;
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("isNoPaging", false);
        url_ += this.convertParamUrl('status', ProductBondSecondaryConst.STATUS.HOAT_DONG);

        return this.requestGet(url_);
    }

    public viewFilePDF(url: string) {
        return this.viewFilePDFFromAPI(url);
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
    toggleIsShowApp(distributionId): Observable<any> {
        let url = `${this.distributionEndPoint}/is-show-app/${distributionId}?`;
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
        let url_ = `${this.distributionEndPoint}/find-policy?`;
        url_ += this.convertParamUrl("policyId", policyId);
        return this.requestGet(url_);
    }

    /**
     * SỬA CHÍNH SÁCH
     * @param body
     * @returns
     */
    updatePolicy(body, id): Observable<any> {
        return this.requestPut(body, `${this.distributionEndPoint}/update-policy/${id}`);
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
        // return this.requestPut(null, "/api/invest/policy-temp/change-status?id=" + id);
    }

    changeStatusPolicyDetail(policyDetailId: number): Observable<any> {
        return this.requestPut(null, `${this.distributionEndPoint}/policyDetail/change-status?policyDetailId=` + policyDetailId);
        // return this.requestPut(null, "/api/invest/policy-temp/change-status?id=" + id);
    }

    getAllPolicyPage(page: Page, distributionId: any, status: any): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-policy-by-ditribution?`;
        if(status) {
            url_ += this.convertParamUrl("status", status);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    getAllPolicy(distributionId): Observable<any> {
        let url_ = `${this.distributionEndPoint}/find-policy/` + distributionId;
        return this.requestGet(url_);
    }

    getAllPolicyActive(distributionId, statusActive): Observable<any> {
        let page: Page = new Page();
        let url_ = `${this.distributionEndPoint}/find-policy/` + distributionId + `?status=${statusActive}&`;
        url_ += this.convertParamUrl('pageSize', page.pageSizeAll);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
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
        return this.requestGet("/api/invest/policy-temp/find-no-permission?status=A");
    }
    
    getAllPolicyTemp(): Observable<any> {
        let url_ = "/api/invest/policy-temp/find?";
        url_ += this.convertParamUrl('pageSize', -1);
        return this.requestGet(url_);
    }

    getAllProject(page: Page): Observable<any> {
        let url_ = "/api/invest/project/find-all-trading-provider?";
        url_ += this.convertParamUrl("pageSize", -1);
        url_ += this.convertParamUrl("keyword", page.keyword || null);
        return this.requestGet(url_);
    }

    // File Chính sách

    createFile(body): Observable<any> {
        return this.requestPost(body, "/api/invest/distri-policy-file/add");
    }
    updateFile(body): Observable<any> {
        return this.requestPut(body, "/api/invest/distri-policy-file/update/" + body.id);
    }

    deleteFile(id: number): Observable<void> {
        let url_ = "/api/invest/distri-policy-file/delete/" + id;
        return this.requestDelete(url_);
    }

    getFile(id: number): Observable<any> {
        let url_ = "/api/invest/distri-policy-file/find/" + id;
        return this.requestGet(url_);
    }

    getAllFile(page: Page, distributionId): Observable<any> {
        let url_ = "/api/invest/distri-policy-file/fileAll/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`;
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    // File mẫu hợp đồng
    createContractTemplate(body): Observable<any> {
        return this.requestPost(body, "/api/invest/contract-template/add");
    }

    changeStatusContractTemplate(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/contract-template/change-status/" + id);
    }

    updateContractTemplate(body): Observable<any> {
        return this.requestPut(body, "/api/invest/contract-template/update");
    }

    deleteContractTemplate(id: number): Observable<void> {
        let url_ = "/api/invest/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    getContractTemplate(id: number): Observable<any> {
        let url_ = "/api/invest/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    // getByOrder(page: Page, orderId): Observable<any> {
    //     let url_ = "/api/bond/contract-template/find-by-order?";
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     if (orderId) {
    //         url_ += this.convertParamUrl("orderId", orderId);
    //     }
    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    //     return this.requestGet(url_);
    // }

    getAllContractTemplate(page: Page, params: any): Observable<any> {
        let url_ = "/api/invest/contract-template/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("distributionId", params?.distributionId);
        if (params?.classify) {
            url_ += this.convertParamUrl("classify", params?.classify);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    changeReceiveContractStatus(id: number): Observable<any> {
        return this.requestPut(null, "/api/invest/receive-contract-template/change-status?id=" + id);
    }

    // File hợp đồng phân phối
    getAllFileDistribution(page: Page, distributionId): Observable<any> {
        let url_ = "/api/invest/distribution-file/find?";
        // url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('distributionId', distributionId);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
    createFileDistribution(body): Observable<any> {
        return this.requestPost(body, "/api/invest/distribution-file/add");
    }
    deleteFileDistribution(id: number): Observable<void> {
        let url_ = "/api/invest/distribution-file/delete/" + id;
        return this.requestDelete(url_);
    }

    /**
     * 
     */
     getAllReceiveContractTemplate(page: Page, params: any): Observable<any> {
        let url_ = "/api/invest/receive-contract-template/find-all-by-distribution?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("distributionId", params?.distributionId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    

    createReceiveContractTemplate(body): Observable<any> {
        return this.requestPost(body, "/api/invest/receive-contract-template/add");
    }

    updateReceiveContractTemplate(body): Observable<any> {
        return this.requestPut(body, "/api/invest/receive-contract-template/update/");
    }
}