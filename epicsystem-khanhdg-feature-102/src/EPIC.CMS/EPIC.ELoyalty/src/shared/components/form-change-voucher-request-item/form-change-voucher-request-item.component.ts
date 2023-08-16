import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { ChangeVoucherRequestItem } from '@shared/interface/request-management/change-voucher-request/ChangeVoucherRequest.model';

@Component({
  selector: 'form-change-voucher-request-item',
  templateUrl: './form-change-voucher-request-item.component.html',
  styleUrls: ['./form-change-voucher-request-item.component.scss'],
})
export class FormChangeVoucherRequestItemComponent implements OnInit {
  @Input()
  public classContainer: string = '';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public dataSource: ChangeVoucherRequestItem = new ChangeVoucherRequestItem();
  @Input()
  public listVoucher: IDropdown[] = [];
  @Input()
  public isMinusPoint: boolean = false;
  @Output()
  public dataSourceChange: EventEmitter<ChangeVoucherRequestItem> = new EventEmitter<ChangeVoucherRequestItem>();
  @Output()
  public _deleteItem: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public onChange(event: any, key?: string) {
    if (event) {
      if (key === 'voucher') {
        const voucher = this.listVoucher.find((e: IDropdown) => e.value === event.value);
        if (voucher) {
          this.dataSource.changePoint = voucher.rawData.point;
        }
      }
      this.getTotalPoint();
    }
  }

  public delete(event: any) {
    if (event) {
      this._deleteItem.emit(event);
    }
  }

  private getTotalPoint() {
    this.dataSource.totalPoint = !this.isMinusPoint
      ? 0
      : (this.dataSource.changePoint || 0) * (this.dataSource.changeQuantity || 0);
    this.dataSourceChange.emit(this.dataSource);
  }
}
