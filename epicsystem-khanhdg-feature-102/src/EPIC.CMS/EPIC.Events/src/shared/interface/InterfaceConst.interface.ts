import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeSortTable,
  FormNotificationConst,
} from '@shared/AppConsts';

export interface IHeaderColumn {
  field: string;
  isSort?: boolean;
  fieldSort?: string;
  header: string;
  width?: string;
  minWidth?: string;
  maxWidth?: string;
  type?: ETypeDataTable;
  isPin?: boolean;
  isResize?: boolean;
  class?: string;
  position?: number;
  posTextCell?: EPositionTextCell;
  isFrozen?: boolean;
  posFrozen?: EPositionFrozenCell;
  funcStyleClassStatus?: Function;
  funcLabelStatus?: Function;
  valueFormatter?: Function;
  valueGetter?: Function;
  isDefaultNotCutText?: boolean;
  hideBtnSetColumn?: boolean;
}

export interface IDropdown {
  label: string;
  value: number | string | boolean;
  severity?: string;
  rawData?: any;
}

export interface IActionTable {
  data: any;
  label: string;
  icon: string;
  command: Function;
}

export interface IConstant {
  id: number | string;
  value: string;
}

export interface ISelectButton {
  label: string;
  value: number | string;
}

export interface ITabView {
  key: string;
  title: string;
  component: any;
  isDisabled: boolean;
  isHide?: boolean;
}

export interface IImage {
  id?: number;
  src: string;
  width: number | string;
  height: number | string;
}

export interface IDescriptionContent {
  contentType: string;
  content: string;
}

export interface IActionButtonTable {
  classButton?: string;
  styleClassButton?: string;
  label: string;
  icon: string;
  isDisabled?: boolean;
  showFunction?: Function;
  callBack: Function;
}

export interface IValueFormatter {
  data: any;
}

export interface IConfigDataModal {
  dataSource: any;
  type: EConfigDataModal | string;
}

export interface INotiDataModal {
  title: string;
  icon: FormNotificationConst;
}

export interface IDataValidator {
  name: string;
  message: string;
}

export interface ISortTable {
  field: string;
  type: ETypeSortTable;
  sort: string;
}
