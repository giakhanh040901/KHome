import { SaleTicketOrder } from '@shared/AppConsts';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';
import { SaleTicketOrderItem } from './SaleTicketOrderDetailOverview.model';

export class CustomerSearchModel {
  public id: number;
  public idenId: number;
  public name: string;
  public phone: string;
  public idNo: string;
  public email: string;
  public addressId: number;
  public addressName: string;
  public abbreviation: string;
  public taxCode: string;
  public type: number;
}

export class IntroduceSearchModel {
  public id: number;
  public code: string;
  public name: string;
  public phone: string;
  public email: string;
  public transaction: string;
  public contractTransaction: string;
}

export class CreateSaleTicketOrderEvent {
  public selectedEvent: any;
  public event: number;
  public eventName: string;
  public organize: string;
  public eventType: string;
  public eventPlace: string;
  public eventProvince: string;
  public eventAddress: string;
  public ticketTime: number;
  public ticketTimeName: string;
  public ticketItems: SaleTicketOrderItem[] = [new SaleTicketOrderItem()];
  public isHardTicket: boolean;
  public isRequestBill: boolean;

  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidEvent() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'event');
    if (this.event) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'event',
      message: 'Vui lòng chọn Sự kiện',
    });
    return false;
  }

  private isValidTicketTime() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'ticketTime');
    if (this.ticketTime) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'ticketTime',
      message: 'Vui lòng chọn Thời gian tổ chức',
    });
    return false;
  }

  private isValidTicketItems() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'ticketItems');
    if (
      this.ticketItems &&
      this.ticketItems.length &&
      this.ticketItems.some((e: SaleTicketOrderItem) => !!e.orderQuantity)
    ) {
      let resultItem: boolean = true;
      let index: number = 0;
      while (index < this.ticketItems.length && !!resultItem) {
        const item = this.ticketItems[index];
        if (!!item.orderQuantity) {
          resultItem = item.isValidData(index, 'ticketItems');
          this._dataValidator.unshift(...item.dataValidator);
        }
        index++;
      }
      return resultItem;
    }
    this._dataValidator.unshift({
      name: 'ticketItems',
      message: 'Vui lòng chọn ít nhất một số lượng vé',
    });
    return false;
  }

  public isValidData() {
    return this.isValidEvent() && this.isValidTicketTime() && this.isValidTicketItems();
  }
}

export class CreateSaleTicketOrderConfirm {
  public customerId: number;
  public customerIdenId: number;
  public customerName: string;
  public customerPhone: string;
  public customerEmail: string;
  public customerIdNo: string;
  public customerAddress: string;
  public customerAddressId: number;
  public customerAbbreviation: string;
  public customerTaxCode: string;
  public customerType: number = SaleTicketOrder.CUSTOMER;
  public eventName: string;
  public eventOrganize: string;
  public eventType: string;
  public eventPlace: string;
  public eventProvince: string;
  public eventAddress: string;
  public ticketTime: number;
  public ticketTimeName: string;
  public ticketItems: SaleTicketOrderItem[] = [new SaleTicketOrderItem()];
  public salerCode: string;
  public salerName: string;
  public salerPhone: string;
  public salerEmail: string;
  public salerTransaction: string;
  public salerContractTransaction: string;
  public isHardTicket: boolean;
  public isRequestBill: boolean;

  public toObjectSendToAPI() {
    const listTicketItems = this.ticketItems.filter((e: SaleTicketOrderItem) => !!e.orderQuantity);
    return {
      investorId: this.customerId,
      investorIdenId: this.customerIdenId,
      contractAddressId: this.customerAddressId,
      saleReferralCode: this.salerCode,
      eventDetailId: this.ticketTime,
      orderDetails: listTicketItems.map((e: SaleTicketOrderItem) => ({
        ticketId: e.ticketId,
        price: e.value,
        quantity: e.orderQuantity,
      })),
      isReceiveHardTicket: !!this.isHardTicket,
      isRequestReceiveRecipt: !!this.isRequestBill,
    };
  }
}
