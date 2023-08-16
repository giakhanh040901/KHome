import { Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SimpleCrypt } from 'ngx-simple-crypt';
import { MessageService } from 'primeng/api';
import { Subject } from 'rxjs';
import { AppConsts, PermissionEventConst } from './AppConsts';
import { IDataValidator, IHeaderColumn } from './interface/InterfaceConst.interface';
import { Page } from './model/page';
import { PermissionsService } from './services/permissions.service';

/**
 * Component base cho tất cả app
 */
export abstract class CrudComponentBase extends AppComponentBase {
  
  private permissionService: PermissionsService;

  constructor(injector: Injector, messageService: MessageService) {
    super(injector, messageService);
    this.permissionService = injector.get(PermissionsService);
    this.permissionService.getPermissions$.subscribe((permissions) => {
      this.permissions = permissions;
    });
  }

  PermissionEventConst = PermissionEventConst;
  permissions: string[] = [];

  localBaseUrl = AppConsts.appBaseUrl;

  AppConsts = AppConsts;
  simpleCrypt = new SimpleCrypt();

  subject = {
    keyword: new Subject(),
  };

  keyword = '';
  status = null;

  page = new Page();
  offset = 0;

  isLoading = false;
  isLoadingPage = false;
  isRefresh = false;

  submitted = false;

  activeIndex = 0;

  blockText: RegExp = /[0-9,.]/;

  /**
   * Check permission theo permission name
   * @param permissionName
   * @returns
   */
  isGranted(permissionKeys: string[] | string): boolean {
    if(!Array.isArray(permissionKeys)) permissionKeys = [permissionKeys];
    for (let key of permissionKeys) {
      if (this.permissions.includes(key)) {
        return true;
      }
    }
    return false;
  }

  changeKeyword() {
    this.subject.keyword.next();
  }

  phoneNumber(event): boolean {
    const charCode = event.which ? event.which : event.keyCode;
    const charCodeException = [43, 40, 41];
    if (!charCodeException.includes(charCode) && charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }

  protected setDataSendApi(model, modelInterface) {
    for (const [key, value] of Object.entries(modelInterface)) {
      modelInterface[key] = model[key];
    }
    return modelInterface;
  }

  cryptEncode(id): string {
    return this.simpleCrypt.encode(AppConsts.keyCrypt, '' + id);
  }

  cryptDecode(codeId) {
    return this.simpleCrypt.decode(AppConsts.keyCrypt, codeId);
  }

  getTableHeight(percent = 65) {
    return this.screenHeight * (percent / 100) + 'px';
  }

  public getLocalStorage(key: string) {
    let items = JSON.parse(localStorage.getItem(key));
    items =
      items && items.length
        ? items.map((e: any) => {
            e = { ...e } as IHeaderColumn;
            Object.keys(e).forEach((key: string) => {
              if (
                ['funcStyleClassStatus', 'funcLabelStatus', 'valueFormatter', 'valueGetter'].some(
                  (string) => string === key
                )
              ) {
                e[key] = eval('(' + e[key] + ')');
              }
            });
            return e;
          })
        : undefined;
    return items;
  }

  public setLocalStorage(data: any[], key: string) {
    let newData: any[] = [];
    data.forEach((e: any) => {
      const newObj = Object.assign({}, e);
      Object.keys(e).forEach((key: string) => {
        if (typeof e[key] === 'function') {
          newObj[key] = newObj[key].toString();
        }
      });
      newData.push(newObj);
    });
    return localStorage.setItem(key, JSON.stringify(newData));
  }

  public getConfigDialogServiceDisplayTableColumn(cols: IHeaderColumn[], comlumnSelected: IHeaderColumn[]) {
    return {
      header: 'Sửa cột hiển thị',
      width: '300px',
      contentStyle: { 'max-height': '600px', overflow: 'auto', 'margin-bottom': '60px' },
      style: { overflow: 'hidden' },
      styleClass: 'dialog-setcolumn',
      baseZIndex: 10000,
      data: {
        cols: cols,
        comlumnSelected: comlumnSelected,
      },
    };
  }

  public formatCurrency(value: any): string | 0 {
    return new Intl.NumberFormat('en-DE').format(value);
  }

  public messageDataValidator(dataValidator: IDataValidator[]) {
    const messageError = dataValidator.length ? dataValidator[0].message : undefined;
    messageError && this.messageError(messageError);
  }
}
