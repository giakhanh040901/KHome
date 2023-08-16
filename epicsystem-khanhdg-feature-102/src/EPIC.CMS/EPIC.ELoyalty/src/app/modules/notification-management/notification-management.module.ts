import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NotificationTemplateComponent } from './notification-template/notification-template.component';
import { AddNotificationTemplateComponent } from './notification-template/add-notification-template/add-notification-template.component';
import { NotificationComponent } from './notification/notification.component';
import { NotificationDetailComponent } from './notification/notification-detail/notification-detail.component';
import { AddPersonListComponent } from './notification/notification-detail/add-person-list/add-person-list.component';
import { SystemTemplateComponent } from './system-template/system-template.component';



@NgModule({
  declarations: [
    NotificationTemplateComponent,
    AddNotificationTemplateComponent,
    NotificationComponent,
    NotificationDetailComponent,
    AddPersonListComponent,
    SystemTemplateComponent
  ],
  imports: [CommonModule, SharedModule,ReactiveFormsModule],
})
export class NotificationManagementModule { }
