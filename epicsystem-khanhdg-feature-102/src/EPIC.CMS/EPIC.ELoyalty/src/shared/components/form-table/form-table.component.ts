import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EPositionFrozenCell, EPositionTextCell, ETypeDataTable, ETypeSortTable } from '@shared/AppConsts';
import {
  IActionButtonTable,
  IActionTable,
  IHeaderColumn,
  ISortTable,
} from '@shared/interface/InterfaceConst.interface';
import { Page } from '@shared/model/page';

@Component({
  selector: 'form-table',
  templateUrl: './form-table.component.html',
  styleUrls: ['./form-table.component.scss'],
})
export class FormTableComponent implements OnInit {
  @Input()
  public columns: IHeaderColumn[] = [];
  @Input()
  public dataSource: any[] = [];
  @Input()
  public isLoading: boolean = false;
  @Input()
  public rowHover: boolean = true;
  @Input()
  public showCurrentPageReport: boolean = true;
  @Input()
  public showPaginator: boolean = true;
  @Input()
  public responsiveLayout: string = 'scroll';
  @Input()
  public dataKey: string = 'id';
  @Input()
  public resizableColumns: boolean = false;
  @Input()
  public scrollable: boolean = true;
  @Input()
  public scrollHeight: string = '500px';
  @Input()
  public styleClass: string = '';
  @Input()
  public actionButtons: IActionButtonTable[] = [];
  @Input()
  public listAction: IActionTable[][] = [];
  @Input()
  public page: Page = new Page();
  @Output()
  public _setColumn: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _changePage: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onSort: EventEmitter<ISortTable> = new EventEmitter<ISortTable>();

  @Input() selectedItems: any[] = [];
	@Output() selectedItemsChange: EventEmitter<any> = new EventEmitter<any>();
	@Output() _onChangeCheckbox: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  public get ETypeDataTable() {
    return ETypeDataTable;
  }

  public get EPositionTextCell() {
    return EPositionTextCell;
  }

  ngOnInit() {}

  public isGetShowButton = (data: any, action: IActionButtonTable, index: number) => {
    if (action.showFunction) {
      return action.showFunction(data, action, index);
    }
    return true;
  };

  public handleClickButton(data: any, action: IActionButtonTable, index: number) {
    if (action.callBack) {
      action.callBack(data, index);
    }
  }

  public getStatus(status: any, key: string, col: IHeaderColumn) {
    if (key === 'style') {
      return col.funcStyleClassStatus(status);
    } else if (key === 'label') {
      return col.funcLabelStatus(status);
    }
    return '';
  }

  public setColumn(event: any) {
    if (event) {
      this._setColumn.emit(event);
    }
  }

  public handleChangePage(event: any) {
    if (event) {
      this._changePage.emit(event);
    }
  }

  public getRightColCss(index: number) {
    if (this.columns[index].isFrozen && this.columns[index].posFrozen === EPositionFrozenCell.RIGHT) {
      let res: string = '0px';
      for (let i = index + 1; i < this.columns.length; i++) {
        if (this.columns[i].isFrozen && this.columns[i].posFrozen === EPositionFrozenCell.RIGHT) {
          res += ` + ${this.columns[i].width}`;
        }
      }
      return `calc(${res})`;
    }
    return undefined;
  }

  public getFieldSort(col: IHeaderColumn) {
    if (!col.isSort) {
      return '';
    }
    return col.fieldSort || col.field;
  }

  onSelectedChange() {
		this.selectedItemsChange.emit(this.selectedItems);
		this._onChangeCheckbox.emit(this.selectedItems);
	}

  public handleSort(event: any) {
    if (event && !!event.sortField) {
      const typeSort = event.sortOrder === ETypeSortTable.ASCENDING ? ETypeSortTable.ASC : ETypeSortTable.DESC;
      this._onSort.emit({
        field: event.sortField,
        type: typeSort,
        sort: event.sortField + '-' + typeSort,
      } as ISortTable);
    }
  }
}
