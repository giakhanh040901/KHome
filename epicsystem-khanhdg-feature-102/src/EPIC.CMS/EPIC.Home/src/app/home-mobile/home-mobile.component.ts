  import { AppConsts, PermissionWebConst, UserConst } from './../../shared/AppConsts';
  import { Component, OnInit } from '@angular/core';
  import { AppAuthService } from '@shared/auth/app-auth.service';
  import { AppSessionService } from '@shared/session/app-session.service';
  import { forkJoin, Subscription } from 'rxjs';
  import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
  import { TokenService } from '@shared/services/token.service';
  import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
  import { Router } from '@angular/router';
  import { SimpleCrypt } from 'ngx-simple-crypt';

  @Component({
    selector: 'app-home-mobile',
    templateUrl: './home-mobile.component.html',
    styleUrls: ['./home-mobile.component.scss']
  })
  export class HomeMobileComponent implements OnInit {

    constructor(
      private breadcrumbService: BreadcrumbService,
      private _appSessionService: AppSessionService,
      private authService: AppAuthService,
      private _tokenService: TokenService,
      private _userService: UserServiceProxy,
      private route: Router,
    ) {
      // this.breadcrumbService.setItems([
      //     {label: 'Dashboard', routerLink: ['/']}
      // ]);
      this.userInfo = this._appSessionService.user;
  }

  simpleCrypt = new SimpleCrypt();

  userInfo: any = {};
  AppConsts = AppConsts;
  PermissionWebConst = PermissionWebConst;

  redirectUrlEbond: string;
  subscription: Subscription;

  isLoading = false;

  permissions: any[] = [];

  ngOnInit() {
    this.isLoading = true;
    forkJoin([this._userService.getAllPermission(), this._userService.getById(this.userInfo.userId)]).subscribe(([resPermission, resUser]) => {
        this.isLoading = false;
        if (resUser?.data.isTempPassword == UserConst.STATUS_YES) {
            this.route.navigate(['/account/change-password-temp']);
        }

        if (resPermission?.data) {
            this.permissions = resPermission.data;
            this.getLinkProduct();
        }
    });
  }

  isGranted(permissionName: string) {
      return this.permissions.includes(permissionName);
  }

  getLinkProduct() {
      this.userInfo = this._appSessionService.user;
      console.log('userInfo', this.userInfo);
  }

  logout() {
      this.authService.logout();
  }

  ngOnDestroy() {
      // this.subscription.unsubscribe();
  }
}
