import { Injector, ElementRef } from "@angular/core";
import {
    DataTableConst,
    StatusResponseConst,
} from "@shared/AppConsts";

import { AppSessionService } from '@shared/session/app-session.service';
import { MessageService } from 'primeng/api';
import { AppLocalStorageService } from "./services/storage.service";
import { TokenService } from "./services/token.service";
import { AppUtilsService } from './services/utils.service';
import jwt_decode from "jwt-decode";

/**
 * Component base cho tất cả app
 */
export abstract class AppComponentBase {

    appSession: AppSessionService;
    elementRef: ElementRef;
    utils: AppUtilsService;
	_storageService: AppLocalStorageService;
    datatableMessage = DataTableConst.message;
    _tokenService: TokenService;
    screenHeight: number;
    screenWidth: number;

    constructor(
        injector: Injector, 
        protected messageService: MessageService,
        ) {
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
        this.utils = injector.get(AppUtilsService);
		this._storageService = injector.get(AppLocalStorageService);
        this._tokenService = injector.get(TokenService);
        //
        this.screenHeight = window.innerHeight;
        this.screenWidth = window.innerWidth;
    }

    getUser() {
        const token = this._tokenService.getToken();
        if(token) {
            const userInfo = jwt_decode(token);
            return userInfo;
        }
        return {};
    }

    isGranted(permissionName: string): boolean {
        return false;
    }

    getBaseUrl() {
        return document.getElementsByTagName("base")[0].href;
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

	messageError(msg = '', summary = 'Lỗi') {
		this.messageService.add({
			severity: 'error',
			summary,
			detail: msg,
			life: 3000,
		});
	}

	messageSuccess(msg = '', summary = '') {
		this.messageService.add({
			severity: 'success',
			summary,
			detail: msg,
			life: 3000,
		});
	}
    
    protected getElementByXpath(path): any {
        return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
    }

    keyPressNumbers(event) {
        var charCode = (event.which) ? event.which : event.keyCode;
        // Only Numbers 0-9
        if ((charCode < 48 || charCode > 57)) {
          event.preventDefault();
          return false;
        } else {
          return true;
        }
      }
}