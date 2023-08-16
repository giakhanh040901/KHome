import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { AppConsts, SortConst } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProductService extends ServiceProxyBase { 
    private endPoint = `/api/real-estate/product-item`;
    public listCard: any[] = [];
    public lastestProduct: any;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    
    importFromExcel(body): Observable<any> {
        let folderPath = `${AppConsts.folder}/import`;
        return this.requestPostFileUtil(body, `${this.endPoint}/import`);
    }    

    importTemplate(projectId): Observable<any> {
        return this.requestDownloadFile(`${this.endPoint}/import-template/${projectId}`);
    }

    findAll(page: Page, projectId: number, fieldFilters?: any, sortData?: any[]): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('projectId', projectId);
        //
        for(const [key, value] of Object.entries(fieldFilters)) {
            if (value) url_ += this.convertParamUrl(key, `${value}`);
        }
        //
        if(page.keyword) url_ += this.convertParamUrl("name", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }

        //
        return this.requestGet(url_);
    }

    findAllPageSize(page: Page, projectId: number, sortData?: any[]): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('projectId', projectId);
        url_ += this.convertParamUrl('pageSize', -1);
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
     
        return this.requestGet(url_);
    }

    findAllNoPaging(projectId: number, fieldFilters?: any): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        url_ += this.convertParamUrl('projectId', projectId);
        url_ += this.convertParamUrl('pageSize', -1);
        if(fieldFilters.redBookType) url_ += this.convertParamUrl("redBookType", fieldFilters.redBookType);
        //
        return this.requestGet(url_);
    }

    createOrEdit(body): Observable<any> {
        if(body.id) {
            return this.requestPut(body, `${this.endPoint}/update`);
        }
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    clone(body){
        return this.requestPost(body, `${this.endPoint}/replicate-product-item`);
    }
    
    findById(id): Observable<any> {
        return this.requestGet(`${this.endPoint}/find-by-id/${id}`);
    }

    delete(id: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete/`+ id);
    }

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status?id=${id}`);
    }

    // KHOÁ/MỞ KHOÁ CĂN HỘ BẢNG HÀNG
    lock(body){
        return this.requestPut(body, `${this.endPoint}/lock-product-item`);
    }

    unlock(id, status){
        return this.requestPut("", `${this.endPoint}/change-status?id=${id}&status=${status}`);
    }

    // Lịch sử
    getHistory(page: Page, productItemId: number): Observable<any>{
        let url_ = "/api/real-estate/history/product-item/find-all?";
        url_ += this.convertParamUrl("RealTableId", productItemId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    lastestProductItem(projectId: number) {
        const url_ = "/api/real-estate/product-item/order-new-project/" + projectId;
        return this.requestGet(url_);
    }
}