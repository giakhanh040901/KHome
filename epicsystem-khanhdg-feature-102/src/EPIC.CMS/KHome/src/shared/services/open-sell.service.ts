import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { SortConst } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class OpenSellService extends ServiceProxyBase { 
    private endPoint = `/api/real-estate/open-sell`;
    private readonly baseAPI = "/api/real-estate/project";
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
    updateOpenSell(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update`);
    }

    getProjectDetail(openSellId: number): Observable<any> {
        let url_ = `${this.endPoint}/find-by-id/${openSellId}`
        return this.requestGet(url_);
    }

    getBankList(projectId: any): Observable<any> {
        let url_ = `/api/real-estate/open-sell/bank-account-can-distribution/${projectId}?`;
        return this.requestGet(url_);
    }

    getHistory(page: Page, openSellDetailId: any): Observable<any>{
        let url_ = "/api/real-estate/history/open-sell-detail/find-all?";
        url_ += this.convertParamUrl("RealTableId", openSellDetailId);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    findAll(page: Page, filter?: any, sortData?: any[]): Observable<any> {
        let url_ = `${this.endPoint}/find-all?`
        // url_ += this.convertParamUrl('productItemId', productId);
        if (filter.status) url_ += this.convertParamUrl('status', filter.status);
        if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        if (filter.ownerId) url_ += this.convertParamUrl("ownerId", filter.ownerId);
        if (filter.projectId) url_ += this.convertParamUrl("projectId", filter.projectId);
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }
    //
    getAllOwner(){

    }

    findById(id: number): Observable<any> {
        let url_ = `${this.endPoint}/find-by-id/${id}`
        return this.requestGet(url_);
    }
    //
    getProductItemInfo(openSellId: number): Observable<any> {
        let url_ = `${this.endPoint}/detail/find-product-item/${openSellId}`
        return this.requestGet(url_);
    }

    getAllNoPagingProject(openSellId): Observable<any> {
        let url_ = this.baseAPI + "/get-all?";
        url_ += this.convertParamUrl("openSellId", openSellId);
        url_ += this.convertParamUrl("pageSize", -1);
        return this.requestGet(url_);
    }

    findAllProduct(page: Page, openSellId, fieldFilters?:any, sortData?: any[]) {
        let url_ = this.endPoint + "/detail?";
        //
        url_ += this.convertParamUrl('openSellId', openSellId);
        //
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if (page.keyword) url_ += this.convertParamUrl("keyword", page.keyword);
        if (fieldFilters.redBookType) url_ += this.convertParamUrl('redBookType', fieldFilters.redBookType);
        if (fieldFilters.firstDensity) url_ += this.convertParamUrl('buildingDensityId', fieldFilters.firstDensity);
        if (fieldFilters.secondDensity) url_ += this.convertParamUrl('buildingDensityId', fieldFilters.secondDensity);
        if (fieldFilters.status) url_ += this.convertParamUrl('status', fieldFilters.status);
        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        return this.requestGet(url_);
    }

    getAllProductItemNoPaging(openSellId, sortData?: any[]) {
        let url_ = this.endPoint + "/detail?";
        url_ += this.convertParamUrl('openSellId', openSellId);
        url_ += this.convertParamUrl("pageSize", -1);

        if(sortData){
            sortData.forEach(item => {
                url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
            })
        }
        return this.requestGet(url_);
    }

    createOrUpdate(body): Observable<any> {
        if(body?.id) {
            return this.requestPut(body, `${this.endPoint}/update`);
        }
        return this.requestPost(body, `${this.endPoint}/add`);
    }

    addProduct(body): Observable<any> {
        return this.requestPost(body, `${this.endPoint}/detail/add`);
    }
    // Ẩn giá
    hidePrice(body): Observable<any> {
        return this.requestPut(body, `/api/real-estate/open-sell/detail/hide-price`)
    }

    // hiện giá
    showPrice(body): Observable<any> {
        return this.requestPut(null, `/api/real-estate/open-sell/detail/show-price/${body?.id}`)
    }
    // Bật tắt showapp cho mở bán
    changeShowappOpenSell(id): Observable <any> {
        return this.requestPut(null, `/api/real-estate/open-sell/show-app/${id}`)
    }
    // Bật tắt showapp cho từng căn hộ trong mở bán
    changeShowapp(id): Observable <any> {
        return this.requestPut(null, `/api/real-estate/open-sell/detail/show-app/${id}`)
    }

    lockOrUnlock(body){
        return this.requestPut(body, `/api/real-estate/open-sell/detail/lock`);
    }
    //
    deleteProduct(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/detail/delete`);
    }

    delete(id: number): Observable<any> {
        return this.requestPut('', `${this.endPoint}/delete/`+ id);
    }

    stop(id: number): Observable <any> {
        return this.requestPut(null, `/api/real-estate/open-sell/stop/${id}`)
    }

    pauseOrActive(id: any): Observable<any> {
        return this.requestPut('',  `${this.endPoint}/pause/${id}`)
    }

    outstanding(id: any): Observable<any> {
        return this.requestPut('',  `${this.endPoint}/outstanding/${id}`)
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

    changeStatus(id: number, status: string): Observable<any> {
        return this.requestPut(null, `${this.endPoint}/change-status/`+ id + `?status=${status}`);
    }

    addUtility(body, productId: number): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/add-utility?productItemId=`+productId);
    }

    lastestOpenSell(id: number) {
        const url_ = "/api/real-estate/open-sell/order-latest/" + id;
        return this.requestGet(url_);
    }
}