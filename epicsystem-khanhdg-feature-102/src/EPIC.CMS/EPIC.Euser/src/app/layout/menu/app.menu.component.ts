import { Component, Injector, Input, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { UserTypes } from '@shared/AppConsts';
import { PermissionUserConst } from '@shared/PermissionUserConfig';
import { MessageService } from 'primeng/api';
import { AppMainComponent } from '../main/app.main.component';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent extends AppComponentBase {

    model: any[];

    @Input() permissions: any[] = [];

    constructor(
        public appMain: AppMainComponent,
        injector: Injector,
        messageService: MessageService,
    ) {
        super(injector, messageService);
        this.userLogin = this.getUser();
    }

    userLogin: any = {};

    UserTypes = UserTypes;

    ngOnInit() {

        this.model = [
            { label: 'Trang chủ', icon: 'pi pi-fw pi-home', routerLink: ['/home'], isShow: true },

            // PARTNER ROOT CẤU HÌNH VAI TRÒ CHO CÁC PARTNER THƯỜNG THEO WEBSITE
            {
                label: 'Phân quyền website', icon: 'pi pi-users', routerLink: ['/permission/web-list'], 
                isShow: UserTypes.TYPE_ROOTS.includes(this.userLogin?.user_type) && this.isPermission(PermissionUserConst.UserPhanQuyen_Websites),
            },

            // QUẢN LÝ TÀI KHOẢN, GẮN VAI TRÒ., CẤU HÌNH PERMISSION-MAX
            { label: "Quản lý tài khoản", icon: 'pi pi-users', routerLink: ['/user/list'], isShow: this.isPermission(PermissionUserConst.UserQL_TaiKhoan) },
           
        ];
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }

    isPermission(keyName) {
        return this.permissions.includes(keyName);
    }
}
