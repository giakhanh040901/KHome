import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { CheckboxConsts } from '@shared/consts/checkbox.consts';

@Component({
  selector: 'e-checkbox-single',
  styleUrls: ['./e-checkbox-single.component.scss'],
  template: `
    <p-checkbox 
        [(ngModel)]="checked" [binary]="true" [readonly]="readonly" [inputId]="id" (onChange)="onChange($event.checked)">
    </p-checkbox>
  `
})

export class ECheckboxSingleComponent implements OnInit {

    constructor() { }

    @Input() id: string;
    @Input() value: boolean | string;
    @Input() type: string = CheckboxConsts.TYPE_YESNO;
    @Input() readonly: boolean = false;
    @Output() valueChange = new EventEmitter<boolean | string>();

    checked: boolean;
    checkedValues = CheckboxConsts.getCheckedValues();

    ngOnInit(): void {}

    ngOnChanges(changes: SimpleChanges): void {     
        if(changes?.value) {
            this.checked = this.checkedValues.includes(changes.value.currentValue);
        }
    }

    onChange(checked) {
        let checkboxValue = CheckboxConsts.values.find(c => c.type === this.type);
        this.value = checkboxValue[checked ? CheckboxConsts.CHECKED : CheckboxConsts.UNCHECKED];
        this.valueChange.emit(this.value);
    }

}
