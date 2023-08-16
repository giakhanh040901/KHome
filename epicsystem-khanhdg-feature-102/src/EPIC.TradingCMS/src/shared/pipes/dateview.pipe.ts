import { Injector, Pipe, PipeTransform } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';

@Pipe({
    name: 'dateview'
})
export class DateViewPipe extends AppComponentBase implements PipeTransform {

    constructor(injector: Injector) {
        super(injector);
    }
    transform(value: string, ...args: any[]): string {

        return moment(value).isValid() ? moment(value).format('DD/MM/YYYY') : '';

    }
}
