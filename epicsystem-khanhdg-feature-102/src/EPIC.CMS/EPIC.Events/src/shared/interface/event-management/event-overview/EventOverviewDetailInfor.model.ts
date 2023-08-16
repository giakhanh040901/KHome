import { COMPARE_TYPE, EventOverview, MARKDOWN_OPTIONS, YES_NO } from '@shared/AppConsts';
import {
  compareDate,
  convertTimeToAPI,
  convertTimesToHHMM,
  formatCalendarItem,
  formatCurrency,
  formatUrlImage,
} from '@shared/function-common';
import { IDataValidator, IImage } from '@shared/interface/InterfaceConst.interface';

export class EventOverviewDetailInforModel {
  public id: number;
  public startDate: string;
  public endDate: string;
  public typeTicketNum: number;
  public ticketNum: number;
  public registerNum: number;
  public payNum: number;
  public settingUser: string;
  public status: number;
}

export class CrudEventDetailInfor {
  public id?: number = undefined;
  public startTime: any = '';
  public endTime: any = '';
  public waitPay: boolean = EventOverview.CO;
  public waitTime: string = '';
  public showRestTicket: boolean = EventOverview.CO;

  public detailInforItem: EventDetailInforItem[] = [];
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.startTime = dto.startDate ? new Date(dto.startDate) : '';
      this.endTime = dto.endDate ? new Date(dto.endDate) : '';
      this.waitPay = !!dto.paymentWaitingTime;
      this.waitTime = !!dto.paymentWaitingTime ? convertTimesToHHMM(dto.paymentWaitingTime) : '';
      (this.showRestTicket = !!dto.isShowRemaingTicketApp),
        (this.detailInforItem = this.mapDataDetailInforItem(dto.tickets));
    }
  }

  public mapDataDetailInforItem(items: any[]) {
    if (items && items.length) {
      return items.map(
        (e: any) =>
          ({
            id: e.id,
            name: e.name,
            price: e.price ? formatCurrency(e.price) : 0,
            total: e.quantity ? formatCurrency(e.quantity) : 0,
            register: e.registerQuantity,
            pay: e.payQuantity,
            isFree: e.isFree,
            isShowApp: e.isShowApp === YES_NO.YES,
            status: e.status,
          } as EventDetailInforItem)
      );
    }
    return [];
  }

  private isValidStartTime() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'startTime');
    if (this.startTime) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'startTime',
      message: 'Vui lòng chọn thông tin Thời gian bắt đầu',
    });
    return false;
  }

  private isValidEndTime() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'endDate');
    if (!this.endTime) {
      this._dataValidator.unshift({
        name: 'endDate',
        message: 'Vui lòng chọn thông tin Thời gian kết thúc',
      });
      return false;
    } else if (compareDate(this.startTime, this.endTime, COMPARE_TYPE.GREATER)) {
      this._dataValidator.unshift({
        name: 'endTime',
        message: 'Ngày bắt đầu không thể lớn hơn Ngày kết thúc',
      });
      return false;
    }
    return true;
  }

  private isValidWaitTime() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'waitTime');
    if (this.waitPay === EventOverview.CO && !this.waitTime) {
      this._dataValidator.unshift({
        name: 'waitTime',
        message: 'Vui lòng nhập thông tin Thời gian',
      });
      return false;
    }
    return true;
  }

  public isValidData() {
    return this.isValidStartTime() && this.isValidEndTime() && this.isValidWaitTime();
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      startDate: this.startTime ? formatCalendarItem(this.startTime) : undefined,
      endDate: this.endTime ? formatCalendarItem(this.endTime) : undefined,
      paymentWaittingTime: this.waitPay !== EventOverview.CO ? undefined : convertTimeToAPI(this.waitTime),
      isShowRemaingTicketApp: this.showRestTicket,
    };
  }
}

export class EventDetailInforItem {
  public id: number;
  public name: string;
  public price: string;
  public total: number;
  public register: number;
  public pay: number;
  public isFree: boolean;
  public isShowApp: boolean;
  public status: number;
}

export class ReplicateTicketInfor {
  public selectTime: number | undefined = undefined;
  public listTicket: ReplicateTicketInforItem[] = [];
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidListTicket() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'listTicket');
    if (this.listTicket && this.listTicket.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'name',
      message: 'Vui lòng chọn thông tin vé',
    });
    return false;
  }

  public isValidData() {
    return this.isValidListTicket();
  }
}

export class ReplicateTicketInforItem {
  public id: number;
  public isSelected: boolean = false;
  public name: string;
  public price: string;
  public total: number;
  public minimum: number;
  public maximum: number;
  public isFree: boolean;
  public isShowApp: boolean;
}

