import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal, EventOverview } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CreateEventOverview } from '@shared/interface/event-management/event-overview/EventOverview.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'create-event-overview',
  templateUrl: './create-event-overview.component.html',
  styleUrls: ['./create-event-overview.component.scss'],
})
export class CreateEventOverviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private eventOverviewService: EventOverviewService
  ) {
    super(injector, messageService);
  }
  public dto: CreateEventOverview = new CreateEventOverview();
  public type: string = EConfigDataModal.CREATE;

  public get listTypeEvent() {
    return EventOverview.listTypeEvent;
  }

  ngOnInit() {}

  public save(event?: any) {
    if (event) {
      if (this.dto.isValidData()) {
        this.eventOverviewService
          .createOrEdit(this.dto.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        this.messageDataValidator(this.dto.dataValidator);
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }
}
