import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EventOverview } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { CreateSaleTicketOrderEvent } from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { SaleTicketOrderItem } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailOverview.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'create-sale-ticket-order-event',
  templateUrl: './create-sale-ticket-order-event.component.html',
  styleUrls: ['./create-sale-ticket-order-event.component.scss'],
})
export class CreateSaleTicketOrderEventComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private dialogService: DialogService,
    public saleTicketOrderService: SaleTicketOrderService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
  }
  public listEvent: IDropdown[] = [];
  public listTicketTime: IDropdown[] = [];
  public selectedEvent: any;

  ngOnInit() {
    this.saleTicketOrderService.getEventActiveToOrder();
  }

  ngAfterViewInit() {
    this.saleTicketOrderService._listEventActiveToOrder$.subscribe((res: IDropdown[]) => {
      if (res) {
        this.listEvent = res;
      }
    });
    this.saleTicketOrderService._listTicketOfEvent$.subscribe((res: IDropdown[]) => {
      if (res) {
        this.listTicketTime = res;
        this.getDefaultData();
      }
    });
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  private getDefaultData() {
    if (!this.saleTicketOrderService.dtoEvent.ticketTime) {
      if (this.listTicketTime && this.listTicketTime.length) {
        this.saleTicketOrderService.dtoEvent.ticketTime = Number(this.listTicketTime[0].value);
        this.onChangeTicketTime(true);
      } else {
        this.mapSelectedTicketItems();
      }
    }
  }

  public onChangeEvent(event: any) {
    if (event) {
      const selectedEvent = this.listEvent.find(
        (e: IDropdown) => e.value === this.saleTicketOrderService.dtoEvent.event
      );
      this.saleTicketOrderService.dtoEvent.selectedEvent = selectedEvent?.rawData;
      this.saleTicketOrderService.dtoEvent.eventName = selectedEvent?.label || '';
      this.saleTicketOrderService.dtoEvent.ticketTime = undefined;
      this.saleTicketOrderService.getTicketOfEvent(this.saleTicketOrderService.dtoEvent.event);
      this.mapSelectedEvent();
    }
  }
  public onChangeTicketTime(event: any) {
    if (event) {
      this.saleTicketOrderService.dtoEvent.ticketTimeName =
        this.listTicketTime.find((e: IDropdown) => e.value === this.saleTicketOrderService.dtoEvent.ticketTime)
          ?.label || '';
      this.mapSelectedTicketItems();
    }
  }

  public onChangeOrderQuantity(event: any, index: number) {
    if (event) {
      this.saleTicketOrderService.dtoEvent.ticketItems[index].total =
        Number(this.saleTicketOrderService.dtoEvent.ticketItems[index].value) * event.value;
    }
  }

  public back(event: any) {
    if (event) {
      this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order/create/customer']);
    }
  }

  public continue(event: any) {
    if (event) {
      if (this.saleTicketOrderService.dtoEvent.isValidData()) {
        this.saleTicketOrderService.selectedOrder.event = {
          ...this.saleTicketOrderService.dtoEvent,
        } as CreateSaleTicketOrderEvent;
        this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order/create/confirm']);
      } else {
        this.messageDataValidator(this.saleTicketOrderService.dtoEvent.dataValidator);
      }
    }
  }

  public mapSelectedEvent() {
    const event = this.listEvent.find(
      (e: IDropdown) => e.value === this.saleTicketOrderService.dtoEvent.event
    )?.rawData;
    if (event) {
      this.saleTicketOrderService.dtoEvent.organize = event.organizator;
      this.saleTicketOrderService.dtoEvent.eventType =
        event.eventTypes && event.eventTypes.length
          ? event.eventTypes
              .map((d: number) => EventOverview.listTypeEvent.find((e: IDropdown) => e.value === d)?.label || '')
              .toString()
          : '';
      this.saleTicketOrderService.dtoEvent.eventPlace = event.location;
      this.saleTicketOrderService.dtoEvent.eventProvince = event.provinceName;
      this.saleTicketOrderService.dtoEvent.eventAddress = event.address;
    }
  }

  public mapSelectedTicketItems() {
    const ticket = this.listTicketTime.find(
      (e: IDropdown) => e.value === this.saleTicketOrderService.dtoEvent.ticketTime
    )?.rawData;
    if (ticket && ticket.tickets && ticket.tickets.length) {
      this.saleTicketOrderService.dtoEvent.ticketItems = ticket.tickets.map((e: any) => {
        let item: SaleTicketOrderItem = new SaleTicketOrderItem();
        item.ticketId = e.id;
        item.ticketType = e.name;
        item.description = e.description;
        item.currentQuantity = e.remainingTickets;
        item.value = e.price;
        item.orderQuantity = 0;
        item.total = 0;
        return item;
      });
    } else {
      this.saleTicketOrderService.dtoEvent.ticketItems = [];
    }
  }

  public get disableIsHardTicket() {
    return !this.saleTicketOrderService.dtoEvent.selectedEvent?.canExportTicket;
  }

  public get disableIsRequestBill() {
    return !this.saleTicketOrderService.dtoEvent.selectedEvent?.canExportRequestRecipt;
  }
}
