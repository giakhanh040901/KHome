import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CrudEventDetailTemplate } from '@shared/interface/event-management/event-overview/CrudEventDetailTemplate.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-event-detail-template',
  templateUrl: './crud-event-detail-template.component.html',
  styleUrls: ['./crud-event-detail-template.component.scss'],
})
export class CrudEventDetailTemplateComponent extends CrudComponentBase {
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
  public dto: CrudEventDetailTemplate = new CrudEventDetailTemplate();
  public label: string = '';
  public type: string = EConfigDataModal.CREATE;
  public apiCrud: Function;

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
      this.label = this.config.data.dataSource.label || '';
    }
    if (this.type !== EConfigDataModal.VIEW) {
      this.apiCrud = this.config.data.dataSource.apiCrud;
    }
    if (this.type !== EConfigDataModal.CREATE) {
      this.dto.mapData(this.config.data.dataSource);
    }
  }

  public get labelName() {
    return `Tên mẫu ${this.label}`;
  }

  public upload(event: any) {
    if (event && event.files && event.files.length && event.files[0]) {
      const file = event.files[0];
      this.eventOverviewService.uploadFileEventDetail(file, 'event-overview').subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Tải file thành công')) {
            this.dto.link = response.data;
          }
        },
        (err) => {
          this.messageError('Có sự cố khi tải file!');
        }
      );
    }
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event: any) {
    if (event) {
      if (this.dto.isValidData()) {
        this.apiCrud(this.dto.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT).subscribe(
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
}
