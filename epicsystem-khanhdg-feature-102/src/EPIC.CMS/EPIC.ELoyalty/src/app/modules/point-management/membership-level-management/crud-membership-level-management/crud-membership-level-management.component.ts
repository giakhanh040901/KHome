import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { EConfigDataModal } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem } from '@shared/function-common';
import { CrudMembershipLevelManagement } from '@shared/interface/point-management/membership-level-management/MembershipLevelManagement.model';
import { MembershipLevelManagementService } from '@shared/services/membership-level-management-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-membership-level-management',
  templateUrl: './crud-membership-level-management.component.html',
  styleUrls: ['./crud-membership-level-management.component.scss'],
})
export class CrudMembershipLevelManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private membershipLevelManagementService: MembershipLevelManagementService
  ) {
    super(injector, messageService);
  }

  public crud: CrudMembershipLevelManagement = new CrudMembershipLevelManagement();
  public type = EConfigDataModal.CREATE;

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
      this.crud.createDate = this.membershipLevelManagementService.currentTime;
      this.crud.createUser = this.membershipLevelManagementService.currentUser;
      this.crud.applyDate = new Date();
    } else {
      this.crud.mapData(this.config.data.dataSource);
    }
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.crud.isValidData()) {
        this.crud.applyDate && (this.crud.applyDate = formatCalendarItem(this.crud.applyDate));
        this.crud.deactiveDate && (this.crud.deactiveDate = formatCalendarItem(this.crud.deactiveDate));
        this.membershipLevelManagementService
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
}
