import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ISelectButton } from '@shared/interface/InterfaceConst.interface';

@Component({
  selector: 'form-select-button',
  templateUrl: './form-select-button.component.html',
  styleUrls: ['./form-select-button.component.scss'],
})
export class FormSelectButtonComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public options: ISelectButton[] = [];
  @Input()
  public classSelectButton: string = '';
  @Input()
  public ngModelValue: string;
  @Input()
  public isDisabled: boolean = false;
  @Output()
  public ngModelValueChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public handleChange(event: any) {
    if (event) {
      this.ngModelValueChange.emit(event.value);
      this._onChange.emit(event);
    }
  }
}
