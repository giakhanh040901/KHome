import { Injector, ElementRef } from "@angular/core";
import {
  AppPermissionNames,
  DataTableConst,
  StatusResponseConst,
} from "@shared/AppConsts";

import { AppSessionService } from '@shared/session/app-session.service';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { NotifyService } from 'abp-ng2-module';
import * as moment from 'moment';
import { AppUtilsService } from './services/utils.service';

/**
 * Component base cho tất cả app
 */
export abstract class AppComponentBase {
  appSession: AppSessionService;
  elementRef: ElementRef;
  utils: AppUtilsService;
  notify: NotifyService;
  permissionName: AppPermissionNames;
  ColumnMode = ColumnMode;
  datatableMessage = DataTableConst.message;

  constructor(injector: Injector) {
    this.appSession = injector.get(AppSessionService);
    this.elementRef = injector.get(ElementRef);
    this.utils = injector.get(AppUtilsService);
    this.notify = injector.get(NotifyService);
    this.permissionName = new AppPermissionNames();
  }

  isLoading = false;
  saving = false;

  isGranted(permissionName: string): boolean {
    return false;
  }

  phoneNumber(event): boolean {
    const charCode = event.which ? event.which : event.keyCode;
    const charCodeException = [43,40,41];
    if (!charCodeException.includes(charCode) && charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  numberOnly(event, exeption = []): boolean {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !exeption.includes(charCode)) {
      return false;
    }
    return true;
  }


  disableKeypress(event) {
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode > 1) {
      return false;
    }
    return true;
  }


  getBaseUrl() {
    return document.getElementsByTagName("base")[0].href;
  }

  handleResponseInterceptor(response, message: string): boolean {
    if (response.status == StatusResponseConst.RESPONSE_TRUE) {
      if(message) this.notify.success(message,'',{ position: 'top-end' });
      return true;
    } else {
      let dataMessage = response.data;
      if (dataMessage) {
        this.notify.error(dataMessage[Object.keys(dataMessage)[0]],'',{timer: 5000, positon: 'top'});
      } else {
        this.notify.error(response.message);
      }
      return false;
    }
  }

  protected convertLowerCase(string: string = '') {
      if(string.length > 0) {
          return string.charAt(0).toLocaleLowerCase() + string.slice(1);
      }
      return '';
  }

  protected formatDateInput(fieldsDate, data) {
      for(let field of fieldsDate) {
          if(data[field]) {
              // datetime lấy theo BsDatepicker là kiểu object đang sai timezone cần +7:00 (UTC+7:00)
              if(typeof data[field] == 'object') {
                  data[field] = this.setValueForTimeZone(data[field]);
              }
          }
      }
  }

  protected formatDateOutput(fieldDates, data, referenData) {
      for(let field of fieldDates) {
        if(data[field]) referenData[field] = moment(data[field]).format("DD-MM-YYYY");
      }
  }

  protected formatCurrencyOutput(dataDisPlay, data) {
      for(let [key, value] of Object.entries(dataDisPlay)) {
        if(data[key]) dataDisPlay[key] = this.formatCurrency(data[key]);
      }
  }

  protected formatCurrencyInput(dataDisPlay, data) {
      for(let [key, value] of Object.entries(dataDisPlay)) {
        if(value) {
          data[key] = this.formatCurrency(value, true);
        } else {
          data[key] = 0;
        }
      }
  }

  protected formatCurrency(value, reverse: boolean = false) {
    let Currency = Intl.NumberFormat('en-US');
    value = '' + value;
    if(value && +value !== 0) {
      value = value.replace(/,/g,'');
      if(reverse) return value;
      return Currency.format(+value);
    }
    return null;
  }

  protected setValueForTimeZone(datetime: string) {
      let miliSeconds = new Date(datetime).getTime() + 7*3600*1000; // UTC+7:00
      return new Date(miliSeconds).toISOString();
  }

  protected getKeyFirstNameError(response) {
      if(!response.data) return '';
      return this.convertLowerCase(Object.keys(response.data)[0]);
  }

  protected getElementByXpath(path): any {
      return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
  }

  protected fixZIndexOption() {
      setTimeout(() => {
          let elements = document.getElementsByClassName('datatable-row-right');
          for (let index = 1; index < elements.length; index++) {
              const element: any = elements[index];
              element.style.zIndex = elements.length - index;
          }
      }, 1000);
  }
}
