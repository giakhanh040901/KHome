import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from "@angular/core";
import * as moment from 'moment';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { Page } from './model/page';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppConsts, PermissionCompanyShareConst } from './AppConsts';
import { PermissionsService } from './services/permissions.service';
import {SimpleCrypt} from "ngx-simple-crypt";

/**
 * Component base cho tất cả app
 */
export abstract class CrudComponentBase extends AppComponentBase{

    permissionName: PermissionCompanyShareConst;
    private permissions: PermissionsService;

    constructor(
        injector: Injector,
        messageService: MessageService,
        ) {
        super(injector, messageService);
        this.permissions = injector.get(PermissionsService);
        this.permissionName = new PermissionCompanyShareConst();

        this.permissions.getAllPermission();
    } 

    AppConsts = AppConsts;
    simpleCrypt = new SimpleCrypt();


	subject = {
		keyword: new Subject(),
	};

    PermissionCompanyShareConst = PermissionCompanyShareConst;

    configDialogServiceRAC = {
        header: 'Trình duyệt',
        width: '600px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
        styleClass:'p-dialog-custom',
        baseZIndex: 10000,
        data: {
            title: '51gF3',
            showApproveBy: true,
        },
    };

    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: '10rem',
        minHeight: '10rem',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '0',
        translate: 'yes',
        enableToolbar: true,
        showToolbar: true,
        placeholder: 'Enter text here...',
        defaultParagraphSeparator: '',
        defaultFontName: '',
        defaultFontSize: '',
        fonts: [
          {class: 'arial', name: 'Arial'},
          {class: 'times-new-roman', name: 'Times New Roman'},
          {class: 'calibri', name: 'Calibri'},
          {class: 'comic-sans-ms', name: 'Comic Sans MS'}
        ],
        customClasses: [],
      sanitize: false,
      toolbarPosition: 'top',
      toolbarHiddenButtons: [
        [
          'undo',
          'redo',
          'strikeThrough',
          'subscript',
          'superscript',
          'heading',
          'fontName'
        ],
        [
          'backgroundColor',
          'customClasses',
          'link',
          'unlink',
          'insertImage',
          'insertVideo',
          'removeFormat',
          'toggleEditorMode'
        ]
      ]
    };

	keyword = '';
    status = null;

	page = new Page();
	offset = 0;

    isLoading = false;
    isLoadingPage = false;

    submitted = false;

    activeIndex = 0;

    frozenRight: boolean = false;

    blockText: RegExp = /[0-9,.]/;

    isGranted(permissionNames = []): boolean {
        return true;
        return this.permissions.isGrantedRoot(permissionNames);

    }


	changeKeyword() {
		this.subject.keyword.next();
	}

     getConfigDialogServiceRAC(title:string, id: number, summary = null) {
        return {
            header: title,
            width: '600px',
            contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
            styleClass:'p-dialog-custom',
            baseZIndex: 10000,
            data: {
                id: id,
                summary: summary,
            },
        };
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
    
    phoneNumber(event): boolean {
        const charCode = event.which ? event.which : event.keyCode;
        const charCodeException = [43, 40, 41];
        if (!charCodeException.includes(charCode) && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    fixFrozenRight() {
        setTimeout(() => {
            this.frozenRight = true;
        }, 0);
    }

    numberOnly(event, exeption = []): boolean {
        const charCode = event.which ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57) && !exeption.includes(charCode)) {
            return false;
        }
        return true;
    }

    protected getKeyFirstNameError(response) {
        if (!response.data) return '';
        return this.convertLowerCase(Object.keys(response.data)[0]);
    }

    callTriggerFiledError(response, fieldErrors) {
		let errField = this.getKeyFirstNameError(response);
    	if(fieldErrors[errField] !== undefined) {
            fieldErrors[errField] = true;
            console.log(fieldErrors[errField], errField);
    	}
	}

    disableKeypress(event) {
        const charCode = event.which ? event.which : event.keyCode;
        if (charCode > 1) {
            return false;
        }
        return true;
    }

    formatDate(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY') : '';
    }

    formatDateYMD(value) {
        return (moment(value).isValid() && value) ? moment(value).format('YYYY/MM/DD') : '';
    }

    formatDateTime(value: string, ...args: any[]) {
        return moment(value).isValid() ? moment(value).format('DD/MM/YYYY HH:mm') : '';
    }

    formatDateTimeView(value) {
        return (moment(value).isValid() && value) ? moment(value).format('YYYY-MM-DDTHH:mm:ss') : '';
    }
    
    protected convertLowerCase(string: string = '') {
        if (string.length > 0) {
            return string.charAt(0).toLocaleLowerCase() + string.slice(1);
        }
        return '';
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

    protected setTimeZone(datetime: string) {
        // let dateTime = new Date(datetime);
        let miliSeconds = new Date(datetime).getTime() + 7 * 3600 * 1000; // UTC+7:00
        return new Date(new Date(miliSeconds).toISOString());
        // return new Date(Date.UTC(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate()));
    }

    protected resetTimeZone(datetime: string) {
        let miliSeconds = new Date(datetime).getTime() - 7 * 3600 * 1000; // UTC+7:00
        console.log(new Date(new Date(miliSeconds).toISOString()));
        
        return new Date(new Date(miliSeconds).toISOString());
    }

    cryptEncode(id): string {
        return this.simpleCrypt.encode(AppConsts.keyCrypt,'' + id);
    }

    cryptDecode(codeId) {
        return this.simpleCrypt.decode(AppConsts.keyCrypt, codeId);
    }

}
