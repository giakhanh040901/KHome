import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Injectable()
export class RouterService {
  private _previousUrl: string = '/home';
  private currentUrl: string = '/home';

  constructor(public router: Router, public routeActive: ActivatedRoute) {
    this.init();
  }

  private init() {
    // get previos router
    this.router.events.pipe(filter((event) => event instanceof NavigationEnd)).subscribe((event: NavigationEnd) => {
      this._previousUrl = this.currentUrl;
      this.currentUrl = event.url;
    });
    //
  }

  public get previousUrl() {
    return this._previousUrl;
  }

  public getRouterInclude(key: string): boolean {
    return this.router.url.includes(key);
  }

  public routerNavigate(urls: any[]) {
    this.router.navigate(urls);
  }
}
