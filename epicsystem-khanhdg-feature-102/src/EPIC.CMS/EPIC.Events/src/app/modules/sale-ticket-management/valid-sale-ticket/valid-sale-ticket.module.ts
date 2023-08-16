import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { ValidSaleTicketComponent } from './valid-sale-ticket.component';

@NgModule({
  declarations: [
    ValidSaleTicketComponent,
  ],
  imports: [SharedModule],
  exports: [ValidSaleTicketComponent],
})
export class ValidSaleTicketModule {}
