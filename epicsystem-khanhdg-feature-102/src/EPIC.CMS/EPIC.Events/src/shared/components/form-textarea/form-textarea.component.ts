import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MIN_HEIGHT_RESIZE_TEXTAREA, RESIZE_TEXTAREA_TYPE } from '@shared/AppConsts';

@Component({
  selector: 'form-textarea',
  templateUrl: './form-textarea.component.html',
  styleUrls: ['./form-textarea.component.scss'],
})
export class FormTextareaComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public classLabel: string = '';
  @Input()
  public showLabel: boolean = true;
  @Input()
  public label: string = '';
  @Input()
  public rows: number = 2;
  @Input()
  public ngModelValue: string | undefined = undefined;
  @Input()
  public placeholder: string = '';
  @Input()
  public classTextarea: string = '';
  @Input()
  public isRequired: boolean = false;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public resizeTextareaType: string = RESIZE_TEXTAREA_TYPE.VERTICAL;
  @Output()
  public _onClick: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onChangeKeyup: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onChangeInput: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();

  constructor() {}

  ngOnInit() {}

  public handleClick(event: any) {
    if (event) {
      this._onClick.emit(event);
    }
  }

  public handleKeyup(event: any) {
    if (event) {
      this._onChangeKeyup.emit(event);
    }
  }

  public handleInput(event: any) {
    this.ngModelValueChange.emit(event.target.value);
    this._onChangeInput.emit(event);
  }

  public get MIN_HEIGHT_RESIZE_TEXTAREA() {
    return MIN_HEIGHT_RESIZE_TEXTAREA;
  }
}
