import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppConsts, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { menus } from 'src/app';
import { AppMainComponent } from '../main/app.main.component';
import { PermissionsService } from '@shared/services/permissions.service';
@Component({
  selector: 'app-menu',
  templateUrl: './app.menu.component.html',
})
export class AppMenuComponent extends CrudComponentBase implements OnInit {

    constructor(
        public appMain: AppMainComponent,
        injector: Injector,
        messageService: MessageService,
        private _permissionService: PermissionsService,
    ) {
        super(injector, messageService);
        this.userLogin = this.getUser();
    }

    model: any[];
    checkPartner: any;
    AppConsts = AppConsts;
    labelCheck: string;
    routerLinkCheck: string;

    permissions: any[] = [];

    userLogin: any = {};
    // _tokenService: TokenService;
    UserTypes = UserTypes;

    ngOnInit() {
        let menuDashboard = [
            {
                label: 'Dashboard',
                icon: 'pi pi-fw pi-home',
                routerLink: ['/'],
                isShow: true,
            },
        ];
        //
        this._permissionService.getPermissions$.subscribe((response) => {
            this.permissions = response;
            this.model = [];
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
            if (this.isPermission(menuItem?.key)) {
                menuItem?.items?.forEach(menuItemChild => {
                    if (!this.isPermission(menuItemChild?.key)){
                        menuItemChild.isShow = false;
                    }
                })
            } else {
                menuItem.isShow = false;
            } 
        });
        return menus;
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        return Array.isArray(this.permissions) && this.permissions.includes(keyName);
    }
}
