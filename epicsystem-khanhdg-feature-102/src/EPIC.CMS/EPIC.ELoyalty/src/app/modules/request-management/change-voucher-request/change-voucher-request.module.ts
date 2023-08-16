import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { ChangeVoucherRequestComponent } from './change-voucher-request.component';
import { CrudChangeVoucherRequestComponent } from './crud-change-voucher-request/crud-change-voucher-request.component';

@NgModule({
  declarations: [ChangeVoucherRequestComponent, CrudChangeVoucherRequestComponent],
  imports: [SharedModule],
  exports: [ChangeVoucherRequestComponent],
})
export class ChangeVoucherRequestModule {}
