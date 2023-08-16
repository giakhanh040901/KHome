import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from "@angular/core";
import * as moment from 'moment';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { Page } from './model/page';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppConsts, PermissionGarnerConst, SearchConst, UserTypes } from './AppConsts';
import { PermissionsService } from './services/permissions.service';
import {SimpleCrypt} from "ngx-simple-crypt";

/**
 * Component base cho tất cả app
 */
export abstract class CrudComponentBase extends AppComponentBase{

    private permissions: PermissionsService;
    PermissionGarnerConst = PermissionGarnerConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
		
        ) {
        super(injector, messageService);

        this.permissions = injector.get(PermissionsService);

        //Khởi tạo danh sách quyền
        this.permissions.getAllPermission();
        this.isLocal = (window.location.hostname == 'localhost');
        this.userLogin = this.getUser();
    } 

    isLocal: boolean = false;

    localBaseUrl = AppConsts.appBaseUrl;

    AppConsts = AppConsts;
    SearchConst = SearchConst;
    simpleCrypt = new SimpleCrypt();
    
	subject = {
		keyword: new Subject(),
        isSetPage: new Subject()
	};

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
    isRefresh = false;

    submitted = false;

    activeIndex = 0;

    frozenRight: boolean = false;

    blockText: RegExp = /[0-9,.]/;
    isPartner: boolean = false;
    userLogin: any;
    UserTypes = UserTypes;

    sortData: any[] = [];
    isSetPage: boolean = false;
   
    /**
     * Check permission theo permission name
     * @param permissionName
     * @returns
     */
    isGranted(permissionNames = []): boolean {
        return this.permissions.isGrantedRoot(permissionNames);
    }

    getIsPartner(){
        if (this.UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
			this.isPartner = true;
		}
        return this.isPartner;
    }

	changeKeyword() {
		this.subject.keyword.next();
	}

    changeIsSetPage() {
		this.subject.isSetPage.next();
    }


    getConfigDialogServiceRAC(title:string, params: any) {
        return {
            header: title,
            width: '600px',
            contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
            styleClass:'p-dialog-custom',
            baseZIndex: 10000,
            data: {
                id: params.id,
                summary: params?.summary,
            },
        };
    }

    getConfigDialogServiceDisplayTableColumn(cols:any[], comlumnSelected:any[]) {
        return {    
            header: "Sửa cột hiển thị",
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

    formatDateMonth(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM') : '';
    }
    formatDate(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY') : '';
    }

    formatDateTime(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY HH:mm') : '';
    }

    formatDateTimeSecond(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY HH:mm:ss') : '';
    }
    
    formatDateYMD(value) {
        return (moment(value).isValid() && value) ? moment(value).format('YYYY/MM/DD') : '';
    }

    formatDateTimeView(value) {
        return (moment(value).isValid() && value) ? moment(value).format('YYYY-MM-DDTHH:mm') : '';
    }

    formatDateTimeTickTack(value) {
        return (moment(value).isValid() && value) ? moment(value).format('YYYY-MM-DDTHH:mm:ss.SSSZZ') : '';
    }

    protected convertLowerCase(string: string = '') {
        if (string.length > 0) {
            return string.charAt(0).toLocaleLowerCase() + string.slice(1);
        }
        return '';
    }
    
    // === CHUYỂN ĐỔI DỮ LIỆU TỪ CALENDAR ĐỂ LƯU VÀ DATABASE KO BỊ SAI TIMEZONE

    protected formatCalendar(fields, model) {
        for(let field of fields) {
            if(model[field]) model[field] = this.formatCalendarItem(model[field]);
        }
        return model;
    }

    protected formatCalendarItem(datetime: string) {
        return moment(new Date(datetime)).format("YYYY-MM-DDTHH:mm:ss");
    }
    // =====

    // === CHUYỂN ĐỔI DỮ LIỆU ĐỂ HIỂN THỊ ĐƯỢC TRONG CALENDAR
    protected formatCalendarDisplay(fields, model) {
        for(let field of fields) {
            if(model[field]) model[field] = this.formatCalendarDisplayItem(model[field]);
        }
        return model;
    }

    protected formatCalendarDisplayItem(datetime: string) {
        return new Date(datetime);
    }

    //===

    protected filterField(model, modelInterface) {
        for (const [key, value] of Object.entries(modelInterface)) {
            modelInterface[key] = model[key];
        }
        return modelInterface;
    }

    cryptEncode(id): string {
        return this.simpleCrypt.encode(AppConsts.keyCrypt,'' + id);
    }

    cryptDecode(codeId) {
        return this.simpleCrypt.decode(AppConsts.keyCrypt, codeId);
    }

    getTableHeight(percent = 65) {
        return (this.screenHeight*(percent/100) + 'px');     
    }

    public getLocalStorage(key: string) {
		return JSON.parse(localStorage.getItem(key))
	}

    public setLocalStorage(data: any[], key: string) {
		return localStorage.setItem(key, JSON.stringify(data));
	}

    public removeSessionStorage(key: string) {
		return sessionStorage.removeItem(key);
	}

    onSort(event: any) {
		console.log('!!! event ', event);
		if (JSON.stringify(this.sortData) === JSON.stringify(event?.multisortmeta)){
		  console.log('Trùng data');
		} else {
		  this.sortData = [];
		  event?.multisortmeta?.forEach(meta => {
                this.sortData.push({
                    field: meta.field,
                    order: meta.order
                  });
		  });
		    console.log('!!! SORT: ', this.sortData );
            this.isSetPage = true;
            this.changeIsSetPage();
		}
	}

    getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}
}
