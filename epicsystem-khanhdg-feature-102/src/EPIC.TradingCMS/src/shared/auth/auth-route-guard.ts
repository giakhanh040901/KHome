import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppSessionService } from '../session/app-session.service';

import {
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild
} from '@angular/router';
import { AppAuthService } from './app-auth.service';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _sessionService: AppSessionService,
  ) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this._sessionService.user || !abp.auth.getToken()) {
      this._router.navigate(['/account/login']);
      return false;
    }

    if (!route.data || !route.data['permission']) {
      return true;
    }

    this._router.navigate([this.selectBestRoute()]);
    return false;
  }

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }

  selectBestRoute(): string {
    if (!this._sessionService.user) {
      return '/account/login';
    }
    return '/home';
  }
}
