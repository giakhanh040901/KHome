import { Sort } from "./p-table.model";

export class CollectFilter {
    keyword?: string;
    status?: number;
    createdDate?: Date;
    sortFields: Sort[];
}

export class PaymentFilter {
    keyword?: string;
    status?: number;
    approveDate?: Date;
    sortFields: Sort[];
}