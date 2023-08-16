import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { BillRequestListComponent } from './bill-request-list.component';

@NgModule({
  declarations: [BillRequestListComponent],
  imports: [SharedModule],
  exports: [BillRequestListComponent],
})
export class BillRequestListModule {}
