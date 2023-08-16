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
  }

  pushNotifyWarning() {
    this.messageWarn('Vui lòng kiểm tra mã OTP trên thiết bị của bạn!');
  }

  getOtp() {
      let tradingBankAccId = this.configDialog.data?.bodySubmit?.tradingBankAccId;
      this._withdrawalService.getOtp(this.requestId, tradingBankAccId).subscribe((res) => {
        if(this.handleResponseInterceptor(res)) {
          this.messageWarn('Vui lòng kiểm tra mã OTP trên thiết bị của bạn!');
        }
      });
  }

  sendOtp() {
    let body = {...this.configDialog.data?.bodySubmit, otp: this.otp}
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
