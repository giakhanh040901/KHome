import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-button',
  templateUrl: './form-button.component.html',
  styleUrls: ['./form-button.component.scss'],
})
export class FormButtonComponent implements OnInit {
  @Input()
  public classButton: string = '';
  @Input()
  public styleClassButton: string = '';
  @Input()
  public label: string = '';
  @Input()
  public icon: string = '';
  @Input()
  public iconPos: string = 'left';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isTypeRipple: boolean = false;
  @Output()
  public _onClick: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public handleClick(event: any) {
    if (event && !this.isDisabled) {
      this._onClick.emit(event);
    }
  }
}
