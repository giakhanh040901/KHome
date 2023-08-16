import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { Page } from '@shared/model/page';
import { API_BASE_URL, ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { AppSessionService } from '@shared/session/app-session.service';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class ChangeVoucherRequestService extends ServiceProxyBase {
  private baseAPI: string = '/api/loyalty/conversion-point';
  public _listVoucherRequest: BehaviorSubject<IDropdown[] | undefined>;
  public _listVoucherRequest$: Observable<IDropdown[] | undefined>;

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    private appSessionService: AppSessionService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
    this._listVoucherRequest = new BehaviorSubject<IDropdown[] | undefined>(undefined);
    this._listVoucherRequest$ = this._listVoucherRequest.asObservable();
  }

  public getAll(page: Page, filter: any): Observable<any> {
    let url_ = this.baseAPI + '/find-all?';
    url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
    url_ += this.convertParamUrl('pageSize', page.pageSize);
    url_ += this.convertParamUrl('keyword', page.keyword);
    if (filter.status) url_ += this.convertParamUrl('status', filter.status);
    if (filter.date) url_ += this.convertParamUrl('date', filter.date);
    return this.requestGet(url_);
  }

  public changeStatus(key: string, id: number, note?: string) {
    let url_ = `${this.baseAPI}/update-status-`;
    const body: any = {
      id,
      note,
    };
    return this.requestPut(body, url_ + key);
  }

  public getDetail(id: number) {
    return this.requestGet(`${this.baseAPI}/find-by-id/${id}`);
  }

  public saveChangeRequest(body: any, isEdit: boolean) {
    if (isEdit) {
      return this.requestPut(body, `${this.baseAPI}/update`);
    }
    return this.requestPost(body, `${this.baseAPI}/add`);
  }

  public getListVoucherRequest() {
    const url_ = `/api/loyalty/voucher/get-all-for-conversion-point`;
    this.requestGet(url_).subscribe((res: any) => {
      const listVoucherRequest: IDropdown[] = res?.data?.map((item: any) => {
        return {
          value: item.voucherId,
          label: `${item.name} - SL đổi tối đa: ${item.exchangeRoundNum}`,
          rawData: {
            value: item.voucherId,
            label: item.name,
            point: item.point,
            applyQuantity: item.publishNum,
            changeQuantity: item.exchangeRoundNum,
          },
        } as IDropdown;
      });
      this._listVoucherRequest.next(listVoucherRequest);
    });
  }
}
