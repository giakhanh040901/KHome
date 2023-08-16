import { Sort } from "./p-table.model";

export class DataFilter {
    keyword?: string;
    status?: string;
    sortFields?: Sort[] = [];
}