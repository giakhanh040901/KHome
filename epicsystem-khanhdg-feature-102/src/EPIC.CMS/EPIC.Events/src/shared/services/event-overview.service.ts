import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class EventOverviewService extends ServiceProxyBase {
  private readonly baseAPI = '/api/event/manager-event';
  private readonly baseAPITicket = '/api/event/ticket';
  public eventId: number;
  public eventDetailInforId: number;
  public currentUser: string = '';
  public currentTime = new Date();
  public showBtnOpenSell: boolean = false;
  public _listProvince: BehaviorSubject<IDropdown[] | undefined>;
  public _listProvince$: Observable<IDropdown[] | undefined>;
  public _listAccountMoney: BehaviorSubject<IDropdown[] | undefined>;
  public _listAccountMoney$: Observable<IDropdown[] | undefined>;
  public _listContractCode: BehaviorSubject<IDropdown[] | undefined>;
  public _listContractCode$: Observable<IDropdown[] | undefined>;
  public isEdit: boolean = false;

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this.currentUser = this.appSessionService.user.userName;
    this._listProvince = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listProvince$ = this._listProvince.asObservable();
    this._listAccountMoney = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listAccountMoney$ = this._listAccountMoney.asObservable();
    this._listContractCode = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listContractCode$ = this._listContractCode.asObservable();
  }

  public findAll(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.status && filter?.status.length) url_ += this.convertParamTypeArrayUrl('status', filter.status);
    if (filter?.type && filter?.type.length) url_ += this.convertParamTypeArrayUrl('eventType', filter.type);
    if (filter?.startDate) url_ += this.convertParamUrl('startDate', filter.startDate);
    return this.requestGet(url_);
  }

  public createOrEdit(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}/update`);
    }
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public changeStatus(id: number) {
    return this.requestPut(
      {
        configContractCodeId: id,
      },
      `${this.baseAPI}/update-status/${id}`
    );
  }

  public changeShowAppEvent(id: number) {
    return this.requestPut(
      {
        id,
      },
      `${this.baseAPI}/change-show-app/${id}`
    );
  }

  public openSellEvent(id: number) {
    return this.requestPut(
      {
        id,
      },
      `${this.baseAPI}/open-event/${id}`
    );
  }

  public stopOrCancelEvent(body: any, type: string) {
    return this.requestPut(body, `${this.baseAPI}/${type}-event/`);
  }

  public getEventDetail() {
    return this.requestGet(`${this.baseAPI}/find-by-id/${this.eventId}`);
  }

  public saveContent(body) {
    return this.requestPut(body, `${this.baseAPI}/update-overview-content`);
  }

  public getListProvice() {
    this.requestGet('/api/core/province').subscribe((res: any) => {
      if (res && res.data) {
        const listProvince: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.code,
              label: item.name,
            } as IDropdown)
        );
        this._listProvince.next(listProvince);
      }
    });
  }

  public getListAccountMoney() {
    this.requestGet('/api/core/trading-provider/list-bank').subscribe((res: any) => {
      if (res && res.data) {
        const listAccountMoney: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.businessCustomerBankAccId,
              label: `${item.bankName} - ${item.bankAccNo}`,
            } as IDropdown)
        );
        this._listAccountMoney.next(listAccountMoney);
      }
    });
  }

  public getListContractCode() {
    this.requestGet('/api/event/config-contract-code/get-all-status-active').subscribe((res: any) => {
      if (res && res.data) {
        const listContractCode: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.id,
              label: item.name,
            } as IDropdown)
        );
        this._listContractCode.next(listContractCode);
      }
    });
  }

  public findAllInfor(page: Page): Observable<any> {
    let url_ = this.baseAPI + '/event-detail/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('eventId', this.eventId);
    return this.requestGet(url_);
  }

  public createOrEditTicket(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPITicket}/update`);
    }
    return this.requestPost(body, `${this.baseAPITicket}/add`);
  }

  public createEventDetailInfor() {
    const body = {
      eventId: this.eventId,
    };
    return this.requestPost(body, `${this.baseAPI}/event-detail/add`);
  }

  public updateEventDetailInfor(body: any) {
    return this.requestPut(body, `${this.baseAPI}/event-detail/update`);
  }

  public getEventDetailInfor(id: number) {
    return this.requestGet(`${this.baseAPI}/event-detail/find-by-id/${id}`);
  }

  public deleteEventDetailInfor(id: number) {
    return this.requestDelete(`${this.baseAPI}/event-detail/delete/${id}`);
  }

  public replicateTicket(body: any) {
    return this.requestPost(body, `${this.baseAPITicket}/replicate-ticket`);
  }

  public changeStatusTicket(id: number) {
    return this.requestPut({ id }, `${this.baseAPITicket}/update-status/${id}`);
  }

  public changeShowAppTicket(id: number) {
    return this.requestPut({ id }, `${this.baseAPITicket}/change-show-app/${id}`);
  }

  public getTicketDetail(id: number) {
    return this.requestGet(`${this.baseAPITicket}/find/${id}`);
  }

  public deleteImgTicket(id: number) {
    return this.requestDelete(`${this.baseAPITicket}/delete/${id}`);
  }

  public changeStatusEventDetailInfor(id: number) {
    return this.requestPut(
      {
        id,
      },
      `${this.baseAPI}/event-detail/change-status/${id}`
    );
  }

  public createOrUpdateEventDetailMedia(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `/api/event/event-media/update`);
    }
    return this.requestPost(body, `/api/event/event-media/add`);
  }

  public getAllEventDetailMedia() {
    let url_ = '/api/event/event-media/find-all?';
    url_ += this.convertParamUrl('pageSize', -1);
    url_ += this.convertParamUrl('eventId', this.eventId);
    return this.requestGet(url_);
  }

  public deleteEventDetailMedia(id: number) {
    return this.requestDelete(`/api/event/event-media/delete/${id}`);
  }

  public createOrUpdateEventAdmin(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `/api/event/admin-event/add`);
    }
    return this.requestPost(body, `/api/event/admin-event/add`);
  }

  public getAllEventAdmin() {
    let url_ = `/api/event/admin-event/find-all/${this.eventId}`;
    return this.requestGet(url_);
  }

  public deleteEventAdmin(id: number) {
    return this.requestDelete(`/api/event/admin-event/delete/${id}`);
  }

  getInfoInvestorCustomer(keyword:string) {
      let url_ = "/api/core/manager-investor/filter?requireKeyword=true&";
      if(keyword) {
        url_ += this.convertParamUrl("keyword", keyword);
      }
      url_ += this.convertParamUrl('pageSize', -1);
      return this.requestGet(url_);
  }

  public uploadFileEventDetail(file: File, folderFnc = ''): Observable<any> {
    let folder = `${AppConsts.folder}/${folderFnc}`;
    let url_: string = `/api/file/upload?folder=${folder}`;
    return this.requestPostFile(file, url_);
  }

  public findAllTicketOfEvent(): Observable<any> {
    let url_ = this.baseAPI + '/ticket-template?';
    url_ += this.convertParamUrl('pageSize', -1);
    url_ += this.convertParamUrl('eventId', this.eventId);
    return this.requestGet(url_);
  }

  public createOrEditTicketOfEvent(body: any, isEdit: boolean) {
    body = { ...body, eventId: this.eventId };
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}/ticket-template`);
    }
    return this.requestPost(body, `${this.baseAPI}/ticket-template`);
  }

  public changeStatusTicketOfEvent(id: number) {
    return this.requestPut(
      {
        id,
      },
      `${this.baseAPI}/ticket-template/change-status?id=${id}`
    );
  }

  public downloadFileTicketWord(id: number): Observable<any> {
    return this.requestDownloadFile(`${this.baseAPI}/file-ticket-template-word/${id}`);
  }

  public viewFileTicketPDF(id: number): Observable<any> {
    return this.viewFilePDFFromAPI(`${this.baseAPI}/file-ticket-template-pdf/${id}`);
  }

  public findAllDeliTicketOfEvent(): Observable<any> {
    let url_ = this.baseAPI + '/delivery-ticket-template/find-all?';
    url_ += this.convertParamUrl('pageSize', -1);
    url_ += this.convertParamUrl('eventId', this.eventId);
    return this.requestGet(url_);
  }

  public createOrEditDeliTicketOfEvent(body: any, isEdit: boolean) {
    body = { ...body, eventId: this.eventId };
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}/delivery-ticket-template/update`);
    }
    return this.requestPost(body, `${this.baseAPI}/delivery-ticket-template/add`);
  }

  public changeStatusDeliTicketOfEvent(id: number) {
    return this.requestPut(
      {
        id,
      },
      `${this.baseAPI}/delivery-ticket-template/change-status/${id}`
    );
  }

  public downloadFileDeliTicketWord(id: number): Observable<any> {
    return this.requestDownloadFile(`${this.baseAPI}/delivery-ticket-template/file-word/${id}`);
  }

  public viewFileDeliTicketPDF(id: number): Observable<any> {
    return this.viewFilePDFFromAPI(`${this.baseAPI}/delivery-ticket-template/file-pdf/${id}`);
  }
}
