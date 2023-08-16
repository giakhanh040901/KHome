import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        //console.log('intercept', request, next);
        return next.handle(request)
                .pipe(catchError(err => {
                    if (err instanceof HttpErrorResponse) {
                        abp.notify.error('Có lỗi xảy ra. Vui lòng thử lại!');
                    }
                    console.log({ 'error_message': err });
                    throw (err);
                }
            )
        );
    }
}
