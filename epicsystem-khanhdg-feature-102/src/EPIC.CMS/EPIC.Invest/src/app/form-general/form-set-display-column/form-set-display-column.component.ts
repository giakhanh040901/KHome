import { Component, OnInit, ViewChild } from '@angular/core';
import { IColumn } from '@shared/interface/p-table.model';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-set-display-column',
  templateUrl: './form-set-display-column.component.html',
  styleUrls: ['./form-set-display-column.component.scss'],
})
export class FormSetDisplayColumnComponent implements OnInit {

  constructor(
    public ref: DynamicDialogRef,
    public configDialog: DynamicDialogConfig 
  ) { }

  title: string;
  submitted = false;
  acceptStatus: boolean = true;

  selectColumns: IColumn[] = [];  
  isCheckedAll:  boolean = true;

  ngOnInit(): void {
    this.selectColumns = JSON.parse(JSON.stringify(this.configDialog.data.cols));
    this.checkIsCheckedAll();
  }

  checkIsCheckedAll() {
    this.isCheckedAll = (!this.selectColumns.find(col => col?.isShow !== true));
  }

  changeSelectedAll() {
    this.selectColumns = this.selectColumns.map(item => { 
      item.isShow = (item?.isPin || !item?.displaySettingColumn) || this.isCheckedAll;
      return item;
    });
  }
 
  accept() {
    this.acceptStatus = true;
    this.onAccept();
  }

  cancel() {
    this.acceptStatus = false;
    this.onAccept();
  }

  onAccept() {
    this.selectColumns = this.selectColumns.sort( (a, b) => a.position - b.position);
    this.ref.close({ data: this.selectColumns, accept: this.acceptStatus });
  }
}