export class CrudTicketInforItem {
  public id?: number = undefined;
  public name: string = '';
  public isFree: boolean = false;
  public price: number = 0;
  public total: number = 0;
  public minimum?: number = 0;
  public maximum?: number = 0;
  public startTime: any = '';
  public endTime: any = '';
  public description: string = '';
  public contentType?: string = MARKDOWN_OPTIONS.MARKDOWN;
  public content?: string = '';
  public listImage: IImage[] = [];
  public isShowApp: string;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.name = dto.name;
      this.isFree = dto.isFree;
      this.price = dto.price;
      this.total = dto.quantity;
      this.minimum = dto.minBuy;
      this.maximum = dto.maxBuy;
      this.startTime = dto.startSellDate ? new Date(dto.startSellDate) : '';
      this.endTime = dto.endSellDate ? new Date(dto.endSellDate) : '';
      this.description = dto.description;
      this.contentType = dto.contentType;
      this.content = dto.overviewContent;
      this.isShowApp = dto.isShowApp;
      this.listImage =
        dto.urlImages && dto.urlImages.length
          ? dto.urlImages.map((image: any) => ({ src: image.urlImage, id: image.id } as IImage))
          : [];
    }
  }

  private isValidName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'name');
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'name',
      message: 'Vui lòng nhập thông tin Tên loại vé',
    });
    return false;
  }

  private isValidPrice() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'price');
    if (this.isFree || (!this.isFree && this.price)) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'price',
      message: 'Vui lòng nhập thông tin Giá vé',
    });
    return false;
  }

  private isValidTotal() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'total');
    if (this.total) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'total',
      message: 'Vui lòng nhập thông tin Số lượng vé',
    });
    return false;
  }

  private isValidMinimum() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'minimum');
    if (this.minimum && Number(this.minimum) >= Number(this.total)) {
      this._dataValidator.unshift({
        name: 'minimum',
        message: 'Số vé tối thiểu phải nhỏ hơn Số lượng vé',
      });
      return false;
    }
    return true;
  }

  private isValidMaximum() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'maximum');
    if (this.maximum) {
      if (Number(this.maximum) >= Number(this.total)) {
        this._dataValidator.unshift({
          name: 'maximum',
          message: 'Số vé tối đa nhỏ hơn Số lượng vé',
        });
        return false;
      } else if (this.minimum && Number(this.minimum) > Number(this.maximum)) {
        this._dataValidator.unshift({
          name: 'maximum',
          message: 'Số vé tối đa không được nhỏ hơn Số vé tối thiểu',
        });
        return false;
      }
      return true;
    }
    return true;
  }

  private isValidStartTime() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'startTime');
    if (this.startTime) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'startTime',
      message: 'Vui lòng nhập thông tin Thời gian bắt đầu bán',
    });
    return false;
  }

  private isValidEndTime() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'endTime');
    if (this.endTime) {
      if (compareDate(new Date(this.endTime), new Date(this.startTime), COMPARE_TYPE.GREATER)) {
        return true;
      } else {
        this._dataValidator.unshift({
          name: 'endTime',
          message: 'Thời gian kết thúc phải lớn hơn Thời gian bắt đầu',
        });
        return false;
      }
    }
    this._dataValidator.unshift({
      name: 'endTime',
      message: 'Vui lòng nhập thông tin Thời gian kết thúc bán',
    });
    return false;
  }

  private isValidDescription() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'description');
    if (this.description && this.description.length) {
      return true;
    }
    debugger;
    this._dataValidator.unshift({
      name: 'description',
      message: 'Vui lòng nhập thông tin Mô tả ngắn',
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidName() &&
      this.isValidPrice() &&
      this.isValidTotal() &&
      this.isValidMinimum() &&
      this.isValidMaximum() &&
      this.isValidStartTime() &&
      this.isValidEndTime() &&
      this.isValidDescription()
    );
  }

  public toObjectSendToAPI(eventDetailInforId: number, imgBackground: string) {
    const images = this.listImage.filter((image: IImage) => image.src !== imgBackground);
    return {
      id: this.id,
      eventDetailId: eventDetailInforId,
      name: this.name,
      isFree: this.isFree,
      price: this.price,
      quantity: this.total,
      minBuy: this.minimum,
      maxBuy: this.maximum,
      startSellDate: this.startTime ? formatCalendarItem(this.startTime) : undefined,
      endSellDate: this.endTime ? formatCalendarItem(this.endTime) : undefined,
      description: this.description,
      contentType: this.contentType,
      overviewContent: this.content,
      urlImages: images.map((image: IImage) => ({
        id: image.id,
        urlImage: formatUrlImage(image.src),
      })),
      isShowApp: this.isShowApp,
    };
  }
}
