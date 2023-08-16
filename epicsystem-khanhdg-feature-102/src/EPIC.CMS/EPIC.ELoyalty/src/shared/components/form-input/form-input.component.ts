import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-input',
  templateUrl: './form-input.component.html',
  styleUrls: ['./form-input.component.scss'],
})
export class FormInputComponent implements OnInit {
  @Input()
  public floatLabel: boolean = false;
  @Input()
  public classContainer: string = '';
  @Input()
  public showIconSearch: boolean = true;
  @Input()
  public type: string = 'text';
  @Input()
  public placeholder: string = '';
  @Input()
  public widthInput: string = '100%';
  @Input()
  public ngModelValue: string | number | undefined = '';
  @Input()
  public showLabel: boolean = true;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Input()
  public label: string = 'Tìm kiếm';
  @Input()
  public classInput: string = '';
  private subjectChangeInput: Subject<any> = new Subject();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<string | number | undefined> = new EventEmitter<
    string | number | undefined
  >();

  constructor() {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.subjectChangeInput.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe((res) => {
      if (res) {
        this._onChange.emit(res);
      }
    });
  }

  public handleInput(event: any) {
    if (event) {
      this.ngModelValueChange.emit(event.target.value);
      this.subjectChangeInput.next(event);
    }
  }
}
