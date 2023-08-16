import { SettingContractCode } from '@shared/AppConsts';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class ContractCodeModel {
  public id: number;
  public name: string;
  public code: string;
  public description: string;
  public settingUser: string;
  public settingDate: string;
  public status: string;
}

export class CreateOrEditContractCode {
  public id?: number = undefined;
  public code: string = '';
  public name: string = '';
  public contractCodeItem: ContractCodeItem[] = [];
  public note?: string = '';
  public contractCodeStructure: string = '';
  public contractCode: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidName() {
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'name');
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'name',
      message: 'Vui lòng nhập thông tin Tên cấu trúc hợp đồng',
    });
    return false;
  }

  private isValidContractCodeItem() {
    this._dataValidator = this._dataValidator.filter(
      (e: IDataValidator) => e.name !== 'contractCodeItem' && e.name !== 'character'
    );
    if (this.contractCodeItem && this.contractCodeItem.length) {
      let resultItem: boolean = true;
      let index: number = 0;
      while (index < this.contractCodeItem.length && !!resultItem) {
        const item = this.contractCodeItem[index];
        resultItem = item.isValidData(index);
        this._dataValidator.unshift(...item.dataValidator);
        index++;
      }
      return resultItem;
    }
    this._dataValidator.unshift({
      name: 'contractCodeItem',
      message: 'Vui lòng chọn Giá trị cấu trúc hợp đồng',
    });
    return false;
  }

  public isValidData() {
    return this.isValidName() && this.isValidContractCodeItem();
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      name: this.name,
      description: this.note,
      configContractCodeDetails: this.contractCodeItem.map((e: ContractCodeItem) => ({
        id: e.id,
        key: e.value,
        value: e.character,
      })),
    };
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.name = dto.name;
      this.note = dto.description;
      this.contractCodeItem = dto.configContractCodeDetails.map((e: any) => {
        let item = new ContractCodeItem();
        Object.assign(item, {
          id: e.id,
          value: e.key,
          character: e.value,
        });
        return item;
      });
    }
  }
}

export class ContractCodeItem {
  public id?: number = undefined;
  public value: string = null;
  public character: string = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidharacter(index: number) {
    console.log('_dataValidator ', this._dataValidator);
    
    this._dataValidator = this._dataValidator.filter((e: IDataValidator) => e.name !== 'character');
    if ((this.character && this.character.length) || this.value != SettingContractCode.KY_TU) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'character',
      message: 'Vui lòng nhập thông tin Tên cấu trúc hợp đồng thứ ' + (index + 1),
    });
    return false;
  }

  public isValidData(index: number) {
    return this.isValidharacter(index);
  }
}
