import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TicketRequestListComponent } from './ticket-request-list.component';

@NgModule({
  declarations: [TicketRequestListComponent],
  imports: [SharedModule],
  exports: [TicketRequestListComponent],
})
export class TicketRequestListModule {}
