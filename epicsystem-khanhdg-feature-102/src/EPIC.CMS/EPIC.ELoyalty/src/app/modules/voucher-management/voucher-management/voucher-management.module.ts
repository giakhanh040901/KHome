import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CreateOrEditVoucherDialogComponent } from './create-or-edit-voucher-dialog/create-or-edit-voucher-dialog.component';
import { VoucherManagementComponent } from './voucher-management.component';

@NgModule({
  declarations: [VoucherManagementComponent, CreateOrEditVoucherDialogComponent],
  imports: [SharedModule],
  exports: [VoucherManagementComponent],
})
export class VoucherManagementModule {}
