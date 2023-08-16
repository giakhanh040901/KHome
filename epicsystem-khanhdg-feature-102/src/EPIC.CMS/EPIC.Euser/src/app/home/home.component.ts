import { AppConsts } from './../../shared/AppConsts';
import { Component, OnInit } from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { TokenService } from '@shared/services/token.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: []
})
export class HomeComponent implements OnInit {

    constructor(
        private breadcrumbService: BreadcrumbService,
        private _appSessionService: AppSessionService,
        private authService: AppAuthService,
        private _tokenService: TokenService,
        ) {
        // this.breadcrumbService.setItems([
        //     {label: 'Dashboard', routerLink: ['/']}
        // ]);
    }

    userInfo: any = {};
    AppConsts = AppConsts;

    redirectUrlEbond : string;
    subscription: Subscription;

    isLoadingPage = true;

    ngOnInit() {
        this.userInfo = this._appSessionService.user;
     }

     logout() {
		this.authService.logout();
	}

    ngOnDestroy() {
        // this.subscription.unsubscribe();
    }

    ngAfterViewInit() {
        this.isLoadingPage = false;
    }
}
