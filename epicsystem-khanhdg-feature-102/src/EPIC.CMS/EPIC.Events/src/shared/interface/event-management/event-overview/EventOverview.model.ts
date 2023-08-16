import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class EventOverviewModel {
  public id: number;
  public name: string;
  public organize: string;
  public type: number[];
  public startDate: string;
  public endDate: string;
  public ticket: number;
  public register: number;
  public valid: number;
  public rest: number;
  public join: number;
  public settingUser: string;
  public settingDate: string;
  public isShowApp: boolean;
  public status: number;
}

export class CreateEventOverview {
  public id?: number = undefined;
  public name: string = '';
  public organize: string = '';
  public type: number[] = [];
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'name');
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'name',
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

  public isValidData() {
    return this.isValidName() && this.isValidOrganize() && this.isValidType();
  }

  public toObjectSendToAPI() {
    return {
      name: this.name,
      organizator: this.organize,
      eventTypes: this.type,
    };
  }
}

export class StopOrCancelEvent {
  public id?: number = undefined;
  public reason: number | undefined = undefined;
  public description: string = '';
  public createUser: string = '';
  public createDate: any = '';
}
