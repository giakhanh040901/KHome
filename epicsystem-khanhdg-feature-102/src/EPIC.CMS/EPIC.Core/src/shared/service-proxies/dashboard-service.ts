import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { API_BASE_URL, ServiceProxyBase } from "./service-proxies-base";
import { Page } from "@shared/model/page";
import * as moment from "moment";
import { SortConst } from "@shared/AppConsts";

@Injectable()
export class DashBoardServiceProxy extends ServiceProxyBase {
  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  public getDataOverview(filterDates: Date[] | undefined) {
    let url = "/api/core/dashboard/find-all?";
    filterDates &&
      filterDates.length &&
      ((url += this.convertParamUrl(
        "startDate",
        moment(filterDates[0]).format("YYYY-MM-DD")
      )),
      (url += this.convertParamUrl(
        "endDate",
        moment(filterDates[1]).format("YYYY-MM-DD")
      )));
    return this.requestGet(url);
  }

  public getDataTabel(page: Page, filterDates: Date[] | undefined, sortData?: any[]) {
    let url = "/api/core/dashboard/find-all-customer?";
    url += this.convertParamUrl("pageNumber", page.getPageNumber());
    url += this.convertParamUrl("pageSize", page.pageSize);
    filterDates &&
      filterDates.length &&
      ((url += this.convertParamUrl(
        "startDate",
        moment(filterDates[0]).format("YYYY-MM-DD")
      )),
      (url += this.convertParamUrl(
        "endDate",
        moment(filterDates[1]).format("YYYY-MM-DD")
      )));

      if(sortData){
        sortData.forEach(item => {
            url += this.convertParamUrl('Sort', `${item.field}-${SortConst.getValueSort(item.order)}`);
        })
      }
    return this.requestGet(url);
  }
}
