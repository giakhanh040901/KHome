import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { AccumulatePointManagementComponent } from './accumulate-point-management.component';
import { CrudAccumulatePointManagementComponent } from './crud-accumulate-point-management/crud-accumulate-point-management.component';

@NgModule({
  declarations: [
    AccumulatePointManagementComponent, 
    CrudAccumulatePointManagementComponent
  ],
  imports: [SharedModule],
  exports: [AccumulatePointManagementComponent],
})
export class AccumulatePointManagementModule {}
