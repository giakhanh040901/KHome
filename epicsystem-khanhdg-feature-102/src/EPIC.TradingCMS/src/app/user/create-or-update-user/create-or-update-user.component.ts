import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AbpValidationError } from '@shared/components/validation/abp-validation.api';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-user',
  templateUrl: './create-or-update-user.component.html',
  styleUrls: ['./create-or-update-user.component.scss']
})
export class CreateOrUpdateUserComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    public _userService: UserServiceProxy,
  ) {
    super(injector);
  }

  @Output() onSave = new EventEmitter<any>();

  title: string;
  user: any = {
    userId: -1,
    displayName: "",
    email: "",
    userName: "",
    password: ""
  }
  rows: any = [];

  passwordValidationErrors: Partial<AbpValidationError>[] = [
    {
        name: 'pattern',
        immediateMessage: 'Mật khẩu có ít nhất 6 kí tự và không chứa khoảng trắng'
    },
  ];
  confirmPasswordValidationErrors: Partial<AbpValidationError>[] = [
      {
          name: 'validateEqual',
          immediateMessage: 'Mật khẩu nhập lại không khớp'
      },
  ];

  ngOnInit(): void {
    if(this.user.userId !== -1) {
      this.isLoading = true;
      this._userService.getById(this.user.userId).subscribe((res) => {
        if(this.handleResponseInterceptor(res, '')) {
          this.user = res.data;
        }
        this.isLoading = false;
      }, (err) => {
        this.notify.error('Không thể lấy dữ liệu');
        this.isLoading = false;
        console.log({ err: err });
      });
    }
  }

  save() {
    this.saving = true;
    if (this.user.userId == -1) {
      this.user.userType = 'I';
      this._userService.create(this.user).subscribe(
          (response) => {
              if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
                this.bsModalRef.hide();
                this.onSave.emit();
              } else {
                this.saving = false;
              }
          },() => {
            this.saving = false;
          }
      );
    } else {
      this._userService.update(this.user,this.user.userId).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
              this.bsModalRef.hide();
              this.onSave.emit();
            } else {
              this.saving = false;
            }
          },() => {
            this.saving = false;
          }
      );
    }
  }
}
