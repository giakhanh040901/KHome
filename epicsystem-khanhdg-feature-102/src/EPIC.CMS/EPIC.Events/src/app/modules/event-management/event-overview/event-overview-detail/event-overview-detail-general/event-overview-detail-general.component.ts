import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EventOverview, FormNotificationConst, YES_NO } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDropdown, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { EventOverviewDetailGeneralModel, TicketInspectorModel } from '@shared/interface/event-management/event-overview/EventOverviewDetailGeneral.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { SpinnerService } from '@shared/services/spinner.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'event-overview-detail-general',
  templateUrl: './event-overview-detail-general.component.html',
  styleUrls: ['./event-overview-detail-general.component.scss'],
})
export class EventOverviewDetailGeneralComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private dialogService: DialogService,
    private spinnerService: SpinnerService
  ) {
    super(injector, messageService);
  }
  public dto: EventOverviewDetailGeneralModel = new EventOverviewDetailGeneralModel();
  public listProvince: IDropdown[] = [];
  public listAccountMoney: IDropdown[] = [];
  public listContractCode: IDropdown[] = [];
  public isEdit: boolean = false;
  public customers: TicketInspectorModel[] = [];

  public get isDisable() {
    return !this.isEdit;
  }

  public get listTypeEvent() {
    return EventOverview.listTypeEvent;
  }

  public get listEventViewer() {
    return EventOverview.listEventViewer;
  }

  public get YES_NO() {
    return YES_NO;
  }

  ngOnInit() {
    this.isEdit = this.eventOverviewService.isEdit;
  }

  ngAfterViewInit() {
    this.getEventDetail();

    this.eventOverviewService._listProvince$.subscribe((res: IDropdown[]) => {
      this.listProvince = res;
    });

    this.eventOverviewService._listAccountMoney$.subscribe((res: IDropdown[]) => {
      this.listAccountMoney = res;
    });

    this.eventOverviewService._listContractCode$.subscribe((res: IDropdown[]) => {
      this.listContractCode = res;
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public edit(event: any) {
    if (event) {
      this.isEdit = true;
    }
  }

  public save(event: any) {
    if (event) {
      if (this.dto.isValidData()) {
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
            title: 'Bạn có chắc chắn lưu dữ liệu?',
            icon: FormNotificationConst.IMAGE_APPROVE,
          } as INotiDataModal,
        });
        ref.onClose.subscribe((dataCallBack) => {
          if (dataCallBack?.accept) {
            this.eventOverviewService.createOrEdit(this.dto.toObjectSendToAPI(), this.isEdit).subscribe(
              (response) => {
                if (this.handleResponseInterceptor(response, 'Lưu thành công')) {
                  this.isEdit = false;
                  this.getEventDetail();
                }
              },
              (err) => {
                this.messageError(`Lưu dữ liệu không thành công`);
              }
            );
          }
        });
      } else {
        this.messageDataValidator(this.dto.dataValidator);
      }
    }
  }

  private getEventDetail() {
    this.spinnerService.isShow = true;
    this.eventOverviewService.getEventDetail().subscribe(
      (res: any) => {
        this.spinnerService.isShow = false;
        if (res.data) {
          this.dto.mapDTO(res.data);
          if (this.dto.status === EventOverview.DANG_MO_BAN) {
            this.eventOverviewService.showBtnOpenSell = false;
          } else {
            this.eventOverviewService.showBtnOpenSell = this.dto.isValidData();
          }
        }
      },
      (err) => {
        this.spinnerService.isShow = false;
      }
    );
  }

  public upload(event: any) {
    if (event && event.files && event.files.length && event.files[0]) {
      const file = event.files[0];
      this.eventOverviewService.uploadFileEventDetail(file, 'event-overview').subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Tải file thành công')) {
            this.dto.policy = response.data;
          }
        },
        (err) => {
          this.messageError('Có sự cố khi tải file!');
        }
      );
    }
  }

  public remove(event: any) {
    if (event) {
      this.dto.policy = '';
    }
  }

  public removeElement(indexToRemove: number) {
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
      if (dataCallBack?.accept && indexToRemove >= 0 && indexToRemove < this.dto.ticketInspector.length) {
        this.dto.ticketInspector.splice(indexToRemove, 1);
      } else {
        this.messageError(
          "Xóa không thành công (FE)"
        );
      }

    });
  }

  public getInfoCustomer() {
    this.keyword = this.keyword.trim();
    this.eventOverviewService.getInfoInvestorCustomer(this.keyword).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, "")) {
          this.customers = res?.data?.items;
          const existingInvestorIds = this.dto.ticketInspector.map(item => item.investorId);
          if (this.customers && this.customers.length > 0 
                && this.customers[0].investorId && !existingInvestorIds.includes(this.customers[0].investorId)) {
            const customer = {
              investorId: this.customers[0].investorId,
              fullName: this.customers[0]?.defaultIdentification?.fullname,
              phone: this.customers[0].phone,
            } as TicketInspectorModel;
            this.dto.ticketInspector.push(customer);
            console.log("this.dto",this.dto);
          } else {
            this.messageError("Không tìm thấy dữ liệu");
          }
        }
      },
      (err) => {
        console.log("Error-------", err);
        this.isLoading = false;
      }
    );
  }
}
