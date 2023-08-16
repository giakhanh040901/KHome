import { Inject, Injectable, Optional } from '@angular/core';
import { API_BASE_URL, ServiceProxyBase } from './service-proxies-base';
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { Page } from '@shared/model/page';
import { Observable } from 'rxjs';

@Injectable()
export class ProvinceServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    getAllProvince(page: Page): Observable<any> {
        let url_ = "/api/core/province?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        return this.requestGet(url_);
    }

    getAllDistrict(page: Page, provinceCode: string): Observable<any> {
        let url_ = "/api/core/district?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('provinceCode', provinceCode ?? '');
        return this.requestGet(url_);
    }

    getAllWard(page: Page, districtCode: string): Observable<any> {
        let url_ = "/api/core/ward?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('districtCode', districtCode ?? '');
        return this.requestGet(url_);
    }
}
