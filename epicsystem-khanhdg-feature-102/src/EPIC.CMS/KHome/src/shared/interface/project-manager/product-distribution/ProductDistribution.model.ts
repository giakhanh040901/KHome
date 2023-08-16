export class ProductDistributionModel {
  id: number;
  code: string;
  name: string;
  agency: string;
  quantity: number;
  sold: number;
  deposit: number;
  startDate: string;
  approveDate: string;
  stopDate: string;
  status: number;
}

export class CreateProductDistribution {
  id?: number = undefined;
  agency: number | undefined = undefined;
  project: number | undefined = undefined;
  startDate: any;
  endDate?: any = "";
  private _dataValidator: any[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidAgency() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "agency"
    );
    if (!!this.agency) {
      return true;
    }
    this._dataValidator.push({
      name: "agency",
      message: "Vui lòng chọn thông tin Đại lý phân phối",
    });
    return false;
  }

  public isValidProject() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "project"
    );
    if (!!this.project) {
      return true;
    }
    this._dataValidator.push({
      name: "project",
      message: "Vui lòng chọn thông tin Dự án",
    });
    return false;
  }

  public isValidStartDate() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "startDate"
    );
    if (!!this.startDate) {
      return true;
    }
    this._dataValidator.push({
      name: "startDate",
      message: "Vui lòng nhập thông tin Ngày phân phối",
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidAgency() && this.isValidProject() && this.isValidStartDate()
    );
  }

  public toObjectSendToAPI() {
    const result = {
      id: this.id,
      projectId: this.project,
      tradingProviderId: this.agency,
      startDate: this.startDate,
      endDate: this.endDate,
    };
    Object.keys(result).forEach((key: string) => {
      result[key] = !!result[key] ? result[key] : undefined;
    });
    return result;
  }

  public mapDTOToModel(dto: any) {
    this.id = dto.id;
    this.project = dto.projectId;
    this.agency = dto?.tradingProvider?.tradingProviderId;
    if (dto.startDate) this.startDate = dto.startDate;
    if (dto.endDate) this.endDate = dto.endDate;
  }
}

export class ProductListModel {
  isSelected?: boolean = false;
  id: number;
  code: string = "";
  name: string = "";
  density: string = "";
  area: number | undefined = undefined;
  price: string = "";
  deposit: string = "";
  bookType: string = "";
  status: number | undefined = undefined;
  statusLock: string = "";
  productId: number | undefined = undefined;
}

export class CreateProductListModel {
  isSelected?: boolean = false;
  id: number;
  code: string = "";
  grade: number | undefined = undefined;
  quantity: number | undefined = undefined;
  density: string = "";
  area: number | undefined = undefined;
  price: string = "";
}

export class DistributionPolicyModel {
  id: number;
  code: string = "";
  name: string = "";
  typePay: string = "";
  deposit: string = "";
  lock: string = "";
  status: string = "";
}

export class CreateOrEditDistributionPolicy {
  code: string | undefined = "";
  policy: number | undefined = undefined;
  name: string = "";
  paymentType: number | undefined = undefined;
  depositType: number | undefined = undefined;
  deposit: number | undefined = undefined;
  lockType: number | undefined = undefined;
  lock: number | undefined = undefined;
  description: string = "";
  private _dataValidator: any[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidValue(data: { key: string; message: string }) {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== data.key
    );
    if (!!this[data.key]) {
      return true;
    }
    this._dataValidator.push({
      name: data.key,
      message: data.message,
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidValue({
        key: "name",
        message: "Vui lòng nhập Tên chính sách",
      }) &&
      this.isValidValue({
        key: "deposit",
        message: "Vui lòng nhập Giá trị đặt cọc",
      }) &&
      this.isValidValue({
        key: "lock",
        message: "Vui lòng nhập Giá trị lock căn",
      })
    );
  }

  public toObjectSendToAPI(distributionId: number) {
    const result = {
      code: this.code,
      name: this.name,
      paymentType: this.paymentType,
      depositType: this.depositType,
      depositValue: this.deposit,
      lockType: this.lockType,
      lockValue: this.lock,
      description: this.description,
      distributionId: distributionId,
    };
    Object.keys(result).forEach((key: string) => {
      result[key] = !!result[key] ? result[key] : undefined;
    });
    return result;
  }

  public mapDTOToModel(dto: any) {
    this.code = dto.code;
    this.name = dto.name;
    this.depositType = dto.depositType;
    this.deposit = dto.depositValue;
    this.lockType = dto.lockType;
    this.lock = dto.lockValue;
    this.description = dto.description;
    this.paymentType = dto.paymentType;
  }
}

export class ContractFormModel {
  id: number;
  name: string = "";
  type: string = "";
  structure: string = "";
  policy: string = "";
  status: number;
}

export class CreateOrEdiContractForm {
  id: number | undefined = undefined;
  contractModel: number | undefined = undefined;
  contractType: number | undefined = undefined;
  contract: number | undefined = undefined;
  policy: number | undefined = undefined;
  structure: number | undefined = undefined;
  startDate: any = "";
  private _dataValidator: any[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public mapDTOToModel(dto: any) {
    this.id = dto.id;
    this.contractModel = dto.name;
    this.contractType = dto.depositType;
    this.contract = dto.contractTemplateTempId;
    this.policy = dto.distributionPolicyId;
    this.structure = dto.configContractCodeId;
    this.startDate = dto.effectiveDate ?? "";
  }

  public isValidValue(data: { key: string; message: string }) {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== data.key
    );
    if (!!this[data.key]) {
      return true;
    }
    this._dataValidator.push({
      name: data.key,
      message: data.message,
    });
    return false;
  }

  public isValidData() {
    return (
      this.isValidValue({
        key: "contractModel",
        message: "Vui lòng chọn Kiểu hợp đồng",
      }) &&
      this.isValidValue({
        key: "contractType",
        message: "Vui lòng chọn Loại hợp đồng",
      }) &&
      this.isValidValue({
        key: "contract",
        message: "Vui lòng chọn Hợp đồng",
      }) &&
      this.isValidValue({
        key: "policy",
        message: "Vui lòng chọn Loại chính sách",
      }) &&
      this.isValidValue({
        key: "structure",
        message: "Vui lòng chọn Cấu trúc mã",
      }) &&
      this.isValidValue({
        key: "startDate",
        message: "Vui lòng nhập Ngày hiệu lực",
      })
    );
  }

  public toObjectSendToAPI(distributionId: number) {
    return {
      id: this.id,
      distributionId: distributionId,
      contractTemplateTempId: this.contract,
      distributionPolicyId: this.policy,
      configContractCodeId: this.structure,
      effectiveDate: this.startDate,
    };
  }
}
