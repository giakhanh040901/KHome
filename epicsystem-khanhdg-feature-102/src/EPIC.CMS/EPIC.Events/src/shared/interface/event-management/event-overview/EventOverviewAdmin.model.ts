import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class EventOverviewAdminModel {
  public id: number;
  public eventId: number;
  public eventName: string = '';
  public investorId: number;
  public fullName: string = '';
  public phone: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapDTO(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.eventName = dto.name;
      
    }
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

  public isValidData() {
    return (
      this.isValidEventName() 
     
    );
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
    };
  }
}
