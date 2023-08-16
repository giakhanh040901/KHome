import { AppConsts, InterestPaymentConst, OrderConst, YesNoConst } from '@shared/AppConsts';
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

import { API_BASE_URL, ServiceProxyBase } from "../service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from '../service-proxies/service-proxies';
import * as moment from 'moment';
import { TableBody } from 'primeng/table';
import { DistributionContractFilter } from '@shared/interface/filter.model';

@Injectable()
export class ContractTemplateService extends ServiceProxyBase {
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
        let folder = `${AppConsts.folder}/${folderFnc}`;
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
        let url_ = "/api/invest/contract-template/find-by-order?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (orderId) {
            url_ += this.convertParamUrl("orderId", orderId);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSizeAll);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

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
    getAllContractForm(page: Page, distributionId : number, dataFilter: DistributionContractFilter): Observable<any> {
        let url_ = `/api/invest/contract-template/find-all?`;
        //
        url_ += this.convertParamUrl("distributionId", distributionId);
        
        for(const [key, value] of Object.entries(dataFilter)) {
            if(value && key !='sortFields') {
                url_ += this.convertParamUrl(key, value);
            }
        }
        //
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

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