import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
    selector: 'e-multiSelect',
    styleUrls: ['./e-multi-select.component.scss'],
    template: `
        <i 
            *ngIf="showClear && selected?.length" (click)="onClear()"
            class="e-multiple-clear-icon pi pi-times">
        </i>
        <p-multiSelect
            [styleClass]="'b-w-100 '+ styleClass + ' ' + (showClear && selected?.length && ' button-clear')"
            emptyMessage="Không có dữ liệu" 
            [placeholder]="placeholder"
            [filter]="filter"
            [options]="options" [optionLabel]="optionLabel" [optionValue]="optionValue"
            [(ngModel)]="selected" 
            (onChange)="onChange($event.value)">
        </p-multiSelect>
    `
})
export class EMultiSelectComponent implements OnInit {

    constructor() { }

    @Input() styleClass: string;
    @Input() placeholder: string;
    @Input() showClear: boolean = true;
    @Input() filter: boolean = true;
    @Input() options: any[];
    @Input() optionLabel: string;
    @Input() optionValue: string = "";

    @Input() selected: any[] = [];
    @Output() selectedChange = new EventEmitter<any>();
    @Output() _onChange = new EventEmitter<any>();

    ngOnInit(): void {
    }

    onClear() {
        this.selected = [];
        this.onChange(this.selected);
    }

    onChange(event) {
        this.selectedChange.emit(event);
        this._onChange.emit();
    }

}
