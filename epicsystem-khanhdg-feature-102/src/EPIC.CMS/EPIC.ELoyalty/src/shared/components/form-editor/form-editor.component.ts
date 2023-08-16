import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-editor',
  templateUrl: './form-editor.component.html',
  styleUrls: ['./form-editor.component.scss'],
})
export class FormEditorComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public ngModelValue: string | undefined = undefined;
  @Input()
  public classEditor: string = '';
  @Input()
  public placeholder: string = '';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isRequired: boolean = false;
  @Input()
  public classLabel: string = '';
  @Input()
  public showLabel: boolean = true;
  @Input()
  public label: string = '';
  public config: any = {};
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();

  constructor() {}

  ngOnInit() {
    this.config = { height: '15rem', sanitize: false, editable: true };
  }

  handleChange(event) {
    this.ngModelValueChange.emit(event);
    this._onChange.emit(event);
  }
}
