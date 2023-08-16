import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppConsts, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CookieService } from 'ngx-cookie-service';
import { MessageService } from 'primeng/api';
import { menus } from 'src/app';
import { AppMainComponent } from '../main/app.main.component';
import { PermissionsService } from '@shared/services/permissions.service';
@Component({
  selector: 'app-menu',
  templateUrl: './app.menu.component.html',
})
export class AppMenuComponent extends CrudComponentBase implements OnInit {
  protected _cookieService: CookieService;
  model: any[];
  checkPartner: any;
  AppConsts = AppConsts;
  labelCheck: string;
  routerLinkCheck: string;
  constructor(
    public appMain: AppMainComponent,
    injector: Injector,
    messageService: MessageService,
    private _permissionService: PermissionsService,
  ) {
    super(injector, messageService);
    this.userLogin = this.getUser();
  }

  userLogin: any = {};
  // _tokenService: TokenService;
  UserTypes = UserTypes;

  permissionMenus: string[] = [];

  ngOnInit() {
    // TODO: redirect to Tong quan su kien
    let menuDashboard = [
      {
          label: 'Dashboard',
          icon: 'pi pi-fw pi-home',
          routerLink: ['/dashboard'],
          isShow: true,
      },
  ];
  //
    this._permissionService.getPermissions$.subscribe((response) => {
      this.permissionMenus = response || [];
      if(response.length > 0) {
        let newMenus = this.handlePermissionMenu([...menus]);
        this.model = [...menuDashboard, ...newMenus];
      } else {
        this.model = [...menuDashboard]
      }
    });
  }
  

  handlePermissionMenu(menus) {
    menus.forEach(menuItem => {
        if (this.isPermission(menuItem?.permission)) {
            if(Array.isArray(menuItem.items)) {
              menuItem.items.forEach(menuItemChild => {
                if (!this.isPermission(menuItemChild?.permission) && menuItemChild?.permission){
                    menuItemChild.isShow = false;
                }
              })
            }
        } else if(menuItem?.permission) {
            menuItem.isShow = false;
        } 
    });
    return menus;
}


  // getUser() {
  //     const token = this._tokenService.getToken();
  //     if(token) {
  //         alert('vao dau');
  //         const userInfo = jwt_decode(token);
  //         return userInfo;
  //     }
  //     return {};
  // }

  onMenuClick() {
    this.appMain.menuClick = true;
  }

  isPermission(permission) {
    return this.permissionMenus.includes(permission)
  }
}
