import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { TransactionProcessingComponent } from './transaction-processing.component';

@NgModule({
  declarations: [TransactionProcessingComponent],
  imports: [SharedModule],
  exports: [TransactionProcessingComponent],
})
export class TransactionProcessingModule {}
