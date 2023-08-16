import { Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';
import { SimpleCrypt } from 'ngx-simple-crypt';
import { MessageService } from 'primeng/api';
import { Subject } from 'rxjs';
import { AppConsts, ETypeDataTable, PermissionLoyaltyConst, TypeFormatDateConst } from './AppConsts';
import { IHeaderColumn } from './interface/InterfaceConst.interface';
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
        //Khởi tạo danh sách quyền
        this.permissionService.getPermissions();
    }

    localBaseUrl = AppConsts.appBaseUrl;
    PermissionLoyaltyConst = PermissionLoyaltyConst;
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
    isGranted(permissionNames = []): boolean {
        // return true;
        return this.permissionService.isGrantedRoot(permissionNames);
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
        return JSON.parse(localStorage.getItem(key));
    }

    public setLocalStorage(data: any[], key: string) {
        return localStorage.setItem(key, JSON.stringify(data));
    }

    public getConfigDialogServiceDisplayTableColumn(cols: IHeaderColumn[], comlumnSelected: IHeaderColumn[]) {
        cols = cols.filter((col: IHeaderColumn) => col.type === ETypeDataTable.TEXT);
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
}
