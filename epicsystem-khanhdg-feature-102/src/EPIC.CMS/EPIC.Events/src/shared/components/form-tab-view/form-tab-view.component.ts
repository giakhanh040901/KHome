import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ITabView } from '@shared/interface/InterfaceConst.interface';

@Component({
  selector: 'form-tab-view',
  templateUrl: './form-tab-view.component.html',
  styleUrls: ['./form-tab-view.component.scss'],
})
export class FormTabViewComponent implements OnInit {
  @Input()
  public activeIndex: number = 0;
  @Input()
  public listTabPanel: ITabView[] = [];
  @Output()
  public activeIndexChange: EventEmitter<number> = new EventEmitter<number>();
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public handleChangeTab(event: any) {
    if (event) {
      this.activeIndexChange.emit(event.index);
      this._onChange.emit(event);
    }
  }
}
