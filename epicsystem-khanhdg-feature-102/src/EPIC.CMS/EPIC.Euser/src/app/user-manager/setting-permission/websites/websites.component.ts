import { Component, EventEmitter, Injector, Input, OnInit, Output } from '@angular/core';
import { UserTypes, WebKeys } from '@shared/AppConsts';
import PermissionCoreConfig from '@shared/PermissionCoreConfig';
import PermissionBondConfig from '@shared/PermissionBondConfig';
import PermissionInvestConfig from '@shared/PermissionInvestConfig';
import { MessageService, TreeNode } from 'primeng/api';
import { CrudComponentBase } from '@shared/crud-component-base';
import { UserRoleService } from '@shared/service-proxies/role-service';
import PermissionWebConfig from '@shared/PermissionWebConfig';
import { forkJoin, Observable } from 'rxjs';

@Component({
  selector: 'app-websites',
  templateUrl: './websites.component.html',
  styleUrls: ['./websites.component.scss']
})
export class WebsitesComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    //
    this.userLogin = this.getUser();
    this.webConfigs = Object.keys(PermissionWebConfig).map((key) => {
      return { ...PermissionWebConfig[key], key: key, label: PermissionWebConfig[key].name };
    });
  }

  @Input() webKey: number;
  @Input() modalDialog: number;
  @Input() params: any;

  @Output() onCloseDialog = new EventEmitter<any>();

  userLogin: any = {};

  webConfigs: any[] = [];

  // permissions: any[] = [];
  permissionWebs: any[] = [];

  selecteds: any[] = [];

  permissionPartner = {
    partnerId: null,
    permissionWebKeys: [],
  };

  permissionTradingProviderWeb = {
    tradingProviderId: null,
    permissionWebKeys: [],
  };

  permissionUser = [];
  permissionWebKeySelecteds = [];

  ngOnInit(): void {
    this.isLoading = true;
    forkJoin([
      this._userRoleService.getWebListOfPartner(this.params?.partnerId || this.userLogin?.partner_id), 
      this._userRoleService.getAllPermission()
    ]).subscribe(([res, resPermissionUser]) => {
      // XỬ LÝ TẦNG EPIC
      if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
        this.isLoading = false;
        if(this.handleResponseInterceptor(resPermissionUser, '')) {
          this.permissionUser = resPermissionUser?.data;
        }
        //
        if(this.handleResponseInterceptor(res, '')) {
          if(res?.data?.length) this.permissionWebKeySelecteds = res.data.map(item => item.permissionKey);
          this.getInit(this.permissionWebKeySelecteds); 
        }
      } else
      // XỬ LÝ TẦNG ĐỐI TÁC
      if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
        this.permissionTradingProviderWeb.tradingProviderId = this.params?.tradingProviderId;
        if(this.handleResponseInterceptor(res, '')) {
          if(res?.data?.length) {
            let permissionWebKeyMax = res.data.map(item => item.permissionKey);
            this.permissionWebs = this.webConfigs.filter(w => permissionWebKeyMax.includes(w.key));
            this.getInit(); 
          }
        }
      }
    });
  }

  hideDialog() {
    this.onCloseDialog.emit();
  }

  getInit(permissionWebKeySelecteds?: any) {
    // XỬ LÝ TẦNG EPIC
    if(permissionWebKeySelecteds) {
      this.permissionPartner.partnerId = this.params?.partnerId;
      //
      this.selecteds = [...this.webConfigs.filter(p => permissionWebKeySelecteds.includes(p.key))];
      //
      if(this.userLogin.user_type == UserTypes.EPIC) {
        // Chỉ hiển thị các web mà user đang login có quyền truy cập
        this.permissionWebs = [...this.webConfigs.filter(p => this.permissionUser.includes(p.key))];
      } else {
        this.permissionWebs = [...this.webConfigs];
      }
    }
    // XỬ LÝ TẦNG ĐỐI TÁC 
    else {
      this._userRoleService.getWebListOfTradingProvider(this.permissionTradingProviderWeb.tradingProviderId).subscribe((res) => {
        this.isLoading = false;
        if(this.handleResponseInterceptor(res, '')) {
          let permissionWebKeyIsset = res.data.map(item => item.permissionKey);
          if(res.data?.length) {
            this.selecteds = this.permissionWebs.filter(p => permissionWebKeyIsset.includes(p.key));
          }
        }
      });
    }
  }

  save() {
    this.submitted = true;
    this.isLoading = true;
    let apiCreatePermission: Observable<any>;
    // API CREATE PERMISSION (EPIC)
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      this.permissionPartner.permissionWebKeys = this.selecteds.map(item => item.key);
      apiCreatePermission = this._userRoleService.createPermissionWebPartner(this.permissionPartner);
    } else
    // API CREATE PERMISSION (PARTNER) 
    if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      this.permissionTradingProviderWeb.permissionWebKeys = this.selecteds.map(item => item.key);
      apiCreatePermission = this._userRoleService.createPermissionWebTradingProvider(this.permissionTradingProviderWeb);
    }
    //
    apiCreatePermission.subscribe((res) => {
      this.submitted = false;
      this.isLoading = false;
      //
      if (this.handleResponseInterceptor(res, 'Thêm thành công')) {
        this.onCloseDialog.emit(true);
      }
    }, (err) => {
      this.submitted = false;
      this.isLoading = false;
    });
  }

}
