import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from "@angular/core";
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { Subject } from 'rxjs';
import { Page } from './model/page';
import { PermissionUserConst } from './PermissionUserConfig';
import { PermissionsService } from './services/permission.service';

/**
 * Component base cho tất cả app
 */
export abstract class CrudComponentBase extends AppComponentBase {

    permissionName: PermissionUserConst;
    private permissions: PermissionsService;
    PermissionUserConst = PermissionUserConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
        ) {
        super(injector, messageService);

        this.permissionName = new PermissionUserConst();
        this.permissions = injector.get(PermissionsService);

        //Khởi tạo danh sách quyền
        this.permissions.getAllPermission();
    } 

	subject = {
		keyword: new Subject(),
	};

    
	keyword = '';
    status = null;

	page = new Page();
	offset = 0;

    isLoading = false;
    isLoadingPage = false;

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
        return this.permissions.isGrantedRoot(permissionNames);

    }

	changeKeyword() {
		this.subject.keyword.next();
	}

    formatDate(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY') : '';
    }

    setTimeZoneList(fields, model) {
        for(let field of fields) {
            if(model[field]) model[field] = this.setTimeZone(model[field]);
        }
    }

    resetTimeZoneList(fields, model) {
        for(let field of fields) {
            if(model[field]) model[field] = this.resetTimeZone(model[field]);
        }
    }

    getConfigDialogServiceDisplayTableColumn(title:string, cols:any[], comlumnSelected:any[]){
        return {    
            header: title,
            width: '300px',
            contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
            style: {"overflow": "hidden"},
            styleClass:'dialog-setcolumn',
            baseZIndex: 10000,
            data: {
                cols: cols,
                comlumnSelected: comlumnSelected,
            },
        };        
    }

    getConfigDialogServiceRAC(title:string, data:any) {
        return {
            header: title,
            width: '600px',
            contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
            styleClass:'p-dialog-custom',
            style: {'overflow': 'hidden'},
            baseZIndex: 10000,
            data: {
                id: data.id,
                summary: data?.summary ?? null,
                actionType: data?.actionType || null,
                isInvestorProf: data?.isInvestorProf, 
            },
        };
    }

    protected convertLowerCase(string: string = '') {
        if (string.length > 0) {
            return string.charAt(0).toLocaleLowerCase() + string.slice(1);
        }
        return '';
    }

    protected getKeyFirstNameError(response) {
        if (!response.data) return '';
        return this.convertLowerCase(Object.keys(response.data)[0]);
    }

    callTriggerFiledError(response, fieldErrors) {
		let errField = this.getKeyFirstNameError(response);
    	if(fieldErrors[errField] !== undefined) {
            fieldErrors[errField] = true;
    	}
	}

    protected setTimeZone(datetime: string) {
        // let dateTime = new Date(datetime);
        let miliSeconds = new Date(datetime).getTime() + 7 * 3600 * 1000; // UTC+7:00
        return new Date(new Date(miliSeconds).toISOString());
        // return new Date(Date.UTC(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate()));
    }

    protected resetTimeZone(datetime: string) {
        let miliSeconds = new Date(datetime).getTime() - 7 * 3600 * 1000; // UTC+7:00
        return new Date(new Date(miliSeconds).toISOString());
    }

    numberOnly(event, exeption = []): boolean {
        const charCode = event.which ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && !exeption.includes(charCode)) {
            return false;
        }
        return true;
    }

    phoneNumber(event): boolean {
        const charCode = event.which ? event.which : event.keyCode;
        const charCodeException = [43, 40, 41];
        if (!charCodeException.includes(charCode) && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    removeVietnameseTones(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g,"a"); 
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g,"e"); 
        str = str.replace(/ì|í|ị|ỉ|ĩ/g,"i"); 
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g,"o"); 
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g,"u"); 
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g,"y"); 
        str = str.replace(/đ/g,"d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        // Remove extra spaces
        // Bỏ các khoảng trắng liền nhau
        str = str.replace(/ + /g," ");
        str = str.trim();
        // Remove punctuations
        // Bỏ dấu câu, kí tự đặc biệt
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g," ");
        return str;
    }

    formatDateTime(value: string, ...args: any[]) {
        return moment(value).isValid() ? moment(value).format('DD/MM/YYYY HH:mm') : '';
    } 

    getTableHeight(percent = 65) {
        return (this.screenHeight*(percent/100) + 'px');     
    }
}
