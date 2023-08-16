export class ProjectSaleModel {
  id: number;
  code: string;
  name: string;
  quantity: number;
  deposit: number;
  buy: number;
  startDate: string;
  endDate: string;
  note: string;
  status: number;
}

export class CreateProjectSale {
  project: number | undefined = undefined;
  startDate: any = "";
  endDate: any = "";
  keepTime: any = "";
}

export class ProjectSaleList {
  isSelected?: boolean = false;
  id: number;
  code: string = "";
  quantity: number | undefined = undefined;
  density: string = "";
  area: number | undefined = undefined;
  sold: string = "";
  showPrice: boolean = false;
  deposit: string = "";
  lock: string = "";
  bookType: string = "";
  status: number | undefined = undefined;
}

export class ProjectSalePolicy {
  id: number;
  code: string = "";
  name: string = "";
  typeDeposit: string = "";
  price: string = "";
  status: number;
}

export class CreateProjectSalePolicy {
  isSelectd: boolean = false;
  id: number;
  name: string;
  price: string;
  typeDeposit: string;
}

export class ProjectSaleDepositForm {
  id: number;
  name: string;
  contractType: string;
  structure: string;
  policy: string;
  customerType: string;
  status: number;
}

export class CreateDepositForm {
  id: number | undefined;
  form: number | undefined;
  contractType: number | undefined;
  contract: number | undefined;
  policy: number | undefined;
  structure: number | undefined;
  displayType: number | undefined;
  startDate: any;
}

export class ProjectSaleFile {
  id: number;
  name: string;
  type: string;
  description: string;
  status: number;
}
