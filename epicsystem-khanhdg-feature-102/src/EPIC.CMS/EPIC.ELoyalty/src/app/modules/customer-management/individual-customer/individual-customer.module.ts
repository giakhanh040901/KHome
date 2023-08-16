import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { IndividualCustomerDetailHistoryComponent } from './individual-customer-detail/individual-customer-detail-history/individual-customer-detail-history.component';
import { IndividualCustomerDetailOfferComponent } from './individual-customer-detail/individual-customer-detail-offer/individual-customer-detail-offer.component';
import { IndividualVoucherDetailDialogComponent } from './individual-customer-detail/individual-customer-detail-offer/individual-voucher-detail-dialog/individual-voucher-detail-dialog.component';
import { IndividualCustomerDetailOverviewComponent } from './individual-customer-detail/individual-customer-detail-overview/individual-customer-detail-overview.component';
import { IndividualCustomerDetailComponent } from './individual-customer-detail/individual-customer-detail.component';
import { IndividualCustomerComponent } from './individual-customer.component';

@NgModule({
  declarations: [
    IndividualCustomerComponent,
    IndividualCustomerDetailComponent,
    IndividualCustomerDetailOverviewComponent,
    IndividualCustomerDetailOfferComponent,
    IndividualVoucherDetailDialogComponent,
    IndividualCustomerDetailHistoryComponent,
  ],
  imports: [SharedModule],
  exports: [IndividualCustomerComponent],
})
export class IndividualCustomerModule {}
