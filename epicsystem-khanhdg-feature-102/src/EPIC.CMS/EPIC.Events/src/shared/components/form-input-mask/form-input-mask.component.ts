import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-input-mask',
  templateUrl: './form-input-mask.component.html',
  styleUrls: ['./form-input-mask.component.scss'],
})
export class FormInputMaskComponent implements OnInit {
  @Input()
  public floatLabel: boolean = false;
  @Input()
  public classContainer: string = '';
  @Input()
  public showIconSearch: boolean = true;
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
  @Input()
  public inputMask: string = '';
  @Input()
  public showDescription: boolean = false;
  @Input()
  public description: string = '';
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();

  constructor() {}

  ngOnInit() {}

  public handleInput(event: any) {
    this.ngModelValueChange.emit(event);
  }
}
