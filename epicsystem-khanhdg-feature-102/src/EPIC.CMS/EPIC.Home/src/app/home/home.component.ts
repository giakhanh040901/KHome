import { AppConsts, PermissionWebConst, StatusResponseConst, UserConst } from './../../shared/AppConsts';
import { Component, OnInit, OnDestroy, Inject, ViewChild } from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { forkJoin, Subscription } from 'rxjs';
import { BreadcrumbService } from '../layout/breadcrumb/breadcrumb.service';
import { TokenService } from '@shared/services/token.service';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';
import { SimpleCrypt } from 'ngx-simple-crypt';
import * as moment from 'moment';
import { DialogService } from 'primeng/dynamicdialog';
import { UploadImageComponent } from '../components/upload-image/upload-image.component';
import { MessageService } from 'primeng/api';
import { AvatarService } from '@shared/services/avatar.service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { Image } from 'primeng/image';
import { ChangeAvatarComponent } from '../components/change-avatar/change-avatar.component';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
    providers: []
})
export class HomeComponent implements OnInit, OnDestroy {

    constructor(
        private breadcrumbService: BreadcrumbService,
        private _appSessionService: AppSessionService,
        private authService: AppAuthService,
        private _tokenService: TokenService,
        private _userService: UserServiceProxy,
        private route: Router,
        public dialogService: DialogService,
		private _avatarService: AvatarService,
        protected messageService: MessageService,
        @Inject(API_BASE_URL) baseUrl?: string,

    ) {
        this.userInfo = this._appSessionService.user;
        this.hostName = window.location.hostname;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";

    }
    private baseUrl: string;
    simpleCrypt = new SimpleCrypt();

    userInfo: any = {};
    userSubscription: Subscription;

    AppConsts = AppConsts;
    PermissionWebConst = PermissionWebConst;

    redirectUrlEbond: string;
    subscription: Subscription;

    isLoading = false;

    permissions: any[] = [];
    hostName: string;
    productHost = 'home.epicpartner.vn';

    privacyInfo: {
        avatarImageUrl?: string;
        lastLogin: string;
        lastDevice: string;
        expirePass: string;
    };

    actions: any[] = [];
    avatar: string;

    ngOnDestroy() {
        this.userSubscription.unsubscribe();
    }

    ngOnInit() {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        this.privacyInfo = {
            avatarImageUrl: '',
            lastLogin: this.formatDateTime(new Date()),
            lastDevice: '',
            expirePass: 'Vô hạn',
        };
        this.setPage();
        this.genAction();
    }

    setPage(){
        this.isLoading = true;
        this.userSubscription = this._appSessionService.getUserObs().subscribe(user => {
            if (user) {
                forkJoin([this._userService.getAllPermission(), this._userService.getById(user.userId), this._userService.getPrivacyInfo()]).subscribe(([resPermission, resUser, resPrivacyInfo]) => {
                    this.isLoading = false;
                    if (resUser?.data.isTempPassword == UserConst.STATUS_YES) {
                        this.route.navigate(['/account/change-password-temp']);
                    }
                    //
                    if (resPermission?.data) {
                        this.permissions = resPermission.data;
                        this.getLinkProduct();
                    }
                    if (resPrivacyInfo?.data) {
                        resPrivacyInfo?.data.lastLogin && (this.privacyInfo.lastLogin = this.formatDateTime(resPrivacyInfo?.data.lastLogin));
                        resPrivacyInfo?.data.lastDevice && (this.privacyInfo.lastDevice = resPrivacyInfo?.data.lastDevice);
                        resPrivacyInfo?.data?.avatarImageUrl && (this.privacyInfo.avatarImageUrl = resPrivacyInfo?.data?.avatarImageUrl);
                    }
                    this.avatar = this.privacyInfo?.avatarImageUrl ? `${this.baseUrl}/${this.privacyInfo.avatarImageUrl}` : AppConsts.defaultAvatar;
                });
            }
        });
    }

