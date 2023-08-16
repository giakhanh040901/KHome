export interface IColumn {
    field: string; 
    header: string;
    title?:string,
    width: number;
    left?: number; // fix frozen left
    right?: number; // fix frozen right
    isShow?: boolean,
    isSort?: boolean,
    type?: string;
    isPin?: boolean; // Khóa cột 
    isFrozen?: boolean; // Fixed cột 
    alignFrozen?: string;   // Vị trí fixed cột left|right
    isResize?: boolean; // width auto
    class?: string;
    icon?: string;
    classButton?: string;
    cutText?: string;
    position?: number;
    displaySettingColumn?: boolean; 
    unit?: string;
    isPermission?: boolean;
}
export interface IAction {
    data: any;
    label: string;
    icon: string;
    command: Function;
}

export class DataTableEmit {
    isShowFilter?: boolean = true;
    selectedItems?: any[] = [];
}

export interface Sort {
    field: string;
    order: number;
}