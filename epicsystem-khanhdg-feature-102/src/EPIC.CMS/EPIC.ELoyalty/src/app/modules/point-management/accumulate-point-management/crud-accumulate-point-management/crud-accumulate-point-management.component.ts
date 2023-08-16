import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { AccumulatePointManegement, EConfigDataModal, VoucherManagement } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { CrudAccumulatePointManagement } from '@shared/interface/point-management/accumulate-point-management/AccumulatePointManegement.model';
import { AccumulatePointManagementService } from '@shared/services/accumulate-point-management-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-accumulate-point-management',
  templateUrl: './crud-accumulate-point-management.component.html',
  styleUrls: ['./crud-accumulate-point-management.component.scss'],
})
export class CrudAccumulatePointManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private accumulatePointManagementService: AccumulatePointManagementService
  ) {
    super(injector, messageService);
  }
  public crud: CrudAccumulatePointManagement = new CrudAccumulatePointManagement();
  public initCusomter: any = undefined;
  public type = EConfigDataModal.CREATE;
  public listReason: IDropdown[] = [];
  public listReasonOfType: IDropdown[] = [];

  public get listType() {
    return AccumulatePointManegement.listType;
  }

  ngOnInit() {
    this.getListReasonOfTpe();
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
      this.crud.createDate = this.accumulatePointManagementService.currentTime;
      this.crud.createUser = this.accumulatePointManagementService.currentUser;
    } else {
      this.crud.mapData(this.config.data.dataSource);
      this.initCusomter = {
        type: VoucherManagement.CUSTOMER.INDIVIDUAL,
        customerId: this.crud.individualId,
      };
    }
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public onChangeType(event: any) {
    if (event) {
      this.getListReason();
      this.crud.reason = undefined;
    }
  }

  public onChangeSearchCustomer(event) {
    if (event) {
      this.crud.individualId = event.individualId;
      this.crud.businessId = event.businessId;
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.crud.isValidData()) {
        this.accumulatePointManagementService
          .createOrEditVoucher(this.crud.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        const messageError = this.crud.dataValidator.length ? this.crud.dataValidator[0].message : undefined;
        messageError && this.messageError(messageError);
      }
    }
  }

  private getListReasonOfTpe() {
    this.accumulatePointManagementService.getAllReasons().subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        if (res.data && res.data.length) {
          this.listReasonOfType = res.data.map(
            (e: any) =>
              ({
                value: e.value,
                label: e.label,
                rawData: {
                  type: e.type,
                },
              } as IDropdown)
          );
          this.type !== EConfigDataModal.CREATE && this.getListReason();
        }
      }
    });
  }

  private getListReason() {
    this.listReason = this.listReasonOfType.filter(
      (e: IDropdown) => e.rawData.type === this.crud.type || !e.rawData.type
    );
  }
}
