import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'input-search',
  styleUrls: ['./input-search.component.scss'],
  template: `
    <span class="block mt-2 md:mt-0 p-input-icon-left">
        <i class="pi pi-search"></i>
        <input
             
            pInputText type="text" [class]="class"
            [placeholder]="placeholder" [style.width.px]="width" [style.width.%]="!width && 100"
            [(ngModel)]="keyword" (input)="changeKeyword($event)"
        />
    </span>
  `
})
export class InputSearchComponent implements OnInit {

    constructor() { }

    @Input() placeholder: string;
    @Input() width: number;
    @Input() class: string;

    @Input() keyword: string;
    @Output() keywordChange: EventEmitter<string> = new EventEmitter<string>();
    @Output() _onChange: EventEmitter<string> = new EventEmitter<string>();

    changeKeywordSubject: Subject<string> = new Subject<string>();

    ngOnInit(): void {
        this.changeKeywordSubject.pipe(debounceTime(800)).subscribe(() => {
            this._onChange.emit();
        }) 
    }

    changeKeyword(event) {
        this.keywordChange.emit(event.target.value);
        this.changeKeywordSubject.next();
    }
}
