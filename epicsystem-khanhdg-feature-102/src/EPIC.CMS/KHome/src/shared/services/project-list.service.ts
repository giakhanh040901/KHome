import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { Page } from "@shared/model/page";
import {
  API_BASE_URL,
  ServiceProxyBase,
} from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class ProjectListService extends ServiceProxyBase {
  public selectedProductItemId: number;
  private readonly baseAPI = "/api/real-estate/product-item";

  constructor(
    messageService: MessageService,
    _cookieService: CookieService,
    @Inject(HttpClient) http: HttpClient,
    @Optional() @Inject(API_BASE_URL) baseUrl?: string
  ) {
    super(messageService, _cookieService, http, baseUrl);
  }

  /* #region utility */
  public getAllProductUtility(page: Page): Observable<any> {
    let url_ = this.baseAPI + "/find-by-id?";
    url_ += this.convertParamUrl("pageNumber", 1);
    url_ += this.convertParamUrl("pageSize", 25);
    url_ += this.convertParamUrl("productItemId", this.selectedProductItemId);
    return this.requestGet(url_);
  }
  /* #endregion */

  /* #region material */
  public updateProductMaterial(id: number, type: string, content: string) {
    const body = {
      id: id,
      materialContentType: type,
      materialContent: content,
    };
    return this.requestPut(body, `${this.baseAPI}/update-material-content`);
  }
  /* #endregion */

	  /* #region diagram */
		public updateProductDiagram(id: number, type: string, content: string) {
			const body = {
				id: id,
				designDiagramContentType: type,
				designDiagramContent: content,
			};
			return this.requestPut(body, `${this.baseAPI}/update-design-diagram-content`);
		}
		/* #endregion */

    // Gọi danh sách Dự án cho Trading
    public getAllProjectByTrading(): Observable<any>{
      let url_ = '/api/real-estate/project/get-all?';
      return this.requestGet(url_);
    }
}
