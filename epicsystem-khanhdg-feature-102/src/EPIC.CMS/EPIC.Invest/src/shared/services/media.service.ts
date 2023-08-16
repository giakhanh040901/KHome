import { AppConsts, SortConst } from '@shared/AppConsts';
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
import { BasicFilter, MediaFilter } from '@shared/interface/filter.model';

@Injectable()
export class MediaService extends ServiceProxyBase {
	confirm(partnerId: any) {
		throw new Error("Method not implemented.");
	}
	private mediaEndpoint = ``;

	constructor(messageService: MessageService, _cookieService: CookieService, @Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
		super(messageService, _cookieService, http, baseUrl);
		this.baseUrl = this.baseUrl.concat('/api/media/product-image')
	}

	getAllMedia(page: Page, dataFilter: MediaFilter): Observable<any> {
        let url_ = `?`;
		for(let [key, value] of Object.entries(dataFilter)) {
			if(value && key != 'sortFields') {
				if(key === 'keyword') key = 'q';
				url_ += this.convertParamUrl(key, value);
			}
		}

		if(dataFilter?.sortFields?.length) {
			let sortValue = dataFilter.sortFields.map(item => item.field+':'+SortConst.getValueSort(item.order)).join(',');
			url_ += this.convertParamUrl('sortBy', sortValue);
		}
		
		url_ += this.convertParamUrl("limit", page.pageSize);
		url_ += this.convertParamUrl("page", page.getPageNumber());
		
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

