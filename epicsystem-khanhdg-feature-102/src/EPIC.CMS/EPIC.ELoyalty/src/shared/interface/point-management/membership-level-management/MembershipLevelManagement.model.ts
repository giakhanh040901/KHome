import { COMPARE_TYPE } from '@shared/AppConsts';
import { compareDate, formatCalendarItem } from '@shared/function-common';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class MembershipLevelManagementModel {
  public id: number;
  public name: string;
  public description: string;
  public startPoint: number;
  public endPoint: number;
  public applyDate: string;
  public createDate: string;
  public status: number;
}

export class CrudMembershipLevelManagement {
  public id?: number = undefined;
  public startPoint: number;
  public endPoint: number;
  public name: string = '';
  public description?: string = '';
  public applyDate: any = '';
  public deactiveDate?: any = '';
  public createDate: any = '';
  public createUser: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.startPoint = dto.pointStart;
      this.endPoint = dto.pointEnd;
      this.name = dto.name;
      this.description = dto.description;
      this.applyDate = dto.activeDate ? new Date(dto.activeDate) : '';
      this.deactiveDate = dto.deactiveDate ? new Date(dto.deactiveDate) : '';
      this.createDate = dto.createdDate ? new Date(dto.createdDate) : '';
      this.createUser = dto.createdBy;
    }
  }

  public isValidStartPoint() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'startPoint');
    if (this.startPoint) {
      return true;
    }
    this._dataValidator.push({
      name: 'startPoint',
      message: 'Vui lòng nhập thông tin Số điểm bắt đầu',
    });
    return false;
  }

  public isValidEndPoint() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'endPoint');
    if (this.endPoint) {
      if (Number(this.startPoint) < Number(this.endPoint)) {
        return true;
      } else {
        this._dataValidator.push({
          name: 'endPoint',
          message: 'Số điểm kết thúc phải lớn hơn Số điểm bắt đầu',
        });
        return false;
      }
    }
    this._dataValidator.push({
      name: 'endPoint',
      message: 'Vui lòng nhập thông tin Số điểm kết thúc',
    });
    return false;
  }

  public isValidName() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'name');
    if (this.name) {
      return true;
    }
    this._dataValidator.push({
      name: 'name',
      message: 'Vui lòng nhập thông tin Tên hạng',
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

  public isValidDeactiveDate() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'deactiveDate');
    if (this.deactiveDate) {
      if (compareDate(new Date(this.applyDate), new Date(this.deactiveDate), COMPARE_TYPE.LESS)) {
        return true;
      } else {
        this._dataValidator.push({
          name: 'deactiveDate',
          message: 'Ngày hủy kích hoạt phải lớn hơn Ngày áp dụng',
        });
        return false;
      }
    }
    return true;
  }

  public isValidData() {
    return this.isValidStartPoint() && this.isValidEndPoint() && this.isValidName() && this.isValidApplyDate() && this.isValidDeactiveDate();
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      name: this.name,
      description: this.description,
      pointStart: Number(this.startPoint),
      pointEnd: Number(this.endPoint),
      activeDate: this.applyDate ? formatCalendarItem(this.applyDate) : undefined,
      deactiveDate: this.deactiveDate ? formatCalendarItem(this.deactiveDate) : undefined,
    };
  }
}
