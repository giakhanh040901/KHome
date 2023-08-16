import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { SaleTicketOrderItemComponent } from './components/sale-ticket-order-item/sale-ticket-order-item.component';
import { SaleTicketOrderSearchSaleComponent } from './components/sale-ticket-order-search-sale/sale-ticket-order-search-sale.component';
import { CreateSaleTicketOrderConfirmComponent } from './create-sale-ticket-order/create-sale-ticket-order-confirm/create-sale-ticket-order-confirm.component';
import { CreateSaleTicketOrderCustomerComponent } from './create-sale-ticket-order/create-sale-ticket-order-customer/create-sale-ticket-order-customer.component';
import { CreateSaleTicketOrderEventComponent } from './create-sale-ticket-order/create-sale-ticket-order-event/create-sale-ticket-order-event.component';
import { CreateSaleTicketOrderComponent } from './create-sale-ticket-order/create-sale-ticket-order.component';
import { SaleTicketOrderDetailHistoryComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-history/sale-ticket-order-detail-history.component';
import { SaleTicketOrderDetailListComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-list/sale-ticket-order-detail-list.component';
import { SaleTicketOrderDetailOverviewComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-overview/sale-ticket-order-detail-overview.component';
import { CrudSaleTicketTransactionComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-transaction/crud-sale-ticket-transaction/crud-sale-ticket-transaction.component';
import { EmulatorMsbDialogComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-transaction/emulator-msb-dialog/emulator-msb-dialog.component';
import { SaleTicketOrderDetailTransactionComponent } from './sale-ticket-order-detail/sale-ticket-order-detail-transaction/sale-ticket-order-detail-transaction.component';
import { SaleTicketOrderDetailComponent } from './sale-ticket-order-detail/sale-ticket-order-detail.component';
import { SaleTicketOrderComponent } from './sale-ticket-order.component';

@NgModule({
  declarations: [
    SaleTicketOrderComponent,
    CreateSaleTicketOrderComponent,
    SaleTicketOrderDetailComponent,
    SaleTicketOrderDetailOverviewComponent,
    SaleTicketOrderDetailTransactionComponent,
    SaleTicketOrderDetailListComponent,
    CrudSaleTicketTransactionComponent,
    CreateSaleTicketOrderCustomerComponent,
    CreateSaleTicketOrderEventComponent,
    CreateSaleTicketOrderConfirmComponent,
    SaleTicketOrderItemComponent,
    SaleTicketOrderSearchSaleComponent,
    SaleTicketOrderDetailHistoryComponent,
    EmulatorMsbDialogComponent,
  ],
  imports: [SharedModule],
  exports: [SaleTicketOrderComponent],
})
export class SaleTicketOrderModule {}
