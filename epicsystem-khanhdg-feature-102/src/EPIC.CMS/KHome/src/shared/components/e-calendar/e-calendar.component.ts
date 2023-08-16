import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import * as moment from 'moment';

@Component({
    selector: 'e-calendar',
    styleUrls: ['./e-calendar.component.scss'],
    template: `
        <p-calendar 
            [class]="'custom-input-disabled ' + class" 
            [inputId]="inputId" [dateFormat]="dateFormat" appendTo="body"
            [(ngModel)]="value"
            [showIcon]="showIcon"
            [readonlyInput]="readonlyInput" 
            [minDate]="minDate" [maxDate]="maxDate" 
            [showTime]="showTime"
            [disabled]="disabled" 
            (onSelect)="onChange($event)"
            [showButtonBar]="showButtonBar"
            [selectionMode]="selectionMode"
            (onClearClick)="onClear()"
            [style]="styleInline"
            [view]="view" [placeholder]="placeholder"
            >
        </p-calendar>
    `

})
export class ECalendarComponent implements OnInit {

    constructor(
        private ref: ChangeDetectorRef,
    ) { }

    @Input() class: string;
    @Input() inputId: string;
    @Input() dateFormat: string = "dd-mm-yy";
    @Input() value: any | any[];
    @Input() showIcon: boolean = true;
    @Input() readonlyInput: boolean = true;
    @Input() minDate: Date = null;
    @Input() maxDate: Date = null;
    @Input() showTime: boolean = false;
    @Input() disabled: boolean = false;
    @Input() showButtonBar: boolean = true;
    @Input() isParam: boolean = false;
    @Input() view: string = "date";
    @Input() placeholder: string = 'dd-mm-yyyy';
    @Input() selectionMode: string = "single";
    @Input() styleInline: any;

    @Output() _onChange = new EventEmitter<string>();
    @Output() _onClear = new EventEmitter<string>();
    @Output() valueChange = new EventEmitter<string>();

    modeRange: string = 'range';

    ngOnInit(): void {
        if(this.value){
            this.onChange(this.value);
        }
    }

    ngOnChanges(changes: SimpleChanges) {
        if(changes?.value && changes.value.currentValue) {
            if(this.selectionMode === this.modeRange) {
                this.value = changes.value.currentValue.map(v => new Date(v));
            } else {
                this.value = new Date(changes.value.currentValue);
            }
        }

        if(changes?.minDate && changes?.minDate.currentValue) {
            this.minDate = new Date(changes.minDate.currentValue);
        }

        if(changes?.maxDate && changes?.maxDate.currentValue) {
            this.maxDate = new Date(changes.maxDate.currentValue);
        }
    }

    onChange(event) {
        let dateFormatOutput = 'YYYY-MM-DDTHH:mm:ss';
        if(this.isParam) dateFormatOutput = 'YYYY-MM-DD';
        let value: any;
        if(this.selectionMode === this.modeRange) {
            if(this.value[1]) {
                value = this.value.map(e => moment(e).format(dateFormatOutput))
                this.emit(value);
            }
        } else {
            console.log({event: event});
            
            value = moment(event).format(dateFormatOutput);
            this.emit(value);
        }
    }

    emit(value) {
        this.valueChange.emit(value);
        this._onChange.emit(value);
    }

    onClear() {
        this.valueChange.emit(null);
        this._onClear.emit();
    }
}
