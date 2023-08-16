import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-checkbox',
  templateUrl: './form-checkbox.component.html',
  styleUrls: ['./form-checkbox.component.scss'],
})
export class FormCheckboxComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public classCheckbox: string = '';
  @Input()
  public ngModelValue: any = undefined;
  @Input()
  public trueValue: any = true;
  @Input()
  public falseValue: any = false;
  @Input()
  public inputId: any;
  @Input()
  public showLabel: boolean = true;
  @Input()
  public label: string = '';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Output()
  public ngModelValueChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public onChange(event: any) {
    if (event) {
      this._onChange.emit(event);
      this.ngModelValueChange.emit(this.ngModelValue);
    }
  }
}
