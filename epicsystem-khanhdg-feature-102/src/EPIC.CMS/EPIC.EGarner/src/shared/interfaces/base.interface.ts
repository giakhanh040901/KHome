export interface IColumn {
    field: string;
    header: string;
    width?: string;
    isPin?: boolean;
    cutText?: string;
    type?: string;
    pTooltip?: string,
    tooltipPosition?: string,
    class?: string,
    position?: number;
}

export interface IBaseAction {
    data: any;
    index: number;
    label: string;
    icon: string;
    command: ($event: any) => void;
}

export interface IBaseListAction extends Array<IBaseAction> {}

