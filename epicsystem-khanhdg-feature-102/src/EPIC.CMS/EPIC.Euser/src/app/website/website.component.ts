import WebConfig from '@shared/PermissionWebConfig';
import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { CrudComponentBase } from "@shared/crud-component-base";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { Router } from '@angular/router';
import { UserRoleService } from '@shared/service-proxies/role-service';
import { UserTypes } from '@shared/AppConsts';

@Component({
  selector: 'app-website',
  templateUrl: './website.component.html',
  styleUrls: ['./website.component.scss']
})

// DANH SÁCH PHÂN QUYỀN WEBSITE THEO VAI TRÒ CHỈ HIỂN THỊ VỚI TK EPIC_ROOT, PARTNER_ROOT, TRADING_PROVIDER_ROOT

export class WebsiteComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Danh sách website" },
    ]);

    this.webList = Object.keys(WebConfig).map((key) => {
      return { ...WebConfig[key], key: key };
    });

    this.userLogin = this.getUser();
  }

  websites: any[] = [];
  listAction: any[] = [];

  webList: any [] = [];

  userLogin:any = {};

  ngOnInit(): void {
    this.getInit();
  }

  getInit() {
    // EPIC ROOOT
    if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
      this.websites = [...this.webList];
      this.genListAction(this.websites);
    } 
    else {
    let selfApiGet: any;
    // PARTNER ROOT
    if(this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
      selfApiGet = this._userRoleService.getWebListOfPartner(this.userLogin?.partner_id);
    }
    // TRADING PROVIDER ROOT
    if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      selfApiGet = this._userRoleService.getWebListOfTradingProvider(this.userLogin?.trading_provider_id);
    }
    //
    this.isLoading = true;
    selfApiGet.subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        let permissionWebKeys = [];
        permissionWebKeys = res.data.map(p => p.permissionKey);
        this.websites = this.webList.filter(w => permissionWebKeys.includes(w.key));
        this.genListAction(this.websites);
      }
    }, (err) => {
      this.isLoading = false;
    });
    }
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const actions = [];
      if(this.isGranted([this.PermissionUserConst.UserPhanQuyen_Website_CapNhatVaiTro]) || this.userLogin.user_type == UserTypes.EPIC_ROOT) {
        actions.push({
          data: item,
          label: "Cấu hình vai trò",
          icon: "pi pi-cog",
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      return actions;
    });
  }

  detail(website) {
    this.router.navigate(['/permission/web-list/detail/' + website.webKey]);
  }

}
