import { Injectable } from '@angular/core';
import jwt_decode from "jwt-decode";

import {
    SessionServiceProxy, UserServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { TokenService } from '@shared/services/token.service';
import { BehaviorSubject } from 'rxjs';

/**
 * các thông tin user hiện tại
 */
@Injectable()
export class AppSessionService {

    private _user: any;
    private _application: any;
    user$ = new BehaviorSubject<any>(null);

    constructor(
        private _userService: UserServiceProxy,
        private _tokenService: TokenService) {
    }

    get getUserInfo() {
        return this.user$.asObservable();
    }

    setUserInfo(user) {
        this.user$.next(user);
    }

    get application(): any {
        return this._application;
    }

    get user(): any {
        return this._user;
    }

    get userId(): number {
        return this.user ? this.user.userId : null;
    }

    getShownLoginName(): string {
        const userName = this._user.userName;
        return userName;
    }

    /**
     * Khởi tạo
     * @returns 
     */
    init(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            try {
                const token = this._tokenService.getToken();
                if (token) {
                    const userInfo: any = jwt_decode(token);
                    this._user = {
                        displayName: userInfo.display_name,
                        userName: userInfo.username,
                        email: userInfo.email,
                        userId: userInfo.sub,
                        userType: userInfo.user_type,
                    };
                    //
                    this.setUserInfo(this._user);
                    localStorage.setItem('userInfo', JSON.stringify(this._user));
                }
                resolve(true);
            }
            catch (err) {
                reject(err);
            }
        });
    }
}
