import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { ETypeFormatDate, SaleTicketOrder, YES_NO } from '@shared/AppConsts';
import { formatDate } from '@shared/function-common';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import {
  CreateSaleTicketOrderEvent,
  CustomerSearchModel,
  IntroduceSearchModel,
} from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { SpinnerService } from './spinner.service';

@Injectable()
export class SaleTicketOrderService extends ServiceProxyBase {
  public selectedOrder: {
    customer: CustomerSearchModel;
    introduce: IntroduceSearchModel;
    event: CreateSaleTicketOrderEvent;
  } = {
    customer: new CustomerSearchModel(),
    introduce: new IntroduceSearchModel(),
    event: new CreateSaleTicketOrderEvent(),
  };
  public dtoCustomer: CustomerSearchModel = new CustomerSearchModel();
  public dtoIntroduce: IntroduceSearchModel = new IntroduceSearchModel();
  public dtoEvent: CreateSaleTicketOrderEvent = new CreateSaleTicketOrderEvent();
  public selectedOrderId: number;
  public selectedOrderStatus: number;
  public currentUser: string;
  public totalValue: number = 0;
  public requestCode: string = '';
  public isEdit: boolean = false;

  public _listEventActiveToOrder: BehaviorSubject<IDropdown[] | undefined>;
  public _listEventActiveToOrder$: Observable<IDropdown[] | undefined>;
  public _listTicketOfEvent: BehaviorSubject<IDropdown[] | undefined>;
  public _listTicketOfEvent$: Observable<IDropdown[] | undefined>;
  public _listBankAccount: BehaviorSubject<IDropdown[] | undefined>;
  public _listBankAccount$: Observable<IDropdown[] | undefined>;
  public _listAddressOfCustomer: BehaviorSubject<IDropdown[] | undefined>;
  public _listAddressOfCustomer$: Observable<IDropdown[] | undefined>;
  public _saleTicketOrderDetail: BehaviorSubject<any | undefined>;
  public _saleTicketOrderDetail$: Observable<any | undefined>;

