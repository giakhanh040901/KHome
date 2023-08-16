import { Component, Injector, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CrudComponentBase } from "@shared/crud-component-base";
import { UserServiceProxy } from "@shared/service-proxies/user-service";
import { MessageService } from "primeng/api";
@Component({
  selector: 'app-change-password-temp',
  templateUrl: './change-password-temp.component.html',
  styleUrls: ['./change-password-temp.component.scss']
})
export class ChangePasswordTempComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private userService: UserServiceProxy,
    private route: Router,
  ) {
    super(injector, messageService);
    this.userInfo = this.getUser();
  }
  
  userInfo: any = {};

  checkPass: boolean = true;
  check_space = true;
  check_eye: boolean;
  pass_noti: string;

  dataPassword: any = {
    oldPass: "",
    newPass: "",
    rePass: "",
    selectedValues: false,
  };

  ngOnInit(): void {}
  //
  check_terms() {
    const disabledButton = document.getElementById(
      "next_button"
    ) as HTMLInputElement;
    if (this.dataPassword.selectedValues) {
      disabledButton.disabled = false;
    } else disabledButton.disabled = true;
  }

  getValue(event) {
    const inputValue = event.target.value;
    let regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)([$@$!%*?&]*)[A-Za-z\d$@$!%*?&]{8,}$/;
    this.checkPass = regex.test(inputValue);
    if (inputValue == "") {
      this.check_space = true;
    } else {
      this.check_space = false;
    }
  }

  showPass() {
    const show_pass = document.getElementById("newPass") as HTMLInputElement;
    if (show_pass.type === "password") {
      show_pass.type = "text";
      this.check_eye = true;
    } else {
      show_pass.type = "password";
      this.check_eye = false;
    }
  }

  checkButton() {
    let isError: boolean = false;

    // if (this.dataPassword.oldPass == "") {
    //   isError = true;
    //   this.pass_noti = "Mật khẩu cũ không được để trống!";
    // }
    if (this.dataPassword.newPass == "" && !isError) {
      isError = true;
      this.pass_noti = "Mật khẩu mới không được để trống!";
    }
    if (!this.checkPass && !isError) {
      isError = true;
      this.pass_noti = "";
    }
    
    if (this.dataPassword.rePass == "" && !isError) {
      isError = true;
      this.pass_noti = "Bạn chưa nhập lại mật khẩu!";
    }
    if (this.dataPassword.newPass != this.dataPassword.rePass && !isError) {
      isError = true;
      this.pass_noti = "Mật khẩu nhập lại không trùng khớp, hãy nhập lại!";
    }
    if (!isError) {
      this.pass_noti = "";
      let body = {
        // oldPassword: this.dataPassword.oldPass,
        newPassword: this.dataPassword.newPass,
      };

      this.isLoading = true;
      this.userService.changePasswordTemp(body).subscribe(
        (response) => {
          if(this.handleResponseInterceptor(response, "Thay đổi mật khẩu thành công")) {
            setTimeout(() => {
              this.route.navigate(['/user']);
            }, 1200);
          } else {
            this.isLoading = false;
          } ;
        }, (err) => { 
          this.isLoading = false;
          this.messageService.add({ severity: "error", summary: "Thay đổi mật khẩu thất bại", detail: "", life: 2000 });
        }
      );
    }
  }

}
