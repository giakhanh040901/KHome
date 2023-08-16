import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-input-number',
  templateUrl: './form-input-number.component.html',
  styleUrls: ['./form-input-number.component.scss'],
})
export class FormInputNumberComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public placeholder: string = '';
  @Input()
  public ngModelValue: number | undefined = undefined;
  @Input()
  public showLabel: boolean = true;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Input()
  public label: string = '';
  @Input()
  public classInput: string = '';
  private subjectChangeInput: Subject<any> = new Subject();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<number | undefined> = new EventEmitter<number | undefined>();

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
      this.ngModelValueChange.emit(event.value);
      this.subjectChangeInput.next(event);
    }
  }
}