  private readonly baseAPI = '/api/event/order';
  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    private spinnerService: SpinnerService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this._listEventActiveToOrder = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listEventActiveToOrder$ = this._listEventActiveToOrder.asObservable();
    this._listTicketOfEvent = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listTicketOfEvent$ = this._listTicketOfEvent.asObservable();
    this._listBankAccount = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listBankAccount$ = this._listBankAccount.asObservable();
    this._listAddressOfCustomer = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listAddressOfCustomer$ = this._listAddressOfCustomer.asObservable();
    this._saleTicketOrderDetail = new BehaviorSubject<any | undefined>(undefined);
    this._saleTicketOrderDetail$ = this._saleTicketOrderDetail.asObservable();
    this.currentUser = this.appSessionService.user.userName;
  }

  public getDataCustomerSearch(keyword: string): Observable<any> {
    if (keyword && keyword.length) {
      let url_ = '/api/core/manager-investor/filter?requireKeyword=true&';
      url_ += this.convertParamUrl('keyword', keyword);
      url_ += this.convertParamUrl('pageSize', -1);
      return this.requestGet(url_);
    }
    return of();
  }

  public getDataBusinessSearch(keyword: string): Observable<any> {
    if (keyword && keyword.length) {
      let url_ = '/api/core/business-customer/find?';
      url_ += this.convertParamUrl('keyword', keyword);
      url_ += this.convertParamUrl('pageSize', -1);
      return this.requestGet(url_);
    }
    return of();
  }

  public getDataIntroduceSearch(keyword: string): Observable<any> {
    if (keyword && keyword.length) {
      let url_ = `/api/core/manager-sale/find-by-trading-provider?`;
      url_ += this.convertParamUrl('referralCode', keyword);
      return this.requestGet(url_);
    }
    return of();
  }

  public getEventActiveToOrder() {
    this.requestGet('/api/event/manager-event/find-active').subscribe((res: any) => {
      if (res && res.data && res.data.length) {
        const listEventActiveToOrder: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.id,
              label: item.name,
              rawData: item,
            } as IDropdown)
        );
        this._listEventActiveToOrder.next(listEventActiveToOrder);
      }
    });
  }

  public getTicketOfEvent(eventId: number) {
    this.requestGet('/api/event/manager-event/event-detail/find-active/' + eventId).subscribe((res: any) => {
      if (res && res.data && res.data && res.data.length) {
        const listTicketOfEvent: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.id,
              label: `${item.startDate ? formatDate(item.startDate, ETypeFormatDate.DATE_TIME) : ''} - ${
                item.endDate ? formatDate(item.endDate, ETypeFormatDate.DATE_TIME) : ''
              }`,
              rawData: item,
            } as IDropdown)
        );
        this._listTicketOfEvent.next(listTicketOfEvent);
      } else {
        this._listTicketOfEvent.next([]);
      }
    });
  }

  public findAllSaleTicketOrder(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.status && filter?.status.length) {
      filter.status.forEach((item: any) => {
        if (item === SaleTicketOrder.HET_THOI_GIAN) {
          url_ += this.convertParamUrl('timeOut', true);
        } else {
          url_ += this.convertParamUrl('status', item);
        }
      });
    }
    if (filter?.event && filter?.event.length) url_ += this.convertParamTypeArrayUrl('eventId', filter.event);
    if (filter?.source && filter?.source.length) url_ += this.convertParamTypeArrayUrl('orderer', filter.source);
    return this.requestGet(url_);
  }

  public getListAddressOfCustomer() {
    this.requestGet(
      `/api/core/manager-investor/list-contact-address?investorId=${this.dtoCustomer.id}&isTemp=false&keyword=&pageSize=25&pageNumber=1`
    ).subscribe((res: any) => {
      this._listAddressOfCustomer.next(undefined);
      if (res && res.data && res.data.items && res.data.items.length) {
        const listAddressOfCustomer: IDropdown[] = res.data.items
          .filter((item: any) => item.isDefault === YES_NO.YES)
          .map(
            (item: any) =>
              ({
                value: item.contactAddressId,
                label: item.contactAddress,
              } as IDropdown)
          );
        this._listAddressOfCustomer.next(listAddressOfCustomer);
      }
    });
  }

  public getSaleTicketOrderDetail() {
    this.spinnerService.isShow = true;
    this.requestGet(`${this.baseAPI}/${this.selectedOrderId}`).subscribe(
      (res: any) => {
        this.spinnerService.isShow = false;
        this._saleTicketOrderDetail.next(res);
      },
      (err) => {
        this.spinnerService.isShow = false;
      }
    );
  }

  public holdSaleTicketOrder(body: any) {
    return this.requestPut(body, `${this.baseAPI}/extended-time`);
  }

  public createSaleTicketOrder(body: any) {
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public updateSaleTicketOrder(body: any) {
    return this.requestPut(body, `${this.baseAPI}/update`);
  }

  public deleteSaleTicketOrder(id: number) {
    return this.requestDelete(`${this.baseAPI}/delete/${id}`);
  }

  public approveSaleTicketOrder(id: number) {
    return this.requestPut({ orderId: id }, `${this.baseAPI}/approve?orderId=${id}`);
  }

  public requestBill(id: number) {
    return this.requestPut({ orderId: id }, `${this.baseAPI}/invoice-request?orderId=${id}`);
  }

  public requestTicket(id: number) {
    return this.requestPut({ orderId: id }, `${this.baseAPI}/receive-hard-ticket?orderId=${id}`);
  }

  public changeReferralCode(body: any) {
    return this.requestPut(body, `${this.baseAPI}/change-referral-code`);
  }

  public findSaleTicketOrderTransaction(page: Page): Observable<any> {
    let url_ = this.baseAPI + '-payment/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('orderId', this.selectedOrderId);
    return this.requestGet(url_);
  }

  public crudSaleTicketOrderTransaction(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}-payment/update`);
    }
    return this.requestPost(body, `${this.baseAPI}-payment/add`);
  }

  public saleTicketOrderTransactionDetail(id: number) {
    let url_ = `${this.baseAPI}-payment/find-by-id?`;
    url_ += this.convertParamUrl('id', id);
    return this.requestGet(url_);
  }

  public deleteSaleTicketOrderTransaction(id: number) {
    return this.requestDelete(`${this.baseAPI}-payment/delete/${id}`);
  }

  public cancelSaleTicketOrderTransaction(id: number) {
    return this.requestPut(
      { orderPaymentId: id },
      `${this.baseAPI}-payment/approve/${id}?status=${SaleTicketOrder.HUY_THANH_TOAN}`
    );
  }

  public approveSaleTicketOrderTransaction(id: number) {
    return this.requestPut(
      { orderPaymentId: id },
      `${this.baseAPI}-payment/approve/${id}?status=${SaleTicketOrder.DA_THANH_TOAN}`
    );
  }

  public notiSaleTicketOrderTransaction(id: number) {
    return this.requestPost({ orderPaymentId: id }, `${this.baseAPI}-payment/send-notify/${id}`);
  }

  public getListBankAccount() {
    this.requestGet(`${this.baseAPI}-payment/find-list-bank?orderId=` + this.selectedOrderId).subscribe((res: any) => {
      if (res && res.data && res.data && res.data.length) {
        const listBankAccount: IDropdown[] = res.data.map(
          (item: any) =>
            ({
              value: item.businessCustomerBankAccId,
              label: `${item.bankAccName} - ${item.bankName}`,
            } as IDropdown)
        );
        this._listBankAccount.next(listBankAccount);
      }
    });
  }

  public getDataSaleOrderDetailList(page: Page): Observable<any> {
    let url_ = this.baseAPI + '/find-order-ticket-info?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('orderId', this.selectedOrderId);
    return this.requestGet(url_);
  }

  public findAllTransactionProcessing(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all-processing?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.event && filter?.event.length) url_ += this.convertParamTypeArrayUrl('eventId', filter.event);
    if (filter?.source && filter?.source.length) url_ += this.convertParamTypeArrayUrl('orderer', filter.source);
    return this.requestGet(url_);
  }

  public findAllValidSaleTicket(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all-valid-orders?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.status && filter?.status.length) url_ += this.convertParamTypeArrayUrl('isLock', filter.status);
    if (filter?.event && filter?.event.length) url_ += this.convertParamTypeArrayUrl('eventId', filter.event);
    if (filter?.source && filter?.source.length) url_ += this.convertParamTypeArrayUrl('orderer', filter.source);
    return this.requestGet(url_);
  }

  public lockValidSaleTicket(body: any) {
    return this.requestPut(body, `${this.baseAPI}/lock`);
  }

  public unlockValidSaleTicket(id: number) {
    return this.requestPut({ id }, `${this.baseAPI}/unlock-order/${id}`);
  }

  public findAllParticipationManagement(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all-order-ticket?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter?.event && filter?.event.length) url_ += this.convertParamTypeArrayUrl('eventIds', filter.event);
    if (filter?.status && filter?.status.length) url_ += this.convertParamTypeArrayUrl('status', filter.status);
    if (filter?.checkinType) url_ += this.convertParamUrl('type', filter.checkinType);
    return this.requestGet(url_);
  }

  public participateParticipationManagement(id: number) {
    return this.requestPut({ orderTickId: id }, `${this.baseAPI}/ticket-confirm-participation?orderTickId=${id}`);
  }

  public lockTicket(body: any) {
    return this.requestPut(body, `${this.baseAPI}/ticket-lock`);
  }

  public unlockTicket(id: number) {
    return this.requestPut({ orderTickId: id }, `${this.baseAPI}/ticket-unlock?orderTickId=${id}`);
  }

  public getSaleTicketOrderDetailHistory(page: Page): Observable<any> {
    let url_ = this.baseAPI + '/find-all-history?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('UpdateTable', SaleTicketOrder.UpdateTable);
    url_ += this.convertParamUrl('RealTableId', this.selectedOrderId);
    return this.requestGet(url_);
  }

  public viewFilePDF(url: string): Observable<any> {
    return this.viewFilePDFFromAPI(url);
  }

  public findByTradingBankId(id: any): Observable<any> {
    return this.requestGet(`/api/core/trading-provider/msb-prefix-account/find-by-trading-bank-id/${id}`);
  }

  public notiMSB(body: any): Observable<any> {
    return this.requestPost(body, '/api/payment/msb/receive-notification');
  }

  public exportFile(url: string) {
    return this.requestDownloadFile(`/${url}&download=true`);
  }
}
