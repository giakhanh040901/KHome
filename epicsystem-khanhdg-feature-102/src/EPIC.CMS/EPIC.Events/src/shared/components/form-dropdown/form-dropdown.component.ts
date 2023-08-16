import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';

export const LABEL = 'label';

@Component({
  selector: 'form-dropdown',
  templateUrl: './form-dropdown.component.html',
  styleUrls: ['./form-dropdown.component.scss'],
})
export class FormDropdownComponent implements OnInit {
  @Input()
  public floatLabel: boolean = false;
  @Input()
  public classContainer: string = '';
  @Input()
  public classLabel: string = '';
  @Input()
  public classDropdown: string = '';
  @Input()
  public widthDropdown: string = '100%';
  @Input()
  public options: IDropdown[] = [];
  @Input()
  public ngModelValue: number | string | undefined = undefined;
  @Input()
  public showClear: boolean = true;
  @Input()
  public showClearFloatLabel: boolean = false;
  @Input()
  public showLabel: boolean = true;
  @Input()
  public label: string = '';
  @Input()
  public placeholder: string = '';
  @Input()
  public isRequired: boolean = false;
  @Input()
  public autoDisplayFirst: boolean = false;
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public isFilter: boolean = false;
  @Input()
  public filterField: string = LABEL;
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public ngModelValueChange: EventEmitter<number | string | undefined> = new EventEmitter<
    number | string | undefined
  >();

  constructor() {}

  ngOnInit() {}

  ngOnChanges(changes: SimpleChanges) {
    this.placeholder = !!this.floatLabel ? this.placeholder || 'Tất cả' : this.placeholder;
  }

  public onChange(event: any) {
    if (event) {
      this.ngModelValueChange.emit(event.value);
      this._onChange.emit(event);
    }
  }

  public get filterBy() {
    return this.isFilter ? (this.filterField === LABEL ? this.filterField : `rawData.${this.filterField}`) : undefined;
  }

  public onClear(event: any) {
    if (event) {
      this.ngModelValueChange.emit(undefined);
    }
  }
}
