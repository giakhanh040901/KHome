import { Action } from "@shared/consts/base.const";

export class RequestApprove {
    id: number;
    actionType?: Action = Action.ADD;
    userApproveId?: number = 1;
    requestNote?: string;
    summary?: string;
}

export class Approve {
    id: number;
    userApproveId?: number = 1;
    approveNote?: string;
    cancelNote?: string;
}