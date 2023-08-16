import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { EventOverview } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { StopOrCancelEvent } from '@shared/interface/event-management/event-overview/EventOverview.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'stop-or-cancel-event',
  templateUrl: './stop-or-cancel-event.component.html',
  styleUrls: ['./stop-or-cancel-event.component.scss'],
})
export class StopOrCancelEventComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private eventOverviewService: EventOverviewService
  ) {
    super(injector, messageService);
  }
  public dto: StopOrCancelEvent = new StopOrCancelEvent();
  public type: string = '';

  public get listReason() {
    return EventOverview.listReason;
  }

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
      this.dto.id = this.config.data.dataSource.id;
    }
    this.listReason.length && (this.dto.reason = Number(this.listReason[0].value));
    this.dto.createUser = this.eventOverviewService.currentUser;
    this.dto.createDate = this.eventOverviewService.currentTime;
  }

  public close(event) {
    this.ref.close();
  }

  public save(event) {
    if (event) {
      const body = {
        id: this.dto.id,
        reason: this.dto.reason,
        summary: this.dto.description,
      };
      this.eventOverviewService.stopOrCancelEvent(body, this.type).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, '')) {
            this.ref.close(response);
          }
        },
        (err) => {}
      );
    }
  }
}
