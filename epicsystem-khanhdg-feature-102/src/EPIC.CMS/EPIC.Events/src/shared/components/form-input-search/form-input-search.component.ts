import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-input-search',
  templateUrl: './form-input-search.component.html',
  styleUrls: ['./form-input-search.component.scss']
})
export class FormInputSearchComponent implements OnInit {

  constructor() { }

  @Input() placeholder: string = 'Tìm kiếm...';
  @Input() width: number;
  @Input() class: string;
  @Input() disabled: boolean = false;
  @Input() keyword: string;
  @Output() keywordChange: EventEmitter<string> = new EventEmitter<string>();
  @Output() _onChange: EventEmitter<string> = new EventEmitter<string>();

  changeKeywordSubject: Subject<string> = new Subject<string>();

  ngOnInit(): void {
    this.changeKeywordSubject.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      this._onChange.emit();
    })
  }

  changeKeyword(event) {
    this.keywordChange.emit(event.target.value);
    this.changeKeywordSubject.next();
  }

}
