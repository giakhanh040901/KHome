import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import {
  API_BASE_URL,
  ServiceProxyBase,
} from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";

@Injectable()
export class ProjectSaleService extends ServiceProxyBase {
  public selectedProjectSaleId: number;

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }
}
