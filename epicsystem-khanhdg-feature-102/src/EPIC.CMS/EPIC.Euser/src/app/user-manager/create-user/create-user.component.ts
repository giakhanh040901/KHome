import { PartnerServiceProxy } from '@shared/service-proxies/partner-service';
import { Component, Injector, OnInit } from '@angular/core';
import { MessageErrorConst, UserConst, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { UserRoleService } from '@shared/service-proxies/role-service';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})

// EPIC ROOT, ĐỐI TÁC ROOT, ĐẠI LÝ ROOT TẠO TÀI KHOẢN

export class CreateUserComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private userService: UserServiceProxy,
    private _userRoleService: UserRoleService,
    private _partnerService: PartnerServiceProxy,
    private ref: DynamicDialogRef, 
    private configDialog: DynamicDialogConfig,
  ) {
    super(injector, messageService);
  }

  user: any = {};

  userLogin: any = {};

  tradingProviders: any[] = [];
  partners: any[] = [];

  investor: any[] = [];

  investorName: string = '';

  UserTypes= UserTypes;
  MessageErrorConst = MessageErrorConst;
  submitted = false;

  isPartner = true;

  isUpdate = false;

  ngOnInit() {
    //
    this.user = this.configDialog.data.user;
    this.isUpdate = this.configDialog.data.isUpdate;
    if(this.user.userId) {
      this.user.userName = this.user?.userInfo?.userName;
      this.user.displayName = this.user?.userInfo?.displayName;
      this.user.password = this.user?.userInfo?.password;
    }

    this.userLogin = this.configDialog.data.userLogin;
    //

    let selfApiGetData: Observable<any>;
    // GET DANH SÁCH ĐỐI TÁC
    if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
      selfApiGetData = this._partnerService.getPartners();
    } else
    // GET DANH SÁCH ĐẠI LÝ
    if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
      selfApiGetData = this._userRoleService.getAllTradingProvider();
    }
    //
    if(selfApiGetData) {
      selfApiGetData.subscribe(res => {
        this.isLoading = false;
        if(this.handleResponseInterceptor(res, '')) {
          // DATA DANH SÁCH ĐỐI TÁC 
          if(UserTypes.TYPE_EPICS.includes(this.userLogin.user_type)) {
            this.partners = res.data?.items;
          } else
          // DATA DANH SÁCH ĐẠI LÝ
          if(UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)) {
            this.tradingProviders = res.data?.items;
            this.tradingProviders = this.tradingProviders.map(item => {
              item.name = item.aliasName ?? item.name;
              return item
            });
          } 
        }
      }, (err) => {
        this.messageError('Không lấy được dữ liệu', '');
        this.isLoading = false;
      });
    }
    //
  }

  cancel() {
    this.ref.destroy();
  }

  searchInvestor(keyword) {
    this._userRoleService.getInvestor(keyword).subscribe(res => {
      if (this.handleResponseInterceptor(res, '')) {
        if(res?.data?.items?.length) {
          this.investor = res?.data?.items.map(item => {
            item.labelName = item?.defaultIdentification?.fullname + ' - ' + item?.phone;
            return item;
          });
        } else {
          this.investor = [];
          this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1800 });
        }
      }
    }, (err) => {
      this.messageService.add({ severity: 'error', summary: '', detail: 'Có lỗi xảy ra vui lòng thử lại sau!', life: 1800 });
    });
  }

  onSelect(event) {
      this.user.investorId = event?.investorId;
  }
  
  randomPassword(checked) {
    if(checked) {
      this.user.password = Math.random().toString(36).slice(-6);
    }
  }
 
  save() {
    if(this.validForm())	{
      this.submitted = true;
      //
      if (this.user.userId && !this.isUpdate) {
        let body = {
          userId: this.user?.userId,
          newPassword: this.user.password,
          isTempPassword: this.user.changePasswordLogin ? UserConst.STATUS_YES : UserConst.STATUS_NO,
        }

        this.userService.resetPassword(body).subscribe(
          (response) => {
            if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
              this.ref.close()
              this.submitted = false;
            } else {
              this.submitted = false;
            }
          }, () => {
            this.submitted = false;
          }
        );
      } else if (this.user.userId && this.isUpdate){
        let body = {
          userId: this.user?.userId,
          displayName : this.user.displayName
        }
        this.userService.changeDisplayName(body).subscribe(
          (response) => {
            if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
              this.ref.close()
              this.submitted = false;
            } else {
              this.submitted = false;
            }
          }, () => {
            this.submitted = false;
          }
        );
      } else {
        this.user.isTempPassword = this.user.changePasswordLogin ? UserConst.STATUS_YES : UserConst.STATUS_NO;
        this._userRoleService.createUser(this.user).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
              this.ref.close(response)
            }
            this.submitted = false;
          }, (err) => {
            this.submitted = false;
          }
        );
      }
    } else	{
			this.messageError(MessageErrorConst.message.Validate);
		}
   
  }

  // validatePassword(): boolean {
  //   return this.user?.password?.trim() && this.user?.confirmPassword?.trim() && this.user?.confirmPassword?.trim() != this.user?.password?.trim();
  // }

  // Reset đại lý khi chọn từ loại tài khoản đại lý sang đối tác
  changeTypeAccount(e) {
      this.user.tradingProviderId = null;
      this.user.partnerId = null;
  }

  validForm(): boolean {
    const validIfCreate = this.user?.userName?.trim() 
                          && this.user?.displayName?.trim() 
                          && this.user?.investorId 
                          && this.user?.password?.trim()
                          // VALIDFORM EPIC
                          && ((this.userLogin.user_type == UserTypes.EPIC_ROOT && ((this.user?.partnerId && this.isPartner) || !this.isPartner)) || this.userLogin.user_type != UserTypes.EPIC_ROOT) 
                          && ((this.userLogin.user_type == UserTypes.EPIC && this.user?.partnerId) || this.userLogin.user_type != UserTypes.EPIC)
                          // VALID FORM PARTNER 
                          && ((this.userLogin.user_type == UserTypes.PARTNER_ROOT && ((this.user?.tradingProviderId && !this.isPartner) || this.isPartner)) || this.userLogin.user_type != UserTypes.PARTNER_ROOT) 
                          && ((this.userLogin.user_type == UserTypes.PARTNER && this.user?.tradingProviderId) || this.userLogin.user_type != UserTypes.PARTNER); 
    // 
    const validIfUpdate = this.user?.password?.trim() || this.isUpdate; 

    return this.user.userId > 0 ? validIfUpdate : validIfCreate;
  }
}
