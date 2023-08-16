import { Component, OnInit } from '@angular/core';
import { ETypeDataTable, ETypeFormatDate, IndividualCustomer } from '@shared/AppConsts';
import { formatDate } from '@shared/function-common';
import { IConstant, IDropdown, IHeaderColumn, IValueFormatter } from '@shared/interface/InterfaceConst.interface';
import { IndividualCustomerVoucherDetail } from '@shared/interface/customer-management/individual-customer/IndividualCustomer.model';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'individual-voucher-detail-dialog',
  templateUrl: './individual-voucher-detail-dialog.component.html',
  styleUrls: ['./individual-voucher-detail-dialog.component.scss'],
})
export class IndividualVoucherDetailDialogComponent implements OnInit {
  public dto: IndividualCustomerVoucherDetail = new IndividualCustomerVoucherDetail();
  public headerColumns: IHeaderColumn[] = [];

  constructor(private ref: DynamicDialogRef, private config: DynamicDialogConfig) {}

  public get listStatus() {
    return IndividualCustomer.listStatusVoucher;
  }

  public get listSource() {
    return IndividualCustomer.listSource;
  }

  ngOnInit() {
    this.headerColumns = [
      {
        field: 'id',
        header: 'ID',
        type: ETypeDataTable.TEXT,
        width: '5rem',
      },
      {
        field: 'date',
        header: 'Ngày',
        type: ETypeDataTable.TEXT,
        width: '12rem',
        valueFormatter: (param: IValueFormatter) =>
          param.data ? formatDate(param.data, ETypeFormatDate.DATE_TIME) : '',
      },
      {
        field: 'status',
        header: 'Trạng thái',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        valueFormatter: (param: IValueFormatter) =>
          this.listStatus.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'user',
        header: 'Người thực hiện',
        type: ETypeDataTable.TEXT,
        width: '15rem',
      },
      {
        field: 'source',
        header: 'Nguồn',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        valueFormatter: (param: IValueFormatter) =>
          this.listSource.find((e: IConstant) => e.id === param.data)?.value || '',
      },
      {
        field: 'description',
        header: 'Ghi chú',
        type: ETypeDataTable.TEXT,
        width: 'auto',
      },
    ] as IHeaderColumn[];

    this.dto.mapData(this.config.data.dataSource);
  }

  public close(event: any) {
    this.ref.close();
  }
}
