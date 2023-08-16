import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { CookieManagerService } from '@shared/services/cookie.service';
import { TokenService } from '@shared/services/token.service';
import { UserInfoService } from '@shared/services/user.service';
import { MessageService } from 'primeng/api';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-garner-verify-receive-contract',
  templateUrl: './garner-verify-receive-contract.component.html',
  styleUrls: ['./garner-verify-receive-contract.component.scss']
})

  export class GarnerVerifyReceiveContractComponent  extends AppComponentBase implements OnInit {
  //
  constructor(
      private injector: Injector,
      messageService: MessageService,
      public authService: AppAuthService,
      private _cookieService: CookieManagerService,
      private _userInfoService: UserInfoService,
      private http: HttpClient,
      private route: ActivatedRoute
  ) {
      super(injector, messageService);
      this.code = this.route.snapshot.paramMap.get('id');
  }

  submitting = false;
  dark: boolean;
  otp: string;
  isLoading: boolean = false;
  AppConsts = AppConsts;
  
  code: any;
  verify: any;
  activeCode = false;
  phoneNumber: string;
  tradingProviderId: any;
  isVerify = false;
  
  ngOnInit(): void {
    this.http.get<any>(`${AppConsts.remoteServiceBaseUrl}/api/garner/receive-contract/get-phone-by-delivery-code/${this.code}`).subscribe(res => {
        this.verify = res?.data;
        if(this.verify?.deliveryStatus == 3) {
          this.isVerify = true;
        }
    })
  }

  onNextInput(event, elementId){
    let element;
    if (event.code !== 'Backspace') element = event.srcElement.nextElementSibling;
    if (event.code === 'Backspace') element = event.srcElement.previousElementSibling;
    if(element == null) return;
      else
          element.focus();
 }

  getPhoneNumber() {
    this.http.post<any>(`${AppConsts.remoteServiceBaseUrl}/api/garner/receive-contract/verify-phone?deliveryCode=${this.code}&phone=${this.phoneNumber}&tradingProviderId=${this.verify?.tradingProviderId}`, { body: '' }).subscribe(res => {
      if (this.handleResponseInterceptor(res, '')) {
        this.activeCode = true;
        console.log("verify",this.verify)
      }
  })
  }

  back(){
    this.activeCode = false;
  }

  getPhoneDeliveryCode() {
    if(this.otp != null) {
      this.http.put<any>(`${AppConsts.remoteServiceBaseUrl}/api/garner/receive-contract/delivery-status-recevired/${this.code}/${this.otp}`, { body: '' }).subscribe(res => {
        console.log("otp",res.status);
        if (this.handleResponseInterceptor(res, 'Xác nhận OTP thành công')) {
          this.isVerify = true;
        }
      })
    }
             
  }

}

