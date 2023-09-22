import { AppConsts, ApproveConst } from '@shared/AppConsts';
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
import { HttpClient, HttpHeaders, HttpResponseBase } from "@angular/common/http";

import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { ChangePasswordDto } from './service-proxies';
import { ApproveFilter } from '@shared/interface/filter.model';

@Injectable()
export class ApproveServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }
    private readonly baseApiRegister = "/api/core/manager-investor";
    
    getAll(page: Page, dataFilter: ApproveFilter): Observable<any> {
        let url_ = "/api/real-estate/approve/find-all?";   
        for (let [key, value] of Object.entries(dataFilter)) {
            if(value && key !='sortFields') {
                url_ += this.convertParamUrl(key, value);
            }
        }   
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    // Quản lý phê duyệt yêu cầu tái tục

    // getAllRequestReinstatement(page: Page): Observable<any> {
    //     let url_ = "/api/invest/approve/all?";
    //     url_ += this.convertParamUrl("dataType", ApproveConst.STATUS_REINSTATEMENT);

    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
    //     return this.requestGet(url_);
    // }

    getAllRequestWithdrawal(page: Page): Observable<any> {
        let url_ = "/api/invest/withdrawal/get-all?";

        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }

    reinstatementApprove(body, action: string) {
        let url_ = "/api/invest/renewals-request/" + action;
        return this.requestPut(body, url_);
    }

    withdrawalApprove(body, action: string) {
        let url_ = "/api/invest/withdrawal/" + action + '/' + body.id;
        return this.requestPut(body, url_);
    }

    public requestPostNoToken<T>(body: any | undefined, url: String): Observable<T> {
		let url_ = this.baseUrl + url;
        console.log('baseUrl', this.baseUrl);
        
		console.log("url!!!!", url);
		
		url_ = url_.replace(/[?&]$/, "");

		const content_ = JSON.stringify(body);

		const fnc = (url_, content_) => {
			let options_: any = {
				body: content_,
				observe: "response",
				responseType: "blob",
				headers: new HttpHeaders({
					"Content-Type": "application/json",
					Accept: "text/plain",
				}),
			};

			return this.http
				.request("post", url_, options_)
				.pipe(
					_observableMergeMap((response_: any) => {
						return this.processResponse(response_);
					})
				)
				.pipe(
					_observableCatch((response_: any) => {
						if (response_ instanceof HttpResponseBase) {
							try {
								return this.processResponse(<any>response_);
							} catch (e) {
								return <Observable<any>>(<any>_observableThrow(e));
							}
						} else return <Observable<any>>(<any>_observableThrow(response_));
					})
				);
		};
		return fnc(url_, content_);
	}

    public registerCustomer(body: any) {
        console.log('vào đây chưaaa');
        
        return this.requestPostNoToken(body, `${this.baseApiRegister}/register-investor`);
      }
}