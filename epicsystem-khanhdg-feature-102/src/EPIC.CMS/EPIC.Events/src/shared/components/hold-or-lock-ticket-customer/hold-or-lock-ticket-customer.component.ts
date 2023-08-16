import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { HoldOrLockTicketCustomerModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/HoldOrLockTicketCustomer.model';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'hold-or-lock-ticket-customer',
  templateUrl: './hold-or-lock-ticket-customer.component.html',
  styleUrls: ['./hold-or-lock-ticket-customer.component.scss'],
})
export class HoldOrLockTicketCustomerComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef
  ) {
    super(injector, messageService);
  }
  public dto: HoldOrLockTicketCustomerModel = new HoldOrLockTicketCustomerModel();

  ngOnInit() {
    if (this.config.data) {
      this.dto = this.config.data.dataSource;
      if (this.dto.listReason && this.dto.listReason.length) {
        this.dto.reason = Number(this.dto.listReason[0].value);
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event: any) {
    if (event) {
      if (this.dto.isValidData()) {
        const body = this.dto.mapToObject(this.dto);
        this.dto.service(body).subscribe(
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
