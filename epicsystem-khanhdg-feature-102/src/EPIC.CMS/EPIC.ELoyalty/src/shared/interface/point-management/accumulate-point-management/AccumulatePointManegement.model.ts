import { formatCalendarItem } from '@shared/function-common';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class AccumulatePointManegementModel {
  public id: number;
  public customerCode: string;
  public customerName: string;
  public numberPhone: string;
  public point: number;
  public type: number;
  public applyDate: string;
  public createDate: string;
  public createUser: string;
  public status: number;
}

export class CrudAccumulatePointManagement {
  public id?: number = undefined;
  public type: number;
  public point: number;
  public reason: number;
  public applyDate: any = '';
  public description?: string = '';
  public individualId?: number = undefined;
  public businessId?: number = undefined;
  public createDate: any = '';
  public createUser: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.type = dto.pointType;
      this.point = dto.point;
      this.reason = dto.reason;
      this.applyDate = dto.applyDate ? new Date(dto.applyDate) : '';
      this.description = dto.description;
      this.individualId = dto.investorId;
      this.createDate = dto.createdDate ? new Date(dto.createdDate) : '';
      this.createUser = dto.createdBy;
    }
  }

  public isValidType() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'type');
    if (this.type) {
      return true;
    }
    this._dataValidator.push({
      name: 'type',
      message: 'Vui lòng nhập chọn Loại hình',
    });
    return false;
  }

  public isValidPoint() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'point');
    if (this.point) {
      return true;
    }
    this._dataValidator.push({
      name: 'point',
      message: 'Vui lòng nhập thông tin Số điểm',
    });
    return false;
  }

  public isValidReason() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'reason');
    if (this.reason) {
      return true;
    }
    this._dataValidator.push({
      name: 'reason',
      message: 'Vui lòng nhập chọn Lý do',
    });
    return false;
  }

  public isValidApplyDate() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'applyDate');
    if (this.applyDate) {
      return true;
    }
    this._dataValidator.push({
      name: 'applyDate',
      message: 'Vui lòng nhập chọn Ngày áp dụng',
    });
    return false;
  }

  public isValidCustomer() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'customer');
    if (this.individualId || this.businessId) {
      return true;
    }
    this._dataValidator.push({
      name: 'customer',
      message: 'Vui lòng chọn Khách hàng',
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidType() &&
      this.isValidPoint() &&
      this.isValidReason() &&
      this.isValidApplyDate() &&
      this.isValidCustomer()
    );
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      investorId: this.individualId,
      point: Number(this.point),
      pointType: this.type,
      reason: this.reason,
      description: this.description,
      applyDate: this.applyDate ? formatCalendarItem(this.applyDate) : undefined,
    };
  }
}
