import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-radio-button',
  templateUrl: './form-radio-button.component.html',
  styleUrls: ['./form-radio-button.component.scss'],
})
export class FormRadioButtonComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public inputId: string = '';
  @Input()
  public name: string = '';
  @Input()
  public value: any;
  @Input()
  public ngModelValue: any = undefined;
  @Input()
  public classLabel: string = '';
  @Input()
  public label: string = '';
  @Input()
  public isShowLabel: boolean = true;
  @Input()
  public isDisabled: boolean = false;
  @Output()
  public ngModelValueChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public handleClick(event: any) {
    if (event) {
      this.ngModelValueChange.emit(this.ngModelValue);
    }
  }
}
