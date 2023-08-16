import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Page } from "@shared/model/page";
import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { AppConsts, SortConst } from "@shared/AppConsts";

@Injectable()
export class DistributionService extends ServiceProxyBase {
    public distributionId: number;
    //
    private endPoint = `/api/real-estate/distribution`;
    private endPointItem = `/api/real-estate/distribution/distribution-product-item`;
    public isHaveProductList: boolean = false;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    // THÊM MỚI AND CẬP NHẬT PHÂN PHỐI SẢN PHẨM
    createOrEdit(body): Observable<any> {
        if(body.id) return this.requestPut(body, `${this.endPoint}/update`);
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    // DANH SÁCH SẢN PHẨM PHÂN PHỐI THEO ĐẠI LÝ VÀ DỰ ÁN
    findAll(page: Page, fieldFilters: any, sortData?: any[]) {
        let url_ = `${this.endPoint}/find-all?`;
        // 
        if(fieldFilters.projectId) url_ += this.convertParamUrl("projectId", fieldFilters.projectId);
        if(fieldFilters.tradingProviderId) url_ += this.convertParamUrl("tradingProviderId", fieldFilters.tradingProviderId);
        if(fieldFilters.status) url_ += this.convertParamUrl("status", fieldFilters.status);
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    // DANH SÁCH SẢN PHẨM ĐƯỢC PHÂN PHỐI CHO ĐẠI LÝ THEO DỰ ÁN (CHI TIẾT PHÂN PHỐI)
    findAllItem(page: Page, status: any, sortData?: any[]) {
        let url_ = `${this.endPoint}/find-all-item?`;
        url_ += this.convertParamUrl("distributionId", this.distributionId);
        if (status) url_ += this.convertParamUrl("status", status);
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    // DANH SÁCH SẢN PHẨM ĐƯỢC PHÂN PHỐI (KHÔNG PHÂN TRANG)
    findAllItemNoPaging() {
        let url_ = `${this.endPoint}/find-all-item?`;
        url_ += this.convertParamUrl("distributionId", this.distributionId);
        url_ += this.convertParamUrl('pageSize', -1);
        return this.requestGet(url_);
    }

    // DANH SÁCH SẢN PHẨM CÓ THỂ PHÂN PHỐI
    findCanDistribution(fillter?:any) {
        let url_ = `/api/real-estate/product-item/can-distribution?distributionId=${this.distributionId}&`;
        if(fillter.keyword) url_ += this.convertParamUrl("keyword", fillter.keyword);
        if(fillter.redBookType) url_ += this.convertParamUrl("redBookType", fillter.redBookType);
        if(fillter.buildingDensityId) url_ += this.convertParamUrl("buildingDensityId", fillter.buildingDensityId);

        return this.requestGet(url_);
    }

    // Lấy phân phối theo Trading
    public getAllByTrading(): Observable<any> {
        return this.requestGet('/api/real-estate/distribution/distribution-by-trading');
      }
    
    // THÔNG TIN CHI TIẾT PHÂN PHỐI
    findById(distributionId) {
        return this.requestGet(`${this.endPoint}/find-by-id/${distributionId}`);
    }

    // THÊM SẢN PHẨM DỰ ÁN VÀO DANH SÁCH PHÂN PHỐI
    createOrEditProductItem(body) {
        return this.requestPost(body, `${this.endPointItem}/add`);
    }

    // TẠM DỪNG/PHÂN PHỐI 
    pauseOrActive(id){
        return this.requestPut("", `${this.endPoint}/pause/${id}`);
    }

    // KHOÁ/MỞ KHOÁ CĂN HỘ
    lockOrUnlock(body){
        return this.requestPut(body, `${this.endPointItem}/lock`);
    }
    
    // XOÁ PHÂN PHỐI
    delete(id){
        return this.requestPut('',`${this.endPoint}/delete/${id}`);
    }

    // XÓA SẢN PHẨM KHỎI DANH SÁCH PHÂN PHỐI
    deleteDistributionProductItem(body) {
        return this.requestPut(body,`${this.endPointItem}/delete`);
    }

    // TRÌNH DUYỆT PHÂN PHỐI SẢN PHẨM
    requestApprove(body) {
        return this.requestPut(body,  `${this.endPoint}/request`);
    }

    // PHÊ DUYỆT HOẶC HỦY PHÂN PHỐI SẢN PHẨM
    approveOrCancel(body, approve: boolean): Observable<any> {
        if(approve) {
            return this.requestPut(body,  `${this.endPoint}/approve`);
        }
        // CANCEL PHÂN PHỐI SP
        return this.requestPut(body,  `${this.endPoint}/cancel`);
    }

    getAllDistributionByTradingProvider(paramFilters: any) {
        let url_ = `${this.endPoint}/distribution-item-by-trading?`;

        url_ += this.convertParamUrl("projectId", paramFilters.projectId);
        if(paramFilters?.redBookType) url_ += this.convertParamUrl("redBookType", paramFilters.redBookType);
        if(paramFilters?.buildingDensityId) url_ += this.convertParamUrl("buildingDensityId", paramFilters.buildingDensityId);
        if(paramFilters?.keyword) url_ += this.convertParamUrl("keyword", paramFilters?.keyword);
        //
        return this.requestGet(url_);
    }

    //
    approve(body): Observable<any> {
        return this.requestPut(body,  `${this.endPoint}/approve`);
    }

    cancel(body): Observable<any> {
        return this.requestPut(body,  `${this.endPoint}/approve`);
    }

    uploadFileGetUrl(file: File, folderFnc = ''): Observable<any> {
        let folder = `${AppConsts.folder}/${folderFnc}`
        let url_: string = `/api/file/upload?folder=${folder}`;
        return this.requestPostFile(file, url_);
    }

    getDistributionsOrder(): Observable<any> {
        let url_ = "/api/invest/distribution/find-distribution-order";
        return this.requestGet(url_);
    }

    public getIsHaveProductList(rowData?: any[], pageNumber?: number) {
        if (!pageNumber) {
            this.findAllItemNoPaging().subscribe((res) => {
                this.isHaveProductList = !!res?.data?.items?.length
            })
        } else {
            // nếu rowData.length === 0 && pageNumber === 1 => không có bản ghi nào => false, ngược lại => true
            this.isHaveProductList = !(!rowData?.length && pageNumber === 1);
        }
    }
}