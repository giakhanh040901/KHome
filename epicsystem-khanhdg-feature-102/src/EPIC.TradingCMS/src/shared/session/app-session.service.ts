import { Injectable } from '@angular/core';
import jwt_decode from "jwt-decode";

import {
    SessionServiceProxy, UserServiceProxy,
} from '@shared/service-proxies/service-proxies';

@Injectable()
export class AppSessionService {

    private _user: any;
    private _application: any;

    constructor(
        private _userService: UserServiceProxy,
        ) {
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

    init(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            const token = abp.auth.getToken();
            if(token) {
                const userInfo: any = jwt_decode(token);
                console.log({ userInfo: userInfo });
                this._user = {
                    displayName: userInfo.DisplayName,
                    userName: userInfo.Username,
                    email: userInfo.Email,
                    userId: userInfo.sub,
                };
            }
            resolve(true);
        });
    }
}
