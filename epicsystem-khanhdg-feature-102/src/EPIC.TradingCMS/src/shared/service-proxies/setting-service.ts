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
import { extend, isBuffer } from "lodash-es";

@Injectable()
export class IssuerServiceProxy extends ServiceProxyBase {
  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(http, baseUrl);
  }

  create(body): Observable<any> {
    return this.requestPost(body, "/api/bond/issuer/add");
  }

  update(body, id: number): Observable<any> {
    return this.requestPut(body, "/api/bond/issuer/update/" + id);
  }

  delete(id: number): Observable<void> {
    let url_ = "/api/bond/issuer/delete/" + id;
    return this.requestDelete(url_);
  }

  get(id: number): Observable<any> {
    let url_ = "/api/bond/issuer/" + id;
    return this.requestGet(url_);
  }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/bond/issuer/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class DepositProviderServiceProxy extends ServiceProxyBase {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl);
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/deposit-provider/add");
    }

    update(body, id: number): Observable<any> {
        return this.requestPut(body, "/api/bond/deposit-provider/update/" + id);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/deposit-provider/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/deposit-provider/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page): Observable<any> {
        let url_ = "/api/bond/deposit-provider/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
}

@Injectable()
export class CalendarServiceProxy extends ServiceProxyBase {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl);
    }

    create(body): Observable<any> {
      return this.requestPost(body, "/api/bond/calendar/add");
    }

    update(body): Observable<any> {
      return this.requestPut(body, "/api/bond/calendar/update/");
    }

    get(workingDate): Observable<any> {
      let url_ = "/api/bond/calendar/" + workingDate;
      return this.requestGet(url_);
    }

    getAll(workingYear): Observable<any> {
        let url_ = "/api/bond/calendar/find/" + workingYear;
        return this.requestGet(url_);
    }
}

@Injectable()
export class TradingProviderServiceProxy extends ServiceProxyBase {
  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(http, baseUrl);
  }

  create(body): Observable<any> {
    return this.requestPost(body, "/api/bond/trading-provider/add");
  }

  update(body, id: number): Observable<any> {
    return this.requestPut(body, "/api/bond/trading-provider/update/" + id);
  }

  delete(id: number): Observable<void> {
    let url_ = "/api/bond/trading-provider/delete/" + id;
    return this.requestDelete(url_);
  }

  get(id: number): Observable<any> {
    let url_ = "/api/bond/trading-provider/find/" + id;
    return this.requestGet(url_);
  }

  getAll(page: Page): Observable<any> {
    let url_ = "/api/bond/trading-provider/find?";
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

    return this.requestGet(url_);
  }
}

@Injectable()
export class ProductCategoryServiceProxy extends ServiceProxyBase {
  constructor(
      @Inject(HttpClient) http: HttpClient,
      @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
      super(http, baseUrl);
  }

  create(body): Observable<any> {
      return this.requestPost(body, "/api/bond/product-category/add");
  }

  update(body, id: number): Observable<any> {
      return this.requestPut(body, "/api/bond/product-category/update/" + id);
  }

  delete(id: number): Observable<void> {
      let url_ = "/api/bond/product-category/delete/" + id;
      return this.requestDelete(url_);
  }

  get(id: number): Observable<any> {
      let url_ = "/api/bond/product-category/find/" + id;
      return this.requestGet(url_);
  }

  getAll(page: Page): Observable<any> {
      let url_ = "/api/bond/product-category/find?";
      url_ += this.convertParamUrl("keyword", page.keyword);
      url_ += this.convertParamUrl('pageSize', page.pageSize);
      url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

      return this.requestGet(url_);
  }
}

@Injectable()
export class ProductBondInterestServiceProxy extends ServiceProxyBase {
  constructor(
      @Inject(HttpClient) http: HttpClient,
      @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
      super(http, baseUrl);
      this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
  }

  create(body): Observable<any> {
      return this.requestPost(body, "/api/bond/product-bond-interest/add");
  }

  update(body, id: number): Observable<any> {
      return this.requestPut(body, "/api/bond/product-bond-interest/update");
  }

  delete(id: number): Observable<void> {
      let url_ = "/api/bond/product-bond-interest/delete/" + id;
      return this.requestDelete(url_);
  }

  get(id: number): Observable<any> {
      let url_ = "/api/bond/product-bond-interest/" + id;
      return this.requestGet(url_);
  }

  getAll(page: Page): Observable<any> {
      let url_ = "/api/bond/product-bond-interest/find?";
      url_ += this.convertParamUrl("keyword", page.keyword);
      url_ += this.convertParamUrl('pageSize', page.pageSize);
      url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

      return this.requestGet(url_);
  }
}

@Injectable()
export class ProductBondTypeServiceProxy extends ServiceProxyBase {
  constructor(
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(http, baseUrl);
  }

  create(body): Observable<any> {
    return this.requestPost(body, "/api/bond/product-bond-type/add");
  }

  update(body, id: number): Observable<any> {
    return this.requestPut(body, "/api/bond/product-bond-type/update/" + id);
  }

  delete(id: number): Observable<void> {
    let url_ = "/api/bond/product-bond-type/delete/" + id;
    return this.requestDelete(url_);
  }

  get(id:number):Observable<any>{
    let url_ = "/api/bond/product-bond-type/find/" +id;
    return this.requestGet(url_);
  }

  getAll(page: Page): Observable<any> {
    let url_ = "/api/bond/product-bond-type/find?";
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
    return this.requestGet(url_);
  }
}

@Injectable()
export class ProductPolicyServiceProxy extends ServiceProxyBase {
    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(http, baseUrl);
        this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
    }

    create(body): Observable<any> {
        return this.requestPost(body, "/api/bond/product-policy/add");
    }

    update(body, id: number): Observable<any> {
        let url_ = "/api/bond/product-policy/update/" + id;
        return this.requestPut(body, url_);
    }

    delete(id: number): Observable<void> {
        let url_ = "/api/bond/product-policy/delete/" + id;
        return this.requestDelete(url_);
    }

    get(id: number): Observable<any> {
        let url_ = "/api/bond/product-policy/find/" + id;
        return this.requestGet(url_);
    }

    getAll(page: Page, market?: string): Observable<any> {

        let url_ = "/api/bond/product-policy/find?";
        url_ += this.convertParamUrl("keyword", page.keyword);
        if(market) {
          console.log({marketSearch: market});
          url_ += this.convertParamUrl("market", market);
        }
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());

        return this.requestGet(url_);
    }
  }
