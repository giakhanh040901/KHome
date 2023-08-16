import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CrudEventDetailTemplateComponent } from './component/crud-event-detail-template/crud-event-detail-template.component';
import { CreateEventOverviewComponent } from './create-event-overview/create-event-overview.component';
import { EventOverviewDetailAdminComponent } from './event-overview-detail/event-overview-detail-admin/event-overview-detail-admin.component';
import { EventOverviewDetailDeliTicketComponent } from './event-overview-detail/event-overview-detail-deli-ticket/event-overview-detail-deli-ticket.component';
import { EventOverviewDetailDescriptionComponent } from './event-overview-detail/event-overview-detail-description/event-overview-detail-description.component';
import { EventOverviewDetailGeneralComponent } from './event-overview-detail/event-overview-detail-general/event-overview-detail-general.component';
import { CrudEventDetailInforComponent } from './event-overview-detail/event-overview-detail-infor/crud-event-detail-infor/crud-event-detail-infor.component';
import { CrudEventTicketInforComponent } from './event-overview-detail/event-overview-detail-infor/crud-event-detail-infor/crud-event-ticket-infor/crud-event-ticket-infor.component';
import { ReplicateEventTicketInforComponent } from './event-overview-detail/event-overview-detail-infor/crud-event-detail-infor/replicate-event-ticket-infor/replicate-event-ticket-infor.component';
import { EventOverviewDetailInforComponent } from './event-overview-detail/event-overview-detail-infor/event-overview-detail-infor.component';
import { EventOverviewDetailMediaComponent } from './event-overview-detail/event-overview-detail-media/event-overview-detail-media.component';
import { EventOverviewDetailTicketComponent } from './event-overview-detail/event-overview-detail-ticket/event-overview-detail-ticket.component';
import { EventOverviewDetailComponent } from './event-overview-detail/event-overview-detail.component';
import { EventOverviewComponent } from './event-overview.component';
import { StopOrCancelEventComponent } from './stop-or-cancel-event/stop-or-cancel-event.component';

@NgModule({
  declarations: [
    EventOverviewComponent,
    StopOrCancelEventComponent,
    CreateEventOverviewComponent,
    EventOverviewDetailComponent,
    EventOverviewDetailGeneralComponent,
    EventOverviewDetailDescriptionComponent,
    EventOverviewDetailInforComponent,
    EventOverviewDetailMediaComponent,
    CrudEventDetailInforComponent,
    ReplicateEventTicketInforComponent,
    CrudEventTicketInforComponent,
    EventOverviewDetailTicketComponent,
    EventOverviewDetailAdminComponent,
    CrudEventDetailTemplateComponent,
    EventOverviewDetailDeliTicketComponent,
  ],
  imports: [SharedModule],
  exports: [EventOverviewComponent],
})
export class EventOverviewModule {}
