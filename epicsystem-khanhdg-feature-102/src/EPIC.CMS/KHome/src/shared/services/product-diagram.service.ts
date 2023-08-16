import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProductDiagramService extends ServiceProxyBase { 
    private endPoint = `/api/real-estate/product-item`;
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findAll(page: Page, productId: number, filter?: any): Observable<any> {
        let url_ = `${this.endPoint}/find-by-id/${productId}`
        // url_ += this.convertParamUrl('projectId', productId);
        // if (filter.selected) url_ += this.convertParamUrl('selected', filter.selected);
        // if (filter.keyword) url_ += this.convertParamUrl("keyword", filter.keyword);
        // url_ += this.convertParamUrl('pageSize', page.pageSize);
        // url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        return this.requestGet(url_);
    }

    updateDiagramContent(body): Observable<any> {
        return this.requestPut(body, `${this.endPoint}/update-design-diagram-content`);
    }

    
    findAllFile(productItemId: number){
        return this.requestGet(`${this.endPoint}/get-design-diagram-file/${productItemId}`);
    }

    createFile(body): Observable<any>{
        return this.requestPost(body, `${this.endPoint}/add-design-diagram-file`);
    }

    updateFile(body): Observable<any>{
        return this.requestPut(body, `${this.endPoint}/update-design-diagram-file`);
    }

    deleteFile(id: number): Observable<any> {
        return this.requestDelete(`${this.endPoint}/delete-design-diagram-file/`+ id);
    }

}