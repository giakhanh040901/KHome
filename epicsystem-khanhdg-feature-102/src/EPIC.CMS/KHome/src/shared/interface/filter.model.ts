import { Sort } from "./p-table.model";

export class BasicFilter {
    keyword?: string;
    status?: number | string;
    sortFields?: Sort[] = [];
}

export class ApproveFilter extends BasicFilter {
    dataType?: number;
    actionType?: string;
    requestDate?: Date;
    approveDate?: Date;
    status?: number;
}

export class SelectedFilter extends BasicFilter {
    selected?: string;
}