import { AppConsts } from '@shared/AppConsts';
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
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class UserRoleService extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getInvestor(keyword: string) {
        let url_ = "/api/core/manager-investor/filter?requireKeyword=true&";
        if (keyword) {
            url_ += this.convertParamUrl("keyword", keyword);
        }
        url_ += this.convertParamUrl('pageSize', -1);
        //
        return this.requestGet(url_);
    }

    // THÊM MỚI VAI TRÒ AND PERMISSION
    createRolePartner(body): Observable<any> {
        return this.requestPost(body, "/api/users/partner/roles/add");
    }

    // CẬP NHẬT VAI TRÒ AND PERMISSION
    updateRolePartner(body): Observable<any> {
        return this.requestPut(body, "/api/users/partner/roles/update-permission/" + body.id);
    }

    // ROOT THÊM QUYỀN TRUY CẬP CÁC WEBSITE CHO PARTNER
    createPermissionWebPartner(body): Observable<any> {
        return this.requestPost(body, "/api/users/partner/max-permissions/web/add");
    }

    // ROOT GẮN QUYỀN CHO PARTNER THEO WEBSITE (QUYỀN TRONG WEBSITE)
    createPermissionWebDetailPartner(body, params): Observable<any> {
        return this.requestPut(body, "/api/users/partner/max-permissions/permission-in-web/update/" + params.partnerId);
    }

    // LẤY DANH SÁCH CÁC WEBSITE ĐƯỢC PHÉP TRUY CẬP CỦA PARTNER
    getWebListOfPartner(partnerId): Observable<any> {
        return this.requestGet("/api/users/partner/max-permissions/web/find-all-list/" + partnerId);
    }

    // LẤY DANH SÁCH QUYỀN CỦA VAI TRÒ (ROLE)
    getPermissionOfRolePartner(roleId): Observable<any> {
        return this.requestGet("/api/users/partner/roles/find/" + roleId);
    }

    // LẤY DANH SÁCH CÁC VAI TRÒ TRONG WEBSITE
    getRoleOfWebPartner(page: Page, webKey: number, status: string): Observable<any> {
        let url_ = "/api/users/partner/roles/find-all?";
        if (status) url_ += this.convertParamUrl("status", status);
        url_ += this.convertParamUrl("permissionInWeb", webKey);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    // LẤY DANH SÁCH QUYỀN CỦA PARTNER TRONG WEBSITE
    getPermissionOfWebPartner(partnerId, webKey): Observable<any> {
        return this.requestGet("/api/users/partner/max-permissions/web/find-all-list/" + partnerId + '/' + webKey);
    }

    // LẤY DANH SÁCH USER THEO PARTNER ĐĂNG NHẬP
    getUserPartner(page: Page, partnerId): Observable<any> {
        let url_ = "/api/users/partner/get-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('partnerId', partnerId);
        return this.requestGet(url_);
    }

    // THÊM VAI TRÒ CHO USER    
    addRoleUser(body) {
        return this.requestPost(body, "/api/users/permission-data/add-role");
    }

    // GET ALL VAI TRÒ CỦA USER
    getRoleUser(userId) {
        let url_ = "/api/users/permission-data/role-by-user/" + userId;
        return this.requestGet(url_);
    }

    // GET ALL VAI TRÒ CỦA PARTNER
    getAllRole() {
        let url_ = "/api/users/permission-data/role-by-partner";
        return this.requestGet(url_);
    }

    // GET DANH SÁCH ĐẠI LÝ
    getAllTradingProvider(): Observable<any> {
        let url_ = "/api/core/trading-provider/find-no-permission?";
        url_ += this.convertParamUrl("pageSize", -1);

        return this.requestGet(url_);
    }

    // PHÂN QUYỀN ĐẠI LÝ ====================

    // GET THÔNG TIN ĐẠI LÝ
    getTradingProvider(id: number): Observable<any> {
        let url_ = "/api/core/trading-provider/find/" + id;
        return this.requestGet(url_);
    }
   
    // GET DANH SÁCH CÁC WEBSITE CỦA ĐẠI LÝ ĐƯỢC PHÉP TRUY CẬP
    getWebListOfTradingProvider(tradingProviderId): Observable<any> {
        return this.requestGet("/api/users/trading-provider/max-permissions/web/find-all-list/" + tradingProviderId);
    }

    // PARTNER THÊM QUYỀN TRUY CẬP CÁC WEBSITE CHO ĐẠI LÝ
    createPermissionWebTradingProvider(body): Observable<any> {
        return this.requestPost(body, "/api/users/trading-provider/max-permissions/web/add");
    }

    // GET ALL QUYỀN USER ĐANG ĐĂNG NHẬP
    getAllPermission(): Observable<any> {
        return this.requestGet("/api/users/get-permission");
    }

    // LẤY DANH SÁCH QUYỀN CỦA ĐẠI LÝ TRONG WEBSITE
    getPermissionOfWebTradingProvider(tradingProviderId, webKey): Observable<any> {
        return this.requestGet("/api/users/trading-provider/max-permissions/web/find-all-list/" + tradingProviderId + '/' + webKey);
    }

    // PARTNER GẮN QUYỀN CHO ĐẠI LÝ THEO WEBSITE (QUYỀN TRONG WEBSITE)
    createPermissionWebDetailTradingProvider(body, params): Observable<any> {
        return this.requestPut(body, "/api/users/trading-provider/max-permissions/permission-in-web/update/" + params.tradingProviderId);
    }

    createRoleTradingProvider(body): Observable<any> {
        return this.requestPost(body, "/api/users/trading-provider/roles/add");
    }

    // LẤY DANH SÁCH QUYỀN CỦA VAI TRÒ (ROLE)
    getPermissionOfRoleTradingProvider(roleId): Observable<any> {
        return this.requestGet("/api/users/trading-provider/roles/find/" + roleId);
    }

    // CẬP NHẬT VAI TRÒ QUYỀN CHO USER CỦA ĐẠI LÝ
    updateRoleTradingProvider(body): Observable<any> {
        return this.requestPut(body, "/api/users/trading-provider/roles/update-permission/" + body.id);
    }

    // LẤY DANH SÁCH CÁC VAI TRÒ TRONG WEBSITE
    getRoleOfWebTradingProvider(page: Page, webKey: number, status: string): Observable<any> {
        let url_ = "/api/users/trading-provider/roles/find-all?";
        if (status) url_ += this.convertParamUrl("status", status);
        url_ += this.convertParamUrl("permissionInWeb", webKey);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    // LẤY DANH SÁCH USER THEO ĐẠI LÝ ĐĂNG NHẬP
    getAllUserTradingProvider(page: Page, tradingProviderId): Observable<any> {
        let url_ = "/api/users/trading-provider/get-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('tradingProviderId', tradingProviderId);
        return this.requestGet(url_);
    }

    // LẤY DANH SÁCH USER THEO PARTNER ĐĂNG NHẬP
    getAllUserPartner(page: Page, partnerId): Observable<any> {
        let url_ = "/api/users/partner/get-all?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('partnerId', partnerId);
        return this.requestGet(url_);
    }

    // LẤY DANH SÁCH USER THEO TÀI KHOẢN ĐĂNG NHẬP
    getAllUser(page: Page, status?: any): Observable<any> {
        let url_ = "/api/users/permission-data/user-by-manager?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        if(status){
            url_ += this.convertParamUrl("status", status);
        }
        return this.requestGet(url_);
    }

    // GET ALL VAI TRÒ CỦA ĐẠI LÝ
    getAllRoleTradingProvider() {
        let url_ = "/api/users/permission-data/role-by-trading-provider";
        return this.requestGet(url_);
    }

    // THÊM MỚI USER
    createUser(body): Observable<any> {
        return this.requestPost(body, "/api/users/root-create-user");
    }

    // API CHO EPIC ========
    // LẤY DANH SÁCH CÁC VAI TRÒ TRONG WEBSITE
    getRoleOfWebEpic(page: Page, webKey: number, status: string): Observable<any> {
        let url_ = "/api/users/epic/roles/find-all?";
        if (status) url_ += this.convertParamUrl("status", status);
        url_ += this.convertParamUrl("permissionInWeb", webKey);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    // THÊM MỚI VAI TRÒ AND PERMISSION
    createRoleEpic(body): Observable<any> {
        return this.requestPost(body, "/api/users/epic/roles/add");
    }

    // CẬP NHẬT VAI TRÒ AND PERMISSION
    updateRoleEpic(body): Observable<any> {
        return this.requestPut(body, "/api/users/epic/roles/update-permission/" + body.id);
    }

    // LẤY DANH SÁCH QUYỀN CỦA VAI TRÒ (ROLE)
    getPermissionOfRoleEpic(roleId): Observable<any> {
        return this.requestGet("/api/users/epic/roles/find/" + roleId);
    }

    // GET ALL VAI TRÒ CỦA EPIC
    getAllRoleEpic() {
        let url_ = "/api/users/permission-data/role-by-epic";
        return this.requestGet(url_);
    }

    //change active/deactive cho cac tai khoan cap duoi
    changeStatus(id: any): Observable<any> {
        return this.requestPut(null, "/api/users/root-change-status?userId=" + id);
    }

    changeStatusRole(id: any): Observable<any> {
        return this.requestPut(null, "/api/users/roles/status?roleId=" + id);
    }

    deleteRole(id: any): Observable<any> {
        let url_ = "/api/users/roles/delete/" + id;
        return this.requestDelete(url_);
    }

    delete(id: number): Observable<any> {
        let url_ = "/api/users/" + id;
        return this.requestDelete(url_);
    }
}