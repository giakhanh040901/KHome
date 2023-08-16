import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { TokenService } from '@shared/services/token.service';
import { AppSessionService } from '@shared/session/app-session.service';
import jwtDecode from 'jwt-decode';
import * as moment from 'moment';
import { SimpleCrypt } from 'ngx-simple-crypt';

@Component({
  selector: 'app-login-url',
  template: `
    <e-loading *ngIf="isLoading"></e-loading>
  `
})
export class LoginUrlComponent implements OnInit {

  constructor(
    private activatedRoute: ActivatedRoute,
    private _tokenService: TokenService,
    private _appSessionService: AppSessionService,
  ) {
    this.accessTokenEnCode = this.activatedRoute.snapshot.paramMap.get('accessToken');
    this.refreshToken = this.activatedRoute.snapshot.paramMap.get('refreshToken');
  }

  simpleCrypt = new SimpleCrypt();

  accessTokenEnCode: string;
  refreshToken: string;
  accessToken: string;
  isLoading = false;

  ngOnInit(): void {
    this.isLoading = true;
    if (this.accessTokenEnCode && this.refreshToken) {
      this.accessToken = this.simpleCrypt.decode("accessTokenEncode", this.accessTokenEnCode);

      const exp = jwtDecode(this.accessToken)['exp'];
      const tokenExpireDate = this.unixToDate(exp);

      this._tokenService.clearToken();
      this._tokenService.setToken(this.accessToken, tokenExpireDate);
      this._tokenService.setRefreshToken(this.refreshToken);
      this._appSessionService.init().then(
        (result) => {
          console.log(result);
        },
        (err) => {
          console.error(err);
        }
      );
      // this._router.navigate(['/']);
      let initialUrl = UrlHelper.initialUrl;
      if (initialUrl.indexOf('/login') > 0) {
        initialUrl = AppConsts.appBaseUrl;
      }

      location.href = initialUrl;
    } else {
      
    }
  }

  private unixToDate(UNIX_timestamp){
		return moment.unix(UNIX_timestamp).toDate();
	};

}
