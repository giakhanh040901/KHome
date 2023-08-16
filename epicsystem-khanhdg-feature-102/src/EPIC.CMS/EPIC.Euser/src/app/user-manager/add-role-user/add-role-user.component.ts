import { Component, Injector, OnInit } from "@angular/core";
import { CrudComponentBase } from "@shared/crud-component-base";
import { MessageService } from "primeng/api";
import { DynamicDialogRef, DynamicDialogConfig } from "primeng/dynamicdialog";
import { forkJoin, Observable } from "rxjs";
import { UserRoleService } from "@shared/service-proxies/role-service";
import WebConfig from '@shared/PermissionWebConfig';
import { UserTypes } from "@shared/AppConsts";

@Component({
  selector: 'app-add-role-user',
  templateUrl: './add-role-user.component.html',
  styleUrls: ['./add-role-user.component.scss']
})
export class AddRoleUserComponent extends CrudComponentBase {

  constructor(
		injector: Injector,
		messageService: MessageService,
    public ref: DynamicDialogRef,
    public configDialog: DynamicDialogConfig,
    private _userRoleService: UserRoleService,
	) {
		super(injector, messageService);

    this.userLogin = this.getUser();

    this.webConfig = Object.keys(WebConfig).map((key) => {
      return { ...WebConfig[key], key: key };
    });
	}

  user: any  = {};

  userLogin: any  = {};

  roleSelected: any[] = [];

  roleUser = {
    userId: 0,
    roleIds: [],
  }

  webConfig: any = [];

  roles: any[] = [];
  websites: any[] = [];


  ngOnInit(): void {
    this.user = this.configDialog.data?.user?.userInfo ?? this.configDialog.data?.user;
    this.roleUser.userId = this.configDialog.data?.user?.userId;
    this.getRoles();
  }

  getRoles() {
    this.isLoading = true;
    let selfApiGets: Observable<any>[] = [];
    // XỬ LÝ TẦNG EPIC
    if(this.userLogin.user_type == UserTypes.EPIC_ROOT) {
      forkJoin([
        this._userRoleService.getAllRoleEpic(), 
        this._userRoleService.getRoleUser(this.roleUser.userId)
      ]).subscribe(([resRoles, resRoleUserIds]) => {
        this.isLoading = false;
        //
        this.roles = resRoles?.data;
        this.websites = [...this.webConfig];
        //
        if(resRoleUserIds.data) this.roleSelected = resRoles.data.filter(d => resRoleUserIds.data.includes(d.id));
        //
        }, (err) => {
          this.messageError('Không lấy được dữ liệu');
          this.isLoading = false;
        }
      );
    }
    // API GET DATA TẦNG ĐỐI TÁC
    if(this.userLogin.user_type == UserTypes.PARTNER_ROOT) { 
      selfApiGets = [
        this._userRoleService.getAllRole(), 
        this._userRoleService.getRoleUser(this.roleUser.userId),
        this._userRoleService.getWebListOfPartner(this.userLogin?.partner_id)
      ];
    } else
    // API GET DATA TẦNG ĐẠI LÝ 
    if(this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      selfApiGets = [
        this._userRoleService.getAllRoleTradingProvider(), 
        this._userRoleService.getRoleUser(this.roleUser.userId),
        this._userRoleService.getWebListOfTradingProvider(this.userLogin?.trading_provider_id)
      ];
    }
    // XỬ LÝ CHUNG TẦNG ĐỐI TÁC VÀ ĐẠI LÝ
    if(this.userLogin.user_type == UserTypes.PARTNER_ROOT || this.userLogin.user_type == UserTypes.TRADING_PROVIDER_ROOT) {
      forkJoin(selfApiGets).subscribe(([resRoles, resRoleUserIds, resWebs]) => {
          this.isLoading = false;
          //
          this.roles = resRoles.data;
          let webKeyIsset = resWebs.data.map(r => r.permissionKey);
          this.websites = this.webConfig.filter(w => webKeyIsset.includes(w.key));
          //
          if(resRoleUserIds.data) this.roleSelected = resRoles.data.filter(d => resRoleUserIds.data.includes(d.id));
          // log
      }, (err) => {
        this.messageError('Không lấy được dữ liệu');
        this.isLoading = false;
      });
    } 
  }

	close() {
		this.ref.close();
	}

  getRoleWeb(webKey) {
    return this.roles.filter(r => r.permissionInWeb == webKey);
  }

  save() {
    this.submitted = true;
    // Xử lý dữ liệu push
    this.roleUser.roleIds = this.roleSelected.map(r => r.id);
    //
    this._userRoleService.addRoleUser(this.roleUser).subscribe((res) => {
      this.submitted = false;
      if(this.handleResponseInterceptor(res,'')) {
        this.ref.close(res);
      }
    }, (err) => {
      this.messageError('Cập nhật thất bại, vui lòng thử lại sau!');
    });
  }

}
