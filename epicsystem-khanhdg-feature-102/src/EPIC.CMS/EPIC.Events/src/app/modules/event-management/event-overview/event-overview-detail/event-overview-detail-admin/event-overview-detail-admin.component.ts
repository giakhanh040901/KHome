import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { FormNotificationConst, MARKDOWN_OPTIONS } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDescriptionContent, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { EventOverviewAdminModel } from '@shared/interface/event-management/event-overview/EventOverviewAdmin.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'event-overview-detail-admin',
  templateUrl: './event-overview-detail-admin.component.html',
  styleUrls: ['./event-overview-detail-admin.component.scss'],
})
export class EventOverviewDetailAdminComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private dialogService: DialogService
  ) {
    super(injector, messageService);
  }
  public dto: EventOverviewAdminModel = new EventOverviewAdminModel();
  public dataSource: EventOverviewAdminModel[] = [];
  public isEdit: boolean = false;
  customers: any[] = [];

  public get isDisable() {
    return !this.isEdit;
  }

  ngOnInit() {
    this.dto.id = this.eventOverviewService.eventId;
    this.getAllEventAdmin();
  }

  private getAllEventAdmin() {
    this.eventOverviewService.getAllEventAdmin().subscribe((res: any) => {
      if (res.data) {
        this.dataSource = res.data
      }
    });
  }

  removeElement(index, data) {
    const ref = this.dialogService.open(FormNotificationComponent, {
      header: 'Thông báo',
      width: '600px',
      contentStyle: {
        'max-height': '600px',
        overflow: 'auto',
        'padding-bottom': '50px',
      },
      styleClass: 'p-dialog-custom',
      baseZIndex: 10000,
      data: {
        title: 'Bạn có chắc chắn muốn xóa nguời quản lý này?',
        icon: FormNotificationConst.IMAGE_APPROVE,
      } as INotiDataModal,
    });

    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this.eventOverviewService
          .deleteEventAdmin(data.id)
          .subscribe(
            (res) => {
              this.isLoading = false;
              if (
                this.handleResponseInterceptor(
                  res,
                  "Xóa thành công!"
                )
              ) {
                this.getAllEventAdmin();
              }
            },
            (err) => {
              this.isLoading = false;
            }
          );
      } else {
        this.messageError(
          "Xóa không thành công (FE)"
        );
      }

    });
  }

  getInfoCustomer() {
    this.keyword = this.keyword.trim();
    this.eventOverviewService.getInfoInvestorCustomer(this.keyword).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, "")) {
          this.customers = res?.data?.items;

          if (!this.customers.length) {
            this.messageError("Không tìm thấy dữ liệu");
          } else {
            let body = {
              eventId: this.eventOverviewService.eventId,
              investorIds: [this.customers[0].investorId]
            }
            this.eventOverviewService
              .createOrUpdateEventAdmin(body, false)
              .subscribe(
                (res) => {
                  this.isLoading = false;
                  if (
                    this.handleResponseInterceptor(
                      res,
                      "Thêm thành công!"
                    )
                  ) {
                    this.getAllEventAdmin();
                  }
                },
                (err) => {
                  this.isLoading = false;
                }
              );
          }
        }

      },
      (err) => {
        console.log("Error-------", err);
        this.isLoading = false;
      }
    );
  }

  public edit(event: any) {
    if (event) {
      this.isEdit = true;
    }
  }

  public save(event: any) {
    this.isEdit = false;
  }
}

