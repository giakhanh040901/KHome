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
import { ChangePasswordDto } from './service-proxies';


@Injectable()
export class IssuerServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/issuer/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/issuer/update/" + body.issuerId);
    }

    
    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/issuer/delete/" + id;
        return this.requestDelete(url_);
    }

    getIssuer(id: number): Observable<any> {
        let url_ = "/api/company-share/issuer/" + id;
        return this.requestGet(url_);
    }
    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/find-company-share/" + id;
        return this.requestGet(url_);
    }
    getAll(taxCode): Observable<any> {
        let url_ = "/api/core/business-customer/findTaxCode/" + taxCode;
        return this.requestGet(url_);
    }

    getAllIssuer(page: Page): Observable<any> {
        let url_ = "/api/company-share/issuer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('isNoPaging', false);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class BusinessCustomerServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/core/business-customer/approve/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/core/business-customer/approve/update/" + body.approveId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/business-customer/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/approve/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/core/business-customer/approve/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}



@Injectable()
export class DepositProviderServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/deposit-provider/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/deposit-provider/update/" + body.depositProviderId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/deposit-provider/delete/" + id;
        return this.requestDelete(url_);
    }

    getDepositProvider(id: number): Observable<any> {
        let url_ = "/api/company-share/deposit-provider/find/" + id;
        return this.requestGet(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/find-company-share/" + id;
        return this.requestGet(url_);
    }
    
    // getAll(page: Page): Observable<any> {
    //     let url_ = "/api/core/business-customer/find?";
    //     url_ += this.convertParamUrl("keyword", page.keyword);
    //     url_ += this.convertParamUrl('pageSize', page.pageSize);
    //     url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    //     return this.requestGet(url_);
    // }

    getAll(page: Page, status?: string): Observable<any> {
        let url_ = "/api/company-share/deposit-provider/find?";
        if(status){
            url_ += this.convertParamUrl("status", status);
        }
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        
        return this.requestGet(url_);
    }
}

@Injectable()
export class CalendarServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-shares/calendar/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-shares/calendar/update");
    }

    get(workingDate): Observable<any> {
        let url_ = "/api/company-shares/calendar/" + workingDate;
        return this.requestGet(url_);
    }

    getAll(workingYear): Observable<any> {
        let url_ = "/api/company-shares/calendar/find/" + workingYear;
        return this.requestGet(url_);
    }

    /**
     * Service Calendar Partner
     */
    createCalendarPartner(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/partner-calendar/add");
    }

    updateCalendarPartner(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/partner-calendar/update/");
    }

    getCalendarPartner(workingDate): Observable<any> {
        let url_ = "/api/company-share/partner-calendar/" + workingDate;
        return this.requestGet(url_);
    }

    getAllCalendarPartner(workingYear): Observable<any> {
        let url_ = "/api/company-share/partner-calendar/find/" + workingYear;
        return this.requestGet(url_);
    }
}

@Injectable()
export class TradingProviderServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/core/trading-provider/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/core/trading-provider/update/" + body.tradingProviderId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/core/trading-provider/delete/" + id;
        return this.requestDelete(url_);
    }

    getTradingProvider(id: number): Observable<any> {
        let url_ = "/api/core/trading-provider/find/" + id;
        return this.requestGet(url_);
    }

    //
    get(id: number): Observable<any> {
        let url_ = "/api/core/business-customer/find-company-share/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/core/business-customer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getTradingProviderTaxCode(taxCode :String): Observable<any> {
        let url_ = "/api/core/trading-provider/find/tax-code/" + taxCode;
        // url_ += this.convertParamUrl("keyword", page.keyword);
        // url_ += this.convertParamUrl('pageSize', page.pageSize);
        // url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllTradingProvider(page: Page): Observable<any> {
        let url_ = "/api/core/trading-provider/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    createUser(body: any): Observable<any> {
        let url_ = "/api/core/trading-provider/user/create";
        return this.requestPost(body, url_);
    }

    changePassword(body: ChangePasswordDto | undefined): Observable<boolean> {
        let url_ = "/api/core/trading-provider/user/change-password";
        return this.requestPut(body, url_);
    }

    getAllUser(page: Page): Observable<any> {
        let url_ = "/api/core/trading-provider/user/find-all-user?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

        return this.requestGet(url_);
    }

    active(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/core/trading-provider/user/active/" + id);
    }

    updateUser(body: any): Observable<any> {
        let url_ = "/api/core/trading-provider/update/" + body.userId;
        url_ = url_.replace(/[?&]$/, "");
        return this.requestPut(body, url_);
    }

    deleteUser(userId: number | undefined): Observable<void> {
        let url_ = "/api/core/trading-provider/user/delete/" + userId;
        url_ = url_.replace(/[?&]$/, "");
        return this.requestDelete(url_);
    }
}

@Injectable()
export class ProductCategoryServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/product-category/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/product-category/update/" + body.productCategoryId);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/product-category/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/product-category/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/company-share/product-category/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class CompanyShareInterestServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/product-company-share/interest/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/company-share/product-company-share/interest/update");
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/product-company-share/interest/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/product-company-share/interest/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/company-share/product-company-share/interest/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class CompanyShareTypeServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/product-company-share/type/add");
    }

    update(body): Observable<any> {
        return this.requestPut(body, "/api/company-share/product-company-share/type/update/" + body.id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/product-company-share/type/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/product-company-share/type/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/company-share/product-company-share/type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl("pageSize", page.pageSize);
        url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
        return this.requestGet(url_);
    }
}

@Injectable()
export class ProductPolicyServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/company-share/product-policy/add");
    }

    update(body, id: number): Observable<any> {
        let url_ = "/api/company-share/product-policy/update/" + id;
        return this.requestPut(body, url_);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/company-share/product-policy/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/company-share/product-policy/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, market?: string): Observable<any> {

        let url_ = "/api/company-share/product-policy/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if (market) {
            console.log({ marketSearch: market });
            url_ += this.convertParamUrl("market", market);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class MediaService extends ServiceProxyBase {
	confirm(partnerId: any) {
		throw new Error("Method not implemented.");
	}
	private newsEndpoint = `/news`;
	private mediaEndpoint = ``;
	private knowledgeBaseEndpoint = `/knowledge-base`;

	constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
		super(messageService, _cookieService, http, baseUrl);
		this.baseUrl = this.baseUrl.concat('/api/media/product-image')

	}

	getAllMedia(page: Page, status: string, type:string, position:string): Observable<any> {
		// let url_ = `${this.mediaEndpoint}?`;
		
        let url_ = `?`;
		url_ += this.convertParamUrl("limit", page.pageSize);
		url_ += this.convertParamUrl("page", page.getPageNumber());
        if(page.keyword.trim()) {
			url_ += this.convertParamUrl("q", page.keyword);
		}
		if(status){
			url_ += this.convertParamUrl("status", status ??'');
		}
		if(type){
			url_ += this.convertParamUrl("type", type ??'');
		}
		if(position){
			url_ += this.convertParamUrl("position", position ??'');
		}
		return this.requestGet(url_);
	}
	createMedia(body): Observable<any> {
		return this.requestPost(body, ``);
	}
	saveMedia(body): Observable<any> {
		let updateUrl = this.mediaEndpoint.concat('/').concat(body.id)
		return this.requestPatch(body, `${updateUrl}`);
	}

}

