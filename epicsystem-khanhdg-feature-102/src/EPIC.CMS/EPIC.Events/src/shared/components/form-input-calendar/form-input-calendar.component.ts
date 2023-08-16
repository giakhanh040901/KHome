import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import * as moment from 'moment';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-input-calendar',
  templateUrl: './form-input-calendar.component.html',
  styleUrls: ['./form-input-calendar.component.scss'],
})
export class FormInputCalendarComponent implements OnInit {
  @Input()
  public floatLabel: boolean = false;
  @Input()
  public classContainer: string = '';
  @Input()
  public showIcon: boolean = true;
  @Input()
  public showTime: boolean = false;
  @Input()
  public placeholder: string = '';
  @Input()
  public ngModelValue: any = '';
  @Input()
  public showLabel: boolean = true;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Input()
  public widthInput: string = '100%';
  @Input()
  public readonlyInput: boolean = false;
  @Input()
  public showButtonBar: boolean = false;
  @Input()
  public timeOnly: boolean = false;
  @Input()
  public minDate: Date | undefined = undefined;
  @Input()
  public maxDate: Date | undefined = undefined;
  @Input()
  public label: string = '';
  private subjectChangeInput: Subject<any> = new Subject();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<any> = new EventEmitter<any>();
  constructor() {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.subjectChangeInput.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((responseDate) => {
      if (responseDate) {
        const dateValue = (responseDate instanceof Date) ? responseDate : new Date(responseDate);
        this._onChange.emit(dateValue);
      }
    });
  }

  public handleInput(event: any) {
    const formatDate = this.validFormatDate(event.target.value);
    if (event && formatDate) {
      let dateValue = (event.target.value ? new Date(moment(event.target.value).format(formatDate)) : '') || this.ngModelValue;
      dateValue = (dateValue != 'Invalid Date') ? dateValue : this.ngModelValue;
      this.ngModelValueChange.emit(dateValue);
      this.subjectChangeInput.next(dateValue);
    }
  }

  validFormatDate(value: string): string {
    const listFormat = [
      'DD/MM/YYYY',
      'DD/MM/YYYY HH:mm',
      'DD/MM/YYYY HH:mm:ss',
    ];
    //
    for(let format of listFormat) {
      if(moment(value, format, true).isValid()) {
        return format;
      }
    }
    return "";
  }

  public handleSelect(event: any) {
    if (event) {
      this.ngModelValueChange.emit(new Date(event));
      this._onChange.emit(new Date(event));
    }
  }

  public handleClearClick(event: any) {
    if (event) {
      this.ngModelValueChange.emit(undefined);
      this._onChange.emit(undefined);
    }
  }
}
