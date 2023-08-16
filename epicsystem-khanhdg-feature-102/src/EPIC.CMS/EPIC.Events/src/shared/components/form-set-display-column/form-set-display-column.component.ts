import { Component, OnInit } from '@angular/core';
import { ETypeDataTable } from '@shared/AppConsts';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-set-display-column',
  templateUrl: './form-set-display-column.component.html',
  styleUrls: ['./form-set-display-column.component.scss'],
})
export class FormSetDisplayColumnComponent implements OnInit {
  constructor(public ref: DynamicDialogRef, public configDialog: DynamicDialogConfig) {}

  title: string;
  submitted = false;
  acceptStatus: boolean = true;

  cols: any[];
  selectedColumns: any[];
  checkAll: boolean;

  public get selectColumns() {
    let result = [...this.cols];
    result = result.filter(
      (col: any) =>
        col.type === ETypeDataTable.TEXT ||
        col.type === ETypeDataTable.CHECK_BOX ||
        col.type === ETypeDataTable.SELECT_CHECK_BOX || 
        col.type === ETypeDataTable.COUNT_DOWN_TIME
    );
    return result;
  }

  ngOnInit(): void {
    this.cols = this.configDialog.data.cols;
    this.selectedColumns = this.configDialog.data.comlumnSelected;
    if (this.selectedColumns.length == this.selectColumns.length) {
      this.checkAll = true;
    } else {
      this.checkAll = false;
    }
  }
  // check nếu đã chọn tất cả ở dưới thì nút chọn tất cả tự động được chọn
  checkCheckAll() {
    if (this.selectedColumns.length == this.selectColumns.length) {
      this.checkAll = true;
    } else {
      this.checkAll = false;
    }
  }

  selectAllCheckbox() {
    if (this.checkAll == true) {
      this.selectedColumns = [...this.cols];
    } else {
      this.selectedColumns = this.selectedColumns.map((column, index) => {
        if (
          column?.isPin ||
          column.type === ETypeDataTable.INDEX ||
          column.type === ETypeDataTable.ACTION ||
          column.type === ETypeDataTable.ACTION_BUTTON ||
          column.type === ETypeDataTable.STATUS
        ) {
          return column;
        }
      });
      this.selectedColumns = this.selectedColumns.filter((item) => item);
    }
  }

  hideDialog() {}

  accept() {
    this.acceptStatus = true;
    this.onAccept();
  }

  cancel() {
    this.acceptStatus = false;
    this.onAccept();
  }

  onAccept() {
    this.ref.close({ data: this.selectedColumns, accept: this.acceptStatus });
  }
}
