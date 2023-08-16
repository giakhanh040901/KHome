import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { YesNoConst } from '@shared/AppConsts';
import { CalendarServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-calendar',
  templateUrl: './create-or-update-calendar.component.html',
  styleUrls: ['./create-or-update-calendar.component.scss']
})
export class CreateOrUpdateCalendarComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    public _calendarService: CalendarServiceProxy,
  ) {
    super(injector);
  }

  @Output() onSave = new EventEmitter<any>();

  title:string;
  statusYesNo = YesNoConst.list;

  calendar: any = {
    workingYear: 2020,
    workingDate: null,
    busDate: null,
    isDayOff: '',
  }

  calendarFieldTypeDates =['workingDate', 'busDate'];
  calendarDateDisplays = {
    'workingDate': null,
    'busDate': null,
  }

  ngOnInit(): void {
    this._calendarService.get(this.calendar.workingDate).subscribe((res)=>{
      this.calendar = res.data;
      this.formatDateOutput(this.calendarFieldTypeDates, this.calendar, this.calendarDateDisplays);
      console.log({ data: res, calendar: this.calendar });
    });
  }

  changeDatePicker(field, value) {
    this.calendar[field] = value;
    console.log({ issuer: this.calendar });
  }

  save() {
    this.saving = true;
      // Fomat date Input
      this.formatDateInput(this.calendarFieldTypeDates, this.calendar);
      const data = { ...this.calendar };
      this._calendarService.update(data).subscribe((response) => {
        if(this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.bsModalRef.hide();
          this.onSave.emit();
        } else {
          this.saving = false;
        }
      },() => {
        this.saving = false;
      }
    );
  }
}
