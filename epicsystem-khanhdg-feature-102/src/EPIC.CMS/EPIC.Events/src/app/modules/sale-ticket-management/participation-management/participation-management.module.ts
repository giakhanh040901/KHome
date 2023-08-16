import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { ParticipationManagementComponent } from './participation-management.component';

@NgModule({
  declarations: [ParticipationManagementComponent],
  imports: [SharedModule],
  exports: [ParticipationManagementComponent],
})
export class ParticipationManagementModule {}
