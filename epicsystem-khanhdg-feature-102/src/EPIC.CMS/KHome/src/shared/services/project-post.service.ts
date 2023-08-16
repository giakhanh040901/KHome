import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable, Inject, Optional } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { MessageService } from "primeng/api";
import { CookieService } from "ngx-cookie-service";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { Page } from "@shared/model/page";
import { AppConsts } from "@shared/AppConsts";

@Injectable()
export class FacebookPostService extends ServiceProxyBase {
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    exchangeToken(token: string): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
        let url_ = "/api/media/facebook/exchange-token";
        return this.requestPost({ token }, url_);
    }

    getFacebookPages(token: string): Observable<any> {
        this.baseUrl = 'https://graph.facebook.com';  
        let url_ = "/me/accounts?fields=id,picture,name,category,access_token&limit=100&access_token=" + token;
        return this.requestGet(url_);
    }
    getFacebookPost(token: string, pageId: string): Observable<any> {
        this.baseUrl = 'https://graph.facebook.com/';  
        let url_ = pageId + "/feed?fields=message,full_picture,created_time,updated_time,id,permalink_url&limit=100&access_token=" + token;
        return this.requestGet(url_);
    }
    getFacebookPostIds(): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
        let url_ = '/api/media/facebook/list-post-ids';
        return this.requestGet(url_);
    }

    addFacebookPost(post: any): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
        let url_ = "/api/media/facebook/add-post";
        return this.requestPost(post, url_);
    }
    
    updateFacebookPost(post: any): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
        let url_ = `/api/media/facebook/update-post/${post.id}`;
        return this.requestPost(post, url_);
    }

    updateStatus(post: any): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
        let url_ = `/api/media/facebook/update-status/${post.id}`;
        return this.requestPost(post, url_);
    }

    getAll(page: Page, status: string, projectId: number): Observable<any> {
        this.baseUrl = AppConsts.nodeBaseUrl; 
		let url_ = '/api/media/facebook/list-post?';
        url_ += this.convertParamUrl("postCategory", 'real_estate');
        url_ += this.convertParamUrl("projectId", projectId);
		if(status){
			url_ += this.convertParamUrl("status", status??'');
		}
		if(page.keyword.trim()) {
			url_ += this.convertParamUrl("q", page.keyword);
		}
		url_ += this.convertParamUrl("limit", page.pageSize);
		url_ += this.convertParamUrl("page", page.getPageNumber());

		return this.requestGet(url_);
	}

}