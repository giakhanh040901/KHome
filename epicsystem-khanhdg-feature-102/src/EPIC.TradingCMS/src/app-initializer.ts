import { Injectable, Injector } from '@angular/core';
import { PlatformLocation, registerLocaleData } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { filter as _filter, merge as _merge } from 'lodash-es';
import { AppConsts } from '@shared/AppConsts';
import { AppSessionService } from '@shared/session/app-session.service';
import { environment } from './environments/environment';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Injectable({
  providedIn: 'root',
})
export class AppInitializer {
  constructor(
    private _injector: Injector,
    private _platformLocation: PlatformLocation,
    private _httpClient: HttpClient,
  ) { }

  init(): () => Promise<boolean> {
    return () => {
      abp.ui.setBusy();
      return new Promise<boolean>((resolve, reject) => {
        AppConsts.appBaseHref = this.getBaseHref();
        const appBaseUrl = this.getDocumentOrigin() + AppConsts.appBaseHref;
        abp.ui.clearBusy();
        this.getApplicationConfig(appBaseUrl, () => {
          abp.event.trigger('abp.dynamicScriptsInitialized');
          const appSessionService = this._injector.get(AppSessionService);
            appSessionService.init().then(
              (result) => {
                abp.ui.clearBusy();
                resolve(result);
              },
              (err) => {
                abp.ui.clearBusy();
                reject(err);
              }
            );
        });
      });
    };
  }

  private getBaseHref(): string {
    const baseUrl = this._platformLocation.getBaseHrefFromDOM();
    if (baseUrl) {
      return baseUrl;
    }

    return '/';
  }

  private getDocumentOrigin(): string {
    if (!document.location.origin) {
      const port = document.location.port ? ':' + document.location.port : '';
      return (
        document.location.protocol + '//' + document.location.hostname + port
      );
    }

    return document.location.origin;
  }

  private getApplicationConfig(appRootUrl: string, callback: () => void) {
    this._httpClient
      .get<any>(`${appRootUrl}assets/${environment.appConfig}`, {
        headers: {},
      })
      .subscribe((response) => {
        AppConsts.appBaseUrl = response.appBaseUrl;
        AppConsts.remoteServiceBaseUrl = response.remoteServiceBaseUrl;
        if(response.remoteServiceBaseUrlLocal) {
          AppConsts.remoteServiceBaseUrlLocal = response.remoteServiceBaseUrlLocal;
        }
        AppConsts.clientId = response.clientId;
        AppConsts.clientSecret = response.clientSecret;

        callback();
      });
  }
}
