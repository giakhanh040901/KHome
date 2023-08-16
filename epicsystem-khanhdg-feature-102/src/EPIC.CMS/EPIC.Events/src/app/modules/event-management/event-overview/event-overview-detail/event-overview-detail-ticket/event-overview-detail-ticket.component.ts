import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal,
} from '@shared/interface/InterfaceConst.interface';
import { EventOverviewDetailTicketModel } from '@shared/interface/event-management/event-overview/EventOverviewDetailTicket.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { SpinnerService } from '@shared/services/spinner.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudEventDetailTemplateComponent } from '../../component/crud-event-detail-template/crud-event-detail-template.component';

@Component({
  selector: 'event-overview-detail-ticket',
  templateUrl: './event-overview-detail-ticket.component.html',
  styleUrls: ['./event-overview-detail-ticket.component.scss'],
})
export class EventOverviewDetailTicketComponent extends CrudComponentBase {
  public headerColumns: IHeaderColumn[] = [];
  public dataSource: EventOverviewDetailTicketModel[] = [];
  public isLoading: boolean;
  public listAction: IActionTable[][] = [];
  public urlfilePDF: string = '';

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private spinnerService: SpinnerService
  ) {
    super(injector, messageService);
  }

  public get listStatus() {
    return EventOverview.listStatusTicket;
  }

  public getStatusSeverity(code: any) {
    return EventOverview.getStatusTicket(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return EventOverview.getStatusTicket(code, ETypeStatus.LABEL);
  }

  ngOnInit() {
    this.headerColumns = [
      {
        field: 'id',
        header: '#ID',
        width: '5rem',
        isPin: true,
        type: ETypeDataTable.INDEX,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.LEFT,
      },
      {
        field: 'name',
        header: 'Tên mẫu vé',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'settingDate',
        header: 'Ngày cài đặt',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'settingUser',
        header: 'Người cài đặt',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: this.funcStyleClassStatus,
        funcLabelStatus: this.funcLabelStatus,
        posTextCell: EPositionTextCell.LEFT,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        class: 'b-border-frozen-right',
      },
      {
        field: '',
        header: '',
        width: '3rem',
        type: ETypeDataTable.ACTION,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        hideBtnSetColumn: true,
      },
    ];
    this.setPage();
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  private genListAction() {
    this.listAction = this.dataSource.map((data: EventOverviewDetailTicketModel, index: number) => {
      const actions: IActionTable[] = [];
      
      if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_CapNhat)) { 
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_XemThuMau)) { 
        actions.push({
          data: data,
          label: 'Xem thử mẫu',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.view($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_TaiMauVe)) { 
        actions.push({
          data: data,
          label: 'Tải mẫu vé',
          icon: 'pi pi-download',
          command: ($event) => {
            this.download($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_DoiTrangThai)) { 
        if(data.status === EventOverview.KICH_HOAT) {
          actions.push({
            data: data,
            label: 'Hủy kích hoạt',
            icon: 'pi pi-times',
            command: ($event) => {
              this.deactivate($event.item.data);
            },
          });
        }
       if(data.status === EventOverview.HUY_KICH_HOAT) {
        actions.push({
          data: data,
          label: 'Kích hoạt',
          icon: 'pi pi-check',
          command: ($event) => {
            this.activate($event.item.data);
          },
        });
       }
      }
      
      return actions;
    });
  }

  public detail(data: EventOverviewDetailTicketModel) {
    if (data.id) {
      const dataSource = { ...data, label: 'vé' };
      this.dialogService.open(CrudEventDetailTemplateComponent, {
        header: 'Xem chi tiết mẫu vé',
        width: '600px',
        baseZIndex: 10000,
        data: {
          dataSource: dataSource,
          type: EConfigDataModal.VIEW,
        } as IConfigDataModal,
      });
    }
  }

  public edit(data: EventOverviewDetailTicketModel) {
    if (data.id) {
      const dataSource = {
        ...data,
        label: 'vé',
        apiCrud: this.eventOverviewService.createOrEditTicketOfEvent.bind(this.eventOverviewService),
      };
      const ref = this.dialogService.open(CrudEventDetailTemplateComponent, {
        header: 'Chỉnh sửa mẫu vé',
        width: '600px',
        baseZIndex: 10000,
        data: {
          dataSource: dataSource,
          type: EConfigDataModal.EDIT,
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Chỉnh sửa thành công',
            life: 1500,
          });
          this.setPage();
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      if (this.dataSource.some((e: EventOverviewDetailTicketModel) => e.status === EventOverview.KICH_HOAT)) {
        const refConfirm = this.dialogService.open(FormNotificationComponent, {
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
            title: 'Mẫu vé hiện tại sẽ bị Hủy kích hoạt khi thêm mới mẫu vé khác.',
            icon: FormNotificationConst.IMAGE_APPROVE,
          } as INotiDataModal,
        });
        refConfirm.onClose.subscribe((dataCallBack) => {
          if (dataCallBack?.accept) {
            const ref = this.dialogService.open(CrudEventDetailTemplateComponent, {
              header: 'Thêm mới mẫu vé',
              width: '600px',
              baseZIndex: 10000,
              data: {
                dataSource: {
                  label: 'vé',
                  apiCrud: this.eventOverviewService.createOrEditTicketOfEvent.bind(this.eventOverviewService),
                },
                type: EConfigDataModal.CREATE,
              } as IConfigDataModal,
            });
            ref.onClose.subscribe((response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.messageService.add({
                  severity: 'success',
                  summary: '',
                  detail: 'Thêm mới thành công',
                  life: 1500,
                });
                this.setPage();
              }
            });
          }
        });
      } else {
        const ref = this.dialogService.open(CrudEventDetailTemplateComponent, {
          header: 'Thêm mới mẫu vé',
          width: '600px',
          baseZIndex: 10000,
          data: {
            dataSource: {
              label: 'vé',
              apiCrud: this.eventOverviewService.createOrEditTicketOfEvent.bind(this.eventOverviewService),
            },
            type: EConfigDataModal.CREATE,
          } as IConfigDataModal,
        });
        ref.onClose.subscribe((response) => {
          if (this.handleResponseInterceptor(response, '')) {
            this.messageService.add({
              severity: 'success',
              summary: '',
              detail: 'Thêm mới thành công',
              life: 1500,
            });
            this.setPage();
          }
        });
      }
    }
  }

  public deactivate(data: EventOverviewDetailTicketModel) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn hủy kích hoạt?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusTicketOfEvent(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Không hủy kích hoạt được mẫu vé`);
            }
          );
        }
      });
    }
  }

  public activate(data: EventOverviewDetailTicketModel) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn kích hoạt?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusTicketOfEvent(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Kích hoạt không thành công`);
            }
          );
        }
      });
    }
  }

  public view(data: EventOverviewDetailTicketModel) {
    if (data) {
      this.spinnerService.isShow = true;
      this.eventOverviewService.viewFileTicketPDF(data.id).subscribe(
        (res: any) => {
          this.spinnerService.isShow = false;
        },
        (err: any) => {
          this.spinnerService.isShow = false;
        }
      );
    }
  }

  public download(data: EventOverviewDetailTicketModel) {
    if (data) {
      this.eventOverviewService.downloadFileTicketWord(data.id).subscribe();
    }
  }

  public setPage() {
    this.isLoading = true;

    this.eventOverviewService.findAllTicketOfEvent().subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  name: item.name,
                  link: item.fileUrl,
                  settingDate: item.createdDate ? formatDate(item.createdDate, ETypeFormatDate.DATE) : '',
                  settingUser: item.createdBy,
                  status: item.status,
                } as EventOverviewDetailTicketModel)
            );
            this.genListAction();
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }
}
