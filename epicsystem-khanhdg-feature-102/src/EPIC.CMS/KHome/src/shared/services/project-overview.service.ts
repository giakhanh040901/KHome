import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { IDropdown, ProjectConst, SortConst, YesNoConst } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import {
  API_BASE_URL,
  ServiceProxyBase,
} from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable()
export class ProjectOverviewService extends ServiceProxyBase {

  public _listOwner: BehaviorSubject<IDropdown[] | undefined>;
  public _listOwner$: Observable<IDropdown[] | undefined>;
  public _listBank: BehaviorSubject<IDropdown[] | undefined>;
  public _listBank$: Observable<IDropdown[] | undefined>;
  public _listProvince: BehaviorSubject<IDropdown[] | undefined>;
  public _listProvince$: Observable<IDropdown[] | undefined>;
  public _projectOverviewDetail: BehaviorSubject<any | undefined>;
  public _projectOverviewDetail$: Observable<any | undefined>;
  public selectedProjectId: number;
  public selectedProjectDTO: any;
  private readonly baseAPI = "/api/real-estate/project";
  private readonly baseAPIOwner = "/api/real-estate/owner";
  private readonly baseAPIBank = "/api/core/bank";
  private readonly baseAPIProvince = "/api/core/province";
  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this._listOwner = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listOwner$ = this._listOwner.asObservable();
    this._listBank = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listBank$ = this._listBank.asObservable();
    this._listProvince = new BehaviorSubject<IDropdown[] | undefined>(
      undefined
    );
    this._listProvince$ = this._listProvince.asObservable();
    this._projectOverviewDetail = new BehaviorSubject<any | undefined>(
      undefined
    );
    this._projectOverviewDetail$ = this._projectOverviewDetail.asObservable();
    this.init();
  }

  private init() {
    const urlListOwner_ = this.baseAPIOwner + "/get-all-by-partner";
    this.requestGet(urlListOwner_).subscribe((res: any) => {
      const listOwner: IDropdown[] = res?.data?.map(
        (item: any) =>
          ({
            code: item.id,
            name: item.ownerName,
          } as IDropdown)
      );
      this._listOwner.next(listOwner);
    });

    const urlListBank_ = this.baseAPIBank + "/all";
    this.requestGet(urlListBank_).subscribe((res: any) => {
      console.log("res ser",res);
      const listBank: IDropdown[] = res.data.items.map(
        (item: any) =>
          ({
            code: item.bankId,
            name: item.bankName + ' - ' + item.fullBankName,
          } as IDropdown)
      );
      this._listBank.next(listBank);
    });

    const urlListProvince_ = this.baseAPIProvince;
    this.requestGet(urlListProvince_).subscribe((res: any) => {
      const listProvince: IDropdown[] = res.data.map(
        (item: any) =>
          ({
            code: item.code,
            name: item.fullName,
          } as IDropdown)
      );
      this._listProvince.next(listProvince);
    });
  }

  public getAllProject(page: Page, filters: any, sortData?: any[]): Observable<any> {
    let url_ = this.baseAPI + "/find-all?";
    if(page.keyword) url_ += this.convertParamUrl("keyword", page.keyword);
    if(filters?.status) url_ += this.convertParamUrl("status", filters.status);
    if(filters?.ownerId) url_ += this.convertParamUrl("ownerId", filters.ownerId);
    if(filters?.projectType) url_ += this.convertParamUrl("projectType", filters.projectType);
    if (filters?.productTypes){
      filters?.productTypes.forEach(item => {
        url_ += this.convertParamUrl("productTypes", item);
      })
    }
    //
    if(sortData){
      sortData.forEach(item => {
          url_ += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
      })
    }
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    //
    return this.requestGet(url_);
  }

  public getAllProjectNoPaging(): Observable<any> {
    return this.requestGet('/api/real-estate/project/find-all?status=3&pageSize=-1');
  }

  public getProjectByTrading(): Observable<any> {
    let url_ = '/api/real-estate/project/get-all?';
    return this.requestGet(url_)
  }

  public getAllNoPaging(): Observable<any> {
    let url_ = this.baseAPI + "/find-all?";
    url_ += this.convertParamUrl("status", ProjectConst.HOAT_DONG);
    url_ += this.convertParamUrl("pageSize", -1);
    return this.requestGet(url_);
  }

  public getAllByTrading(): Observable<any> {
    let url_ = this.baseAPI + "/get-all";
    return this.requestGet(url_);
  }

  public createProject(body: any) {
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public updateProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}/update`);
  }

  public updateDescriptionProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}/update-overview-content`);
  }

  public delete(body: any) {
    return this.requestPut(body, `${this.baseAPI}/delete/${body.id}`);
  }

  public getProjectById() {
    const url_ = this.baseAPI + "/find-by-id/" +  this.selectedProjectId;
    this.requestGet(url_).subscribe((res: any) => {
      if (res.data) {
        this._projectOverviewDetail.next(res.data);
        this.selectedProjectDTO = res.data;
      }
    });
  }

  public getAllUtilitiProject(
    page?: Page,
    type?: number,
    group?: number,
    name?: string,
    isHighlight?: string,
    isSelected?: string,
  ): Observable<any> {
    let url_ = this.baseAPI + "-utility/find-all?";
    url_ += this.convertParamUrl("pageNumber", page?.getPageNumber() || 1);
    url_ += this.convertParamUrl("pageSize", page?.pageSize || -1);
    url_ += this.convertParamUrl("projectId", this.selectedProjectId);
    url_ += this.convertParamUrl("type", type || "");
    url_ += this.convertParamUrl("groupId", group || "");
    url_ += this.convertParamUrl("name", name || "");
    url_ += this.convertParamUrl("isHighlight", isHighlight || "");

    if(isSelected) url_ += this.convertParamUrl("isSelected", isSelected);
    return this.requestGet(url_);
  }

  public updateUtilitiProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}-utility/update`);
  }

  public deleteUtilitiProject(id: number) {
    return this.requestDelete(`/api/real-estate/project-utility/delete/${id}`);
  }

  public getAllOtherUtilitiProject(page: Page): Observable<any> {
    let url_ = this.baseAPI + "-utility-extend/get-all-utility-extend?";
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("projectId", this.selectedProjectId);
    return this.requestGet(url_);
  }

  public createOtherUtilitiProject(body: any) {
    return this.requestPost(body, `${this.baseAPI}-utility-extend/add`);
  }

  public getOtherUtilitiProjectById(id: number): Observable<any> {
    const url_ = this.baseAPI + "-utility-extend/get/" + id;
    return this.requestGet(url_);
  }

  public updateOtherUtilitiProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}-utility-extend/update`);
  }

  public updateStatusOtherUtilitiProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}-utility-extend/change-status`);
  }

  public deleteOtherUtilitiProject(id: number) {
    return this.requestDelete(`${this.baseAPI}-utility-extend/delete/${id}`);
  }

  public getAllGroupUtiliti() {
    let url_ = this.baseAPI + "-utility/get-all-group-utility";
    return this.requestGet(url_);
  }

  public getAllImageUtilitiProject(page: Page): Observable<any> {
    let url_ = this.baseAPI + "-utility-media/get-all-utility-media?";
    url_ += this.convertParamUrl("pageNumber", page.getPageNumber());
    url_ += this.convertParamUrl("pageSize", page.pageSize);
    url_ += this.convertParamUrl("projectId", this.selectedProjectId);
    return this.requestGet(url_);
  }

  public createImageUtilitiProject(body: any) {
    return this.requestPost(body, `${this.baseAPI}-utility-media/add`);
  }

  public getImageUtilitiProjectById(id: number): Observable<any> {
    const url_ = this.baseAPI + "-utility-media/get/" + id;
    return this.requestGet(url_);
  }

  public updateImageUtilitiProject(body: any) {
    return this.requestPut(body, `${this.baseAPI}-utility-media/update`);
  }

  public updateStatusImageUtilitiProject(id: number) {
    return this.requestPut(undefined, `${this.baseAPI}-utility-media/change-status/${id}`);
  }

  public deleteImageUtilitiProject(id: number) {
    return this.requestDelete(`${this.baseAPI}-utility-media/delete/${id}`);
  }

  // TRÌNH DUYỆT SẢN PHẨM
  requestApprove(body) {
    return this.requestPut(body, `${this.baseAPI}/request`);
  }

  // PHÊ DUYỆT HOẶC HỦY PHÂN PHỐI SẢN PHẨM
  approveOrCancel(body, approve: boolean): Observable<any> {
    if(approve) {
        return this.requestPut(body,  `${this.baseAPI}/approve`);
    }
    // CANCEL SP
    return this.requestPut(body,  `${this.baseAPI}/cancel`);
  }
}
