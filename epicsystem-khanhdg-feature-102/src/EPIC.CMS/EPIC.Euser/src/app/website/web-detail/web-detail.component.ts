import { ActiveDeactiveConst, FormNotificationConst, RoleTypeConst, UserTypes } from '@shared/AppConsts';
import { WebKeys } from './../../../shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { ActivatedRoute, Router } from '@angular/router';
import { UserRoleService } from '@shared/service-proxies/role-service';
import PermissionCoreConfig from '@shared/PermissionCoreConfig';
import PermissionBondConfig from '@shared/PermissionBondConfig';
import PermissionInvestConfig from '@shared/PermissionInvestConfig';
import PermissionUserConfig from '@shared/PermissionUserConfig';
import PermissionGarnerConfig from '@shared/PermissionGarnerConfig';
import WebConfig from '@shared/PermissionWebConfig';
import { DialogService } from 'primeng/dynamicdialog';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import PermissionRealStateConfig from '@shared/PermissionRealStateConfig';
import PermissionLoyaltyConfig from '@shared/PermissionLoyaltyConfig';
import PermissionEventConfig from '@shared/PermissionEventConfig';

@Component({
  selector: 'app-web-detail',
  templateUrl: './web-detail.component.html',
  styleUrls: ['./web-detail.component.scss']
})
export class WebDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private route: ActivatedRoute,
    private dialogService: DialogService,
    private _userRoleService: UserRoleService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Danh sách website", routerLink: ["/permission/web-list"] },
      { label: "Cấu hình vai trò" },
    ]);

    this.webKey = +this.route.snapshot.paramMap.get('key');
    this.userLogin = this.getUser();
    //
  }

  userLogin: any;
  webKey: number;
  webInfo: any;

  permissionFull: any[] = [];

  modalDialog: boolean = false;

  rows = [];

  listAction: any[] = [];
  roleInfo: any = {};
  UserTypes = UserTypes;
  ActiveDeactiveConst = ActiveDeactiveConst;
  RoleTypeConst = RoleTypeConst;
  isDefault: boolean = false;
  //
  ngOnInit(): void {
    for (const [key, value] of Object.entries(WebConfig)) {
      if(value['webKey'] == this.webKey) {
        this.webInfo = value;
      }
    }
    //----------
    this.setPage();
    this.getPermissionWeb();
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const actions = [];
      actions.push({
        data: item,
        label: "Cập nhật",
        icon: "pi pi-user-edit",
        command: ($event) => {
          this.updateRole($event.item.data);
        }
      });

      actions.push({
        data: item,
        label: item?.status == ActiveDeactiveConst.ACTIVE ? "Khóa" : "Kích hoạt",
        icon: item?.status == ActiveDeactiveConst.ACTIVE ? "pi pi-times-circle" : "pi pi-check-circle",
        command: ($event) => {
          this.changeStatus($event.item.data);
        }
      });

      actions.push({
        data: item,
        label: "Xóa",
        icon: "pi pi-trash",
        command: ($event) => {
          this.deleteRole($event.item.data);
        }
      });
      return actions;
    });
  }

  changeStatusPage() {
    this.setPage({ page: this.offset });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    // API GET DANH SÁCH VAI TRÒ THEO WEBSITE
    let selfApiGet: any;
    // EPIC ROOT
    if (this.userLogin.user_type == UserTypes.EPIC_ROOT) {
      selfApiGet = this._userRoleService.getRoleOfWebEpic(this.page, this.webKey, this.status);
    } else
    // ĐỐI TÁC ROOT
    if (this.userLogin.user_type == UserTypes.PARTNER_ROOT) {
      selfApiGet = this._userRoleService.getRoleOfWebPartner(this.page, this.webKey, this.status);
    } else
    // ĐẠI LÝ ROOT
    if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      selfApiGet = this._userRoleService.getRoleOfWebTradingProvider(this.page, this.webKey, this.status);
    } 
    //
    this.isLoading = true;
    selfApiGet.subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, "")) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        this.genListAction(this.rows);
      }
    }, () => {
      this.isLoading = false;
    });
  }

  // CREATE OR UPDATE ROLE PARTNER
  createRole() {
    this.roleInfo = {};
    this.modalDialog = true;
    this.isDefault = false;
  }

  createDefaultRole(){
    this.roleInfo = {};
    this.modalDialog = true;
    this.isDefault = true;
  }

  updateRole(role) {
    this.roleInfo = role;
    this.modalDialog = true;
    // this.isDefault = false;
  }

  // CREATE OR UPDATE ROLE PARTNER
  createRolePartner() {
    this.roleInfo = {};
    this.modalDialog = true;
  }

  updateRolePartner(role) {
    this.roleInfo = role;
    this.modalDialog = true;
  }

  // END ======

  // CREATE OR UPDATE ROLE Epic
  createRoleEpic() {
    this.roleInfo = {};
    this.modalDialog = true;
  }

  updateRoleEpic(role) {
    this.roleInfo = role;
    this.modalDialog = true;
  }
  // END ====

  changeStatus(row) {
		const ref = this.dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '400px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {	
                    title: row.status == ActiveDeactiveConst.ACTIVE ? `Bạn có chắc chắn muốn khóa vai trò ${row.name} không ?` : `Bạn có chắc chắn muốn kích hoạt vai trò ${row.name} không ?`,
                    icon: row.status == ActiveDeactiveConst.ACTIVE ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._userRoleService.changeStatusRole(row.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                });
            } 
        });
	}

  deleteRole(role){
    const ref = this.dialogService.open(
      FormNotificationComponent,
      {
          header: "Xóa vai trò phân quyền",
          width: '400px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
          styleClass: 'p-dialog-custom',
          baseZIndex: 10000,
          data: {	
              title: `Bạn có chắc chắc muốn xóa vai trò ${role?.name} ?`,
              icon:  FormNotificationConst.IMAGE_CLOSE ,
          },
      }
  );
  ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
          this._userRoleService.deleteRole(role?.id).subscribe((response) => {
              if (this.handleResponseInterceptor(response,"Xóa thành công")) {
                  this.setPage({ page: this.page.pageNumber });
              }
          }, (err) => {
              console.log('err____', err);
          });
      } 
  });
    
  }

  getPermissionWeb() {
    // --------
    if (this.webKey == WebKeys.Core) {
      this.handlePermission(PermissionCoreConfig);
    }
    //
    if (this.webKey == WebKeys.Bond) {
      this.handlePermission(PermissionBondConfig);
    }
    //
    if (this.webKey == WebKeys.Invest) {
      this.handlePermission(PermissionInvestConfig);
    }

    if (this.webKey == WebKeys.User) {
      this.handlePermission(PermissionUserConfig);
    }

    if (this.webKey == WebKeys.Garner) {
      this.handlePermission(PermissionGarnerConfig);
    }

    if (this.webKey == WebKeys.RealState){
      this.handlePermission(PermissionRealStateConfig)
    }

    if (this.webKey == WebKeys.Loyalty){
      this.handlePermission(PermissionLoyaltyConfig)
    }

    if (this.webKey == WebKeys.Event){
      this.handlePermission(PermissionEventConfig)
    }
  }

  handlePermission(PermissionConfig) {
    this.permissionFull = [];
    this.permissionFull = Object.keys(PermissionConfig).map((key) => {
      return { ...PermissionConfig[key], key: key, label: PermissionConfig[key].name };
    });
  }

  hideModal(reload: boolean = false) {
    this.modalDialog = false;
    if (reload) this.setPage();
  }
}
