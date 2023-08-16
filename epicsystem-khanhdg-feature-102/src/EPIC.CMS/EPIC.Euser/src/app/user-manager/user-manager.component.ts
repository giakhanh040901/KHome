import { Router } from '@angular/router';
import { Component, Injector, OnInit } from '@angular/core';
import { ActiveDeactiveConst, FormNotificationConst, SearchConst, UserConst, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { UserRoleService } from '@shared/service-proxies/role-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateUserComponent } from './create-user/create-user.component';
// import { AddRoleUserPartnerComponent } from '../user-partner/add-role-user-partner/add-role-user-partner.component';
// import { AddRoleUserTradingProviderComponent } from '../user-trading-provider/add-role-user-trading-provider/add-role-user-trading-provider.component';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { AddRoleUserComponent } from './add-role-user/add-role-user.component';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormNotificationComponent } from '../form-notification/form-notification.component';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrls: ['./user-manager.component.scss']
})
export class UserManagerComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private _userRoleService: UserRoleService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,

  ) {
    super(injector, messageService)

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Danh sách tài khoản' },
    ]);

    this.userLogin = this.getUser();
  }

  isLoading: boolean;
  isLoadingPage: boolean;
  rows: any[] = [];
  status: any;
  listAction: any[] = [];

  UserTypes = UserTypes;
  ActiveDeactiveConst = ActiveDeactiveConst;
  userLogin: any = {};
  
  subject = {
    keyword: new Subject(),
  };

  ngOnInit(): void {
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
  }

  changeStatusPage() {
    this.setPage({ page: this.offset });
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const actions = [];
      if (UserTypes.TYPE_ROOTS.includes(item?.userInfo?.userType)) {
        if ( this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_ChiTiet]) && !UserTypes.TYPE_EPICS.includes(item?.userInfo?.userType)) {
          actions.push({
            data: item,
            label: "Thông tin chi tiết",
            icon: "pi pi-info-circle",
            command: ($event) => {
              this.detail($event.item.data);
            }
          });
        }
        
        //
        if(this.userLogin.sub != item?.userId) {
          if (item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE && this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_SetMatKhau])) {
            actions.push({
              data: item,
              label: "Set mật khẩu",
              icon: "pi pi-key",
              command: ($event) => {
                this.resetPassword($event.item.data);
              }
            });
          }
          //
          if (item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE && this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_CauHinhQuyen])) {
            actions.push({
              data: item,
              label: "Cấu hình quyền",
              icon: "pi pi-cog",
              command: ($event) => {
                this.settingPermission($event.item.data);
              }
            });
          }
          //
          
        }
      } else
      if(item?.userId != this.userLogin.sub) {
        if(item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE && this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_CapNhatVaiTro])) {
          actions.push({
            data: item,
            label: "Cập nhật vai trò",
            icon: "pi pi-pencil",
            command: ($event) => {
              this.addRole($event.item.data);
            }
          });
        }
        //
        if(item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE && this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_SetMatKhau])) {
          actions.push({
            data: item,
            label: "Set mật khẩu",
            icon: "pi pi-key",
            command: ($event) => {
              this.resetPassword($event.item.data);
            }
          });
        }
      } 

      if(item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE) {
        actions.push({
          data: item,
          label: "Sửa tên hiển thị",
          icon: "pi pi-user-edit",
          command: ($event) => {
            this.updateDisplayName($event.item.data);
          }
        });
      }

      if(item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE) {
        actions.push({
          data: item,
          label: item?.userInfo?.statusFE == ActiveDeactiveConst.ACTIVE ? "Khóa" : "Kích hoạt",
					icon: item?.userInfo?.statusFE == ActiveDeactiveConst.ACTIVE ? "pi pi-times-circle" : "pi pi-check-circle",
          command: ($event) => {
            this.changeStatus($event.item.data);
          }
        });
      }
      if (item?.userInfo?.statusFE != ActiveDeactiveConst.DELETE && this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_Xoa])) {  ///this.isGranted([this.PermissionUserConst.UserQL_TaiKhoan_Xoa])
        actions.push({ 
          data: item,
          label: "Xóa tài khoản",
          icon: "pi pi-trash",
          command: ($event) => {
            this.delete($event.item.data);
          }
        });
      }

      return actions;
    });
  }

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
                    title: row.userInfo?.statusFE == ActiveDeactiveConst.ACTIVE ? `Bạn có chắc chắn muốn khóa tài khoản ${row.userInfo.userName} không ?` : `Bạn có chắc chắn muốn kích hoạt tài khoản ${row.userInfo.userName} không ?`,
                    icon: row.userInfo?.statusFE == ActiveDeactiveConst.ACTIVE ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._userRoleService.changeStatus(row.userId).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                });
            } 
        });
	}

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._userRoleService.getAllUser(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        this.rows = this.rows.map(element => {
          if(element.userInfo.isDeleted == UserConst.STATUS_YES) {
            element.userInfo.statusFE = UserConst.STATUS_DELETE
            return element;
          } else if (element.userInfo.isDeleted == UserConst.STATUS_NO) {
            element.userInfo.statusFE = element.userInfo.status
            return element;
          }
 
        })
        this.genListAction(this.rows);
      }
    }, () => {
      this.isLoading = false;
    });
  }

  createUser() {
    const ref = this.dialogService.open(
      CreateUserComponent,
      {
        header: 'Thêm tài khoản',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        width: '450px',
        baseZIndex: 10000,
        data: {
          userLogin: this.userLogin,
          user: {
            userId: 0,
            displayName: null,
            email: null,
            userName: null,
            password: null,
            confirmPassword: null,
            tradingProviderId: null,
            partnerId: null,
            investorId: null,
            isTempPassword: false,
            sendMail: false,
          }
        }
      }
    );
    //
    ref.onClose.subscribe(res=> {
      if(res) {
        this.setPage();
      }
    });
  }

  resetPassword(user) {
    const ref = this.dialogService.open(
      CreateUserComponent,
      {
        header: 'Cập nhật mật khẩu',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        width: '500px',
        baseZIndex: 10000,
        data: {
          userLogin: this.userLogin,
          user: user,
        }
      }
    );
    //
    ref.onClose.subscribe(res => {
    });
  }

  updateDisplayName(user) {
    const ref = this.dialogService.open(
      CreateUserComponent,
      {
        header: 'Cập nhật tên hiển thị',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        width: '450px',
        baseZIndex: 10000,
        data: {
          userLogin: this.userLogin,
          user: user,
          isUpdate: true
        }
      }
    );
    //
    ref.onClose.subscribe(res=> {
        this.setPage();
    });
  }

  detail(item) {
    let routerDetail: string;
    if (item?.partnerId) {
      routerDetail = '/partner/detail/' + item.partnerId;
    } else
    if (item?.tradingProviderId) {
      routerDetail = '/trading-provider/detail/' + item.tradingProviderId;
    }
    //
    this.router.navigate([routerDetail]);
  }

  settingPermission(item) {
    let routerSetting: string;
    routerSetting = '/setting/permission-max/' + (item?.partnerId || item?.tradingProviderId);
    this.router.navigate([routerSetting]);
  }
  
  //
  delete(item) {
    const ref = this.dialogService.open(
      FormNotificationComponent,
      {
          header: "Thông báo",
          width: '400px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
          styleClass: 'p-dialog-custom',
          baseZIndex: 10000,
          data: {	
              title: `Bạn có chắc chắn muốn xóa tài khoản ${item.userInfo.userName} không ?`,
              icon:  FormNotificationConst.IMAGE_CLOSE ,
          },
      }
  );
  ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
          this._userRoleService.delete(item.userId).subscribe((response) => {
              if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                  this.setPage({ page: this.page.pageNumber });
              }
          }, (err) => {
              console.log('err____', err);
          });
      } 
  });
  }

  // Gắn vai trò cho user
  addRole(user) {
    // GẮN VAI TRÒ CHO USER EPIC
    if(UserTypes.TYPE_ROOTS.includes(this.userLogin.user_type)) {
      const ref = this.dialogService.open(
        AddRoleUserComponent,
        {
          header: 'Vai trò cho người dùng',
          width: '600px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
          baseZIndex: 10000,
          data: {
            user: user,
          }
        }
      );
      //
      ref.onClose.subscribe((res) => {
        if(res) {
          this.handleResponseInterceptor(res, 'Cập nhật thành công');
        }
      });
    } 
  }
//----
}
