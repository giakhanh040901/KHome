import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { IDropdown } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import {
  API_BASE_URL,
  ServiceProxyBase,
} from "@shared/service-proxies/service-proxies-base";
import { TradingProviderServiceProxy } from "@shared/service-proxies/setting-service";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { BehaviorSubject, Observable, of } from "rxjs";

@Injectable()
export class ProductDistributionService extends ServiceProxyBase {
  public _listProject: BehaviorSubject<IDropdown[] | undefined>;
  public _listProject$: Observable<IDropdown[] | undefined>;
  public _listAgency: BehaviorSubject<IDropdown[] | undefined>;
  public _listAgency$: Observable<IDropdown[] | undefined>;
  public _listPolicy: BehaviorSubject<IDropdown[] | undefined>;
  public _listPolicy$: Observable<IDropdown[] | undefined>;
  public _listContractFormCode: BehaviorSubject<IDropdown[] | undefined>;
  public _listContractFormCode$: Observable<IDropdown[] | undefined>;
  public distributionId: number;
  private readonly baseAPI = "/api/real-estate/distribution";

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    public tradingProviderService: TradingProviderServiceProxy,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this._listProject = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listProject$ = this._listProject.asObservable();
    this._listAgency = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listAgency$ = this._listAgency.asObservable();
    this._listPolicy = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listPolicy$ = this._listPolicy.asObservable();
    this._listContractFormCode = new BehaviorSubject<IDropdown[] | undefined>(
      undefined
    );
    this._listContractFormCode$ = this._listContractFormCode.asObservable();
    // this.init();
  }

  public init() {
    let urlListProject_ = "/api/real-estate/project/find-all?";
    urlListProject_ += this.convertParamUrl("pageSize", -1);
    this.requestGet(urlListProject_).subscribe((res: any) => {
      if (res?.data?.items) {
        const listProject: IDropdown[] = res?.data?.items.map(
          (item: any) =>
            ({
              code: item.id,
              name: item.name,
            } as IDropdown)
        );
        this._listProject.next(listProject);
      }
    });

    this.tradingProviderService.getAllNoPaging().subscribe((res: any) => {
      if (res?.data?.items) {
        const listAgency: IDropdown[] = res?.data?.items.map(
          (item: any) =>
            ({
              code: item.tradingProviderId,
              name: item.name,
            } as IDropdown)
        );
        this._listAgency.next(listAgency);
      }
    });

    let urlListContractFormCode_ = `/api/real-estate/config-contract-code/get-all?pageSize=-1`;
    this.requestGet(urlListContractFormCode_).subscribe((res) => {
      if (res?.data?.items) {
        const listContractFormCode = res.data.items.map(
          (e: any) =>
            ({
              code: e.id,
              name: e.name,
            } as IDropdown)
        );
        this._listContractFormCode.next(listContractFormCode);
      }
    });

    let urlListPolicy_ = `/api/real-estate/distribution-policy-temp/find-all?pageSize=-1`;
    this.requestGet(urlListPolicy_).subscribe((res) => {
      if (res?.data?.items) {
        const listPolicy = res.data.items.map(
          (e: any) =>
            ({
              code: e.id,
              name: e.name,
              rawData: e,
            } as IDropdown)
        );
        this._listPolicy.next(listPolicy);
      }
    });
  }

  /* #region product-distribution */
  public getAllProductDistribution(
    page: Page,
    project?: number,
    agency?: number,
    status?: number
  ): Observable<any> {
    let url_ = this.baseAPI + "/find-all?";
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("keyword", page.keyword);
    url_ += this.convertParamUrl("projectId", project ?? "");
    url_ += this.convertParamUrl("tradingProviderId", agency ?? "");
    url_ += this.convertParamUrl("status", status ?? "");
    return this.requestGet(url_);
  }

  public findById(id: number) {
    return this.requestGet(`${this.baseAPI}/find-by-id/${id}`);
  }

  public createOrEditProductDistribution(body): Observable<any> {
    if (body.id) return this.requestPut(body, `${this.baseAPI}/update`);
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public deleteProductDistribution(id: number) {
    return this.requestPut({ id }, `${this.baseAPI}/delete/${id}`);
  }

  public stopDistribution(id: number) {
    return this.requestPut({ id }, `${this.baseAPI}/pause/${id}`);
  }
  /* #endregion */

  /* #region product-distribution-list */
  public getAllProductList(page: Page) {
    let url_ = `${this.baseAPI}/find-all-item?`;
    url_ += this.convertParamUrl("distributionId", this.distributionId);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

    return this.requestGet(url_);
  }

  public deleteProductDistributionItem(ids: number[]) {
    const body = {
      id: this.distributionId,
      distributionItemIds: ids,
    };
    return this.requestPut(
      body,
      `${this.baseAPI}/distribution-product-item/delete`
    );
  }

  public setLock(id: number) {
    return this.requestPut(
      id,
      `${this.baseAPI}/distribution-product-item/lock/${id}`
    );
  }
  /* #endregion */

  /* #region distribution-policy */
  public getAllDistributionPolicy(page: Page) {
    let url_ = `${this.baseAPI}-policy/find-all?`;
    url_ += this.convertParamUrl("distributionId", this.distributionId);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

    return this.requestGet(url_);
  }

  public createDistributionPolicy(body): Observable<any> {
    return this.requestPost(body, `${this.baseAPI}-policy/add`);
  }

  public getDistributionPolicyDetail(id: number) {
    return this.requestGet(`${this.baseAPI}-policy/find-by-id/${id}`);
  }

  public deleteDistributionPolicy(id: number) {
    return this.requestDelete(`${this.baseAPI}-policy/delete/${id}`);
  }

  public activePolicy(id: number, distributionId: number){
    return this.requestPut(null, `/api/real-estate/distribution-policy/active-distribution-policy/${id}?DistributionId=${distributionId}`)
  }

  public changeStatusDistributionPolicy(id: number) {
    return this.requestPut(id, `${this.baseAPI}-policy/change-status/${id}`);
  }
  /* #endregion */

  /* #region contract-form */
  public getAllContractForm(page: Page) {
    let url_ = `${this.baseAPI}-contract-template/find-all?`;
    url_ += this.convertParamUrl("distributionId", this.distributionId);
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());

    return this.requestGet(url_);
  }

  public getListContractForm() {
    let urlListContractForm_ =
      "/api/real-estate/contract-template-temp/find-all?";
    urlListContractForm_ += this.convertParamUrl("pageSize", -1);
    return this.requestGet(urlListContractForm_);
  }

  public getContractFormDetail(id: number) {
    return this.requestGet(
      `${this.baseAPI}-contract-template/find-by-id/${id}`
    );
  }

  public createOrEditContractForm(body): Observable<any> {
    if (body.id)
      return this.requestPut(body, `${this.baseAPI}-contract-template/update`);
    return this.requestPost(body, `${this.baseAPI}-contract-template/add`);
  }

  public getListDistributionPolicy() {
    let urlListDistributionPolicy_ = `${this.baseAPI}-policy/find-all?`;
    urlListDistributionPolicy_ += this.convertParamUrl(
      "distributionId",
      this.distributionId
    );
    urlListDistributionPolicy_ += this.convertParamUrl("pageSize", -1);
    return this.requestGet(urlListDistributionPolicy_);
  }

  public downloadFile(id: number, key: string) {
    if (key === "word") {
    } else if (key === "pdf") {
    }
    return of(1);
  }

  public cancelActiveContractForm(id: number) {
    return this.requestPut(id, `${this.baseAPI}-contract-template/change-status/${id}`);
  }

  public getListContract(contractModel: number, contractType: number) {
    let urlListContract_ = `/api/real-estate/contract-template-temp/find-all?`;
    urlListContract_ += this.convertParamUrl("pageSize", -1);
    urlListContract_ += this.convertParamUrl(
      "contractSource",
      contractModel || ""
    );
    urlListContract_ += this.convertParamUrl(
      "contractType",
      contractType || ""
    );
    return this.requestGet(urlListContract_);
  }
  /* #endregion */
}
