import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'e-input-switch',
  templateUrl: './e-input-switch.component.html',
  styleUrls: ['./e-input-switch.component.scss']
})
export class EInputSwitchComponent implements OnInit {

  @Input()
  public classContainer: string = 'flex align-items-end';
  @Input()
  public ngModelValue: any = undefined;
  @Input()
  public showLabel: boolean = true;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Input()
  public label: string = '';
  @Input()
  public classLabel: string = '';
  @Input()
  public trueValue: any = true;
  @Input()
  public falseValue: any = false;
  @Output()
  public ngModelValueChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public onChange(event: any) {
    if (event) {
      this.ngModelValueChange.emit(this.ngModelValue);
    }
  }
}

