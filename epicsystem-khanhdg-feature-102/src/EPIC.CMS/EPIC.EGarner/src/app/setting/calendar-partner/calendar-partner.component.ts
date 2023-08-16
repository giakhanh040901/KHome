
import { Component, Injector, OnInit } from '@angular/core';
import { CalendarConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CalendarServiceProxy } from '@shared/service-proxies/calendar-service';
@Component({
  selector: 'app-calendar-partner',
  templateUrl: './calendar-partner.component.html',
  styleUrls: ['../calendar/calendar.component.scss']
})
export class CalendarPartnerComponent extends CrudComponentBase {

    constructor(injector: Injector,
        private _calendarService: CalendarServiceProxy,
        messageService: MessageService,
        private breadcrumbService: BreadcrumbService) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Cấu hình ngày nghỉ lễ' },
        ]);
    }

    fieldErrors = {};

    fieldDates = ['workingDate', 'workingEndDate'];

    modalDialog: boolean;

    rows: any[] = [];

    page = new Page();
    offset = 0;

    submitted: boolean;

    YesNoConst = YesNoConst;

    interface = {
        workingDate: null,
        workingEndDate: null,
        isDayOff: '',
    }

    calendar: any = {...this.interface};

    headers = [...CalendarConst.HEADER];
    months = [];
    years = [];
    year: number = new Date().getFullYear();

    ngOnInit(): void {
        this.init();
    }

    setFieldError() {
        for (const [key, value] of Object.entries(this.calendar)) {
            this.fieldErrors[key] = false;
        }
        console.log({ filedError: this.fieldErrors });
    }

    init() {
        // this.getCalendar();
        this.getListYear();
        this.getAllList();
    }

    getAllList() {
        this.isLoading = true;
        this._calendarService.getAllCalendarPartner(this.year).subscribe((resGet) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resGet, '')) {
                if (resGet.data.length > 0) {
                    this.getFullDay(resGet);
                } else {
                    console.log('year', this.year);
                    this._calendarService.createCalendarPartner({ workingYear: this.year }).subscribe((resCreate) => {
                        console.log(this.year);
                        if (this.handleResponseInterceptor(resCreate, '')) {
                            this._calendarService.getAllCalendarPartner(this.year).subscribe((resGetCallback) => {
                                if (this.handleResponseInterceptor(resGetCallback, '')) {
                                    if (resGetCallback.data.length > 0) {
                                        this.getFullDay(resGetCallback);
                                    } 
                                }
                            });
                        }
                    });
                }
            }
            else {
                console.log('res cal', resGet);
            }
            console.log({ calendar: this.months });
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    getFullDay(fullDay) {
        if (fullDay.data.length > 0) {
            this.months = [];
            for (let i = 1; i <= 12; i++) {
                this.months.push({
                    'value': i,
                    'data': []
                });
            }
            this.calendar = fullDay.data;
            console.log('month', this.months);
            for (let item of this.calendar) {
                const month = new Date(item.workingDate).getMonth() + 1;
                this.months[month - 1].data.push({
                    day: moment(item.workingDate).format('DD'),
                    datetime: moment(item.workingDate).format('DD-MM-YYYY'),
                    dayDate: new Date(item.workingDate).getDay(),
                    workingDate: item.workingDate,
                    busDate: item.busDate,
                    isDayOff: item.isDayOff,
                    workingYear: item.workingYear
                });
            }
            this.months.forEach(month => {
                let temp:[{day:string}] = month?.data;
                month.data = temp.sort((a, b) => {
                    if (+a?.day < +b?.day) return -1;
                    if (+a?.day > +b?.day) return 1;
                    return 0;
                });                
                return month;
            });
        } 
        console.log({ calendar: this.months });
    }


    getListYear() {
        const year = new Date().getFullYear();
        for (let i = 2022; i <= (year + 10); i++) {
            this.years.push(i);
            console.log('i: ---------------', i)
            // this.getListYear();
        }
        console.log(year);
    }

    changeYear() {
        this.getAllList();
    }

    next() {
        if (this.year < this.years[this.years.length - 1]) {
            this.year = this.year + 1;
            this.getAllList();
        }
    }

    refresh() {
        this.year = new Date().getFullYear();
        this.getAllList();
    }

    previous() {
        if (this.year > this.years[0]) {
            this.year = this.year - 1;
            this.getAllList();
        }
    }

    changeDatePicker(field, value) {
        this.calendar[field] = value;
        console.log({ calendar: this.calendar });
    }

    setPage(pageInfo?: any) {
        this.setFieldError();
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        this.page.keyword = this.keyword;
        this.isLoading = true;

        this._calendarService.getAllCalendarPartner(this.page).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res.data.items;
                console.log({ rows: res.data.items, totalItems: res.data.totalItems });
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }


    edit(calendar) {
        this.calendar = {
            ...calendar,
            workingDate: calendar?.workingDate ? new Date(calendar?.workingDate) : null,
            workingEndDate: calendar?.workingDate ? new Date(calendar?.workingDate) : null,
            isDayOff: calendar?.isDayOff,
        };
        console.log({ calendar: this.calendar });
        this.modalDialog = true;
    }

    save() {
        this.submitted = true;
        let body = this.filterField(this.calendar, {...this.interface});
        body = this.formatCalendar(this.fieldDates, body);
        //
        this._calendarService.updateCalendarPartner(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                this.submitted = false;
                this.init();
                this.hideDialog();
            } else {
                this.submitted = false;
            }
        }, () => {
            this.submitted = false;
        });
    }
}


