import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { WithdrawalService } from '@shared/services/withdrawal-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-otpwithdraw',
  templateUrl: './otpwithdraw.component.html',
  styleUrls: ['./otpwithdraw.component.scss']
})
export class OTPWithdrawComponent extends CrudComponentBase {

  otp: number;
  otpLength = 6;

  requestId: number;
  interestType: string;
  constructor(
    injector: Injector,
		messageService: MessageService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    private _withdrawalService: WithdrawalService,
  ) {
    super(injector, messageService);
   }

  ngOnInit(): void {
    this.requestId = this.configDialog.data?.requestId;
    this.interestType = this.configDialog.data?.interestType;
    this.pushNotifyWarning();
  }

  changeOtp(otp) {
    this.otp = otp;
    // if(otp.toString()?.length == this.otpLength) {
    //   setTimeout(() => {
    //     this.sendOtp();
    //   }, 1000);
    // }
  }

  pushNotifyWarning() {
    this.messageWarn('Vui lòng kiểm tra mã OTP trên thiết bị của bạn!');
  }

  getOtp() {
    this.messageWarn('Mã OTP đã được gửi về thiết bị của bạn. Vui lòng kiểm tra!');
      this._withdrawalService.getOtp(this.requestId).subscribe((res) => {
        console.log('resOtp__', res);
      });
  }

  sendOtp() {
    let body = {...this.configDialog.data?.bodySendOtp, otp: this.otp}
    this.isLoading = true;
    this.submitted = true;
    this._withdrawalService.approve(body, this.interestType).subscribe((res) => {
      this.isLoading = false;
      if(this.handleResponseInterceptor(res, 'Duyệt chi thành công!')) {
        this.ref.close(true);
      } else {
        this.submitted = false;
      }
    }, (err) => {
      console.log('err__', err);
      this.isLoading = false;
      this.submitted = false;
    });
  }
}
