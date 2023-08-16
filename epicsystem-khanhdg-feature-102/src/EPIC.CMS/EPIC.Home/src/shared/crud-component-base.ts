import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from "@angular/core";
import * as moment from 'moment';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Subject } from 'rxjs';
import { Page } from './model/page';
import { AngularEditorConfig } from '@kolkov/angular-editor';

/**
 * Component base cho tất cả app
 */
export abstract class CrudComponentBase extends AppComponentBase{

    constructor(
        injector: Injector,
        messageService: MessageService,
		
        ) {
        super(injector, messageService);
    } 

	subject = {
		keyword: new Subject(),
	};

    
	keyword = '';

	page = new Page();
	offset = 0;

    isLoading = false;
    isLoadingPage = false;

    submitted = false;

    activeIndex = 0;
    
    blockText: RegExp = /[0-9,.]/;

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

    isGranted() {
        return true;
    }

    
}
