import { Inject, Injectable, Optional } from '@angular/core';
import { API_BASE_URL, ServiceProxyBase } from './service-proxies-base';
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Page } from '@shared/model/page';

@Injectable()
export class DepartmentService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/core/department/create");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/core/department/update");
    }

    moveSalesToNewDepartment(body): Observable<any> {
        return this.requestPut(body, "/api/core/department/update-sale-to-new-department");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/core/department/delete/" + id;
        return this.requestDelete(url_);
    }

    deleteSaleManager(id: number, type: number): Observable<void> {
        let url_ = `/api/core/department/delete-department-manager?managerType=${type}&departmentId=${id}`;
        return this.requestDelete(url_);
    }

    getDepartmentChild(parentId?: number): Observable<any> {
        let url_ = "/api/core/department/find-all-list?";
        if(parentId) {
            url_ += this.convertParamUrl("parentId", parentId);
        }
        return this.requestGet(url_);
    }
    //
    getById(departmentId: number): Observable<any> {
        let url_ = "/api/core/department/find/" + departmentId;
        return this.requestGet(url_);
    }
    //
    addSale(body): Observable<any> {
        return this.requestPost(body, "/api/core/department/move-sale");
    }

    addSaleManager(body): Observable<any> {
        return this.requestPut(body, "/api/core/department/assign-manager");
    }
    
    addSaleManager2(body): Observable<any> {
        return this.requestPut(body, "/api/core/department/assign-manager-2");
    }
    


    // getAllSale(page: Page): Observable<any> {
    //     let url_ = "/api/core/department/find-all-list-sale?";
    //     //
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    //     //
    //     url_ += this.convertParamUrl('position', null);
    //     url_ += this.convertParamUrl('departmentId', null);

    //     return this.requestGet(url_);
    // }

    getAllDepartmentChild(dataFilter?: any): Observable<any> {
        let url_ = "/api/core/department/find-department-child?departmentId=" + dataFilter.departmentId;
        return this.requestGet(url_);
    }

    getAllSaleDepartments(page: Page, dataFilter?: any): Observable<any> {
        let url_ = "/api/core/department/find-all-list-sale?";
        //
        if(dataFilter.departmentId) url_ += this.convertParamUrl('departmentId', dataFilter.departmentId);
        if(page.keyword) url_ += this.convertParamUrl(dataFilter?.field, page.keyword);
        if(dataFilter?.customerType) {
            if (dataFilter?.customerType == 'Y') {
                url_ += this.convertParamUrl('isInvestor', true);
            } else {
                url_ += this.convertParamUrl('isInvestor', false);
            }
        } 
        if(dataFilter?.saleType) url_ += this.convertParamUrl('saleType', dataFilter?.saleType);
        if(dataFilter?.status) url_ += this.convertParamUrl('status', dataFilter?.status);
        //
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}
