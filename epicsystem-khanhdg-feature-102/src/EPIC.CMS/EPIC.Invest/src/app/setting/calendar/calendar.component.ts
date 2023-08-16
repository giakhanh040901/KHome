import { Component, Injector, OnInit } from '@angular/core';
import { YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CalendarServiceProxy } from '@shared/services/calender-service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
    selector: 'app-calendar',
    templateUrl: './calendar.component.html',
    styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _calendarService: CalendarServiceProxy,
        private breadcrumbService: BreadcrumbService
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Cấu hình ngày nghỉ lễ' },
        ]);
    }

    fieldErrors = {};

    expandedRows = {};

    fieldDates = ['workingDate', 'busDate'];

    modalDialog: boolean;

    deleteItemDialog: boolean = false;

    deleteItemsDialog: boolean = false;

    rows: any[] = [];

    page = new Page();
    offset = 0;

    submitted: boolean;

    cols: any[];

    statuses: any[];

    statusYesNo = YesNoConst.list;
    YesNoConst = YesNoConst;

    calendar: any = {
        workingYear: 2020,
        workingDate: null,
        busDate: null,
        isDayOff: '',
    }

    headers = [
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': 0,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': 1,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': 2,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': 3,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': 4,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': 5,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': 6,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
        {
            'vi': 'T3',
            'en': 'Tuesday',
            'stt': null,
        },
        {
            'vi': 'T4',
            'en': 'Wednesday',
            'stt': null,
        },
        {
            'vi': 'T5',
            'en': 'Thursday',
            'stt': null,
        },
        {
            'vi': 'T6',
            'en': 'Friday',
            'stt': null,
        },
        {
            'vi': 'T7',
            'en': 'Saturday',
            'stt': null,
        },
        {
            'vi': 'CN',
            'en': 'Sunday',
            'stt': null,
        },
        {
            'vi': 'T2',
            'en': 'Monday',
            'stt': null,
        },
    ];
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
        this._calendarService.getAll(this.year).subscribe((resGet) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resGet, '')) {
                if (resGet.data.length > 0) {
                    this.getFullDay(resGet);
                } else {
                    console.log('year', this.year);
                    this._calendarService.create({ workingYear: this.year }).subscribe((resCreate) => {
                        console.log(this.year);
                        if (this.handleResponseInterceptor(resCreate, '')) {
                            this._calendarService.getAll(this.year).subscribe((resGetCallback) => {
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
        } 
        console.log({ calendar: this.months });
    }


    getListYear() {
        const year = new Date().getFullYear();
        for (let i = (year - 5); i <= (year + 10); i++) {
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

    setPage(event?: Page) {
        this.isLoading = true;
        this._calendarService.getAll(this.page).subscribe((res) => {
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
        // console.log(dayInfo);
        // this.calendar.workingDate = workingDate;
        this.calendar = {
            ...calendar,
            workingDate: calendar?.workingDate ? new Date(calendar?.workingDate) : null,
            busDate: calendar?.busDate ? new Date(calendar?.busDate) : null,
            isDayOff: calendar?.isDayOff,
        };
        console.log({ calendar: this.calendar });
        this.modalDialog = true;
        // this._calendarService.get(this.calendar.workingDate).subscribe((res) => {
        //     console.log('vào đây rồi----------')
        //     this.calendar = res.data;
        //     console.log({ data: res, calendar: this.calendar });
        // });
        // this.calendar = dayInfo;
    }

    save() {
        this.submitted = true;
        // Fomat date Input
        let body = this.formatCalendar(this.fieldDates, {...this.calendar});
        console.log({ calendar: this.calendar });
        this._calendarService.update(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                console.log('--------------', this.calendar);
                this.submitted = false;
                // this.setPage({ page: this.page.pageNumber });
                this.init();
                this.hideDialog();
            } else {
                this.submitted = false;
            }
        }, () => {
            this.submitted = false;
        });
    }

    resetValid(field) {
        this.fieldErrors[field] = false;
    }
}
