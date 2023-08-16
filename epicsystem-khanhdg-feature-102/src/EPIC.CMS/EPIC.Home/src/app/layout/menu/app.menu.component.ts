import { Component, OnInit } from '@angular/core';
import { AppMainComponent } from '../main/app.main.component';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[];

    constructor(public appMain: AppMainComponent) { }

    ngOnInit() {
        this.model = [
            { label: 'Trang chủ', icon: 'pi pi-fw pi-home', routerLink: ['/home'] },
            { label: 'Thông tin cá nhân', icon: 'pi pi-user', routerLink: ['/user'] },
            // { label: 'DashBoard', icon: 'pi pi-book', routerLink: ['/report'] },
            // { label: 'Hướng dẫn', icon: 'pi pi-book', routerLink: ['/home'] },
            // { label: 'Hỗ trợ khách hàng', icon: 'pi pi-message', routerLink: ['/support/chat'] },
        ];
    }

    onMenuClick() {
        this.appMain.menuClick = true;
    }
}
