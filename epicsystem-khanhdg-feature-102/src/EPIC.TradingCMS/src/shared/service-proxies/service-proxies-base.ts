import { options } from './../../app/app.module';
import {
    mergeMap as _observableMergeMap,
    catchError as _observableCatch,
} from "rxjs/operators";
import {
    Observable,
    throwError as _observableThrow,
    of as _observableOf,
} from "rxjs";
import { Injectable, Inject, Optional, InjectionToken } from "@angular/core";
import {
    HttpClient,
    HttpHeaders,
    HttpResponse,
    HttpResponseBase,
} from "@angular/common/http";
import { AppConsts } from '@shared/AppConsts';

export const API_BASE_URL = new InjectionToken<string>("API_BASE_URL");

/**
 * Base cho request
 */
export class ServiceProxyBase {
    protected http: HttpClient;
    protected baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined =
        undefined;

    constructor(
        @Inject(HttpClient) http: HttpClient,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    /**
     * Kiểm tra null hoặc undefined
     * @param value
     * @returns trả về true nếu thoả mãn
     */
    protected checkNullOrUndefined(value: any, name: String = null): boolean {
        if (value === null || typeof(value) === "undefined")
            throw new Error(`The parameter ${name} cannot be null or undefined`);
        return true;
    }

    /**
     * nếu value undefined throw ra Error
     * @param name
     * @param value
     * @returns trả về dạng name=value&
     */
    protected convertParamUrl(name: string, value: number | string | boolean) {
        return name + "=" + encodeURIComponent("" + value) + "&"
    }

    /**
     * Đẩy về object
     * @param data
     * @returns
     */
    protected fromJS(data: any): object {
        data = typeof data === "object" ? data : {};
        return data;
    }

    /**
     * Get method
     * @param url
     * @returns
     */
    public requestGet(url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                Accept: "text/plain",
            }),
        };

        return this.http
            .request("get", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processResponse(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<any>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<any>>(<any>_observableThrow(response_));
                })
            );
    }

    /**
     * request post
     * @param body
     * @returns
     */
    public requestPost (body: any | undefined, url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
                Accept: "text/plain",
            }),
        };

        return this.http
            .request("post", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processResponse(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<any>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<any>>(<any>_observableThrow(response_));
                })
            );
    }

    public requestPostFormData (body: FormData, url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, "");

        return this.http
            .post(url_, body)
            .pipe(
                _observableCatch((response_: any) => {
                    abp.notify.error(`${response_.error.error.message}`);
                    return response_;
                })
            );
    }

    /**
     * put method
     * @param body object gửi đi
     * @param url dạng "/api/services/app/Role/Update"
     * @returns Observable<TDto>
     */
    public requestPut(body: any | undefined, url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, ""); //? hoặc & ở cuối sẽ được loại bỏ

        const content_ = JSON.stringify(body);

        let options_: any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json-patch+json",
                Accept: "text/plain",
            }),
        };

        return this.http
            .request("put", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processResponse(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<any>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<any>>(<any>_observableThrow(response_));
                })
            );
    }

    public requestPutFormData(body: any | undefined, url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, ""); //? hoặc & ở cuối sẽ được loại bỏ

        return this.http
            .put(url_, body)
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<any>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<any>>(<any>_observableThrow(response_));
                })
            );
    }

    /**
     * Delete method
     * @param url
     * @returns
     */
    public requestDelete(url: String): Observable<any> {
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, "");

        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({}),
        };

        return this.http
            .request("delete", url_, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processResponse(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<void>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<void>>(<any>_observableThrow(response_));
                })
            );
    }

    /**
     * request post
     * @param body
     * @returns
     */
    protected requestPostFile (file: File, url: String): Observable<any> {
        // this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
        let url_ = this.baseUrl + url;
        url_ = url_.replace(/[?&]$/, "");
        let formData:FormData = new FormData();
        formData.append("file", file, file.name);
        let options_: any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                Accept: "text/plain",
            }),
        };

        return this.http
            .post(url_, formData, options_)
            .pipe(
                _observableMergeMap((response_: any) => {
                    return this.processResponse(response_);
                })
            )
            .pipe(
                _observableCatch((response_: any) => {
                    if (response_ instanceof HttpResponseBase) {
                        try {
                            return this.processResponse(<any>response_);
                        } catch (e) {
                            return <Observable<any>>(<any>_observableThrow(e));
                        }
                    } else return <Observable<any>>(<any>_observableThrow(response_));
                })
            );
    }

    public processResponse(response: HttpResponseBase, fromJs: Function = null): Observable<any> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse
                ? response.body
                : (<any>response).error instanceof Blob
                    ? (<any>response).error
                    : undefined;

        let _headers: any = {};
        if (response.headers) {
            for (let key of response.headers.keys()) {
                _headers[key] = response.headers.get(key);
            }
        }

        if (status === 200) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    let result200: any = null;
                    let resultData200 =
                        _responseText === ""
                            ? null
                            : JSON.parse(_responseText, this.jsonParseReviver);
                    if (fromJs) {
                        result200 = fromJs(resultData200);
                    } else {
                        result200 = this.fromJS(resultData200);
                    }
                    return _observableOf(result200);
                })
            );
        } else if (status == 400) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    try {
                        const resJson = JSON.parse(_responseText);
                        console.log('_responseText -> json', resJson);
                        abp.notify.error(resJson.error.validationErrors[0].message, '', { timer: 3500 });
                    } catch {
                        console.log('_responseText', _responseText);
                    }
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(
                _observableMergeMap((_responseText) => {
                    try {
                        const resJson = JSON.parse(_responseText);
                        console.log('_responseText -> json', resJson);
                        abp.notify.error(resJson.error.message, '', { timer: 3500 });
                    } catch {
                        console.log('_responseText', _responseText);
                    }
                    return throwException(
                        "An unexpected server error occurred.",
                        status,
                        _responseText,
                        _headers
                    );
                })
            );
        }
        return _observableOf<any>(<any>null);
    }
}


export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any };
    result: any;

    constructor(
        message: string,
        status: number,
        response: string,
        headers: { [key: string]: any },
        result: any
    ) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

export function throwException(
    message: string,
    status: number,
    response: string,
    headers: { [key: string]: any },
    result?: any
): Observable<any> {
    if (result !== null && result !== undefined) return _observableThrow(result);
    else {
        return _observableThrow(
            new ApiException(message, status, response, headers, null)
        );
    }
}

export function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = (event) => {
                observer.next((<any>event.target).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}
