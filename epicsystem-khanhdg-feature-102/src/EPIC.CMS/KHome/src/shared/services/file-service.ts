import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { AppConsts } from "@shared/AppConsts";
import { IResponseItem } from "@shared/interface/response.interface";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class FileService extends ServiceProxyBase {
	private fileEndpoint = `/api/file`;
	constructor(
		messageService: MessageService,
		_cookieService: CookieService, 
		@Inject(HttpClient) http: HttpClient, 
		@Optional() @Inject(API_BASE_URL) baseUrl?: string
	) {
		super(messageService, _cookieService, http, baseUrl);
	}
	/**
	 * UPLOAD FILE 
	 * @param body TRUYỀN JSON NHƯ BÌNH THƯỜNG. BÊN TRONG TỰ CHUYỂN JSON => FORM DATA
	 * @param folder TÊN FOLDER SẼ LƯU TRÊN BACKEND
	 * @returns 
	 */
	uploadFile(body: File, folderUpload: string): Observable<IResponseItem<string>> {
		let folderPath = `${AppConsts.folder}/${folderUpload}`;
        let url_: string = `${this.fileEndpoint}/upload?folder=${folderPath}`;
        return this.requestPostFile(body, url_);
    }
	
}