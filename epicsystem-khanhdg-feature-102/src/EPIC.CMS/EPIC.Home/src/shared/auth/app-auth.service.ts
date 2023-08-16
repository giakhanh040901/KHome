import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppConsts, UserConst } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import {
    AuthenticateModel,
    AuthenticateResultModel,
    TokenAuthServiceProxy,
    UserServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { TokenService } from '@shared/services/token.service';
import { CookieManagerService } from '@shared/services/cookie.service';
import jwtDecode from 'jwt-decode';
import * as moment from 'moment';

@Injectable()
export class AppAuthService {
    authenticateModel: AuthenticateModel;
    authenticateResult: AuthenticateResultModel;
    rememberMe: boolean;

    constructor(
        private _tokenAuthService: TokenAuthServiceProxy,
        private _router: Router,
        private _tokenService: TokenService,
        private _cookieService: CookieManagerService,
        private _appSessionService: AppSessionService,
        private _userService: UserServiceProxy,
    ) {
        this.clear();
    }

    logout(reload?: boolean): void {
        this._tokenService.clearAllCookie();
        if (reload !== false) {
            let logoutPath = '/account/login';
            //
            if(this.isMobile()) {
                logoutPath = '/account/login-mobile';
            }
            //
            this._router.navigate([logoutPath]);
        }
    }

    authenticate(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => { });
    this._tokenAuthService
        .authenticate(this.authenticateModel)
        .pipe(
            finalize(() => {
                finallyCallback();
            })
        )
        .subscribe((result: AuthenticateResultModel) => {
            console.log({ resultLogin: result });
            this.processAuthenticateResult(result);
        });
    }

    private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
        this.authenticateResult = authenticateResult;
        if (authenticateResult.access_token) {
            // Successfully logged in
            this.login(
                authenticateResult.access_token,
                authenticateResult.refresh_token,
                authenticateResult.encryptedAccessToken,
                authenticateResult.expires_in,
                this.rememberMe
            );
        } else {
            // Unexpected result!
            this._router.navigate(['account/login']);
        }
    }

    private login(
        accessToken: string,
		refreshToken: string,
        encryptedAccessToken: string,
        expiresIn: number,
        rememberMe?: boolean
    ): void {
		/**
		 * exp trong token la unix timestamp
		 */
        const tokenInfo = jwtDecode(accessToken);
		const exp = tokenInfo['exp'];
		const tokenExpireDate = this.unixToDate(exp);
        this._tokenService.setToken(accessToken, tokenExpireDate);
		this._tokenService.setRefreshToken(refreshToken);
        //
        this._appSessionService.init().then((result) => {
                console.log(result);
            },(err) => {
                console.error(err);
            }
        );
        // this._router.navigate(['/']);
        let initialUrl = UrlHelper.initialUrl;
        if (initialUrl.indexOf('/login') > 0) {
            initialUrl = AppConsts.appBaseUrl;
        }

        if(this.isMobile()) {
            this._router.navigate(['/mobile/home']);
        } else {
            this._userService.getById(+tokenInfo['sub']).subscribe(res => {
            
                if(res?.data?.isTempPassword == UserConst.STATUS_YES) {
                    this._router.navigate(['/account/change-password-temp']);
                } else {
                    this._router.navigate(['/home']);
                }
            }, (err) => {
                this._router.navigate(['/home']);
            });
        }
    }

	private unixToDate(UNIX_timestamp){
		return moment.unix(UNIX_timestamp).toDate();
	};

    private clear(): void {
        this.authenticateModel = new AuthenticateModel();
        this.authenticateModel.rememberClient = false;
        this.authenticateResult = null;
        this.rememberMe = false;
    }

    isMobile() {
        return window.innerWidth <= 991;
    }
}