    changeAvatar(){
        const ref = this.dialogService.open(ChangeAvatarComponent, {
            header: 'Đổi ảnh đại diện',
            width: '500px',
        });
        ref.onClose.subscribe(result => {
            if (result){
                window.location.reload();
            }
        })
    }

    handleResponseInterceptor(response, message: string): boolean {
        if (response.status == StatusResponseConst.RESPONSE_TRUE) {
            if (message) {
				this.messageService.add({
					severity: 'success',
					summary: '',
					detail: message,
					life: 800,
				});
            }
            return true;
        } else {
            let dataMessage = response.data;
            if (dataMessage) {
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: dataMessage[Object.keys(dataMessage)[0]],
					life: 3000,
				});
            } else {
                let message = response.message;
                if(response.code == 101 || response.code == 204) message = "Có lỗi xảy ra vui lòng thử lại sau!";
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: message,
					life: 3000,
				});
                console.log('error-------:', response.message);
            }
            return false;
        }
    }

    genAction(){
        this.actions.push({
            label: 'Xem ảnh',
            icon: 'pi pi-eye',
            command: ($event) => {
              this.viewAvatar($event.item.data);
            }
        })

        this.actions.push({
            label: 'Đổi ảnh đại diện',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.changeAvatar();
            }
        })
    }

    @ViewChild('avatarView') avatarView: Image;
    src: string
    viewAvatar(event){
        this.src = this.avatar;
        // this.avatarView.previewClick = true;        
        this.avatarView.onImageClick();        
    }

    isGranted(permissionName: string) {
        return this.permissions.includes(permissionName);
    }

    navigate(router) {
        switch (router) {
            case this.AppConsts.core:
                window.open(this.AppConsts.redirectUrlEcore, "_blank");
                break;
            case this.AppConsts.invest:
                window.open(this.AppConsts.redirectUrlEinvest, "_blank");
                break;
            case this.AppConsts.realEState:
                window.open(this.AppConsts.redirectUrlRealEState, "_blank");
                break;
            case this.AppConsts.garner:
                window.open(this.AppConsts.redirectUrlEgarner, "_blank");
                break;
            case this.AppConsts.bond:
                window.open(this.AppConsts.redirectUrlEbond, "_blank");
                break;
            case this.AppConsts.support:
                window.open(this.AppConsts.redirectUrlEsupport, "_blank");
                break;
            case this.AppConsts.user:
                window.open(this.AppConsts.redirectUrlEuser, "_blank");
                break;
            case this.AppConsts.loyalty:
                window.open(this.AppConsts.redirectUrlEloyalty, "_blank");
                break;
            case this.AppConsts.events:
                window.open(this.AppConsts.redirectUrlEvents, "_blank");
                break;
            default:
                break;
        }
    }

    getLinkProduct() {
        // set đường dẫn kèm token cho các trang chuyển hướng
        const accessToken = this._tokenService.getToken();

        const refreshToken = this._tokenService.getRefreshToken() || null;
        const accessTokenHandle = encodeURIComponent(this.simpleCrypt.encode("accessTokenEncode", accessToken));

        AppConsts.redirectUrlEcore = AppConsts.baseUrlEcore + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEuser = AppConsts.baseUrlEuser + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        //
        AppConsts.redirectUrlEbond = AppConsts.baseUrlEbond + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEinvest = AppConsts.baseUrlEinvest + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEgarner = AppConsts.baseUrlEgarner + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlRealEState = AppConsts.baseUrlRealEState + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEloyalty = AppConsts.baseUrlEloyalty + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEvents = AppConsts.baseUrlEvents + '/login/url/' + accessTokenHandle + '/' + refreshToken;
        AppConsts.redirectUrlEsupport = AppConsts.rocketchat.iframeSrc;
        
        this.userInfo = this._appSessionService.user;
    }

    logout() {
        this.authService.logout();
    }

    private formatDateTime(value: any) {
        return moment(value).isValid() && value ? moment(value).format('DD/MM/YYYY HH:mm') : ''
    }
}
