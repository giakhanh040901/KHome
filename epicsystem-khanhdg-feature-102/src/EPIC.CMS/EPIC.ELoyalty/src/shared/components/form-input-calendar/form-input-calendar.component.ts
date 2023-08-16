import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
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
  public placeholder: string = 'dd/mm/yyyy';
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
  public label: string = '';
  private subjectChangeInput: Subject<any> = new Subject();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<any> = new EventEmitter<any>();
  constructor() {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.subjectChangeInput.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((res) => {
      if (res) {
        const change: any = res.target.value ? new Date(res.target.value) : '';
        this._onChange.emit(change);
      }
    });
  }

  public handleInput(event: any) {
    if (event) {
      const modelChange: any = event.target.value ? new Date(event.target.value) : '';
      this.ngModelValueChange.emit(modelChange);
      this.subjectChangeInput.next(event);
    }
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
