import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { ApplyVoucherHistoryComponent } from './apply-voucher-history.component';

@NgModule({
  declarations: [ApplyVoucherHistoryComponent],
  imports: [SharedModule],
  exports: [ApplyVoucherHistoryComponent],
})
export class ApplyVoucherHistoryModule {}
