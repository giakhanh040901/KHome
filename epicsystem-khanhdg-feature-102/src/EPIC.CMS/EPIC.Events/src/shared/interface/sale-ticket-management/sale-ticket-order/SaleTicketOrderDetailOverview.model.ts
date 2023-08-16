import { ETypeFormatDate, EventOverview } from '@shared/AppConsts';
import { formatDate } from '@shared/function-common';
import { IDataValidator, IDropdown } from '@shared/interface/InterfaceConst.interface';

export class SaleTicketOrderDetailOverviewModel {
  public id: number;
  public customerId: number;
  public customerIdenId: number;
  public customerName: string = '';
  public customerPhone: string = '';
  public customerEmail: string = '';
  public customerIdNo: string = '';
  public customerAddress: string = '';
  public customerAddressId: number;
  public eventName: string = '';
  public eventOrganize: string = '';
  public eventType: string = '';
  public eventPlace: string = '';
  public eventProvince: string = '';
  public eventAddress: string = '';
  public orderRequestCode: string = '';
  public orderCreateDate: string = '';
  public orderApproveDate: string = '';
  public ticketTimeId: number;
  public ticketTime: any = '';
  public ticketInfor: SaleTicketOrderItem[] = [];
  public isHardTicket: boolean;
  public isRequestBill: boolean;
  public saleCode: string = '';
  public saleName: string = '';
  public salePhone: string = '';
  public saleEmail: string = '';
  public transactionRoom: string = '';
  public transactionRoomCommand: string = '';
  public status: number;

  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapDTO(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.customerId = dto.investorId;
      this.customerIdenId = dto.investorIdenId;
      this.customerName = dto.fullname;
      this.customerPhone = dto.phone;
      this.customerEmail = dto.email;
      this.customerIdNo = dto.idNo;
      this.customerAddress = dto.contractAddressName;
      this.customerAddressId = dto.contractAddressId;
      this.eventName = dto.eventName;
      this.eventOrganize = dto.organizator;
      this.eventType =
        dto.types && dto.types.length
          ? dto.types
              .map((d: number) => EventOverview.listTypeEvent.find((e: IDropdown) => e.value === d)?.label || '')
              .toString()
          : '';
      this.eventPlace = dto.location;
      this.eventProvince = dto.province;
      this.eventAddress = dto.address;
      this.orderRequestCode = dto.contractCode;
      this.orderCreateDate = dto.createdDate ? formatDate(dto.createdDate, ETypeFormatDate.DATE_TIME) : '';
      this.orderApproveDate = dto.approveDate ? formatDate(dto.approveDate, ETypeFormatDate.DATE_TIME) : '';
      this.ticketTimeId = dto.id;
      this.ticketTime = `${formatDate(dto.startDate, ETypeFormatDate.DATE_TIME)} - ${formatDate(
        dto.endDate,
        ETypeFormatDate.DATE_TIME
      )}`;
      this.ticketInfor =
        dto.orderDetails && dto.orderDetails.length
          ? dto.orderDetails.map((e: any) => {
              let item: SaleTicketOrderItem = new SaleTicketOrderItem();
              item.id = e.id;
              item.ticketId = e.ticketId;
              item.ticketType = e.name;
              item.description = e.depscription;
              item.currentQuantity = e.currentTickets;
              item.value = e.price;
              item.orderQuantity = e.quantity;
              item.total = Number(e.price) * Number(e.quantity);
              return item;
            })
          : [];
      this.isHardTicket = dto.isReceiveHardTicket;
      this.isRequestBill = dto.isRequestReceiveRecipt;
      this.saleCode = dto.saleInfo && dto.saleInfo.referralCode ? dto.saleInfo.referralCode : '';
      this.saleName = dto.saleInfo && dto.saleInfo.saleName ? dto.saleInfo.saleName : '';
      this.salePhone = dto.saleInfo && dto.saleInfo.salePhone ? dto.saleInfo.salePhone : '';
      this.saleEmail = dto.saleInfo && dto.saleInfo.saleEmail ? dto.saleInfo.saleEmail : '';
      this.transactionRoom = dto.saleInfo && dto.saleInfo.departmentName ? dto.saleInfo.departmentName : '';
      this.transactionRoomCommand =
        dto.saleInfo && dto.saleInfo.managerDepartmentName ? dto.saleInfo.managerDepartmentName : '';
      this.status = dto.status;
    }
  }

  private isValidTicketInfor() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'ticketInfor');
    if (
      this.ticketInfor &&
      this.ticketInfor.length &&
      this.ticketInfor.some((e: SaleTicketOrderItem) => !!e.orderQuantity)
    ) {
      let resultItem: boolean = true;
      let index: number = 0;
      while (index < this.ticketInfor.length && !!resultItem) {
        const item = this.ticketInfor[index];
        if (!!item.orderQuantity) {
          resultItem = item.isValidData(index, 'ticketInfor');
          this._dataValidator.unshift(...item.dataValidator);
        }
        index++;
      }
      return resultItem;
    }
    this._dataValidator.unshift({
      name: 'ticketInfor',
      message: 'Vui lòng chọn ít nhất một số lượng vé',
    });
    return false;
  }

  public isValidData() {
    return this.isValidTicketInfor();
  }

  public toObjectSendToAPI() {
    const listTicketItems = this.ticketInfor.filter((e: SaleTicketOrderItem) => !!e.orderQuantity);
    return {
      id: this.id,
      investorId: this.customerId,
      investorIdenId: this.customerIdenId,
      contractAddressId: this.customerAddressId,
      saleReferralCode: this.saleCode,
      eventDetailId: this.ticketTimeId,
      orderDetails: listTicketItems.map((e: SaleTicketOrderItem) => ({
        id: e.id,
        ticketId: e.ticketId,
        price: e.value,
        quantity: e.orderQuantity,
      })),
      isReceiveHardTicket: !!this.isHardTicket,
      isRequestReceiveRecipt: !!this.isRequestBill,
    };
  }
}

export class SaleTicketOrderItem {
  public id?: number;
  public ticketId: number;
  public ticketType: string = '';
  public description: string = '';
  public currentQuantity: number = 0;
  public value: number = 0;
  public orderQuantity: number = 0;
  public total: number = 0;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidValue(index: number, name: string) {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== name);
    if (Number(this.orderQuantity) > Number(this.currentQuantity)) {
      this._dataValidator.unshift({
        name: name,
        message: 'Số lượng vé đặt không thể lớn hơn Số lượng vé hiện tại của vé thứ ' + (index + 1),
      });
      return false;
    }
    return true;
  }

  public isValidData(index: number, name: string) {
    return this.isValidValue(index, name);
  }
}
