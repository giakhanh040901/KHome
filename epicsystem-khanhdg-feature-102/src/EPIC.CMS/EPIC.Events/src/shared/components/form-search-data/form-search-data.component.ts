import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EConfigDataModal, SearchConst } from '@shared/AppConsts';
import { IActionButtonTable, IHeaderColumn, ITabView } from '@shared/interface/InterfaceConst.interface';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-search-data',
  templateUrl: './form-search-data.component.html',
  styleUrls: ['./form-search-data.component.scss'],
})
export class FormSearchDataComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public showTitle: boolean = true;
  @Input()
  public classTitle: string = 'title-info';
  @Input()
  public title: string = '';
  @Input()
  public showHeader: boolean = true;
  @Input()
  public activeIndex: number = 0;
  @Input()
  public listTabPanel: ITabView[] = [];
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public headerColumns: IHeaderColumn[][] = [];
  @Input()
  public listButtonTable: IActionButtonTable[] = [];
  @Input()
  public typeModal: EConfigDataModal = EConfigDataModal.CREATE;
  @Input()
  public apiGetData: Function[];
  @Input()
  public apiGetResponse: string[];
  @Input()
  public apiGetMapData: Function[];
  @Output()
  public _onChangeTabView: EventEmitter<any> = new EventEmitter<any>();

  public keyword: string = '';
  public subject = {
    keyword: new Subject(),
  };
  public dataSource: any[] = [];
  public isLoading: boolean = false;
  constructor(private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword.trim()) {
        this.getData();
      } else {
        this.dataSource = [];
      }
    });
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public changeKeyword() {
    this.subject.keyword.next();
  }

  public onClickSearch(event: any) {
    if (event && !this.isDisabled) {
      this.getData();
    }
  }

  private getData() {
    this.apiGetData[this.activeIndex](this.keyword).subscribe((res: any) => {
      if (res[this.apiGetResponse[this.activeIndex]]) {
        this.dataSource = this.apiGetMapData[this.activeIndex](res[this.apiGetResponse[this.activeIndex]]);
      }
    });
  }

  public get selectedColumns() {
    return this.headerColumns && this.headerColumns.length ? this.headerColumns[this.activeIndex] : [];
  }

  public onChangeTabView(event: any) {
    if (event) {
      this.dataSource = [];
      this.keyword = '';
      this._onChangeTabView.emit(event);
    }
  }

  public onKeyUp(event) {
    if (event && event.key === 'Enter' && event.keyCode === 13) {
      this.getData();
    }
  }
}
