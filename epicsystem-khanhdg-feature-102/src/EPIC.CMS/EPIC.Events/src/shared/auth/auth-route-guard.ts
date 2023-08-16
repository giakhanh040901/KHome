import { AppConsts } from '@shared/AppConsts';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppSessionService } from '../session/app-session.service';

import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { TokenService } from '@shared/services/token.service';
import { TokenAuthServiceProxy, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { MessageService } from 'primeng/api';
import { PermissionsService } from '@shared/services/permissions.service';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {
  constructor(
    private router: Router,
    private _tokenService: TokenService,
    private _tokenAuthService: TokenAuthServiceProxy,
    private _userService: UserServiceProxy,
    private messageService: MessageService,
    private _permissionService: PermissionsService,

  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    if (!this._tokenService.getToken()) {
        this._tokenAuthService.postRefreshToken().subscribe((res) => {
            return Promise.resolve(true);
        }, (err) => {
            return Promise.resolve(false);
        });
    }

    return new Promise<boolean>((resolve) => {
        this._userService.getAllPermission().subscribe((res) => {
            let permissions = res.data || [];
            this._permissionService.setPermissions(permissions);
            if(!route?.data?.['permissions']) return resolve(true);
            //
            if(Array(route?.data?.['permissions'])) {
                for (let permission of route.data['permissions']) {
                    if (permissions.includes(permission)) {
                        return resolve(true);
                    }
                }
            }
            //
            this.messageService.add({
                severity: 'error',
                detail: 'Bạn không có quyền truy cập đường dẫn này!',
                life: 2000,
            });
            return resolve(false);
        });
    });
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return this.canActivate(route, state);
  }
}
