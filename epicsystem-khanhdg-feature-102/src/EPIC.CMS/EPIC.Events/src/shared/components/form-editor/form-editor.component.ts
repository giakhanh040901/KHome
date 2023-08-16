import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';

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
  @Input()
  public height: string = '15rem';
  public config: any = {};
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<string | undefined> = new EventEmitter<string | undefined>();

  constructor() {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.height) {
      this.config = { height: this.height, sanitize: false, editable: true };
    }
  }

  handleChange(event) {
    this.ngModelValueChange.emit(event);
    this._onChange.emit(event);
  }
}
