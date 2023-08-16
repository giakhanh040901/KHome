import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SaleTicketOrderItem } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailOverview.model';

@Component({
  selector: 'sale-ticket-order-item',
  templateUrl: './sale-ticket-order-item.component.html',
  styleUrls: ['./sale-ticket-order-item.component.scss'],
})
export class SaleTicketOrderItemComponent implements OnInit {
  @Input()
  public dto: SaleTicketOrderItem = new SaleTicketOrderItem();
  @Input()
  public isDisableOrderQuantity: boolean = true;
  @Output()
  public _onChangeOrderQuantity: EventEmitter<any> = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  public onChangeOrderQuantity(event: any) {
    if (event) {
      this._onChangeOrderQuantity.emit(event);
    }
  }
}
