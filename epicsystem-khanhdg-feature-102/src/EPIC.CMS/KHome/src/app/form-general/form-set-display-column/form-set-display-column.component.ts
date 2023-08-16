import { Component, OnInit, ViewChild } from '@angular/core';
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

  cols: any[];  
  selectedColumns: any[];
  selectColumns: any[];  
  checkAll:  boolean;

  ngOnInit(): void {
    this.cols = this.configDialog.data.cols;
    this.selectedColumns = this.configDialog.data.comlumnSelected;
    this.selectColumns = [...this.cols];
    if(this.selectedColumns.length == this.selectColumns.length) {
      this.checkAll=true;
    } else {
      this.checkAll = false;
    }
  }
  // check nếu đã chọn tất cả ở dưới thì nút chọn tất cả tự động được chọn
  checkCheckAll(){
    if(this.selectedColumns.length == this.selectColumns.length) {
      this.checkAll=true;
    } else {
      this.checkAll = false;
    }
  }

  selectAllCheckbox(){
   
    if(this.checkAll == true){
      this.selectedColumns  = [...this.cols]
    } else {
      this.selectedColumns = this.selectedColumns.map((column,index)=>{
        if(column?.isPin){
          return column;
        }
      });
      this.selectedColumns=this.selectedColumns.filter(item=>item);
    }
  }
 
  hideDialog() { 
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
    this.ref.close({ data: this.selectedColumns, accept: this.acceptStatus });
  }
}