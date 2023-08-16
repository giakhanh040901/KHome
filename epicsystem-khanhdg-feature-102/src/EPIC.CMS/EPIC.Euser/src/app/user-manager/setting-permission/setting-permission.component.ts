import { PartnerServiceProxy } from '@shared/service-proxies/partner-service';
import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import WebConfig from '@shared/PermissionWebConfig';
import { UserRoleService } from '@shared/service-proxies/role-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { forkJoin, Observable } from 'rxjs';
import { UserTypes, WebKeys } from '@shared/AppConsts';
import PermissionCoreConfig from '@shared/PermissionCoreConfig';
import PermissionBondConfig from '@shared/PermissionBondConfig';
import PermissionInvestConfig from '@shared/PermissionInvestConfig';
import PermissionUserConfig from '@shared/PermissionUserConfig';
import PermissionGarnerConfig from '@shared/PermissionGarnerConfig';
import PermissionRealStateConfig from '@shared/PermissionRealStateConfig';
import PermissionLoyaltyConfig from '@shared/PermissionLoyaltyConfig';
import PermissionEventConfig from '@shared/PermissionEventConfig';

@Component({
  selector: 'app-setting-permission',
  templateUrl: './setting-permission.component.html',
  styleUrls: ['./setting-permission.component.scss']
})
export class SettingPermissionComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private router : Router,
    private _partnerService: PartnerServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Danh sách tài khoản', routerLink: ['/user/list'] },
      { label: 'Cấu hình quyền' },
    ]);
    //
    this.userLogin = this.getUser();
    // EPIC CẤU HÌNH QUYỀN MAX CHO ĐỐI TÁC ROOT
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      this.partnerId = +this.routeActive.snapshot.paramMap.get('id');
      this.paramCreatePermissionWebsite.partnerId = this.partnerId;
    } else
    // ĐỐI TÁC CẤU HÌNH QUYỀN MAX CHO ĐẠI LÝ ROOT
    if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      this.tradingProviderId = +this.routeActive.snapshot.paramMap.get('id');
      this.paramCreatePermissionWebsite.tradingProviderId = this.tradingProviderId;
    }
    
    this.webConfigs = Object.keys(WebConfig).map((key) => {
      return { ...WebConfig[key], key: key };
    });
  }

  isEdit = false;
  labelButtonEdit = "Chỉnh sửa";
  //
  listAction: any[] = [];

  webConfigs: any[] = [];

  userLogin: any ;

  permissionWebs: any[] = [];
  partnerId: number;
  partnerDetail: any = {};
  //
  tradingProviderId: number;
  tradingProviderDetail: any = {};

  paramCreatePermissionWebsite = {
    tradingProviderId: 0,
    partnerId: 0,
    permissionWeb: null,
    permissionDetails: [],
  };

  permissionUser = [];
  permissionWebKeys = [];

  // MODAL PHÂN QUYỀN WEBSITE
  modalDialog: boolean = false;
  // MODAL PHÂN QUYỀN CHI TIẾT WEBSITE
  modalDialogPermissionDetail: boolean = false;

  ngOnInit(): void {
    this.getInit();
  }

  getInit() {
    this.isLoadingPage = true;
    let apiGets: Observable<any>[] = [];
    // EPIC
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      apiGets = [ 
        this._partnerService.get(this.partnerId),
        this._userRoleService.getWebListOfPartner(this.partnerId),
        this._userRoleService.getAllPermission()
      ];
     } else
     // PARTNER
     if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      apiGets = [ 
        this._userRoleService.getTradingProvider(this.tradingProviderId),
        this._userRoleService.getWebListOfTradingProvider(this.tradingProviderId),
        this._userRoleService.getAllPermission()
      ];
     }
    //
    forkJoin(apiGets).subscribe(([resBusiness, resPermisionWeb, resPermissionUser]) => {
        this.isLoadingPage = false;
        // EPIC CẤU HÌNH PERMISSION MAX CHO ĐỐI TÁC
        if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
          if (this.handleResponseInterceptor(resBusiness, '')) {
            this.partnerDetail = resBusiness.data;
          }
          //
          if (this.handleResponseInterceptor(resPermisionWeb, '')) {
            let permissionWebKeys = resPermisionWeb.data.map(p => p.permissionKey);
            this.permissionWebs = this.webConfigs.filter(w => permissionWebKeys.includes(w.key));
          }
        } else {
          // ĐỐI TÁC CẤU HÌNH PERMISSION MAX CHO ĐẠI LÝ
          if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
            if (this.handleResponseInterceptor(resBusiness, '')) {
              this.tradingProviderDetail = resBusiness.data?.businessCustomer;
            }
            //
            if (this.handleResponseInterceptor(resPermisionWeb, '')) {
              let permissionWebKeys = resPermisionWeb.data.map(p => p.permissionKey);
              this.permissionWebs = this.webConfigs.filter(w => permissionWebKeys.includes(w.key));
              // this.genListAction(this.permissionWebs);
            }
          }
        }
        //
        if (this.handleResponseInterceptor(resPermissionUser, '')) {
          this.permissionUser = resPermissionUser?.data;
        }
      }, (err) => {
        this.isLoadingPage = false;
    });
  }

  reloadList() {
    // RELOAD LẠI DỮ LIỆU SAU KHI CÓ THAY ĐỔI VỀ QUYỀN
    this.isLoadingPage = true;
    let apiGets: Observable<any>[] = [];
    // API TẦNG EPIC
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) { 
      apiGets = [
        this._userRoleService.getWebListOfPartner(this.partnerId),
        this._userRoleService.getAllPermission()
      ];
    }
    // API TẦNG ĐỐI TÁC
    if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) { 
      apiGets = [
        this._userRoleService.getWebListOfTradingProvider(this.tradingProviderId),
        this._userRoleService.getAllPermission()
      ];
    }
    //
    forkJoin(apiGets).subscribe(([resPermisionWeb, resPermissionUser]) => {
        this.isLoadingPage = false;
        // XỬ LÝ TẦNG EPIC
        if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
          if (this.handleResponseInterceptor(resPermisionWeb, '')) {
            this.permissionWebKeys = resPermisionWeb.data.map(p => {
              if(p.permissionKey && this.permissionUser.includes(p.permissionKey)) {
                return p.permissionKey;
              }
            });
            //
            this.permissionWebs = this.webConfigs.filter(w => this.permissionWebKeys.includes(w.key));
            // this.genListAction(this.permissionWebs);
          }
        } else
        // XỬ LÝ TẦNG ĐỐI TÁC 
        if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
          if (this.handleResponseInterceptor(resPermisionWeb, '')) {
            let permissionWebKeys = resPermisionWeb.data.map(p => p.permissionKey);
            this.permissionWebs = this.webConfigs.filter(w => permissionWebKeys.includes(w.key));
            // this.genListAction(this.permissionWebs);
          }
          //
          if (this.handleResponseInterceptor(resPermissionUser, '')) {
            this.permissionUser = resPermissionUser?.data;
          }
        }
      }, (err) => {
        this.isLoadingPage = false;
    });
  }

  // genListAction(data = []) {
  //   this.listAction = data.map((item) => {
  //     const actions = [
  //       {
  //         data: item,
  //         label: "Thông tin chi tiết",
  //         icon: "pi pi-info-circle",
  //         command: ($event) => {
  //           this.detail($event.item.data);
  //         },
  //       }
  //     ];
  //     return actions;
  //   });
  //   console.log(this.listAction);
  // }

  create() {
    this.modalDialog = true;
  }

  // GẮN QUYỀN CHO PARTNER THEO WEBSITE
  createPermissionWebDetail(permissionWeb) {
    this.paramCreatePermissionWebsite.permissionDetails = this.getPermissionWebDetail(permissionWeb);
    this.paramCreatePermissionWebsite.permissionWeb = permissionWeb;
    this.modalDialogPermissionDetail = true;
  }

  // LẤY DANH SÁCH TẤT CẢ CÁC QUYỀN CỦA WEBSITE
  getPermissionWebDetail(permissionWeb) {
    let permissions = [];
    if(permissionWeb.webKey == WebKeys.Core) {
      permissions = Object.keys(PermissionCoreConfig).map((key) => {
        return { ...PermissionCoreConfig[key], key: key, label: PermissionCoreConfig[key].name };
      });
    } 
    //
    if(permissionWeb.webKey == WebKeys.Bond) {
      permissions = Object.keys(PermissionBondConfig).map((key) => {
        return { ...PermissionBondConfig[key], key: key, label: PermissionBondConfig[key].name };
      });
    } 
    //
    if(permissionWeb.webKey == WebKeys.Invest) {
      permissions = Object.keys(PermissionInvestConfig).map((key) => {
        return { ...PermissionInvestConfig[key], key: key, label: PermissionInvestConfig[key].name };
      });
    }
    //
    if(permissionWeb.webKey == WebKeys.User) {
      permissions = Object.keys(PermissionUserConfig).map((key) => {
        return { ...PermissionUserConfig[key], key: key, label: PermissionUserConfig[key].name };
      });
    }
    //
    if(permissionWeb.webKey == WebKeys.Garner) {
      permissions = Object.keys(PermissionGarnerConfig).map((key) => {
        return { ...PermissionGarnerConfig[key], key: key, label: PermissionGarnerConfig[key].name };
      });
    }
    //
    if(permissionWeb.webKey == WebKeys.RealState){
      permissions = Object.keys(PermissionRealStateConfig).map((key) => {
        return { ...PermissionRealStateConfig[key], key: key, label: PermissionRealStateConfig[key].name };
      });
    }
    //
    if(permissionWeb.webKey == WebKeys.Loyalty) {
      permissions = Object.keys(PermissionLoyaltyConfig).map((key) => {
        return { ...PermissionLoyaltyConfig[key], key: key, label: PermissionLoyaltyConfig[key].name };
      });
    }
    // 
    if(permissionWeb.webKey == WebKeys.Event) {
      permissions = Object.keys(PermissionEventConfig).map((key) => {
        return { ...PermissionEventConfig[key], key: key, label: PermissionEventConfig[key].name };
      });
    }
    // CHỈ LỌC QUYỀN KHI TÀI KHOẢN KHÁC EPIC ROOT
    if(this.userLogin.user_type != UserTypes.EPIC_ROOT) {
      permissions = permissions.filter(p => this.permissionUser.includes(p.key));
    }

    return permissions;
  } 

  hideModal(reload: boolean = false) {
    this.modalDialog = false;
    this.modalDialogPermissionDetail = false;
    if(reload) this.reloadList();
  }

  detail(website) {
    this.router.navigate(['/web-list/detail/' + website.key]);
  }
}
