import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class CrudEventDetailTemplate {
  public id?: number = undefined;
  public name: string = '';
  public link: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'name');
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'name',
      message: 'Vui lòng nhập thông tin Tên mẫu',
    });
    return false;
  }

  private isValidLink() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'link');
    if (this.link && this.link.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'link',
      message: 'Vui lòng chọn thông tin Mẫu chính sách',
    });
    return false;
  }

  public isValidData() {
    return this.isValidName() && this.isValidLink();
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      name: this.name,
      fileUrl: this.link,
    };
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.name = dto.name;
      this.link = dto.link;
    }
  }
}
