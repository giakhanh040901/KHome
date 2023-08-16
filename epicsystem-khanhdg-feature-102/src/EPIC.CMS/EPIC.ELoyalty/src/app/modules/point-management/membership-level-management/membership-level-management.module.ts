import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CrudMembershipLevelManagementComponent } from './crud-membership-level-management/crud-membership-level-management.component';
import { MembershipLevelManagementComponent } from './membership-level-management.component';

@NgModule({
  declarations: [MembershipLevelManagementComponent, CrudMembershipLevelManagementComponent],
  imports: [SharedModule],
  exports: [MembershipLevelManagementComponent],
})
export class MembershipLevelManagementModule {}
