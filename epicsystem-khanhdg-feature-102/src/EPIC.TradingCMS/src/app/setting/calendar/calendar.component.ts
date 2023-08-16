import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { YesNoConst } from '@shared/AppConsts';
import { CalendarServiceProxy } from '@shared/service-proxies/setting-service';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateCalendarComponent } from './create-or-update-calendar/create-or-update-calendar.component';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _calendarService: CalendarServiceProxy,
  ) {
    super(injector);
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
  year = new Date().getFullYear();
  calendar: any = [];

  YesNoConst = YesNoConst;

  ngOnInit(): void {
    this.init();
  }

  init() {
    // this.getCalendar();
    this.getListYear();
    this.getAllList();
  }

  getCalendar(year?: number) {
    //
    this.months = [];
    for(let i=1; i<=12; i++){
      this.months.push({
        'value': i,
        'data': []
      })
    }
    //
    year = year ?? new Date().getFullYear();
    let startDay = year + '-01-01';
    let date = new Date(startDay);
    while (date.getFullYear() === year) {
      const month = date.getMonth() + 1;
      this.months[month - 1].data.push({
        day: moment(date).format('DD'),
        datetime: moment(date).format('DD-MM-YYYY'),
        dayDate: date.getDay(),
        isDayOff: (date.getDay() == 0 || date.getDay() == 6) ? 'Y' : 'N',
      });
      date = new Date(moment(date).add(1, 'days').format('YYYY/MM/DD'));
    }
    console.log(this.months);
  }

  getAllList() {
    this.isLoading = true;
    this._calendarService.getAll(this.year).subscribe((res) => {
      if(res.data.length  > 0) {
        this.months = [];
        for(let i=1; i<=12; i++) {
          this.months.push({
            'value': i,
            'data': []
          });
        }
        this.calendar = res.data;
        console.log(this.months);
        for(let item of this.calendar) {
          const month = new Date(item.workingDate).getMonth() + 1;
          this.months[month - 1].data.push({
            day: moment(item.workingDate).format('DD'),
            datetime: moment(item.workingDate).format('DD-MM-YYYY'),
            dayDate: new Date(item.workingDate).getDay(),
            workingDate: item.workingDate,
            isDayOff: item.isDayOff,
          });
        }
      } else {
        this._calendarService.create({workingYear: this.year}).subscribe((response)=>{
          if(this.handleResponseInterceptor(response, '')) {
            this.init();
          }
        });
      }
      this.isLoading = false;
      console.log({ calendar: this.months });
    },() => {
      this.isLoading = false;
    });
  }

  getListYear() {
    const year = new Date().getFullYear();
    for(let i = (year-10); i<= (year+5); i++){
      this.years.push(i);
    }
  }

  changeYear() {
    this.getAllList();
  }

  create() {
    this.showCreateEditDialog();
  }

  edit(workingDate) {
    this.showEditEditDialog(workingDate);
  }

  private showCreateEditDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
        CreateOrUpdateCalendarComponent,
        {
            class: 'modal-sm',
            initialState: {
                title: 'Thêm ngày nghỉ lễ'
            },
        }
    );

    dialog.content.onSave.subscribe(() => {
        this.init();
    });
  }

  private showEditEditDialog(workingDate): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateCalendarComponent,
      {
          class: 'modal-sm',
          initialState: {
              calendar: {
                  workingDate: workingDate,
              },

              title: 'Cập nhật ngày làm việc'
          },
      }
    );
    dialog.content.onSave.subscribe(() => {
        this.init();
    });
  }

  next() {
    if(this.year < this.years[this.years.length -1]) {
      this.year = this.year + 1;
      this.getAllList();
    }
  }
  refresh() {
    this.year = new Date().getFullYear();
    this.getAllList();
  }
  previous() {
    if(this.year > this.years[0]) {
      this.year = this.year - 1;
      this.getAllList();
    }
  }
}
