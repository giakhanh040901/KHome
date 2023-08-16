import { Inject, Injectable, Optional } from '@angular/core';
import { API_BASE_URL, ServiceProxyBase } from './service-proxies-base';
import { MessageService } from 'primeng/api';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DigitalSignServiceService {

  constructor() { }
}

@Injectable()
export class DigitalSignServiceProxy extends ServiceProxyBase {
    constructor(
        messageService: MessageService,
        _cookieService: CookieService,
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    /**
     * cập nhập chữ ký số của khách hàng doanh nghiệp bảng chính
     * @param body 
     * @param id 
     * @returns 
     */
    updateDigitalSign(body, id: number): Observable<any> {
        let url_ = "/api/core/business-customer/update-digital-sign/" + id;
        return this.requestPut(body, url_);
    }

    /**
     * cập nhập chữ ký số của khách hàng doanhg nghiệp bảng tạm
     * @param body 
     * @param id 
     * @returns 
     */
    updateDigitalSignTemp(body, id: number): Observable<any> {
        let url_ = "/api/core/business-customer/update-digital-sign-temp/" + id;
        return this.requestPut(body, url_);
    }

    /**
     * lấy dữ liệu chữ ký số của bảng khách hàng doanh nghiệp bảng tạm
     * @param id 
     * @returns 
     */
    getDigitalSignTemp(id: number) {
        let url_ = "/api/core/business-customer/get-digital-sign-temp/" + id;
        return this.requestGet(url_);
    }

    /**
     * lấy dữ liệu thống tin chữ ký số của khách hàng doanh nghiệp bảng thật
     * @param id 
     * @returns 
     */
    getDigitalSign(id: number) {
        let url_ = "/api/core/business-customer/get-digital-sign/" + id;
        return this.requestGet(url_);
    }

    getDigitalSignTradingProvider(): Observable<any> {
		let url_ = `/api/core/trading-provider/get-digital-sign`;
		return this.requestGet(url_);
	}
    updateDigitalSignTradingProvider(body): Observable<any> {
        let url_ = "/api/core/trading-provider/update-digital-sign";
        return this.requestPut(body, url_);
    }
}