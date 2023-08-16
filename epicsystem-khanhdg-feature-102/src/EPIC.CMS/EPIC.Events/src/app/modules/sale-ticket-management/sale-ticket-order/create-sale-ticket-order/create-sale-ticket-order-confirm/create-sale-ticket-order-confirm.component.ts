import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { SaleTicketOrder } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CreateSaleTicketOrderConfirm } from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-sale-ticket-order-confirm',
  templateUrl: './create-sale-ticket-order-confirm.component.html',
  styleUrls: ['./create-sale-ticket-order-confirm.component.scss'],
})
export class CreateSaleTicketOrderConfirmComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private dialogService: DialogService,
    private saleTicketOrderService: SaleTicketOrderService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
  }

  public dto: CreateSaleTicketOrderConfirm = new CreateSaleTicketOrderConfirm();

  public get CUSTOMER() {
    return SaleTicketOrder.CUSTOMER;
  }

  public get BUSINESS() {
    return SaleTicketOrder.BUSINESS;
  }

  ngOnInit() {
    this.mapDTO();
  }

  public back(event: any) {
    if (event) {
      this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order/create/event']);
    }
  }

  public done(event: any) {
    if (event) {
      this.saleTicketOrderService.createSaleTicketOrder(this.dto.toObjectSendToAPI()).subscribe((res: any) => {
        if (this.handleResponseInterceptor(res, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Thêm mới thành công',
            life: 1500,
          });
          if (res.data) {
            this.routerService.routerNavigate([
              '/sale-ticket-management/sale-ticket-order/edit/' + this.cryptEncode(res.data),
            ]);
          } else {
            this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order']);
          }
        }
      });
    }
  }

  private mapDTO() {
    const { customer, introduce, event } = this.saleTicketOrderService.selectedOrder;

    this.dto.customerId = customer.id;
    this.dto.customerIdenId = customer.idenId;
    this.dto.customerName = customer.name;
    this.dto.customerPhone = customer.phone;
    this.dto.customerEmail = customer.email;
    this.dto.customerIdNo = customer.idNo;
    this.dto.customerAddress = customer.addressName;
    this.dto.customerAddressId = customer.addressId;
    this.dto.customerAbbreviation = customer.abbreviation;
    this.dto.customerTaxCode = customer.taxCode;
    this.dto.customerType = customer.type;

    this.dto.eventName = event.eventName;
    this.dto.eventOrganize = event.organize;
    this.dto.eventType = event.eventType;
    this.dto.eventPlace = event.eventPlace;
    this.dto.eventProvince = event.eventProvince;
    this.dto.eventAddress = event.eventAddress;
    this.dto.ticketTime = event.ticketTime;
    this.dto.ticketTimeName = event.ticketTimeName;
    this.dto.ticketItems = event.ticketItems;

    this.dto.salerCode = introduce.code;
    this.dto.salerName = introduce.name;
    this.dto.salerPhone = introduce.phone;
    this.dto.salerEmail = introduce.email;
    this.dto.salerTransaction = introduce.transaction;
    this.dto.salerContractTransaction = introduce.contractTransaction;

    this.dto.isHardTicket = event.isHardTicket;
    this.dto.isRequestBill = event.isRequestBill;
  }
}
