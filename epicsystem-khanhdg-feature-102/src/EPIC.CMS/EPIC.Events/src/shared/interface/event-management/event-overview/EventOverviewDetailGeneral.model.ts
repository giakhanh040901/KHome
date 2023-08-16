import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class TicketInspectorModel {
  public investorId: number;
  public fullName: string;
  public phone: string;
  public defaultIdentification?: DefaultIdentificationModel;
}

export class DefaultIdentificationModel {
  public fullname: string;
}

export class EventOverviewDetailGeneralModel {
  public id: number;
  public eventName: string = '';
  public organize: string = '';
  public type: number[] = [];
  public placeName: string = '';
  public province: string;
  public address: string = '';
  public longitude?: string = '';
  public latitude?: string = '';
  public eventViewer?: number;
  public contractCode?: number;
  public accountMoney: number[] = [];
  public policy?: string = '';
  public website?: string = '';
  public facebook?: string = '';
  public phone: string = '';
  public email?: string = '';
  public isShowApp?: boolean;
  public isHighlight?: boolean;
  public isRequestTicket?: boolean;
  public isRequestBill?: boolean;
  public contentType?: string = '';
  public content?: string = '';
  public status: number = 0;
  public ticketInspector: TicketInspectorModel[] = [new TicketInspectorModel()];
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapDTOTicketInspector(dto: any) {
    if (dto) {
      this.ticketInspector = dto.map(
        (item: any) =>
          ({
            investorId: item.investorId,
            fullName: item?.defaultIdentification?.fullname,
            phone: item.phone,
          } as TicketInspectorModel)
      );
     
    }
  }

  public mapDTO(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.eventName = dto.name;
      this.organize = dto.organizator;
      this.type = dto.eventTypes;
      this.placeName = dto.location;
      this.province = dto.provinceCode;
      this.address = dto.address;
      this.longitude = dto.longitude;
      this.latitude = dto.latitude;
      this.eventViewer = dto.viewing;
      this.contractCode = dto.configContractCodeId || undefined;
      this.accountMoney = dto.tradingBankAccounts && dto.tradingBankAccounts.length ? dto.tradingBankAccounts : [];
      this.policy = dto.ticketPurchasePolicy;
      this.website = dto.website;
      this.facebook = dto.facebook;
      this.phone = dto.phone;
      this.email = dto.email;
      this.isShowApp = dto.isShowApp;
      this.isHighlight = dto.isHighlight;
      this.isRequestTicket = dto.canExportTicket;
      this.isRequestBill = dto.canExportRequestRecipt;
      this.contentType = dto.contentType;
      this.content = dto.overviewContent;
      this.status = dto.status;
      this.ticketInspector = (dto.ticketInspector && dto.ticketInspector?.length) ? dto.ticketInspector : [];
    }
  }

  public mapDTODescription(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.contentType = dto.contentType;
      this.content = dto.overviewContent || '';
    }
  }

  private isValidTicketInspector() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'ticketInspector');
    if (this.ticketInspector && this.ticketInspector.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'ticketInspector',
      message: 'Vui lòng nhập thông tin nhân viên soát vé',
    });
    return false;
  }

  private isValidEventName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'eventName');
    if (this.eventName && this.eventName.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'eventName',
      message: 'Vui lòng nhập thông tin Tên sự kiện',
    });
    return false;
  }

  private isValidOrganize() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'organize');
    if (this.organize && this.organize.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'organize',
      message: 'Vui lòng nhập thông tin Ban tổ chức',
    });
    return false;
  }

  private isValidType() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'type');
    if (this.type && this.type.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'type',
      message: 'Vui lòng chọn thông tin Loại hình sự kiện',
    });
    return false;
  }

  private isValidPlaceName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'placeName');
    if (this.placeName && this.placeName.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'placeName',
      message: 'Vui lòng nhập thông tin Địa điểm tổ chức',
    });
    return false;
  }

  private isValidProvince() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'province');
    if (this.province) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'province',
      message: 'Vui lòng chọn thông tin Tỉnh/ Thành phố',
    });
    return false;
  }

  private isValidAddress() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'address');
    if (this.address && this.address.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'address',
      message: 'Vui lòng nhập thông tin Địa chỉ',
    });
    return false;
  }

  private isValidAccountMoney() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'accountMoney');
    if (this.accountMoney && this.accountMoney.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'accountMoney',
      message: 'Vui lòng chọn thông tin Tài khoản nhận tiền',
    });
    return false;
  }

  private isValidPhone() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'phone');
    if (this.phone) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'phone',
      message: 'Vui lòng nhập thông tin Điện thoại',
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidEventName() &&
      this.isValidOrganize() &&
      this.isValidType() &&
      this.isValidPlaceName() &&
      this.isValidProvince() &&
      this.isValidAddress() &&
      this.isValidAccountMoney() &&
      this.isValidPhone()
    );
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      name: this.eventName,
      organizator: this.organize,
      eventTypes: this.type,
      location: this.placeName,
      provinceCode: this.province,
      address: this.address,
      latitude: this.latitude,
      longitude: this.longitude,
      viewing: this.eventViewer,
      configContractCodeId: this.contractCode,
      tradingBankAccounts: this.accountMoney,
      ticketPurchasePolicy: this.policy,
      website: this.website,
      facebook: this.facebook,
      phone: this.phone.toString(),
      email: this.email,
      isShowApp: this.isShowApp,
      isHighlight: this.isHighlight,
      canExportTicket: this.isRequestTicket,
      canExportRequestRecipt: this.isRequestBill,
      ticketInspectorIds: this.ticketInspector.map(e => e.investorId),
    };
  }
}
