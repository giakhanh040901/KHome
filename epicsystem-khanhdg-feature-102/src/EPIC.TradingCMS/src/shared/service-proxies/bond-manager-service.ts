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
  import { AppConsts } from "@shared/AppConsts";

  @Injectable()
  export class ProductBondDetailServiceProxy extends ServiceProxyBase {
      constructor(
          @Inject(HttpClient) http: HttpClient,
          @Optional() @Inject(API_BASE_URL) baseUrl?: string
      ) {
          super(http, baseUrl);
          this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
      }

      create(body): Observable<any> {
          return this.requestPost(body, "/api/bond/product-bond-detail/add");
      }

      update(body, id: number): Observable<any> {
          return this.requestPut(body, "/api/bond/product-bond-detail/update");
      }

      delete(id: number): Observable<void> {
          let url_ = "/api/bond/product-bond-detail/delete/" + id;
          return this.requestDelete(url_);
      }

      get(id: number): Observable<any> {
          let url_ = "/api/bond/product-bond-detail/" + id;
          return this.requestGet(url_);
      }

      getAll(page: Page, market?: string): Observable<any> {
          let url_ = "/api/bond/product-bond-detail/find?";
          url_ += this.convertParamUrl("keyword", page.keyword);
          if(market) {
            url_ += this.convertParamUrl("market", market);
          }
          url_ += this.convertParamUrl('pageSize', page.pageSize);
          url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

          return this.requestGet(url_);
      }

      getAllBondInfo(page: Page): Observable<any> {
        let url_ = "/api/bond/product-bond-info/find?";
          url_ += this.convertParamUrl('pageSize', -1);

        return this.requestGet(url_);
    }
  }

  @Injectable()
export class ProductBondInfoServiceProxy extends ServiceProxyBase {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/product-bond-info/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/bond/product-bond-info/update/" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/product-bond-info/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/product-bond-info/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/bond/product-bond-info/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    return this.requestGet(url_);
  }

  getAllIssuer(page: Page): Observable<any> {
    let url_ = "/api/bond/issuer/find?";
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    return this.requestGet(url_);
  }

  getAllTradingProvider(page: Page): Observable<any> {
    let url_ = "/api/bond/trading-provider/find?";
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

    return this.requestGet(url_);
  }

  getAllDepositProvider(page: Page): Observable<any> {
    let url_ = "/api/bond/deposit-provider/find?";
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    return this.requestGet(url_);
  }

  getAllBondType(): Observable<any> {
    let url_ = "/api/bond/product-bond-type/find?";
    url_ += this.convertParamUrl('pageSize', -1);
    return this.requestGet(url_);
  }
}

@Injectable()
export class ContractTemplateServiceProxy extends ServiceProxyBase {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl);
        this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/contract-template/add");
    }

    uploadFileGetUrl(file: File): Observable<any> {
        let url_: string = "/api/image/file/upload";
        return this.requestPostFile(file, url_);
      }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/bond/contract-template/update/" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/contract-template/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/contract-template/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, contractTypeId: number): Observable<any> {
        let url_ = "/api/bond/contract-template/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if(contractTypeId) {
            url_ += this.convertParamUrl("contractTypeId", contractTypeId);
          }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }

    getAllContractTypeIssuer(page: Page): Observable<any> {
        let url_ = "//api/bond/contract-type/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

    return this.requestGet(url_);
    }

}

@Injectable()
export class ContractTypeServiceProxy extends ServiceProxyBase {
  constructor(
      @Inject(HttpClient) http: HttpClient,
      @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
      super(http, baseUrl);
  }

  create(body): Observable<any> {
      return this.requestPost(body, "/api/bond/contract-type/add");
  }

  update(body, id: number): Observable<any> {
      return this.requestPut(body, "/api/bond/contract-type/update/" + id);
  }

  delete(id: number): Observable<void> {
      let url_ = "/api/bond/contract-type/delete/" + id;
      return this.requestDelete(url_);
  }

  get(id: number): Observable<any> {
      let url_ = "/api/bond/contract-type/find/" + id;
      return this.requestGet(url_);
  }

  getAll(page: Page): Observable<any> {
      let url_ = "/api/bond/contract-type/find?";
      url_ += this.convertParamUrl("keyword", page.keyword);
      url_ += this.convertParamUrl('pageSize', page.pageSize);
      url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

      return this.requestGet(url_);
  }
}
